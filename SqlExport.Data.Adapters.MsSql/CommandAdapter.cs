using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Diagnostics;

namespace SqlExport.Data.Adapters.MsSql
{
    using SqlExport.Common.Data;

    internal class CommandAdapter : IDbCommandAdapter
    {
        /// <summary>
        /// The line number pattern.
        /// </summary>
        private static readonly Regex LineNumberPattern = new Regex(
            @"^Line (?<line>\d+):", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private int _timeout;

        /// <summary>
        /// The current query.
        /// </summary>
        private Query currentQuery = new Query();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAdapter"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        public CommandAdapter(string connectionString, int commandTimeout)
        {
            _connection = new SqlConnection(connectionString);
            _connection.InfoMessage += this.OnInfoMessage;
            _connection.Open();
            _timeout = commandTimeout;
        }

        internal SqlCommand CreateCommand(string sql)
        {
            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.CommandTimeout = _timeout;
            if (_transaction != null)
            {
                cmd.Transaction = _transaction;
            }

            return cmd;
        }

        private void ExecuteStatement(string statement, DataSet dataSet)
        {
            if (!string.IsNullOrEmpty(statement))
            {
                using (SqlCommand cmd = CreateCommand(statement))
                {
                    cmd.StatementCompleted += this.OnStatementCompleted;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataSet);
                    }
                }
            }
        }

        private void OnStatementCompleted(object sender, StatementCompletedEventArgs e)
        {
            OnMessage(MessageType.Information, string.Concat(e.RecordCount, " records affected."), null);
        }

        /// <summary>
        /// Called when an info message is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Data.SqlClient.SqlInfoMessageEventArgs"/> instance containing the event data.</param>
        private void OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError error in e.Errors)
            {
                this.currentQuery.OnMessage(error, this.OnMessage);
            }
        }

        /// <summary>
        /// Raises the Message event.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="lineNumber">The line number.</param>
        private void OnMessage(MessageType type, string message, int? lineNumber)
        {
            if (this.Message != null)
            {
                this.Message(type, message, lineNumber);
            }
        }

        private void SetNames(DataTableCollection tables, string statement, int startIndex)
        {
            if (tables.Count > 0)
            {
                int i = 0;
                using (StringReader sr = new StringReader(statement))
                {
                    string line = sr.ReadLine();
                    while (line != null && i < tables.Count)
                    {
                        if (line.StartsWith("--Name: "))
                        {
                            string name = line.Substring(8).Trim();
                            if (name != string.Empty)
                            {
                                tables[i].TableName = line.Substring(8);
                                i++;
                                startIndex++;
                            }
                        }

                        line = sr.ReadLine();
                    }
                }

                for (; i < tables.Count; i++)
                {
                    tables[i].TableName = string.Concat("Table ", startIndex++);
                }
            }
        }

        /// <summary>
        /// Gets the queries.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>All the separate queries.</returns>
        private IEnumerable<Query> GetQueries(TextReader sql)
        {
            string line = null;
            int lineNumber = 0;
            Query query = null;
            while ((line = sql.ReadLine()) != null)
            {
                if (query == null)
                {
                    query = new Query(lineNumber);
                }

                // TODO multi line comment check

                if (string.Compare(line, "GO", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    yield return query;
                    query = null;
                }
                else
                {
                    query.Text.AppendLine(line);
                }

                lineNumber++;
            }

            if (query != null)
            {
                yield return query;
            }
        }

        #region IDbCommandAdapter Members

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public DbTransaction Transaction
        {
            get { return _transaction; }
        }

        #endregion

        #region ICommandAdapter Members

        public event ConnectionMessageHandler Message;

        public void BeginTransaction()
        {
            if (!HasTransaction)
            {
                _transaction = _connection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (HasTransaction)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (HasTransaction)
            {
                _transaction.Rollback();
                _transaction = null;
            }
        }

        public bool HasTransaction
        {
            get { return _transaction != null; }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>The results.</returns>
        public IEnumerable<IDataResult> ExecuteCommand(TextReader sql)
        {
            int count = 0;
            foreach (var query in this.GetQueries(sql))
            {
                this.currentQuery = query;
                if (query.Text.Length != 0)
                {
                    DataSet tmp = new DataSet();

                    try
                    {
                        this.ExecuteStatement(query.Text.ToString(), tmp);
                    }
                    catch (SqlException exp)
                    {
                        foreach (SqlError error in exp.Errors)
                        {
                            query.OnMessage(error, this.OnMessage);
                        }

                        throw new ExecuteCommandException();
                    }

                    this.SetNames(tmp.Tables, query.Text.ToString(), count);
                    foreach (DataTable table in tmp.Tables)
                    {
                        count++;
                        yield return DataResultHelper.FromDataTable(table.Copy());
                    }

                    tmp.Dispose();
                }
            }

            this.currentQuery = new Query();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// Defines the Query type.
        /// </summary>
        private class Query
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query"/> class.
            /// </summary>
            /// <param name="offsetLineNumber">The offset line number.</param>
            public Query(int offsetLineNumber = 0)
            {
                this.Text = new StringBuilder();
                this.OffsetLineNumber = offsetLineNumber;
            }

            /// <summary>
            /// Gets the text.
            /// </summary>
            public StringBuilder Text { get; private set; }

            /// <summary>
            /// Gets or sets the offset line number.
            /// </summary>
            private int OffsetLineNumber { get; set; }

            /// <summary>
            /// Raises the Message event.
            /// </summary>
            /// <param name="error">The error.</param>
            /// <param name="messageCallback">The message callback.</param>
            public void OnMessage(SqlError error, Action<MessageType, string, int?> messageCallback)
            {
                MessageType type = MessageType.Error;
                if (error.Class == 0)
                {
                    type = MessageType.Information;
                }

                string message = UpdateLineNumberWithOffset(error.Message, this.OffsetLineNumber);
                messageCallback(type, message, error.LineNumber + this.OffsetLineNumber);
            }

            /// <summary>
            /// Updates the line number with offset.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="offsetLineNumber">The offset line number.</param>
            /// <returns>The updated message.</returns>
            private static string UpdateLineNumberWithOffset(string message, int offsetLineNumber)
            {
                if (offsetLineNumber > 0)
                {
                    message = LineNumberPattern.Replace(
                        message,
                        m =>
                        {
                            var lineNumber = int.Parse(m.Groups["line"].Value);
                            return string.Concat("Line ", lineNumber + offsetLineNumber, ":");
                        });
                }

                return message;
            }
        }
    }
}

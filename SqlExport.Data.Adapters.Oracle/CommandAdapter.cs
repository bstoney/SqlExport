using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;
using System.Data.Common;

namespace SqlExport.Data.Adapters.Oracle
{
    using SqlExport.Common.Data;

    internal class CommandAdapter : IDbCommandAdapter
    {
        private OracleConnection _connection;
        private OracleTransaction _transaction;
        private int _timeout;

        private const string NullParameterValue = "*NULL*";
        private Regex _statementParse = new Regex(@"(?s:(?<statement>.+?))(?:\r\n/\s*(\r\n|$)|$)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public CommandAdapter(string connectionString, int commandTimeout)
        {
            _connection = new OracleConnection(connectionString);
            _connection.InfoMessage += new OracleInfoMessageEventHandler(OnInfoMessage);
            _connection.Open();
            _timeout = commandTimeout;
        }

        internal OracleCommand CreateCommand(string sql)
        {
            OracleCommand cmd = _connection.CreateCommand();
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
                using (OracleCommand cmd = CreateCommand(statement))
                {
                    foreach (OracleParameter op in GetParameters(statement))
                    {
                        cmd.Parameters.Add(op);
                    }

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dataSet);
                    }
                }
            }
        }

        private IEnumerable<OracleParameter> GetParameters(string statement)
        {
            using (StringReader sr = new StringReader(statement))
            {
                string line = sr.ReadLine();
                while (line != null && (line == string.Empty || line.StartsWith("--:")))
                {
                    if (line != string.Empty)
                    {
                        string[] param = line.Substring(3).Split(new char[] { ',' }, 3);
                        if (param.Length == 3)
                        {
                            OracleType ot = OracleType.VarChar;
                            try
                            {
                                ot = (OracleType)Enum.Parse(typeof(OracleType), param[1], true);
                            }
                            catch { }

                            OracleParameter op = new OracleParameter(param[0], ot);
                            op.Value = param[2] == NullParameterValue ? DBNull.Value : (object)param[2];
                            yield return op;
                        }
                    }

                    line = sr.ReadLine();
                }
            }
        }

        private void OnInfoMessage(object sender, OracleInfoMessageEventArgs e)
        {
            OnMessage(e.Code, e.Message);
        }

        private void OnMessage(int code, string message)
        {
            MessageType type = MessageType.Error;
            if (code == 0)
            {
                type = MessageType.Information;
            }

            if (Message != null)
            {
                Message(type, message, null);
            }
        }

        private void SetNames(DataTableCollection tables, string statement, int start)
        {
            if (tables.Count > 0)
            {
                int i = 0;
                using (StringReader sr = new StringReader(statement))
                {
                    string line = sr.ReadLine();
                    while (line != null && (line == string.Empty || line.StartsWith("--Name: ")))
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            string name = line.Substring(8).Trim();
                            if (name != string.Empty)
                            {
                                tables[i].TableName = line.Substring(8);
                                i++;
                                start++;
                                break;
                            }
                        }

                        line = sr.ReadLine();
                    }
                }

                for (; i < tables.Count; i++)
                {
                    tables[i].TableName = string.Concat("Table ", start++);
                }
            }
        }

        private IEnumerable<string> GetQueries(TextReader sql)
        {
            string line = null;
            StringBuilder query = new StringBuilder();
            while ((line = sql.ReadLine()) != null)
            {
                // TODO multi line comment check

                if (string.Compare(line, "/", true) == 0)
                {
                    yield return query.ToString();
                    query = new StringBuilder();
                }
                else
                {
                    query.AppendLine(line);
                }
            }

            yield return query.ToString();
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

        public IEnumerable<IDataResult> ExecuteCommand(TextReader sql)
        {
            int count = 0;
            foreach (var query in GetQueries(sql))
            {
                DataSet tmp = new DataSet();
                try
                {
                    ExecuteStatement(query, tmp);
                }
                catch (OracleException exp)
                {
                    if (exp.Data.Count > 0)
                    {
                        System.Diagnostics.Debugger.Break();
                    }

                    OnMessage(exp.Code, exp.Message);

                    throw new ExecuteCommandException();
                }

                SetNames(tmp.Tables, query, count);
                foreach (DataTable table in tmp.Tables)
                {
                    count++;
                    yield return DataResultHelper.FromDataTable(table.Copy());
                }
            }
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;
using System.Data;
using System.IO;
using SqlExport.Data.Adapters.Text.Query;

namespace SqlExport.Data.Adapters.Text
{
    using SqlExport.Common.Data;

    internal class CommandAdapter : ICommandAdapter
    {
        internal ConnectionString ConnectionString { get; set; }

        internal int CommandTimeout { get; set; }

        private void OnMessage(string message, int? lineNumber)
        {
            if (Message != null)
            {
                Message(MessageType.Error, message, lineNumber);
            }
        }

        private DataTable ExecuteQueryAsDataTable(Query.Query query)
        {
            DataTable table = new DataTable(query.Name);
            foreach (var item in query.DestinationColumns)
            {
                if (SchemaAdapter.IsRowNumberColumnName(item))
                {
                    table.Columns.Add(item, typeof(int));
                }
                else
                {
                    table.Columns.Add(item, typeof(string));
                }
            }

            int rowNumber = 0;
            foreach (var line in query.Lines)
            {
                rowNumber++;
                DataRow row = table.NewRow();
                for (int i = 0; i < query.DestinationColumns.Length; i++)
                {
                    row[i] = line[i]; //query.GetValue(query.DestinationColumns[i], line, rowNumber);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private IDataReader ExecuteQueryAsReader(Query.Query query)
        {
            return new Results(query);
        }

        private void OnParseError(QueryParser parser, string message, int lineNumber)
        {
            OnMessage(message, lineNumber);
        }

        #region ICommandAdapter Members

        public event ConnectionMessageHandler Message;

        public bool HasTransaction
        {
            get { return false; }
        }

        public void BeginTransaction()
        {
        }

        public void CommitTransaction()
        {
        }

        public void RollbackTransaction()
        {
        }

        public IEnumerable<IDataResult> ExecuteCommand(TextReader sql)
        {
            QueryParser qp = new QueryParser(sql);
            qp.ParseError += new Action<QueryParser, string, int>(OnParseError);
            foreach (var query in qp.Parse())
            {
                var hasErrors = false;
                query.Prepare(ex =>
                    {
                        hasErrors = true;
                        OnMessage(ex.Message, ex.LineNumber);
                    },
                    ConnectionString);

                var result = hasErrors ? null : ExecuteQueryAsDataTable(query);

                if (result != null)
                {
                    yield return DataResultHelper.FromDataTable(result);
                }
                else
                {
                    throw new ExecuteCommandException();
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}

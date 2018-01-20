using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace SqlExport.Data.Adapters.Odbc
{
    using SqlExport.Common.Data;

    internal class CommandAdapter : ICommandAdapter, IDbCommandAdapter
    {
        private OdbcConnection _connection;
        private OdbcTransaction _transaction;
        private int _timeout;

        public CommandAdapter(string connectionString, int commandTimeout)
        {
            _connection = new OdbcConnection(connectionString);
            _connection.InfoMessage += new OdbcInfoMessageEventHandler(OnInfoMessage);
            _connection.Open();
            _timeout = commandTimeout;
        }

        internal OdbcCommand CreateCommand(string sql)
        {
            OdbcCommand cmd = _connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.CommandTimeout = _timeout;
            if (_transaction != null)
            {
                cmd.Transaction = _transaction;
            }
            return cmd;
        }

        private void OnInfoMessage(object sender, OdbcInfoMessageEventArgs e)
        {
            for (int i = 0; i < e.Errors.Count; i++)
            {
                OnMessage(e.Errors[i]);
            }
        }

        private void OnMessage(OdbcError error)
        {
            MessageType type = MessageType.Error;
            if (error.NativeError == 0)
            {
                type = MessageType.Information;
            }
            if (Message != null)
            {
                Message(type, error.Message, null);
            }
        }

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
            DataSet results = new DataSet();
            try
            {
                string sqlStatement = sql.ReadToEnd();
                using (OdbcCommand cmd = CreateCommand(sqlStatement))
                {
                    using (OdbcDataAdapter adapter = new OdbcDataAdapter(cmd))
                    {
                        adapter.Fill(results);
                    }
                }
            }
            catch (OdbcException exp)
            {
                for (int i = 0; i < exp.Errors.Count; i++)
                {
                    OnMessage(exp.Errors[i]);
                }

                throw new ExecuteCommandException();
            }

            return results.Tables.OfType<DataTable>().Select(t => DataResultHelper.FromDataTable(t));
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
    }
}

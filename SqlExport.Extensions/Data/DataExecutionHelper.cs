using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Linq;
using SqlExport.Common.Data;

namespace SqlExport.Data
{
    using SqlExport.Common;

    internal static class DataExecutionHelper
    {
        private static Dictionary<Guid, CommandExecuter> _executer = new Dictionary<Guid, CommandExecuter>();

        public static object BeginOpenRecord(DatabaseConnectionContext database, TextReader sql, AsyncCallback completeHandler)
        {
            Guid key = Guid.NewGuid();
            CommandExecuter ce = new CommandExecuter(database, sql, completeHandler, key);
            _executer.Add(key, ce);
            ce.Run();
            return key;
        }

        public static IEnumerable<IDataResult> EndOpenRecord(IAsyncResult result)
        {
            CommandExecuter ce = result as CommandExecuter;
            Guid key = (Guid)ce.AsyncState;
            if (!ce.IsCompleted)
            {
                _executer[key].Abort();
            }

            _executer.Remove(key);
            return ce.Results;
        }

        public static DatabaseConnectionContext GetDatabase(IAsyncResult result)
        {
            CommandExecuter ce = result as CommandExecuter;
            return ce.GetDatabase();
        }

        public static Exception[] GetErrors(IAsyncResult result)
        {
            CommandExecuter ce = result as CommandExecuter;
            return ce.GetErrors();
        }

        public static TextReader GetReader(IAsyncResult result)
        {
            CommandExecuter ce = result as CommandExecuter;
            return ce.GetReader();
        }

        public static void StopRequest(object identifier)
        {
            if (identifier != null && identifier is Guid)
            {
                Guid key = (Guid)identifier;
                _executer[key].Abort();
                _executer.Remove(key);
            }
        }

        public static void StopAllRequests()
        {
            Guid[] keys = new Guid[_executer.Count];
            _executer.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                _executer[keys[i]].Abort();
                _executer.Remove(keys[i]);
            }
        }

        private class CommandExecuter : IAsyncResult
        {
            private DatabaseConnectionContext _database;
            private TextReader _sql;
            private AsyncCallback _completeHandler;
            private bool _closeConnection;
            private bool _closeTransaction;
            private bool _completed;
            private object _state;
            private IEnumerable<IDataResult> _results;
            private List<Exception> _executeExceptions;
            private Thread _thread;

            public CommandExecuter(DatabaseConnectionContext database, TextReader sql, AsyncCallback completeHandler, object state)
            {
                _database = database;
                _sql = sql;
                _completeHandler = completeHandler;
                _state = state;
                _completed = false;
                _executeExceptions = new List<Exception>();
            }

            public IEnumerable<IDataResult> Results
            {
                get { return _results; }
            }

            public DatabaseConnectionContext GetDatabase()
            {
                return _database;
            }

            public Exception[] GetErrors()
            {
                lock (_executeExceptions)
                {
                    Exception[] errors = _executeExceptions.ToArray();
                    _executeExceptions.Clear();
                    return errors;
                }
            }

            public TextReader GetReader()
            {
                return _sql;
            }

            public void BeginRun()
            {
                bool success = true;
                try
                {
                    _closeConnection = _closeTransaction = false;
                    if (!_database.HasConnection)
                    {
                        _closeConnection = true;
                        _database.Connect();
                    }

                    if (Configuration.Current.Transactional && !_database.HasTransaction)
                    {
                        _closeTransaction = true;
                        _database.BeginTransaction();
                    }

                    ICommandAdapter ca = _database.GetCommandAdapter();
                    // TODO let this continue and execute when it's needed. 
                    // At the present the connection is closed if ToList is not called.
                    _results = ca.ExecuteCommand(_sql).ToList();
                }
                catch (ExecuteCommandException)
                {
                    success = false;
                }
                catch (Exception exp)
                {
                    lock (_executeExceptions)
                    {
                        _executeExceptions.Add(exp);
                    }

                    success = false;
                }

                EndRun(success);
            }

            public void EndRun(bool success)
            {
                if (!_completed)
                {
                    if (_database != null)
                    {
                        try
                        {
                            if (_closeTransaction)
                            {
                                if (success)
                                {
                                    _database.CommitTransaction();
                                }
                                else
                                {
                                    _database.RollbackTransaction();
                                }
                            }

                            if (_closeConnection)
                            {
                                _database.Disconnect();
                            }
                        }
                        catch (Exception exp)
                        {
                            lock (_executeExceptions)
                            {
                                _executeExceptions.Add(exp);
                            }
                        }
                    }

                    _completed = true;
                }

                _completeHandler(this);
            }

            public void Abort()
            {
                if (_thread != null)
                {
                    _thread.Abort();
                }

                EndRun(false);
            }

            public void Run()
            {
                _thread = new Thread(new ThreadStart(BeginRun));
                _thread.Name = "CommandExecuter";
                _thread.Start();
            }

            #region IAsyncResult Members

            public object AsyncState
            {
                get { return _state; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public bool CompletedSynchronously
            {
                get { return false; }
            }

            public bool IsCompleted
            {
                get { return _completed; }
            }

            #endregion
        }
    }
}

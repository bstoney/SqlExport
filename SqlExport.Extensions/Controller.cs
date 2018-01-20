using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Data;

namespace SqlExport
{
    using SqlExport.Common.Data;

    public sealed class Controller
    {
        private int _errorMessageCount;
        private object _identifier;
        private ManualResetEvent _queryComplete = new ManualResetEvent(false);
        private bool _queryCancelled;

        public event ConnectionMessageHandler Message;

        public Stopwatch Duration { get; private set; }

        public IEnumerable<IDataResult> Data { get; private set; }

        public RunResult Result { get; private set; }

        public TextReader Reader { get; private set; }

        /// <summary>
        /// Performs an asynchronous database query.
        /// </summary>
        public void BeginRun(DatabaseConnectionContext database, TextReader query, AsyncCallback endRunCallback)
        {
            InternalRun(database, query, endRunCallback);
        }

        /// <summary>
        /// Performs an synchronous database query.
        /// </summary>
        public void Run(DatabaseConnectionContext database, TextReader query)
        {
            _queryComplete.Reset();
            InternalRun(database, query, new AsyncCallback(OnEndRunQuery));
            ////Debugger.Break();
            _queryComplete.WaitOne();
        }

        private void InternalRun(DatabaseConnectionContext database, TextReader query, AsyncCallback endRunCallback)
        {
            database.Message += new ConnectionMessageHandler(OnConnectionMessage);

            Duration = new Stopwatch();
            Duration.Start();

            _identifier = DataExecutionHelper.BeginOpenRecord(database, query, endRunCallback);
        }

        /// <summary>
        /// Cancels an asynchronous database query.
        /// </summary>
        public void StopRun()
        {
            _queryCancelled = true;
            DataExecutionHelper.StopRequest(_identifier);
        }

        /// <summary>
        /// Ends an asynchronous run and populates the result data. 
        /// </summary>
        public void EndRun(IAsyncResult result)
        {
            DatabaseConnectionContext database = DataExecutionHelper.GetDatabase(result);
            database.Message -= new ConnectionMessageHandler(OnConnectionMessage);
            Data = DataExecutionHelper.EndOpenRecord(result);
            Exception[] errors = DataExecutionHelper.GetErrors(result);
            Reader = DataExecutionHelper.GetReader(result);

            Duration.Stop();

            for (int i = 0; i < errors.Length; i++)
            {
                OnMessage(MessageType.Error, errors[i].Message);
                _errorMessageCount++;
            }

            if (_queryCancelled)
            {
                Result = RunResult.Cancelled;
            }
            else if (_errorMessageCount > 0)
            {
                Result = RunResult.Failure;
            }
            else
            {
                Result = RunResult.Success;
            }
        }

        private void OnConnectionMessage(MessageType type, string message, int? lineNumber)
        {
            if (type == MessageType.Error)
            {
                _errorMessageCount++;
            }
        }

        private void OnMessage(MessageType type, string message)
        {
            if (Message != null)
            {
                Message(type, message, null);
            }
        }

        private void OnEndRunQuery(IAsyncResult result)
        {
            try
            {
                EndRun(result);
            }
            finally
            {
                _queryComplete.Set();
            }
        }
    }
}

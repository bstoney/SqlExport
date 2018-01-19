namespace SqlExport.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.Common;
    using SqlExport.Common.Data;

    /// <summary>
    /// Defines the DatabaseConnectionContext type.
    /// </summary>
    public class DatabaseConnectionContext : DatabaseDetails
    {
        /// <summary>
        /// The message handlers.
        /// </summary>
        private readonly Dictionary<string, ConnectionMessageHandler> messageHandlers = new Dictionary<string, ConnectionMessageHandler>();

        /// <summary>
        /// The command adapter.
        /// </summary>
        private ICommandAdapter commandAdapter;

        /// <summary>
        /// The close on transaction end.
        /// </summary>
        private bool closeOnTransactionEnd;

        /// <summary>
        /// Occurs when a connection message is raised.
        /// </summary>
        public event ConnectionMessageHandler Message
        {
            add
            {
                string key = string.Concat(value.Target.GetHashCode(), value.Method.Name);
                if (!this.messageHandlers.ContainsKey(key))
                {
                    this.messageHandlers.Add(key, value);
                    if (this.commandAdapter != null)
                    {
                        this.commandAdapter.Message += value;
                    }
                }
            }

            remove
            {
                string key = string.Concat(value.Target.GetHashCode(), value.Method.Name);
                if (this.messageHandlers.ContainsKey(key))
                {
                    this.messageHandlers.Remove(key);
                    if (this.commandAdapter != null)
                    {
                        this.commandAdapter.Message -= value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has connection.
        /// </summary>
        public bool HasConnection
        {
            get { return this.commandAdapter != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has transaction.
        /// </summary>
        public bool HasTransaction
        {
            get { return this.HasConnection && this.commandAdapter.HasTransaction; }
        }

        /// <summary>
        /// Gets the command adapter.
        /// </summary>
        /// <returns>A connection adapter.</returns>
        public ICommandAdapter GetCommandAdapter()
        {
            // If no command adapter has been created open a new one.
            if (this.commandAdapter == null)
            {
                // Assume this will be handled and disposes by the caller.
                IConnectionAdapter ca = this.GetConnectionAdapter();
                return ca.GetCommandAdapter(this.ConnectionString, Configuration.Current.CommandTimeout);
            }

            return this.commandAdapter;
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            if (!this.HasConnection)
            {
                this.commandAdapter = this.GetCommandAdapter();
                if (this.commandAdapter != null)
                {
                    foreach (ConnectionMessageHandler cmh in this.messageHandlers.Values)
                    {
                        this.commandAdapter.Message += cmh;
                    }
                }
            }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            if (this.HasConnection)
            {
                if (this.HasTransaction)
                {
                    this.commandAdapter.RollbackTransaction();
                }

                foreach (ConnectionMessageHandler cmh in this.messageHandlers.Values)
                {
                    this.commandAdapter.Message -= cmh;
                }

                this.commandAdapter.Dispose();
                this.commandAdapter = null;
            }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (!this.HasTransaction)
            {
                this.closeOnTransactionEnd = false;
                if (!this.HasConnection)
                {
                    this.Connect();
                    this.closeOnTransactionEnd = true;
                }

                if (this.HasConnection)
                {
                    this.commandAdapter.BeginTransaction();
                }
            }
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (this.HasTransaction)
            {
                this.commandAdapter.CommitTransaction();
                if (this.closeOnTransactionEnd)
                {
                    this.Disconnect();
                }
            }
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (this.HasTransaction)
            {
                this.commandAdapter.RollbackTransaction();
                if (this.closeOnTransactionEnd)
                {
                    this.Disconnect();
                }
            }
        }
    }
}

namespace SqlExport.ViewModel
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Monads;
    using System.Windows.Controls;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Logic;
    using SqlExport.Messages;

    /// <summary>
    /// Some helper methods for the ScrollableTabControl until the functionality can be incorporated fully.
    /// </summary>
    public partial class MainWindowViewModel
    {
        /// <summary>
        /// The query order.
        /// </summary>
        private readonly List<QueryViewModel> queryOrder = new List<QueryViewModel>();

        /// <summary>
        /// Selects the previous query.
        /// </summary>
        /// <param name="m">The m.</param>
        private void SelectPreviousQuery(PreviousQueryMessage m)
        {
            var thisQuery = this.queryOrder.FirstOrDefault();
            var previousViewModel = this.queryOrder.Skip(1).FirstOrDefault();
            if (previousViewModel != null)
            {
                this.queryOrder.Remove(thisQuery);
                this.queryOrder.Add(thisQuery);
                this.SelectedQuery = previousViewModel;
            }
        }

        /// <summary>
        /// Selects the next query.
        /// </summary>
        /// <param name="m">The m.</param>
        private void SelectNextQuery(NextQueryMessage m)
        {
            var nextViewModel = this.queryOrder.LastOrDefault();
            if (nextViewModel != null)
            {
                this.SetSelectedQuery(nextViewModel);
            }
        }

        /// <summary>
        /// Sets the selected query.
        /// </summary>
        /// <param name="query">The next view model.</param>
        private void SetSelectedQuery(QueryViewModel query)
        {
            if (query != null)
            {
                this.queryOrder.Remove(query);
                this.queryOrder.Insert(0, query);
            }

            this.SelectedQuery = query;
        }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="queryText">The query text.</param>
        /// <param name="hasChanged">if set to <c>true</c> [has changed].</param>
        private void AddQuery(string filename, string databaseName, string queryText, bool hasChanged)
        {
            if (!string.IsNullOrEmpty(filename) && !File.Exists(filename))
            {
                ErrorDialogLogic.AddError(string.Concat("The file '", filename, "' could not be found."));
                ErrorDialogLogic.ShowForm();
            }
            else
            {
                var query = new QueryViewModel(filename, hasChanged);
                this.Queries.Add(query);
                this.queryOrder.Insert(0, query);

                this.SetSelectedQuery(query);

                // Wire up the events and messages.
                Messenger.Default.Register<QueryRunningMessage>(this, query, this.QueryRunning);
                Messenger.Default.Register<QueryStoppedMessage>(this, query, this.QueryStopped);
                Messenger.Default.Register<StatusChangedMessage>(this, query, this.StatusChanged);

                // Set all the actual query properties.
                if (!string.IsNullOrEmpty(filename))
                {
                    query.Load();
                }
                else
                {
                    query.EditorViewDataContext.AllText = queryText;
                }

                if (this.Databases != null)
                {
                    var database = this.Databases.FirstOrDefault(d => d != null && d.Name == databaseName);

                    if (database != null)
                    {
                        Messenger.Default.Send(new SetDatabaseMessage(database), query);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the file.
        /// </summary>
        public void RemoveFile()
        {
            this.RemoveQuery(this.SelectedQuery);
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            // TODO
            ////this.SelectedQuery.Connect();
            ////SetQueryImage(this.SelectedQuery);
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            // TODO
            ////this.SelectedQuery.Disconnect();
            ////SetQueryImage(this.SelectedQuery);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            // TODO
            ////this.SelectedQuery.BeginTransaction();
            ////SetQueryImage(this.SelectedQuery);
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            // TODO
            ////this.SelectedQuery.CommitTransaction();
            ////SetQueryImage(this.SelectedQuery.QueryDetails);
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            // TODO
            ////this.SelectedQuery.RollbackTransaction();
            ////SetQueryImage(this.SelectedQuery.QueryDetails);
        }

        /// <summary>
        /// Closes the selected query.
        /// </summary>
        private void CloseQuery()
        {
            this.SelectedQuery.Do(this.RemoveQuery);
        }

        /// <summary>
        /// Saves the selected query.
        /// </summary>
        private void SaveQuery()
        {
            this.SelectedQuery.Do(q => q.Save());
        }

        /// <summary>
        /// Saves the selected query as.
        /// </summary>
        private void SaveQueryAs()
        {
            this.SelectedQuery.Do(q => q.Save());
        }

        /// <summary>
        /// Removes the query.
        /// </summary>
        /// <param name="query">The query.</param>
        private void RemoveQuery(QueryViewModel query)
        {
            if (query != null)
            {
                Messenger.Default.Unregister<QueryRunningMessage>(this, query, this.QueryRunning);
                Messenger.Default.Unregister<QueryStoppedMessage>(this, query, this.QueryStopped);
                Messenger.Default.Unregister<StatusChangedMessage>(this, query, this.StatusChanged);

                this.Queries.Remove(query);
                this.queryOrder.Remove(query);

                Configuration.SetRecentFile(query.Filename);
            }
        }

        /// <summary>
        /// Handles the query running message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void QueryRunning(QueryRunningMessage message)
        {
            // Bubble if the message was raised by the selected query.
            if (this.SelectedQuery != null && message.Sender == this.SelectedQuery)
            {
                Messenger.Default.Send(new StatusChangedMessage(this));
            }
        }

        /// <summary>
        /// Handles the query stopped message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void QueryStopped(QueryStoppedMessage message)
        {
            // Bubble if the message was raised by the selected query.
            if (this.SelectedQuery != null && message.Sender == this.SelectedQuery)
            {
                Messenger.Default.Send(new StatusChangedMessage(this));
            }
        }

        /// <summary>
        /// The status changed.
        /// </summary>
        /// <param name="message">The message.</param>
        private void StatusChanged(StatusChangedMessage message)
        {
            // Bubble if the message was raised by the selected query.
            if (this.SelectedQuery != null && message.Sender == this.SelectedQuery)
            {
                Messenger.Default.Send(new StatusChangedMessage(this));
            }
        }
    }
}

namespace SqlExport.ViewModel
{
    using System;
    using System.IO;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Data;
    using SqlExport.Editor;
    using SqlExport.Logic;
    using SqlExport.Messages;
    using SqlExport.Messages.StatusPanel;
    using SqlExport.View;

    /// <summary>
    /// Defines the QueryViewModel type.
    /// </summary>
    public partial class QueryViewModel : ViewModelBase
    {
        /// <summary>
        /// The elapsed timer.
        /// </summary>
        private readonly DispatcherTimer elapsedTimer;

        /// <summary>
        /// The message count.
        /// </summary>
        private int messageCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryViewModel"/> class.
        /// </summary>
        public QueryViewModel()
            : this(null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryViewModel"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="hasChanged">if set to <c>true</c> [has changed].</param>
        internal QueryViewModel(string filename, bool hasChanged)
        {
            this.ResultsPanelDataContext = DependencyResolver.Default.Resolve<ResultsPanelViewModel>();
            this.StatusPanelDataContext = DependencyResolver.Default.Resolve<StatusPanelViewModel>();
            this.ObjectViewDataContext = DependencyResolver.Default.Resolve<ObjectViewViewModel>();

            try
            {
                this.EditorViewDataContext = DependencyResolver.Default.Resolve<EditorViewViewModel>();
            }
            catch (Exception exp)
            {
                if (!this.IsInDesignMode)
                {
                    ErrorDialogLogic.AddError(exp);
                }

                // Load the default editor if there are any errors.
                this.EditorViewDataContext = new EditorViewViewModel();
            }

            this.EditorViewDataContext.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == EditorViewViewModel.HasChangedPropertyName)
                    {
                        this.RaisePropertyChanged(DisplayTextPropertyName);
                    }
                };

            this.EditorView = DependencyResolver.Default.Resolve<EditorView>();
            this.EditorView.DataContext = this.EditorViewDataContext;

            this.elapsedTimer = new DispatcherTimer();
            this.elapsedTimer.Interval = TimeSpan.FromMilliseconds(10);
            this.elapsedTimer.Tick +=
                (s, e) =>
                Messenger.Default.Send(
                    new SetElapsedTimeMessage(this.controller.Duration.Elapsed), this.StatusPanelDataContext);

            // Results view data context messages.
            Messenger.Default.Register<SelectedResultSetChanged>(
                this,
                this.ResultsPanelDataContext,
                m =>
                {
                    int? resultCount = m.GetResultCount();
                    Messenger.Default.Send(new SetRecordCountMessage(resultCount), this.StatusPanelDataContext);
                    this.CanExport = resultCount.HasValue;
                    Messenger.Default.Send(new StatusChangedMessage(this), this);
                });

            Messenger.Default.Register<ExportStartedMessage>(
                this,
                this.ResultsPanelDataContext,
                m =>
                {
                    this.isRunning = true;
                    OnExport();
                });

            Messenger.Default.Register<ExportCompletedMessage>(
                this,
                this.ResultsPanelDataContext,
                m =>
                {
                    this.isRunning = false;
                    OnStopped();
                });

            Messenger.Default.Register<ErrorSelectedMessage>(
                this,
                this.ResultsPanelDataContext.MessageListDataContext,
                m => Messenger.Default.Send(m, this.EditorViewDataContext));

            // Editor view data context messages.
            Messenger.Default.Register<RunQueryMessage>(this, this.EditorViewDataContext, m => this.RunQuery());
            Messenger.Default.Register<SaveQueryMessage>(this, this.EditorViewDataContext, m => this.Save());
            Messenger.Default.Register<CaretChangedMessage>(
                this, this.EditorViewDataContext, m => Messenger.Default.Send(m, this.StatusPanelDataContext));
            Messenger.Default.Register<DisplayMessage>(
                this,
                this.EditorViewDataContext,
                m => Messenger.Default.Send(m, this.ResultsPanelDataContext.MessageListDataContext));

            // Status view data context messages.
            Messenger.Default.Register<SetDatabaseMessage>(
                this, this.StatusPanelDataContext, m => this.SetDatabase(m.Database));

            // Query view data context messages.
            Messenger.Default.Register<SaveQueryMessage>(this, this, m => this.Save());
            Messenger.Default.Register<SaveQueryAsMessage>(this, this, m => this.SaveAs());
            Messenger.Default.Register<SetDatabaseMessage>(this, this, m => this.SetDatabase(m.Database));
            Messenger.Default.Register<ClearConnectionsMessage>(this, this, m => this.ObjectViewDataContext.Connections.Clear());
            Messenger.Default.Register<AddConnectionMessage>(
                this, this, m => this.ObjectViewDataContext.Connections.Add(m.DatabaseDetails));
            Messenger.Default.Register<RunQueryMessage>(this, this, m => this.RunQuery());
            Messenger.Default.Register<StopQueryMessage>(this, this, m => this.StopQuery());


            this.Filename = filename;
            this.EditorViewDataContext.HasChanged = hasChanged;

            Messenger.Default.Send(new CaretChangedMessage(new CaretDetails()), this.StatusPanelDataContext);

            this.ResetResults();
        }

        /// <summary>
        /// Resets the results.
        /// </summary>
        internal void ResetResults()
        {
            this.ResultsPanelDataContext.SetDataSource(null);
            Messenger.Default.Send(new SetRecordCountMessage(null), this.StatusPanelDataContext);
            Messenger.Default.Send(new SetElapsedTimeMessage(new TimeSpan(0)), this.StatusPanelDataContext);
            Messenger.Default.Send(new SetStatusMessage(string.Empty), this.StatusPanelDataContext);
            this.messageCount = 0;
        }

        /// <summary>
        /// Saves this architect instance.
        /// </summary>
        internal void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Filename))
                {
                    this.SaveAs();
                }
                else
                {
                    using (var fs = new FileStream(this.Filename, FileMode.Create))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            sw.Write(this.EditorViewDataContext.AllText);
                        }
                    }

                    this.EditorViewDataContext.HasChanged = false;
                }
            }
            catch (Exception exp)
            {
                Messenger.Default.Send((DisplayMessage)exp, this.ResultsPanelDataContext.MessageListDataContext);
                Messenger.Default.Send(new ShowMessagesMessage(), this.ResultsPanelDataContext);
            }

            Messenger.Default.Send(new StatusChangedMessage(this), this);
        }

        /// <summary>
        /// Saves as.
        /// </summary>
        internal void SaveAs()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            if (!string.IsNullOrEmpty(this.Filename))
            {
                dlg.FileName = this.Filename;
            }
            else
            {
                dlg.InitialDirectory = Configuration.Current.CurrentDirectory;
            }

            bool? result = dlg.ShowDialog();
            if (result ?? false)
            {
                Configuration.Current.CurrentDirectory = Path.GetDirectoryName(dlg.FileName);
                this.Filename = dlg.FileName;
                this.Save();
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        internal void Load()
        {
            if (!string.IsNullOrEmpty(this.Filename) && File.Exists(this.Filename))
            {
                using (var fs = new FileStream(this.Filename, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        this.EditorViewDataContext.AllText = sr.ReadToEnd();
                    }
                }

                this.EditorViewDataContext.HasChanged = false;
            }
        }

        /// <summary>
        /// Runs the query.
        /// </summary>
        private void RunQuery()
        {
            var sqlStatement = this.EditorViewDataContext.QueryText;
            if (!string.IsNullOrEmpty(sqlStatement) && !this.isRunning)
            {
                if (this.database != null)
                {
                    this.isRunning = true;
                    try
                    {
                        this.ResetResults();

                        Messenger.Default.Send(
                            DisplayMessage.Separator, this.ResultsPanelDataContext.MessageListDataContext);

                        Messenger.Default.Send(new SetStatusMessage("Executing..."), this.StatusPanelDataContext);

                        Messenger.Default.Send(new QueryRunningMessage(this), this);
                        this.SetQueryImage();
                        this.elapsedTimer.Start();

                        this.controller = new Controller();
                        this.controller.Message += this.OnConnectionMessage;
                        this.controller.BeginRun(this.database, new StringReader(sqlStatement), this.OnEndRunQuery);
                    }
                    catch (Exception exp)
                    {
                        this.StopRun(exp);
                    }
                }
                else
                {
                    Messenger.Default.Send(
                        new DisplayMessage("No database has been selected.", DisplayMessageType.Warning),
                        this.ResultsPanelDataContext.MessageListDataContext);
                    Messenger.Default.Send(new ShowMessagesMessage(), this.ResultsPanelDataContext);
                }
            }
        }

        /// <summary>
        /// Stops the query.
        /// </summary>
        private void StopQuery()
        {
            if (this.IsRunning && this.controller != null)
            {
                Messenger.Default.Send(new SetStatusMessage("Canceling..."), this.StatusPanelDataContext);
                this.QueryStatusImage = "QueryStoppedBitmap";
                this.controller.StopRun();
            }
        }

        /// <summary>
        /// Sets the database.
        /// </summary>
        /// <param name="databaseDetails">The database details.</param>
        private void SetDatabase(DatabaseDetails databaseDetails)
        {
            if (this.Database == databaseDetails)
            {
                return;
            }

            if (this.Database != null && this.Database.HasTransaction)
            {
                if (System.Windows.MessageBox.Show(
                    "Rollback transaction and close current connection?",
                    "Set Connection",
                    System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.Cancel)
                {
                    return;
                }

                this.Database.RollbackTransaction();
                this.Database.Disconnect();
            }
            else if (this.Database != null && this.Database.HasConnection)
            {
                if (System.Windows.MessageBox.Show(
                    "Close current connection?",
                    "Set Connection",
                    System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.Cancel)
                {
                    return;
                }

                this.Database.Disconnect();
            }

            if (this.Database != null)
            {
                this.Database.Message -= this.OnConnectionMessage;
            }

            this.Database = databaseDetails != null ? databaseDetails.CreateConnectionContext() : null;
            if (this.Database != null)
            {
                this.Database.Message += this.OnConnectionMessage;
            }

            Messenger.Default.Send(new SetStatusMessage(string.Empty), this.StatusPanelDataContext);
            Messenger.Default.Send(new SetElapsedTimeMessage(TimeSpan.FromSeconds(0)), this.StatusPanelDataContext);
            this.ObjectViewDataContext.Connections.Clear();
            this.ObjectViewDataContext.Connections.Add(this.Database);
            Messenger.Default.Send(new SetDatabaseMessage(databaseDetails), this.StatusPanelDataContext);
            Messenger.Default.Send(new SetDatabaseMessage(databaseDetails), this.EditorViewDataContext);
            Messenger.Default.Send(new StatusChangedMessage(this), this);
        }

        /// <summary>
        /// Called when export starts.
        /// </summary>
        private void OnExport()
        {
            Messenger.Default.Send(new SetStatusMessage("Exporting..."), this.StatusPanelDataContext);
            Messenger.Default.Send(new QueryRunningMessage(this), this);
            this.SetQueryImage();
        }

        /// <summary>
        /// Called when query is stopped.
        /// </summary>
        private void OnStopped()
        {
            Messenger.Default.Send(new SetStatusMessage(string.Empty), this.StatusPanelDataContext);
            Messenger.Default.Send(new QueryStoppedMessage(this), this);
            this.SetQueryImage();
        }

        /// <summary>
        /// Stops the run.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void StopRun(Exception exception)
        {
            this.elapsedTimer.Stop();
            this.isRunning = false;
            Messenger.Default.Send(new SetElapsedTimeMessage(this.controller.Duration.Elapsed), this.StatusPanelDataContext);
            this.controller = null;

            if (exception != null)
            {
                Messenger.Default.Send((DisplayMessage)exception, this.ResultsPanelDataContext.MessageListDataContext);
                Messenger.Default.Send(new ShowMessagesMessage(), this.ResultsPanelDataContext);
            }

            this.OnStopped();
        }

        /// <summary>
        /// Called when end run query.
        /// </summary>
        /// <param name="result">The result.</param>
        private void OnEndRunQuery(IAsyncResult result)
        {
            // This event handler may be called from the run thread and needs to check InvokeRequired.
            Exception error = null;
            try
            {
                this.controller.EndRun(result);

                var ds = this.controller.Data;

                this.elapsedTimer.Stop();

                // Make sure the messages are visible by default.
                Messenger.Default.Send(new ShowMessagesMessage(), this.ResultsPanelDataContext);

                if (this.messageCount == 0)
                {
                    switch (this.controller.Result)
                    {
                        case RunResult.Success:
                            this.OnConnectionMessage(MessageType.Success, "Query completed successfully.", null);
                            break;
                        case RunResult.Failure:
                            this.OnConnectionMessage(MessageType.Information, "Query failed.", null);
                            break;
                        case RunResult.Cancelled:
                            this.OnConnectionMessage(MessageType.Information, "Query cancelled.", null);
                            break;
                    }
                }

                // If there is any new data, those tabs should be made visible by this method.
                this.ResultsPanelDataContext.Readonly = !Configuration.Current.Editable;
                this.ResultsPanelDataContext.SetDataSource(ds);

                TextReader reader = this.controller.Reader;
                reader.Close();
                reader.Dispose();
            }
            catch (Exception exp)
            {
                error = exp;
            }

            this.StopRun(error);
        }

        /// <summary>
        /// Handles the connection message.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="lineNumber">The line number.</param>
        private void OnConnectionMessage(MessageType type, string message, int? lineNumber)
        {
            // This event handler may be called from the run thread and needs to check InvokeRequired.
            this.messageCount++;
            switch (type)
            {
                case MessageType.Error:
                    Messenger.Default.Send(
                        new DisplayMessage(
                            message,
                            message,
                            DisplayMessageType.Warning,
                            () =>
                            {
                                if (lineNumber.HasValue)
                                {
                                    Messenger.Default.Send(new ErrorSelectedMessage(lineNumber.Value), this);
                                }
                            }),
                        this.ResultsPanelDataContext.MessageListDataContext);
                    break;
                case MessageType.Warning:
                    Messenger.Default.Send(
                        new DisplayMessage(message, DisplayMessageType.Warning),
                        this.ResultsPanelDataContext.MessageListDataContext);
                    break;
                case MessageType.Success:
                    Messenger.Default.Send(
                        new DisplayMessage(message, DisplayMessageType.Success),
                        this.ResultsPanelDataContext.MessageListDataContext);
                    break;
                case MessageType.Information:
                default:
                    Messenger.Default.Send(
                        new DisplayMessage(message, DisplayMessageType.Information),
                        this.ResultsPanelDataContext.MessageListDataContext);
                    break;
            }

            Messenger.Default.Send(new ShowMessagesMessage(), this.ResultsPanelDataContext);
        }

        /// <summary>
        /// Sets the query image.
        /// </summary>
        private void SetQueryImage()
        {
            if (this.IsRunning || this.IsExecuting)
            {
                this.QueryStatusImage = "QueryRunningBitmap";
            }
            else if (this.HasTransaction)
            {
                this.QueryStatusImage = "QueryTransactionBitmap";
            }
            else if (this.HasConnection)
            {
                this.QueryStatusImage = "QueryConnectedBitmap";
            }
            else
            {
                this.QueryStatusImage = null;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.DisplayText;
        }
    }
}

namespace SqlExport
{
    using System;
    using System.Windows.Forms;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Data;
    using SqlExport.Logic;
    using SqlExport.Messages;
    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the ApplicationCommands type.
    /// </summary>
    public class ApplicationCommands : ViewModelBase
    {
        /// <summary>
        /// The <see cref="SelectedQuery" /> property's name.
        /// </summary>
        public const string SelectedQueryPropertyName = "SelectedQuery";

        /// <summary>
        /// The selected query.
        /// </summary>
        private QueryViewModel selectedQuery = null;

        /// <summary>
        /// The export results command
        /// </summary>
        private RelayCommand exportResultsCommand;

        /// <summary>
        /// The close query command
        /// </summary>
        private RelayCommand closeQueryCommand;

        /// <summary>
        /// The run query command
        /// </summary>
        private RelayCommand runQueryCommand;

        /// <summary>
        /// The stop query command
        /// </summary>
        private RelayCommand stopQueryCommand;

        /// <summary>
        /// The save query command
        /// </summary>
        private RelayCommand saveQueryCommand;

        /// <summary>
        /// The save query as command
        /// </summary>
        private RelayCommand saveQueryAsCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationCommands"/> class.
        /// </summary>
        public ApplicationCommands()
        {
            Messenger.Default.Register<StatusChangedMessage>(this, m => this.UpdateStatus());
        }

        /// <summary>
        /// Gets the NewQueryCommand.
        /// </summary>
        public RelayCommand<IStatementTemplate> NewQueryCommand
        {
            get
            {
                return new RelayCommand<IStatementTemplate>(
                    t => Messenger.Default.Send(
                        new OpenQueryMessage
                            {
                                QueryText = t != null ? t.Statement : null
                            }));
            }
        }

        /// <summary>
        /// Gets the open command.
        /// </summary>
        public RelayCommand OpenQueryCommand
        {
            get
            {
                return new RelayCommand(this.OpenQuery);
            }
        }

        /// <summary>
        /// Gets the close command.
        /// </summary>
        public RelayCommand CloseQueryCommand
        {
            get
            {
                return this.closeQueryCommand
                       ?? (this.closeQueryCommand =
                           new RelayCommand(() => Messenger.Default.Send(new CloseQueryMessage()), this.CanClose));
            }
        }

        /// <summary>
        /// Gets the save command.
        /// </summary>
        public RelayCommand SaveQueryCommand
        {
            get
            {
                return this.saveQueryCommand
                       ?? (this.saveQueryCommand =
                           new RelayCommand(
                               () => Messenger.Default.Send(new SaveQueryMessage()), this.CanSave));
            }
        }

        /// <summary>
        /// Gets the save as command.
        /// </summary>
        public RelayCommand SaveQueryAsCommand
        {
            get
            {
                return this.saveQueryAsCommand
                       ?? (this.saveQueryAsCommand =
                           new RelayCommand(
                               () => Messenger.Default.Send(new SaveQueryAsMessage()), this.CanSave));
            }
        }

        /// <summary>
        /// Gets the save temp command.
        /// </summary>
        public RelayCommand SaveTempQueriesCommand
        {
            get
            {
                return new RelayCommand(() => Messenger.Default.Send(new SaveTempQueriesMessage()));
            }
        }

        /// <summary>
        /// Gets the exit command.
        /// </summary>
        public RelayCommand ExitCommand
        {
            get
            {
                return new RelayCommand(() => Messenger.Default.Send(new ExitAppMessage()));
            }
        }

        /// <summary>
        /// Gets the run query command.
        /// </summary>
        public RelayCommand RunQueryCommand
        {
            get
            {
                return this.runQueryCommand
                       ?? (this.runQueryCommand =
                           new RelayCommand(
                               () => Messenger.Default.Send(new RunQueryMessage(), this.SelectedQuery), this.CanRun));
            }
        }

        /// <summary>
        /// Gets the stop query command.
        /// </summary>
        public RelayCommand StopQueryCommand
        {
            get
            {
                return this.stopQueryCommand
                       ?? (this.stopQueryCommand =
                           new RelayCommand(
                               () => Messenger.Default.Send(new StopQueryMessage(), this.SelectedQuery), this.CanStop));
            }
        }

        /// <summary>
        /// Gets the export results command.
        /// </summary>
        public RelayCommand ExportResultsCommand
        {
            get
            {
                return this.exportResultsCommand
                       ?? (this.exportResultsCommand =
                           new RelayCommand(() => Messenger.Default.Send(new ExportResultsMessage()), this.CanExport));
            }
        }

        /// <summary>
        /// Gets or sets the SelectedQuery property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public QueryViewModel SelectedQuery
        {
            get
            {
                return this.selectedQuery;
            }

            set
            {
                if (this.selectedQuery == value)
                {
                    return;
                }

                this.selectedQuery = value;
                this.RaisePropertyChanged(SelectedQueryPropertyName);
            }
        }


        /// <summary>
        /// Determines whether this instance can close.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can close; otherwise, <c>false</c>.
        /// </returns>
        private bool CanClose()
        {
            return this.SelectedQuery != null && !(this.SelectedQuery.IsRunning || this.SelectedQuery.IsExecuting);
        }

        /// <summary>
        /// Determines whether this instance can stop.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        /// </returns>
        private bool CanStop()
        {
            return this.SelectedQuery != null && (this.SelectedQuery.IsRunning || this.SelectedQuery.IsExecuting);
        }

        /// <summary>
        /// Determines whether this instance can export.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can export; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExport()
        {
            return this.SelectedQuery != null && this.SelectedQuery.CanExport;
        }

        /// <summary>
        /// Determines whether this instance can run.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can run; otherwise, <c>false</c>.
        /// </returns>
        private bool CanRun()
        {
            return this.SelectedQuery != null &&
                this.SelectedQuery.Database != null &&
                !(this.SelectedQuery.IsRunning || this.SelectedQuery.IsExecuting);
        }

        /// <summary>
        /// Determines whether this instance can save.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can save; otherwise, <c>false</c>.
        /// </returns>
        private bool CanSave()
        {
            return this.SelectedQuery != null;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus()
        {
            var message = new GetSelectedQueryMessage();
            Messenger.Default.Send(message);
            this.SelectedQuery = message.Query;
            
            this.ExportResultsCommand.RaiseCanExecuteChanged();
            this.CloseQueryCommand.RaiseCanExecuteChanged();
            this.RunQueryCommand.RaiseCanExecuteChanged();
            this.StopQueryCommand.RaiseCanExecuteChanged();
            this.SaveQueryCommand.RaiseCanExecuteChanged();
            this.SaveQueryAsCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Opens the query.
        /// </summary>
        private void OpenQuery()
        {
            try
            {
                var openFile = new OpenFileDialog
                    {
                        Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
                        InitialDirectory = Configuration.Current.CurrentDirectory,
                        Multiselect = true
                    };
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    Configuration.Current.CurrentDirectory = openFile.FileName.Substring(0, openFile.FileName.LastIndexOf("\\"));

                    foreach (var item in openFile.FileNames)
                    {
                        Messenger.Default.Send(new OpenQueryMessage { Filename = item });
                    }
                }
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }
        }
    }
}

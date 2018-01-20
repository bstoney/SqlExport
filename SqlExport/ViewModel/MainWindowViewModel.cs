namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Monads;
    using System.Reflection;
    using System.Text;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Data;
    using SqlExport.Logic;
    using SqlExport.Messages;

    /// <summary>
    /// Defines the MainWindowViewModel type.
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The connect command
        /// </summary>
        private RelayCommand connectCommand;

        /// <summary>
        /// The close query command
        /// </summary>
        private RelayCommand<QueryViewModel> closeQueryCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="applicationCommands">The application commands.</param>
        public MainWindowViewModel(ApplicationCommands applicationCommands)
        {
            this.ApplicationCommands = applicationCommands;
            this.Title = Assembly.GetExecutingAssembly().GetName().Name;

            // TODO
            ////this.tabControl.SelectionChanged +=
            ////    (s, a) =>
            ////    {
            ////        // Ensure doc order.
            ////        if (!this.suspendQuerySelection)
            ////        {
            ////            this.SetSelectedQuery(this.SelectedQuery);
            ////        }

            ////        Messenger.Default.Send(
            ////            new SelectedQueryChangedMessage(
            ////                this.SelectedQuery != null ? this.SelectedQuery.QueryDetails : null));
            ////    };
            ////this.tabControl.CloseTab += this.OnQueriesCloseTab;
            ////this.tabControl.NewTab += this.OnQueriesNewTab;

            Messenger.Default.Register<AppStartingMessage>(this, this.AppStarting);
            Messenger.Default.Register<AppExitingMessage>(this, this.AppClosing);
            Messenger.Default.Register<SaveTempQueriesMessage>(this, m => TemporaryFilesLogic.SaveQueryTemp(this.Queries));

            Messenger.Default.Register<ExportResultsMessage>(this, m => this.SelectedQuery.ResultsPanelDataContext.ExportResults());

            Messenger.Default.Register<OpenQueryMessage>(this, m => this.AddQuery(m.Filename, m.DatabaseName, m.QueryText, m.HasChanged));
            Messenger.Default.Register<CloseQueryMessage>(this, m => this.CloseQuery());
            Messenger.Default.Register<SaveQueryMessage>(this, m => this.SaveQuery());
            Messenger.Default.Register<SaveQueryAsMessage>(this, m => this.SaveQueryAs());
            Messenger.Default.Register<NextQueryMessage>(this, this.SelectNextQuery);
            Messenger.Default.Register<PreviousQueryMessage>(this, this.SelectPreviousQuery);

            Messenger.Default.Register<OptionsChangedMessage>(this, m => this.LoadOptions());

            Messenger.Default.Register<StatusChangedMessage>(this, m => this.UpdateStatus());

            Messenger.Default.Register<GetSelectedQueryMessage>(this, m => m.Query = this.SelectedQuery);
            Messenger.Default.Register<GetSelectedDatabase>(this, m => m.SelectedDatabase = this.SelectedDatabase);

            this.LoadOptions();
        }

        /// <summary>
        /// Gets the application commands.
        /// </summary>
        public ApplicationCommands ApplicationCommands { get; private set; }

        /// <summary>
        /// Gets the connect command.
        /// </summary>
        public RelayCommand ConnectCommand
        {
            get
            {
                return this.connectCommand
                       ?? (this.connectCommand =
                           new RelayCommand(
                               () => Messenger.Default.Send(new SetDatabaseMessage(this.SelectedDatabase), this.SelectedQuery),
                               this.CanConnect));
            }
        }

        /// <summary>
        /// Gets the CloseQueryCommand.
        /// </summary>
        public RelayCommand<QueryViewModel> CloseQueryCommand
        {
            get
            {
                return this.closeQueryCommand
                       ?? (this.closeQueryCommand = new RelayCommand<QueryViewModel>(this.RemoveQuery));
            }
        }

        /// <summary>
        /// Determines whether this instance can connect.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can connect; otherwise, <c>false</c>.
        /// </returns>
        private bool CanConnect()
        {
            return this.SelectedDatabase != null && this.SelectedQuery != null
                   && !(this.SelectedQuery.IsRunning || this.SelectedQuery.IsExecuting);
        }

        /// <summary>
        /// Loads the query templates from the selected database templates.
        /// </summary>
        private void LoadTemplates()
        {
            if (this.SelectedDatabase != null)
            {
                try
                {
                    var databaseTemplates = this.SelectedDatabase.GetTemplates();
                    if (databaseTemplates != null)
                    {
                        var catagorisedTemplates = from c in databaseTemplates.Keys.OfType<string>()
                                                   select new
                                                   {
                                                       Name = c,
                                                       Templates =
                                                           from t in databaseTemplates[c]
                                                           select new
                                                           {
                                                               t.Name,
                                                               Command = ApplicationCommands.NewQueryCommand,
                                                               Template = t
                                                           }
                                                   };

                        this.Templates =
                            new ObservableCollection<object>(
                                new object[]
                                    {
                                        new 
                                        { 
                                            Name = "Empty", 
                                            Command = this.ApplicationCommands.NewQueryCommand 
                                        }
                                    }.Concat(catagorisedTemplates));
                    }
                }
                catch (Exception exp)
                {
                    ErrorDialogLogic.AddError(exp);
                }
            }
            else
            {
                this.Templates =
                    new ObservableCollection<object>(
                        new object[]
                            {
                                new
                                {
                                    Name = "Empty", 
                                    Command = this.ApplicationCommands.NewQueryCommand
                                }
                            });
            }
        }

        /// <summary>
        /// Main window opening.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AppStarting(AppStartingMessage message)
        {
            TemporaryFilesLogic.LoadQueryTemp();
            this.SetSelectedQuery(this.Queries.FirstOrDefault());
        }

        /// <summary>
        /// Main window closed.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AppClosing(AppExitingMessage message)
        {
            TemporaryFilesLogic.SaveQueryTemp(this.Queries);
        }

        /// <summary>
        /// Loads the options.
        /// </summary>
        private void LoadOptions()
        {
            this.Databases = new ObservableCollection<DatabaseDetails>(Configuration.Current.GetDatabaseList());
            this.SelectedDatabase = this.Databases.FirstOrDefault(d => d != null && d.Name == Configuration.Current.CurrentDatabase);
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus()
        {
            var sb = new StringBuilder();
            var query = this.SelectedQuery;
            if (query != null && query.Database != null && query.Database.Name != null)
            {
                sb.Append(query.Database.Name);
                sb.Append(" - ");
            }

            sb.Append(ApplicationEnvironment.Default.ProductName);

            if (query != null && !string.IsNullOrEmpty(query.DisplayText))
            {
                sb.AppendFormat(" ({0})", query.DisplayText);
            }

            this.Title = sb.ToString();

            if (query != null)
            {
                this.AddRecentFile(query.Filename);
                this.SelectedDatabase = query.Database;

                Configuration.Current.CurrentDatabase = query.Database.With(d => d.Name);
            }

            this.ConnectCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Adds the recent file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void AddRecentFile(string filename)
        {
            Configuration.SetRecentFile(filename);
        }
    }
}

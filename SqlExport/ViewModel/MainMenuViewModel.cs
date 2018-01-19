namespace SqlExport.ViewModel
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Logic;
    using SqlExport.Messages;
    using SqlExport.View;

    /// <summary>
    /// Defines the MainMenuViewModel type.
    /// </summary>
    public class MainMenuViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="RecentFiles" /> property's name.
        /// </summary>
        public const string RecentFilesPropertyName = "RecentFiles";

        /// <summary>
        /// The recent files.
        /// </summary>
        private ObservableCollection<object> recentFiles = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuViewModel"/> class.
        /// </summary>
        /// <param name="applicationCommands">The application commands.</param>
        public MainMenuViewModel(ApplicationCommands applicationCommands)
        {
            this.ApplicationCommands = applicationCommands;

            Messenger.Default.Register<OptionsChangedMessage>(this, m => this.LoadOptions());
        }

        /// <summary>
        /// Gets the application commands.
        /// </summary>
        public ApplicationCommands ApplicationCommands { get; private set; }

        /// <summary>
        /// Gets or sets the RecentFiles property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<object> RecentFiles
        {
            get
            {
                return this.recentFiles;
            }

            set
            {
                if (this.recentFiles == value)
                {
                    return;
                }

                this.recentFiles = value;
                this.RaisePropertyChanged(RecentFilesPropertyName);
            }
        }

        /// <summary>
        /// Gets the show options command.
        /// </summary>
        public RelayCommand ShowOptionsCommand
        {
            get
            {
                return new RelayCommand(
                    () =>
                    {
                        var optionsWindow = new OptionsWindow();
                        optionsWindow.ShowDialog();
                    });
            }
        }

        /// <summary>
        /// Gets the show all options command.
        /// </summary>
        public RelayCommand ShowAllOptionsCommand
        {
            get
            {
                return new RelayCommand(
                    () =>
                    {
                        var optionsWindow = new AllOptionsWindow();
                        optionsWindow.ShowDialog();
                    });
            }
        }

        /// <summary>
        /// Gets the show errors command.
        /// </summary>
        public RelayCommand ShowErrorsCommand
        {
            get { return new RelayCommand(ShowErrors); }
        }

        /// <summary>
        /// Gets the show about command.
        /// </summary>
        /// <value>
        /// The show about command.
        /// </value>
        public RelayCommand ShowAboutCommand
        {
            get { return new RelayCommand(ShowAbout); }
        }

        /// <summary>
        /// Shows the errors window.
        /// </summary>
        private static void ShowErrors()
        {
            ErrorDialogLogic.ShowForm();
            Messenger.Default.Send(new ApplicationDisplayMessage("Error opened."));
        }

        /// <summary>
        /// Shows the status.
        /// </summary>
        private static void ShowAbout()
        {
            var aboutView = new AboutView();
            aboutView.ShowDialog();
            Messenger.Default.Send(new ApplicationDisplayMessage("About opened."));
        }

        /// <summary>
        /// Loads the options.
        /// </summary>
        private void LoadOptions()
        {
            var optionRecentFiles = Configuration.GetRecentFiles();
            var recentFileItems = from f in optionRecentFiles
                                  where !string.IsNullOrEmpty(f)
                                  select new 
                                    {
                                        Path = f,
                                        Name = Path.GetFileName(f),
                                        Command = new RelayCommand<string>(s => Messenger.Default.Send(new OpenQueryMessage { Filename = s }))
                                    };

            this.RecentFiles = new ObservableCollection<object>(recentFileItems);
        }
    }
}

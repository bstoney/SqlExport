namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Logic;
    using SqlExport.Messages;

    /// <summary>
    /// Defines the OptionsViewModel type.
    /// </summary>
    internal class OptionsViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Databases" /> property's name.
        /// </summary>
        public const string DatabasesPropertyName = "Databases";

        /// <summary>
        /// The <see cref="SelectedDatabase" /> property's name.
        /// </summary>
        public const string SelectedDatabasePropertyName = "SelectedDatabase";

        /// <summary>
        /// The <see cref="DatabaseTypes" /> property's name.
        /// </summary>
        public const string DatabaseTypesPropertyName = "DatabaseTypes";

        /// <summary>
        /// The databases
        /// </summary>
        private ObservableCollection<DatabaseDetailsViewModel> databases = null;

        /// <summary>
        /// The open file command
        /// </summary>
        private RelayCommand openFileCommand;

        /// <summary>
        /// The ok command
        /// </summary>
        private RelayCommand okCommand;

        /// <summary>
        /// The add database command
        /// </summary>
        private RelayCommand addDatabaseCommand;

        /// <summary>
        /// The selected database
        /// </summary>
        private DatabaseDetailsViewModel selectedDatabase = null;

        /// <summary>
        /// The database types
        /// </summary>
        private ObservableCollection<string> databaseTypes = null;

        /// <summary>
        /// The delete database command
        /// </summary>
        private RelayCommand deleteDatabaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsViewModel"/> class.
        /// </summary>
        public OptionsViewModel()
        {
            this.LoadOptions();
        }

        /// <summary>
        /// Gets or sets the Databases property.
        /// </summary>
        public ObservableCollection<DatabaseDetailsViewModel> Databases
        {
            get
            {
                return this.databases;
            }

            set
            {
                if (this.databases == value)
                {
                    return;
                }

                this.databases = value;
                this.RaisePropertyChanged(DatabasesPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the SelectedDatabase property.
        /// </summary>
        public DatabaseDetailsViewModel SelectedDatabase
        {
            get
            {
                return this.selectedDatabase;
            }

            set
            {
                if (this.selectedDatabase == value)
                {
                    return;
                }

                this.selectedDatabase = value;
                this.RaisePropertyChanged(SelectedDatabasePropertyName);
                this.DeleteDatabaseCommand.RaiseCanExecuteChanged();
                this.MoveUpCommand.RaiseCanExecuteChanged();
                this.MoveDownCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the DatabaseTypes property.
        /// </summary>
        public ObservableCollection<string> DatabaseTypes
        {
            get
            {
                return this.databaseTypes;
            }

            set
            {
                if (this.databaseTypes == value)
                {
                    return;
                }

                this.databaseTypes = value;
                this.RaisePropertyChanged(DatabaseTypesPropertyName);
            }
        }

        /// <summary>
        /// Gets the OpenFileCommand.
        /// </summary>
        public RelayCommand OpenFileCommand
        {
            get
            {
                return this.openFileCommand ?? (this.openFileCommand = new RelayCommand(
                    () =>
                    {
                        var p = new Process();
                        p.StartInfo = new ProcessStartInfo(Configuration.SettingsPath);
                        p.Start();

                        Messenger.Default.Send(new CloseWindow(), this);
                    },
                    () => File.Exists(Configuration.SettingsPath)));
            }
        }

        /// <summary>
        /// Gets the OkCommand.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return this.okCommand ?? (this.okCommand = new RelayCommand(() =>
                    {
                        this.SaveOptions();
                        Messenger.Default.Send(new CloseWindow(), this);
                    }));
            }
        }

        /// <summary>
        /// Gets the AddDatabaseCommand.
        /// </summary>
        public RelayCommand AddDatabaseCommand
        {
            get
            {
                return this.addDatabaseCommand ?? (this.addDatabaseCommand = new RelayCommand(
                                               () =>
                                               {
                                                   var newDatabase = new DatabaseDetailsViewModel();
                                                   this.Databases.Add(newDatabase);
                                                   this.SelectedDatabase = newDatabase;
                                               }));
            }
        }

        /// <summary>
        /// Gets the DeleteDatabaseCommand.
        /// </summary>
        public RelayCommand DeleteDatabaseCommand
        {
            get
            {
                return this.deleteDatabaseCommand ?? (this.deleteDatabaseCommand = new RelayCommand(
                                               () =>
                                               {
                                                   var index = this.Databases.IndexOf(this.SelectedDatabase);
                                                   this.Databases.Remove(this.SelectedDatabase);
                                                   this.SelectedDatabase = this.Databases[Math.Min(index, this.Databases.Count - 1)];
                                               },
                                               () => this.SelectedDatabase != null));
            }
        }

        /// <summary>
        /// The move up command
        /// </summary>
        private RelayCommand moveUpCommand;

        /// <summary>
        /// The move down command
        /// </summary>
        private RelayCommand moveDownCommand;

        /// <summary>
        /// Gets the MoveUpCommand.
        /// </summary>
        public RelayCommand MoveUpCommand
        {
            get
            {
                return this.moveUpCommand ?? (this.moveUpCommand = new RelayCommand(
                                               () =>
                                               {
                                                   // Capture the selected database to avoid DataGrid binding errors.
                                                   var database = this.SelectedDatabase;
                                                   this.SelectedDatabase = null;
                                                   var index = this.Databases.IndexOf(database);
                                                   this.Databases.Remove(database);
                                                   this.Databases.Insert(index - 1, database);
                                                   this.SelectedDatabase = database;

                                                   this.MoveUpCommand.RaiseCanExecuteChanged();
                                                   this.MoveDownCommand.RaiseCanExecuteChanged();
                                               },
                                               () => this.SelectedDatabase != null && this.Databases.IndexOf(this.SelectedDatabase) > 0));
            }
        }

        /// <summary>
        /// Gets the MoveDownCommand.
        /// </summary>
        public RelayCommand MoveDownCommand
        {
            get
            {
                return this.moveDownCommand ?? (this.moveDownCommand = new RelayCommand(
                                               () =>
                                               {
                                                   var database = this.SelectedDatabase;
                                                   this.SelectedDatabase = null;
                                                   var index = this.Databases.IndexOf(database);
                                                   this.Databases.Remove(database);
                                                   this.Databases.Insert(index + 1, database);
                                                   this.SelectedDatabase = database;

                                                   this.MoveUpCommand.RaiseCanExecuteChanged();
                                                   this.MoveDownCommand.RaiseCanExecuteChanged();
                                               },
                                               () => this.SelectedDatabase != null && this.Databases.IndexOf(this.SelectedDatabase) < this.Databases.Count - 1));
            }
        }

        /// <summary>
        /// Loads the options.
        /// </summary>
        private void LoadOptions()
        {
            ////chkSameWindow.Checked = Configuration.Current.SingleInstance;
            ////chkEditable.Checked = Configuration.Current.Editable;
            ////chkTransactional.Checked = Configuration.Current.Transactional;
            ////txtTimeout.Text = Configuration.Current.CommandTimeout.ToString();

            ////ddlExportType.Items.Clear();
            ////ddlExportType.Items.AddRange(ExportAdapterHelper.RegisteredExportAdapters);
            ////ddlExportType.Text = Configuration.Current.ExportType;

            var databaseViewModels = from d in Configuration.GetDatabases()
                                     select
                                         new DatabaseDetailsViewModel
                                             {
                                                 Name = d.Name,
                                                 ConnectionString = d.ConnectionString,
                                                 Type = d.Type
                                             };

            this.Databases = new ObservableCollection<DatabaseDetailsViewModel>(databaseViewModels);
            this.SelectedDatabase = this.Databases.FirstOrDefault();

            ////this.editorStyle = new EditorStyle();

            ////this.txtTabWidth.Text = this.editorStyle.TabSize.ToString();
            ////this.chkWordWrap.Checked = this.editorStyle.WordWrap;
            ////this.chkLineNumbers.Checked = this.editorStyle.LineNumbers;

            ////lblEditorFont.Text = GetFontName();

            this.DatabaseTypes = new ObservableCollection<string>(ConnectionAdapterHelper.RegisteredConnectionAdapters);
        }

        /// <summary>
        /// Saves the options.
        /// </summary>
        private void SaveOptions()
        {
            ////var options = Configuration.Current;
            ////options.SingleInstance = chkSameWindow.Checked;
            ////options.Editable = chkEditable.Checked;
            ////options.Transactional = chkTransactional.Checked;
            ////options.CommandTimeout = Convert.ToInt32(txtTimeout.Text);
            ////options.ExportType = ddlExportType.Text;

            ////Configuration.SaveOptionsFrom(options);


            ////this.editorStyle.TabSize = int.Parse(this.txtTabWidth.Text);
            ////this.editorStyle.WordWrap = this.chkWordWrap.Checked;
            ////this.editorStyle.LineNumbers = this.chkLineNumbers.Checked;

            ////Configuration.SaveOptionsFrom(this.editorStyle);

            var databases = (from i in this.Databases
                             select new DatabaseDetails
                             {
                                 Name = i.Name,
                                 ConnectionString = i.ConnectionString,
                                 Type = i.Type
                             }).ToList();

            Configuration.SetDatabases(databases);

            try
            {
                Configuration.SaveOptions();
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }
        }

        /// <summary>
        /// Defines the DatabaseDetailsViewModel class.
        /// </summary>
        public class DatabaseDetailsViewModel : ViewModelBase
        {
            /// <summary>
            /// The name
            /// </summary>
            private string name = null;

            /// <summary>
            /// The connection string
            /// </summary>
            private string connectionString = null;

            /// <summary>
            /// The type
            /// </summary>
            private string type = null;

            /// <summary>
            /// Gets or sets the Name property.
            /// </summary>
            public string Name
            {
                get
                {
                    return this.name;
                }

                set
                {
                    if (this.name == value)
                    {
                        return;
                    }

                    this.name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }

            /// <summary>
            /// Gets or sets the ConnectionString property.
            /// </summary>
            public string ConnectionString
            {
                get
                {
                    return this.connectionString;
                }

                set
                {
                    if (this.connectionString == value)
                    {
                        return;
                    }

                    this.connectionString = value;
                    this.RaisePropertyChanged(() => this.ConnectionString);
                }
            }

            /// <summary>
            /// Gets or sets the Type property.
            /// </summary>
            public string Type
            {
                get
                {
                    return this.type;
                }

                set
                {
                    if (this.type == value)
                    {
                        return;
                    }

                    this.type = value;
                    this.RaisePropertyChanged(() => this.Type);
                }
            }
        }
    }
}

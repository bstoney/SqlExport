namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Messages;

    /// <summary>
    /// Defines the MainWindowViewModel type.
    /// </summary>
    public partial class MainWindowViewModel
    {
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        /// <summary>
        /// The <see cref="Databases" /> property's name.
        /// </summary>
        public const string DatabasesPropertyName = "Databases";

        /// <summary>
        /// The <see cref="SelectedDatabase" /> property's name.
        /// </summary>
        public const string SelectedDatabasePropertyName = "SelectedDatabase";

        /// <summary>
        /// The <see cref="SelectedQuery" /> property's name.
        /// </summary>
        public const string SelectedQueryPropertyName = "SelectedQuery";

        /// <summary>
        /// The <see cref="Templates" /> property's name.
        /// </summary>
        public const string TemplatesPropertyName = "Templates";

        /// <summary>
        /// The <see cref="Queries" /> property's name.
        /// </summary>
        public const string QueriesPropertyName = "Queries";

        /// <summary>
        /// The queries.
        /// </summary>
        private ObservableCollection<QueryViewModel> queries = new ObservableCollection<QueryViewModel>();

        /// <summary>
        /// The templates.
        /// </summary>
        private ObservableCollection<object> templates = null;

        /// <summary>
        /// The selected query.
        /// </summary>
        private QueryViewModel selectedQuery = null;

        /// <summary>
        /// The selected database.
        /// </summary>
        private DatabaseDetails selectedDatabase = null;

        /// <summary>
        /// The databases.
        /// </summary>
        private ObservableCollection<DatabaseDetails> databases = null;

        /// <summary>
        /// The title.
        /// </summary>
        private string title = string.Empty;

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
                Messenger.Default.Send(new StatusChangedMessage(this));
            }
        }

        /// <summary>
        /// Gets or sets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title == value)
                {
                    return;
                }

                this.title = value;
                this.RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the Databases property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<DatabaseDetails> Databases
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
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DatabaseDetails SelectedDatabase
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
                this.ConnectCommand.RaiseCanExecuteChanged();
                this.LoadTemplates();
            }
        }

        /// <summary>
        /// Gets or sets the Templates property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<object> Templates
        {
            get
            {
                return this.templates;
            }

            set
            {
                if (this.templates == value)
                {
                    return;
                }

                this.templates = value;
                this.RaisePropertyChanged(TemplatesPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the Queries property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<QueryViewModel> Queries
        {
            get
            {
                return this.queries;
            }

            set
            {
                if (this.queries == value)
                {
                    return;
                }

                this.queries = value;
                this.RaisePropertyChanged(QueriesPropertyName);
            }
        }
    }
}

namespace SqlExport.ViewModel
{
    using System;
    using System.IO;
    using System.Monads;
    using System.Windows;

    using SqlExport.Data;
    using SqlExport.Editor;

    /// <summary>
    /// Defines the QueryViewModel type.
    /// </summary>
    public partial class QueryViewModel
    {
        /// <summary>
        /// The <see cref="Filename" /> property's name.
        /// </summary>
        public const string FilenamePropertyName = "Filename";

        /// <summary>
        /// The <see cref="Database" /> property's name.
        /// </summary>
        public const string DatabasePropertyName = "Database";

        /// <summary>
        /// The <see cref="IsRunning" /> property's name.
        /// </summary>
        public const string IsRunningPropertyName = "IsRunning";

        /// <summary>
        /// The <see cref="CanExport" /> property's name.
        /// </summary>
        public const string CanExportPropertyName = "CanExport";

        /// <summary>
        /// The <see cref="IsExecuting" /> property's name.
        /// </summary>
        public const string IsExecutingPropertyName = "IsExecuting";

        /// <summary>
        /// The <see cref="HasConnection" /> property's name.
        /// </summary>
        public const string HasConnectionPropertyName = "HasConnection";

        /// <summary>
        /// The <see cref="HasTransaction" /> property's name.
        /// </summary>
        public const string HasTransactionPropertyName = "HasTransaction";

        /// <summary>
        /// The <see cref="QueryStatusImage" /> property's name.
        /// </summary>
        public const string QueryStatusImagePropertyName = "QueryStatusImage";

        /// <summary>
        /// The <see cref="DisplayText" /> property's name.
        /// </summary>
        public const string DisplayTextPropertyName = "DisplayText";

        /// <summary>
        /// The query status image.
        /// </summary>
        private string queryStatusImage = null;

        /// <summary>
        /// The is executing.
        /// </summary>
        private bool isExecuting = false;

        /// <summary>
        /// The has connection.
        /// </summary>
        private bool hasConnection = false;

        /// <summary>
        /// The has transaction.
        /// </summary>
        private bool hasTransaction = false;

        /// <summary>
        /// The database.
        /// </summary>
        private DatabaseConnectionContext database = null;

        /// <summary>
        /// The is running.
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// The can export.
        /// </summary>
        private bool canExport = false;

        /// <summary>
        /// The filename.
        /// </summary>
        private string filename = null;

        /// <summary>
        /// The controller.
        /// </summary>
        private Controller controller;

        /// <summary>
        /// Gets or sets the Filename property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Filename
        {
            get
            {
                return this.filename;
            }

            set
            {
                if (this.filename == value)
                {
                    return;
                }

                this.filename = value;
                this.RaisePropertyChanged(FilenamePropertyName);
                this.RaisePropertyChanged(DisplayTextPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can export.
        /// </summary>
        public bool CanExport
        {
            get
            {
                return this.canExport;
            }

            set
            {
                if (this.canExport == value)
                {
                    return;
                }

                this.canExport = value;
                this.RaisePropertyChanged(CanExportPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }

            set
            {
                if (this.isRunning == value)
                {
                    return;
                }

                this.isRunning = value;
                this.RaisePropertyChanged(IsRunningPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the Database property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DatabaseConnectionContext Database
        {
            get
            {
                return this.database;
            }

            set
            {
                if (this.database == value)
                {
                    return;
                }

                this.database = value;
                this.RaisePropertyChanged(DatabasePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a transaction.
        /// </summary>
        public bool HasTransaction
        {
            get
            {
                return this.hasTransaction;
            }

            set
            {
                if (this.hasTransaction == value)
                {
                    return;
                }

                this.hasTransaction = value;
                this.RaisePropertyChanged(HasTransactionPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a connection.
        /// </summary>
        public bool HasConnection
        {
            get
            {
                return this.hasConnection;
            }

            set
            {
                if (this.hasConnection == value)
                {
                    return;
                }

                this.hasConnection = value;
                this.RaisePropertyChanged(HasConnectionPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is executing.
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                return this.isExecuting;
            }

            set
            {
                if (this.isExecuting == value)
                {
                    return;
                }

                this.isExecuting = value;
                this.RaisePropertyChanged(IsExecutingPropertyName);
            }
        }

        /// <summary>
        /// Gets the results panel data context.
        /// </summary>
        public ResultsPanelViewModel ResultsPanelDataContext { get; private set; }

        /// <summary>
        /// Gets the status panel data context.
        /// </summary>
        public StatusPanelViewModel StatusPanelDataContext { get; private set; }

        /// <summary>
        /// Gets the object view data context.
        /// </summary>
        public ObjectViewViewModel ObjectViewDataContext { get; private set; }

        /// <summary>
        /// Gets the editor view data context.
        /// </summary>
        public EditorViewViewModel EditorViewDataContext { get; private set; }

        /// <summary>
        /// Gets the editor view.
        /// </summary>
        public FrameworkElement EditorView { get; private set; }

        /// <summary>
        /// Gets or sets the QueryStatusImage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string QueryStatusImage
        {
            get
            {
                return this.queryStatusImage;
            }

            set
            {
                if (this.queryStatusImage == value)
                {
                    return;
                }

                this.queryStatusImage = value;
                this.RaisePropertyChanged(QueryStatusImagePropertyName);
            }
        }

        /// <summary>
        /// Gets the DisplayText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DisplayText
        {
            get
            {
                var text = string.IsNullOrEmpty(this.Filename) ? "Untitled" : Path.GetFileName(this.Filename);

                // FUTURE only display changed text for named files.
                if (this.EditorViewDataContext.HasChanged)
                {
                    text = string.Concat("*", text);
                }

                // Replace single underscores with double to escape hot-key assignment.
                return text.With(t => t.Replace("_", "__"));
            }
        }
    }
}

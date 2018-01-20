namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Editor;
    using SqlExport.Logic;
    using SqlExport.Messages;
    using SqlExport.Messages.StatusPanel;
    using SqlExport.SampleData;

    /// <summary>
    /// Defines the StatusPanelViewModel type.
    /// </summary>
    public class StatusPanelViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="SelectedDatabase" /> property's name.
        /// </summary>
        public const string SelectedDatabasePropertyName = "SelectedDatabase";

        /// <summary>
        /// The <see cref="RecordCount" /> property's name.
        /// </summary>
        public const string RecordCountPropertyName = "RecordCount";

        /// <summary>
        /// The <see cref="Status" /> property's name.
        /// </summary>
        public const string StatusPropertyName = "Status";

        /// <summary>
        /// The <see cref="Caret" /> property's name.
        /// </summary>
        public const string CaretPropertyName = "Caret";

        /// <summary>
        /// The <see cref="ElapsedTime" /> property's name.
        /// </summary>
        public const string ElapsedTimePropertyName = "ElapsedTime";

        /// <summary>
        /// The <see cref="Databases" /> property's name.
        /// </summary>
        public const string DatabasesPropertyName = "Databases";

        /// <summary>
        /// The databases
        /// </summary>
        private IEnumerable<DatabaseDetails> databases = null;

        /// <summary>
        /// The elapsed time.
        /// </summary>
        private TimeSpan elapsedTime = TimeSpan.FromSeconds(0);

        /// <summary>
        /// The record count.
        /// </summary>
        private int? recordCount = null;

        /// <summary>
        /// The status.
        /// </summary>
        private string status = "<Status>";

        /// <summary>
        /// The _connection.
        /// </summary>
        private DatabaseDetails selectedDatabase = null;

        /// <summary>
        /// The lines.
        /// </summary>
        private CaretDetails caret;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusPanelViewModel"/> class.
        /// </summary>
        public StatusPanelViewModel()
        {
            Messenger.Default.Register<SetDatabaseMessage>(this, this, m => this.SelectedDatabase = m.Database);
            Messenger.Default.Register<SetStatusMessage>(this, this, m => this.Status = m.Status);
            Messenger.Default.Register<SetElapsedTimeMessage>(this, this, m => this.ElapsedTime = m.ElapsedTime);
            Messenger.Default.Register<CaretChangedMessage>(this, this, m => this.Caret = m.CaretDetails);
            Messenger.Default.Register<SetRecordCountMessage>(this, this, m => this.RecordCount = m.RecordCount);
            Messenger.Default.Register<OptionsChangedMessage>(this, m => this.LoadOptions());

            this.LoadOptions();

            if (this.IsInDesignMode)
            {
                SampleConnectionAdapter.Initialise();

                this.Databases = new[] 
                { 
                    null,
                    new DatabaseDetails 
                    {
                        Name = "Demo Data",
                        Type = SampleConnectionAdapter.SampleConnectionAdapterName
                    } 
                };
            }
        }

        /// <summary>
        /// Gets or sets the selected database.
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

                Messenger.Default.Send(new SetDatabaseMessage(this.SelectedDatabase), this);
            }
        }

        /// <summary>
        /// Gets or sets the caret.
        /// </summary>
        /// <value>
        /// The caret.
        /// </value>
        public CaretDetails Caret
        {
            get
            {
                return this.caret;
            }

            set
            {
                if (this.caret == value)
                {
                    return;
                }

                this.caret = value;
                this.RaisePropertyChanged(CaretPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the elapsed time.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get
            {
                return this.elapsedTime;
            }

            set
            {
                if (this.elapsedTime == value)
                {
                    return;
                }

                this.elapsedTime = value;
                this.RaisePropertyChanged(ElapsedTimePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the record count.
        /// </summary>
        public int? RecordCount
        {
            get
            {
                return this.recordCount;
            }

            set
            {
                if (this.recordCount == value)
                {
                    return;
                }

                this.recordCount = value;
                this.RaisePropertyChanged(RecordCountPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                if (this.status == value)
                {
                    return;
                }

                this.status = value;
                this.RaisePropertyChanged(StatusPropertyName);
            }
        }

        /// <summary>
        /// Gets the databases.
        /// </summary>
        public IEnumerable<DatabaseDetails> Databases
        {
            get
            {
                return this.databases;
            }

            private set
            {
                this.databases = value;
                this.RaisePropertyChanged(DatabasesPropertyName);
            }
        }

        /// <summary>
        /// Loads the options.
        /// </summary>
        private void LoadOptions()
        {
            this.Databases = Configuration.Current.GetDatabaseList();
        }
    }
}

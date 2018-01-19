using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using SqlExport.Ui.Messages.StatusPanel;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Data;
using System.Windows.Data;
using SqlExport.Ui.Messages;

namespace SqlExport.ViewModel
{
    using SqlExport.Common.Data;
    using SqlExport.Ui.Business;

    /// <summary>
    /// Defines the StatusPanelViewModel type.
    /// </summary>
    public class StatusPanelViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Connection" /> property's name.
        /// </summary>
        public const string ConnectionPropertyName = "Connection";

        /// <summary>
        /// The <see cref="RecordCount" /> property's name.
        /// </summary>
        public const string RecordCountPropertyName = "RecordCount";

        /// <summary>
        /// The <see cref="Status" /> property's name.
        /// </summary>
        public const string StatusPropertyName = "Status";

        /// <summary>
        /// The <see cref="SelectedTextRange" /> property's name.
        /// </summary>
        public const string LinesPropertyName = "SelectedTextRange";

        /// <summary>
        /// The <see cref="ElapsedTime" /> property's name.
        /// </summary>
        public const string ElapsedTimePropertyName = "ElapsedTime";

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
        private DatabaseDetails connection = null;

        /// <summary>
        /// The lines.
        /// </summary>
        private SelectedTextRange lines = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusPanelViewModel"/> class.
        /// </summary>
        public StatusPanelViewModel()
        {
            Messenger.Default.Register<SetDatabaseMessage>(this, this, m => this.Connection = m.Database);
            Messenger.Default.Register<SetStatusMessage>(this, this, m => this.Status = m.Status);
            Messenger.Default.Register<SetElapsedTimeMessage>(this, this, m => this.ElapsedTime = m.ElapsedTime);
            Messenger.Default.Register<SetLinesMessage>(this, this, m => this.SelectedTextRange = m.SelectedTextRange);
            Messenger.Default.Register<SetRecordCountMessage>(this, this, m => this.RecordCount = m.RecordCount);
        }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public DatabaseDetails Connection
        {
            get
            {
                return this.connection;
            }

            set
            {
                if (this.connection == value)
                {
                    return;
                }

                this.connection = value;
                this.RaisePropertyChanged(ConnectionPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the selected text range.
        /// </summary>
        public SelectedTextRange SelectedTextRange
        {
            get
            {
                return this.lines;
            }

            set
            {
                if (this.lines == value)
                {
                    return;
                }

                this.lines = value;
                this.RaisePropertyChanged(LinesPropertyName);
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
    }
}

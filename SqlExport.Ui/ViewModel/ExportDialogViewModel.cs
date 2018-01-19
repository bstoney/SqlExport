using System;
using System.Collections.Generic;
using System.Linq;
using SqlExport.Ui.Messages;

namespace SqlExport.ViewModel
{
    using System.Text;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using SqlExport.Ui.Messages;

    /// <summary>
    /// Defines the ExportDialogViewModel type.
    /// </summary>
    public class ExportDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Properties" /> property's name.
        /// </summary>
        public const string PropertiesPropertyName = "Properties";

        /// <summary>
        /// The properties.
        /// </summary>
        private IEnumerable<PropertyItem> properties = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDialogViewModel"/> class.
        /// </summary>
        public ExportDialogViewModel()
        {
            Messenger.Default.Register<SetPropertiesMessage>(this, this, m => this.Properties = m.Properties);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                Messenger.Default.Send(
                    new SetPropertiesMessage(new[] { new FilePropertyItem("General", "Filename", string.Empty) }), this);
            }
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public IEnumerable<PropertyItem> Properties
        {
            get
            {
                return this.properties;
            }

            set
            {
                if (this.properties == value)
                {
                    return;
                }

                this.properties = value;
                this.RaisePropertyChanged(PropertiesPropertyName);
            }
        }

        /// <summary>
        /// Gets the OkCommand.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return new RelayCommand(
                                          () =>
                                          {
                                              Messenger.Default.Send(new CloseWindow(), this);
                                              Messenger.Default.Send(new DialogOkMessage(), this);
                                          });
            }
        }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return new RelayCommand(
                                          () =>
                                          {
                                              Messenger.Default.Send(new CloseWindow(), this);
                                              Messenger.Default.Send(new DialogCancelMessage(), this);
                                          });
            }
        }
    }
}

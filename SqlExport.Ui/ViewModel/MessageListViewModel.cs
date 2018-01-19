using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Windows;
using SqlExport.Ui.Properties;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Ui.Messages;

namespace SqlExport.ViewModel
{
    using SqlExport.Messages;

    /// <summary>
    /// Defines the MessageListViewModel type.
    /// </summary>
    public class MessageListViewModel : ViewModelBase
    {
        /// <summary>
        /// The images.
        /// </summary>
        private static readonly Dictionary<DisplayMessageType, System.Drawing.Image> Images =
            new Dictionary<DisplayMessageType, System.Drawing.Image>
                {
                    { DisplayMessageType.Error, Resources.error },
                    { DisplayMessageType.Information, Resources.information },
                    { DisplayMessageType.None, null },
                    { DisplayMessageType.Success, Resources.accept },
                    { DisplayMessageType.Warning, Resources.exclamation }
                };

        /// <summary>
        /// The <see cref="DetailsText" /> property's name.
        /// </summary>
        public const string DetailsTextPropertyName = "DetailsText";

        /// <summary>
        /// The <see cref="ScopeToken" /> property's name.
        /// </summary>
        public const string ScopeTokenPropertyName = "ScopeToken";

        /// <summary>
        /// The <see cref="SelectedItem" /> property's name.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";

        /// <summary>
        /// The mutex.
        /// </summary>
        private readonly object mutex = new object();

        /// <summary>
        /// The selected item.
        /// </summary>
        private MessageItemViewModel selectedItem = null;

        /// <summary>
        /// The scope token.
        /// </summary>
        private string scopeToken = null;

        /// <summary>
        /// The details text.
        /// </summary>
        private string detailsText = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListViewModel"/> class.
        /// </summary>
        public MessageListViewModel()
        {
            this.Messages = new SafeObservable<MessageItemViewModel>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                this.Messages.Add(new MessageItemViewModel(Resources.information, "A sample message for design time\nThis illustrate a multi line message.", null));
                this.Messages.Add(new MessageItemViewModel(Resources.accept, "A second sample message.", null));
                this.Messages.Add(new MessageItemViewModel(Resources.accept, "A second sample message.", null));
                this.Messages.Add(new MessageItemViewModel(null, MessageItemViewModel.SeparatorText, null));

                this.DetailsText = "Details go here.";
            }
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        public SafeObservable<MessageItemViewModel> Messages { get; private set; }

        /// <summary>
        /// Gets the clear command.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get { return new RelayCommand(this.ClearItems); }
        }

        /// <summary>
        /// Gets the item selection changed command.
        /// </summary>
        public RelayCommand ItemSelectionChangedCommand
        {
            get { return new RelayCommand(this.UpdateDetails); }
        }

        /// <summary>
        /// Gets the item execute command.
        /// </summary>
        public RelayCommand ItemExecuteCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedItems.Count() == 1)
                    {
                        MessageData ed = SelectedItem.Tag as MessageData;
                        if (ed != null && ed.Callback != null)
                        {
                            ed.Callback();
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Gets or sets the ScopeToken property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string ScopeToken
        {
            get
            {
                return this.scopeToken;
            }

            set
            {
                if (this.scopeToken != value)
                {
                    Messenger.Default.Unregister<DisplayMessage>(this);

                    this.scopeToken = value;

                    Messenger.Default.Register<DisplayMessage>(this, this.scopeToken, MessageReceived);

                    // Update bindings, no broadcast
                    this.RaisePropertyChanged(ScopeTokenPropertyName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the DetailsText property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string DetailsText
        {
            get
            {
                return this.detailsText;
            }

            set
            {
                if (this.detailsText != value)
                {
                    this.detailsText = value;

                    // Update bindings, no broadcast
                    this.RaisePropertyChanged(DetailsTextPropertyName);
                }
            }
        }

        /// <summary>
        /// Gets the SelectedItem property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public MessageItemViewModel SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                if (this.selectedItem == value)
                {
                    return;
                }

                this.selectedItem = value;

                // Update bindings, no broadcast
                this.RaisePropertyChanged(SelectedItemPropertyName);
                this.UpdateDetails();
            }
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        private IEnumerable<MessageItemViewModel> SelectedItems
        {
            get { return this.Messages.Where(m => m.IsSelected); }
        }

        /// <summary>
        /// Removes all the items.
        /// </summary>
        public void ClearItems()
        {
            this.Messages.Clear();
            this.DetailsText = null;
        }

        /// <summary>
        /// Adds a separator line.
        /// </summary>
        public void AddSeperator()
        {
            this.AddItem(MessageItemViewModel.SeparatorText, null, DisplayMessageType.None);
        }

        /// <summary>
        /// Adds an exception with the stack trace as message detail.
        /// </summary>
        /// <param name="error">The error.</param>
        public void AddError(Exception error)
        {
            this.AddMessage(error.Message, error.ToString(), DisplayMessageType.Error, null);
        }

        /// <summary>
        /// Adds a message with the error icon and optional line number.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="lineNumber">The line number.</param>
        public void AddError(string message, int? lineNumber)
        {
            this.AddMessage(
                message,
                message,
                DisplayMessageType.Error,
                () =>
                {
                    if (lineNumber.HasValue)
                    {
                        Messenger.Default.Send(new ErrorSelectedMessage(lineNumber.Value), ScopeToken);
                    }
                });
        }

        /// <summary>
        /// Adds a message of the specified type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public void AddMessage(string message, DisplayMessageType type)
        {
            this.AddMessage(message, type, null);
        }

        /// <summary>
        /// Adds a message of the specified type, and will invoke the call back on double click.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <param name="callback">The callback.</param>
        public void AddMessage(string message, DisplayMessageType type, Action callback)
        {
            this.AddMessage(message, message, type, callback);
        }

        /// <summary>
        /// Messages the received.
        /// </summary>
        /// <param name="message">The message.</param>
        private void MessageReceived(DisplayMessage message)
        {
            this.AddMessage(message.Text, message.Details, message.Type, message.Callback);
        }

        /// <summary>
        /// Adds a message of the specified type, and will invoke the call back on double click.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="description">The description.</param>
        /// <param name="type">The type.</param>
        /// <param name="callback">The callback.</param>
        private void AddMessage(string message, string description, DisplayMessageType type, Action callback)
        {
            this.AddItem(
                message,
                new MessageData
                    {
                        Message = message,
                        Description = description,
                        Callback = callback
                    },
                type);
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        private void AddItem(string message, object data, DisplayMessageType type)
        {
            lock (this.mutex)
            {
                var newItem = new MessageItemViewModel(Images[type], message, data);
                var item = this.Messages.FirstOrDefault();

                if (item == null || newItem.IsSeparator || !(newItem.Text == item.Text && object.Equals(newItem.Tag, item.Tag)))
                {
                    this.Messages.Insert(0, newItem);
                }
                else
                {
                    item.Occurrences++;
                }

                this.SelectedItem = item;
            }
        }

        /// <summary>
        /// Updates the details.
        /// </summary>
        private void UpdateDetails()
        {
            var selectedItems = this.SelectedItems;
            if (selectedItems.Any())
            {
                StringBuilder sb = new StringBuilder();

                var query = from item in selectedItems
                            from repeatedItems in Enumerable.Repeat(item, item.Occurrences)
                            select repeatedItems;

                lock (this.mutex)
                {
                    foreach (var item in query.Select((c, i) => new { c, i }))
                    {
                        if (item.i > 0)
                        {
                            sb.AppendLine("--------------------------------------------------");
                        }

                        var messageData = item.c.Tag as MessageData;
                        if (messageData != null)
                        {
                            sb.AppendLine(messageData.Description);
                        }
                        else
                        {
                            var details = item.c.Tag as string;
                            if (details != null)
                            {
                                sb.AppendLine(details);
                            }
                            else
                            {
                                sb.AppendLine(item.c.ToString());
                            }
                        }
                    }
                }

                this.DetailsText = sb.ToString();
            }
            else
            {
                this.DetailsText = null;
            }
        }

        /// <summary>
        /// A wrapper class for messages with line numbers.
        /// </summary>
        private class MessageData
        {
            /// <summary>
            /// Gets or sets the message text.
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Gets or sets the description text.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets a double click call back.
            /// </summary>
            public Action Callback { get; set; }

            /// <summary>
            /// Returns a string which represents the current object.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return Message;
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="a">A.</param>
            /// <param name="b">The b.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator ==(MessageData a, MessageData b)
            {
                if (object.ReferenceEquals(a, b))
                {
                    return true;
                }

                if ((object)a == null)
                {
                    return false;
                }

                return a.Equals(b);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="a">A.</param>
            /// <param name="b">The b.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator !=(MessageData a, MessageData b)
            {
                if (object.ReferenceEquals(a, b))
                {
                    return false;
                }

                if ((object)a == null)
                {
                    return true;
                }

                return !a.Equals(b);
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                return Equals(obj as MessageData);
            }

            /// <summary>
            /// Equalses the specified message data.
            /// </summary>
            /// <param name="messageData">The message data.</param>
            /// <returns></returns>
            public bool Equals(MessageData messageData)
            {
                return (object)messageData != null && Message == messageData.Message
                       && Description == messageData.Description && Callback == messageData.Callback;
            }
        }
    }
}

namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Messages;
    using SqlExport.Properties;

    /// <summary>
    /// Defines the MessageListViewModel type.
    /// </summary>
    public class MessageListViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="DetailsText" /> property's name.
        /// </summary>
        public const string DetailsTextPropertyName = "DetailsText";

        /// <summary>
        /// The <see cref="SelectedItem" /> property's name.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";

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
        /// The mutex.
        /// </summary>
        private readonly object mutex = new object();

        /// <summary>
        /// The selected item.
        /// </summary>
        private MessageItemViewModel selectedItem = null;

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

            Messenger.Default.Register<DisplayMessage>(this, this, this.MessageReceived);
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
        /// Gets or sets the details text.
        /// </summary>
        public string DetailsText
        {
            get
            {
                return this.detailsText;
            }

            set
            {
                if (this.detailsText == value)
                {
                    return;
                }

                this.detailsText = value;
                this.RaisePropertyChanged(DetailsTextPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
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
        private void ClearItems()
        {
            this.Messages.Clear();
            this.DetailsText = null;
        }

        /// <summary>
        /// Messages the received.
        /// </summary>
        /// <param name="message">The message.</param>
        private void MessageReceived(DisplayMessage message)
        {
            // Only add a separator as requested if there is an existing message and its not a separator
            var lastMessage = this.Messages.FirstOrDefault();
            if (message == DisplayMessage.Separator && lastMessage != null && lastMessage.IsSeparator)
            {
                return;
            }

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
            var query = from item in this.SelectedItems
                        from repeatedItems in Enumerable.Repeat(item, item.Occurrences)
                        select repeatedItems;

            List<MessageItemViewModel> selectedItemsList;
            lock (this.mutex)
            {
                selectedItemsList = query.ToList();
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < selectedItemsList.Count; i++)
            {
                var item = selectedItemsList[i];
                if (i > 0)
                {
                    sb.AppendLine("--------------------------------------------------");
                }

                var messageData = item.Tag as MessageData;
                if (messageData != null)
                {
                    sb.AppendLine(messageData.Description);
                }
                else
                {
                    var details = item.Tag as string;
                    sb.AppendLine(details ?? item.ToString());
                }
            }

            this.DetailsText = sb.ToString();
        }

        /// <summary>
        /// A wrapper class for messages with line numbers.
        /// </summary>
        private class MessageData : IEquatable<MessageData>
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
            /// Implements the operator ==.
            /// </summary>
            /// <param name="a">Message A.</param>
            /// <param name="b">Message B.</param>
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
            /// <param name="a">Message A.</param>
            /// <param name="b">Message B.</param>
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
            /// Returns a string which represents the current object.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return this.Message;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = this.Message != null ? this.Message.GetHashCode() : 0;
                    hashCode = (hashCode * 397) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (this.Callback != null ? this.Callback.GetHashCode() : 0);
                    return hashCode;
                }
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
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return this.Equals((MessageData)obj);
            }

            /// <summary>
            /// Determines whether the specified <see cref="MessageData"/> is equal to this instance.
            /// </summary>
            /// <param name="messageData">The message data.</param>
            /// <returns>
            ///   <c>true</c> if the specified <see cref="MessageData"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public bool Equals(MessageData messageData)
            {
                return messageData != null && string.Equals(this.Message, messageData.Message)
                       && string.Equals(this.Description, messageData.Description)
                       && this.Callback == messageData.Callback;
            }
        }
    }
}

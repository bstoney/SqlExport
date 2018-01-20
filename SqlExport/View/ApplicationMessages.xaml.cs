namespace SqlExport.View
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interop;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Messages;
    using SqlExport.ViewModel;

    /// <summary>
    /// Interaction logic for ApplicationMessages
    /// </summary>
    public partial class ApplicationMessages
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationMessages"/> class.
        /// </summary>
        public ApplicationMessages()
        {
            this.InitializeComponent();

            this.MessageListDataContext = DependencyResolver.Default.Resolve<MessageListViewModel>();

            Messenger.Default.Register<ApplicationDisplayMessage>(this, this.MessageReceived);
        }

        /// <summary>
        /// Gets the message list data context.
        /// </summary>
        public object MessageListDataContext { get; private set; }

        /// <summary>
        /// Shows the form internal.
        /// </summary>
        internal void ShowFormInternal()
        {
            if (!this.IsFocused)
            {
                var message = new GetMainWindowMessage();
                Messenger.Default.Send(message);
                this.Owner = message.MainWindow; 
                
                this.Show();
                this.Focus();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// Messages the received.
        /// </summary>
        /// <param name="message">The message.</param>
        private void MessageReceived(ApplicationDisplayMessage message)
        {
            Messenger.Default.Send((DisplayMessage)message, this.MessageListDataContext);
        }
    }
}

namespace SqlExport.View
{
    using System.Windows;
    using System.Windows.Interop;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Logic;
    using SqlExport.Messages;

    /// <summary>
    /// Defines the DialogViewBase type.
    /// </summary>
    public class DialogViewBase : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewBase"/> class.
        /// </summary>
        public DialogViewBase()
        {
            var message = new GetMainWindowMessage();
            Messenger.Default.Send(message);
            this.Owner = message.MainWindow;

            this.WindowStyle = WindowStyle.ToolWindow;
            this.ResizeMode = ResizeMode.CanResize;
            this.Name = "Dialog";
            this.RemoveIcon();
            this.ShowInTaskbar = false;
            this.Topmost = true;

            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.Initialized += this.DialogViewBaseInitialized;
        }

        /// <summary>
        /// Handles the initialized event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DialogViewBaseInitialized(object sender, System.EventArgs e)
        {
            Messenger.Default.Register<CloseWindow>(this, this.DataContext, m => this.Hide());
        }
    }
}

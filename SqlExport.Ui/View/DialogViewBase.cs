namespace SqlExport.View
{
    using System.Windows;
    using System.Windows.Interop;
    using GalaSoft.MvvmLight.Messaging;
    using SqlExport.Ui.Messages;

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
            Messenger.Default.Send(new GetMainWindowMessage(w => this.Owner = w));

            this.Initialized += this.DialogViewBaseInitialized;
        }

        private void DialogViewBaseInitialized(object sender, System.EventArgs e)
        {
            Messenger.Default.Register<CloseWindow>(this, this.DataContext, m => this.Hide());
        }
    }
}

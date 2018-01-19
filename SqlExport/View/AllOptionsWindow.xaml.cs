namespace SqlExport.View
{
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Messages;
    using SqlExport.ViewModel;

    /// <summary>
    /// Interaction logic for AllOptionsWindow
    /// </summary>
    public partial class AllOptionsWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllOptionsWindow"/> class.
        /// </summary>
        public AllOptionsWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Trees the view_ selected item changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Messenger.Default.Send(new OptionSelectedMessage(this.OptionsList.SelectedItem as OptionViewModel), this.DataContext);
        }
    }
}

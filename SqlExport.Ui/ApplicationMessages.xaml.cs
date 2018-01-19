namespace SqlExport
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interop;

    using SqlExport.ViewModel;

    /// <summary>
    /// Interaction logic for ApplicationMessages.xaml
    /// </summary>
    public partial class ApplicationMessages : Window
    {
        public ApplicationMessages()
        {
            InitializeComponent();

            var locator = ViewModelLocator.FindLocator(this);
            var vm = locator.ApplicationMessages;
            DataContext = vm;

            // TODO this doesn't seem to work with binding???
            lstMessages.ScopeToken = vm.ScopeToken;
        }

        // TODO change this to use messaging.
        internal MessageListViewModel MessageList { get { return lstMessages.ViewModel; } }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Hide();
            e.Cancel = true;
        }

        private void OnFormMouseHover(object sender, EventArgs e)
        {
            Focus();
        }

        private void OnCloseClick(System.Object sender, System.EventArgs e)
        {
            Hide();
        }

        /// <summary>
        /// Shows the form internal.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="ownderHandle">The owner handle.</param>
        internal void ShowFormInternal(Window owner = null, IntPtr ownerHandle = default(IntPtr))
        {
            if (!this.IsFocused)
            {
                // TODO
                ////SetLocation();
                
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (owner != null)
                {
                    this.Owner = owner;
                }
                else if (ownerHandle != default(IntPtr))
                {
                    WindowInteropHelper helper = new WindowInteropHelper(this);
                    helper.Owner = ownerHandle;
                }

                this.Show();
                this.Focus();
            }
        }

    }
}

namespace SqlExport.View
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Monads;
    using System.Windows;
    using System.Windows.Forms;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Common.Editor;
    using SqlExport.Data;
    using SqlExport.Editor;
    using SqlExport.Logic;
    using SqlExport.Messages;
    using SqlExport.Messages.StatusPanel;
    using SqlExport.ViewModel;

    using Clipboard = System.Windows.Clipboard;
    using Control = System.Windows.Controls.Control;
    using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

    /// <summary>
    /// Interaction logic for EditorView
    /// </summary>
    public partial class EditorView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorView"/> class.
        /// </summary>
        public EditorView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement"/> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == Control.DataContextProperty)
            {
                var oldViewModel = e.OldValue as EditorViewViewModel;
                var newViewModel = e.NewValue as EditorViewViewModel;

                Messenger.Default.Unregister<FocusFindMessage>(this);

                if (oldViewModel != null)
                {
                    oldViewModel.EditorControl.SetContextMenu(null);
                }

                if (newViewModel != null)
                {
                    newViewModel.EditorControl.SetContextMenu(this.ContextMenu.ToForms());
                    this.EditorHost.Child = newViewModel.EditorControl.Control;

                    Messenger.Default.Register<FocusFindMessage>(this, newViewModel, m => this.FindText.Focus());
                }
                else
                {
                    this.EditorHost.Child = null;
                }
            }
        }
    }
}

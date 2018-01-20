namespace SqlExport.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    using SqlExport.Common.Data;
    using SqlExport.ViewModel;

    /// <summary>
    /// Interaction logic for ObjectView
    /// </summary>
    public partial class ObjectView
    {
        /// <summary>
        /// The drag start point
        /// </summary>
        private Point dragStartPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectView"/> class.
        /// </summary>
        public ObjectView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event of the ObjectView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void ObjectView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dragStartPoint = e.GetPosition(null);
        }

        /// <summary>
        /// Handles the MouseMove event of the ObjectView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ObjectView_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = this.dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var dragDropNodes = new[] { this.TreeView.SelectedItem };

                var dragText = ObjectViewItemViewModel.GetItemsAsText(dragDropNodes.Cast<ObjectViewItemViewModel>());

                DragDrop.DoDragDrop(this.TreeView, dragText, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Handles the ContextMenuOpening event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ContextMenuEventArgs"/> instance containing the event data.</param>
        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                var item = element.DataContext as ObjectViewItemViewModel;
                if (item != null)
                {
                    item.IsSelected = true;
                }
            }
        }
    }
}

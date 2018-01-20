namespace SqlExport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Threading;

    /// <summary>
    /// Defines the ScrollableTabControl type.
    /// </summary>
    public class ScrollableTabControl : TabControl
    {
        /// <summary>
        /// The close tab event.
        /// </summary>
        public static readonly RoutedEvent CloseTabEvent = EventManager.RegisterRoutedEvent(
            "CloseTab", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollableTabControl));

        /// <summary>
        /// The new tab event.
        /// </summary>
        public static readonly RoutedEvent NewTabEvent = EventManager.RegisterRoutedEvent(
            "NewTab", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollableTabControl));

        /// <summary>
        /// The new button.
        /// </summary>
        private Button newButton;

        /// <summary>
        /// The close button.
        /// </summary>
        private Button closeButton;

        /// <summary>
        /// The scroll left button.
        /// </summary>
        private RepeatButton scrollLeftButton;

        /// <summary>
        /// The scroll right button.
        /// </summary>
        private RepeatButton scrollRightButton;

        /// <summary>
        /// The select button.
        /// </summary>
        private Button selectButton;

        /// <summary>
        /// The tab scroll viewer.
        /// </summary>
        private ScrollViewer tabScrollViewer;

        /// <summary>
        /// Initializes the <see cref="ScrollableTabControl"/> class.
        /// </summary>
        static ScrollableTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ScrollableTabControl), new FrameworkPropertyMetadata(typeof(ScrollableTabControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollableTabControl"/> class.
        /// </summary>
        public ScrollableTabControl()
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("pack://application:,,,/SQLExport;component/Resources/Styles/Popup.xaml");
            Resources.MergedDictionaries.Add(rd);
        }

        /// <summary>
        /// Occurs when [close tab].
        /// </summary>
        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }

        /// <summary>
        /// Occurs when [new tab].
        /// </summary>
        public event RoutedEventHandler NewTab
        {
            add { AddHandler(NewTabEvent, value); }
            remove { RemoveHandler(NewTabEvent, value); }
        }

        /// <summary>
        /// Updates the control status.
        /// </summary>
        private void UpdateControlStatus()
        {
            this.closeButton.IsEnabled = Items.Count > 0;
            this.scrollLeftButton.IsEnabled = this.tabScrollViewer.HorizontalOffset > 0;
            this.scrollRightButton.IsEnabled = this.tabScrollViewer.HorizontalOffset < this.tabScrollViewer.ScrollableWidth;
            this.selectButton.IsEnabled = Items.Count > 0;
        }

        /// <summary>
        /// Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/> is called.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.newButton = base.GetTemplateChild("PART_New") as Button;
            if (this.newButton != null)
            {
                this.newButton.Click += this.OnNewClicked;
            }

            this.closeButton = base.GetTemplateChild("PART_Close") as Button;
            if (this.closeButton != null)
            {
                this.closeButton.Click += this.OnCloseClicked;
            }

            this.scrollLeftButton = base.GetTemplateChild("PART_Left") as RepeatButton;
            if (this.scrollLeftButton != null)
            {
                this.scrollLeftButton.Click += this.OnLeftClick;
            }

            this.scrollRightButton = base.GetTemplateChild("PART_Right") as RepeatButton;
            if (this.scrollRightButton != null)
            {
                this.scrollRightButton.Click += this.OnRightClick;
            }

            this.selectButton = base.GetTemplateChild("PART_Select") as Button;
            if (this.selectButton != null)
            {
                this.selectButton.Click += this.OnSelectClicked;
                this.selectButton.ContextMenu = new ContextMenu();
                this.selectButton.ContextMenu.Style = TryFindResource("SelectionPopupStyle") as Style;
                this.selectButton.ContextMenu.VerticalOffset = -5;
            }

            this.tabScrollViewer = base.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            this.tabScrollViewer.ScrollChanged += this.OnScrollChanged;
            this.tabScrollViewer.MouseDoubleClick += this.OnScrollViewerMouseDoubleClick;

            UpdateControlStatus();
        }

        /// <summary>
        /// Called when [scroll viewer mouse double click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnScrollViewerMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnNewTab();
        }

        /// <summary>
        /// Called when [scroll changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.ScrollChangedEventArgs"/> instance containing the event data.</param>
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateControlStatus();
        }

        /// <summary>
        /// Called when [left click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLeftClick(object sender, RoutedEventArgs e)
        {
            this.tabScrollViewer.LineLeft();
            UpdateControlStatus();
        }

        /// <summary>
        /// Called when [right click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        public void OnRightClick(object sender, RoutedEventArgs e)
        {
            this.tabScrollViewer.LineRight();
            UpdateControlStatus();
        }

        /// <summary>
        /// Called when [new clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnNewClicked(object sender, RoutedEventArgs e)
        {
            OnNewTab();
        }

        /// <summary>
        /// Called when [close clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            OnCloseTab();
        }

        /// <summary>
        /// Called when [select clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnSelectClicked(object sender, RoutedEventArgs e)
        {
            this.selectButton.ContextMenu.Items.Clear();
            foreach (var item in Items.OfType<TabItem>())
            {
                MenuItem mi = new MenuItem();
                mi.Header = item.Header.ToString();
                mi.Tag = item;
                mi.Click += this.OnMenuSelectItemClicked;
                this.selectButton.ContextMenu.Items.Add(mi);
            }

            this.selectButton.ContextMenu.PlacementTarget = this.selectButton;
            this.selectButton.ContextMenu.Placement = PlacementMode.Bottom;
            this.selectButton.ContextMenu.IsOpen = true;

            StackPanel sp = this.selectButton.ContextMenu.Template.FindName("Items", this.selectButton.ContextMenu) as StackPanel;
            if (sp != null)
            {
                Mouse.AddMouseLeaveHandler(sp, (s, mea) => this.selectButton.ContextMenu.IsOpen = false);
            }
        }

        /// <summary>
        /// Called when [menu select item clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void OnMenuSelectItemClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                SelectedItem = mi.Tag;
            }

            UpdateControlStatus();
        }

        /// <summary>
        /// Called when [new tab].
        /// </summary>
        protected void OnNewTab()
        {
            RaiseEvent(new RoutedEventArgs(NewTabEvent, this));
            UpdateControlStatus();
        }

        /// <summary>
        /// Called when [close tab].
        /// </summary>
        protected void OnCloseTab()
        {
            RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
            UpdateControlStatus();
        }
    }
}

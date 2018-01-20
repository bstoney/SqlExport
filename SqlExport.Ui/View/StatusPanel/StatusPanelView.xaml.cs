using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SqlExport.Data;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Ui.Messages;

namespace SqlExport.View.StatusPanel
{
	/// <summary>
	/// Interaction logic for StatusPanel.xaml
	/// </summary>
	public partial class StatusPanelView : UserControl
	{
		////private DatabaseDetails _selectedConnection;

		////public event EventHandler ConnectionChanged;

		public StatusPanelView()
		{
			InitializeComponent();

			////btnConnections.Click += new RoutedEventHandler( OnConnectionsClicked );
			////btnConnections.ContextMenu = new ContextMenu();
			////btnConnections.ContextMenu.Style = TryFindResource( "SelectionPopupStyle" ) as Style;
			////btnConnections.ContextMenu.PlacementTarget = btnConnections;
			////btnConnections.ContextMenu.Placement = PlacementMode.Top;
			////btnConnections.ContextMenu.VerticalOffset = 5;
			////btnConnections.ContextMenu.HorizontalOffset = -10;

			////ReloadOptions();
		}

		////void OnContextMenuLostFocus( object sender, RoutedEventArgs e )
		////{
		////    if( !btnConnections.ContextMenu.IsMouseOver )
		////    {
		////        btnConnections.ContextMenu.IsOpen = false;
		////    }
		////}

		////public void ReloadOptions()
		////{
		////    btnConnections.ContextMenu.Items.Clear();

		////    MenuItem mi = new MenuItem();
		////    mi.Foreground = SpecialConnectionBrush;
		////    mi.Header = "<No Database>";
		////    mi.Click += new RoutedEventHandler( OnConnectionSelectionChanged );
		////    mi.GotFocus += new RoutedEventHandler( MenuItemGotFocus );
		////    mi.LostFocus += new RoutedEventHandler( MenuItemLostFocus );
		////    btnConnections.ContextMenu.Items.Add( mi );

		////    mi = new MenuItem();
		////    mi.Foreground = SpecialConnectionBrush;
		////    mi.Header = "<Manage Connections>";
		////    mi.Click += new RoutedEventHandler( OnManageConnectionsClick );
		////    mi.GotFocus += new RoutedEventHandler( MenuItemGotFocus );
		////    mi.LostFocus += new RoutedEventHandler( MenuItemLostFocus );
		////    btnConnections.ContextMenu.Items.Add( mi );

		////    for( int i = 0; i < Options.Instance.DatabaseCount; i++ )
		////    {
		////        mi = new MenuItem();
		////        mi.Tag = Options.Instance.GetDatabase( i );
		////        mi.Header = mi.Tag.ToString();
		////        mi.Click += new RoutedEventHandler( OnConnectionSelectionChanged );
		////        btnConnections.ContextMenu.Items.Add( mi );
		////    }
		////}

		////private void MenuItemLostFocus( object sender, RoutedEventArgs e )
		////{
		////    MenuItem mi = sender as MenuItem;
		////    mi.Foreground = SpecialConnectionBrush;
		////}

		////private void MenuItemGotFocus( object sender, RoutedEventArgs e )
		////{
		////    MenuItem mi = sender as MenuItem;
		////    mi.Foreground = SystemColors.HighlightTextBrush;
		////}

		////public void ManageConnections()
		////{
		////    ConnectionDialog cd = new ConnectionDialog();
		////    bool result = cd.ShowDialog().Value;
		////    if( result )
		////    {
		////        ////    SetDatabase( cd.GetSelectedDatabase() );
		////    }
		////}

		////private void OnConnectionsClicked( object sender, RoutedEventArgs e )
		////{
		////    btnConnections.ContextMenu.IsOpen = true;

		////    StackPanel sp = btnConnections.ContextMenu.Template.FindName( "Items", btnConnections.ContextMenu ) as StackPanel;
		////    if( sp != null )
		////    {
		////        Mouse.AddMouseLeaveHandler( sp, ( s, mea ) => btnConnections.ContextMenu.IsOpen = false );
		////    }
		////}

		////private void OnConnectionSelectionChanged( object sender, RoutedEventArgs e )
		////{
		////    MenuItem mi = sender as MenuItem;
		////    SelectedConnection = mi.Tag as DatabaseDetails;

		////    // Delay the ConnectionChanged event until the context menu has closed.
		////    btnConnections.ContextMenu.Closed += new RoutedEventHandler( OnContextMenuClosed );
		////}

		////private void OnContextMenuClosed( object sender, RoutedEventArgs e )
		////{
		////    OnConnectionChanged();
		////    btnConnections.ContextMenu.Closed -= new RoutedEventHandler( OnContextMenuClosed );
		////}

		////private void OnConnectionChanged()
		////{
		////    if( ConnectionChanged != null )
		////    {
		////        ConnectionChanged( this, EventArgs.Empty );
		////    }
		////}

		////private void OnManageConnectionsClick( object sender, RoutedEventArgs e )
		////{
		////    ManageConnections();
		////}
	}
}

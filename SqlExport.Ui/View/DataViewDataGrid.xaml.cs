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
using SqlExport.Ui.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Ui.Messages;
using SqlExport.Data;
using System.ComponentModel;
using DevZest.Windows.DataVirtualization;

namespace SqlExport.View
{
	/// <summary>
	/// Interaction logic for DataViewDataGrid.xaml
	/// </summary>
	public partial class DataViewDataGrid : UserControl, IDataView
	{
		private IValueConverter _converter;

		public DataViewDataGrid()
		{
			InitializeComponent();

			_converter = new DataValueConverter();

			Messenger.Default.Register<InitialiseDataViewMessage>( this, DataContext, m =>
			{
				CreateColumns( m.Columns );
			} );
		}

		private void CreateColumns( IEnumerable<Column> columns )
		{
			grid.Columns.Clear();

			foreach( var item in columns )
			{
				var column = new DataGridTextColumn()
				{
					Header = item.Name,
					Binding = new Binding( "[" + item.Name + "]" )
					{
						Converter = _converter
					},
					SortMemberPath = item.Name
				};

				grid.Columns.Add( column );
			}
		}
	}
}

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
using SqlExport.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Messages;
using SqlExport.Data;
using System.ComponentModel;

namespace SqlExport.View
{
    using SqlExport.Common.Data;
    using SqlExport.View.Converters;

    /// <summary>
    /// Interaction logic for DataViewListView.xaml
    /// </summary>
    public partial class DataViewListView : UserControl, IDataView
    {
        public DataViewListView()
        {
            InitializeComponent();

            Messenger.Default.Register<InitialiseDataViewMessage>(this, DataContext, m =>
            {
                CreateColumns(m.Columns);
            });
        }

        private void CreateColumns(IEnumerable<ISchemaItem> columns)
        {
            grdView.Columns.Clear();

            foreach (var column in columns)
            {
                var templateFactory = new FrameworkElementFactory(typeof(TextBlock));
                var binding = new Binding("[" + column.Name + "]") //new Binding( "Data.[" + column.Name + "]" )
                {
                    Converter = new DataValueConverter(),
                    Mode = BindingMode.OneWay
                };

                templateFactory.SetBinding(TextBlock.TextProperty, binding);
                ////templateFactory.SetValue( TextBox.StyleProperty, FindResource( "CellTextStyle" ) as Style );
                ////templateFactory.AddHandler( TextBox.MouseDownEvent, new MouseButtonEventHandler( ( o, e ) => ((TextBox)o).SelectAll() ) );

                var viewColumn = new GridViewColumn()
                {
                    Header = column.Name,
                    Width = double.NaN,
                    HeaderTemplate = FindResource("ListViewColumnHeaderTemplate") as DataTemplate,
                    CellTemplate = new DataTemplate() { VisualTree = templateFactory }
                };

                ////GridViewSort.SetPropertyName( viewColumn, column.Name );

                grdView.Columns.Add(viewColumn);
            }
        }
    }
}

﻿using System;
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

    /// <summary>
    /// Interaction logic for DataViewDataGridView.xaml
    /// </summary>
    public partial class DataViewDataGridView : UserControl, IDataView
    {
        private const string DislpayNullValue = "(null)";

        private GetCellValueHandler _cellValueCallback = new GetCellValueHandler((c, r) => null);

        public DataViewDataGridView()
        {
            InitializeComponent();

            grdView.DefaultCellStyle.NullValue = DislpayNullValue;

            grdView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(grid_CellValueNeeded);

            Messenger.Default.Register<InitialiseDataViewMessage>(this, DataContext, m =>
            {
                _cellValueCallback = m.CellValueCallback;
                CreateColumns(m.Columns);
                grdView.RowCount = m.RowCount;
            });
        }

        private void grid_CellValueNeeded(object sender, System.Windows.Forms.DataGridViewCellValueEventArgs e)
        {
            e.Value = _cellValueCallback(e.ColumnIndex, e.RowIndex);
        }

        private void CreateColumns(IEnumerable<ISchemaItem> columns)
        {
            grdView.Columns.Clear();

            foreach (var column in columns)
            {
                var viewColumn = new System.Windows.Forms.DataGridViewColumn()
                {
                    CellTemplate = new CustomDataGridViewTextBoxCell(),
                    HeaderText = column.Name,
                    DataPropertyName = column.Name
                };

                grdView.Columns.Add(viewColumn);
            }
        }

        private class CustomDataGridViewTextBoxCell : System.Windows.Forms.DataGridViewTextBoxCell
        {
            protected override object GetFormattedValue(object value, int rowIndex, ref System.Windows.Forms.DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, System.Windows.Forms.DataGridViewDataErrorContexts context)
            {
                if (value == cellStyle.DataSourceNullValue && context.HasFlag(System.Windows.Forms.DataGridViewDataErrorContexts.ClipboardContent))
                {
                    return string.Empty;
                }

                return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
            }
        }
    }
}

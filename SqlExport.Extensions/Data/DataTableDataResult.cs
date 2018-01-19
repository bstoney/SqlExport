using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace SqlExport.Data
{
    using SqlExport.Common.Data;

    internal class DataTableDataResult : IDataResult
    {
        private DataTable _table;

        public DataTableDataResult(DataTable table)
        {
            _table = table;
        }

        public string Name
        {
            get { return this._table.TableName; }
        }

        public IEnumerable AsEnumerable()
        {
            return _table.AsEnumerable();
        }

        public int FetchCount()
        {
            return _table.Rows.Count;
        }

        public IEnumerable<ISchemaItem> FetchColumns()
        {
            return _table.Columns.OfType<DataColumn>()
                .Select(c => new Column(c.ColumnName, c.ColumnName, c.DataType.Name, c.DataType, c.AllowDBNull));
        }

        public object FetchValue(object row, string column)
        {
            return ((DataRow)row)[column];
        }

        public object FetchValue(int columnIndex, int rowIndex)
        {
            return _table.Rows[rowIndex][columnIndex];
        }
    }
}

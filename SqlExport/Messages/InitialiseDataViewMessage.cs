using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;

namespace SqlExport.Messages
{
    using SqlExport.Common.Data;

    internal class InitialiseDataViewMessage
    {
        public IEnumerable<ISchemaItem> Columns { get; set; }

        public int RowCount { get; set; }

        public GetCellValueHandler CellValueCallback { get; set; }
    }
}

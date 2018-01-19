using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Data;

namespace SqlExport.Ui
{
    using SqlExport.Common;
    using SqlExport.Common.Data;

    internal class ObjectNodeData
    {

        public ObjectNodeData()
        {
            Database = new DatabaseDetails();
        }

        public ObjectNodeData(int defaultIndex, DatabaseDetails dbdatabase, SchemaItem item, string displayText, string sqlText)
        {
            DefaultIndex = defaultIndex;
            Database = dbdatabase;
            Item = item;
            DisplayText = displayText;
            SqlText = sqlText;
        }

        public int DefaultIndex { get; set; }

        public DatabaseDetails Database { get; set; }

        public SchemaItem Item { get; set; }

        public string DisplayText { get; set; }

        public string SqlText { get; set; }

        public override string ToString()
        {
            return SqlText;
        }
    }
}

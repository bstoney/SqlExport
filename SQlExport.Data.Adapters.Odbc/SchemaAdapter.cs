using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace SqlExport.Data.Adapters.Odbc
{
    using SqlExport.Common.Data;

    internal class SchemaAdapter : IRelationalSchemaAdapter
    {
        private CommandAdapter _commandAdapter;

        public SchemaAdapter(CommandAdapter commandAdapter)
        {
            _commandAdapter = commandAdapter;
        }

        #region IRelationalSchemaAdapter Members

        public ISchemaItem[] GetTables()
        {
            List<string> tables = new List<string>();
            DataTable dt = ((OdbcConnection)_commandAdapter.Connection).GetSchema(OdbcMetaDataCollectionNames.Columns);
            foreach (DataRow r in dt.Rows)
            {
                if (!tables.Contains(r["TABLE_NAME"].ToString()))
                {
                    tables.Add(r["TABLE_NAME"].ToString());
                }
            }
            return tables.ConvertAll<SchemaItem>(new Converter<string, SchemaItem>(
                delegate(string t) { return new SchemaItem(t, SchemaItemType.Table); })).ToArray();
        }

        public ISchemaItem[] GetColumns(string tableName)
        {
            List<Column> columns = new List<Column>();
            DataTable dt = ((OdbcConnection)_commandAdapter.Connection).GetSchema(
                OdbcMetaDataCollectionNames.Columns, new string[] { null, null, tableName });
            foreach (DataRow r in dt.Rows)
            {
                string typeName = ((string)r["TYPE_NAME"]).ToUpper();
                if (typeName.EndsWith("CHAR"))
                {
                    typeName = string.Format("{0}({1})", typeName, Convert.ToInt32(r["COLUMN_SIZE"]));
                }
                //else if( typeName == "DECIMAL" )
                //{
                //    typeName = String.Format( "{0}({1},{2})", typeName,
                //        Convert.ToInt32( r["PRECISION"] ), Convert.ToInt32( r["LENGTH"] ) );
                //}
                columns.Add(new Column((string)r["COLUMN_NAME"], typeName, Convert.ToBoolean(r["NULLABLE"])));
            }
            return columns.ToArray();
        }

        public Type GetColumnType(ISchemaItem schemaItem)
        {
            var column = schemaItem as Column;
            string type = column.Type;
            if (type.Contains("("))
            {
                type = type.Substring(0, type.IndexOf('('));
            }
            switch (type)
            {
                case "CHAR":
                case "LONGCHAR":
                    return typeof(string);
                case "FLOAT":
                    return typeof(float);
                case "INTEGER":
                    return typeof(int);
                case "DATETIME":
                    return typeof(DateTime);
                default:
                    throw new Exception(string.Concat("Unsuported type ", column.Type));
            }
        }

        #endregion

        #region ISchemaAdapter Members

        public string[] GetSections()
        {
            return new string[] { "Tables" };
        }

        public ISchemaItem[] PopulateFromPath(string[] path)
        {
            if (path.Length == 1 && path[0] == "Tables")
            {
                return GetTables();
            }
            else if (path.Length == 2)
            {
                return GetColumns(path[1]);
            }
            return null;
        }

        public string GetSchemaItemScript(string[] path)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_commandAdapter != null)
            {
                _commandAdapter.Dispose();
            }
        }

        #endregion
    }
}

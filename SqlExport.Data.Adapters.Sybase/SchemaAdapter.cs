using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;

namespace SqlExport.Data.Adapters.Sybase
{
    using SqlExport.Common.Data;

    internal class SchemaAdapter : IRelationalSchemaAdapter
    {
        private CommandAdapter _commandAdapter;

        public SchemaAdapter(CommandAdapter commandAdapter)
        {
            _commandAdapter = commandAdapter;
        }

        private Column[] GetTableColumns(string tableName)
        {
            List<Column> columns = new List<Column>();
            string sql = String.Concat(@"SELECT syscolumn.column_name, domain_name, width, scale, nulls
FROM syscolumn
INNER JOIN sysdomain ON sysdomain.domain_id = syscolumn.domain_id
INNER JOIN sysobjects ON RIGHT(sysobjects.id, 5) = syscolumn.table_id
WHERE sysobjects.name = ?
ORDER BY column_id");
            using (OdbcCommand cmd = _commandAdapter.CreateCommand(sql))
            {
                cmd.Parameters.Add(new OdbcParameter("@name", tableName));
                using (OdbcDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string typeName = ((string)reader["domain_name"]).ToUpper();
                        if (typeName.EndsWith("CHAR"))
                        {
                            typeName = String.Format("{0}({1})", typeName, Convert.ToInt32(reader["width"]));
                        }
                        else if (typeName == "DECIMAL")
                        {
                            typeName = String.Format("{0}({1},{2})", typeName, Convert.ToInt32(reader["scale"]),
                                Convert.ToInt32(reader["width"]));
                        }
                        columns.Add(new Column((string)reader["column_name"], typeName, ((string)reader["nulls"] == "Y")));
                    }
                }
            }
            return columns.ToArray();
        }

        private Column[] GetProcedureColumns(string procedureName)
        {
            List<Column> columns = new List<Column>();
            string sql = String.Concat(@"SELECT parm_name, domain_name, width, scale
FROM sysprocparm
INNER JOIN sysdomain ON sysdomain.domain_id = sysprocparm.domain_id
INNER JOIN sysprocedure ON sysprocedure.proc_id = sysprocparm.proc_id
WHERE proc_name = ?
ORDER BY parm_id");
            using (OdbcCommand cmd = _commandAdapter.CreateCommand(sql))
            {
                cmd.Parameters.Add(new OdbcParameter("@name", procedureName));
                using (OdbcDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string typeName = ((string)reader["domain_name"]).ToUpper();
                        if (typeName.EndsWith("CHAR"))
                        {
                            typeName = String.Format("{0}({1})", typeName, Convert.ToInt32(reader["width"]));
                        }
                        else if (typeName == "DECIMAL")
                        {
                            typeName = String.Format("{0}({1},{2})", typeName, Convert.ToInt32(reader["scale"]),
                                Convert.ToInt32(reader["width"]));
                        }
                        columns.Add(new Column((string)reader["parm_name"], typeName, null));
                    }
                }
            }
            return columns.ToArray();
        }

        public SchemaItem[] GetObjects(string type, SchemaItemType schemaType)
        {
            List<SchemaItem> tables = new List<SchemaItem>();
            using (OdbcCommand cmd = _commandAdapter.CreateCommand(@"SELECT name 
FROM sysobjects
WHERE type = ?
ORDER BY name"))
            {
                cmd.Parameters.Add(new OdbcParameter("@type", type));
                using (OdbcDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(new SchemaItem((string)reader["name"], schemaType));
                    }
                }
            }
            return tables.ToArray();
        }

        public SchemaItem[] GetCodeObjects(string type)
        {
            List<SchemaItem> tables = new List<SchemaItem>();
            string sql = null;
            switch (type)
            {
                case "P":
                    sql = @"SELECT proc_name AS name
FROM sysprocedure
WHERE proc_defn LIKE 'create procedure%'
ORDER BY proc_name";
                    break;
                case "F":
                    sql = @"SELECT proc_name AS name
FROM sysprocedure
WHERE proc_defn LIKE 'create function%'
ORDER BY proc_name";
                    break;
            }
            using (OdbcCommand cmd = _commandAdapter.CreateCommand(sql))
            {
                cmd.Parameters.Add(new OdbcParameter("@type", type));
                using (OdbcDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(new SchemaItem((string)reader["name"],
                            (type == "P" ? SchemaItemType.Procedure : SchemaItemType.Function)));
                    }
                }
            }
            return tables.ToArray();
        }

        #region IRelationalSchemaAdapter Members

        public ISchemaItem[] GetTables()
        {
            return GetObjects("U", SchemaItemType.Table);
        }

        public ISchemaItem[] GetColumns(string tableName)
        {
            return GetTableColumns(tableName);
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
                case "INTEGER":
                    return typeof(int);
                case "SMALLINT":
                    return typeof(short);
                case "TINYINT":
                    return typeof(byte);
                case "BIT":
                    return typeof(bool);
                case "DECIMAL":
                case "NUMERIC":
                    return typeof(decimal);
                case "FLOAT":
                    return typeof(float);
                case "DOUBLE":
                    return typeof(double);
                case "DATE":
                case "TIME":
                case "DATETIME":
                    return typeof(DateTime);
                case "CHAR":
                case "VARCHAR":
                case "LONG VARCHAR":
                    return typeof(string);
                case "BINARY":
                case "LONG BINARY":
                case "TIMESTAMP":
                    return typeof(object);
                default:
                    throw new Exception(string.Format("Unsupported type '{0}'.", column.Type));
            }
        }

        #endregion

        #region ISchemaAdapter Members

        public string[] GetSections()
        {
            return new string[] { "User Tables", "System Tables", "Views", "Procedures", "Functions" };
        }

        public ISchemaItem[] PopulateFromPath(string[] path)
        {
            if (path.Length == 1)
            {
                switch (path[0])
                {
                    case "User Tables":
                        return GetTables();
                    case "System Tables":
                        return GetObjects("S", SchemaItemType.Table);
                    case "Views":
                        return GetObjects("V", SchemaItemType.View);
                    case "Procedures":
                        return GetCodeObjects("P");
                    case "Functions":
                        return GetCodeObjects("F");
                    default:
                        break;
                }
            }
            else if (path.Length == 2)
            {
                switch (path[0])
                {
                    case "User Tables":
                    case "System Tables":
                    case "Views":
                        return GetTableColumns(path[1]);
                    case "Procedures":
                    case "Functions":
                        return GetProcedureColumns(path[1]);
                }
            }
            return new SchemaItem[] { };
        }

        public string GetSchemaItemScript(string[] path)
        {
            if (path.Length == 2 && (path[0] == "Procedures" || path[0] == "Functions"))
            {
                using (OdbcCommand cmd = _commandAdapter.CreateCommand(@"SELECT proc_defn
FROM sysprocedure
WHERE proc_name = ?"))
                {
                    cmd.Parameters.Add(new OdbcParameter("@name", path[1]));
                    return cmd.ExecuteScalar() as string;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
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

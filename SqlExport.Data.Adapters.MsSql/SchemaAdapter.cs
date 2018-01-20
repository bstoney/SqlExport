using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;

#if USE_SQLDMO
using SQLDMO;
#endif

namespace SqlExport.Data.Adapters.MsSql
{
    using SqlExport.Common.Data;

    internal class SchemaAdapter : IRelationalSchemaAdapter
    {
        private readonly static string[] SystemDatabases = new[] { "master", "tempdb", "model", "msdb" };
        private const int DatabaseFolderPath = 0;
        private const int DatabaseNamePath = 1;
        private const int ObjectFolderPath = 2;
        private const int ObjectNamePath = 3;
        private const int ItemNamePath = 4;

        private CommandAdapter commandAdapter;

        public SchemaAdapter(CommandAdapter commandAdapter)
        {
            this.commandAdapter = commandAdapter;
        }

        private ISchemaItem[] GetDatabases(bool systemDatabases)
        {
            List<SchemaItem> databases = new List<SchemaItem>();
            string sql = @"USE master
SELECT name FROM sysdatabases
ORDER BY name";
            using (SqlCommand cmd = this.commandAdapter.CreateCommand(sql))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["name"];
                        bool isSystemDatabase = SystemDatabases.Contains(name);
                        if ((systemDatabases && isSystemDatabase) || (!systemDatabases && !isSystemDatabase))
                        {
                            databases.Add(new SchemaItem(name, name, GetQuerySafeName(name), SchemaItemType.Database));
                        }
                    }
                }
            }

            return databases.ToArray();
        }

        private SchemaItem[] GetObjects(string type, SchemaItemType itemType)
        {
            return GetObjects(type, itemType, null);
        }

        private SchemaItem[] GetObjects(string type, SchemaItemType itemType, string database)
        {
            List<SchemaItem> objects = new List<SchemaItem>();
            string sql = @"SELECT name FROM sysobjects
		WHERE type = @type
		ORDER BY name";
            using (SqlCommand cmd = this.commandAdapter.CreateCommand(sql))
            {
                cmd.Parameters.Add(new SqlParameter("@type", type));
                if (!string.IsNullOrEmpty(database))
                {
                    cmd.Connection.ChangeDatabase(database);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objects.Add(new SchemaItem((string)reader["name"], (string)reader["name"],
                            GetQuerySafeName((string)reader["name"]), itemType));
                    }
                }
            }

            return objects.ToArray();
        }

        private Column[] GetObjectColumns(string name, string database)
        {
            List<Column> columns = new List<Column>();
            var columnSql = @"SELECT syscolumns.name AS COLUMN_NAME, 
	basetype.name AS TYPE_NAME, 
	syscolumns.length, 
	syscolumns.prec, 
	syscolumns.scale,
	isnullable
FROM sysobjects
INNER JOIN syscolumns ON syscolumns.id = sysobjects.id
INNER JOIN systypes ON systypes.xtype = syscolumns.xtype
	AND systypes.xusertype = syscolumns.xusertype
LEFT OUTER JOIN systypes basetype ON basetype.xtype = systypes.xtype
	AND basetype.xtype = basetype.xusertype
WHERE sysobjects.name = @name
ORDER BY colorder";
            using (SqlCommand cmd = this.commandAdapter.CreateCommand(columnSql))
            {
                cmd.Parameters.Add(new SqlParameter("@name", name));
                if (!string.IsNullOrEmpty(database))
                {
                    cmd.Connection.ChangeDatabase(database);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string typeName = ((string)reader["TYPE_NAME"]).ToUpper();
                        if (typeName.EndsWith("CHAR"))
                        {
                            typeName = string.Format("{0}({1})", typeName, Convert.ToInt32(reader["LENGTH"]));
                        }
                        else if (typeName == "DECIMAL")
                        {
                            typeName = string.Format(
                                "{0}({1},{2})",
                                typeName,
                                Convert.ToInt32(reader["prec"]),
                                Convert.ToInt32(reader["scale"]));
                        }

                        columns.Add(
                            new Column(
                                (string)reader["COLUMN_NAME"],
                                string.Concat(this.GetQuerySafeName(name), ".", this.GetQuerySafeName((string)reader["COLUMN_NAME"])),
                                typeName,
                                Convert.ToBoolean(reader["isnullable"])));
                    }
                }
            }

            return columns.ToArray();
        }

        /// <summary>
        /// Gets the query safe name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The safe name.</returns>
        private string GetQuerySafeName(string name)
        {
            if (Regex.IsMatch(name, @"(?:^[0-9])|[^\w]") ||
                Array.IndexOf<string>(SyntaxDefinition.MsSqlKeywords, name) >= 0 ||
                Array.IndexOf<string>(SyntaxDefinition.MsSqlFunctions, name) >= 0)
            {
                name = string.Concat("[", name, "]");
            }

            return name;
        }

        #region IRelationalSchemaAdapter Members

        public ISchemaItem[] GetTables()
        {
            return GetObjects("U", SchemaItemType.Table);
        }

        public ISchemaItem[] GetColumns(string tableName)
        {
            return GetObjectColumns(tableName, null);
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
                case "BIGINT":
                    return typeof(long);
                case "INT":
                    return typeof(int);
                case "SMALLINT":
                    return typeof(short);
                case "TINYINT":
                    return typeof(byte);
                case "BIT":
                    return typeof(bool);
                case "DECIMAL":
                case "NUMERIC":
                case "MONEY":
                case "SMALLMONEY":
                    return typeof(decimal);
                case "FLOAT":
                    return typeof(float);
                case "REAL":
                    return typeof(double);
                case "DATETIME":
                case "SMALLDATETIME":
                    return typeof(DateTime);
                case "CHAR":
                case "VARCHAR":
                case "TEXT":
                case "NCHAR":
                case "NVARCHAR":
                case "NTEXT":
                    return typeof(string);
                case "BINARY":
                case "VARBINARY":
                case "IMAGE":
                case "TIMESTAMP":
                    return typeof(object);
                case "UNIQUEIDENTIFIER":
                    return typeof(Guid);
                default:
                    throw new Exception(string.Format("Unsupported type '{0}'.", column.Type));
            }
        }

        #endregion

        #region ISchemaAdapter Members

        public string[] GetSections()
        {
            return new string[] { "Databases", "System Databases" };
        }

        public ISchemaItem[] PopulateFromPath(string[] path)
        {
            if (path.Length == DatabaseFolderPath + 1)
            {
                switch (path[DatabaseFolderPath])
                {
                    case "Databases":
                        return GetDatabases(false);
                    case "System Databases":
                        return GetDatabases(true);
                }
            }
            else if (path.Length == DatabaseNamePath + 1)
            {
                return new SchemaItem[] { new SchemaItem( "User Tables", SchemaItemType.Folder ), 
					new SchemaItem( "System Tables", SchemaItemType.Folder ), 
					new SchemaItem( "User Views", SchemaItemType.Folder ), 
					new SchemaItem( "Procedures", SchemaItemType.Folder ), 
					new SchemaItem( "Functions", SchemaItemType.Folder ) };
            }
            else if (path.Length == ObjectFolderPath + 1)
            {
                switch (path[ObjectFolderPath])
                {
                    case "User Tables":
                        return GetObjects("U", SchemaItemType.Table, path[DatabaseNamePath]);
                    case "System Tables":
                        return GetObjects("S", SchemaItemType.Table, path[DatabaseNamePath]);
                    case "User Views":
                        return GetObjects("V", SchemaItemType.View, path[DatabaseNamePath]);
                    case "Procedures":
                        return GetObjects("P", SchemaItemType.Procedure, path[DatabaseNamePath]);
                    case "Functions":
                        return GetObjects("FN", SchemaItemType.Function, path[DatabaseNamePath]);
                }
            }
            else if (path.Length == ObjectNamePath + 1)
            {
                return GetObjectColumns(path[ObjectNamePath], path[DatabaseNamePath]);
            }
            return new SchemaItem[] { };
        }

        public string GetSchemaItemScript(string[] path)
        {
            if (path.Length == ObjectNamePath + 1 && path[ObjectFolderPath] != "Functions")
            {
#if USE_SQLDMO
				SqlConnection cnn = _commandAdapter.Connection as SqlConnection;
				SQLServer server = new SQLServerClass();
				server.LoginSecure = true;
				server.Connect( cnn.DataSource, null, null );

				_Database database = server.Databases.Item( path[DatabaseNamePath], null );

				string script = null;
				switch( path[ObjectFolderPath] )
				{
					case "User Tables":
					case "System Tables":
						script = database.Tables.Item( path[ObjectNamePath], null ).Script( SQLDMO_SCRIPT_TYPE.SQLDMOScript_Default | SQLDMO_SCRIPT_TYPE.SQLDMOScript_Drops, null, null, SQLDMO_SCRIPT2_TYPE.SQLDMOScript2_Default );
						break;
					case "User Views":
						script = database.Views.Item( path[ObjectNamePath], null ).Script( SQLDMO_SCRIPT_TYPE.SQLDMOScript_Default | SQLDMO_SCRIPT_TYPE.SQLDMOScript_Drops, null, SQLDMO_SCRIPT2_TYPE.SQLDMOScript2_Default );
						break;
					case "Procedures":
						script = database.StoredProcedures.Item( path[ObjectNamePath], null ).Script( SQLDMO_SCRIPT_TYPE.SQLDMOScript_Default | SQLDMO_SCRIPT_TYPE.SQLDMOScript_Drops, null, SQLDMO_SCRIPT2_TYPE.SQLDMOScript2_Default );
						break;
				}

				return script;
#else
                using (SqlCommand cmd = this.commandAdapter.CreateCommand(@"
						DECLARE @command VARCHAR(255)
						DECLARE @script VARCHAR(4096)
						DECLARE @object INT
						DECLARE @hr INT
						
						SET NOCOUNT ON
						
						EXEC @hr = sp_OACreate 'SQLDMO.SQLServer', @object OUT
						EXEC @hr = sp_OASetProperty @object, 'LoginSecure', TRUE
						
						SET @command = 'Connect(' + @serverName + ')'
						EXEC @hr = sp_OAMethod @object, @command
						
						SET @command = CASE @objectType
							WHEN 'Job' THEN 
								'Jobserver.Jobs(""' + @ObjectName + '"").Script(5)'
							WHEN 'Database' THEN  
								'Databases(""' + @ObjectName + '"").Script(5)'
							WHEN 'Procedure' THEN 
								'Databases(""' + @databaseName + '"").StoredProcedures(""' + @ObjectName + '"").Script(5)'
							WHEN 'View' THEN 
								'Databases(""' + @databaseName + '"").Views(""' + @ObjectName + '"").Script(5)'
							WHEN 'Table' THEN 
								'Databases(""' + @databaseName + '"").Tables(""' + @ObjectName + '"").Script(5)'
							WHEN 'Index' THEN 
								'Databases(""' + @databaseName + '"").Tables(""' + @TableName + '"").Indexes(""' + @ObjectName + '"").Script(5)'
							WHEN 'Trigger' THEN 
								'Databases(""' + @databaseName + '"").Tables(""' + @TableName + '"").Triggers(""' + @ObjectName + '"").Script(5)'
							WHEN 'Key' THEN 
								'Databases(""' + @databaseName + '"").Tables(""' + @TableName + '"").Keys(""' + @ObjectName + '"").Script(5)'
							WHEN 'Check' THEN 
								'Databases(""' + @databaseName + '"").Tables(""' + @TableName + '"").Checks(""' + @ObjectName + '"").Script(5)'
							END
						EXEC @hr = sp_OAMethod @object, @command, @script OUT
						
						EXEC @hr = sp_OADestroy @object
						
						SELECT @script
						"))
                {
                    SqlConnection cnn = this.commandAdapter.Connection as SqlConnection;
                    cmd.Parameters.Add(new SqlParameter("@serverName", cnn.DataSource));
                    cmd.Parameters.Add(new SqlParameter("@databaseName", cnn.Database));
                    cmd.Parameters.Add(new SqlParameter("@objectName", path[ObjectNamePath]));
                    switch (path[ObjectFolderPath])
                    {
                        case "User Tables":
                        case "System Tables":
                            cmd.Parameters.Add(new SqlParameter("@objectType", "Table"));
                            break;
                        case "User Views":
                            cmd.Parameters.Add(new SqlParameter("@objectType", "View"));
                            break;
                        case "Procedures":
                            cmd.Parameters.Add(new SqlParameter("@objectType", "Procedure"));
                            break;
                    }

                    cmd.Parameters.Add(new SqlParameter("@tableName", String.Empty));
                    return cmd.ExecuteScalar() as string;
                }
#endif
            }

            return null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.commandAdapter != null)
            {
                this.commandAdapter.Dispose();
            }
        }

        #endregion

    }
}

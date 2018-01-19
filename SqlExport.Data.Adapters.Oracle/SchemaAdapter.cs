namespace SqlExport.Data.Adapters.Oracle
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;
    using System.Linq;
    using System.Text;

    using SqlExport.Common.Data;

    /// <summary>
    /// Defines the SchemaAdapter class.
    /// </summary>
    internal class SchemaAdapter : ISchemaAdapter
    {
        /// <summary>
        /// The system owners
        /// </summary>
        private const string SystemOwnersInClause = "'SYS', 'SYSTEM', 'OUTLN', 'DIP', 'TSMSYS', 'DBSNMP', 'WMSYS', 'EXFSYS', 'DMSYS', 'CTXSYS', 'XDB', 'ORDSYS', 'ORDPLUGINS', 'SI_INFORMTN_SCHEMA', 'MDSYS', 'OLAPSYS'";

        /// <summary>
        /// The command adapter
        /// </summary>
        private readonly CommandAdapter commandAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaAdapter"/> class.
        /// </summary>
        /// <param name="commandAdapter">The command adapter.</param>
        public SchemaAdapter(CommandAdapter commandAdapter)
        {
            this.commandAdapter = commandAdapter;
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>
        /// A list of sections.
        /// </returns>
        public string[] GetSections()
        {
            return new[] { Sections.Tables, Sections.Views, Sections.Procedures };
        }

        /// <summary>
        /// Populates from path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A list of schema items.
        /// </returns>
        public ISchemaItem[] PopulateFromPath(string[] path)
        {
            if (path.Length == 1)
            {
                switch (path[0])
                {
                    case Sections.Tables:
                    case Sections.Views:
                        return this.GetObjects(path[0], SchemaItemType.Table).ToArray();
                    case Sections.Procedures:
                        return this.GetObjects(Sections.Procedures, SchemaItemType.Procedure).ToArray();
                }
            }
            else if (path.Length == 2)
            {
                switch (path[0])
                {
                    case Sections.Tables:
                        return this.GetObjects(path[0], SchemaItemType.Table, path[1]).ToArray();
                    case Sections.Procedures:
                        return this.GetObjects(path[0], SchemaItemType.Procedure, path[1]).ToArray();
                }
            }
            else if (path.Length == 3)
            {
                return this.GetColumns(path[1], path[2]).ToArray<ISchemaItem>();
            }

            return new ISchemaItem[] { };
        }

        /// <summary>
        /// Gets the schema item script.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A string.
        /// </returns>
        public string GetSchemaItemScript(string[] path)
        {
            if (path.Length == 3)
            {
                string commandText = null;
                switch (path[0])
                {
                    case Sections.Tables:
                        commandText = @"SELECT TEXT FROM ALL_TABLES WHERE OWNER = :owner AND TABLE_NAME = :name";
                        break;
                    case Sections.Views:
                        commandText = @"SELECT TEXT FROM ALL_VIEWS WHERE OWNER = :owner AND VIEW_NAME = :name";
                        break;
                    case Sections.Procedures:
                        commandText = @"SELECT TEXT FROM ALL_SOURCE WHERE TYPE = 'PROCEDURE' AND OWNER = :owner AND NAME = :name ORDER BY LINE";
                        break;
                }

                using (var cmd = this.commandAdapter.CreateCommand(commandText))
                {
                    cmd.Parameters.Add(new OracleParameter(":owner", path[1]));
                    cmd.Parameters.Add(new OracleParameter(":name", path[2]));

                    var source = new StringBuilder();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            source.AppendLine(reader["TEXT"] as string);
                        }
                    }

                    return source.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.commandAdapter != null)
            {
                this.commandAdapter.Dispose();
            }
        }

        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="dataLength">Length of the data.</param>
        /// <param name="dataPrecision">The data precision.</param>
        /// <returns>A string.</returns>
        private static string GetTypeName(string dataType, object dataLength, object dataPrecision)
        {
            string typeName = dataType.ToUpper();
            if (typeName.Contains("CHAR"))
            {
                typeName = string.Format("{0}({1})", typeName, (decimal)dataLength);
            }
            else if (!(dataPrecision is DBNull))
            {
                typeName = string.Format("{0}({1},{2})", typeName, (decimal)dataPrecision, (decimal)dataLength);
            }

            return typeName;
        }

        /// <summary>
        /// Gets the schema item.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="createSchemaItem">The create schema item.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A list of schema items.</returns>
        private IEnumerable<ISchemaItem> GetSchemaItem(string sql, Func<IDataReader, SchemaItem> createSchemaItem, params OracleParameter[] parameters)
        {
            using (var cmd = this.commandAdapter.CreateCommand(sql))
            {
                cmd.Parameters.AddRange(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return createSchemaItem(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the objects.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="schemaItemType">Type of the schema item.</param>
        /// <param name="owner">The owner.</param>
        /// <returns>
        /// A list of schema items.
        /// </returns>
        private IEnumerable<ISchemaItem> GetObjects(string section, SchemaItemType schemaItemType, string owner = null)
        {
            var commandText = new StringBuilder();
            switch (section)
            {
                case Sections.Tables:
                    commandText.AppendLine("SELECT OWNER, TABLE_NAME AS Name FROM ALL_TABLES");
                    break;
                case Sections.Views:
                    commandText.AppendLine("SELECT OWNER, VIEW_NAME AS Name FROM ALL_VIEWS");
                    break;
                case Sections.Procedures:
                    commandText.AppendLine("SELECT OWNER, OBJECT_NAME AS Name FROM ALL_PROCEDURES");
                    break;
            }

            if (owner != null)
            {
                commandText.AppendLine("    AND OWNER = :owner");
            }

            commandText.AppendLine("ORDER BY OWNER, Name");

            Func<IDataReader, SchemaItem> rowConverter =
                r => new SchemaItem(string.Format("{0}.{1}", r["OWNER"], r["Name"]), (string)r["Name"], schemaItemType);
            if (owner != null)
            {
                return this.GetSchemaItem(commandText.ToString(), rowConverter, new OracleParameter(":owner", owner));
            }

            var schemaItems = this.GetSchemaItem(commandText.ToString(), rowConverter).ToList();

            var owners = from s in schemaItems
                         group s by s.Name.Substring(0, s.Name.IndexOf('.')) into o
                         select new SchemaItem(o.Key, o.Key, SchemaItemType.Folder) { Children = o };

            return owners;
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>A list of columns.</returns>
        private IEnumerable<Column> GetColumns(string owner, string tableName)
        {
            const string CommandText = @"SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH, DATA_PRECISION, NULLABLE 
FROM ALL_TAB_COLUMNS
WHERE OWNER = :owner 
	AND TABLE_NAME = :name
ORDER BY COLUMN_ID";
            return
                this.GetSchemaItem(
                    CommandText,
                    r =>
                    new Column(
                        (string)r["COLUMN_NAME"],
                        GetTypeName((string)r["DATA_TYPE"], r["DATA_LENGTH"], r["DATA_PRECISION"]),
                        (string)r["NULLABLE"] == "Y"),
                    new OracleParameter(":owner", owner),
                    new OracleParameter(":name", tableName)).OfType<Column>();
        }

        /// <summary>
        /// Defines the Sections class.
        /// </summary>
        private static class Sections
        {
            /// <summary>
            /// The tables section
            /// </summary>
            public const string Tables = "Tables";

            /// <summary>
            /// The views section
            /// </summary>
            public const string Views = "Views";

            /// <summary>
            /// The procedures section
            /// </summary>
            public const string Procedures = "Procedures";
        }
    }
}

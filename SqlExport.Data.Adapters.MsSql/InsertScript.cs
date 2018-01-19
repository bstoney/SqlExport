namespace SqlExport.Data.Adapters.MsSql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Common.Extensions;
    using SqlExport.Common.Options;
    using SqlExport.Export;

    /// <summary>
    /// Defines the InsertScript class.
    /// </summary>
    public class InsertScript : ExporterBase, IExtension
    {
        /// <summary>
        /// The SQL max column count
        /// </summary>
        private const int SqlMaxColumnCount = 1024;

        /// <summary>
        /// The SQL max row size
        /// </summary>
        private const int SqlMaxRowSize = 8060;

        /// <summary>
        /// The max index width
        /// </summary>
        private const int MaxIndexWidth = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertScript"/> class.
        /// </summary>
        public InsertScript()
        {
            Configuration.SetOptionsOn(this);
        }

        /// <summary>
        /// Get the name of the adapter.
        /// </summary>
        public override string Name
        {
            get { return "MsSql"; }
        }

        /// <summary>
        /// Gets the dialog filename filter.
        /// </summary>
        public override string FileFilter
        {
            get { return "Sql Files (*.sql)|*.sql|All Files (*.*)|*.*"; }
        }


        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        [SelectionOption(
            "Method",
            new[] { InsertMethod.InsertStatement, InsertMethod.UnionAll },
            DisplayName = "Insert Method",
            DefaultValue = InsertMethod.InsertStatement)]
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use go statement.
        /// </summary>
        [Option("UseGoStatement", DisplayName = "Use Go Statement", DefaultValue = "True")]
        public bool UseGoStatement { get; set; }

        public override void Export(IDataResult data, TextWriter writer)
        {
            Status = "Exporting data as SQL";

            // TODO code for more than SqlMaxColumnCount
            int startColumn = 0;
            int endColumn = 0;
            int rowSize = 0;
            var columns = data.FetchColumns().OfType<Column>().ToList();
            for (int tableIndex = 1; startColumn < columns.Count; tableIndex++)
            {
                for (endColumn = startColumn; endColumn < columns.Count - 1; endColumn++)
                {
                    int columnSize = columns[endColumn].GetSize(data);

                    if (rowSize + columnSize > SqlMaxRowSize ||
                        (endColumn - startColumn >= SqlMaxColumnCount - 2 && columns.Count > SqlMaxColumnCount))
                    {
                        break;
                    }

                    rowSize += columnSize;
                }

                bool spanMultipleTables = tableIndex > 1 || startColumn > 0 || endColumn < columns.Count - 1;

                WriteTableDefinition(data, writer, tableIndex, startColumn, endColumn, spanMultipleTables);

                startColumn = endColumn + 1;
                rowSize = 0;
            }

            Status = "Export complete";
        }

        private void WriteTableDefinition(IDataResult data, TextWriter writer, int tableIndex, int startColumn, int endColumn, bool spanMultipleTables)
        {
            // The data will span multiple tables if there are more than one table or the column range is less than number of columns.
            var columns = data.FetchColumns().ToList();
            var rowCount = data.FetchCount();
            string tableName = (spanMultipleTables ? (data.Name + tableIndex).PadLeft(MaxIndexWidth, '0') : data.Name);

            ColumnCollection columnDefinitions = new ColumnCollection();
            if (spanMultipleTables)
            {
                columnDefinitions.Add("InsertScriptAutoID", new ColumnDefinition()
                {
                    Name = "InsertScriptAutoID",
                    Type = typeof(int),
                    Nullable = false,
                    IsIdentity = true
                });
            }

            string tempFile = Path.GetTempFileName();
            using (StreamWriter tempFileWriter = new StreamWriter(tempFile, false))
            {
                int counter = 0;
                for (int i = 0; i < rowCount; i++)
                {
                    if (i % 10 == 0)
                    {
                        Value += counter;
                    }

                    counter++;
                    using (StringWriter tempLineWriter = new StringWriter())
                    {
                        if (spanMultipleTables)
                        {
                            // Write the row number as the auto generated key.
                            tempLineWriter.Write(i);
                        }

                        for (int j = startColumn; j <= endColumn; j++)
                        {
                            object value = data.FetchValue(j, i);
                            var column = columns[j] as Column;

                            // Ensure the column definition exists.
                            if (!columnDefinitions.ContainsKey(column.Name))
                            {
                                columnDefinitions.Add(column.Name, new ColumnDefinition(column));
                            }

                            ColumnDefinition columnDefinition = columnDefinitions[column.Name];
                            if (j != startColumn || spanMultipleTables)
                            {
                                tempLineWriter.Write(',');
                            }

                            if (!(value is DBNull))
                            {
                                switch (column.UnderlyingType.ToString())
                                {
                                    case "System.String":
                                        columnDefinition.Size = Math.Max(columnDefinition.Size, Encoding.Default.GetByteCount(value.ToString()));
                                        break;
                                    case "System.Decimal":
                                        // TODO get size and precision.
                                        break;
                                    case "System.Int32":
                                        if (j != startColumn || spanMultipleTables)
                                        {
                                            // If this is not the first column don't bother with the identity determining logic.
                                            columnDefinition.IsIdentity = false;
                                        }
                                        else if (!columnDefinition.IsIdentity.HasValue)
                                        {
                                            // If this is an in and the first column try to determine if it has consecutive values and is there fore an identity.
                                            var threeDataPoints = data.AsEnumerable().OfType<object>().Select(r => data.FetchValue(r, column.Name)).Take(3);
                                            bool hasThreePoints = threeDataPoints.Count() == 3;
                                            bool hasNullValues = threeDataPoints.Any(v => v is DBNull);
                                            var threeInts = threeDataPoints.OfType<int>();

                                            // Determine if the three integers are consecutive.
                                            bool hasConsecutiveValues = threeInts.Skip(1).Aggregate<int, int[], bool>(new[] { threeInts.FirstOrDefault(), 0 },
                                                (m, v) => new[] { m[0] + 1, (m[1] == 0 && m[0] + 1 == v) ? 0 : 1 },
                                                m => m[1] == 0);

                                            // The column can be assumed to be an identity if it is the first column, 
                                            // there are at least three data points, none of them are null and they are consecutive
                                            columnDefinition.IsIdentity = hasThreePoints && !hasNullValues && hasConsecutiveValues;
                                        }

                                        break;
                                }

                                switch (column.UnderlyingType.ToString())
                                {
                                    case "System.String":
                                    case "System.Char":
                                        tempLineWriter.Write("'");
                                        tempLineWriter.Write(value.ToString().Replace("\'", "''"));
                                        tempLineWriter.Write("'");
                                        break;
                                    case "System.DateTime":
                                        tempLineWriter.Write("'{0:yyyy-MM-dd HH:mm:ss}'", value);
                                        break;
                                    case "System.Boolean":
                                        tempLineWriter.Write((bool)value ? "1" : "0");
                                        break;
                                    default:
                                        tempLineWriter.Write(value);
                                        break;
                                }
                            }
                            else
                            {
                                // Ensure any column with a null value is not an identity.
                                columnDefinition.IsIdentity = false;
                                tempLineWriter.Write("NULL");
                            }
                        }

                        switch (this.Method)
                        {
                            case InsertMethod.UnionAll:
                                if (i > 0)
                                {
                                    tempFileWriter.WriteLine("UNION ALL");
                                }

                                tempFileWriter.WriteLine("SELECT " + tempLineWriter);

                                break;
                            default:
                                tempFileWriter.Write("INSERT INTO [dbo].[{0}] (", tableName);
                                tempFileWriter.Write(string.Join(",", columnDefinitions.Columns.Select(c => "[" + c.Name + "]").ToArray()));
                                tempFileWriter.Write(") VALUES ( ");
                                tempFileWriter.Write(tempLineWriter.ToString());
                                tempFileWriter.WriteLine(" )");
                                if (this.UseGoStatement)
                                {
                                    tempFileWriter.WriteLine("GO");
                                }

                                break;
                        }
                    }
                }
            }

            writer.Write(@"
/****** Create Object:  Table [dbo].[{0}]    Script Date: {1:dd/MM/yyyy HH:mm:ss} ******/
IF EXISTS( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[{0}]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1 )
    DROP TABLE [dbo].[{0}]
",
                tableName,
                DateTime.Now);

            if (this.UseGoStatement)
            {
                writer.WriteLine("GO");
            }

            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("CREATE TABLE [dbo].[{0}] (", tableName);
            writer.Write("\t");

            writer.Write(string.Join(",\n\t", columnDefinitions.Columns.Select(c => c.ToString()).ToArray()));

            ColumnDefinition identity = columnDefinitions.Columns.FirstOrDefault(c => c.IsIdentity ?? false);
            if (identity != null)
            {
                writer.WriteLine(",");
                writer.WriteLine("\tCONSTRAINT [PK_{0}] PRIMARY KEY  CLUSTERED", tableName);
                writer.WriteLine("\t(");
                writer.WriteLine("\t\t[{0}]", identity.Name);
                writer.WriteLine("\t) ON [PRIMARY]");
                writer.WriteLine(") ON [PRIMARY]");
            }
            else
            {
                writer.WriteLine();
                writer.WriteLine(")");
            }

            if (this.UseGoStatement)
            {
                writer.WriteLine("GO");
            }

            writer.WriteLine();

            if (identity != null)
            {
                writer.WriteLine("SET IDENTITY_INSERT [{0}] ON", tableName);
                writer.WriteLine();
            }

            if (this.Method == InsertMethod.UnionAll)
            {
                writer.WriteLine("INSERT INTO [{0}]", tableName);
            }

            writer.Write(File.ReadAllText(tempFile));
            File.Delete(tempFile);

            if (identity != null)
            {
                writer.WriteLine();
                writer.WriteLine("SET IDENTITY_INSERT [{0}] OFF", tableName);
            }

            if (this.UseGoStatement)
            {
                writer.WriteLine();
                writer.WriteLine("GO");
            }

            writer.WriteLine();
        }

        /// <summary>
        /// Defines the InsertMethod class.
        /// </summary>
        internal class InsertMethod
        {
            /// <summary>
            /// The insert statement.
            /// </summary>
            public const string InsertStatement = "Insert Statements";

            /// <summary>
            /// The union all
            /// </summary>
            public const string UnionAll = "Union All";
        }

        private class ColumnDefinition
        {
            public ColumnDefinition()
            {
            }

            public ColumnDefinition(Column column)
            {
                Name = column.Name;
                Type = column.UnderlyingType;
                Nullable = column.Nullable ?? true;
            }

            public string Name { get; set; }

            public Type Type { get; set; }

            public bool Nullable { get; set; }

            public int Size { get; set; }

            public int Precision { get; set; }

            public bool? IsIdentity { get; set; }

            public int Ordinal { get; set; }

            private string GetColumnDefinition()
            {
                string columnDef = string.Empty;
                switch (Type.ToString())
                {
                    case "System.String":
                        // Sizes doubled to allow for \n converted to \r\n
                        if (Size < 50)
                        {
                            columnDef = "VARCHAR(100)";
                        }
                        else if (Size < 250)
                        {
                            columnDef = "VARCHAR(500)";
                        }
                        else
                        {
                            columnDef = "TEXT";
                        }

                        break;
                    case "System.Decimal":
                        columnDef = "DECIMAL"; // TODO get size and precision.
                        break;
                    case "System.Double":
                        columnDef = "FLOAT";
                        break;
                    case "System.Int32":
                        columnDef = "INT";
                        break;
                    case "System.DateTime":
                        columnDef = "DATETIME";
                        break;
                    case "System.Boolean":
                        columnDef = "BIT";
                        break;
                    default:
                        columnDef = string.Format("VARCHAR(250) /* {0} */", Type);
                        break;
                }

                return columnDef;
            }

            public override string ToString()
            {
                if (IsIdentity ?? false)
                {
                    return string.Format("[{0}] [INT] IDENTITY (1, 1) NOT NULL", Name);
                }
                else
                {
                    return string.Format("[{0}] {1} {2}", Name, GetColumnDefinition(), (Nullable ? "NULL" : "NOT NULL"));
                }
            }
        }

        /// <summary>
        /// Defines the ColumnCollection class.
        /// </summary>
        private class ColumnCollection : Dictionary<string, ColumnDefinition>
        {
            /// <summary>
            /// Gets the columns.
            /// </summary>
            /// <value>
            /// The columns.
            /// </value>
            public IEnumerable<ColumnDefinition> Columns
            {
                get { return this.Values.OrderBy(c => c.Ordinal); }
            }
        }
    }
}

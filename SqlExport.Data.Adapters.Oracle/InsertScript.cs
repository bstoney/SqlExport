namespace SqlExport.Data.Adapters.Oracle
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    using SqlExport.Common.Data;
    using SqlExport.Export;

    /// <summary>
    /// Defines the InsertScript class.
    /// </summary>
    public class InsertScript : ExporterBase
    {
        ////private const int SqlMaxColumnCount = 1024;
        ////private const int SqlMaxRowSize = 8060;

        /// <summary>
        /// The max index width
        /// </summary>
        private const int MaxIndexWidth = 3;

        /// <summary>
        /// Gets the name of the adapter.
        /// </summary>
        public override string Name
        {
            get { return "Oracle"; }
        }

        /// <summary>
        /// Gets the dialog filename filter.
        /// </summary>
        public override string FileFilter
        {
            get { return "Sql Files (*.sql)|*.sql|All Files (*.*)|*.*"; }
        }

        /// <summary>
        /// Exports the data to the supplied text writer.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="writer">The writer.</param>
        public override void Export(IDataResult data, TextWriter writer)
        {
            Status = "Exporting data as SQL";

            // TODO code for more than SqlMaxColumnCount
            int startColumn = 0;
            int endColumn = 0;
            ////int rowSize = 0;
            var columns = data.FetchColumns().ToList();
            for (int tableIndex = 1; startColumn < columns.Count; tableIndex++)
            {
                ////for (endColumn = startColumn; endColumn < columns.Count - 1; endColumn++)
                ////{
                ////    int columnSize = columns[endColumn].GetSize(data);

                ////    if (rowSize + columnSize > SqlMaxRowSize ||
                ////        (endColumn - startColumn >= SqlMaxColumnCount - 2 && columns.Count > SqlMaxColumnCount))
                ////    {
                ////        break;
                ////    }

                ////    rowSize += columnSize;
                ////}

                endColumn = columns.Count - 1;

                bool spanMultipleTables = tableIndex > 1 || startColumn > 0 || endColumn < columns.Count - 1;

                WriteTableDefinition(data, writer, tableIndex, startColumn, endColumn, spanMultipleTables);

                startColumn = endColumn + 1;
                ////rowSize = 0;
            }

            Status = "Export complete";
        }

        /// <summary>
        /// Writes the table definition.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="tableIndex">Index of the table.</param>
        /// <param name="startColumn">The start column.</param>
        /// <param name="endColumn">The end column.</param>
        /// <param name="spanMultipleTables">if set to <c>true</c> [span multiple tables].</param>
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
                    IsPrimaryKey = true
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
                                            columnDefinition.IsPrimaryKey = false;
                                        }
                                        else if (!columnDefinition.IsPrimaryKey.HasValue)
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
                                            columnDefinition.IsPrimaryKey = hasThreePoints && !hasNullValues && hasConsecutiveValues;
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
                                columnDefinition.IsPrimaryKey = false;
                                tempLineWriter.Write("NULL");
                            }
                        }

                        tempFileWriter.Write("INSERT INTO \"{0}\" (", tableName);
                        tempFileWriter.Write(string.Join(",", columnDefinitions.Columns.Select(c => "\"" + c.Name + "\"").ToArray()));
                        tempFileWriter.Write(") VALUES ( ");
                        tempFileWriter.Write(tempLineWriter.ToString());
                        tempFileWriter.WriteLine(" )");
                        tempFileWriter.WriteLine("/");
                    }
                }
            }

            writer.Write(@"
/****** Create Object:  Table ""{0}""    Script Date: {1:dd/MM/yyyy HH:mm:ss} ******/
IF EXISTS( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[{0}]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1 )
DROP TABLE ""{0}""
/
",
                tableName,
                DateTime.Now);
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("CREATE TABLE \"{0}\" (", tableName);
            writer.Write("\t");

            writer.Write(string.Join(",\n\t", columnDefinitions.Columns.Select(c => c.ToString()).ToArray()));

            ColumnDefinition primaryKey = columnDefinitions.Columns.Where(c => c.IsPrimaryKey ?? false).FirstOrDefault();
            if (primaryKey != null)
            {
                writer.WriteLine(",");
                writer.WriteLine("\tCONSTRAINT \"PK_{0}\" PRIMARY KEY ", tableName);
                writer.WriteLine("\t(");
                writer.WriteLine("\t\t\"{0}\"", primaryKey.Name);
                writer.WriteLine("\t)");
                writer.WriteLine(")");
            }
            else
            {
                writer.WriteLine(")");
            }

            writer.WriteLine("/");
            writer.WriteLine();

            writer.Write(File.ReadAllText(tempFile));
            File.Delete(tempFile);

            writer.WriteLine("/");
            writer.WriteLine();
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

            public bool? IsPrimaryKey { get; set; }

            public int Ordinal { get; set; }

            private string GetColumnDefinition()
            {
                string columnDef;
                switch (Type.ToString())
                {
                    case "System.String":
                        // Sizes doubled to allow for \n converted to \r\n
                        if (Size < 50)
                        {
                            columnDef = "VARCHAR2(100)";
                        }
                        else if (Size < 250)
                        {
                            columnDef = "VARCHAR2(500)";
                        }
                        else
                        {
                            columnDef = "CLOB";
                        }

                        break;
                    case "System.Decimal":
                        columnDef = "DECIMAL"; // TODO get size and precision.
                        break;
                    case "System.Double":
                        columnDef = "DOUBLE";
                        break;
                    case "System.Int32":
                        columnDef = "INT";
                        break;
                    case "System.DateTime":
                        columnDef = "DATETIME";
                        break;
                    case "System.Boolean":
                        columnDef = "BOOLEAN";
                        break;
                    default:
                        columnDef = string.Format("VARCHAR2(250) /* {0} */", Type);
                        break;
                }

                return columnDef;
            }

            public override string ToString()
            {
                return string.Format("\"{0}\" {1} {2}", Name, GetColumnDefinition(), (Nullable ? "NULL" : "NOT NULL"));
            }
        }

        private class ColumnCollection : Dictionary<string, ColumnDefinition>
        {
            public IEnumerable<ColumnDefinition> Columns
            {
                get { return base.Values.OrderBy(c => c.Ordinal); }
            }
        }
    }
}

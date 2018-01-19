using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SqlExport.Data.Adapters.Text.Query
{
    internal class From
    {
        public From(string table, string alias)
        {
            Table = table;
            Alias = alias ?? table;
        }

        public string Table { get; private set; }

        public string Alias { get; private set; }

        public CodeLine CodeLine { get; set; }

        public virtual IEnumerable<Column> GetColumns(ConnectionString connectionString)
        {
            if (SchemaAdapter.IsFileDetailsTableName(Table))
            {
                yield return new Column("Filename", "String", false);
                yield return new Column("FullPath", "String", false);
                yield return new Column("Size", "Long", false);
                yield return new Column("DateCreated", "DateTime", false);
                yield return new Column("DateModified", "DateTime", false);
            }
            else if (SchemaAdapter.IsColumnDetailsTableName(Table))
            {
                yield return new Column("Filename", "String", false);
                yield return new Column("ColumnName", "String", false);
                yield return new Column("DataType", "Type", false);
            }
            else
            {
                using (TextReader reader = File.OpenText(GetFilename(connectionString)))
                {
                    CsvParser cp = new CsvParser(reader, connectionString.HasHeaders);
                    foreach (var item in cp.ColumnNames)
                    {
                        yield return new Column(item, "String", true);
                    }
                }
            }
        }

        public virtual IEnumerable<DataLine> GetLines(ConnectionString connectionString)
        {
            if (SchemaAdapter.IsFileDetailsTableName(Table))
            {
                return GetTableDetails(connectionString);
            }
            else if (SchemaAdapter.IsColumnDetailsTableName(Table))
            {
                return GetColumnDetails(connectionString);
            }
            else
            {
                if (File.Exists(GetFilename(connectionString)))
                {
                    return GetTableData(connectionString);
                }
                else
                {
                    throw new QueryRunnerException("File not found " + GetFilename(connectionString) + ".", CodeLine.LineNumber);
                }
            }
        }

        public override string ToString()
        {
            return "FROM " + Table + (Table == Alias ? string.Empty : " " + Alias);
        }

        protected virtual string GetFilename(ConnectionString connectionString)
        {
            return Path.Combine(connectionString.BasePath, Table);
        }

        private IEnumerable<DataLine> GetTableDetails(ConnectionString connectionString)
        {
            int i = 0;
            foreach (var item in Directory.GetFileSystemEntries(connectionString.BasePath, "*.csv"))
            {
                FileInfo fi = new FileInfo(item);
                yield return new DataLine(new object[] { fi.Name, item, fi.Length, fi.CreationTime, fi.LastWriteTime }, i);
                i++;
            }
        }

        private IEnumerable<DataLine> GetColumnDetails(ConnectionString connectionString)
        {
            int i = 0;
            foreach (var file in Directory.GetFileSystemEntries(connectionString.BasePath, "*.csv"))
            {
                string filename = Path.GetFileName(file);
                TextReader reader = File.OpenText(file);
                try
                {
                    CsvParser cp = new CsvParser(reader, connectionString.HasHeaders);
                    foreach (var item in cp.ColumnNames)
                    {
                        yield return new DataLine(new object[] { filename, item, typeof(string) }, i);
                        i++;
                    }
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        private IEnumerable<DataLine> GetTableData(ConnectionString connectionString)
        {
            int i = 0;
            TextReader reader = File.OpenText(GetFilename(connectionString));
            try
            {
                CsvParser cp = new CsvParser(reader, connectionString.HasHeaders);
                foreach (var line in cp.ReadAllLines().Skip(connectionString.HasHeaders ? 1 : 0))
                {
                    yield return new DataLine(line, i);
                    i++;
                }
            }
            finally
            {
                reader.Close();
                reader.Dispose();
            }
        }
    }
}

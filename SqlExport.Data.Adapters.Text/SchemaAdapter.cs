using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.IO;

namespace SqlExport.Data.Adapters.Text
{
    using SqlExport.Common.Data;

    internal class SchemaAdapter : ISchemaAdapter
    {
        internal const string RowNumberColumnName = "$RowNumber";
        private const string FileDetailsTableName = "$Files";
        private const string ColumnDetailsTableName = "$Columns";

        private const int FilesFolderPath = 0;
        private const int FilenamePath = 1;
        private const int ItemNamePath = 2;

        private CommandAdapter _commandAdapter;

        public SchemaAdapter(CommandAdapter commandAdapter)
        {
            _commandAdapter = commandAdapter;
        }

        private IEnumerable<SchemaItem> GetFiles()
        {
            try
            {
                var results = _commandAdapter.ExecuteCommand(new StringReader("SELECT *\nFROM " + FileDetailsTableName));

                var data = results.First();
                return data.AsEnumerable().OfType<object>().Select(r => new SchemaItem((string)data.FetchValue(r, "Filename"), SchemaItemType.Table));
            }
            catch (ExecuteCommandException)
            {
                return Enumerable.Empty<SchemaItem>();
            }
        }

        private IEnumerable<SchemaItem> GetColumns(string filename)
        {
            try
            {
                var results = _commandAdapter.ExecuteCommand(new StringReader("SELECT *\nFROM " + ColumnDetailsTableName));

                var data = results.First();
                return (from r in data.AsEnumerable().OfType<object>()
                        where (string)data.FetchValue(r, "Filename") == filename
                        select new Column((string)data.FetchValue(r, "ColumnName"), (string)data.FetchValue(r, "DataType"), null))
                        .Cast<SchemaItem>();
            }
            catch (ExecuteCommandException)
            {
                return Enumerable.Empty<SchemaItem>();
            }
        }

        public static bool IsFileDetailsTableName(string tableName)
        {
            return string.Compare(tableName, FileDetailsTableName, true) == 0;
        }

        public static bool IsColumnDetailsTableName(string tableName)
        {
            return string.Compare(tableName, ColumnDetailsTableName, true) == 0;
        }

        public static bool IsRowNumberColumnName(string columnName)
        {
            return string.Compare(columnName, RowNumberColumnName, true) == 0;
        }

        #region ISchemaAdapter Members

        public string[] GetSections()
        {
            return new string[] { "Files", "FileDetails" };
        }

        public ISchemaItem[] PopulateFromPath(string[] path)
        {
            if (path.Length == FilesFolderPath + 1)
            {
                switch (path[FilesFolderPath])
                {
                    case "Files":
                        return GetFiles().ToArray();
                    case "FileDetails":
                        return new[] { 
							new SchemaItem( FileDetailsTableName, SchemaItemType.Table ),
							new SchemaItem( ColumnDetailsTableName, SchemaItemType.Table ) 
						};
                }
            }
            else if (path.Length == FilenamePath + 1)
            {
                return GetColumns(path[FilenamePath]).ToArray();
            }

            return new SchemaItem[] { };
        }

        public string GetSchemaItemScript(string[] path)
        {
            return null;
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

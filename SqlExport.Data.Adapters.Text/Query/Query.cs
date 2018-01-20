using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text.Query
{
    using System.IO;

    internal class Query
    {
        private Dictionary<string, Func<DataLine, object>> _valueMap;

        public int QueryIndex { get; set; }

        public virtual Selection Selection { get; set; }

        public virtual From From { get; set; }

        public virtual Where Where { get; set; }

        public virtual string Name
        {
            get { return From != null ? From.Alias : string.Empty; }
        }

        public virtual ConnectionString ConnectionString { get; protected set; }

        public virtual string[] DestinationColumns { get; protected set; }

        public virtual IEnumerable<object[]> Lines { get; protected set; }

        public virtual bool IsPrepared { get; protected set; }

        public virtual object GetValue(string column, DataLine line)
        {
            try
            {
                return _valueMap[column](line) ?? DBNull.Value;
            }
            catch (IndexOutOfRangeException)
            {
                throw this.GetInvalidColumnException(column);
            }
        }

        public virtual void Prepare(Action<QueryRunnerException> onError, ConnectionString connectionString)
        {
            ConnectionString = connectionString;
            IsPrepared = false;
            Lines = null;
            DestinationColumns = null;
            _valueMap = null;

            if (!Directory.Exists(ConnectionString.BasePath) &&
                !SchemaAdapter.IsFileDetailsTableName(this.From.Table) &&
                !SchemaAdapter.IsColumnDetailsTableName(this.From.Table))
            {
                onError(new QueryRunnerException("Directory not found " + ConnectionString.BasePath + "."));
                return;
            }

            // TODO make the value map key based so that the where clause works with $rownumber.
            bool hasErrors = false;
            var sourceColumnNames = this.From.GetColumns(ConnectionString);
            DestinationColumns = this.Selection.Columns.SelectMany(c => c == "*" ? sourceColumnNames.Select(a => a.Name) : new[] { c }).ToArray();
            _valueMap = DestinationColumns.ToDictionary(c => c, c =>
            {
                if (SchemaAdapter.IsRowNumberColumnName(c))
                {
                    return new Func<DataLine, object>(l => l.Index);
                }

                var sourceColumn = sourceColumnNames.Select((sc, i) => new { Name = sc.Name, Index = i }).FirstOrDefault(sc => sc.Name == c);
                if (sourceColumn == null)
                {
                    hasErrors = true;
                    onError(this.GetInvalidColumnException(c));
                    return new Func<DataLine, object>(l => null);
                }

                return new Func<DataLine, object>(l => (sourceColumn.Index < l.Length ? l[sourceColumn.Index] : null));
            });

            if (!hasErrors)
            {
                try
                {
                    this.Where.Initialise(sourceColumnNames.Select(sc => sc.Name).ToArray());

                    var linesQuery = from l in From.GetLines(ConnectionString)
                                     where Where.IsMatch(l)
                                     select DestinationColumns.Select(c => GetValue(c, l)).ToArray();

                    Lines = linesQuery;

                    IsPrepared = true;
                }
                catch (QueryParserException ex)
                {
                    onError(new QueryRunnerException(ex.Message, ex) { LineNumber = this.Where.CodeLine.LineNumber });
                }
            }

        }

        private QueryRunnerException GetInvalidColumnException(string column)
        {
            return new QueryRunnerException(
                "Invalid column " + column + ", line " + this.Selection.CodeLine.LineNumberOf(column) + ".",
                this.Selection.CodeLine.LineNumberOf(column));
        }

        public override string ToString()
        {
            return Selection + "\n" + From + (Where != Where.Empty ? "\n" + Where : string.Empty);
        }
    }
}

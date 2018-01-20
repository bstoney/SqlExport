using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text.Query
{
    internal class Union : Query
    {
        public Union()
        {
            QueryParts = new List<Query>();
        }

        public IList<Query> QueryParts { get; private set; }

        public override Selection Selection
        {
            get { return QueryParts.First().Selection; }
            set { base.Selection = value; }
        }

        public override From From
        {
            get { return new UnionFrom(this); }
            set { }
        }

        public override Where Where
        {
            get { return Where.Empty; }
            set { }
        }

        public override string ToString()
        {
            return string.Join("\nUNION\n", QueryParts.Select(q => q.ToString()).ToArray());
        }

        public override string Name
        {
            get { return QueryParts.First().From.Alias; }
        }

        public override object GetValue(string column, DataLine line)
        {
            return QueryParts.First().GetValue(column, line);
        }

        public override void Prepare(Action<QueryRunnerException> onError, Text.ConnectionString connectionString)
        {
            foreach (var queryPart in QueryParts)
            {
                queryPart.Prepare(onError, connectionString);
            }

            DestinationColumns = QueryParts.First().DestinationColumns;

            Lines = from q in this.QueryParts
                    from l in q.Lines
                    select l;
        }

        private class UnionFrom : From
        {
            private readonly Union _union;

            public UnionFrom(Union union) :
                base(string.Empty, null)
            {
                _union = union;
            }

            public override IEnumerable<Column> GetColumns(ConnectionString connectionString)
            {
                return _union.QueryParts.First().From.GetColumns(connectionString);
            }

            public override IEnumerable<DataLine> GetLines(ConnectionString connectionString)
            {
                var query = from q in _union.QueryParts
                            from l in q.From.GetLines(connectionString)
                            select l;

                return query;
            }
        }
    }
}

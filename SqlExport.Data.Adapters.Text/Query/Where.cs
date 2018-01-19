using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SqlExport.Data.Adapters.Text.Query
{
    internal class Where
    {
        public static readonly Where Empty = new EmptyWhere();

        private Func<DataLine, bool> _matchFunction;

        protected Where()
        {
        }

        public Where(string column, string op, string value)
        {
            Column = column;
            Operator = op;
            Value = value;
        }

        public string Column { get; private set; }

        public string Operator { get; private set; }

        public string Value { get; private set; }

        public CodeLine CodeLine { get; set; }

        public virtual void Initialise(string[] columns)
        {
            int columnIndex = Array.IndexOf<string>(columns, Column);
            if (columnIndex < 0)
            {
                throw new QueryParserException("Invalid column '" + Column + "'.");
            }

            _matchFunction = line =>
            {
                switch (Operator)
                {
                    case "=":
                        return CaseInsensitiveComparer.DefaultInvariant.Compare(Convert.ToString(line[columnIndex]), Value) == 0;
                    case "<":
                        return CaseInsensitiveComparer.DefaultInvariant.Compare(Convert.ToString(line[columnIndex]), Value) < 0;
                    case ">":
                        return CaseInsensitiveComparer.DefaultInvariant.Compare(Convert.ToString(line[columnIndex]), Value) < 0;
                    case "<>":
                    case "!=":
                        return CaseInsensitiveComparer.DefaultInvariant.Compare(Convert.ToString(line[columnIndex]), Value) != 0;
                    default:
                        return false;
                }
            };
        }

        public virtual bool IsMatch(DataLine line)
        {
            return _matchFunction(line);
        }

        private class EmptyWhere : Where
        {
            public override void Initialise(string[] columns)
            {
            }

            public override bool IsMatch(DataLine line)
            {
                return true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal class QueryPart<T>
	{
		public T Part { get; set; }

		public IEnumerable<string> Rest { get; set; }

		public CodeLine CodeLine { get; set; }
	}
}

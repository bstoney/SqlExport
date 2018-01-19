using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal class Selection
	{
		public Selection( IEnumerable<string> columns )
		{
			Columns = columns;
		}

		public IEnumerable<string> Columns { get; private set; }

		public CodeLine CodeLine { get; set; }

		public override string ToString()
		{
			return "SELECT " + string.Join( ", ", Columns.Select( ( c, i ) => (i < 4 ? c : "...") ).Take( 4 ).ToArray() );
		}
	}
}

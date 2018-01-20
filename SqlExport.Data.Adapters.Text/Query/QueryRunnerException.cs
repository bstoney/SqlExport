using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SqlExport.Data.Adapters.Text.Query
{
	[Serializable]
	public class QueryRunnerException : Exception
	{
		public QueryRunnerException()
		{
		}

		public QueryRunnerException( string message )
			: base( message )
		{
		}

		public QueryRunnerException( string message, int? lineNumber )
			: base( message )
		{
			LineNumber = lineNumber;
		}

		public QueryRunnerException( string message, Exception inner )
			: base( message, inner )
		{
		}

		protected QueryRunnerException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
		}

		public int? LineNumber { get; set; }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SqlExport.Data.Adapters.Text.Query
{
	[Serializable]
	public class QueryParserException : Exception
	{
		public QueryParserException()
		{
		}

		public QueryParserException( string message )
			: base( message )
		{
		}

		public QueryParserException( string message, Exception inner )
			: base( message, inner )
		{
		}

		protected QueryParserException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
		}
	}
}

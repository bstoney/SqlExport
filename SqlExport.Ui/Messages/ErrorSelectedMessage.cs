using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages
{
	public class ErrorSelectedMessage
	{
		public ErrorSelectedMessage( int lineNumber )
		{
			LineNumber = lineNumber;
		}

		public int LineNumber { get; set; }
	}
}

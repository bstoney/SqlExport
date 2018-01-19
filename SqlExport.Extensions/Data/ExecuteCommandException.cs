using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SqlExport.Data
{
	[global::System.Serializable]
	public class ExecuteCommandException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public ExecuteCommandException()
		{
		}

		public ExecuteCommandException( string message )
			: base( message )
		{
		}

		public ExecuteCommandException( string message, Exception inner )
			: base( message, inner )
		{
		}

		protected ExecuteCommandException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages
{
	public class ApplicationDisplayMessage : DisplayMessage
	{
		public ApplicationDisplayMessage( string text )
			: base( text )
		{
		}

		public ApplicationDisplayMessage( string text, string details, DisplayMessageType type, Action callback )
			: base( text, details, type, callback )
		{
		}
	}
}

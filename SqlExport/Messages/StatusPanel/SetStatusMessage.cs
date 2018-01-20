using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages.StatusPanel
{
	public class SetStatusMessage
	{
		public SetStatusMessage( string status )
		{
			this.Status = status;
		}

		public string Status { get; set; }
	}
}

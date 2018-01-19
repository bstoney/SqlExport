using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages.StatusPanel
{
	public class SetElapsedTimeMessage
	{
		public SetElapsedTimeMessage( TimeSpan elapsedTime )
		{
			this.ElapsedTime = elapsedTime;
		}

		public TimeSpan ElapsedTime { get; set; }
	}
}

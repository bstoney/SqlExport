using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages.StatusPanel
{
	public class SetRecordCountMessage
	{
		public SetRecordCountMessage( int? recordCount )
		{
			this.RecordCount = recordCount;
		}

		public int? RecordCount { get; private set; }
	}
}

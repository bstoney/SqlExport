using System;
using System.Collections.Generic;
using System.Text;

namespace SqlExport.Data.Adapters.SqLite
{
	class SelectTemplate : IStatementTemplate
	{
		#region IStatementTemplate Members

		public string Name
		{
			get { return "Select"; }
		}

		public string Statement
		{
			get { return "SELECT * \r\nFROM \r\n"; }
		}

		#endregion
	}
}

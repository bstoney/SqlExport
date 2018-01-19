using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;

namespace SqlExport.Messages
{
	internal class InitialiseDataViewMessage
	{
		public IEnumerable<Column> Columns { get; set; }

		public int RowCount { get; set; }

		public GetCellValueHandler CellValueCallback { get; set; }
	}
}

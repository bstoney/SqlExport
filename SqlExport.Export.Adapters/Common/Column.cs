using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Represents a work sheet column.
	/// </summary>
	public class Column
	{
		/// <summary>
		/// Initializes a new instance of the Column class.
		/// </summary>
		internal Column( int index )
		{
			Index = index;
		}

		/// <summary>
		/// Gets the column index.
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Gets or sets the index of the column.
		/// </summary>
		public int? Width { get; set; }
	}
}

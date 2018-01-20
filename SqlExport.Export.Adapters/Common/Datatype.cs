using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Datatype of a cell.
	/// </summary>
	public enum Datatype
	{
		/// <summary>
		/// Use value type.
		/// </summary>
		None,
		/// <summary>
		/// Boolean datatype.
		/// </summary>
		Boolean,
		/// <summary>
		/// Number datatype.
		/// </summary>
		Number,
		/// <summary>
		/// String datatype.
		/// </summary>
		String,
		/// <summary>
		/// Datetime datatype.
		/// </summary>
		DateTime
	}
}

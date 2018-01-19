using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SqlExport.Export.Adapters.Common
{
	public class CellFont : IEquatable<CellFont>
	{
		public string Name { get; set; }

		public int? Size { get; set; }

		public FontStyle Style { get; set; }

		#region IEquatable<CellFont> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		public bool Equals( CellFont other )
		{
			return other != null && Name == other.Name && Size == other.Size && Style == other.Style;
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Defines the complete border style for a cell.
	/// </summary>
	public class CellBorder : IEquatable<CellBorder>
	{
		/// <summary>
		/// Initializes a new instance of the Border class.
		/// </summary>
		public CellBorder()
			: this( Border.Empty )
		{
		}

		/// <summary>
		/// Initializes a new instance of the Border class.
		/// </summary>
		public CellBorder( Border style )
		{
			Left = style;
			Top = style;
			Right = style;
			Bottom = style;
		}

		/// <summary>
		/// Initializes a new instance of the Border class.
		/// </summary>
		public CellBorder( Border vertical, Border horizontal )
		{
			Left = Right = vertical;
			Top = Bottom = horizontal;
		}

		/// <summary>
		/// Initializes a new instance of the Border class.
		/// </summary>
		public CellBorder( Border left, Border top, Border right, Border bottom )
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>
		/// Gets or sets the left border style.
		/// </summary>
		public Border Left { get; set; }

		/// <summary>
		/// Gets or sets the top border style.
		/// </summary>
		public Border Top { get; set; }

		/// <summary>
		/// Gets or sets the right border style.
		/// </summary>
		public Border Right { get; set; }

		/// <summary>
		/// Gets or sets the bottom border style.
		/// </summary>
		public Border Bottom { get; set; }

		#region IEquatable<CellBorder> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		public bool Equals( CellBorder other )
		{
			return other != null && Left.Equals( other.Left ) && Top.Equals( other.Top ) && Right.Equals( other.Right ) && Bottom.Equals( other.Bottom );
		}

		#endregion
	}
}

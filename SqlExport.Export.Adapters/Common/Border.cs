using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Border style information.
	/// </summary>
	public struct Border
	{
		/// <summary>
		/// No border defined.
		/// </summary>
		public static Border Empty = new Border( LineStyle.Default, null, Color.Empty );

		/// <summary>
		/// Single thin line in default colour.
		/// </summary>
		public static Border Thin = new Border( LineStyle.Solid, 1, Color.Black );

		/// <summary>
		/// Single medium line in default colour.
		/// </summary>
		public static Border Medium = new Border( LineStyle.Solid, 2, Color.Black );

		/// <summary>
		/// Single thick line in default colour.
		/// </summary>
		public static Border Thick = new Border( LineStyle.Solid, 3, Color.Black );

		/// <summary>
		/// Double thin lines in default colour.
		/// </summary>
		public static Border Double = new Border( LineStyle.Double, 1, Color.Black );

		/// <summary>
		/// Initializes a new instance of the BorderStyle struct.
		/// </summary>
		public Border( LineStyle style, int? weight, Color colour )
			: this()
		{
			Style = style;
			Weight = weight;
			Color = colour;
		}

		/// <summary>
		/// Gets or sets the line style.
		/// </summary>
		public LineStyle Style { get; set; }

		/// <summary>
		/// gets or sets the line thickness.
		/// </summary>
		public int? Weight { get; set; }

		/// <summary>
		/// Gets or sets the line colour.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Is this an empty border reference.
		/// </summary>
		/// <returns>True if this is equivelent to BorderStyle.Empty; otherwise false.</returns>
		public bool IsEmpty
		{
			get
			{
				return Style == LineStyle.Default && Weight == null && Color.IsEmpty;
			}
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the current Object.
		/// </summary>
		public override bool Equals( object obj )
		{
			if( obj == null || !(obj is Border) )
			{
				return false;
			}
			Border other = (Border)obj;
			return Style == other.Style && Weight == other.Weight && Color == other.Color;
		}

		/// <summary>
		/// Returns a hashcode for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return string.Concat( Style, Weight, Color ).GetHashCode();
		}
	}
}

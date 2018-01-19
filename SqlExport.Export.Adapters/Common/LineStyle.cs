using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Border line style.
	/// </summary>
	public enum LineStyle
	{
		/// <summary>
		/// Default excel line settings - no line.
		/// </summary>
		Default,
		/// <summary>
		/// Hidden line.
		/// </summary>
		Hidden,
		/// <summary>
		/// Dotted line.
		/// </summary>
		Dotted,
		/// <summary>
		/// Dashed line.
		/// </summary>
		Dash,
		/// <summary>
		/// Solid line.
		/// </summary>
		Solid,
		/// <summary>
		/// Double line.
		/// </summary>
		Double,
		/// <summary>
		/// Groove border.
		/// </summary>
		Groove,
		/// <summary>
		/// Ridge borde.
		/// </summary>
		Ridge,
		/// <summary>
		/// Inset border.
		/// </summary>
		inset,
		/// <summary>
		/// Outset border.
		/// </summary>
		Outset
	}
}

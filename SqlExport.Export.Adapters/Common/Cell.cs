using System;
using System.Drawing;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Represents a data cell.
	/// </summary>
	public class Cell
	{
		/// <summary>
		/// Initializes a new instance of the Cell class.
		/// </summary>
		/// <param name="column">The index of the cell column.</param>
		/// <param name="row">The index of the cell row.</param>
		public Cell( Column column, Row row )
			: base()
		{
			Datatype = Datatype.None;
			Alignment = Alignment.Default;
			Background = Color.Empty;
			Foreground = Color.Empty;

			Column = column;
			Row = row;
		}

		/// <summary>
		/// Gets or sets the value of this cell.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Gets or sets the number of columns to span.
		/// </summary>
		public int? ColumnSpan { get; set; }

		/// <summary>
		/// Gets or sets the number of rows to span.
		/// </summary>
		public int RowSpan { get; set; }

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		public CellFont Font { get; set; }

		/// <summary>
		/// Gets or sets the cell border style.
		/// </summary>
		public CellBorder Border { get; set; }

		/// <summary>
		/// Gets or sets the cell text alignment.
		/// </summary>
		public Alignment Alignment { get; set; }

		/// <summary>
		/// Gets or sets the cell foreground colour.
		/// </summary>
		public Color Foreground { get; set; }

		/// <summary>
		/// Gets or sets the cell background colour.
		/// </summary>
		public Color Background { get; set; }

		/// <summary>
		/// Gets or sets the cell display format.
		/// Eg. \#\,\#\#0 or Short Date or _-\0022$\0022* \#\,\#\#0_-\;\\-\0022$\0022* \#\,\#\#0_-\;_-\0022$\0022* \0022-\0022_-\;_-\@_- .
		/// </summary>
		public string CellFormat { get; set; }

		/// <summary>
		/// Gets or sets an enfoced data type for the cell.
		/// </summary>
		public Datatype Datatype { get; set; }

		/// <summary>
		/// Gets the cell column index.
		/// </summary>
		public Column Column { get; private set; }

		/// <summary>
		/// Gets the cell row index.
		/// </summary>
		public Row Row { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the cell value is a numeric type.
		/// </summary>
		public bool IsNumeric
		{
			get
			{
				return Value == null || Value is byte || Value is sbyte
					|| Value is decimal || Value is double || Value is float
					|| Value is int || Value is uint || Value is long || Value is ulong
					|| Value is short || Value is ushort;
			}
		}

		/// <summary>
		/// Gets the Excel cell reference.
		/// </summary>
		public string Reference
		{
			get
			{
				if( Column.Index < 26 )
				{
					return Convert.ToChar( (Column.Index % 26) + 65 ).ToString() + (Row.Index + 1);
				}
				else
				{
					return Convert.ToChar( (int)Math.Floor( Column.Index / 26.0 ) + 64 ).ToString() + 
						Convert.ToChar( (Column.Index % 26) + 65 ) + (Row.Index + 1);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace SqlExport.Export.Adapters.Common
{
	public class ExcelWriter : IWorkbookWriter
	{
		private const string SpreadsheetNamespace = "urn:schemas-microsoft-com:office:spreadsheet";
		private const string SpreadsheetPrefix = "ss";
		private const string ExcelNamespace = "urn:schemas-microsoft-com:office:excel";
		private const string OfficeNamespace = "urn:schemas-microsoft-com:office:office";

		XmlWriter _writer;
		Dictionary<Cell, string> _styles = new Dictionary<Cell, string>( new CellStyleComparer() );

		#region IWorkbookWriter Members

		/// <summary>
		/// Write the workbook to the text stream.
		/// </summary>
		public void Write( Workbook workbook, TextWriter writer )
		{
			using( _writer = new XmlWriter( writer ) )
			{
				_writer.IndentCharacters = " ";
				_writer.NewLineCharacters = "\n";
				_writer.WriteStartDocument();
				_writer.WriteProcessingInstruction( "mso-application", "progid=\"Excel.Sheet\"" );
				_writer.WriteStartElement( "Workbook", SpreadsheetNamespace );
				_writer.WriteAttributeString( "xmlns", SpreadsheetPrefix, SpreadsheetNamespace );

				_writer.WriteStartElement( "DocumentProperties", OfficeNamespace );

				if( !string.IsNullOrEmpty( workbook.Author ) )
				{
					_writer.WriteElementString( "Author", workbook.Author );
					_writer.WriteElementString( "LastAuthor", workbook.Author );
				}

				_writer.WriteElementString( "Created", DateTime.Now.ToUniversalTime().ToString( "yyyy-MM-ddTHH:mm:ssZ" ) );
				_writer.WriteElementString( "LastSaved", DateTime.Now.ToUniversalTime().ToString( "yyyy-MM-ddTHH:mm:ssZ" ) );
				// TODO Company
				_writer.WriteElementString( "Version", "11.5606" );

				_writer.WriteEndElement();

				_writer.WriteStartElement( "ExcelWorkbook", ExcelNamespace );
				_writer.WriteElementString( "ProtectStructure", "False" );
				_writer.WriteElementString( "ProtectWidows", "False" );
				_writer.WriteEndElement();

				var cs = workbook.Sheets.SelectMany( ws => ws.Rows.SelectMany( r => r ) ).Where( c => CellStyleComparer.HasStyle( c ) );

				_writer.WriteStartElement( "Styles" );
				foreach( var c in cs )
				{
					if( !_styles.ContainsKey( c ) )
					{
						string styleId = string.Concat( "s", _styles.Count );
						_styles.Add( c, styleId );

						_writer.WriteStartElement( "Style" );
						_writer.WritePrefixedAttribute( SpreadsheetPrefix, "ID", styleId );
						WriteCellStyle( c );
						_writer.WriteEndElement();
					}
				}
				_writer.WriteEndElement();


				foreach( Worksheet sheet in workbook.Sheets )
				{
					WriteWorksheet( sheet );
				}

				_writer.WriteEndElement();
			}
		}

		private void WriteWorksheet( Worksheet sheet )
		{
			_writer.WriteStartElement( "Worksheet" );
			_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Name", sheet.Name ?? string.Concat( "Sheet ", sheet.Index ) );

			_writer.WriteStartElement( "Table" );

			foreach( Column column in sheet.Columns )
			{
				if( column.Width.HasValue )
				{
					_writer.WriteStartElement( "Column" );
					_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Index", (column.Index + 1).ToString() );
					_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Width", column.Width.ToString() );
					_writer.WriteEndElement();
				}
			}

			foreach( Row row in sheet.Rows )
			{
				_writer.WriteStartElement( "Row" );
				if( row.Height.HasValue )
				{
					_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Height", row.Height.ToString() );
				}

				foreach( Cell cell in row )
				{
					WriteCell( cell );
				}

				_writer.WriteEndElement();
			}

			_writer.WriteEndElement();

			_writer.WriteStartElement( "WorksheetOptions", ExcelNamespace );

			if( sheet.Zoom.HasValue )
			{
				_writer.WriteElementString( "Zoom", sheet.Zoom.ToString() );
			}

			WritePanes( sheet );

			_writer.WriteElementString( "ProtectObjects", "False" );
			_writer.WriteElementString( "ProtectScenarios", "False" );

			_writer.WriteEndElement();

			_writer.WriteEndElement();
		}

		private void WritePanes( Worksheet sheet )
		{
			string activePane = null;
			string[] panes;
			if( sheet.FrozenX.HasValue || sheet.FrozenY.HasValue )
			{
				_writer.WriteElementString( "FreezePanes", null );
				_writer.WriteElementString( "FrozenNoSplit", null );

				if( sheet.FrozenX.HasValue )
				{
					_writer.WriteElementString( "SplitVertical", sheet.FrozenX.ToString() );
					_writer.WriteElementString( "LeftColumnRightPane", sheet.FrozenX.ToString() );
				}
				if( sheet.FrozenY.HasValue )
				{
					_writer.WriteElementString( "SplitHorizontal", sheet.FrozenY.ToString() );
					_writer.WriteElementString( "TopRowBottomPane", sheet.FrozenY.ToString() );
				}
				if( sheet.FrozenX.HasValue && sheet.FrozenY.HasValue )
				{
					activePane = "0";
					panes = new[] { "3", "1", "2", "0" };
				}
				else if( sheet.FrozenX.HasValue )
				{
					activePane = "1";
					panes = new[] { "3", "1" };
				}
				else
				{
					activePane = "2";
					panes = new[] { "3", "2" };
				}
			}
			else
			{
				panes = new[] { "3" };
			}
			if( activePane != null )
			{
				_writer.WriteElementString( "ActivePane", activePane );
			}
			_writer.WriteStartElement( "Panes" );
			foreach( string pane in panes )
			{
				_writer.WriteStartElement( "Pane" );
				_writer.WriteElementString( "Number", pane );
				_writer.WriteEndElement();
			}

			_writer.WriteEndElement();
		}

		private void WriteCell( Cell cell )
		{
			_writer.WriteStartElement( "Cell" );
			string style = GetCellStyle( cell );
			if( style != null )
			{
				_writer.WritePrefixedAttribute( SpreadsheetPrefix, "StyleID", style );
			}

			_writer.WriteStartElement( "Data" );
			switch( cell.Datatype )
			{
				case Datatype.Boolean:
				case Datatype.Number:
				case Datatype.String:
				case Datatype.DateTime:
					_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Type", cell.Datatype.ToString() );
					break;
				default:
					_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Type", "String" );
					break;
			}

			_writer.WriteString( cell.Value.ToString() );

			_writer.WriteEndElement();
			_writer.WriteEndElement();
		}

		private string GetCellStyle( Cell cell )
		{
			if( CellStyleComparer.HasStyle( cell ) )
			{
				return _styles[cell];
			}
			return null;
		}

		private void WriteCellStyle( Cell cell )
		{
			if( cell.Alignment != Alignment.Default )
			{
				_writer.WriteStartElement( "Alignment" );
				_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Horizontal", cell.Alignment.ToString() );
				_writer.WriteEndElement();
			}
			if( cell.Border != null )
			{
				_writer.WriteStartElement( "Borders" );
				WriteBorderStyle( "Left", cell.Border.Left );
				WriteBorderStyle( "Top", cell.Border.Left );
				WriteBorderStyle( "Right", cell.Border.Left );
				WriteBorderStyle( "Bottom", cell.Border.Left );
				_writer.WriteEndElement();
			}
			if( cell.Font != null || !cell.Foreground.IsEmpty )
			{
				_writer.WriteStartElement( "Font" );
				if( cell.Font != null )
				{
					if( cell.Font.Name != null )
					{
						_writer.WritePrefixedAttribute( SpreadsheetPrefix, "FontName", cell.Font.Name );
					}
					if( cell.Font.Size.HasValue )
					{
						_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Size", cell.Font.Size.ToString() );
					}
					if( cell.Font.Style != FontStyle.Regular )
					{
						if( (cell.Font.Style & FontStyle.Bold) == FontStyle.Bold )
						{
							_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Bold", "1" );
						}
						if( (cell.Font.Style & FontStyle.Italic) == FontStyle.Italic )
						{
							_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Italic", "1" );
						}
						if( (cell.Font.Style & FontStyle.Underline) == FontStyle.Underline )
						{
							_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Underline", "1" );
						}
						if( (cell.Font.Style & FontStyle.Strikeout) == FontStyle.Strikeout )
						{
							_writer.WritePrefixedAttribute( SpreadsheetPrefix, "StrikeThrough", "1" );
						}
					}
				}
				if( !cell.Foreground.IsEmpty )
				{
					_writer.WriteColorAttribute( SpreadsheetPrefix, cell.Foreground );
				}
				_writer.WriteEndElement();
			}
			if( !cell.Background.IsEmpty )
			{
				_writer.WriteStartElement( "Interior" );
				_writer.WriteColorAttribute( SpreadsheetPrefix, cell.Background );
				_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Pattern", "Solid" );
				_writer.WriteEndElement();
			}
		}

		private void WriteBorderStyle( string position, Border border )
		{
			if( !border.IsEmpty )
			{
				_writer.WriteStartElement( "Border" );
				_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Position", position );
				switch( border.Style )
				{
					case LineStyle.Default:
					case LineStyle.Solid:
						_writer.WritePrefixedAttribute( SpreadsheetPrefix, "LineStyle", "Continuous" );
						break;
					default:
						throw new NotImplementedException( string.Format( "Line style '{0}' not supported.", border.Style ) );
				}
				if( border.Weight.HasValue )
				{
					_writer.WritePrefixedAttribute( SpreadsheetPrefix, "Weight", border.Weight.ToString() );
				}
				_writer.WriteEndElement();
			}
		}

		#endregion

		#region CellStyleComparer Class

		private class CellStyleComparer : IEqualityComparer<Cell>
		{
			/// <summary>
			/// Gets a value indicating whether the cell has any style defined.
			/// </summary>
			public static bool HasStyle( Cell cell )
			{
				return cell.Alignment != Alignment.Default || !cell.Background.IsEmpty || cell.Border != null ||
					cell.Font != null || !cell.Foreground.IsEmpty;
			}

			#region IEqualityComparer<Cell> Members

			/// <summary>
			/// Determines whether the specified objects are equal.
			/// </summary>
			public bool Equals( Cell x, Cell y )
			{
				return x.Alignment == y.Alignment && x.Background == y.Background && x.Border.Equals( y.Border ) &&
					x.Font.Equals( y.Font ) && x.Foreground == y.Foreground;
			}

			/// <summary>
			/// Returns a hash code for the specified object.
			/// </summary>
			public int GetHashCode( Cell obj )
			{
				return string.Concat( obj.Alignment, obj.Background, obj.Border, obj.Font, obj.Foreground ).GetHashCode();
			}

			#endregion
		}

		#endregion
	}

	internal static class XmlWriterExtensions
	{
		public static void WritePrefixedAttribute( this XmlWriter writer, string prefix, string localName, string value )
		{
			writer.WriteAttributeString( prefix, localName, value );
		}

		public static void WriteColorAttribute( this XmlWriter writer, string prefix, Color color )
		{
			writer.WritePrefixedAttribute( prefix, "Color", ColorTranslator.ToHtml( color ) );
		}
	}
}

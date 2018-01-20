using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Write a workbook in excel format.
	/// </summary>
	public class Excel2000Writer : IWorkbookWriter
	{
		/// <summary>
		/// Write a workbook in excel format.
		/// </summary>
		/// <param name="workbook">The workbook to write.</param>
		/// <param name="writer">The writer to write to.</param>
		public void Write( Workbook workbook, TextWriter writer )
		{
			writer.WriteLine( "<html xmlns:o=\"urn:schemas-microsoft-com:office:office\"" );
			writer.WriteLine( "      xmlns:x=\"urn:schemas-microsoft-com:office:excel\"" );
			writer.WriteLine( "      xmlns=\"http://www.w3.org/TR/REC-html40\">" );
			writer.WriteLine( "" );
			writer.WriteLine( " <head>" );
			writer.WriteLine( "  <meta http-equiv=Content-Type content=\"text/html;Charset=windows-1252\">" );
			writer.WriteLine( "  <meta name=ProgId content=Excel.Sheet>" );
			writer.WriteLine( "  <meta name=Generator content=\"Microsoft Excel 9\">" );
			writer.WriteLine( "  <!--[if gte mso 9]><xml>" );
			writer.WriteLine( "   <o:DocumentProperties>" );
			writer.WriteLine( "    <o:Author>Smart Excel</o:Author>" );
			writer.WriteLine( "    <o:Created>" + DateTime.Now.ToString( "yyyy-MM-dd\\T\\Oh:mm:ss" ) + "Z</o:Created>" );
			writer.WriteLine( "    <o:Company>Plan B</o:Company>" );
			writer.WriteLine( "    <o:Version>9.4402</o:Version>" );
			writer.WriteLine( "   </o:DocumentProperties>" );
			writer.WriteLine( "   <o:OfficeDocumentSettings>" );
			writer.WriteLine( "    <o:DownloadComponents/>" );
			writer.WriteLine( "    <o:LocationOfComponents HRef=\"file:L:\\BUILDER\\Programs\\Office%202000\\msowc.cab\"/>" );
			writer.WriteLine( "   </o:OfficeDocumentSettings>" );
			writer.WriteLine( "  </xml><![endif]-->" );
			writer.WriteLine( "  <!--[if gte mso 9]><xml>" );
			writer.WriteLine( "   <x:ExcelWorkbook>" );
			writer.WriteLine( "    <x:ProtectStructure>False</x:ProtectStructure>" );
			writer.WriteLine( "    <x:ProtectWindows>False</x:ProtectWindows>" );
			writer.WriteLine( "    <x:ExcelWorksheets>" );

			// This writer only supports a single worksheet.
			Worksheet sheet = workbook.Sheets.FirstOrDefault();
			if( sheet != null )
			{
				writer.WriteLine( "     <x:ExcelWorksheet>" );
				writer.WriteLine( "      <x:Name>" + sheet.Name + "</x:Name>" );
				writer.WriteLine( "      <x:WorksheetOptions>" );
				writer.WriteLine( "       <x:Selected/>" );
				writer.WriteLine( "       <x:Zoom>" + (sheet.Zoom.HasValue ? sheet.Zoom : 100) + "</x:Zoom>" );
				writer.Write( Panes( sheet ) );
				writer.WriteLine( "       <x:ProtectContents>False</x:ProtectContents>" );
				writer.WriteLine( "       <x:ProtectObjects>False</x:ProtectObjects>" );
				writer.WriteLine( "       <x:ProtectScenarios>False</x:ProtectScenarios>" );
				writer.WriteLine( "      </x:WorksheetOptions>" );
				writer.WriteLine( "     </x:ExcelWorksheet>" );
			}

			writer.WriteLine( "    </x:ExcelWorksheets>" );
			writer.WriteLine( "   </x:ExcelWorkbook>" );
			writer.WriteLine( "  </xml><![endif]-->" );
			writer.WriteLine( " </head>" );
			writer.WriteLine( " <body link=blue vlink=purple>" );

			if( sheet != null )
			{
				WriteWorksheet( sheet, writer );
			}

			writer.WriteLine( " </body>" );
			writer.WriteLine( "</html>" );
			writer.Flush();
		}

		private void WriteWorksheet( Worksheet sheet, TextWriter sw )
		{
			sw.WriteLine( "  <table name=\"" + sheet.Name + "\" x:str border=0 cellpadding=0 cellspacing=0 width=64 style='border-collapse:collapse;table-layout:fixed;width:48pt'>" );
			foreach( Column c in sheet.Columns )
			{
				if( c.Width.HasValue )
				{
					if( c.Width == 0 )
					{
						sw.WriteLine( "   <col style=\"display: none\" />" );
					}
					else
					{
						sw.WriteLine( "   <col style=\"width: {0}px;\" />", c.Width );
					}
				}
				else
				{
					sw.WriteLine( "   <col />" );
				}
			}

			foreach( Row r in sheet.Rows )
			{
				if( r == null )
				{
					sw.WriteLine( "   <tr></tr>" );
				}
				else
				{
					if( r.Height == 0 )
					{
						sw.WriteLine( "   <tr style=\"display:none\">" );
					}
					else if( r.Height > 0 )
					{
						sw.WriteLine( "   <tr style=\"height: {0}px;\">", r.Height );
					}
					else
					{
						sw.WriteLine( "   <tr>" );
					}
					foreach( Cell ec in r )
					{
						if( ec != null )
						{
							sw.WriteLine( "    " + GetCellHtml( ec ) );
						}
						else
						{
							sw.WriteLine( "    <td></td>" );
						}
					}
					sw.WriteLine( "   </tr>" );
				}
			}
			sw.WriteLine( "  </table>" );
		}

		private string GetCellHtml( Cell cell )
		{
			string str = GetStyle( cell );

			str = (str.Length == 0 ? "" : " style=\"" + str + "\"");
			str = (cell.ColumnSpan > 1 ? " colspan=" + cell.ColumnSpan : "") + str;
			str = (cell.RowSpan > 1 ? " rowspan=" + cell.RowSpan : "") + str;
			str = (cell.Column.Width >= 0 ? " width=" + cell.Column.Width : "") + str;
			str = (cell.Row.Height >= 0 ? " height=" + cell.Row.Height : "") + str;
			switch( cell.Datatype )
			{
				case Datatype.Boolean:
					str += " x:bool";
					break;
				case Datatype.Number:
					// If the cell value is set here and the data is not numerical excel will throw an error.
					str += " x:num";
					break;
				case Datatype.String:
					str += " x:str";
					break;
				default:
					if( cell.Value is bool )
					{
						str += " x:bool";
					}
					else if( cell.Value is string || cell.Value is char )
					{
						str += " x:str";
					}
					else if( cell.IsNumeric )
					{
						// Set the cell value as the type is guaranteed.
						str += String.Format( " x:num=\"{0}\"", cell.Value );
					}
					break;
			}

			return "<td" + str + ">" + (cell.Value != null ? cell.Value.ToString() : "") + "</td>";
		}

		private string GetStyle( Cell cell )
		{
			StringBuilder sb = new StringBuilder();
			if( cell.Alignment != Alignment.Default )
			{
				sb.Append( "text-align: " + cell.Alignment.ToString() + ";" );
			}
			if( !cell.Background.IsEmpty )
			{
				sb.Append( " background-color: " + ColorTranslator.ToHtml( cell.Background ) + ";" );
			}

			if( !cell.Border.Top.IsEmpty )
			{
				sb.AppendFormat( " border-top: {0};", GetBorderStyle( cell.Border.Top ) );
			}
			if( !cell.Border.Right.IsEmpty )
			{
				sb.AppendFormat( " border-right: {0};", GetBorderStyle( cell.Border.Right ) );
			}
			if( !cell.Border.Bottom.IsEmpty )
			{
				sb.AppendFormat( " border-botom: {0};", GetBorderStyle( cell.Border.Bottom ) );
			}
			if( !cell.Border.Left.IsEmpty )
			{
				sb.AppendFormat( " border-left: {0};", GetBorderStyle( cell.Border.Left ) );
			}

			if( !string.IsNullOrEmpty( cell.CellFormat ) )
			{
				sb.Append( " mso-number-format: '" + cell.CellFormat + "';" );
			}
			if( cell.Font != null )
			{
				if( cell.Font.Style == FontStyle.Bold )
				{
					sb.Append( " font-weight: bold;" );
				}
				if( !string.IsNullOrEmpty( cell.Font.Name ) )
				{
					sb.Append( " font-family: " + cell.Font.Name + ";" );
				}
				if( cell.Font.Size != -1 )
				{
					sb.Append( " font-size: " + cell.Font.Size + "pt;" );
				}
			}
			if( !cell.Foreground.IsEmpty )
			{
				sb.Append( " color: " + ColorTranslator.ToHtml( cell.Foreground ) + ";" );
			}

			return sb.ToString();
		}

		private string GetBorderStyle( Border border )
		{
			StringBuilder format = new StringBuilder();
			format.AppendFormat( "{0} {1}", border.Weight, border.Style.ToString() );
			if( border.Color != Color.Empty )
			{
				format.Append( " " + ColorTranslator.ToHtml( border.Color ) );
			}
			return format.ToString();
		}

		private string Panes( Worksheet sheet )
		{
			StringBuilder sb = new StringBuilder();
			if( sheet.FrozenX.HasValue || sheet.FrozenY.HasValue )
			{
				sb.AppendLine( "      <x:FreezePanes />" );
				sb.AppendLine( "      <x:FrozenNoSplit />" );
				if( sheet.FrozenX.HasValue )
				{
					sb.AppendLine( "      <x:SplitVertical>" + sheet.FrozenX + "</x:SplitVertical>" );
					sb.AppendLine( "      <x:LeftColumnRightPane>" + sheet.FrozenX + "</x:LeftColumnRightPane>" );
				}
				if( sheet.FrozenY.HasValue )
				{
					sb.AppendLine( "      <x:SplitHorizontal>" + sheet.FrozenY + "</x:SplitHorizontal>" );
					sb.AppendLine( "      <x:TopRowBottomPane>" + sheet.FrozenY + "</x:TopRowBottomPane>" );
				}
				if( sheet.FrozenX.HasValue && sheet.FrozenY.HasValue )
				{
					sb.AppendLine( "      <x:ActivePane>0</x:ActivePane>" );
					sb.AppendLine( "      <x:Panes>" );
					sb.AppendLine( " 	   <x:Pane>" );
					sb.AppendLine( "        <x:Number>3</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "       <x:Pane>" );
					sb.AppendLine( "        <x:Number>1</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "       <x:Pane>" );
					sb.AppendLine( "        <x:Number>2</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "       <x:Pane>" );
					sb.AppendLine( "        <x:Number>0</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "      </x:Panes>" );
				}
				else if( sheet.FrozenX.HasValue )
				{
					sb.AppendLine( "      <x:ActivePane>1</x:ActivePane>" );
					sb.AppendLine( "      <x:Panes>" );
					sb.AppendLine( "       <x:Pane>" );
					sb.AppendLine( "        <x:Number>3</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "       <x:Pane>" );
					sb.AppendLine( "        <x:Number>1</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "      </x:Panes>" );
				}
				else
				{
					sb.AppendLine( "      <x:ActivePane>2</x:ActivePane>" );
					sb.AppendLine( "      <x:Panes>" );
					sb.AppendLine( "       <x:Pane>" );
					sb.AppendLine( "        <x:Number>3</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "       <x:Pane>" );
					sb.AppendLine( "        <x:Number>2</x:Number>" );
					sb.AppendLine( "       </x:Pane>" );
					sb.AppendLine( "      </x:Panes>" );
				}
			}
			else
			{
				sb.AppendLine( "      <x:Panes>" );
				sb.AppendLine( "       <x:Pane>" );
				sb.AppendLine( "        <x:Number>3</x:Number>" );
				sb.AppendLine( "       </x:Pane>" );
				sb.AppendLine( "      </x:Panes>" );
			}
			return sb.ToString();
		}
	}
}

using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SqlExport.Export.Adapters.Common;
using System;
using System.IO;

namespace SqlExport.Export.Adapters
{
	/// <summary>
	/// Exports data in Microsoft Excel format.
	/// </summary>
	internal class Excel : ExporterBase
	{
		public override string Name
		{
			get { return "Excel"; }
		}

		/// <summary>
		/// Gets the dialog filename filter.
		/// </summary>
		public override string FileFilter
		{
			get { return "Excel Files (*.xls)|*.xls|All Files (*.*)|*.*"; }
		}

		/// <summary>
		/// Gets a control which allows setting of the export options.
		/// </summary>
		public override Control GetOptionPanel()
		{
			return null;
		}

		/// <summary>
		/// Exports the data to the supplied file.
		/// </summary>
		public void Export( DataTable data, string filename )
		{
			Workbook wb = new Workbook();
			wb.Author = System.Environment.UserName;

			wb.AddWorksheet( data.TableName );

			Status = "Creating Excel Spreadsheet";

			for( int i = 0; i < data.Columns.Count; i++ )
			{
				Cell c = wb.CurrentSheet[0][i];
				c.Value = data.Columns[i].ColumnName;
				c.Background = Color.FromArgb( 220, 220, 220 );
				c.Border = new CellBorder( Border.Empty, Border.Empty, Border.Empty, Border.Thin );
				c.Font = new CellFont() { Style = FontStyle.Bold };
			}

			for( int row = 0; row < data.Rows.Count; row++ )
			{
				for( int column = 0; column < data.Columns.Count; column++ )
				{
					Cell c = wb.CurrentSheet[row + 1][column];
					c.Value = data.Rows[row][column];
					switch( data.Rows[row][column].GetType().Name )
					{
						case "Int32":
						case "Double":
						case "Decimal":
							c.Datatype = Datatype.Number;
							break;
					}
				}
			}

			wb.CurrentSheet.FrozenY = 1;

			wb.SaveFile( filename, new ExcelWriter() );
		}

		public void Export( DataTable data, TextWriter writer )
		{
			throw new NotImplementedException();
		}
	}
}

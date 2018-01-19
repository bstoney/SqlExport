using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using SqlExport.Data;

namespace SqlExport.Export.Adapters
{
	internal class Xml : ExporterBase
	{
		public override string Name
		{
			get { return "Xml"; }
		}

		public override string FileFilter
		{
			get { return "Excel Files (*.xls)|*.xls|All Files (*.*)|*.*"; }
		}

		public override Control GetOptionPanel()
		{
			return null;
		}

		public void Export( DataResult data, TextWriter writer )
		{
			throw new NotImplementedException();
		}

		public void Export( DataTable data, string filename )
		{
			//System.IO.FileInfo fi;
			//int i;
			//int j;
			//int start;
			//clsExcel ex = new clsExcel();
			//DataTable t = (DataTable)frm.grdResults.DataSource;

			//fi = new System.IO.FileInfo( ExportSaveFilename );
			//if( fi.Exists ) // the file must be deleted
			//{
			//    try
			//    {
			//        fi.Delete();
			//    }
			//    catch( Exception exp )
			//    {
			//        Interaction.MsgBox( exp.Message, MsgBoxStyle.Critical, "Error" );
			//    }
			//}

			//frm.lblStatus.Text = "Creating XML Spreadsheet";
			//frm.prbProgress.Maximum = t.Rows.Count;
			//System.Windows.Forms.Application.DoEvents();

			//ex.AddWorkSheet( ExportSaveFilename.Substring( Strings.InStrRev( ExportSaveFilename, "\\", -1, 0 ) + 1 - 1, Strings.InStr( Strings.InStrRev( ExportSaveFilename, "\\", -1, 0 ) + 1, ExportSaveFilename, ".", 0 ) - Strings.InStrRev( ExportSaveFilename, "\\", -1, 0 ) - 1 ), t.Columns.Count );
			//ex.Magnification = 100;
			//if( System.Convert.ToBoolean( GetOption( OptionType.ExportHeaders, null ) ) )
			//{
			//    ex.FreezePanes( 0, 1 );
			//    for( i = 0; i <= t.Columns.Count - 1; i++ )
			//    {
			//        ex.cell( 0, i ).Value = t.Columns[i].ColumnName;
			//        ex.cell( 0, i ).Background = System.Drawing.ColorTranslator.FromOle( Information.RGB( 220, 220, 220 ) );
			//        ex.cell( 0, i ).Border = CellType.BorderType.bdrBottom;
			//        ex.cell( 0, i ).FontBold = true;
			//    }
			//    start = 1;
			//}
			//else
			//{
			//    start = 0;
			//}

			//for( i = 0; i <= t.Rows.Count - 1; i++ )
			//{
			//    if( i % 10 == 0 )
			//    {
			//        frm.prbProgress.Value = i;
			//    }
			//    for( j = 0; j <= t.Columns.Count - 1; j++ )
			//    {
			//        ex.cell( i + start, j ).Value = (Information.IsDBNull( t.Rows[i][j] )) ? "" : (t.Rows[i][j].ToString());
			//    }
			//}
			//frm.lblStatus.Text = "Saving Spreadsheet";
			//System.Windows.Forms.Application.DoEvents();
			//ex.SaveFile( ExportSaveFilename );

			//ex = null;

			//frm.prbProgress.Value = 0;
			//frm.lblStatus.Text = "";
			//System.Windows.Forms.Application.DoEvents();
		}

		public void Export( DataTable data, TextWriter writer )
		{
			throw new NotImplementedException();
		}
	}
}

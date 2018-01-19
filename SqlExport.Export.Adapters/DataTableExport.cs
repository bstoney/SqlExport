using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace SqlExport.Export.Adapters
{
	public class DataTableExport : ExporterBase
	{
		public override string Name
		{
			get { return "DataTableExport"; }
		}

		public override string FileFilter
		{
			get { return "CS Files (*.cs)|*.cs|All Files (*.*)|*.*"; ; }
		}

		public void Export( DataTable data, TextWriter writer )
		{
			string tableName = GetSaveVariableName( data.TableName );
			writer.WriteLine( "DataTable {0} = new DataTable( \"{0}\" );", tableName );
			foreach( DataColumn column in data.Columns )
			{
				writer.WriteLine( "{0}.Columns.Add( \"{1}\", typeof( {2} ) );",
					tableName, column.ColumnName, column.DataType.Name );
			}

			foreach( DataRow row in data.Rows )
			{
				writer.WriteLine( "{0}.Rows.Add( {1} );", tableName,
					string.Join( ", ", row.ItemArray.Select( i => FormatItem( i ) ).ToArray() ) );
			}
		}

		private string FormatItem( object item )
		{
			if( item == null || item is DBNull )
			{
				return "null";
			}
			else
			{
				switch( item.GetType().Name )
				{
					case "String":
						return "\"" + item + "\"";
					case "Char":
						return "'" + item + "'";
					case "Boolean":
						return item.ToString().ToLower();
					case "DateTime":
						return "DateTime.Parse( \"" + item.ToString() + "\" )";
					default:
						// TODO better stuff.
						return item.ToString();
				}
			}
		}

		private string GetSaveVariableName( string variable )
		{
			// TODO better stuff.
			return variable.Replace( ' ', '_' );
		}

		public override Control GetOptionPanel()
		{
			return null;
		}
	}
}

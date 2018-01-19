using System;
using System.Collections.Generic;
using System.IO;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Represents a collection of work spaces.
	/// </summary>
	public class Workbook
	{
		private List<Worksheet> _sheets;
		private int _currentSheetIndex;

		/// <summary>
		/// Initializes a new instance of the Workbook class.
		/// </summary>
		public Workbook()
		{
			_sheets = new List<Worksheet>();
			_currentSheetIndex = -1;
		}

		/// <summary>
		/// Gets an array of sheets for this workbook.
		/// </summary>
		public Worksheet[] Sheets
		{
			get { return _sheets.ToArray(); }
		}

		/// <summary>
		/// Gets or sets the current worksheet index.
		/// </summary>
		public int CurrentSheetIndex
		{
			get
			{
				return _currentSheetIndex;
			}

			set
			{
				if( value < 0 || value >= _sheets.Count )
				{
					throw new ArgumentOutOfRangeException( "CurrentSheetIndex" );
				}

				_currentSheetIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets the current worksheet.
		/// </summary>
		public Worksheet CurrentSheet
		{
			get
			{
				return _sheets[_currentSheetIndex];
			}

			set
			{
				int index = _sheets.IndexOf( value );
				if( index < 0 )
				{
					throw new ArgumentOutOfRangeException( "CurrentSheet", "The worksheet is not valid for this workbook." );
				}

				_currentSheetIndex = index;
			}
		}

		/// <summary>
		/// Gets or sets the document author.
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Add a worksheet to this workbook and set it to be current. Returns the index of the new work sheet.
		/// </summary>
		/// <param name="name">Name of the new worksheet.</param>
		public Worksheet AddWorksheet( string name )
		{
			Worksheet sheet = new Worksheet( name, _sheets.Count );
			_sheets.Add( sheet );
			_currentSheetIndex = sheet.Index;
			return sheet;
		}

		#region Output

		/// <summary>
		/// Save the workbook to file.
		/// </summary>
		/// <param name="filename">Filename and path to use; an existing file will be overwritten.</param>
		public void SaveFile( string filename )
		{
			SaveFile( filename, new Excel2000Writer() );
		}

		/// <summary>
		/// Save the workbook to file.
		/// </summary>
		/// <param name="filename">Filename and path to use; an existing file will be overwritten.</param>
		/// <param name="workbookWriter">The work book writer to use for output.</param>
		public void SaveFile( string filename, IWorkbookWriter workbookWriter )
		{
			using( StreamWriter sw = new StreamWriter( new FileStream( filename, FileMode.Create, FileAccess.Write ) ) )
			{
				WriteDocument( sw, workbookWriter );
			}
		}

		/// <summary>
		/// Gets a new byte array containing the excel document.
		/// </summary>
		public byte[] GetDocumentBytes( IWorkbookWriter workbookWriter )
		{
			using( MemoryStream ms = new MemoryStream() )
			{
				using( StreamWriter sw = new StreamWriter( ms ) )
				{
					WriteDocument( sw, workbookWriter );
					sw.Flush();
				}
				ms.Flush();
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Writes the workbook to the text writer.
		/// </summary>
		public void WriteDocument( TextWriter writer, IWorkbookWriter workbookWriter )
		{
			workbookWriter.Write( this, writer );
		}

		#endregion
	}
}

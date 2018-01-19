using System;
using System.IO;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// A writer to output a workbook in a custom format.
	/// </summary>
	public interface IWorkbookWriter 
	{
		/// <summary>
		/// Write the workbook to the text stream.
		/// </summary>
		void Write( Workbook workbook, TextWriter writer );
	}
}

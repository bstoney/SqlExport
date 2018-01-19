using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal class CodeLine
	{
		private List<SourceLine> _lines = new List<SourceLine>();

		public int LineNumber { get; set; }

		public SourceLine Line
		{
			get { return Lines.First(); }
		}

		/// <summary>
		/// Gets or sets the main code line.
		/// </summary>
		public IEnumerable<SourceLine> Lines
		{
			get { return _lines; }
		}

		/// <summary>
		/// Gets all code lines (excludes blank and comment lines).
		/// </summary>
		public IEnumerable<SourceLine> CodeLines
		{
			get { return Lines.Where( l => !(l.IsBlank || l.IsComment) ); }
		}

		public void Add( SourceLine line )
		{
			_lines.Add( line );
		}

		public int? LineNumberOf( string text )
		{
			// Try to find the text in the lines.
			var line = Lines.Select( (l,i) => new { L = l, I = i } ).First( l => !l.L.IsBlank && !l.L.IsComment && l.L.Text.Contains( text ) );
			
			// If it's found return the line number + offset otherwise return null.
			return line != null ? (int?)(LineNumber + line.I) : null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.ViewModel
{
	public class SelectedTextRange
	{
		public SelectedTextRange( int startLine, int startCharacter, int endLine, int endCharacter )
		{
			this.StartLine = startLine;
			this.StartCharacter = startCharacter;
			this.EndLine = endLine;
			this.EndCharacter = endCharacter;
		}

		public int StartLine { get; private set; }

		public int StartCharacter { get; private set; }

		public int EndLine { get; private set; }

		public int EndCharacter { get; private set; }
	}
}

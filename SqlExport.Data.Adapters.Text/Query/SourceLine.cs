using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal class SourceLine
	{
		private const string CommentCharacter = "--";
		private static readonly char[] Whitespace = new char[] { ' ', '\t', '\n', '\r' };
		
		public SourceLine( string text )
		{
			Text = text;
			IsBlank = Text.Trim().Length == 0;
			IsComment = !IsBlank && Text.TrimStart().StartsWith( CommentCharacter );
			IsContinuation = !IsBlank && Whitespace.Any( c => c == Text[0] );
		}

		public string Text { get; private set; }

		public bool IsComment { get; private set; }

		public bool IsBlank { get; private set; }

		public bool IsContinuation { get; private set; }

		public static implicit operator string( SourceLine line )
		{
			return line.Text;
		}

		public static implicit operator SourceLine( string line )
		{
			return line != null ? new SourceLine( line ) : null;
		}

		public override string ToString()
		{
			return Text;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Custom XmlWriter to allow writing to document fragments to different files.
	/// </summary>
	public sealed class XmlWriter : IDisposable
	{
		private TextWriter _writer;
		private Stack<string> _elementStack;
		private bool _isWritingElement;
		private bool _needsNewLine;

		/// <summary>
		/// Initializes a new instance of the XmlWriter class.
		/// </summary>
		public XmlWriter( TextWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( "writer" );
			}

			_writer = writer;
			_elementStack = new Stack<string>();
		}

		/// <summary>
		/// Gets or sets the current writer indent level.
		/// </summary>
		public int Indent { get; set; }

		/// <summary>
		/// Gets or sets the string used to indent.
		/// </summary>
		public string IndentCharacters { get; set; }

		/// <summary>
		/// Gets or sets the string used for a new line.
		/// </summary>
		public string NewLineCharacters { get; set; }

		private void NewLine()
		{
			if( _needsNewLine )
			{
				WriteNewLine();
			}
		}

		private void Write( params string[] args )
		{
			args.ToList().ForEach( arg => _writer.Write( arg ) );
		}

		private void EnsureElementStartEnd()
		{
			if( _isWritingElement )
			{
				Write( ">" );
				_isWritingElement = false;
				_needsNewLine = true;
			}
		}

		/// <summary>
		/// Writes the NewLineCharacters and the Indent multiple of the IndentCharacters.
		/// </summary>
		public void WriteNewLine()
		{
			Write( NewLineCharacters );
			for( int i = 0; i < Indent; i++ )
			{
				Write( IndentCharacters );
			}

			_needsNewLine = false;
		}

		/// <summary>
		/// Writes the standard xml document header.
		/// </summary>
		public void WriteStartDocument()
		{
			NewLine();
			Write( "<?xml version=\"1.0\"?>" );
		}

		/// <summary>
		/// Writes a processing instruction to the document.
		/// </summary>
		public void WriteProcessingInstruction( string name, string value )
		{
			NewLine();
			Write( "<?", name, " ", value, "?>" );
		}

		/// <summary>
		/// Writes a new element with the supplied name.
		/// </summary>
		public void WriteStartElement( string name )
		{
			EnsureElementStartEnd();
			NewLine();
			Write( "<", name );
			_elementStack.Push( name );
			_isWritingElement = true;
			Indent++;
		}

		/// <summary>
		/// Writes a new element with the supplied name and namespace.
		/// </summary>
		public void WriteStartElement( string name, string ns )
		{
			WriteStartElement( name );
			WriteAttributeString( "xmlns", ns );
		}

		/// <summary>
		/// Writes a complete element start and end tag pair with the supplied name and inner text.
		/// </summary>
		public void WriteElementString( string name, string value )
		{
			WriteStartElement( name );
			EnsureElementStartEnd();
			_needsNewLine = false;
			WriteString( value );
			_needsNewLine = false;
			WriteEndElement();
		}

		/// <summary>
		/// Writes the end tag or closes the current element. 
		/// </summary>
		public void WriteEndElement()
		{
			Indent--;
			string name = _elementStack.Pop();
			if( _isWritingElement )
			{
				Write( " />" );
			}
			else
			{
				NewLine();
				Write( "</", name, ">" );
			}

			_isWritingElement = false;
			_needsNewLine = true;
		}

		/// <summary>
		/// Writes a complete attribute with the supplies name and value.
		/// </summary>
		public void WriteAttributeString( string name, string value )
		{
			Write( " ", name, "=\"", value, "\"" );
		}

		/// <summary>
		/// Writes a complete attribute with the supplied prefix, name and value.
		/// </summary>
		public void WriteAttributeString( string prefix, string name, string value )
		{
			if( prefix == null )
			{
				WriteAttributeString( name, value );
			}
			else
			{
				Write( " ", prefix, ":", name, "=\"", value, "\"" );
			}
		}

		/// <summary>
		/// Writes a literal string.
		/// </summary>
		public void WriteString( string value )
		{
			EnsureElementStartEnd();
			Write( value );
			// Strings must not have additional characters on either side.
			_needsNewLine = false;
		}

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_writer.Flush();
		}

		#endregion
	}
}

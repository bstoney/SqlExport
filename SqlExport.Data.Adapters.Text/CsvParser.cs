using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace SqlExport.Data.Adapters.Text
{
	/// <summary>
	/// Parses CSV files one line at a time. Each line is returned as an array of strings.
	/// <remarks>
	/// Based on a description of the file format found at http://www.creativyst.com/Doc/Articles/CSV/CSV01.htm
	/// </remarks>
	/// </summary>
	public class CsvParser
	{
		private readonly TextReader _reader;
		private List<string> _currentLine;

		/// <summary>
		/// Parses CSV files one line at a time. Each line is returned as an array of strings.
		/// </summary>
		public CsvParser( TextReader reader )
			: this( reader, true )
		{
		}

		/// <summary>
		/// Parses CSV files one line at a time. Each line is returned as an array of strings.
		/// </summary>
		/// <param name="reader">The reader to read text from.</param>
		public CsvParser( TextReader reader, bool columnNamesOnFirstLine )
		{
			_reader = reader;
			ColumnNames = GetColumnNames( columnNamesOnFirstLine );
		}

		public IEnumerable<string> ColumnNames { get; set; }

		private IEnumerable<string> GetColumnNames( bool columnNamesOnFirstLine )
		{
			Dictionary<string, int> columns = new Dictionary<string, int>();
			foreach( var item in ReadLine().Select( ( c, i ) => new { c, i } ) )
			{
				// Store the original column name, using "Column #" for columns with blank names.
				string originalName = string.IsNullOrEmpty( item.c ) || !columnNamesOnFirstLine ? "Column " + item.i : item.c;
				string name = originalName;

				// Make sure the column name hasn't already been used.
				while( columns.ContainsKey( name ) )
				{
					// Increment the number of times this column name has been used.
					columns[name]++;

					// Try to make the column name unique by appending the usage count to the end.
					name = originalName + " " + columns[name];
				}

				// Add the column name to the dictionary of previously used name.
				columns[name] = 1;

				yield return name;
			}
		}

		/// <summary>
		/// Returns the next char in the stream without consuming it or '\0' if there are no more chars.
		/// </summary>
		private char NextChar
		{
			get
			{
				int c = _reader.Peek();
				if( c >= 0 )
				{
					return (char)c;
				}
				return (char)0;
			}
		}

		/// <summary>
		/// Consumes the next character in the stream.
		/// </summary>
		private void Read()
		{
			_reader.Read();
		}

		/// <summary>
		/// Read the next field from the stream.
		/// </summary>
		private string ReadField()
		{
			// Trim leading whitespace.
			ReadWhitespace();

			StringBuilder sb = new StringBuilder();

			// If the field is enclosed in quotes it can contain newlines or '""' 
			bool escaped = false;
			if( NextChar == '"' )
			{
				escaped = true;
				// Consume the escaping quote.
				Read();
			}

			char c = NextChar;
			while( true )
			{
				if( c == 0 )
				{
					// EOF
					return null;
				}

			    if( escaped )
			    {
			        if( c == '"' )
			        {
			            // Consume the quote.
			            this.Read();
			            if( this.NextChar == '"' )
			            {
			                // Escaped "
			                sb.Append( c );
			            }
			            else
			            {
			                // End of field.
			                // If the field is the last in the row, consume the \r before exiting.
			                if( this.NextChar == '\r' )
			                {
			                    // Consume the \r and exit.
			                    this.Read();
			                }

			                break;
			            }
			        }
			        else
			        {
			            // Char is content.
			            sb.Append( c );
			        }
			    }
			    else
			    {
			        if( c == '\r' )
			        {
			            // Consume the \r and exit.
			            this.Read();
			            break;
			        }
			        if( c == '\n' || c == ',' )
			        {
			            // Exit and let the char be consumed by ReadComma or ReadLine.
			            break;
			        }
			        sb.Append( c );
			    }
			    // Process the next char.
				Read();
				c = NextChar;
			}

			string line = sb.ToString();

			if( !escaped )
			{
				// Trim any trailing whitespace.
				line = line.TrimEnd();
			}

			// Discard any trailing whitespace after the escaping quote.
			ReadWhitespace();

			return line;
		}

		/// <summary>
		/// Consumes whitespace.
		/// </summary>
		private void ReadWhitespace()
		{
			while( NextChar == ' ' )
			{
				Read();
			}
		}

		/// <summary>
		/// Consume the next char if it is a comma.
		/// </summary>
		/// <returns>True if the char was consumed.</returns>
		private bool ReadComma()
		{
			if( NextChar == ',' )
			{
				Read();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Reads the next line from the CSV.
		/// </summary>
		/// <returns>An array of strings containing the fields from the next line of data or null if at EOF.</returns>
		public string[] ReadLine()
		{
			// At EOF?
			if( NextChar == 0 )
			{
				return null;
			}

			_currentLine = new List<string>();

			// Read until we're at a newline.
			while( NextChar != '\n' )
			{
				// Read the next field.
				string field = ReadField();
				if( field != null )
				{
					_currentLine.Add( field );
				}
				// Next char should be a comma.
				if( !ReadComma() )
				{
					break;
				}
			}

			// Consume the last char of the line.
			Read();

			// Done.
			return _currentLine.ToArray();
		}

		/// <summary>
		/// Reads all the lines from the Csv.
		/// </summary>
		public IEnumerable<string[]> ReadAllLines()
		{
			string[] line;
			while( (line = ReadLine()) != null )
			{
				yield return line;
			}
		}
	}
}

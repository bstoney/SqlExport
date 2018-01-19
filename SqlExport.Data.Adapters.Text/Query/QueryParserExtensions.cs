using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal static class QueryParserExtensions
	{
		private const char WordSeparator = ' ';
		private const char WordGroupCharacter = '"';

		public static IEnumerable<CodeLine> GetLines( this TextReader reader )
		{
			CodeLine codeLine = null;
			int lineNumber = 0;
			SourceLine line = null;
			while( (line = reader.ReadLine()) != null )
			{
				lineNumber++;

				// Blank lines and comments are do not have continuations and can be safely returned
				if( codeLine != null && (codeLine.Line.IsBlank || codeLine.Line.IsComment || !line.IsContinuation) )
				{
					yield return codeLine;
					codeLine = null;
				}

				if( codeLine == null )
				{
					codeLine = new CodeLine()
					{
						LineNumber = lineNumber,
					};
				}

				codeLine.Add( line );
			}

			if( codeLine != null )
			{
				yield return codeLine;
			}
		}

		public static IEnumerable<CodeLine> GetLines( this string rules )
		{
			using( StringReader reader = new StringReader( rules ) )
			{
				return reader.GetLines();
			}
		}

		public static IEnumerable<string> GetWords( this string line )
		{
			return line.GetTokens( WordSeparator, WordGroupCharacter );
		}

		public static QueryPart<Selection> GetSelection( this CodeLine codeLine )
		{
			IEnumerable<string> words = codeLine.CodeLines.SelectMany( l => l.Text.Trim().GetWords() );
			if( string.Compare( words.FirstOrDefault(), "SELECT", true ) != 0 )
			{
				throw new QueryParserException( "Missing SELECT" );
			}

			return new QueryPart<Selection>()
			{
				Part = new Selection( words.Skip( 1 ) ) { CodeLine = codeLine },
				Rest = Enumerable.Empty<string>(),
				CodeLine = codeLine
			};
		}

		public static QueryPart<From> GetFrom( this CodeLine codeLine )
		{
			IEnumerable<string> words = codeLine.Line.Text.GetWords();
			if( string.Compare( words.FirstOrDefault(), "FROM", true ) != 0 )
			{
				throw new QueryParserException( "Missing FROM" );
			}

			return new QueryPart<From>()
			{
				Part = new From( words.Second(), words.Last() ) { CodeLine = codeLine },
				Rest = words.Skip( 2 ),
				CodeLine = codeLine
			};
		}

		public static QueryPart<Where> GetWhere( this CodeLine codeLine )
		{
			IEnumerable<string> words = codeLine.Line.Text.GetWords();
			if( string.Compare( words.FirstOrDefault(), "WHERE", true ) != 0 )
			{
				return null;
			}

			words = words.Skip( 1 );
			if( words.Count() != 3 )
			{
				throw new QueryParserException( "Invalid WHERE clause." );
			}

			return new QueryPart<Where>()
			{
				Part = new Where( words.First(), words.Second(), words.Last() ) { CodeLine = codeLine },
				Rest = Enumerable.Empty<string>(),
				CodeLine = codeLine
			};
		}

		public static bool IsUnion( this CodeLine codeLine )
		{
			IEnumerable<string> words = codeLine.Line.Text.GetWords();
			return string.Compare( words.FirstOrDefault(), "UNION", true ) == 0;
		}

		public static T Second<T>( this IEnumerable<T> list )
		{
			return list.Skip( 1 ).First();
		}

		/// <summary>
		/// Performs a case insensitive comparison.
		/// </summary>
		public static bool IsEquivilantTo( this string word1, string word )
		{
			return string.Compare( word1, word, true ) == 0;
		}

		public static IEnumerable<string> GetTokens( this string value, char tokenSeparator, char tokenBoundry )
		{
			string wordBuffer = null;
			string boundry = tokenBoundry.ToString();
			foreach( var item in value.Split( tokenSeparator ) )
			{
				if( wordBuffer == null )
				{
					// Nothing exists in the word buffer.
					if( item.StartsWith( boundry ) )
					{
						if( item.Length > 1 && item.EndsWith( boundry ) )
						{
							// A Word grouping character has been found at the start and end.
							yield return item.Substring( 1, item.Length - 2 );
						}
						else
						{
							// A Word grouping character has been found, start building the word buffer.
							wordBuffer = item.Substring( 1 );
						}
					}
					else
					{
						yield return item;
					}
				}
				else
				{
					// There is currently text in the word buffer.
					if( item.EndsWith( boundry ) )
					{
						// The ending character has been found, complete the word buffer and return it.
						wordBuffer += tokenSeparator + item.Substring( 0, item.Length - 1 );
						yield return wordBuffer;
						wordBuffer = null;
					}
					else
					{
						// No ending character has been found yet, continue to build the word buffer.
						wordBuffer += tokenSeparator + item;
					}
				}
			}
		}
	}
}

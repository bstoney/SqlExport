using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal class QueryParser
	{
		public event Action<QueryParser, string, int> ParseError;

		public QueryParser( TextReader rules )
		{
			Rules = rules;
		}

		public TextReader Rules { get; set; }

		public bool HasParseErrors { get; private set; }

		protected virtual void OnParseError( string message, int lineNumber )
		{
			HasParseErrors = true;
			if( ParseError != null )
			{
				ParseError( this, string.Format( "{0}: Line {1}.", message.TrimEnd( '.' ), lineNumber ), lineNumber );
			}
		}

		public IEnumerable<Query> Parse()
		{
			var codeLines = Rules.GetLines().Where( l => !l.Line.IsComment && !l.Line.IsBlank );
			int ruleIndex = 1;
			var e = new EnumeratorReader<CodeLine>( codeLines.GetEnumerator() );
			Union currentUnion = null;
			while( e.HasNext )
			{
				Query query = null;
				QueryPart<Selection> selection = null;
				while( e.HasNext && selection == null )
				{
					try
					{
						e.MoveNext();
						selection = e.Current.GetSelection();
					}
					catch( Exception ex )
					{
						OnParseError( ex.Message, e.Current.LineNumber );
					}
				}

				if( selection != null )
				{
					try
					{
						if( !e.MoveNext() )
						{
							throw new QueryParserException( "Missing FROM" );
						}

						QueryPart<From> from = e.Current.GetFrom();

						QueryPart<Where> condition = e.Next.GetWhere();
						if( condition != null )
						{
							e.MoveNext();
						}

						query = new Query()
						{
							QueryIndex = ruleIndex,
							Selection = selection.Part,
							From = from.Part,
							Where = condition != null ? condition.Part : Where.Empty
						};
					}
					catch( QueryParserException ex )
					{
						OnParseError( ex.Message, e.Current.LineNumber );
					}
				}

				if( e.HasNext && e.Next.IsUnion() )
				{
					if( currentUnion == null )
					{
						currentUnion = new Union()
						{
							QueryIndex = ruleIndex
						};
					}

					if( query != null )
					{
						currentUnion.QueryParts.Add( query );
					}

					e.MoveNext();
					if( !e.HasNext )
					{
						throw new QueryParserException( "Missing SELECT" );
					}
				}
				else if( currentUnion != null )
				{
					if( query != null )
					{
						currentUnion.QueryParts.Add( query );
					}

					yield return currentUnion;
					currentUnion = null;
					ruleIndex++;
				}
				else
				{
					if( query != null )
					{
						yield return query;
						ruleIndex++;
					}
				}
			}
		}

		private class EnumeratorReader<T>
		{
			private IEnumerator<T> _e;
			private T _current;
			private T _next;
			private bool _hasNext;
			private bool _hasCurrent;

			public EnumeratorReader( IEnumerator<T> enumerator )
			{
				_e = enumerator;
				_current = default( T );
				_hasCurrent = false;
				_hasNext = _e.MoveNext();

				if( _hasNext )
				{
					_next = _e.Current;
				}
			}

			public T Current
			{
				get { return _current; }
			}

			public bool HasNext
			{
				get { return _hasNext; }
			}

			public T Next
			{
				get { return _next; }
			}

			public bool MoveNext()
			{
				if( _hasNext )
				{
					_current = _next;
					_hasCurrent = true;
				}

				if( _hasNext )
				{
					_hasNext = _e.MoveNext();

					if( _hasNext )
					{
						_next = _e.Current;
					}
				}
				else if( _hasCurrent )
				{
					_hasCurrent = false;
				}

				return _hasCurrent;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal class QueryRunner
	{
		private Query _query;
		private IEnumerable<object[]> _lines;
		private string[] _destColumnNames;
		private Dictionary<string, Func<object[], int, object>> _valueMap;

		public QueryRunner( Query query, ConnectionString connectionString )
		{
			_query = query;
			ConnectionString = connectionString;
		}

		public ConnectionString ConnectionString { get; private set; }

		public string Name
		{
			get { return _query.From.Alias; }
		}

		public string[] DestinationColumns
		{
			get { return _destColumnNames; }
		}

		public IEnumerable<object[]> Lines
		{
			get { return _lines; }
		}

		public object GetValue( string column, object[] line, int rowIndex )
		{
			try
			{
				return _valueMap[column]( line, rowIndex ) ?? DBNull.Value;
			}
			catch( IndexOutOfRangeException )
			{
				throw new IndexOutOfRangeException( "Invalid column " + column +
					", line " + _query.Selection.CodeLine.LineNumberOf( column ) + "." );
			}
		}

		public bool Prepare()
		{
			_lines = null;
			_destColumnNames = null;
			_valueMap = null;

			if( !Directory.Exists( ConnectionString.BasePath ) && 
				!SchemaAdapter.IsFileDetailsTableName( _query.From.Table ) && 
				!SchemaAdapter.IsColumnDetailsTableName( _query.From.Table ) )
			{
				throw new QueryRunnerException( "Directory not found " + ConnectionString.BasePath + "." );
			}

			// TODO make the value map key based so that the where clause works with $rownumber.
			bool hasErrors = false;
			var sourceColumnNames = _query.From.GetColumns( ConnectionString );
			_destColumnNames = _query.Selection.Columns.SelectMany( c => c == "*" ? sourceColumnNames.Select( a => a.Name ) : new[] { c } ).ToArray();
			_valueMap = _destColumnNames.ToDictionary( c => c, c =>
			{
				if( SchemaAdapter.IsRowNumberColumnName( c ) )
				{
					return new Func<object[], int, object>( ( v, i ) => i );
				}
				else
				{
					var sourceColumn = sourceColumnNames.Select( ( sc, i ) => new { Name = sc.Name, Index = i } ).FirstOrDefault( sc => sc.Name == c );
					if( sourceColumn == null )
					{
						throw new QueryRunnerException( "Invalid column " + c, _query.Selection.CodeLine.LineNumberOf( c ) );
					}
					else
					{
						return new Func<object[], int, object>( ( v, i ) => (sourceColumn.Index < v.Length ? v[sourceColumn.Index] : null) );
					}
				}
			} );

			if( !hasErrors )
			{
				try
				{
					_query.Where.Initialise( sourceColumnNames.Select( sc => sc.Name ).ToArray() );

					_lines = _query.From.GetLines( ConnectionString ).Where( l => _query.Where.IsMatch( l ) );
				}
				catch( QueryParserException ex )
				{
					throw new QueryRunnerException( ex.Message, ex ) { LineNumber = _query.Where.CodeLine.LineNumber };
				}
			}

			return hasErrors;
		}
	}
}

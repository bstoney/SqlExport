using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SqlExport.Data.Adapters.Linq
{
	internal class ScriptHelper
	{
		private string[] _usings;

		public ScriptHelper( string[] usings )
		{
			this._usings = usings;
		}

		public List<Exception> Errors { get; set; }

		public int ScriptLineOffset { get; private set; }

		public string BuildObjectCode( IRelationalSchemaAdapter schema )
		{
			Errors = new List<Exception>();
			StringBuilder code = new StringBuilder();

			code.AppendLine( "using System;" );
			code.AppendLine( "using System.Data;" );
			code.AppendLine( "using System.Data.Common;" );
			code.AppendLine( "using System.Data.Linq;" );
			code.AppendLine( "using System.Data.Linq.Mapping;" );
			code.AppendLine( "using System.Linq;" );
			code.AppendLine( "using SqlExport.Data.Adapters.Linq;" );

			code.AppendLine( "namespace SqlExport.Data.Adapters.Linq" );
			code.AppendLine( "{" );
			code.AppendLine( "	public abstract class GeneratedQueryWrapper" );
			code.AppendLine( "	{" );

			// Data context.
			code.AppendLine( "		private DataContext _db =  null;" );
			// Tables.
			foreach( SchemaItem table in schema.GetTables() )
			{
				code.AppendFormat( "		protected Table<{0}> {0} {{ get {{ return _db.GetTable<{0}>(); }} }}\n",
					CodeSafeName( table.Name ) );
			}
			// Output SQL.
			code.AppendLine( "		protected void PrintSql( IQueryable query )" );
			code.AppendLine( "		{" );
			code.AppendLine( "			if( _db != null )" );
			code.AppendLine( "			{" );
			code.AppendLine( "				_db.GetCommand( query ).CommandText.Print();" );
			code.AppendLine( "			}" );
			code.AppendLine( "		}" );
			// Initialisation.
			code.AppendLine( "		public void InitDataContext( IDbConnection connection, DbTransaction transaction, int commandTimeout )" );
			code.AppendLine( "		{" );
			code.AppendLine( "			if( connection != null )" );
			code.AppendLine( "			{" );
			code.AppendLine( "				_db = new DataContext( connection );" );
			code.AppendLine( "				if( transaction != null )" );
			code.AppendLine( "				{" );
			code.AppendLine( "					_db.Transaction = transaction;" );
			code.AppendLine( "				}" );
			code.AppendLine( "				_db.CommandTimeout = commandTimeout;" );
			code.AppendLine( "			}" );
			code.AppendLine( "		}" );
			// Execution.
			code.AppendLine( "		public abstract void Execute();" );

			code.AppendLine( "	}" );

			foreach( SchemaItem table in schema.GetTables() )
			{
				string typeName = CodeSafeName( table.Name );
				Dictionary<string, int> columnNameCount = new Dictionary<string, int>();
				code.AppendFormat( "	[Table(Name=\"[{0}]\")]\n", table.Name );
				code.AppendFormat( "	public class {0}\n", typeName );
				code.AppendLine( "	{" );
				foreach( Column col in schema.GetColumns( table.Name ) )
				{
					string propertyName;
					string propertyType;

					propertyName = CodeSafeName( col.Name );
					if( columnNameCount.ContainsKey( propertyName ) )
					{
						columnNameCount[propertyName]++;
						propertyName = string.Concat( propertyName, columnNameCount[propertyName] );
					}
					else
					{
						columnNameCount.Add( propertyName, 0 );
					}
					if( propertyName == typeName )
					{
						propertyName = string.Concat( "c_", propertyName );
					}

					Type fieldType = typeof( object );
					try
					{
						fieldType = schema.GetColumnType( col );
					}
					catch( Exception exp )
					{
						Errors.Add( exp );
					}
					if( fieldType.IsValueType && col.Nullable.HasValue && col.Nullable.Value )
					{
						propertyType = string.Format( "{0}?", fieldType.Name );
					}
					else
					{
						propertyType = fieldType.Name;
					}

					code.AppendFormat( "		[Column(Name=\"[{0}]\")]\n", col.Name );
					code.AppendFormat( "		public {0} {1} {{ get; set; }}\n", propertyType, propertyName );
				}
				code.AppendLine( "	}" );
			}
			code.AppendLine( "}" );

			System.Diagnostics.Debug.WriteLine( code.ToString() );

			return code.ToString();
		}

		public string BuildGeneratedQuery( TextReader script, IRelationalSchemaAdapter schema )
		{
			Errors = new List<Exception>();
			StringBuilder code = new StringBuilder();

			code.AppendLine( "using System;" );
			code.AppendLine( "using System.Data;" );
			code.AppendLine( "using System.Data.Common;" );
			code.AppendLine( "using System.Data.Linq;" );
			code.AppendLine( "using System.Data.Linq.Mapping;" );
			code.AppendLine( "using System.Linq;" );
			code.AppendLine( "using SqlExport.Data.Adapters.Linq;" );

			foreach( string item in _usings )
			{
				code.AppendFormat( "using {0};\n", item );
			}

			code.AppendLine();
			code.AppendFormat( "public class GeneratedQuery : GeneratedQueryWrapper, {0}\n", typeof( IGeneratedScript ).Name );
			code.AppendLine( "{" );

			// Execution.
			code.AppendLine( "	public override void Execute()" );
			code.AppendLine( "	{" );

			ScriptLineOffset = _usings.Length + 12;
			code.AppendLine( script.ReadToEnd() );

			code.AppendLine( "	}" );

			code.AppendLine( "}" );

			System.Diagnostics.Debug.WriteLine( code.ToString() );

			return code.ToString();
		}

		private string CodeSafeName( string name )
		{
			name = Regex.Replace( name, @"(?:^[0-9])|[^\w]", "_" );
			if( Array.IndexOf<string>( SyntaxDefinition.CSharpKeywords, name ) >= 0 )
			{
				name = string.Concat( "_", name );
			}
			return name;
		}
	}
}


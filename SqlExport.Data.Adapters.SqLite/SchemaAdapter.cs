using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SqlExport.Data.Adapters.SqLite
{
	internal class SchemaAdapter : ISchemaAdapter
	{
		private CommandAdapter _commandAdapter;

		public SchemaAdapter( CommandAdapter commandAdapter )
		{
			_commandAdapter = commandAdapter;
		}

		private Column[] GetTableColumns( string tableName )
		{
			List<Column> columns = new List<Column>();
			string sql = String.Concat( "PRAGMA table_info( ", tableName, ");" );
			using( SQLiteCommand cmd = _commandAdapter.CreateCommand( sql ) )
			{
				using( SQLiteDataReader reader = cmd.ExecuteReader() )
				{
					while( reader.Read() )
					{
						columns.Add( new Column( (string)reader["name"], null, null ) );
					}
				}
			}
			return columns.ToArray();
		}

		#region IDisposable Members

		public void Dispose()
		{
			if( _commandAdapter != null )
			{
				_commandAdapter.Dispose();
			}
		}

		#endregion

		public SchemaItem[] GetObjects( string type, SchemaItemType itemType )
		{
			List<SchemaItem> tables = new List<SchemaItem>();
			string sql = null;
			switch( type )
			{
				case "Tables":
					sql = @"SELECT name FROM sqlite_master
WHERE type='table'
ORDER BY name";
					break;
				case "System Tables":
					return new SchemaItem[] { 
						new SchemaItem( "sqlite_master", itemType), 
						new SchemaItem( "sqlite_temp_master", itemType) 
					};
				case "Views":
					sql = @"SELECT name FROM sqlite_master
WHERE type = 'view'
ORDER BY name;";
					break;
				default:
					throw new NotImplementedException();
			}

			using( SQLiteCommand cmd = _commandAdapter.CreateCommand( sql ) )
			{
				using( SQLiteDataReader reader = cmd.ExecuteReader() )
				{
					while( reader.Read() )
					{
						tables.Add( new SchemaItem( (string)reader["name"], itemType ) );
					}
				}
			}

			return tables.ToArray();
		}

		#region ISchemaAdapter Members

		public string[] GetSections()
		{
			return new string[] { "Tables", "System Tables", "Views" };
		}

		public SchemaItem[] PopulateFromPath( string[] path )
		{
			if( path.Length == 1 )
			{
				switch( path[0] )
				{
					case "Tables":
					case "System Tables":
						return GetObjects( path[0], SchemaItemType.Table );
					case "Views":
						return GetObjects( path[0], SchemaItemType.View );
					default:
						break;
				}
			}
			else if( path.Length == 2 )
			{
				switch( path[0] )
				{
					case "User Tables":
					case "System Tables":
					case "Views":
						return GetTableColumns( path[1] );
				}
			}

			return new SchemaItem[] { };
		}

		public string GetSchemaItemScript( string[] path )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

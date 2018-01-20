using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;
using SqlExport.Collections;

namespace SqlExport.Data.Adapters.Linq
{
	public class ConnectionInterface : IConnectionAdapter
	{
		private string[] _references = new string[] { };
		private string[] _usings = new string[] { };

		private static IConnectionAdapter GetConnectionAdapter( ref string connectionString )
		{
			IConnectionAdapter ca = null;
			try
			{
				List<string> param = new List<string>( connectionString.Split( ';' ) );
				string type = param.Where( p => p.StartsWith( "Type",
					StringComparison.InvariantCultureIgnoreCase ) ).First();
				param.Remove( type );
				type = type.Split( '=' ).Last();
				connectionString = string.Join( ";", param.ToArray() );
				ca = ConnectionAdapterHelper.GetConnectionAdapter( type );
			}
			catch( InvalidOperationException )
			{
				// Connection type has not been set, defaulting to an empty one.
				ca = new EmptyConnectionAdapter();
			}
			return ca;
		}

		[ConnectionOption]
		public string References
		{
			get { return ToOption( this._references ); }
			set { this._references = this.FromOption( value ); }
		}

		[ConnectionOption]
		public string Usings
		{
			get { return ToOption( this._usings ); }
			set { this._usings = this.FromOption( value ); }
		}

		/// <summary>
		/// Buils a single string option from an array of values.
		/// </summary>
		private string ToOption( string[] values )
		{
			return string.Join( "\n", values );
		}

		/// <summary>
		/// Splits a single option into an array of values.
		/// </summary>
		private string[] FromOption( string value )
		{
			if( string.IsNullOrEmpty( value ) )
			{
				return new string[] { };
			}
			else
			{
				return value.Split( '\n' ).Select( s => s.Trim() ).Where( s => !string.IsNullOrEmpty( s ) ).ToArray();
			}
		}

		#region IConnectionAdapter Members

		public string Name
		{
			get { return "Linq"; }
		}

		public ICommandAdapter GetCommandAdapter( string connectionString, int commandTimeout )
		{
			IConnectionAdapter ca = GetConnectionAdapter( ref connectionString );
			IDbCommandAdapter dca = ca.GetCommandAdapter( connectionString, commandTimeout ) as IDbCommandAdapter;
			if( dca == null )
			{
				throw new Exception( "Adapter type does not support IDbCommandAdapter." );
			}
			IRelationalSchemaAdapter rsa = ca.GetSchemaAdapter( connectionString, commandTimeout ) as IRelationalSchemaAdapter;
			if( rsa == null )
			{
				throw new Exception( "Adapter type does not support IRelationalSchemaAdapter." );
			}
			return new CommandAdapter( dca, rsa, commandTimeout, _references, _usings );
		}

		public ISchemaAdapter GetSchemaAdapter( string connectionString, int commandTimeout )
		{
			IConnectionAdapter ca = GetConnectionAdapter( ref connectionString );
			return new SchemaAdapter( ca.GetSchemaAdapter( connectionString, commandTimeout ) );
		}

		public StatementTemplateCollection GetTemplates()
		{
			StatementTemplateCollection templates = new StatementTemplateCollection();
			return templates;
		}

		public ISyntaxDefinition GetSyntaxDefinition()
		{
			return new SyntaxDefinition();
		}

		#endregion
	}
}

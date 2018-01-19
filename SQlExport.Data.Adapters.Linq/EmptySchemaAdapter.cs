using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Linq
{
	internal class EmptySchemaAdapter : IRelationalSchemaAdapter
	{
		#region IRelationalSchemaAdapter Members

		public SchemaItem[] GetTables()
		{
			return new SchemaItem[] { };
		}

		public Column[] GetColumns( string tableName )
		{
			throw new NotImplementedException();
		}

		public Type GetColumnType( Column column )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region ISchemaAdapter Members

		public string[] GetSections()
		{
			return new string[] { };
		}

		public SchemaItem[] PopulateFromPath( string[] path )
		{
			return new SchemaItem[] { };
		}

		public string GetSchemaItemScript( string[] path )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}

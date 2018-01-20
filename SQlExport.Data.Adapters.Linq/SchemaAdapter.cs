using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;

namespace SqlExport.Data.Adapters.Linq
{
	internal class SchemaAdapter : ISchemaAdapter
	{
		private ISchemaAdapter _innerAdapter;

		public SchemaAdapter( ISchemaAdapter innerAdapter )
		{
			_innerAdapter = innerAdapter;
		}

		#region ISchemaAdapter Members

		public string[] GetSections()
		{
			return _innerAdapter.GetSections();
		}

		public SchemaItem[] PopulateFromPath( string[] path )
		{
			return _innerAdapter.PopulateFromPath( path );
		}

		public string GetSchemaItemScript( string[] path )
		{
			return _innerAdapter.GetSchemaItemScript( path );
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_innerAdapter.Dispose();
		}

		#endregion
	}
}

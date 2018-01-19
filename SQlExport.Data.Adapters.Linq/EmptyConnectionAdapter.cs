using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Collections;

namespace SqlExport.Data.Adapters.Linq
{
	internal class EmptyConnectionAdapter : IConnectionAdapter
	{
		#region IConnectionAdapter Members

		public string Name
		{
			get { return "Empty"; }
		}

		public ICommandAdapter GetCommandAdapter( string connectionString, int commandTimeout )
		{
			return new EmptyCommandAdapter();
		}

		public ISchemaAdapter GetSchemaAdapter( string connectionString, int commandTimeout )
		{
			return new EmptySchemaAdapter();
		}

		public StatementTemplateCollection GetTemplates()
		{
			throw new NotImplementedException();
		}

		public ISyntaxDefinition GetSyntaxDefinition()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

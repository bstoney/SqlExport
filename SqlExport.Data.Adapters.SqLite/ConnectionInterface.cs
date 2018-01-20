using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using SqlExport.Collections;

namespace SqlExport.Data.Adapters.SqLite
{
	public class ConnectionInterface : IConnectionAdapter
	{
		#region IConnectionAdapter Members

		public string Name
		{
			get { return "SqLite"; }
		}

		public ICommandAdapter GetCommandAdapter( string connectionString, int commandTimeout )
		{
			return new CommandAdapter( connectionString, commandTimeout );
		}

		public ISchemaAdapter GetSchemaAdapter( string connectionString, int commandTimeout )
		{
			return new SchemaAdapter( new CommandAdapter( connectionString, commandTimeout ) );
		}

		public StatementTemplateCollection GetTemplates()
		{
			StatementTemplateCollection templates = new StatementTemplateCollection();
			templates["Standard"] = new IStatementTemplate[] { new SelectTemplate() };
			return templates;
		}

		public ISyntaxDefinition GetSyntaxDefinition()
		{
			return new SyntaxDefinition();
		}

		#endregion
	}
}

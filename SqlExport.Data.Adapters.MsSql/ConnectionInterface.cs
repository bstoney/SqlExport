using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace SqlExport.Data.Adapters.MsSql
{
    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Common.Editor;

    public class ConnectionInterface : IConnectionAdapter
    {
        #region IConnectionAdapter Members

        public string Name
        {
            get { return "MsSql"; }
        }

        public ICommandAdapter GetCommandAdapter(string connectionString, int commandTimeout)
        {
            return new CommandAdapter(connectionString, commandTimeout);
        }

        public ISchemaAdapter GetSchemaAdapter(string connectionString, int commandTimeout)
        {
            return new SchemaAdapter(new CommandAdapter(connectionString, commandTimeout));
        }

        public StatementTemplateCollection GetTemplates()
        {
            StatementTemplateCollection templates = new StatementTemplateCollection()
			{
				{ "Standard", new IStatementTemplate[] { new TemplateSelect() } },
				{ 
					"Objects", new IStatementTemplate[] 
					{ 
						new TemplateCreateTable(),
						new TemplateCreateView() 
					} 
				}
			};

            return templates;
        }

        public ISyntaxDefinition GetSyntaxDefinition()
        {
            return new SyntaxDefinition();
        }

        #endregion
    }
}

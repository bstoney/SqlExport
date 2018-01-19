using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace SqlExport.Data.Adapters.Sybase
{
    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Common.Editor;

    public class ConnectionInterface : IConnectionAdapter
    {
        #region IConnectionAdapter Members

        public string Name
        {
            get { return "Sybase"; }
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

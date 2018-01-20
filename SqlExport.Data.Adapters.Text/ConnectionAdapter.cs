using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text
{
    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Common.Editor;

    public class ConnectionAdapter : IConnectionAdapter
    {
        #region IConnectionAdapter Members

        public string Name
        {
            get { return "Text"; }
        }

        public ICommandAdapter GetCommandAdapter(string connectionString, int commandTimeout)
        {
            return new CommandAdapter() { ConnectionString = ConnectionString.Parse(connectionString), CommandTimeout = commandTimeout };
        }

        public ISchemaAdapter GetSchemaAdapter(string connectionString, int commandTimeout)
        {
            return new SchemaAdapter(new CommandAdapter() { ConnectionString = ConnectionString.Parse(connectionString), CommandTimeout = commandTimeout });
        }

        public StatementTemplateCollection GetTemplates()
        {
            throw new NotImplementedException();
        }

        public ISyntaxDefinition GetSyntaxDefinition()
        {
            return new SyntaxDefinition();
        }

        #endregion
    }
}

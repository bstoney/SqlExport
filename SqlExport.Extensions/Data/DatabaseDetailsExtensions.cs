namespace SqlExport.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Common.Editor;

    /// <summary>
    /// Defines the DatabaseDetailsExtensions type.
    /// </summary>
    public static class DatabaseDetailsExtensions
    {
        /// <summary>
        /// Gets the syntax definition.
        /// </summary>
        /// <param name="databaseDetails">The database details.</param>
        /// <returns>A syntax definition.</returns>
        public static ISyntaxDefinition GetSyntaxDefinition(this DatabaseDetails databaseDetails)
        {
            return databaseDetails.GetConnectionAdapter().GetSyntaxDefinition();
        }

        /// <summary>
        /// Gets the templates.
        /// </summary>
        /// <param name="databaseDetails">The database details.</param>
        /// <returns>The database templates.</returns>
        public static StatementTemplateCollection GetTemplates(this DatabaseDetails databaseDetails)
        {
            return databaseDetails.GetConnectionAdapter().GetTemplates();
        }

        /// <summary>
        /// Gets the schema adapter.
        /// </summary>
        /// <param name="databaseDetails">The database details.</param>
        /// <returns>A schema adapter.</returns>
        public static ISchemaAdapter GetSchemaAdapter(this DatabaseDetails databaseDetails)
        {
            IConnectionAdapter ca = databaseDetails.GetConnectionAdapter();
            return ca.GetSchemaAdapter(databaseDetails.ConnectionString, Configuration.Current.CommandTimeout);
        }

        /// <summary>
        /// Creates the connection context.
        /// </summary>
        /// <param name="databaseDetails">The database details.</param>
        /// <returns>A database connection context.</returns>
        public static DatabaseConnectionContext CreateConnectionContext(this DatabaseDetails databaseDetails)
        {
            var connectionContext = new DatabaseConnectionContext
                {
                    Name = databaseDetails.Name,
                    ConnectionString = databaseDetails.ConnectionString,
                    Type = databaseDetails.Type
                };

            return connectionContext;
        }

        /// <summary>
        /// Gets the connection adapter.
        /// </summary>
        /// <param name="databaseDetails">The database details.</param>
        /// <returns>A connection adapter.</returns>
        internal static IConnectionAdapter GetConnectionAdapter(this DatabaseDetails databaseDetails)
        {
            IConnectionAdapter ca = ConnectionAdapterHelper.GetConnectionAdapter(databaseDetails.Type);
            if (ca == null)
            {
                throw new InvalidOperationException(string.Concat("Unable to load connection for database type '", databaseDetails.Type, "'."));
            }

            return ca;
        }
    }
}

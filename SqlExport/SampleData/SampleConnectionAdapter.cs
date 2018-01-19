namespace SqlExport.SampleData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Common.Editor;
    using SqlExport.Data;

    /// <summary>
    /// Defines the SampleConnectionAdapter class.
    /// </summary>
    internal class SampleConnectionAdapter : IConnectionAdapter
    {
        /// <summary>
        /// The sample connection adapter name
        /// </summary>
        internal const string SampleConnectionAdapterName = "Sample";

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return SampleConnectionAdapterName; }
        }

        /// <summary>
        /// Gets the command adapter.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>
        /// A command adapter.
        /// </returns>
        public ICommandAdapter GetCommandAdapter(string connectionString, int commandTimeout)
        {
            return null;
        }

        /// <summary>
        /// Gets the schema adapter.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>
        /// A schema adapter.
        /// </returns>
        public ISchemaAdapter GetSchemaAdapter(string connectionString, int commandTimeout)
        {
            return new SampleSchemaAdapter();
        }

        /// <summary>
        /// Gets the templates.
        /// </summary>
        /// <returns>
        /// A statement template collection.
        /// </returns>
        public StatementTemplateCollection GetTemplates()
        {
            return new StatementTemplateCollection();
        }

        /// <summary>
        /// Gets the syntax definition.
        /// </summary>
        /// <returns>
        /// A syntax definition.
        /// </returns>
        public ISyntaxDefinition GetSyntaxDefinition()
        {
            return null;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialise()
        {
            if (!ConnectionAdapterHelper.IsConnectionAdapterRegistered(SampleConnectionAdapterName))
            {
                ConnectionAdapterHelper.RegisterConnectionAdapter(
                    SampleConnectionAdapterName, typeof(SampleConnectionAdapter).AssemblyQualifiedName);
            }
        }

        /// <summary>
        /// Defines the SampleSchemaAdapter class.
        /// </summary>
        private class SampleSchemaAdapter : ISchemaAdapter
        {
            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Gets the sections.
            /// </summary>
            /// <returns>
            /// A list of sections.
            /// </returns>
            public string[] GetSections()
            {
                return new[] { "Section 1", "Section 2" };
            }

            /// <summary>
            /// Populates from path.
            /// </summary>
            /// <param name="path">The path.</param>
            /// <returns>
            /// A list of schema items.
            /// </returns>
            public ISchemaItem[] PopulateFromPath(string[] path)
            {
                return new[] { new SchemaItem((path.LastOrDefault() ?? "<Name>") + path.Length, SchemaItemType.Table) };
            }

            /// <summary>
            /// Gets the schema item script.
            /// </summary>
            /// <param name="path">The path.</param>
            /// <returns>
            /// A string.
            /// </returns>
            public string GetSchemaItemScript(string[] path)
            {
                return null;
            }
        }
    }
}

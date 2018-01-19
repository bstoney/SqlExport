namespace SqlExport.Common
{
    using SqlExport.Common.Data;
    using SqlExport.Common.Editor;

    /// <summary>
    /// Defines the IConnectionAdapter interface.
    /// </summary>
    public interface IConnectionAdapter
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the command adapter.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>A command adapter.</returns>
        ICommandAdapter GetCommandAdapter(string connectionString, int commandTimeout);

        /// <summary>
        /// Gets the schema adapter.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>A schema adapter.</returns>
        ISchemaAdapter GetSchemaAdapter(string connectionString, int commandTimeout);

        /// <summary>
        /// Gets the templates.
        /// </summary>
        /// <returns>A statement template collection.</returns>
        StatementTemplateCollection GetTemplates();

        /// <summary>
        /// Gets the syntax definition.
        /// </summary>
        /// <returns>A syntax definition.</returns>
        ISyntaxDefinition GetSyntaxDefinition();
    }
}

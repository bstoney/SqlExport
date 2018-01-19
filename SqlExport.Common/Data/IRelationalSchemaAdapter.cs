namespace SqlExport.Common.Data
{
    using System;

    /// <summary>
    /// Defines the IRelationalSchemaAdapter interface.
    /// </summary>
    public interface IRelationalSchemaAdapter : ISchemaAdapter
    {
        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <returns>A list of tables.</returns>
        ISchemaItem[] GetTables();

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>A list of columns.</returns>
        ISchemaItem[] GetColumns(string tableName);

        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns>The column type.</returns>
        Type GetColumnType(ISchemaItem column);
    }
}

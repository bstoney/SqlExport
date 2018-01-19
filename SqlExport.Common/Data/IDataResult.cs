namespace SqlExport.Common.Data
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the IDataResult interface.
    /// </summary>
    public interface IDataResult
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the results as an enumerable.
        /// </summary>
        /// <returns>The results as an enumerable.</returns>
        IEnumerable AsEnumerable();

        /// <summary>
        /// Fetches the count.
        /// </summary>
        /// <returns>An integer.</returns>
        int FetchCount();

        /// <summary>
        /// Fetches the columns.
        /// </summary>
        /// <returns>A list of the result columns.</returns>
        IEnumerable<ISchemaItem> FetchColumns();

        /// <summary>
        /// Fetches the value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <returns>An object.</returns>
        object FetchValue(object row, string column);

        /// <summary>
        /// Fetches the value.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>An object.</returns>
        object FetchValue(int columnIndex, int rowIndex);
    }
}
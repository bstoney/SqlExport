namespace SqlExport.Common.Data
{
    using System;

    /// <summary>
    /// Defines the ISchemaAdapter interface.
    /// </summary>
    public interface ISchemaAdapter : IDisposable
    {
        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>A list of sections.</returns>
        string[] GetSections();

        /// <summary>
        /// Populates from path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A list of schema items.</returns>
        ISchemaItem[] PopulateFromPath(string[] path);

        /// <summary>
        /// Gets the schema item script.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A string.</returns>
        string GetSchemaItemScript(string[] path);
    }
}

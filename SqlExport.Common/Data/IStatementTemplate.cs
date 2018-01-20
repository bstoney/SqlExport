namespace SqlExport.Common.Data
{
    /// <summary>
    /// Defines the IStatementTemplate interface.
    /// </summary>
    public interface IStatementTemplate
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the statement.
        /// </summary>
        string Statement { get; }
    }
}

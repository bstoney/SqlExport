namespace SqlExport.Common.Data
{
    /// <summary>
    /// Specifies the schema item types.
    /// </summary>
    public enum SchemaItemType
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// The table
        /// </summary>
        Table,

        /// <summary>
        /// The view
        /// </summary>
        View,

        /// <summary>
        /// The column
        /// </summary>
        Column,

        /// <summary>
        /// The procedure
        /// </summary>
        Procedure,

        /// <summary>
        /// The function
        /// </summary>
        Function,

        /// <summary>
        /// The folder
        /// </summary>
        Folder,

        /// <summary>
        /// The database
        /// </summary>
        Database
    }
}

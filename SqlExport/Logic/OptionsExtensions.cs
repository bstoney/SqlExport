namespace SqlExport.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using SqlExport.Common;

    /// <summary>
    /// Defines the OptionsExtensions class.
    /// </summary>
    public static class OptionsExtensions
    {
        /// <summary>
        /// Gets the databases.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>A list of the databases.</returns>
        public static IEnumerable<DatabaseDetails> GetDatabaseList(this IOptions options)
        {
            return new DatabaseDetails[] { DatabaseDetails.NoConnection }.Concat(Configuration.GetDatabases());
        }
    }
}

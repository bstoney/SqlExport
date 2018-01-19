namespace SqlExport.Messages
{
    using SqlExport.Common.Data;

    /// <summary>
    /// Defines the OpenQueryMessage class.
    /// </summary>
    public class OpenQueryMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenQueryMessage"/> class.
        /// </summary>
        public OpenQueryMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenQueryMessage"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="queryText">The query text.</param>
        /// <param name="hasChanged">if set to <c>true</c> [has changed].</param>
        public OpenQueryMessage(string filename, string databaseName, string queryText, bool hasChanged)
        {
            this.Filename = filename;
            this.DatabaseName = databaseName;
            this.QueryText = queryText;
            this.HasChanged = hasChanged;
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the query text.
        /// </summary>
        /// <value>
        /// The query text.
        /// </value>
        public string QueryText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this query has changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this query has changed; otherwise, <c>false</c>.
        /// </value>
        public bool HasChanged { get; set; }
    }
}

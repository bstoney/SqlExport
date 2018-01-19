namespace SqlExport.Common
{
    /// <summary>
    /// Defines the IOptions type.
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// Gets or sets the recent file count.
        /// </summary>
        int RecentFileCount { get; set; }

        /// <summary>
        /// Gets or sets the type of the export.
        /// </summary>
        /// <value>
        /// The type of the export.
        /// </value>
        string ExportType { get; set; }

        /// <summary>
        /// Gets or sets the current directory.
        /// </summary>
        string CurrentDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IOptions"/> is editable.
        /// </summary>
        bool Editable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IOptions"/> is transactional.
        /// </summary>
        bool Transactional { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [single instance].
        /// </summary>
        bool SingleInstance { get; set; }

        /// <summary>
        /// Gets or sets the command timeout.
        /// </summary>
        int CommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets the current database.
        /// </summary>
        string CurrentDatabase { get; set; }
    }
}
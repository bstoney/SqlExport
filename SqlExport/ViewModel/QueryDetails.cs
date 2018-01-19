namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Defines the Query type.
    /// </summary>
    public class QueryDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryDetails"/> class.
        /// </summary>
        /// <param name="uniqueID">The unique ID.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="hasChanged">if set to <c>true</c> [has changed].</param>
        /// <param name="isRunning">if set to <c>true</c> [is running].</param>
        /// <param name="isExecuting">if set to <c>true</c> [is executing].</param>
        /// <param name="hasConnection">if set to <c>true</c> [has connection].</param>
        /// <param name="hasTransaction">if set to <c>true</c> [has transaction].</param>
        /// <param name="canExport">if set to <c>true</c> [can export].</param>
        public QueryDetails(
            Guid uniqueID,
            string displayText,
            string databaseName,
            string filename,
            bool hasChanged,
            bool isRunning,
            bool isExecuting,
            bool hasConnection,
            bool hasTransaction,
            bool canExport)
        {
            this.UniqueID = uniqueID;
            this.DisplayText = displayText;
            this.DatabaseName = databaseName;
            this.Filename = filename;
            this.HasChanged = hasChanged;
            this.IsRunning = isRunning;
            this.IsExecuting = isExecuting;
            this.HasConnection = hasConnection;
            this.HasTransaction = hasTransaction;
            this.CanExport = canExport;
        }

        /// <summary>
        /// Gets the get display text.
        /// </summary>
        public string DisplayText { get; private set; }

        /// <summary>
        /// Gets the unique ID.
        /// </summary>
        public Guid UniqueID { get; private set; }

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the query has changed since last save.
        /// </summary>
        public bool HasChanged { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the query is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the query is executing.
        /// </summary>
        public bool IsExecuting { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the query has an active transaction.
        /// </summary>
        public bool HasTransaction { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the query has an active connection.
        /// </summary>
        public bool HasConnection { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the query can export.
        /// </summary>
        public bool CanExport { get; private set; }
    }
}

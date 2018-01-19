namespace SqlExport.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Defines a command adapter interface.
    /// </summary>
    public interface ICommandAdapter : IDisposable
    {
        /// <summary>
        /// Occurs when the command send a message.
        /// </summary>
        event ConnectionMessageHandler Message;

        /// <summary>
        /// Gets a value indicating whether or not the command has a current transaction.
        /// </summary>
        bool HasTransaction { get; }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the current command transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the current command transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Executes the query, this method should throw an ExecuteCommandException if the query fails.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A data result.</returns>
        IEnumerable<IDataResult> ExecuteCommand(TextReader query);
    }
}

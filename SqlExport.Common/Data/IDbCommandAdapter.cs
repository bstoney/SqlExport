namespace SqlExport.Common.Data
{
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// Defines the IDBCommandAdapter interface.
    /// </summary>
    public interface IDbCommandAdapter : ICommandAdapter
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        DbTransaction Transaction { get; }
    }
}

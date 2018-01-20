namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Data;

    /// <summary>
    /// Defines the SetDatabaseMessage class.
    /// </summary>
    public class SetDatabaseMessage : MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetDatabaseMessage" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="sender">The sender.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the database null.</exception>
        public SetDatabaseMessage(DatabaseDetails database, object sender = null)
            : base(sender)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            this.Database = database;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        public DatabaseDetails Database { get; private set; }
    }
}

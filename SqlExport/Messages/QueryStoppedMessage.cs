namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Defines the QueryStoppedMessage class.
    /// </summary>
    public class QueryStoppedMessage : MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStoppedMessage"/> class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        public QueryStoppedMessage(object sender)
            : base(sender)
        {
        }
    }
}

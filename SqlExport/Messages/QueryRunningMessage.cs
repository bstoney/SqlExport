namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the QueryRunningMessage class.
    /// </summary>
    public class QueryRunningMessage : MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRunningMessage"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public QueryRunningMessage(object sender)
            : base(sender)
        {
        }
    }
}

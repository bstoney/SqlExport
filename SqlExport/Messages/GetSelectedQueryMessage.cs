namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the GetSelectedQueryMessage class.
    /// </summary>
    internal class GetSelectedQueryMessage : MessageBase
    {
        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public QueryViewModel Query { get; set; }
    }
}

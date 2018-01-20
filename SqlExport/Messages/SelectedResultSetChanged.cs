namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Defines the SelectedResultSetChanged class.
    /// </summary>
    public class SelectedResultSetChanged : MessageBase
    {
        /// <summary>
        /// The result set context
        /// </summary>
        private readonly object resultSetContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedResultSetChanged" /> class.
        /// </summary>
        /// <param name="resultSetContext">The result set context.</param>
        internal SelectedResultSetChanged(object resultSetContext)
        {
            this.resultSetContext = resultSetContext;
        }

        /// <summary>
        /// Gets to row count of the currently selected result set.
        /// </summary>
        /// <returns>A row count.</returns>
        public int? GetResultCount()
        {
            int? count = null;
            var context = this.resultSetContext;
            if (context != null)
            {
                Messenger.Default.Send(new GetDataResultMessage(r => count = r.FetchCount()), context);
            }

            return count;
        }
    }
}

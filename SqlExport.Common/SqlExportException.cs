namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Defines the SQLExportException class.
    /// </summary>
    [Serializable]
    public class SqlExportException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExportException"/> class.
        /// </summary>
        public SqlExportException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExportException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SqlExportException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExportException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public SqlExportException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExportException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected SqlExportException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The available types of messages.
    /// </summary>
    public enum DisplayMessageType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = -1,

        /// <summary>
        /// The error
        /// </summary>
        Error = 0,

        /// <summary>
        /// The warning
        /// </summary>
        Warning = 1,

        /// <summary>
        /// The information
        /// </summary>
        Information = 2,

        /// <summary>
        /// The success
        /// </summary>
        Success = 4
    }
}

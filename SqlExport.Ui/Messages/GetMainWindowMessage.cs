namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Defines the GetMainWindowMessage type.
    /// </summary>
    public class GetMainWindowMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMainWindowMessage"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public GetMainWindowMessage(Action<Window> callback)
        {
            this.Callback = callback;
        }

        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        public Action<Window> Callback { get; set; }
    }
}

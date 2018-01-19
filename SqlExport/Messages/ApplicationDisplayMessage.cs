namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the ApplicationDisplayMessage class.
    /// </summary>
    public class ApplicationDisplayMessage : DisplayMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDisplayMessage"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public ApplicationDisplayMessage(string text)
            : base(text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDisplayMessage"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        public ApplicationDisplayMessage(string text, DisplayMessageType type)
            : base(text, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDisplayMessage"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="details">The details.</param>
        /// <param name="type">The type.</param>
        /// <param name="callback">The callback.</param>
        public ApplicationDisplayMessage(string text, string details, DisplayMessageType type, Action callback)
            : base(text, details, type, callback)
        {
        }

        /// <summary>
        /// Gets a display message from the the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// A new display message.
        /// </returns>
        public static explicit operator ApplicationDisplayMessage(Exception exception)
        {
            return new ApplicationDisplayMessage(exception.Message, exception.ToString(), DisplayMessageType.Error, null);
        }
    }
}

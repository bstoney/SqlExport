namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.View;

    /// <summary>
    /// Defines the DisplayMessage class.
    /// </summary>
    public class DisplayMessage
    {
        /// <summary>
        /// A separator
        /// </summary>
        public static readonly DisplayMessage Separator = new DisplayMessage(MessageItemViewModel.SeparatorText);

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMessage"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <exception cref="System.ArgumentException">Text is required for a display message.</exception>
        public DisplayMessage(string text)
            : this(text, DisplayMessageType.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMessage"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        public DisplayMessage(string text, DisplayMessageType type)
            : this(text, text, type, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMessage"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="details">The details.</param>
        /// <param name="type">The type.</param>
        /// <param name="callback">The callback.</param>
        public DisplayMessage(string text, string details, DisplayMessageType type, Action callback)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text is required for a display message.");
            }

            this.Text = text;
            this.Details = details;
            this.Type = type;
            this.Callback = callback;
        }

        /// <summary>
        /// Gets or sets the text to display in the <see cref="MessageList"/>.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets any additional details of the message.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the type of message.
        /// </summary>
        public DisplayMessageType Type { get; set; }

        /// <summary>
        /// Gets or sets a callback method which will be call when the message is selected.
        /// </summary>
        public Action Callback { get; set; }

        /// <summary>
        /// Gets a display message from the the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// A new display message.
        /// </returns>
        public static explicit operator DisplayMessage(Exception exception)
        {
            return new DisplayMessage(exception.Message, exception.ToString(), DisplayMessageType.Error, null);
        }
    }
}

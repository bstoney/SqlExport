namespace SqlExport.Messages.StatusPanel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.Editor;
    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the CaretChangedMessage class.
    /// </summary>
    public class CaretChangedMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaretChangedMessage"/> class.
        /// </summary>
        /// <param name="caretDetails">The caret details.</param>
        public CaretChangedMessage(CaretDetails caretDetails)
        {
            this.CaretDetails = caretDetails;
        }

        /// <summary>
        /// Gets or sets the caret details.
        /// </summary>
        /// <value>
        /// The caret details.
        /// </value>
        public CaretDetails CaretDetails { get; set; }
    }
}

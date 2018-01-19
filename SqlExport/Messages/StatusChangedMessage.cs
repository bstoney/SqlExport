namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the StatusChangedMessage type.
    /// </summary>
    public class StatusChangedMessage : MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusChangedMessage"/> class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        public StatusChangedMessage(object sender)
            : base(sender)
        {
        }
    }
}

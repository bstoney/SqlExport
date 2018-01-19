namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the OptionSelectedMessage class.
    /// </summary>
    internal class OptionSelectedMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionSelectedMessage"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public OptionSelectedMessage(OptionViewModel option)
        {
            this.Option = option;
        }

        /// <summary>
        /// Gets the option.
        /// </summary>
        public OptionViewModel Option { get; private set; }
    }
}

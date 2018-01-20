namespace SqlExport.Common.Extensions
{
    using System;

    using SqlExport.Common.Options;

    /// <summary>
    /// Defines the ExtensionOption class.
    /// </summary>
    public class ExtensionOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionOption"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ExtensionOption(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            this.Name = name;
            this.OptionType = OptionType.String;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the extension option.
        /// </summary>
        /// <value>
        /// The type of the extension option.
        /// </value>
        public OptionType OptionType { get; protected set; }

        /// <summary>
        /// Converts the specified option to a string.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>A string.</returns>
        public static implicit operator string(ExtensionOption option)
        {
            return option == null ? null : option.Value;
        }
    }
}

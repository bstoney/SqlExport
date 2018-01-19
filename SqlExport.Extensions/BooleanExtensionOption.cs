namespace SqlExport
{
    using System;

    using SqlExport.Common.Extensions;
    using SqlExport.Common.Options;

    /// <summary>
    /// Defines the BooleanExtensionOption class.
    /// </summary>
    public class BooleanExtensionOption : ExtensionOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanExtensionOption"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public BooleanExtensionOption(string name)
            : base(name)
        {
            this.OptionType = OptionType.Boolean;
        }

        /// <summary>
        /// converts the specified option to a Boolean.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>A Boolean.</returns>
        public static implicit operator bool(BooleanExtensionOption option)
        {
            return option != null && Convert.ToBoolean(option.Value);
        }
    }
}
namespace SqlExport.Common.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.Common.Util;

    /// <summary>
    /// Defines the OptionAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionAttribute" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public OptionAttribute(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            this.Path = path;
            this.OptionType = OptionType.String;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Path { get; set; }

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
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the option.
        /// </summary>
        /// <value>
        /// The type of the option.
        /// </value>
        public OptionType OptionType { get; protected set; }

        /// <summary>
        /// Values from string.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>
        /// A value of the correct type.
        /// </returns>
        public virtual object ConvertToValue(string stringValue, Type valueType)
        {
            // Return the default value for a null string value.
            if (stringValue == null)
            {
                if (valueType.IsValueType)
                {
                    return Activator.CreateInstance(valueType);
                }

                return null;
            }

            if (valueType.ImplementsInterface<IConvertible>())
            {
                return Convert.ChangeType(stringValue, valueType);
            }

            return stringValue;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string.</returns>
        public virtual string ConvertToString(object value)
        {
            return value == null ? null : value.ToString();
        }
    }
}

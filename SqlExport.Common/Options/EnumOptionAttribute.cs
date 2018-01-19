namespace SqlExport.Common.Options
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the EnumOptionAttribute class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class EnumOptionAttribute : SelectionOptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumOptionAttribute" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentNullException">The type must be supplied.</exception>
        /// <exception cref="System.ArgumentException">The supplied type must be an enumeration.;type</exception>
        public EnumOptionAttribute(string path, Type type)
            : base(path, Enum.GetNames(type))
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Converts to value.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>A value.</returns>
        public override object ConvertToValue(string stringValue, Type valueType)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Activator.CreateInstance(this.Type);
            }

            return Enum.Parse(this.Type, stringValue);
        }
    }
}

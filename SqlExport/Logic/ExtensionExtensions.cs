namespace SqlExport.Logic
{
    using System;
    using System.Collections.Generic;

    using SqlExport.Common.Extensions;
    using SqlExport.Common.Options;
    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the ExtensionExtensions class.
    /// </summary>
    internal static class ExtensionExtensions
    {
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>A list of extension properties.</returns>
        public static IEnumerable<PropertyItem> GetProperties(this IExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            var extensionType = extension.GetType();

            foreach (var option in extensionType.GetOptionAccessors())
            {
                PropertyItem propertyItem;
                switch (option.GetOptionTypeFrom(extension))
                {
                    case OptionType.Boolean:
                        propertyItem = new BooleanPropertyItem(extension.Name, option.DisplayName);
                        break;
                    case OptionType.Numeric:
                        propertyItem = new NumericPropertyItem(extension.Name, option.DisplayName);
                        break;
                    case OptionType.Selection:
                        var selectionOption = option.OptionAttribute as SelectionOptionAttribute;
                        string[] items = selectionOption != null ? selectionOption.SelectionItems : new string[] { };
                        propertyItem = new SelectionPropertyItem(extension.Name, option.DisplayName, items);
                        break;
                    case OptionType.String:
                    default:
                        propertyItem = new TextPropertyItem(extension.Name, option.DisplayName);
                        break;
                }

                // Use a local variable to ensure getter and setter references aren't mangled.
                var thisOption = option;

                propertyItem.DisplayName = option.DisplayName;
                propertyItem.ValueGetter = () => thisOption.GetOptionFrom(extension);
                propertyItem.ValueSetter = v => thisOption.SetOptionOn(extension, v);
                yield return propertyItem;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Ui.ViewModel;
using SqlExport.Export;
using System.Reflection;

namespace SqlExport.Business
{
    using SqlExport.Common;

    public static class ExtensionExtensions
    {
        public static void LoadPropertiesAndValues(this ExporterBase exporter, List<PropertyItem> properties)
        {
            // Get custom options.
            var extension = exporter as IExtension;
            if (extension != null)
            {
                properties.AddRange(extension.GetProperties());
            }

            // Load values from configuration.
            foreach (var prop in properties)
            {
                prop.Value = Options.Current.GetExportOption(exporter.Name, prop.Name);
            }
        }

        internal static IEnumerable<PropertyItem> GetProperties(this IExtension extension)
        {
            foreach (var option in extension.ExtensionOptions)
            {
                PropertyItem propertyItem;
                CustomExtensionOption customOption = option as CustomExtensionOption;
                if (customOption != null)
                {
                    propertyItem = customOption.GetCustomPropertyItem();
                }

                switch (option.ExtensionOptionType)
                {
                    case ExtensionOptionType.Boolean:
                        propertyItem = new BooleanPropertyItem(extension.Name, option.Name);
                        break;
                    case ExtensionOptionType.Numeric:
                        propertyItem = new NumericPropertyItem(extension.Name, option.Name);
                        break;
                    case ExtensionOptionType.String:
                    default:
                        propertyItem = new TextPropertyItem(extension.Name, option.Name);
                        break;
                }

                // Use a local variable to ensure getter and setter references aren't mangled.
                var thisOption = option;

                propertyItem.DisplayName = option.DisplayName ?? option.Name;
                propertyItem.ValueGetter = () => thisOption.Value;
                propertyItem.ValueSetter = v => thisOption.Value = v;
                yield return propertyItem;
            }
        }
    }
}

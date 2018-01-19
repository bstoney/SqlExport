namespace SqlExport.View.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using SqlExport.Ui.ViewModel;

    /// <summary>
    /// Defines the PropertyItemTypeToControlConverter type.
    /// </summary>
    public class PropertyItemTypeToControlConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var propertyItem = value as PropertyItem;
            if (propertyItem != null)
            {
                var binding = new Binding(PropertyItem.ValuePropertyName);

                var control = propertyItem.GetEditControl(binding);

                if (control != null)
                {
                    control.Tag = propertyItem;
                }

                return control; 
            }

            return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var control = value as FrameworkElement;

            return control != null ? control.Tag : null;
        }

        #endregion
    }
}

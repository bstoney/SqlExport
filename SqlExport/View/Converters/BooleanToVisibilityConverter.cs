namespace SqlExport.View.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Defines the BooleanToVisibilityConverter class.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BooleanToVisibilityConverter"/> is operates as a VisibilityToBooleanConverter.
        /// </summary>
        public bool Inverted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BooleanToVisibilityConverter"/> is not.
        /// </summary>
        public bool Not { get; set; }

        /// <summary>
        /// Visibility to Boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A visibility value.</returns>
        private object VisibilityToBool(object value)
        {
            if (!(value is Visibility))
            {
                return DependencyProperty.UnsetValue;
            }

            return (((Visibility)value) == Visibility.Visible) ^ this.Not;
        }

        /// <summary>
        /// Boolean to visibility.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A Boolean value.</returns>
        private object BoolToVisibility(object value)
        {
            if (!(value is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            return ((bool)value ^ this.Not) ? Visibility.Visible : Visibility.Collapsed;
        }

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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Inverted ? this.VisibilityToBool(value) : this.BoolToVisibility(value);
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Inverted ? this.BoolToVisibility(value) : this.VisibilityToBool(value);
        }
    }
}

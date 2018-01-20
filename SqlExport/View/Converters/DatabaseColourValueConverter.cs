namespace SqlExport.View.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    using SqlExport.Common;
    using SqlExport.Common.Data;

    /// <summary>
    /// Defines the DatabaseColourValueConverter class.
    /// </summary>
    public class DatabaseColourValueConverter : IValueConverter
    {
        /// <summary>
        /// The special connection brush.
        /// </summary>
        private static readonly Brush SpecialConnectionBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0x00, 0x00));

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
            var database = value as DatabaseDetails;
            if (database != null && database != DatabaseDetails.NoConnection)
            {
                return SystemColors.ControlTextBrush;
            }

            return SpecialConnectionBrush;
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
            throw new NotImplementedException();
        }

        #endregion
    }
}

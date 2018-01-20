namespace SqlExport.View.Converters
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Defines the ElapsedTimeValueConverter type.
    /// </summary>
    public class ElapsedTimeValueConverter : IValueConverter
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
            TimeSpan elapsedTime = TimeSpan.FromSeconds(0);
            if (value is TimeSpan)
            {
                elapsedTime = (TimeSpan)value;
            }

            if (elapsedTime.Hours == 0)
            {
                return string.Format(
                    "Time: {0:D}:{1:D2}.{2:D3}", elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds);
            }

            return string.Format(
                "Time: {0:D}:{0:D2}:{1:D2}.{2:D3}",
                elapsedTime.Hours,
                elapsedTime.Minutes,
                elapsedTime.Seconds,
                elapsedTime.Milliseconds);
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

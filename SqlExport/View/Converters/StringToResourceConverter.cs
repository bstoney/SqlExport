﻿namespace SqlExport.View.Converters
{
    using System;
    using System.Windows.Data;

    using SqlExport.Editor;
    using SqlExport.Logic;
    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the SelectedTextValueConverter type.
    /// </summary>
    public class StringToResourceConverter : IValueConverter
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
            var resourceKey = value as string;
            if (resourceKey != null)
            {
                try
                {
                    return App.Current.FindResource(resourceKey);
                }
                catch (Exception ex)
                {
                    ErrorDialogLogic.AddError(ex);
                }
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
            throw new NotImplementedException();
        }

        #endregion
    }
}

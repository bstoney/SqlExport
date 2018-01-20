using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SqlExport.Data;
using System.Windows;
using System.Windows.Media;

namespace SqlExport.View.StatusPanel
{
    using SqlExport.Common.Data;

    public class ConnectionColourValueConverter : IValueConverter
    {
        private static Brush SpecialConnectionBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0x00, 0x00));

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var database = value as DatabaseDetails;
            if (database != null)
            {
                return SystemColors.ControlTextBrush;
            }

            return SpecialConnectionBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

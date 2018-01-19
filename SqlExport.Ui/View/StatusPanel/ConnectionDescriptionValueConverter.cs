using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SqlExport.Data;

namespace SqlExport.View.StatusPanel
{
    using SqlExport.Common.Data;

    public class ConnectionDescriptionValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			var database = value as DatabaseDetails;
			if( database != null )
			{
				return database.Name;
			}

			return "<No Database>";
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

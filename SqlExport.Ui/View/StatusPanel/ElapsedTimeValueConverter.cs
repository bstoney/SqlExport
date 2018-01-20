using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SqlExport.Data;

namespace SqlExport.View.StatusPanel
{
	public class ElapsedTimeValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			TimeSpan elapsedTime = TimeSpan.FromSeconds( 0 );
			if( value is TimeSpan )
			{
				elapsedTime = (TimeSpan)value;
			}

			if( elapsedTime.Hours == 0 )
			{
				return string.Format( "Time: {0:D}:{1:D2}.{2:D3}", 
					elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds );
			}
			else
			{
				return string.Format( "Time: {0:D}:{0:D2}:{1:D2}.{2:D3}", 
					elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds );
			}
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

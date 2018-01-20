using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SqlExport.Data;

namespace SqlExport.View.StatusPanel
{
	public class RecordCountValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			var recordCount = value as int?;

			return (recordCount.HasValue ? string.Concat( "Records: ", recordCount ) : string.Empty);
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

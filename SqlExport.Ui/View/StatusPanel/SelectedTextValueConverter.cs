using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SqlExport.Data;
using SqlExport.Ui.ViewModel;

namespace SqlExport.View.StatusPanel
{
	public class SelectedTextValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			var selectedTextRange = value as SelectedTextRange;
			if( selectedTextRange != null )
			{
				if( selectedTextRange.EndLine - selectedTextRange.StartLine > 0 )
				{
					return string.Concat( "Lines: ", (selectedTextRange.StartLine + 1), "-", (selectedTextRange.EndLine + 1) );
				}

				return string.Concat( "Line: ", (selectedTextRange.StartLine + 1) );
			}

			return string.Empty;
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

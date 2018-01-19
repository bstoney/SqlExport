using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace SqlExport.View
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public bool Inverted { get; set; }

		public bool Not { get; set; }

		private object VisibilityToBool( object value )
		{
			if( !(value is Visibility) )
			{
				return DependencyProperty.UnsetValue;
			}

			return (((Visibility)value) == Visibility.Visible) ^ Not;
		}

		private object BoolToVisibility( object value )
		{
			if( !(value is bool) )
			{
				return DependencyProperty.UnsetValue;
			}

			return ((bool)value ^ Not) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return Inverted ? VisibilityToBool( value ) : BoolToVisibility( value );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return Inverted ? BoolToVisibility( value ) : VisibilityToBool( value );
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace SqlExport.ViewModel
{
	public class BooleanPropertyItem : PropertyItem
	{
		public BooleanPropertyItem( string category, string name )
			: base( category, name )
		{
		}

		public override FrameworkElement GetEditControl( Binding binding )
		{
			var boolean = new CheckBox();
			boolean.SetBinding( CheckBox.IsCheckedProperty, binding );
			return boolean;
		}
	}
}

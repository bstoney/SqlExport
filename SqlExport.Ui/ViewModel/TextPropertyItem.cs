using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace SqlExport.ViewModel
{
	public class TextPropertyItem : PropertyItem
	{
		public TextPropertyItem( string category, string name )
			: base( category, name )
		{
		}

		public override FrameworkElement GetEditControl( Binding binding )
		{
			var text = new TextBox();
			text.SetBinding( TextBox.TextProperty, binding );

			return text;
		}
	}
}

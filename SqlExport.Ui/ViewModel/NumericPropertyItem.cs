using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace SqlExport.ViewModel
{
	internal class NumericPropertyItem : PropertyItem
	{
		public NumericPropertyItem( string category, string name )
			: base( category, name )
		{
		}

		public override FrameworkElement GetEditControl( Binding binding )
		{
			var numeric = new TextBox();
			numeric.PreviewTextInput += new TextCompositionEventHandler( textbox_PreviewTextInput );
			numeric.SetBinding( TextBox.TextProperty, binding );
			return numeric;
		}

		private void textbox_PreviewTextInput( object sender, TextCompositionEventArgs e )
		{
			// Set the event as handled is any characters are not a number.
			e.Handled = e.Text.Any( c => !Char.IsNumber( c ) );
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SqlExport
{
	public static class ScopeMessageToken
	{
		public static Guid GetMessageToken( DependencyObject obj )
		{
			return (Guid)obj.GetValue( MessageTokenProperty );
		}

		public static void SetMessageToken( DependencyObject obj, Guid value )
		{
			obj.SetValue( MessageTokenProperty, value );
		}

		public static readonly DependencyProperty MessageTokenProperty = DependencyProperty.RegisterAttached( "MessageToken", typeof( Guid ), typeof( ScopeMessageToken ),
				new FrameworkPropertyMetadata( Guid.Empty ) );
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Ui.ViewModel;

namespace SqlExport
{
	public class CustomExtensionOption : ExtensionOption
	{
		public CustomExtensionOption( string name )
			: base( name )
		{
		}

		/// <summary>
		/// Gets or sets the custom editor for the property, must return a <see cref="SqlExport.Ui.ViewModel.PropertyItem"/>.
		/// </summary>
        public Func<PropertyItem> GetCustomPropertyItem { get; set; }
	}
}

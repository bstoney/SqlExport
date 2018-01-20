using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.ViewModel;

namespace SqlExport.Messages
{
	public class SetPropertiesMessage
	{
		public SetPropertiesMessage( IEnumerable<PropertyItem> properties )
		{
			Properties = properties;
		}

		public IEnumerable<PropertyItem> Properties { get; set; }
	}
}

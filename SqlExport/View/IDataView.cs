using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.View
{
	public interface IDataView
	{
		object DataContext { get; set; }
	}
}

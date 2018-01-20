using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Tomers.WPF.DataVirtualization.Data;
using System.Collections;

namespace Tomers.WPF.DataVirtualization
{
	public class HugeCollection : ObservableCollection<object>, IDataVirtualizationItemSponsor
	{
		public HugeCollection( IEnumerable source )
		{
			foreach( var item in source )
			{
				Add( item );
			}
		}

		#region IDataVirtualizationItemSponsor Members

		public void ExtendItems( IEnumerable<object> items )
		{
			////foreach( Entry entry in items )
			////{
			////    entry.IsExtended = true;
			////}
		}

		public void DeflateItem( object item )
		{
			////var entry = item as Entry;
			////entry.IsExtended = false;
		}

		#endregion
	}
}

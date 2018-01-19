using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Data;
using System.Windows.Threading;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Diagnostics;

namespace Tomers.WPF.DataVirtualization.Data
{
	public class DataVirtualizationCollectionView : ListCollectionView, ICacheItemRefreshAction
	{
		private readonly IDataVirtualizationItemSponsor _sponsor;
		private readonly ICacheManager _cache = CacheFactory.GetCacheManager();
		private readonly HashSet<object> _deferredItems = new HashSet<object>();
		private bool _isDeferred;

		public DataVirtualizationCollectionView( IList list )
			: base( list )
		{
			this._sponsor = list as IDataVirtualizationItemSponsor;
		}

		public override object GetItemAt( int index )
		{
			if( !_isDeferred )
			{
				_deferredItems.Clear();

				Dispatcher.BeginInvoke( DispatcherPriority.Normal, (Action)LoadDeferredItems );

				_isDeferred = true;
			}

			var item = base.GetItemAt( index );
			if( !_deferredItems.Contains( item ) )
			{
				_deferredItems.Add( item );
			}

			return item;
		}

		#region ICacheItemRefreshAction Members

		public void Refresh( string removedKey, object expiredValue, CacheItemRemovedReason removalReason )
		{
			_sponsor.DeflateItem( expiredValue );
		}

		#endregion

		private void LoadDeferredItems()
		{
			var uniqueSet = new HashSet<object>();
			foreach( object item in _deferredItems )
			{
				var hashCode = item.GetHashCode();
				if( !_cache.Contains( hashCode.ToString() ) )
				{
					uniqueSet.Add( item );
				}

				_cache.Add( hashCode.ToString(), item, CacheItemPriority.Normal, this );
			}

			_sponsor.ExtendItems( uniqueSet );
			_isDeferred = false;
		}
	}
}

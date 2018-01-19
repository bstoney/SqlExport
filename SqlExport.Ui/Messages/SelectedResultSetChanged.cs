using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace SqlExport.Messages
{
	public class SelectedResultSetChanged
	{
		private object resultSetContext;

		internal SelectedResultSetChanged( object resultSetContext )
		{
			this.resultSetContext = resultSetContext;
		}

		/// <summary>
		/// Gets to row count of the currently selected result set.
		/// </summary>
		public int? GetResultCount()
		{
			int? count = null;
			var context = this.resultSetContext;
			if( context != null )
			{
				Messenger.Default.Send( new GetDataResultMessage( r => count = r.FetchCount() ), context );
			}

			return count;
		}
	}
}

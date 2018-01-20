using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;

namespace SqlExport.Data.Adapters.Linq
{
	internal class EmptyCommandAdapter : IDbCommandAdapter
	{
		private bool _hasTransaction = false;

		private void OnMessage( MessageType messageType, string message )
		{
			if( Message != null )
			{
				Message( messageType, message, null );
			}
		}

		#region IDbCommandAdapter Members

		public IDbConnection Connection
		{
			get { return null; }
		}

		public DbTransaction Transaction
		{
			get { return null; }
		}

		#endregion

		#region ICommandAdapter Members

		public event ConnectionMessageHandler Message;

		public void BeginTransaction()
		{
			_hasTransaction = true;
		}

		public void CommitTransaction()
		{
			RollbackTransaction();
		}

		public void RollbackTransaction()
		{
			_hasTransaction = false;
		}

		public bool HasTransaction
		{
			get { return _hasTransaction; }
		}

		public bool ExecuteCommand( TextReader sql, out DataSet results )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}

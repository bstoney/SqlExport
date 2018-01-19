using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Linq;

namespace SqlExport.Data.Adapters.SqLite
{
	internal class CommandAdapter : ICommandAdapter
	{
		private SQLiteConnection _connection;
		private SQLiteTransaction _transaction;
		private int _timeout;

		public CommandAdapter( string connectionString, int commandTimeout )
		{
			_connection = new SQLiteConnection( connectionString );
			_connection.Open();
			_timeout = commandTimeout;
		}

		internal SQLiteCommand CreateCommand( string sql )
		{
			SQLiteCommand cmd = _connection.CreateCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			cmd.CommandTimeout = _timeout;
			if( _transaction != null )
			{
				cmd.Transaction = _transaction;
			}
			return cmd;
		}

		#region ICommandAdapter Members

		public event ConnectionMessageHandler Message;

		public void BeginTransaction()
		{
			if( !HasTransaction )
			{
				_transaction = _connection.BeginTransaction();
			}
		}

		public void CommitTransaction()
		{
			if( HasTransaction )
			{
				_transaction.Commit();
				_transaction = null;
			}
		}

		public void RollbackTransaction()
		{
			if( HasTransaction )
			{
				_transaction.Rollback();
				_transaction = null;
			}
		}

		public bool HasTransaction
		{
			get { return _transaction != null; }
		}

		public IEnumerable<DataResult> ExecuteCommand( TextReader sql )
		{
			DataSet results = new DataSet();
			try
			{
				using( SQLiteCommand cmd = CreateCommand( sql.ReadToEnd() ) )
				{
					using( SQLiteDataAdapter adapter = new SQLiteDataAdapter( cmd ) )
					{
						adapter.Fill( results );
					}
				}
			}
			catch( SQLiteException exp )
			{
				if( Message != null )
				{
					Message( MessageType.Error, exp.Message, null );
				}

				throw new ExecuteCommandException();
			}

			return results.Tables.OfType<DataTable>().Select( t => DataResult.FromDataTable( t ) );
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if( _connection != null )
			{
				_connection.Close();
				_connection.Dispose();
			}
		}

		#endregion
	}
}

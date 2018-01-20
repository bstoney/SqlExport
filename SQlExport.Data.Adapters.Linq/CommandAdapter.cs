using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;
using System.Data;
using System.IO;
using System.Data.Linq;

namespace SqlExport.Data.Adapters.Linq
{
	internal class CommandAdapter : ICommandAdapter
	{
		private ScriptHelper _scriptHelper;
		private QueryCompiler _queryCompiler;
		private IDbCommandAdapter _innerAdapter;
		private IRelationalSchemaAdapter _scemaAdapter;
		private bool _hasBuiltObjectLibrary;
		private int _commandTimeout;

		public CommandAdapter( IDbCommandAdapter innerAdapter, IRelationalSchemaAdapter schemaAdapter, int commandTimeout, 
			string[] references, string[] usings )
		{
			_innerAdapter = innerAdapter;
			_scemaAdapter = schemaAdapter;
			_innerAdapter.Message += new ConnectionMessageHandler( OnInnerAdapterMessage );
			_scriptHelper = new ScriptHelper( usings );
			_queryCompiler = new QueryCompiler( references );
			_queryCompiler.Message += new ConnectionMessageHandler( OnQueryCompilerMessage );
			_commandTimeout = commandTimeout;
		}

		private void OnInnerAdapterMessage( MessageType type, string message, int? lineNumber )
		{
			OnMessage( type, message, lineNumber );
		}

		private void OnQueryCompilerMessage( MessageType type, string message, int? lineNumber )
		{
			OnMessage( type, message, lineNumber );
		}

		private void OnMessage( MessageType type, string message, int? lineNumber )
		{
			if( Message != null )
			{
				Message( type, message, lineNumber );
			}
		}

		private void BuildObjectLibrary()
		{
			string code = _scriptHelper.BuildObjectCode( _scemaAdapter );
			foreach( Exception exp in _scriptHelper.Errors )
			{
				OnMessage( MessageType.Warning, exp.Message, null );
			}
			_queryCompiler.CompileObjectLibrary( code );
			_hasBuiltObjectLibrary = true;
		}

		#region ICommandAdapter Members

		public event ConnectionMessageHandler Message;

		public void BeginTransaction()
		{
			// Warn the user when loading the empty adapter.
			if( _innerAdapter is EmptyCommandAdapter )
			{
				OnMessage( MessageType.Information, "Using empty Linq adapter.", null );
			}

			_innerAdapter.BeginTransaction();
			BuildObjectLibrary();
		}

		public void CommitTransaction()
		{
			_innerAdapter.CommitTransaction();
			_hasBuiltObjectLibrary = false;
		}

		public void RollbackTransaction()
		{
			_innerAdapter.RollbackTransaction();
			_hasBuiltObjectLibrary = false;
		}

		public bool HasTransaction
		{
			get { return _innerAdapter.HasTransaction; }
		}

		public bool ExecuteCommand( TextReader script, out DataSet results )
		{
			if( !_hasBuiltObjectLibrary )
			{
				BuildObjectLibrary();
			}	
			string code = _scriptHelper.BuildGeneratedQuery( script, _scemaAdapter );
			foreach( Exception exp in _scriptHelper.Errors )
			{
				OnMessage( MessageType.Warning, exp.Message, null );
			}
			results = _queryCompiler.CompileAndExecute( code, _scriptHelper.ScriptLineOffset, 
				_innerAdapter.Connection, _innerAdapter.Transaction, _commandTimeout );
			return results != null;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_innerAdapter.Dispose();
			_queryCompiler.Dispose();
		}

		#endregion
	}
}

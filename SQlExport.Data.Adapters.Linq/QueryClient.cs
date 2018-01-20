﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;

namespace SqlExport.Data.Adapters.Linq
{
	/// <summary>
	/// Client class for building ans executing c# statements.
	/// </summary>
	[Serializable]
	internal class QueryClient : MarshalByRefObject, IDisposable
	{
		private const string ErrorFormat = "Line {0}: error {1}: {2}";

		private IGeneratedScript _script;
		private CompilerResults _compileResults;
		private DataSet _results;
		private bool _hasCompiledLibrary = false;
		private string _objectLibraryPath = Path.Combine( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
			string.Concat( "_linq_", Path.GetRandomFileName(), ".dll" ) );

		/// <summary>
		/// Occurs when a message is generated by the client.
		/// </summary>
		public event ConnectionMessageHandler Message;

		/// <summary>
		/// Gets or sets a list of custom compiler references.
		/// </summary>
		public string[] References { get; set; }

		/// <summary>
		/// Gets the execution results of the script.
		/// </summary>
		public DataSet Results
		{
			get { return this._results; }
		}

		/// <summary>
		/// Compiles the script data context and object library.
		/// </summary>
		public bool CompileObjectLibrary( string script )
		{
			if( File.Exists( this._objectLibraryPath ) )
			{
				File.Delete( this._objectLibraryPath );
			}

			CompilerParameters cp = new CompilerParameters();
			cp.GenerateExecutable = false;
			cp.IncludeDebugInformation = false;
			cp.GenerateInMemory = false;
			cp.OutputAssembly = this._objectLibraryPath;
			cp.ReferencedAssemblies.Add( "System.dll" );
			cp.ReferencedAssemblies.Add( "System.Core.dll" );
			cp.ReferencedAssemblies.Add( "System.Data.dll" );
			cp.ReferencedAssemblies.Add( "System.Data.Linq.dll" );
			cp.ReferencedAssemblies.Add( Assembly.GetExecutingAssembly().Location );

			CodeDomProvider provider = new CSharpCodeProvider( new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } } );
			CompilerResults compileResults = provider.CompileAssemblyFromSource( cp, script );

			foreach( CompilerError error in compileResults.Errors )
			{
				// Cannot report line number here as the user never sees the source.
				this.OnMessage( MessageType.Error, error.ErrorText, null );
			}

			this._hasCompiledLibrary = !compileResults.Errors.HasErrors;
			return this._hasCompiledLibrary;
		}

		/// <summary>
		/// Compiles the script.
		/// </summary>
		public bool Compile( string script, int scriptLineOffset )
		{
			CompilerParameters cp = new CompilerParameters();
			cp.GenerateExecutable = false;
			cp.IncludeDebugInformation = false;
			cp.GenerateInMemory = true;
			cp.ReferencedAssemblies.Add( "System.dll" );
			cp.ReferencedAssemblies.Add( "System.Core.dll" );
			cp.ReferencedAssemblies.Add( "System.Data.dll" );
			cp.ReferencedAssemblies.Add( "System.Data.Linq.dll" );

			foreach( string item in this.References )
			{
				cp.ReferencedAssemblies.Add( item );
			}

			cp.ReferencedAssemblies.Add( Assembly.GetExecutingAssembly().Location );
			if( this._hasCompiledLibrary )
			{
				cp.ReferencedAssemblies.Add( this._objectLibraryPath );
			}

			CodeDomProvider provider = new CSharpCodeProvider( new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } } );
			this._compileResults = provider.CompileAssemblyFromSource( cp, script );

			foreach( CompilerError error in this._compileResults.Errors )
			{
				this.OnMessage( MessageType.Error,
					string.Format( ErrorFormat, error.Line - scriptLineOffset, error.ErrorNumber, error.ErrorText ),
					error.Line - scriptLineOffset );
			}

			return !this._compileResults.Errors.HasErrors;
		}

		/// <summary>
		/// Loads the compiled script ready for execution.
		/// </summary>
		public void Load( IDbConnection connection, DbTransaction transaction, int commandTimeout )
		{
			this._script = this._compileResults.CompiledAssembly.CreateInstance( "GeneratedQuery" ) as IGeneratedScript;
			this._script.InitDataContext( connection, transaction, commandTimeout );
		}

		/// <summary>
		/// Executes the script.
		/// </summary>
		public void Run()
		{
			try
			{
				this._results = new DataSet();
				OutputExtensions.RegisterClient( this );
				this._script.Execute();
			}
			finally
			{
				OutputExtensions.UnregisterClient( this );
			}
		}

		/// <summary>
		/// Adds a result table the the results data set.
		/// </summary>
		public void AddResult( DataTable table )
		{
			table.TableName = this._results.Tables.Count.ToString();
			this._results.Tables.Add( table.Copy() );
		}

		/// <summary>
		/// Raises the Message event.
		/// </summary>
		public void AddMessage( string message )
		{
			this.OnMessage( MessageType.Information, message, null );
		}

		private void OnMessage( MessageType messageType, string message, int? lineNumber )
		{
			if( this.Message != null )
			{
				this.Message( messageType, message, lineNumber );
			}
		}

		#region IDisposable Members

		/// <summary>
		/// Dispose the QueryClient object.
		/// </summary>
		public void Dispose()
		{
			if( this._hasCompiledLibrary && File.Exists( this._objectLibraryPath ) )
			{
				try
				{
					File.Delete( this._objectLibraryPath );
				}
				catch( Exception ) { }
			}
		}

		#endregion
	}
}
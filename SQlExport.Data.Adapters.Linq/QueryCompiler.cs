using System;
using System.IO.Pipes;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;
using System.Reflection;
using System.Security.Policy;

namespace SqlExport.Data.Adapters.Linq
{
	[Serializable]
	internal class QueryCompiler : MarshalByRefObject, IDisposable
	{
		private QueryClient _queryClient;
		private AppDomain _sandboxDomain;
		public event ConnectionMessageHandler Message;

		public QueryCompiler( string[] references )
		{
			_sandboxDomain = CreateSandboxDomain();
			Type type = typeof( QueryClient );
			_queryClient = (QueryClient)_sandboxDomain.CreateInstanceAndUnwrap( type.Assembly.FullName, type.FullName );
			_queryClient.References = references;
			_queryClient.Message += new ConnectionMessageHandler( OnQueryClientMessage );
		}

		private AppDomain CreateSandboxDomain()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();

			// create the permission set to grant other assemblies
			PermissionSet pset = new PermissionSet( PermissionState.Unrestricted );
			pset.AddPermission( new SecurityPermission( SecurityPermissionFlag.Execution ) );
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = Path.GetDirectoryName( executingAssembly.Location );

			// create the sandboxed domain
			return AppDomain.CreateDomain( "Remote Execute", AppDomain.CurrentDomain.Evidence, setup, pset,
				CreateStrongName( executingAssembly ) );
		}

		/// <summary>
		/// Create a StrongName that matches a specific assembly
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="assembly"/> is null
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// if <paramref name="assembly"/> does not represent a strongly named assembly
		/// </exception>
		/// <param name="assembly">Assembly to create a StrongName for</param>
		/// <returns>A StrongName that matches the given assembly</returns>
		public static StrongName CreateStrongName( Assembly assembly )
		{
			if( assembly == null )
			{
				throw new ArgumentNullException( "assembly" );
			}

			AssemblyName assemblyName = assembly.GetName();

			// get the public key blob
			byte[] publicKey = assemblyName.GetPublicKey();
			if( publicKey == null || publicKey.Length == 0 )
			{
				throw new InvalidOperationException( "Assembly is not strongly named" );
			}

			StrongNamePublicKeyBlob keyBlob = new StrongNamePublicKeyBlob( publicKey );

			// and create the StrongName
			return new StrongName( keyBlob, assemblyName.Name, assemblyName.Version );
		}

		public bool CompileObjectLibrary( string script )
		{
			return _queryClient.CompileObjectLibrary( script );
		}

		public DataSet CompileAndExecute( string script, int scriptLineOffset,
			IDbConnection connection, DbTransaction transaction, int commandTimeout )
		{
			DataSet results = null;
			if( _queryClient.Compile( script, scriptLineOffset ) )
			{
				_queryClient.Load( connection, transaction, commandTimeout );
				_queryClient.Run();
				results = _queryClient.Results;
			}
			return results;
		}

		private void OnQueryClientMessage( MessageType type, string message, int? lineNumber )
		{
			if( Message != null )
			{
				Message( type, message, lineNumber );
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			try
			{
				_queryClient.Dispose();
			}
			catch( Exception )
			{
				// Avoid causing an error here.
			}
			try
			{
				AppDomain.Unload( _sandboxDomain );
			}
			catch( Exception )
			{
				// Avoid causing an error here.
			}
		}

		#endregion
	}
}
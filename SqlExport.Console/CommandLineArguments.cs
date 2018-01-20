using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using System.Reflection;
using System.IO;

namespace SqlExport
{
	internal class CommandLineArguments
	{
		public bool NewInstance { get; private set; }

		/// <summary>
		/// Gets a value indicating whether to read the script from the standard input.
		/// </summary>
		public bool InputScript { get; private set; }

		public string ConnectionName { get; private set; }

		public string ConnectionType { get; private set; }

		public string ConnectionString { get; private set; }

		public string OutputType { get; private set; }

		public string OutputFile { get; private set; }

		public IEnumerable<string> Files { get; private set; }

		public void Parse( params string[] args )
		{
			if( args == null )
			{
				throw new ArgumentNullException( "args" );
			}

			var p = new OptionSet() {
				{ "name=", v => ConnectionName = v },
				{ "type=", v => ConnectionType = v },
				{ "cnn=", v => ConnectionString = v },
				{ "out=", v => OutputType = v },
				{ "outfile=", v => OutputFile = v },
				{ "script", var => InputScript = true },
				{ "h|?", v => ShowHelp( true, null ) }
			};

			Files = p.Parse( args );
		}

		public static void ShowHelp( bool exitOnCompletion, string message )
		{
			if( !string.IsNullOrEmpty( message) )
			{
				Console.WriteLine( message );
				Console.WriteLine();
			}

			string cmd = Path.GetFileName( Assembly.GetEntryAssembly().Location );
			Console.WriteLine( "Usage: " + cmd + " -exec -type=<Type> -cnn=<connection string> [-out=<output type>] [-outfile=<output file>] <filename1, ...>" );
			Console.WriteLine( "       " + cmd + " -exec -name=<connection name> -script" );
			Console.WriteLine( "       " + cmd + " [-new] [<filename1, ...>]" );
			Console.WriteLine();
			Console.WriteLine( "  -new       Start a new GUI instance." );
			Console.WriteLine( "  -exec      Execute a script." );
			Console.WriteLine( "  -name=<name>" );
			Console.WriteLine( "             Use a named connection." );
			Console.WriteLine( "  -type=<type>" );
			Console.WriteLine( "             Connection Type." );
			Console.WriteLine( "  -cnn=<connection string>" );
			Console.WriteLine( "             Connection string." );
			Console.WriteLine( "  -out=<output type>" );
			Console.WriteLine( "             Output type." );
			Console.WriteLine( "  -outfile=<output file>" );
			Console.WriteLine( "             Output file name, uses the console as the default." );
			Console.WriteLine( "  -h | -?    Show help." );

			if( exitOnCompletion )
			{
				Environment.Exit( 0 );
			}
		}
	}
}

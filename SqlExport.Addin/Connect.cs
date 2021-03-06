using System;
using System.Reflection;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;

namespace SqlExport.Addin
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{

		private DTE2 _applicationObject;
		private AddIn _addInInstance;

		private static int _windowCount;

		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection( object application, ext_ConnectMode connectMode, object addInInst, ref Array custom )
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;

			Commands commands = _applicationObject.Commands;
			CommandBars commandBars = (CommandBars)_applicationObject.CommandBars;

			try
			{
				object[] contextGUIDS = new object[] { };
				Command command = commands.AddNamedCommand( _addInInstance, "SqlExportQuery", "Sql Query",
					"Opens a new SqlExport query window.", false, 1, ref contextGUIDS,
					(int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled );

				CommandBar commandBar = (CommandBar)commandBars["Tools"];
				CommandBarControl commandBarControl = (CommandBarControl)command.AddControl( commandBar, 1 );
			}
			catch( Exception )
			{
			}
		}

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection( ext_DisconnectMode disconnectMode, ref Array custom )
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate( ref Array custom )
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete( ref Array custom )
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown( ref Array custom )
		{
		}

		private void CreateQueryWindow()
		{
			_windowCount++;

			object ctlobj = null;
			Window newWinobj;

			Windows2 wins2obj = (Windows2)_applicationObject.Windows;

			Type controlType = typeof( QueryControl );

			Program.InitialiseEnvironment();

			// Create the new tool window and insert the user control in it.
			// "{36cae730-369f-11dd-ae16-0800200c9a66}"
			newWinobj = wins2obj.CreateToolWindow2( _addInInstance, Assembly.GetAssembly( controlType ).Location,
				controlType.FullName, string.Concat( "Query ", _windowCount ), Guid.NewGuid().ToString( "B" ), ref ctlobj );
			newWinobj.Visible = true;

			_applicationObject.ExecuteCommand( "Window.TabbedDocument", "" );

			QueryControl qc = ctlobj as QueryControl;
			if( qc != null )
			{
				qc.Run += new EventHandler( OnRun );
				qc.Stop += new EventHandler( OnStop );
				qc.StatusChanged += new EventHandler( OnStatusChanged );
				qc.Tag = newWinobj;
			}
		}

		private void OnStatusChanged( object sender, EventArgs e )
		{
			QueryControl qc = sender as QueryControl;
			if( qc != null )
			{
				SetStatus( qc );
			}
		}

		private void OnStop( object sender, EventArgs e )
		{
			QueryControl qc = sender as QueryControl;
			if( qc != null )
			{
				SetStatus( qc );
			}
		}

		private void OnRun( object sender, EventArgs e )
		{
			QueryControl qc = sender as QueryControl;
			if( qc != null )
			{
				SetStatus( qc );
			}
		}

		private static void SetStatus( QueryControl query )
		{
			Window w = query.Tag as Window;
			if( w != null )
			{
				w.Caption = query.DisplayText;
				if( query.IsRunning || query.IsExecuting )
				{
					// TODO tp.ImageIndex = 0;
				}
				else if( query.HasTransaction )
				{
					// TODO tp.ImageIndex = 2;
				}
				else if( query.HasConnection )
				{
					// TODO tp.ImageIndex = 1;
				}
				else
				{
					// TODO tp.ImageIndex = -1;
				}
			}
		}

		#region IDTCommandTarget Members

		public void QueryStatus( string commandName, vsCommandStatusTextWanted neededText,
			ref vsCommandStatus status, ref object commandText )
		{
			if( neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone )
			{
				if( commandName == "SqlExport.Addin.Connect.SqlExportQuery" )
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported |
						vsCommandStatus.vsCommandStatusEnabled;
					return;
				}
			}
		}

		public void Exec( string commandName, vsCommandExecOption executeOption,
			ref object varIn, ref object varOut, ref bool handled )
		{
			handled = false;
			if( executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault )
			{
				if( commandName == "SqlExport.Addin.Connect.SqlExportQuery" )
				{
					CreateQueryWindow();
					handled = true;
					return;
				}
			}
		}
		#endregion
	}
}

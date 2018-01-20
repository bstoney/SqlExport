using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SqlExport.Ui;
using SqlExport.Editor;
using SqlExport.Data;
using System.IO;

namespace SqlExport.Addin
{
	public partial class QueryControl : QueryAreaBase
	{
		private QueryPane _query;

		public QueryControl()
		{
			InitializeComponent();

			DatabaseDetails database = null;
			string filename = null;
			string sql = null;
			bool hasChanged = false;

			_query = new QueryPane();

			_query.Dock = DockStyle.Fill;
			_query.SetEditorStyle( DefaultOptions.GetEditorStyle() );
			_query.SetDatabase( database );

			_query.Stop += new EventHandler( OnQueryStop );
			_query.Run += new EventHandler( OnQueryRun );
			_query.StateChanged += new EventHandler( OnQueryStateChanged );

			_query.Filename = filename;
			if( !String.IsNullOrEmpty( filename ) )
			{
				if( File.Exists( filename ) )
				{
					_query.Load();
				}
				else
				{
					ErrorDialog.AddError( String.Concat( "The file '", filename, "' could not be found." ), null );
					ErrorDialog.ShowForm();
				}
			}

			Controls.Add( _query );

			if( !String.IsNullOrEmpty( sql ) )
			{
				_query.Text = sql;
			}

			_query.HasChanged = hasChanged;
		}

		public override bool IsRunning
		{
			get { return _query.IsRunning; }
		}

		public override bool IsExecuting
		{
			get { return _query.IsExecuting; }
		}

		public override bool HasConnection
		{
			get { return _query.HasConnection; }
		}

		public override bool HasTransaction
		{
			get { return _query.HasTransaction; }
		}

		public override string DisplayText
		{
			get { return GetDisplayText( _query ); }
		}

		public override void RunQuery()
		{
			_query.RunQuery();
		}

		public override void StopQuery()
		{
			_query.StopQuery();
		}

		public override void ChooseDatabase()
		{
			_query.ChooseDatabase();
		}

		public override void Connect()
		{
			_query.Connect();
		}

		public override void Disconnect()
		{
			_query.Disconnect();
		}

		public override void BeginTransaction()
		{
			_query.BeginTransaction();
		}

		public override void CommitTransaction()
		{
			_query.CommitTransaction();
		}

		public override void RollbackTransaction()
		{
			_query.RollbackTransaction();
		}

		#region Event Handlers

		private void OnQueryRun( object sender, EventArgs e )
		{
			OnRun();
		}

		private void OnQueryStop( object sender, EventArgs e )
		{
			OnStop();
		}

		private void OnQueryStateChanged( object sender, EventArgs e )
		{
			OnStatusChanged();
		}

		#endregion
	}
}

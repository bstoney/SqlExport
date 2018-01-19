namespace SqlExport.Ui
{
	partial class QueryAreaBase
	{
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mnuActions = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.mnuRun = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisconnect = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBeginTransaction = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCommitTransaction = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRollbackTransaction = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSetDatabase = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuActions.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuActions
			// 
			this.mnuActions.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mnuRun,
            this.mnuStop,
            this.toolStripMenuItem1,
            this.mnuConnect,
            this.mnuDisconnect,
            this.mnuBeginTransaction,
            this.mnuCommitTransaction,
            this.mnuRollbackTransaction,
            this.toolStripMenuItem2,
            this.mnuSetDatabase} );
			this.mnuActions.Name = "mnuActions";
			this.mnuActions.Size = new System.Drawing.Size( 171, 192 );
			// 
			// mnuRun
			// 
			this.mnuRun.Image = global::SqlExport.Properties.Resources.go;
			this.mnuRun.Name = "mnuRun";
			this.mnuRun.Size = new System.Drawing.Size( 170, 22 );
			this.mnuRun.Text = "Run";
			// 
			// mnuStop
			// 
			this.mnuStop.Enabled = false;
			this.mnuStop.Image = global::SqlExport.Properties.Resources.stop;
			this.mnuStop.Name = "mnuStop";
			this.mnuStop.Size = new System.Drawing.Size( 170, 22 );
			this.mnuStop.Text = "Stop";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size( 167, 6 );
			// 
			// mnuConnect
			// 
			this.mnuConnect.Image = global::SqlExport.Properties.Resources.connect;
			this.mnuConnect.Name = "mnuConnect";
			this.mnuConnect.Size = new System.Drawing.Size( 170, 22 );
			this.mnuConnect.Text = "Connect";
			// 
			// mnuDisconnect
			// 
			this.mnuDisconnect.Enabled = false;
			this.mnuDisconnect.Image = global::SqlExport.Properties.Resources.disconnect;
			this.mnuDisconnect.Name = "mnuDisconnect";
			this.mnuDisconnect.Size = new System.Drawing.Size( 170, 22 );
			this.mnuDisconnect.Text = "Disconnect";
			// 
			// mnuBeginTransaction
			// 
			this.mnuBeginTransaction.Image = global::SqlExport.Properties.Resources.table_lightning;
			this.mnuBeginTransaction.Name = "mnuBeginTransaction";
			this.mnuBeginTransaction.Size = new System.Drawing.Size( 170, 22 );
			this.mnuBeginTransaction.Text = "Begin Transaction";
			// 
			// mnuCommitTransaction
			// 
			this.mnuCommitTransaction.Enabled = false;
			this.mnuCommitTransaction.Image = global::SqlExport.Properties.Resources.table_row_insert;
			this.mnuCommitTransaction.Name = "mnuCommitTransaction";
			this.mnuCommitTransaction.Size = new System.Drawing.Size( 170, 22 );
			this.mnuCommitTransaction.Text = "Commit";
			// 
			// mnuRollbackTransaction
			// 
			this.mnuRollbackTransaction.Enabled = false;
			this.mnuRollbackTransaction.Image = global::SqlExport.Properties.Resources.table_row_delete;
			this.mnuRollbackTransaction.Name = "mnuRollbackTransaction";
			this.mnuRollbackTransaction.Size = new System.Drawing.Size( 170, 22 );
			this.mnuRollbackTransaction.Text = "Rollback";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size( 167, 6 );
			// 
			// mnuSetDatabase
			// 
			this.mnuSetDatabase.Image = global::SqlExport.Properties.Resources.database_connect;
			this.mnuSetDatabase.Name = "mnuSetDatabase";
			this.mnuSetDatabase.Size = new System.Drawing.Size( 170, 22 );
			this.mnuSetDatabase.Text = "Set Database";
			// 
			// QueryAreaBase
			// 
			this.Name = "QueryAreaBase";
			this.mnuActions.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ContextMenuStrip mnuActions;
		private System.Windows.Forms.ToolStripMenuItem mnuSetDatabase;
		private System.Windows.Forms.ToolStripMenuItem mnuRun;
		private System.Windows.Forms.ToolStripMenuItem mnuStop;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuConnect;
		private System.Windows.Forms.ToolStripMenuItem mnuDisconnect;
		private System.Windows.Forms.ToolStripMenuItem mnuBeginTransaction;
		private System.Windows.Forms.ToolStripMenuItem mnuCommitTransaction;
		private System.Windows.Forms.ToolStripMenuItem mnuRollbackTransaction;
	}
}

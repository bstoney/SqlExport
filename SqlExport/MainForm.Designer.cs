using SqlExport.Ui;
namespace SqlExport
{
	partial class MainForm
	{
		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnClose;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnRun;
		private System.Windows.Forms.ToolStripButton btnStop;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnExport;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripTextBox txtSearch;
		private System.Windows.Forms.ToolStripTextBox txtReplace;
		private System.Windows.Forms.ToolStripButton btnFind;
		private System.Windows.Forms.ToolStripButton btnReplace;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripComboBox ddlConnection;
		private System.Windows.Forms.ToolStripButton btnSetConnection;
        private System.Windows.Forms.ToolStripButton btnAddConnection;

		//Form overrides dispose to clean up the component list.
		protected override void Dispose( bool Disposing )
		{
			if( Disposing )
			{
				if( components != null )
				{
					components.Dispose();
				}
			}
			base.Dispose( Disposing );
		}

		private void InitializeComponent()
		{
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripSplitButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.txtReplace = new System.Windows.Forms.ToolStripTextBox();
            this.btnFind = new System.Windows.Forms.ToolStripButton();
            this.btnReplace = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ddlConnection = new System.Windows.Forms.ToolStripComboBox();
            this.btnSetConnection = new System.Windows.Forms.ToolStripButton();
            this.btnAddConnection = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveTemp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile6ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuActions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRun = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBeginTransaction = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCommitTransaction = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRollbackTransaction = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExportResults = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AllowDrop = true;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnClose,
            this.toolStripSeparator1,
            this.btnRun,
            this.btnStop,
            this.toolStripSeparator2,
            this.btnExport,
            this.toolStripSeparator3,
            this.txtSearch,
            this.txtReplace,
            this.btnFind,
            this.btnReplace,
            this.toolStripSeparator4,
            this.ddlConnection,
            this.btnSetConnection,
            this.btnAddConnection});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(737, 25);
            this.toolStrip1.TabIndex = 61;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = global::SqlExport.Properties.Resources.script_add;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(32, 22);
            this.btnNew.Text = "toolStripButton1";
            this.btnNew.ToolTipText = "Add new query window.";
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = global::SqlExport.Properties.Resources.folder_page;
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.Text = "toolStripButton2";
            this.btnOpen.ToolTipText = "Open existing query.";
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = global::SqlExport.Properties.Resources.script_save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "toolStripButton4";
            this.btnSave.ToolTipText = "Save current query.";
            // 
            // btnClose
            // 
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClose.Image = global::SqlExport.Properties.Resources.script_delete;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 22);
            this.btnClose.Text = "toolStripButton3";
            this.btnClose.ToolTipText = "Close current query.";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRun
            // 
            this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRun.Image = global::SqlExport.Properties.Resources.go;
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(23, 22);
            this.btnRun.Text = "toolStripButton1";
            this.btnRun.ToolTipText = "Run current query.";
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Enabled = false;
            this.btnStop.Image = global::SqlExport.Properties.Resources.stop;
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(23, 22);
            this.btnStop.Text = "toolStripButton2";
            this.btnStop.ToolTipText = "Stop currently running query.";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExport
            // 
            this.btnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExport.Image = global::SqlExport.Properties.Resources.table_go;
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(23, 22);
            this.btnExport.Text = "Export";
            this.btnExport.ToolTipText = "Export results to a spreadsheet.";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // txtSearch
            // 
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 25);
            // 
            // txtReplace
            // 
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(100, 25);
            // 
            // btnFind
            // 
            this.btnFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFind.Image = global::SqlExport.Properties.Resources.find;
            this.btnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(23, 22);
            this.btnFind.Text = "toolStripButton4";
            this.btnFind.ToolTipText = "Find text in current query.";
            // 
            // btnReplace
            // 
            this.btnReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnReplace.Image = global::SqlExport.Properties.Resources.replace;
            this.btnReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(23, 22);
            this.btnReplace.Text = "toolStripButton5";
            this.btnReplace.ToolTipText = "Find and replace text in current query.";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // ddlConnection
            // 
            this.ddlConnection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlConnection.Name = "ddlConnection";
            this.ddlConnection.Size = new System.Drawing.Size(125, 25);
            // 
            // btnSetConnection
            // 
            this.btnSetConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSetConnection.Image = global::SqlExport.Properties.Resources.database_connect;
            this.btnSetConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetConnection.Name = "btnSetConnection";
            this.btnSetConnection.Size = new System.Drawing.Size(23, 22);
            this.btnSetConnection.Text = "toolStripButton6";
            this.btnSetConnection.ToolTipText = "Set selected database as the current connection.";
            // 
            // btnAddConnection
            // 
            this.btnAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddConnection.Image = global::SqlExport.Properties.Resources.database_add;
            this.btnAddConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddConnection.Name = "btnAddConnection";
            this.btnAddConnection.Size = new System.Drawing.Size(23, 22);
            this.btnAddConnection.Text = "toolStripButton7";
            this.btnAddConnection.ToolTipText = "Add database listing to tree view.";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.editToolStripMenuItem,
            this.mnuActions,
            this.mnuOptions});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(737, 24);
            this.menuStrip1.TabIndex = 67;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuClose,
            this.mnuSave,
            this.mnuSaveAs,
            this.mnuSaveTemp,
            this.toolStripMenuItem3,
            this.mnuRecentFiles,
            this.toolStripMenuItem4,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuNew
            // 
            this.mnuNew.Image = global::SqlExport.Properties.Resources.script_add;
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(136, 22);
            this.mnuNew.Text = "&New";
            // 
            // mnuOpen
            // 
            this.mnuOpen.Image = global::SqlExport.Properties.Resources.folder_page;
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(136, 22);
            this.mnuOpen.Text = "&Open";
            // 
            // mnuClose
            // 
            this.mnuClose.Image = global::SqlExport.Properties.Resources.script_delete;
            this.mnuClose.Name = "mnuClose";
            this.mnuClose.Size = new System.Drawing.Size(136, 22);
            this.mnuClose.Text = "&Close";
            // 
            // mnuSave
            // 
            this.mnuSave.Image = global::SqlExport.Properties.Resources.script_save;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(136, 22);
            this.mnuSave.Text = "&Save";
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(136, 22);
            this.mnuSaveAs.Text = "Save &As";
            // 
            // mnuSaveTemp
            // 
            this.mnuSaveTemp.Name = "mnuSaveTemp";
            this.mnuSaveTemp.Size = new System.Drawing.Size(136, 22);
            this.mnuSaveTemp.Text = "Save Temp";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(133, 6);
            // 
            // mnuRecentFiles
            // 
            this.mnuRecentFiles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recentFile1ToolStripMenuItem,
            this.recentFile2ToolStripMenuItem,
            this.recentFile3ToolStripMenuItem,
            this.recentFile4ToolStripMenuItem,
            this.recentFile5ToolStripMenuItem,
            this.recentFile6ToolStripMenuItem,
            this.recentFile7ToolStripMenuItem,
            this.recentFile8ToolStripMenuItem});
            this.mnuRecentFiles.Name = "mnuRecentFiles";
            this.mnuRecentFiles.Size = new System.Drawing.Size(136, 22);
            this.mnuRecentFiles.Text = "&Recent Files";
            // 
            // recentFile1ToolStripMenuItem
            // 
            this.recentFile1ToolStripMenuItem.Name = "recentFile1ToolStripMenuItem";
            this.recentFile1ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile1ToolStripMenuItem.Text = "Recent File 1";
            // 
            // recentFile2ToolStripMenuItem
            // 
            this.recentFile2ToolStripMenuItem.Name = "recentFile2ToolStripMenuItem";
            this.recentFile2ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile2ToolStripMenuItem.Text = "Recent File 2";
            // 
            // recentFile3ToolStripMenuItem
            // 
            this.recentFile3ToolStripMenuItem.Name = "recentFile3ToolStripMenuItem";
            this.recentFile3ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile3ToolStripMenuItem.Text = "Recent File 3";
            // 
            // recentFile4ToolStripMenuItem
            // 
            this.recentFile4ToolStripMenuItem.Name = "recentFile4ToolStripMenuItem";
            this.recentFile4ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile4ToolStripMenuItem.Text = "Recent File 4";
            // 
            // recentFile5ToolStripMenuItem
            // 
            this.recentFile5ToolStripMenuItem.Name = "recentFile5ToolStripMenuItem";
            this.recentFile5ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile5ToolStripMenuItem.Text = "Recent File 5";
            // 
            // recentFile6ToolStripMenuItem
            // 
            this.recentFile6ToolStripMenuItem.Name = "recentFile6ToolStripMenuItem";
            this.recentFile6ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile6ToolStripMenuItem.Text = "Recent File 6";
            // 
            // recentFile7ToolStripMenuItem
            // 
            this.recentFile7ToolStripMenuItem.Name = "recentFile7ToolStripMenuItem";
            this.recentFile7ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile7ToolStripMenuItem.Text = "Recent File 7";
            // 
            // recentFile8ToolStripMenuItem
            // 
            this.recentFile8ToolStripMenuItem.Name = "recentFile8ToolStripMenuItem";
            this.recentFile8ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.recentFile8ToolStripMenuItem.Text = "Recent File 8";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(133, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(136, 22);
            this.mnuExit.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // mnuActions
            // 
            this.mnuActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRun,
            this.mnuStop,
            this.toolStripMenuItem6,
            this.mnuConnect,
            this.mnuDisconnect,
            this.mnuBeginTransaction,
            this.mnuCommitTransaction,
            this.mnuRollbackTransaction,
            this.toolStripMenuItem1,
            this.mnuExportResults,
            this.toolStripMenuItem2,
            this.mnuShowErrors});
            this.mnuActions.Name = "mnuActions";
            this.mnuActions.Size = new System.Drawing.Size(54, 20);
            this.mnuActions.Text = "&Action";
            // 
            // mnuRun
            // 
            this.mnuRun.Image = global::SqlExport.Properties.Resources.go;
            this.mnuRun.Name = "mnuRun";
            this.mnuRun.Size = new System.Drawing.Size(169, 22);
            this.mnuRun.Text = "&Run";
            // 
            // mnuStop
            // 
            this.mnuStop.Image = global::SqlExport.Properties.Resources.stop;
            this.mnuStop.Name = "mnuStop";
            this.mnuStop.Size = new System.Drawing.Size(169, 22);
            this.mnuStop.Text = "&Stop";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(166, 6);
            // 
            // mnuConnect
            // 
            this.mnuConnect.Image = global::SqlExport.Properties.Resources.connect;
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(169, 22);
            this.mnuConnect.Text = "Connect";
            // 
            // mnuDisconnect
            // 
            this.mnuDisconnect.Image = global::SqlExport.Properties.Resources.disconnect;
            this.mnuDisconnect.Name = "mnuDisconnect";
            this.mnuDisconnect.Size = new System.Drawing.Size(169, 22);
            this.mnuDisconnect.Text = "Disconnect";
            // 
            // mnuBeginTransaction
            // 
            this.mnuBeginTransaction.Image = global::SqlExport.Properties.Resources.table_lightning;
            this.mnuBeginTransaction.Name = "mnuBeginTransaction";
            this.mnuBeginTransaction.Size = new System.Drawing.Size(169, 22);
            this.mnuBeginTransaction.Text = "Begin Transaction";
            // 
            // mnuCommitTransaction
            // 
            this.mnuCommitTransaction.Image = global::SqlExport.Properties.Resources.table_row_insert;
            this.mnuCommitTransaction.Name = "mnuCommitTransaction";
            this.mnuCommitTransaction.Size = new System.Drawing.Size(169, 22);
            this.mnuCommitTransaction.Text = "Commit";
            // 
            // mnuRollbackTransaction
            // 
            this.mnuRollbackTransaction.Image = global::SqlExport.Properties.Resources.table_row_delete;
            this.mnuRollbackTransaction.Name = "mnuRollbackTransaction";
            this.mnuRollbackTransaction.Size = new System.Drawing.Size(169, 22);
            this.mnuRollbackTransaction.Text = "Rollback";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(166, 6);
            // 
            // mnuExportResults
            // 
            this.mnuExportResults.Image = global::SqlExport.Properties.Resources.table_go;
            this.mnuExportResults.Name = "mnuExportResults";
            this.mnuExportResults.Size = new System.Drawing.Size(169, 22);
            this.mnuExportResults.Text = "&Export Results";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(166, 6);
            // 
            // mnuShowErrors
            // 
            this.mnuShowErrors.Name = "mnuShowErrors";
            this.mnuShowErrors.Size = new System.Drawing.Size(169, 22);
            this.mnuShowErrors.Text = "&Show Errors";
            // 
            // mnuOptions
            // 
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(61, 20);
            this.mnuOptions.Text = "&Options";
            // 
            // MainForm
            // 
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(11, 15);
            this.Name = "MainForm";
            this.Size = new System.Drawing.Size(737, 357);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private System.Windows.Forms.ToolStripSplitButton btnNew;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFiles;
		private System.Windows.Forms.ToolStripMenuItem recentFile1ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFile2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFile3ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFile4ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFile5ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFile6ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFile7ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFile8ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuActions;
		private System.Windows.Forms.ToolStripMenuItem mnuRun;
		private System.Windows.Forms.ToolStripMenuItem mnuStop;
		private System.Windows.Forms.ToolStripMenuItem mnuExportResults;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuShowErrors;
		private System.Windows.Forms.ToolStripMenuItem mnuOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuNew;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveTemp;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem mnuBeginTransaction;
		private System.Windows.Forms.ToolStripMenuItem mnuDisconnect;
		private System.Windows.Forms.ToolStripMenuItem mnuConnect;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuCommitTransaction;
		private System.Windows.Forms.ToolStripMenuItem mnuRollbackTransaction;
	}
}

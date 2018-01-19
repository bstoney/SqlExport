namespace SqlExport.Ui
{
	partial class ObjectView
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ObjectView ) );
			this.tmrDragDrop = new System.Windows.Forms.Timer( this.components );
			this.lstObjectTree = new SqlExport.Ui.MultiSelectTreeview();
			this.mnuActions = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.mnuLoad = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGetList = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGetSource = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSortByDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSortAlphabetically = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSeperator = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAddDatabase = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuActions.SuspendLayout();
			this.SuspendLayout();
			// 
			// tmrDragDrop
			// 
			this.tmrDragDrop.Interval = 500;
			// 
			// lstObjectTree
			// 
			this.lstObjectTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstObjectTree.Location = new System.Drawing.Point( 0, 0 );
			this.lstObjectTree.Name = "lstObjectTree";
			this.lstObjectTree.SelectedNodes = ((System.Collections.Generic.List<System.Windows.Forms.TreeNode>)(resources.GetObject( "lstObjectTree.SelectedNodes" )));
			this.lstObjectTree.Size = new System.Drawing.Size( 150, 150 );
			this.lstObjectTree.TabIndex = 0;
			// 
			// mnuActions
			// 
			this.mnuActions.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mnuLoad,
            this.mnuGetList,
            this.mnuGetSource,
            this.mnuSortByDefault,
            this.mnuSortAlphabetically,
            this.mnuRemove,
            this.mnuSeperator,
            this.mnuAddDatabase} );
			this.mnuActions.Name = "mnuActions";
			this.mnuActions.Size = new System.Drawing.Size( 190, 186 );
			// 
			// mnuLoad
			// 
			this.mnuLoad.Name = "mnuLoad";
			this.mnuLoad.Size = new System.Drawing.Size( 189, 22 );
			this.mnuLoad.Text = "Load/Reload";
			// 
			// mnuGetList
			// 
			this.mnuGetList.Name = "mnuGetList";
			this.mnuGetList.Size = new System.Drawing.Size( 189, 22 );
			this.mnuGetList.Text = "Get Item List";
			// 
			// mnuGetSource
			// 
			this.mnuGetSource.Name = "mnuGetSource";
			this.mnuGetSource.Size = new System.Drawing.Size( 189, 22 );
			this.mnuGetSource.Text = "Get Source";
			// 
			// mnuSortByDefault
			// 
			this.mnuSortByDefault.Name = "mnuSortByDefault";
			this.mnuSortByDefault.Size = new System.Drawing.Size( 189, 22 );
			this.mnuSortByDefault.Text = "Sort by Default Order";
			// 
			// mnuSortAlphabetically
			// 
			this.mnuSortAlphabetically.Name = "mnuSortAlphabetically";
			this.mnuSortAlphabetically.Size = new System.Drawing.Size( 189, 22 );
			this.mnuSortAlphabetically.Text = "Sort Alphabetically";
			// 
			// mnuRemove
			// 
			this.mnuRemove.Name = "mnuRemove";
			this.mnuRemove.Size = new System.Drawing.Size( 189, 22 );
			this.mnuRemove.Text = "Remove";
			// 
			// mnuSeperator
			// 
			this.mnuSeperator.Name = "mnuSeperator";
			this.mnuSeperator.Size = new System.Drawing.Size( 186, 6 );
			// 
			// mnuAddDatabase
			// 
			this.mnuAddDatabase.Name = "mnuAddDatabase";
			this.mnuAddDatabase.Size = new System.Drawing.Size( 189, 22 );
			this.mnuAddDatabase.Text = "Add Database";
			// 
			// ObjectView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.lstObjectTree );
			this.Name = "ObjectView";
			this.mnuActions.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private SqlExport.Ui.MultiSelectTreeview lstObjectTree;
		private System.Windows.Forms.Timer tmrDragDrop;
		private System.Windows.Forms.ContextMenuStrip mnuActions;
		private System.Windows.Forms.ToolStripMenuItem mnuLoad;
		private System.Windows.Forms.ToolStripMenuItem mnuGetList;
		private System.Windows.Forms.ToolStripMenuItem mnuGetSource;
		private System.Windows.Forms.ToolStripMenuItem mnuSortAlphabetically;
		private System.Windows.Forms.ToolStripMenuItem mnuSortByDefault;
		private System.Windows.Forms.ToolStripSeparator mnuSeperator;
		private System.Windows.Forms.ToolStripMenuItem mnuAddDatabase;
		private System.Windows.Forms.ToolStripMenuItem mnuRemove;
	}
}

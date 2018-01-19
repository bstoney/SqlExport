namespace SqlExport.Ui
{
	partial class QueryPane
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
			this.tmrUpdate = new System.Windows.Forms.Timer( this.components );
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.lstObjects = new SqlExport.Ui.ObjectView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.txtView = new SqlExport.Ui.SqlView();
			this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
			this.ctlResults = new SqlExport.View.ResultsPanel();
			this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			this.pnlStatus = new SqlExport.View.StatusPanelView();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tmrUpdate
			// 
			this.tmrUpdate.Interval = 10;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add( this.lstObjects );
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add( this.splitContainer1 );
			this.splitContainer2.Size = new System.Drawing.Size( 697, 406 );
			this.splitContainer2.SplitterDistance = 183;
			this.splitContainer2.TabIndex = 1;
			// 
			// lstObjects
			// 
			this.lstObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstObjects.Location = new System.Drawing.Point( 0, 0 );
			this.lstObjects.Name = "lstObjects";
			this.lstObjects.Size = new System.Drawing.Size( 183, 406 );
			this.lstObjects.TabIndex = 65;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add( this.txtView );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.elementHost2 );
			this.splitContainer1.Panel2.Controls.Add( this.elementHost1 );
			this.splitContainer1.Size = new System.Drawing.Size( 510, 406 );
			this.splitContainer1.SplitterDistance = 139;
			this.splitContainer1.TabIndex = 1;
			// 
			// txtView
			// 
			this.txtView.CursorPosLength = 0;
			this.txtView.CursorPosStart = 0;
			this.txtView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtView.Location = new System.Drawing.Point( 0, 0 );
			this.txtView.Name = "txtView";
			this.txtView.Size = new System.Drawing.Size( 510, 139 );
			this.txtView.TabIndex = 0;
			// 
			// elementHost2
			// 
			this.elementHost2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementHost2.Location = new System.Drawing.Point( 0, 0 );
			this.elementHost2.Name = "elementHost2";
			this.elementHost2.Size = new System.Drawing.Size( 510, 244 );
			this.elementHost2.TabIndex = 3;
			this.elementHost2.Text = "elementHost2";
			this.elementHost2.Child = this.ctlResults;
			// 
			// elementHost1
			// 
			this.elementHost1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.elementHost1.Location = new System.Drawing.Point( 0, 244 );
			this.elementHost1.Margin = new System.Windows.Forms.Padding( 0 );
			this.elementHost1.Name = "elementHost1";
			this.elementHost1.Size = new System.Drawing.Size( 510, 19 );
			this.elementHost1.TabIndex = 2;
			this.elementHost1.TabStop = false;
			this.elementHost1.Text = "elementHost1";
			this.elementHost1.Child = this.pnlStatus;
			// 
			// QueryPane
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.splitContainer2 );
			this.Name = "QueryPane";
			this.Size = new System.Drawing.Size( 697, 406 );
			this.splitContainer2.Panel1.ResumeLayout( false );
			this.splitContainer2.Panel2.ResumeLayout( false );
			this.splitContainer2.ResumeLayout( false );
			this.splitContainer1.Panel1.ResumeLayout( false );
			this.splitContainer1.Panel2.ResumeLayout( false );
			this.splitContainer1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Timer tmrUpdate;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private SqlView txtView;
		private ObjectView lstObjects;
		private System.Windows.Forms.Integration.ElementHost elementHost1;
		private SqlExport.View.StatusPanelView pnlStatus;
		private System.Windows.Forms.Integration.ElementHost elementHost2;
        private SqlExport.View.ResultsPanel ctlResults;
	}
}

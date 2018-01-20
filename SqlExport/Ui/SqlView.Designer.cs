using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SqlExport.Ui
{
	internal partial class SqlView
	{
		private ContextMenuStrip mnuContext;
		private System.ComponentModel.IContainer components;

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopySpecial = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopyCS = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopyVB = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopySource = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPasteSpecial = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPasteVB = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPasteCS = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuContext
            // 
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUndo,
            this.mnuRedo,
            this.toolStripMenuItem1,
            this.mnuCut,
            this.mnuCopy,
            this.mnuCopySpecial,
            this.mnuPaste,
            this.mnuPasteSpecial,
            this.mnuDelete,
            this.toolStripMenuItem2,
            this.mnuSelectAll,
            this.mnuAutoFormat});
            this.mnuContext.Name = "contextMenuStrip1";
            this.mnuContext.Size = new System.Drawing.Size(143, 236);
            // 
            // mnuUndo
            // 
            this.mnuUndo.Name = "mnuUndo";
            this.mnuUndo.Size = new System.Drawing.Size(142, 22);
            this.mnuUndo.Text = "Undo";
            // 
            // mnuRedo
            // 
            this.mnuRedo.Name = "mnuRedo";
            this.mnuRedo.Size = new System.Drawing.Size(142, 22);
            this.mnuRedo.Text = "Redo";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(139, 6);
            // 
            // mnuCut
            // 
            this.mnuCut.Name = "mnuCut";
            this.mnuCut.Size = new System.Drawing.Size(142, 22);
            this.mnuCut.Text = "Cut";
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(142, 22);
            this.mnuCopy.Text = "Copy";
            // 
            // mnuCopySpecial
            // 
            this.mnuCopySpecial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyCS,
            this.mnuCopyVB,
            this.mnuCopySource});
            this.mnuCopySpecial.Name = "mnuCopySpecial";
            this.mnuCopySpecial.Size = new System.Drawing.Size(142, 22);
            this.mnuCopySpecial.Text = "Copy Special";
            // 
            // mnuCopyCS
            // 
            this.mnuCopyCS.Name = "mnuCopyCS";
            this.mnuCopyCS.Size = new System.Drawing.Size(210, 22);
            this.mnuCopyCS.Text = "Copy C# String";
            // 
            // mnuCopyVB
            // 
            this.mnuCopyVB.Name = "mnuCopyVB";
            this.mnuCopyVB.Size = new System.Drawing.Size(210, 22);
            this.mnuCopyVB.Text = "Copy VB String";
            // 
            // mnuCopySource
            // 
            this.mnuCopySource.Name = "mnuCopySource";
            this.mnuCopySource.Size = new System.Drawing.Size(210, 22);
            this.mnuCopySource.Text = "Copy Without Formatting";
            // 
            // mnuPaste
            // 
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.Size = new System.Drawing.Size(142, 22);
            this.mnuPaste.Text = "Paste";
            // 
            // mnuPasteSpecial
            // 
            this.mnuPasteSpecial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPasteVB,
            this.mnuPasteCS});
            this.mnuPasteSpecial.Name = "mnuPasteSpecial";
            this.mnuPasteSpecial.Size = new System.Drawing.Size(142, 22);
            this.mnuPasteSpecial.Text = "Paste Special";
            // 
            // mnuPasteVB
            // 
            this.mnuPasteVB.Name = "mnuPasteVB";
            this.mnuPasteVB.Size = new System.Drawing.Size(154, 22);
            this.mnuPasteVB.Text = "Paste VB String";
            // 
            // mnuPasteCS
            // 
            this.mnuPasteCS.Name = "mnuPasteCS";
            this.mnuPasteCS.Size = new System.Drawing.Size(154, 22);
            this.mnuPasteCS.Text = "Paste C# String";
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(142, 22);
            this.mnuDelete.Text = "Delete";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(139, 6);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.Size = new System.Drawing.Size(142, 22);
            this.mnuSelectAll.Text = "Select All";
            // 
            // mnuAutoFormat
            // 
            this.mnuAutoFormat.Name = "mnuAutoFormat";
            this.mnuAutoFormat.Size = new System.Drawing.Size(142, 22);
            this.mnuAutoFormat.Text = "Auto Format";
            // 
            // SqlView
            // 
            this.Name = "SqlView";
            this.mnuContext.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		private ToolStripMenuItem mnuUndo;
		private ToolStripMenuItem mnuRedo;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem mnuCut;
		private ToolStripMenuItem mnuCopy;
		private ToolStripMenuItem mnuCopySpecial;
		private ToolStripMenuItem mnuPaste;
		private ToolStripMenuItem mnuPasteSpecial;
		private ToolStripMenuItem mnuDelete;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem mnuCopySource;
		private ToolStripMenuItem mnuCopyVB;
		private ToolStripMenuItem mnuCopyCS;
		private ToolStripMenuItem mnuPasteVB;
		private ToolStripMenuItem mnuPasteCS;
		private ToolStripMenuItem mnuAutoFormat;
		private ToolStripMenuItem mnuSelectAll;

	}
}

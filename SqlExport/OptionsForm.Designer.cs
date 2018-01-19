namespace SqlExport
{
	partial class OptionsForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgFont = new System.Windows.Forms.FontDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tspPages = new System.Windows.Forms.TabControl();
            this.tbpProgram = new System.Windows.Forms.TabPage();
            this.btnSetFileAssociations = new System.Windows.Forms.Button();
            this.lblEditorFont = new System.Windows.Forms.Label();
            this.btnFont = new System.Windows.Forms.Button();
            this.chkWordWrap = new System.Windows.Forms.CheckBox();
            this.chkLineNumbers = new System.Windows.Forms.CheckBox();
            this.txtTabWidth = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.btnOpenConfig = new System.Windows.Forms.Button();
            this.chkSameWindow = new System.Windows.Forms.CheckBox();
            this.lblSameWindow = new System.Windows.Forms.Label();
            this.tbpDatabase = new System.Windows.Forms.TabPage();
            this.fraDetails = new System.Windows.Forms.GroupBox();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.txtCS = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCS = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.lstDB = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCnnString = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbpExecution = new System.Windows.Forms.TabPage();
            this.chkTransactional = new System.Windows.Forms.CheckBox();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.chkEditable = new System.Windows.Forms.CheckBox();
            this.tbpExport = new System.Windows.Forms.TabPage();
            this.Label4 = new System.Windows.Forms.Label();
            this.ddlExportType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tspPages.SuspendLayout();
            this.tbpProgram.SuspendLayout();
            this.tbpDatabase.SuspendLayout();
            this.fraDetails.SuspendLayout();
            this.tbpExecution.SuspendLayout();
            this.tbpExport.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgOpen
            // 
            this.dlgOpen.CheckFileExists = false;
            this.dlgOpen.DefaultExt = "xml";
            this.dlgOpen.Filter = "XML Files (*.XML)|*.xml";
            // 
            // dlgFont
            // 
            this.dlgFont.Color = System.Drawing.SystemColors.ControlText;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tspPages, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(836, 448);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // tspPages
            // 
            this.tspPages.Controls.Add(this.tbpProgram);
            this.tspPages.Controls.Add(this.tbpDatabase);
            this.tspPages.Controls.Add(this.tbpExecution);
            this.tspPages.Controls.Add(this.tbpExport);
            this.tspPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tspPages.Location = new System.Drawing.Point(3, 3);
            this.tspPages.Name = "tspPages";
            this.tspPages.SelectedIndex = 0;
            this.tspPages.Size = new System.Drawing.Size(830, 406);
            this.tspPages.TabIndex = 26;
            // 
            // tbpProgram
            // 
            this.tbpProgram.Controls.Add(this.btnSetFileAssociations);
            this.tbpProgram.Controls.Add(this.lblEditorFont);
            this.tbpProgram.Controls.Add(this.btnFont);
            this.tbpProgram.Controls.Add(this.chkWordWrap);
            this.tbpProgram.Controls.Add(this.chkLineNumbers);
            this.tbpProgram.Controls.Add(this.txtTabWidth);
            this.tbpProgram.Controls.Add(this.Label6);
            this.tbpProgram.Controls.Add(this.btnOpenConfig);
            this.tbpProgram.Controls.Add(this.chkSameWindow);
            this.tbpProgram.Controls.Add(this.lblSameWindow);
            this.tbpProgram.Location = new System.Drawing.Point(4, 23);
            this.tbpProgram.Name = "tbpProgram";
            this.tbpProgram.Size = new System.Drawing.Size(822, 379);
            this.tbpProgram.TabIndex = 0;
            this.tbpProgram.Text = "Program";
            // 
            // btnSetFileAssociations
            // 
            this.btnSetFileAssociations.AutoSize = true;
            this.btnSetFileAssociations.Enabled = false;
            this.btnSetFileAssociations.Location = new System.Drawing.Point(146, 234);
            this.btnSetFileAssociations.Name = "btnSetFileAssociations";
            this.btnSetFileAssociations.Size = new System.Drawing.Size(132, 24);
            this.btnSetFileAssociations.TabIndex = 23;
            this.btnSetFileAssociations.Text = "Set File Associations";
            // 
            // lblEditorFont
            // 
            this.lblEditorFont.Location = new System.Drawing.Point(92, 136);
            this.lblEditorFont.Name = "lblEditorFont";
            this.lblEditorFont.Size = new System.Drawing.Size(366, 14);
            this.lblEditorFont.TabIndex = 22;
            this.lblEditorFont.Text = "Font";
            // 
            // btnFont
            // 
            this.btnFont.AutoSize = true;
            this.btnFont.Location = new System.Drawing.Point(11, 132);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(75, 24);
            this.btnFont.TabIndex = 21;
            this.btnFont.Text = "Font";
            // 
            // chkWordWrap
            // 
            this.chkWordWrap.BackColor = System.Drawing.SystemColors.Control;
            this.chkWordWrap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkWordWrap.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWordWrap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkWordWrap.Location = new System.Drawing.Point(11, 86);
            this.chkWordWrap.Name = "chkWordWrap";
            this.chkWordWrap.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkWordWrap.Size = new System.Drawing.Size(145, 17);
            this.chkWordWrap.TabIndex = 20;
            this.chkWordWrap.Text = "Enable word wrap";
            this.chkWordWrap.UseVisualStyleBackColor = false;
            // 
            // chkLineNumbers
            // 
            this.chkLineNumbers.BackColor = System.Drawing.SystemColors.Control;
            this.chkLineNumbers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLineNumbers.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkLineNumbers.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLineNumbers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkLineNumbers.Location = new System.Drawing.Point(11, 109);
            this.chkLineNumbers.Name = "chkLineNumbers";
            this.chkLineNumbers.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkLineNumbers.Size = new System.Drawing.Size(145, 17);
            this.chkLineNumbers.TabIndex = 19;
            this.chkLineNumbers.Text = "Show line numbers";
            this.chkLineNumbers.UseVisualStyleBackColor = false;
            // 
            // txtTabWidth
            // 
            this.txtTabWidth.Location = new System.Drawing.Point(76, 60);
            this.txtTabWidth.Name = "txtTabWidth";
            this.txtTabWidth.Size = new System.Drawing.Size(80, 20);
            this.txtTabWidth.TabIndex = 18;
            this.txtTabWidth.Text = "4";
            this.txtTabWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(8, 63);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(54, 14);
            this.Label6.TabIndex = 17;
            this.Label6.Text = "Tab width";
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.AutoSize = true;
            this.btnOpenConfig.Location = new System.Drawing.Point(8, 234);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(132, 24);
            this.btnOpenConfig.TabIndex = 16;
            this.btnOpenConfig.Text = "Open Current Config";
            // 
            // chkSameWindow
            // 
            this.chkSameWindow.BackColor = System.Drawing.SystemColors.Control;
            this.chkSameWindow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSameWindow.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkSameWindow.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSameWindow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSameWindow.Location = new System.Drawing.Point(11, 3);
            this.chkSameWindow.Name = "chkSameWindow";
            this.chkSameWindow.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkSameWindow.Size = new System.Drawing.Size(145, 17);
            this.chkSameWindow.TabIndex = 9;
            this.chkSameWindow.Text = "Open in Same Window";
            this.chkSameWindow.UseVisualStyleBackColor = false;
            // 
            // lblSameWindow
            // 
            this.lblSameWindow.BackColor = System.Drawing.SystemColors.Control;
            this.lblSameWindow.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSameWindow.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSameWindow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSameWindow.Location = new System.Drawing.Point(8, 23);
            this.lblSameWindow.Name = "lblSameWindow";
            this.lblSameWindow.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblSameWindow.Size = new System.Drawing.Size(328, 41);
            this.lblSameWindow.TabIndex = 10;
            this.lblSameWindow.Text = "For file associations use \'%1\' for DDE when the application is open and \'%1 -new\'" +
    " when the application needs to be opened.";
            // 
            // tbpDatabase
            // 
            this.tbpDatabase.Controls.Add(this.fraDetails);
            this.tbpDatabase.Controls.Add(this.Label1);
            this.tbpDatabase.Controls.Add(this.lstDB);
            this.tbpDatabase.Location = new System.Drawing.Point(4, 22);
            this.tbpDatabase.Name = "tbpDatabase";
            this.tbpDatabase.Size = new System.Drawing.Size(822, 380);
            this.tbpDatabase.TabIndex = 2;
            this.tbpDatabase.Text = "Database";
            // 
            // fraDetails
            // 
            this.fraDetails.BackColor = System.Drawing.SystemColors.Control;
            this.fraDetails.Controls.Add(this.btnMoveDown);
            this.fraDetails.Controls.Add(this.btnMoveUp);
            this.fraDetails.Controls.Add(this.cboType);
            this.fraDetails.Controls.Add(this.btnSave);
            this.fraDetails.Controls.Add(this.btnDelete);
            this.fraDetails.Controls.Add(this.btnNew);
            this.fraDetails.Controls.Add(this.txtCS);
            this.fraDetails.Controls.Add(this.txtName);
            this.fraDetails.Controls.Add(this.lblName);
            this.fraDetails.Controls.Add(this.lblCS);
            this.fraDetails.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraDetails.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fraDetails.Location = new System.Drawing.Point(8, 136);
            this.fraDetails.Name = "fraDetails";
            this.fraDetails.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fraDetails.Size = new System.Drawing.Size(456, 120);
            this.fraDetails.TabIndex = 33;
            this.fraDetails.TabStop = false;
            this.fraDetails.Text = "Connection Details";
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.BackColor = System.Drawing.SystemColors.Control;
            this.btnMoveDown.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnMoveDown.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDown.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMoveDown.Location = new System.Drawing.Point(391, 13);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnMoveDown.Size = new System.Drawing.Size(57, 21);
            this.btnMoveDown.TabIndex = 37;
            this.btnMoveDown.Text = "Down";
            this.btnMoveDown.UseVisualStyleBackColor = false;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.BackColor = System.Drawing.SystemColors.Control;
            this.btnMoveUp.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnMoveUp.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMoveUp.Location = new System.Drawing.Point(328, 13);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnMoveUp.Size = new System.Drawing.Size(57, 21);
            this.btnMoveUp.TabIndex = 36;
            this.btnMoveUp.Text = "Up";
            this.btnMoveUp.UseVisualStyleBackColor = false;
            // 
            // cboType
            // 
            this.cboType.BackColor = System.Drawing.SystemColors.Window;
            this.cboType.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboType.ItemHeight = 14;
            this.cboType.Location = new System.Drawing.Point(8, 64);
            this.cboType.Name = "cboType";
            this.cboType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cboType.Size = new System.Drawing.Size(61, 22);
            this.cboType.TabIndex = 35;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSave.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Location = new System.Drawing.Point(134, 94);
            this.btnSave.Name = "btnSave";
            this.btnSave.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSave.Size = new System.Drawing.Size(57, 21);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDelete.Location = new System.Drawing.Point(71, 94);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnDelete.Size = new System.Drawing.Size(57, 21);
            this.btnDelete.TabIndex = 33;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.SystemColors.Control;
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnNew.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNew.Location = new System.Drawing.Point(8, 94);
            this.btnNew.Name = "btnNew";
            this.btnNew.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNew.Size = new System.Drawing.Size(57, 21);
            this.btnNew.TabIndex = 32;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = false;
            // 
            // txtCS
            // 
            this.txtCS.AcceptsReturn = true;
            this.txtCS.BackColor = System.Drawing.SystemColors.Window;
            this.txtCS.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCS.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCS.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCS.Location = new System.Drawing.Point(8, 40);
            this.txtCS.MaxLength = 0;
            this.txtCS.Name = "txtCS";
            this.txtCS.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCS.Size = new System.Drawing.Size(440, 20);
            this.txtCS.TabIndex = 31;
            // 
            // txtName
            // 
            this.txtName.AcceptsReturn = true;
            this.txtName.BackColor = System.Drawing.SystemColors.Window;
            this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtName.Location = new System.Drawing.Point(8, 16);
            this.txtName.MaxLength = 0;
            this.txtName.Name = "txtName";
            this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtName.Size = new System.Drawing.Size(145, 20);
            this.txtName.TabIndex = 30;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.SystemColors.Control;
            this.lblName.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblName.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblName.Location = new System.Drawing.Point(8, 20);
            this.lblName.Name = "lblName";
            this.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblName.Size = new System.Drawing.Size(0, 14);
            this.lblName.TabIndex = 29;
            // 
            // lblCS
            // 
            this.lblCS.AutoSize = true;
            this.lblCS.BackColor = System.Drawing.SystemColors.Control;
            this.lblCS.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblCS.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCS.Location = new System.Drawing.Point(8, 64);
            this.lblCS.Name = "lblCS";
            this.lblCS.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCS.Size = new System.Drawing.Size(0, 14);
            this.lblCS.TabIndex = 28;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(8, 7);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(59, 14);
            this.Label1.TabIndex = 31;
            this.Label1.Text = "Databases";
            // 
            // lstDB
            // 
            this.lstDB.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chCnnString,
            this.chType});
            this.lstDB.FullRowSelect = true;
            this.lstDB.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstDB.HideSelection = false;
            this.lstDB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lstDB.Location = new System.Drawing.Point(8, 24);
            this.lstDB.MultiSelect = false;
            this.lstDB.Name = "lstDB";
            this.lstDB.Size = new System.Drawing.Size(456, 104);
            this.lstDB.TabIndex = 0;
            this.lstDB.UseCompatibleStateImageBehavior = false;
            this.lstDB.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 80;
            // 
            // chCnnString
            // 
            this.chCnnString.Text = "Connection String";
            this.chCnnString.Width = 310;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            // 
            // tbpExecution
            // 
            this.tbpExecution.Controls.Add(this.chkTransactional);
            this.tbpExecution.Controls.Add(this.txtTimeout);
            this.tbpExecution.Controls.Add(this.Label3);
            this.tbpExecution.Controls.Add(this.chkEditable);
            this.tbpExecution.Location = new System.Drawing.Point(4, 22);
            this.tbpExecution.Name = "tbpExecution";
            this.tbpExecution.Size = new System.Drawing.Size(822, 380);
            this.tbpExecution.TabIndex = 1;
            this.tbpExecution.Text = "Execution";
            // 
            // chkTransactional
            // 
            this.chkTransactional.Location = new System.Drawing.Point(8, 3);
            this.chkTransactional.Name = "chkTransactional";
            this.chkTransactional.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkTransactional.Size = new System.Drawing.Size(128, 16);
            this.chkTransactional.TabIndex = 9;
            this.chkTransactional.Text = "Run in a transaction";
            this.chkTransactional.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(112, 47);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(104, 20);
            this.txtTimeout.TabIndex = 8;
            this.txtTimeout.Text = "30";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(8, 50);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(77, 14);
            this.Label3.TabIndex = 7;
            this.Label3.Text = "Query Timeout";
            // 
            // chkEditable
            // 
            this.chkEditable.Location = new System.Drawing.Point(8, 25);
            this.chkEditable.Name = "chkEditable";
            this.chkEditable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkEditable.Size = new System.Drawing.Size(128, 16);
            this.chkEditable.TabIndex = 2;
            this.chkEditable.Text = "Dataset is Editable";
            this.chkEditable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbpExport
            // 
            this.tbpExport.Controls.Add(this.Label4);
            this.tbpExport.Controls.Add(this.ddlExportType);
            this.tbpExport.Location = new System.Drawing.Point(4, 22);
            this.tbpExport.Name = "tbpExport";
            this.tbpExport.Size = new System.Drawing.Size(822, 380);
            this.tbpExport.TabIndex = 3;
            this.tbpExport.Text = "Export";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(8, 32);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(76, 14);
            this.Label4.TabIndex = 16;
            this.Label4.Text = "Export method";
            // 
            // ddlExportType
            // 
            this.ddlExportType.BackColor = System.Drawing.SystemColors.Window;
            this.ddlExportType.Cursor = System.Windows.Forms.Cursors.Default;
            this.ddlExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlExportType.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlExportType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ddlExportType.ItemHeight = 14;
            this.ddlExportType.Location = new System.Drawing.Point(111, 29);
            this.ddlExportType.Name = "ddlExportType";
            this.ddlExportType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddlExportType.Size = new System.Drawing.Size(151, 22);
            this.ddlExportType.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdOk);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 415);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 30);
            this.panel1.TabIndex = 0;
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOk.AutoSize = true;
            this.cmdOk.BackColor = System.Drawing.SystemColors.Control;
            this.cmdOk.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdOk.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdOk.Location = new System.Drawing.Point(683, 3);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdOk.Size = new System.Drawing.Size(69, 24);
            this.cmdOk.TabIndex = 4;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = false;
            this.cmdOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.AutoSize = true;
            this.cmdCancel.BackColor = System.Drawing.SystemColors.Control;
            this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdCancel.Location = new System.Drawing.Point(758, 3);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdCancel.Size = new System.Drawing.Size(69, 24);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // OptionsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(836, 448);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(3, 23);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "SQL Export - Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tspPages.ResumeLayout(false);
            this.tbpProgram.ResumeLayout(false);
            this.tbpProgram.PerformLayout();
            this.tbpDatabase.ResumeLayout(false);
            this.tbpDatabase.PerformLayout();
            this.fraDetails.ResumeLayout(false);
            this.fraDetails.PerformLayout();
            this.tbpExecution.ResumeLayout(false);
            this.tbpExecution.PerformLayout();
            this.tbpExport.ResumeLayout(false);
            this.tbpExport.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.FontDialog dlgFont;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tspPages;
        private System.Windows.Forms.TabPage tbpProgram;
        private System.Windows.Forms.Button btnSetFileAssociations;
        private System.Windows.Forms.Label lblEditorFont;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.CheckBox chkWordWrap;
        private System.Windows.Forms.CheckBox chkLineNumbers;
        private System.Windows.Forms.TextBox txtTabWidth;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.CheckBox chkSameWindow;
        private System.Windows.Forms.Label lblSameWindow;
        private System.Windows.Forms.TabPage tbpDatabase;
        private System.Windows.Forms.GroupBox fraDetails;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.TextBox txtCS;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCS;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListView lstDB;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chCnnString;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.TabPage tbpExecution;
        private System.Windows.Forms.CheckBox chkTransactional;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.CheckBox chkEditable;
        private System.Windows.Forms.TabPage tbpExport;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.ComboBox ddlExportType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
	}
}

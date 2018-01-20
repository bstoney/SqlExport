namespace SqlExport
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Business;
    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Data;
    using SqlExport.Editor;
    using SqlExport.Export;
    using SqlExport.Messages;

    partial class OptionsForm : Form
    {
        private IEditorStyleConfiguration editorStyle;

        public OptionsForm()
        {
            //This call is required by the Windows Form Designer.
            InitializeComponent();

            btnNew.Click += new EventHandler(OnNewClick);
            btnDelete.Click += new EventHandler(OnDeleteClick);
            btnSave.Click += new EventHandler(OnSaveClick);
            btnOpenConfig.Click += new EventHandler(OnOpenConfigClick);
            btnSetFileAssociations.Click += new EventHandler(OnSetFileAssociationsClick);
            btnFont.Click += new EventHandler(OnFontClick);
            btnMoveUp.Click += new EventHandler(OnMoveUpClick);
            btnMoveDown.Click += new EventHandler(OnMoveDownClick);
        }

        private void SaveOptions()
        {
            var options = Configuration.Current;
            options.SingleInstance = chkSameWindow.Checked;
            options.Editable = chkEditable.Checked;
            options.Transactional = chkTransactional.Checked;
            options.CommandTimeout = Convert.ToInt32(txtTimeout.Text);
            options.ExportType = ddlExportType.Text;

            Configuration.SaveOptionsFrom(options);

            var databases = (from i in this.lstDB.Items.OfType<ListViewItem>()
                             select new DatabaseDetails
                                 {
                                     Name = i.Text,
                                     ConnectionString = i.SubItems[1].Text,
                                     Type = i.SubItems[2].Text
                                 }).ToList();

            Configuration.SetDatabases(databases);

            this.editorStyle.TabSize = int.Parse(this.txtTabWidth.Text);
            this.editorStyle.WordWrap = this.chkWordWrap.Checked;
            this.editorStyle.LineNumbers = this.chkLineNumbers.Checked;

            Configuration.SaveOptionsFrom(this.editorStyle);

            try
            {
                Configuration.Save();
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }
        }

        private void LoadOptions()
        {
            chkSameWindow.Checked = Configuration.Current.SingleInstance;
            chkEditable.Checked = Configuration.Current.Editable;
            chkTransactional.Checked = Configuration.Current.Transactional;
            txtTimeout.Text = Configuration.Current.CommandTimeout.ToString();

            ddlExportType.Items.Clear();
            ddlExportType.Items.AddRange(ExportAdapterHelper.RegisteredExportAdapters);
            ddlExportType.Text = Configuration.Current.ExportType;

            foreach (var db in Configuration.GetDatabases())
            {
                lstDB.Items.Add(new ListViewItem(new[] { db.Name, db.ConnectionString, db.Type }));
            }

            this.editorStyle = new EditorStyle();

            this.txtTabWidth.Text = this.editorStyle.TabSize.ToString();
            this.chkWordWrap.Checked = this.editorStyle.WordWrap;
            this.chkLineNumbers.Checked = this.editorStyle.LineNumbers;

            lblEditorFont.Text = GetFontName();

            cboType.Items.AddRange(ConnectionAdapterHelper.RegisteredConnectionAdapters);
        }

        private string GetFontName()
        {
            return string.Format("{0}, {1}pt", this.editorStyle.Font.Name, this.editorStyle.Font.SizeInPoints);
        }

        #region Event Handlers

        private void OnOkClick(System.Object eventSender, System.EventArgs eventArgs)
        {
            SaveOptions();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancelClick(System.Object eventSender, System.EventArgs eventArgs)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnDeleteClick(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (lstDB.SelectedItems.Count > 0)
            {
                lstDB.Items.RemoveAt(lstDB.SelectedItems[0].Index);
                lstDB.Items[0].Selected = true;
            }
        }

        private void OnNewClick(System.Object eventSender, System.EventArgs eventArgs)
        {
            lstDB.Items.Add("New DB").SubItems.AddRange(new string[] { "Connection String", "MsSql" });
        }

        private void OnSaveClick(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (lstDB.SelectedItems.Count == 1)
            {
                lstDB.SelectedItems[0].Text = txtName.Text;
                lstDB.SelectedItems[0].SubItems[1].Text = txtCS.Text;
                lstDB.SelectedItems[0].SubItems[2].Text = cboType.Text;
            }
        }

        void OnMoveDownClick(object sender, EventArgs e)
        {
            if (lstDB.SelectedItems.Count == 1 && lstDB.SelectedItems[0].Index < lstDB.Items.Count - 1)
            {
                ListViewItem item = lstDB.SelectedItems[0];
                int index = item.Index;
                lstDB.Items.RemoveAt(index);
                lstDB.Items.Insert(index + 1, item);
            }
        }

        void OnMoveUpClick(object sender, EventArgs e)
        {
            if (lstDB.SelectedItems.Count == 1 && lstDB.SelectedItems[0].Index > 0)
            {
                ListViewItem item = lstDB.SelectedItems[0];
                int index = item.Index;
                lstDB.Items.RemoveAt(index);
                lstDB.Items.Insert(index - 1, item);
            }
        }

        private void frmOptions_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            LoadOptions();
        }

        private void lstDB_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (lstDB.SelectedItems.Count == 1)
            {
                txtName.Text = lstDB.SelectedItems[0].Text;
                txtCS.Text = lstDB.SelectedItems[0].SubItems[1].Text;
                cboType.Text = lstDB.SelectedItems[0].SubItems[2].Text;
            }
        }

        private void OnFontClick(object sender, EventArgs e)
        {
            dlgFont.Font = this.editorStyle.Font;
            if (dlgFont.ShowDialog() == DialogResult.OK)
            {
                this.editorStyle.Font = dlgFont.Font;
                lblEditorFont.Text = GetFontName();
            }
        }

        private void OnOpenConfigClick(Object sender, EventArgs e)
        {
            if (!File.Exists(Configuration.SettingsPath))
            {
                Configuration.Save();
            }

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Configuration.SettingsPath);
            p.Start();

            // Cancel the options for after opening the config to avoid overriding manual changes.
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnSetFileAssociationsClick(object sender, EventArgs e)
        {
            // TODO
            ////Debugger.Break();

            ////FileAssociationInfo fai = new FileAssociationInfo( ".sql" );

            ////if( !fai.Exists )
            ////{
            ////    fai.Create( "SQLExport.SQL" );

            ////    // Specify MIME type (optional)
            ////    fai.ContentType = "text/sql";

            ////    // Programs automatically displayed in open with list
            ////    fai.OpenWithList = new string[] { "sqlexport.exe", "ssms.exe", "notepad.exe" };
            ////}

            ////ProgramAssociationInfo pai = new ProgramAssociationInfo( fai.ProgID );

            ////if( !pai.Exists )
            ////{
            ////    pai.Create();
            ////    pai.Description = "Microsoft SQL Server Query File";
            ////}

            ////pai.RemoveVerb( "Open" );
            ////pai.AddVerb( new ProgramVerb(
            ////    // Verb name
            ////        "Open",
            ////    // Path and arguments to use
            ////         @"rundll32.exe dfshim.dll, ShOpenVerbExtension {" + System.Reflection.Assembly.GetEntryAssembly().ManifestModule.ModuleVersionId + "} %1" ) );

            ////pai.EditFlags &= EditFlags.OpenIsSafe;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SqlExport.Data;
using SqlExport.Editor;
using System.IO;
using System.Diagnostics;
using System.Linq;
using SqlExport.ViewModel;
using SqlExport.Messages;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.View;
using SqlExport.Business;
using SqlExport.Messages.StatusPanel;
using SqlExport.Messages;

namespace SqlExport.Ui
{
    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Messages;
    using SqlExport.ViewModel;

    public partial class QueryPane : UserControl
    {
        private DatabaseConnectionContext database;
        private IEditorStyleConfiguration currentStyle;
        private int _messageCount;
        private Controller controller;

        public QueryPane()
        {
            InitializeComponent();

            // TODO add this to the temp files and define a global default.
            lstObjects.Width = 121;

            txtView.Next += new EventHandler(OnNext);
            txtView.Previous += new EventHandler(OnPrevious);

            this.currentStyle = DefaultOptions.GetEditorStyle();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPane"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public QueryPane(QueryViewModel dataContext)
            : this()
        {
            this.DataContext = dataContext;

            Messenger.Default.Register<OptionsChangedMessage>(
                this,
                m =>
                {
                    this.currentStyle = DefaultOptions.GetEditorStyle();
                    SetEditorStyle();
                });

            Messenger.Default.Register<SetEditorStyleMessage>(
                this,
                this.DataContext,
                m =>
                {
                    if (this.currentStyle != null)
                    {
                        this.SetEditorStyle();
                    }
                });

            Messenger.Default.Register<ErrorSelectedMessage>(
                this,
                this.DataContext.ResultsPanelDataContext.MessageListDataContext.ScopeToken,
                m => txtView.SelectLine(m.LineNumber));
            Messenger.Default.Register<ClearConnectionsMessage>(
                this, this.DataContext.UniqueID, m => this.lstObjects.ClearConnections());
            Messenger.Default.Register<AddConnectionMessage>(
                this, this.DataContext.UniqueID, m => this.lstObjects.AddConnection(m.DatabaseDetails));
            Messenger.Default.Register<GetQueryTextMessage>(
                this, this.DataContext.UniqueID, m => m.Text = m.AllText ? this.txtView.Text : this.txtView.SQLStatement);
            Messenger.Default.Register<SetQueryTextMessage>(
                this,
                this.DataContext.UniqueID,
                m =>
                {
                    if (m.AllText)
                    {
                        this.txtView.Text = m.Text;
                    }
                });
        }

        /// <summary>
        /// Gets the query details.
        /// </summary>
        public QueryDetails QueryDetails
        {
            get { return this.DataContext.QueryDetails; }
        }

        public void SaveAs()
        {
            this.DataContext.SaveAs();
        }

        public void Save()
        {
            this.DataContext.Save();
        }

        public new void Load()
        {
            this.DataContext.Load();
        }

        public void ExportResults()
        {
            this.DataContext.ResultsPanelDataContext.ExportResults();
        }

        public void Connect()
        {
            if (this.database != null)
            {
                this.database.Connect();
            }
            else
            {
                this.DataContext.ResultsPanelDataContext.MessageListDataContext.AddMessage(
                    "No database has been selected.", DisplayMessageType.Warning);
                Messenger.Default.Send(new ShowMessagesMessage(), this.DataContext.ResultsPanelDataContext);
            }
        }

        public void Disconnect()
        {
            if (this.database != null)
            {
                if (this.QueryDetails.HasTransaction)
                {
                    if (MessageBox.Show("Rollback transaction and close current connection?",
                        "Disconnect", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                this.database.Disconnect();
            }
        }

        public void BeginTransaction()
        {
            if (this.database != null)
            {
                this.database.BeginTransaction();
            }
            else
            {
                this.DataContext.ResultsPanelDataContext.MessageListDataContext.AddMessage("No database has been selected.", DisplayMessageType.Warning);
                Messenger.Default.Send(new ShowMessagesMessage(), this.DataContext.ResultsPanelDataContext);
            }
        }

        public void CommitTransaction()
        {
            if (this.database != null)
            {
                this.database.CommitTransaction();
            }
        }

        public void RollbackTransaction()
        {
            if (this.database != null)
            {
                this.database.RollbackTransaction();
            }
        }

        public void RunQuery()
        {
            this.DataContext.RunQuery();
        }

        public void StopQuery()
        {
            this.DataContext.StopQuery();
        }

        public void FindText(string match, bool ignoreCase)
        {
            txtView.FindText(match, ignoreCase);
        }

        public void ReplaceText(string match, string replace, bool ignoreCase)
        {
            txtView.ReplaceText(match, replace, ignoreCase);
        }

        public void ChooseDatabase()
        {
            // TODO pnlStatus.ManageConnections();
            throw new NotImplementedException();
        }

        private void SetEditorStyle()
        {
            if (this.database != null)
            {
                try
                {
                    ISyntaxDefinition sd = this.database.GetSyntaxDefinition();
                    this.currentStyle.KeywordStyle.Words = sd.Keywords;
                    this.currentStyle.KeywordStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.FunctionStyle.Words = sd.Functions;
                    this.currentStyle.FunctionStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.DataTypeStyle.Words = sd.DataTypes;
                    this.currentStyle.DataTypeStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.OperatorStyle.Words = sd.Operators;
                    this.currentStyle.OperatorStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.CommentStyle.CommentSyntax = sd.CommentSyntax;
                }
                catch (Exception exp)
                {
                    this.DataContext.ResultsPanelDataContext.MessageListDataContext.AddError(exp);
                }
            }

            txtView.SetEditorStyle(this.currentStyle);
        }

        #region Event Handlers

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!this.DataContext.HasChanged)
            {
                this.DataContext.HasChanged = true;
            }
        }

        void OnPrevious(object sender, EventArgs e)
        {
            // TODO Move next previous Tab
        }

        void OnNext(object sender, EventArgs e)
        {
            // TODO Move next Tab
        }

        void OnSave(object sender, EventArgs e)
        {
            this.Save();
        }

        void OnRun(object sender, EventArgs e)
        {
            this.RunQuery();
        }

        private void OnCaretChanged(object sender, int startLine, int endLine)
        {
            pnlStatus.SendMessage(new SetLinesMessage(new SelectedTextRange(startLine, 0, endLine, 0)));

            // don't put this back in!
            //Application.DoEvents()
        }

        #endregion

        public QueryViewModel DataContext { get; set; }
    }
}

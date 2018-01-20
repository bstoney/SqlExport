using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SqlExport.Data;
using SqlExport.Editor;
using System.Drawing;
using SqlExport.Properties;
using SqlExport.Ui;
using System.ComponentModel;

namespace SqlExport.Ui
{
    public abstract partial class QueryAreaBase : UserControl, IContextMenuProvider
    {
        public event EventHandler Run;
        public event EventHandler Stop;
        public event EventHandler StatusChanged;

        public QueryAreaBase()
        {
            InitializeComponent();

            mnuActions.Opening += new CancelEventHandler(OnActionsOpening);
            mnuRun.Click += new EventHandler(OnRunClick);
            mnuStop.Click += new EventHandler(OnStopClick);
            mnuSetDatabase.Click += new EventHandler(OnSetDatabaseClick);

            mnuConnect.Click += new EventHandler(OnConnectClick);
            mnuDisconnect.Click += new EventHandler(OnDisconnectClick);
            mnuBeginTransaction.Click += new EventHandler(OnBeginTransactionClick);
            mnuCommitTransaction.Click += new EventHandler(OnCommitTransactionClick);
            mnuRollbackTransaction.Click += new EventHandler(OnRollbackTransactionClick);

            ContextMenuStrip = GetContextMenu();
        }

        public abstract bool IsRunning { get; }

        public abstract bool IsExecuting { get; }

        public abstract bool HasConnection { get; }

        public abstract bool HasTransaction { get; }

        public abstract string DisplayText { get; }

        public abstract void RunQuery();

        public abstract void StopQuery();

        public abstract void ChooseDatabase();

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void BeginTransaction();

        public abstract void CommitTransaction();

        public abstract void RollbackTransaction();

        protected virtual void OnRun()
        {
            if (Run != null)
            {
                Run(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStop()
        {
            if (Stop != null)
            {
                Stop(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStatusChanged()
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, EventArgs.Empty);
            }
        }

        #region Event Handlers

        private void OnActionsOpening(object sender, CancelEventArgs e)
        {
            if (IsRunning || IsExecuting)
            {
                mnuRun.Enabled = false;
                mnuStop.Enabled = true;
                mnuConnect.Enabled = false;
                mnuDisconnect.Enabled = false;
                mnuBeginTransaction.Enabled = false;
                mnuCommitTransaction.Enabled = false;
                mnuRollbackTransaction.Enabled = false;
                mnuSetDatabase.Enabled = false;
            }
            else
            {
                mnuRun.Enabled = true;
                mnuStop.Enabled = false;
                if (HasTransaction)
                {
                    mnuConnect.Enabled = false;
                    mnuDisconnect.Enabled = true;
                    mnuBeginTransaction.Enabled = false;
                    mnuCommitTransaction.Enabled = true;
                    mnuRollbackTransaction.Enabled = true;
                }
                else
                {
                    mnuBeginTransaction.Enabled = true;
                    mnuCommitTransaction.Enabled = false;
                    mnuRollbackTransaction.Enabled = false;
                    if (HasConnection)
                    {
                        mnuConnect.Enabled = false;
                        mnuDisconnect.Enabled = true;
                    }
                    else
                    {
                        mnuConnect.Enabled = true;
                        mnuDisconnect.Enabled = false;
                    }
                }
                mnuSetDatabase.Enabled = true;
            }

            ContextMenuHelper.AddHierachicalContextMenu(this, GetContextMenu());
        }

        private void OnSetDatabaseClick(object sender, EventArgs e)
        {
            ChooseDatabase();
        }

        private void OnRunClick(object sender, EventArgs e)
        {
            RunQuery();
        }

        private void OnStopClick(object sender, EventArgs e)
        {
            StopQuery();
        }

        private void OnConnectClick(object sender, EventArgs e)
        {
            Connect();
        }

        private void OnDisconnectClick(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void OnBeginTransactionClick(object sender, EventArgs e)
        {
            BeginTransaction();
        }

        private void OnCommitTransactionClick(object sender, EventArgs e)
        {
            CommitTransaction();
        }

        private void OnRollbackTransactionClick(object sender, EventArgs e)
        {
            RollbackTransaction();
        }

        #endregion

        #region IContextMenuProvider Members

        public string MenuTitle
        {
            get { return "Query Actions"; }
        }

        public bool HasContextMenu
        {
            get { return true; }
        }

        public ContextMenuStrip GetContextMenu()
        {
            return mnuActions;
        }

        #endregion
    }
}

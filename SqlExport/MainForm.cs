using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using SqlExport.Export;
using SqlExport.Data;
using SqlExport.Properties;
using SqlExport.Editor;
using System.IO;
using System.Collections.Generic;
using SqlExport.Collections;
using System.Text;
using System.ComponentModel;
using SqlExport.Ui;
using System.Collections.Specialized;
using System.Configuration;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Messages;

namespace SqlExport
{
    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Messages;

    public partial class MainForm : UserControl
    {
        string[] recentFiles = new string[Options.Current.RecentFileCount];

        public string CurrentTitle;

        private SqlExport.ViewModel.MainWindowViewModel ctlQueryArea;

        public MainForm()
        {
            InitializeComponent();

            mnuActions.DropDownOpening += new EventHandler(OnActionsOpening);

            mnuConnect.Click += new EventHandler(OnConnectClick);
            mnuDisconnect.Click += new EventHandler(OnDisconnectClick);
            mnuBeginTransaction.Click += new EventHandler(OnBeginTransactionClick);
            mnuCommitTransaction.Click += new EventHandler(OnCommitTransactionClick);
            mnuRollbackTransaction.Click += new EventHandler(OnRollbackTransactionClick);
        }

        private void OnConnectClick(object sender, EventArgs e)
        {
            ctlQueryArea.Connect();
        }

        private void OnDisconnectClick(object sender, EventArgs e)
        {
            ctlQueryArea.Disconnect();
        }

        private void OnBeginTransactionClick(object sender, EventArgs e)
        {
            ctlQueryArea.BeginTransaction();
        }

        private void OnCommitTransactionClick(object sender, EventArgs e)
        {
            ctlQueryArea.CommitTransaction();
        }

        private void OnRollbackTransactionClick(object sender, EventArgs e)
        {
            ctlQueryArea.RollbackTransaction();
        }

        private void OnActionsOpening(object sender, EventArgs e)
        {
            // TODO
            ////if (ctlQueryArea.SelectedQuery == null || ctlQueryArea.SelectedQuery.QueryDetails.IsRunning ||
            ////    ctlQueryArea.SelectedQuery.QueryDetails.IsExecuting)
            ////{
            ////    mnuRun.Enabled = false;
            ////    mnuStop.Enabled = ctlQueryArea.SelectedQuery != null;
            ////    mnuConnect.Enabled = false;
            ////    mnuDisconnect.Enabled = false;
            ////    mnuBeginTransaction.Enabled = false;
            ////    mnuCommitTransaction.Enabled = false;
            ////    mnuRollbackTransaction.Enabled = false;
            ////    mnuExportResults.Enabled = false;
            ////}
            ////else
            ////{
            ////    mnuRun.Enabled = true;
            ////    mnuStop.Enabled = false;
            ////    if (ctlQueryArea.SelectedQuery.QueryDetails.HasTransaction)
            ////    {
            ////        mnuConnect.Enabled = false;
            ////        mnuDisconnect.Enabled = true;
            ////        mnuBeginTransaction.Enabled = false;
            ////        mnuCommitTransaction.Enabled = true;
            ////        mnuRollbackTransaction.Enabled = true;
            ////    }
            ////    else
            ////    {
            ////        mnuBeginTransaction.Enabled = true;
            ////        mnuCommitTransaction.Enabled = false;
            ////        mnuRollbackTransaction.Enabled = false;
            ////        if (ctlQueryArea.SelectedQuery.QueryDetails.HasConnection)
            ////        {
            ////            mnuConnect.Enabled = false;
            ////            mnuDisconnect.Enabled = true;
            ////        }
            ////        else
            ////        {
            ////            mnuConnect.Enabled = true;
            ////            mnuDisconnect.Enabled = false;
            ////        }
            ////    }
            ////    mnuExportResults.Enabled = ctlQueryArea.SelectedQuery.QueryDetails.CanExport;
            ////}
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SqlExport.Ui.ViewModel;
using SqlExport.Ui.View;
using SqlExport.Ui.Messages;

namespace SqlExport
{
    using GalaSoft.MvvmLight.Messaging;

    public partial class ResultsPanel : UserControl // TODO, IContextMenuProvider
    {
        public ResultsPanel()
        {
            InitializeComponent();

            // TODO
            ////_resultPages = new List<TabPage>();
            ////ctlResults.SelectedIndexChanged += new EventHandler( OnSelectedIndexChanged );
            ////mnuResults.Opening += new CancelEventHandler( OnMenuOpening );
            ////mnuKeep.Click += new EventHandler( OnKeepClick );
            ////mnuRemove.Click += new EventHandler( OnRemoveClick );
            ////mnuChangeName.Click += new EventHandler( OnChangeNameClick );
            ////mnuExportToFile.Click += new EventHandler( OnExportToFileClick );
            ////mnuExportToClipboard.Click += new EventHandler( OnExportToClipboardClick );
        }

        // TODO
        ////#region IContextMenuProvider Members

        ////public string MenuTitle
        ////{
        ////    get { return "Result Set"; }
        ////}

        ////public bool HasContextMenu
        ////{
        ////    get { return CurrentResultSet != null; }
        ////}

        ////public ContextMenuStrip GetContextMenu()
        ////{
        ////    return mnuResults;
        ////}

        ////#endregion
    }
}

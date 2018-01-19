namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Business;
    using SqlExport.Common;
    using SqlExport.Data;
    using SqlExport.Messages;
    using SqlExport.View;

    public class ResultsPanelViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Readonly" /> property's name.
        /// </summary>
        public const string ReadonlyPropertyName = "Readonly";

        /// <summary>
        /// The <see cref="SelectedTabItem" /> property's name.
        /// </summary>
        public const string SelectedTabItemPropertyName = "SelectedTabItem";

        /// <summary>
        /// The <see cref="MessageListControl" /> property's name.
        /// </summary>
        public const string MessageListControlPropertyName = "MessageListControl";

        /// <summary>
        /// The <see cref="MessageListDataContext" /> property's name.
        /// </summary>
        public const string MessageListDataContextPropertyName = "MessageListDataContext";

        /// <summary>
        /// The message list data context.
        /// </summary>
        private MessageListViewModel messageListDataContext = null;

        /// <summary>
        /// The result pages.
        /// </summary>
        private List<TabItem> resultPages;

        /// <summary>
        /// The export helper.
        /// </summary>
        private ExportHelper exportHelper;

        /// <summary>
        /// The read only.
        /// </summary>
        private bool @readonly = true;

        /// <summary>
        /// The selected tab item.
        /// </summary>
        private TabItem selectedTabItem = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsPanelViewModel"/> class.
        /// </summary>
        public ResultsPanelViewModel()
        {
            this.resultPages = new List<TabItem>();
            this.DisplayPages = new ObservableCollection<TabItem>();

            TabItem ti = new TabItem();
            ti.Header = "Messages";

            this.MessageListDataContext = DependencyResolver.Default.Resolve<MessageListViewModel>();
            var messageList = new MessageList();
            messageList.DataContext = this.MessageListDataContext;

            ti.Content = messageList;
            this.DisplayPages.Add(ti);

            Messenger.Default.Register<ShowMessagesMessage>(this, this, m => this.SelectedTabItem = ti);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                var table = new System.Data.DataTable("articletable");
                table.Columns.Add("articleID");
                table.Columns.Add("title");
                table.Columns.Add("content");

                var row = table.NewRow();
                row[0] = "1";
                row[1] = "article name";
                row[2] = "article contents go here";
                table.Rows.Add(row);
                this.SetDataSource(new[] { DataResult.FromDataTable(table) });
            }
        }

        /// <summary>
        /// Gets or sets the read-only property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        [Obsolete("This functionality is no longer supported.")]
        public bool Readonly
        {
            get
            {
                return this.@readonly;
            }

            set
            {
                if (this.@readonly != value)
                {
                    this.@readonly = value;

                    // Update bindings, no broadcast
                    this.RaisePropertyChanged(ReadonlyPropertyName);
                }
            }
        }

        public ObservableCollection<TabItem> DisplayPages { get; private set; }

        /// <summary>
        /// Gets or sets the SelectedTabItem property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public TabItem SelectedTabItem
        {
            get
            {
                return this.selectedTabItem;
            }

            set
            {
                if (this.selectedTabItem != value)
                {
                    this.selectedTabItem = value;

                    // Update bindings, no broadcast
                    this.RaisePropertyChanged(SelectedTabItemPropertyName);
                    if (this.SelectedTabItem != null)
                    {
                        this.selectedTabItem.Dispatcher.BeginInvoke(
                                        new Action(() =>
                                            Messenger.Default.Send(new SelectedResultSetChanged(this.CurrentResultSetContext), this))); 
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the MessageListDataContext property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MessageListViewModel MessageListDataContext
        {
            get
            {
                return this.messageListDataContext;
            }

            set
            {
                if (this.messageListDataContext == value)
                {
                    return;
                }

                this.messageListDataContext = value;
                this.RaisePropertyChanged(MessageListDataContextPropertyName);
            }
        }

        /// <summary>
        /// Gets the currently selected results set.
        /// </summary>
        private object CurrentResultSetContext
        {
            get
            {
                if (this.SelectedTabItem != null)
                {
                    object context = null;
                    this.SelectedTabItem.Dispatcher.Invoke(
                        new Action(
                            () =>
                            {
                                var grid = this.SelectedTabItem.Content as IDataView;
                                if (grid != null)
                                {
                                    context = grid.DataContext;
                                }
                            }));

                    return context;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the export started.
        /// </summary>
        internal Action ExportStarted { get; set; }

        /// <summary>
        /// Gets or sets the export complete.
        /// </summary>
        internal Action ExportComplete { get; set; }

        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetDataSource(IEnumerable<DataResult> data)
        {
            foreach (var item in this.resultPages)
            {
                UnloadResultSet(item);
            }

            this.resultPages.Clear();
            if (data != null)
            {
                this.selectedTabItem.Dispatcher.Invoke(
                    new Action(
                        () =>
                        {
                            foreach (var item in data)
                            {
                                TabItem tp = new TabItem();

                                // Replace single underscores with double to escape hot-key assignment.
                                tp.Header = item.Name.Replace("_", "__");

                                // TODO 
                                ////tp.ContextMenuStrip = GetContextMenu();

                                var view = DependencyResolver.Default.Resolve<IDataView>();

                                tp.Content = view;

                                // TODO
                                ////AddCustomDataTableStyle( dg, item );

                                DisplayPages.Insert(DisplayPages.Count - 1, tp);
                                this.resultPages.Add(tp);

                                Messenger.Default.Send(new SetDataResultMessage() { Data = item }, view.DataContext);
                            }

                            if (this.resultPages.Count > 0)
                            {
                                this.SelectedTabItem = this.resultPages[0];
                            }
                        }));
            }
        }

        /// <summary>
        /// Exports the results.
        /// </summary>
        public void ExportResults()
        {
            this.ExportResults(true);
        }

        /// <summary>
        /// Exports the results.
        /// </summary>
        /// <param name="toFile">if set to <c>true</c> [to file].</param>
        public void ExportResults(bool toFile)
        {
            try
            {
                DataResult data = null;

                var context = this.CurrentResultSetContext;
                if (context != null)
                {
                    Messenger.Default.Send(new GetDataResultMessage(r => data = r), context);
                }

                if (data != null)
                {
                    this.exportHelper = new ExportHelper() { ExportToFile = toFile };
                    this.exportHelper.ExportStarted += new EventHandler(this.OnExportStarted);
                    this.exportHelper.ExportComplete += new EventHandler(this.OnExportComplete);
                    this.exportHelper.ExportData(data, Options.Current.ExportType);
                }
                else
                {
                    this.MessageListDataContext.AddMessage("No data is currently selected.", DisplayMessageType.Information);
                    Messenger.Default.Send(new ShowMessagesMessage(), this);
                }
            }
            catch (Exception exp)
            {
                this.MessageListDataContext.AddError(exp);
                Messenger.Default.Send(new ShowMessagesMessage(), this);
            }
        }

        ////private void AddCustomDataTableStyle( DataGrid dataGrid, DataTable data )
        ////{
        ////    dataGrid.TableStyles.Clear();

        ////    Graphics graphics = dataGrid.CreateGraphics();
        ////    // Create a new DataGridTableStyle and set
        ////    // its MappingName to the TableName of a DataTable.
        ////    DataGridTableStyle dgts = new DataGridTableStyle();
        ////    // TODO dgts.MappingName = dataTable.TableName;

        ////    // Add a GridColumnStyle and set its MappingName
        ////    // to the name of a DataColumn in the DataTable.
        ////    // Set the HeaderText and Width properties.
        ////    foreach( DataColumn column in data.Columns )
        ////    {
        ////        DataGridColumnStyle colMapping;
        ////        switch( column.DataType.ToString() )
        ////        {
        ////            case "System.Boolean":
        ////                colMapping = new BooleanWidthGridColumn();
        ////                break;
        ////            default:
        ////                colMapping = new TextWidthGridColumn();
        ////                break;
        ////        }
        ////        colMapping.MappingName = column.ColumnName;
        ////        colMapping.HeaderText = column.ColumnName;
        ////        colMapping.Width = (int)(graphics.MeasureString( column.ColumnName, dataGrid.Font ).Width + 8);

        ////        // TODO Depends on DataReader.Reset!
        ////        //foreach( var item in data.Take( 10 ) )
        ////        //{
        ////        //    if( !(item[column.ColumnName] is DBNull) )
        ////        //    {
        ////        //        colMapping.Width = Math.Max( (int)graphics.MeasureString( item[column.ColumnName].ToString(),
        ////        //            dataGrid.Font ).Width, colMapping.Width );
        ////        //    }
        ////        //}

        ////        dgts.GridColumnStyles.Add( colMapping );
        ////    }

        ////    // Add the DataGridTableStyle objects to the collection.
        ////    dataGrid.TableStyles.Add( dgts );
        ////}

        private void UnloadResultSet(TabItem page)
        {
            this.DisplayPages.Remove(page);

            var context = this.CurrentResultSetContext;
            if (context != null)
            {
                ////IDisposable data = dg.ItemsSource as IDisposable;
                ////if( data != null )
                ////{
                ////    data.Dispose();
                ////}

                Messenger.Default.Send(new UnloadDataMessage(), context);
            }
        }

        ////private void OnSelectedResultsChanged()
        ////{
        ////    if( SelectedResultsChanged != null )
        ////    {
        ////        SelectedResultsChanged( this, EventArgs.Empty );
        ////    }
        ////}

        ////#region Event Handlers

        ////private void OnMenuOpening( object sender, CancelEventArgs e )
        ////{
        ////    TabPage tp = ctlResults.SelectedTab;
        ////    if( tp != null )
        ////    {
        ////        if( _resultPages.Contains( tp ) )
        ////        {
        ////            mnuKeep.Visible = true;
        ////        }
        ////        else
        ////        {
        ////            mnuKeep.Visible = false;
        ////        }
        ////    }

        ////    ContextMenuHelper.AddHierachicalContextMenu( this, GetContextMenu() );
        ////}

        ////private void OnKeepClick( object sender, EventArgs e )
        ////{
        ////    TabPage tp = ctlResults.SelectedTab;
        ////    if( tp != null && _resultPages.Contains( tp ) )
        ////    {
        ////        _resultPages.Remove( tp );
        ////    }
        ////}

        ////private void OnRemoveClick( object sender, EventArgs e )
        ////{
        ////    TabPage tp = ctlResults.SelectedTab;
        ////    if( tp != null )
        ////    {
        ////        UnloadResultSet( tp );
        ////        if( _resultPages.Contains( tp ) )
        ////        {
        ////            _resultPages.Remove( tp );
        ////        }
        ////    }
        ////}

        ////private void OnChangeNameClick( object sender, EventArgs e )
        ////{
        ////    TabPage tp = ctlResults.SelectedTab;
        ////    if( tp != null )
        ////    {
        ////        InputDialog id = new InputDialog();
        ////        tp.Text = id.Show( "Rename", "Enter new result set name", tp.Text );
        ////        DataTable data;
        ////        DataGrid grid = CurrentResultSet;
        ////        if( grid != null )
        ////        {
        ////            data = grid.DataSource as DataTable;
        ////            if( data != null )
        ////            {
        ////                data.TableName = tp.Text;
        ////            }
        ////        }
        ////    }
        ////}

        ////private void OnExportToFileClick( object sender, EventArgs e )
        ////{
        ////    ExportResults( true );
        ////}

        ////private void OnExportToClipboardClick( object sender, EventArgs e )
        ////{
        ////    ExportResults( false );
        ////}

        ////private void OnSelectedIndexChanged( object sender, EventArgs e )
        ////{
        ////    OnSelectedResultsChanged();
        ////}

        /// <summary>
        /// Called when [export started].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnExportStarted(object sender, EventArgs e)
        {
            Messenger.Default.Send(new ExportStartedMessage(), this);
        }

        /// <summary>
        /// Called when [export complete].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnExportComplete(object sender, EventArgs e)
        {
            if (this.MessageListDataContext.Messages.Count > 0)
            {
                this.MessageListDataContext.AddSeperator();
            }

            if (this.exportHelper.Error == null)
            {
                string filename = this.exportHelper.Filename;
                if (!this.exportHelper.ExportToFile)
                {
                    try
                    {
                        Clipboard.SetText(this.exportHelper.ClipboardData.ToString());
                        this.MessageListDataContext.AddMessage("Export to clipboard complete.", DisplayMessageType.Success);
                    }
                    catch (OutOfMemoryException)
                    {
                        this.MessageListDataContext.AddMessage("Export is too large for the clipboard.", DisplayMessageType.Error);
                    }
                }
                else if (!string.IsNullOrEmpty(filename))
                {
                    this.MessageListDataContext.AddMessage(
                        string.Format("Export to {0} complete.", filename),
                        DisplayMessageType.Success,
                        () =>
                        {
                            Process p = new Process();
                            p.StartInfo = new ProcessStartInfo(filename);
                            p.Start();
                        });
                }
                else
                {
                    this.MessageListDataContext.AddMessage("Export cancelled.", DisplayMessageType.Information);
                }
            }
            else
            {
                this.MessageListDataContext.AddError(this.exportHelper.Error);
                this.MessageListDataContext.AddMessage(string.Format("Export to {0} failed.", this.exportHelper.Filename), DisplayMessageType.Warning);
            }

            Messenger.Default.Send(new ShowMessagesMessage(), this);

            this.exportHelper.ExportStarted -= this.OnExportStarted;
            this.exportHelper.ExportComplete -= this.OnExportComplete;
            this.exportHelper = null;

            Messenger.Default.Send(new ExportCompletedMessage(), this);
        }
    }
}

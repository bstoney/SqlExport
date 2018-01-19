namespace SqlExport.View
{
    using System.ComponentModel;
    using System.Windows.Controls;

    /// <summary>
    /// Defines the ResultsPanel type.
    /// </summary>
    public partial class ResultsPanel : UserControl // TODO, IContextMenuProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsPanel"/> class.
        /// </summary>
        public ResultsPanel()
        {
            // Create local resources for Blend.
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                App.CreateStaticResourcesForDesigner(this);
            }

            this.InitializeComponent();

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

namespace SqlExport.View
{
    using System.ComponentModel;

    /// <summary>
    /// Handler for item selected events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="lineNumber">The line number.</param>
    public delegate void ErrorSelectedHandler(object sender, int lineNumber);

    /// <summary>
    /// A control to display a list of errors or messages and associated details.
    /// </summary>
    public partial class MessageList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageList"/> class.
        /// </summary>
        public MessageList()
        {
            // Create local resources for Blend.
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                App.CreateStaticResourcesForDesigner(this);
            }

            this.InitializeComponent();
        }
    }
}
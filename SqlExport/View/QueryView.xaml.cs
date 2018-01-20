namespace SqlExport.View
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    using SqlExport.ViewModel;

    /// <summary>
    /// Interaction logic for QueryView
    /// </summary>
    public partial class QueryView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryView"/> class.
        /// </summary>
        public QueryView()
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

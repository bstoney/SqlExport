namespace SqlExport
{
    using System.Windows;
    using System.Windows.Controls;

    using SqlExport.Common;
    using SqlExport.View;
    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the Locator type.
    /// </summary>
    public class Locator
    {
        /// <summary>
        /// Gets the main window view model.
        /// </summary>
        public static MainWindowViewModel MainWindowViewModel
        {
            get { return DependencyResolver.Default.Resolve<MainWindowViewModel>(); }
        }

        /// <summary>
        /// Gets the main menu view model.
        /// </summary>
        public MainMenuViewModel MainMenuViewModel
        {
            get { return DependencyResolver.Default.Resolve<MainMenuViewModel>(); }
        }

        /// <summary>
        /// Gets the results panel view model.
        /// </summary>
        /// <value>
        /// The results panel view model.
        /// </value>
        public ResultsPanelViewModel ResultsPanelViewModel
        {
            get { return DependencyResolver.Default.Resolve<ResultsPanelViewModel>(); }
        }

        /// <summary>
        /// Gets the data view view model.
        /// </summary>
        /// <value>
        /// The data view view model.
        /// </value>
        public DataViewViewModel DataViewViewModel
        {
            get { return DependencyResolver.Default.Resolve<DataViewViewModel>(); }
        }

        /// <summary>
        /// Gets the export dialog view model.
        /// </summary>
        public ExportDialogViewModel ExportDialogViewModel
        {
            get { return DependencyResolver.Default.Resolve<ExportDialogViewModel>(); }
        }

        /// <summary>
        /// Gets the status panel view.
        /// </summary>
        public Control StatusPanelView
        {
            get { return DependencyResolver.Default.Resolve<StatusPanelView>(); }
        }

        /// <summary>
        /// Gets the status panel view model.
        /// </summary>
        public StatusPanelViewModel StatusPanelViewModel
        {
            get { return DependencyResolver.Default.Resolve<StatusPanelViewModel>(); }
        }

        /// <summary>
        /// Gets the about view model.
        /// </summary>
        public AboutViewModel AboutViewModel
        {
            get { return DependencyResolver.Default.Resolve<AboutViewModel>(); }
        }

        /// <summary>
        /// Finds the locator.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The resource locator.</returns>
        public static Locator FindLocator(FrameworkElement element)
        {
            return element.FindResource("Locator") as Locator;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Windows;
using SqlExport.Ui;
using SqlExport.Ui.View;
using System.Windows.Controls;
using SqlExport.Ui.View.StatusPanel;

namespace SqlExport.ViewModel
{
    using SqlExport.Common;

    public class ViewModelLocator
    {
        public static ViewModelLocator FindLocator(FrameworkElement element)
        {
            return element.FindResource("Locator") as ViewModelLocator;
        }

        public ViewModelLocator Locator
        {
            get { return DependencyResolver.Default.Resolve<ViewModelLocator>(); }
        }

        public ResultsPanelViewModel ResultsPanelViewModel
        {
            get { return DependencyResolver.Default.Resolve<ResultsPanelViewModel>(); }
        }

        public MessageListViewModel MessageListViewModel
        {
            get { return DependencyResolver.Default.Resolve<MessageListViewModel>(); }
        }

        public ApplicationMessagesViewModel ApplicationMessages
        {
            get { return DependencyResolver.Default.Resolve<ApplicationMessagesViewModel>(); }
        }

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
    }
}

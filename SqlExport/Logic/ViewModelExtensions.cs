namespace SqlExport.Logic
{
    using System;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// Defines the ViewModelExtensions class.
    /// </summary>
    internal static class ViewModelExtensions
    {
        /// <summary>
        /// Invokes the on dispatcher.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="action">The action.</param>
        public static void InvokeOnDispatcher(this ViewModelBase viewModel, Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }
    }
}

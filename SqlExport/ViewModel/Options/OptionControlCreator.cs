namespace SqlExport.ViewModel.Options
{
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Defines the OptionControlCreator class.
    /// </summary>
    internal abstract class OptionControlCreator
    {
        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>A new UI control.</returns>
        public abstract FrameworkElement GetEditControl(Binding binding);
    }
}

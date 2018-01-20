namespace SqlExport.ViewModel.Options
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Defines the BooleanOptionControlCreator class.
    /// </summary>
    internal class BooleanOptionControlCreator : OptionControlCreator
    {
        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>A new UI control.</returns>
        public override FrameworkElement GetEditControl(Binding binding)
        {
            var boolean = new CheckBox();
            boolean.SetBinding(CheckBox.IsCheckedProperty, binding);
            return boolean;
        }
    }
}

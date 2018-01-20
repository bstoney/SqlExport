namespace SqlExport.ViewModel.Options
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Defines the TextOptionControlCreator class.
    /// </summary>
    internal class TextOptionControlCreator : OptionControlCreator
    {
        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>A new UI control.</returns>
        public override FrameworkElement GetEditControl(Binding binding)
        {
            var text = new TextBox();
            text.SetBinding(TextBox.TextProperty, binding);
            return text;
        }
    }
}

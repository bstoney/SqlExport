namespace SqlExport.ViewModel.Options
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Defines the NumericOptionControlCreator class.
    /// </summary>
    internal class NumericOptionControlCreator : OptionControlCreator
    {
        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>A new UI control.</returns>
        public override FrameworkElement GetEditControl(Binding binding)
        {
            var numeric = new TextBox();
            numeric.PreviewTextInput += this.textbox_PreviewTextInput;
            numeric.SetBinding(TextBox.TextProperty, binding);
            return numeric;
        }

        /// <summary>
        /// Handles the PreviewTextInput event of the textbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Set the event as handled is any characters are not a number.
            e.Handled = e.Text.Any(c => !Char.IsNumber(c));
        }
    }
}

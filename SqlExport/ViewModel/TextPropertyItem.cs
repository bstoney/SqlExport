namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Defines the TextPropertyItem class.
    /// </summary>
    public class TextPropertyItem : PropertyItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextPropertyItem"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="name">The name.</param>
        public TextPropertyItem(string category, string name)
            : base(category, name)
        {
        }

        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>
        /// A new UI control.
        /// </returns>
        public override FrameworkElement GetEditControl(Binding binding)
        {
            var text = new TextBox();
            text.SetBinding(TextBox.TextProperty, binding);

            return text;
        }
    }
}

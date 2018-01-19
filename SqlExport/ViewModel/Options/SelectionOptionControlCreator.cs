namespace SqlExport.ViewModel.Options
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Defines the SelectionOptionControlCreator class.
    /// </summary>
    internal class SelectionOptionControlCreator : OptionControlCreator
    {
        /// <summary>
        /// The items
        /// </summary>
        private readonly IEnumerable<string> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionOptionControlCreator"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public SelectionOptionControlCreator(IEnumerable<string> items)
        {
            this.items = (items ?? Enumerable.Empty<string>()).ToArray();
        }

        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>A new UI control.</returns>
        public override FrameworkElement GetEditControl(Binding binding)
        {
            var combo = new ComboBox();
            combo.SetBinding(ComboBox.SelectedValueProperty, binding);
            this.items.ToList().ForEach(i => combo.Items.Add(i));
            return combo;
        }
    }
}

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
    /// Defines the SelectionPropertyItem class.
    /// </summary>
    internal class SelectionPropertyItem : PropertyItem
    {
        /// <summary>
        /// The items.
        /// </summary>
        private readonly IEnumerable<string> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionPropertyItem" /> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="name">The name.</param>
        /// <param name="items">The items.</param>
        public SelectionPropertyItem(string category, string name, IEnumerable<string> items)
            : base(category, name)
        {
            this.items = items;
        }

        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>A UI control.</returns>
        public override FrameworkElement GetEditControl(Binding binding)
        {
            var combo = new ComboBox();
            combo.SetBinding(ComboBox.TextProperty, binding);
            this.items.ToList().ForEach(i => combo.Items.Add(i));

            return combo;
        }
    }
}

namespace SqlExport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SqlExport.Common.Extensions;
    using SqlExport.Common.Options;

    /// <summary>
    /// Defines the SelectionExtensionOption class.
    /// </summary>
    public class SelectionExtensionOption : ExtensionOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionExtensionOption" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="selectionItems">The selection items.</param>
        public SelectionExtensionOption(string name, IEnumerable<string> selectionItems)
            : base(name)
        {
            this.OptionType = OptionType.Selection;
            this.SelectionItems = selectionItems.ToArray();
        }

        /// <summary>
        /// Gets the selection items.
        /// </summary>
        /// <value>
        /// The selection items.
        /// </value>
        public string[] SelectionItems { get; private set; }
    }
}

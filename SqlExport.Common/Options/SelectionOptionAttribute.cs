namespace SqlExport.Common.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the SelectionOptionAttribute class.
    /// </summary>
    public class SelectionOptionAttribute : OptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionOptionAttribute" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="selectionItems">The option values.</param>
        public SelectionOptionAttribute(string path, string[] selectionItems)
            : base(path)
        {
            this.OptionType = OptionType.Selection;
            this.SelectionItems = selectionItems;
        }

        /// <summary>
        /// Gets or sets the selection items.
        /// </summary>
        public string[] SelectionItems { get; set; }
    }
}

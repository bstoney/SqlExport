namespace SqlExport.Common.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the ISchemaItem interface.
    /// </summary>
    public interface ISchemaItem
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the query text.
        /// </summary>
        string QueryText { get; }

        /// <summary>
        /// Gets the type of the schema item.
        /// </summary>
        /// <value>
        /// The type of the schema item.
        /// </value>
        SchemaItemType SchemaItemType { get; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        IEnumerable<ISchemaItem> Children { get; }
    }
}
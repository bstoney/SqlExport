namespace SqlExport.Data
{
    using System;
    using System.Collections.Generic;

    using SqlExport.Common.Data;

    /// <summary>
    /// Defines the SchemaItem class.
    /// </summary>
    public class SchemaItem : ISchemaItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public SchemaItem(string name, SchemaItemType type)
            : this(name, name, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="type">The type.</param>
        public SchemaItem(string name, string displayName, SchemaItemType type)
            : this(name, displayName, name, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="queryText">The query text.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if name is null.</exception>
        public SchemaItem(string name, string displayName, string queryText, SchemaItemType type)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.Name = name;
            this.DisplayName = displayName ?? name;
            this.QueryText = queryText ?? name;
            this.SchemaItemType = type;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; protected set; }

        /// <summary>
        /// Gets or sets the query text.
        /// </summary>
        public string QueryText { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the schema item.
        /// </summary>
        /// <value>
        /// The type of the schema item.
        /// </value>
        public SchemaItemType SchemaItemType { get; protected set; }

        /// <summary>
        /// Gets or sets the children items.
        /// </summary>
        public IEnumerable<ISchemaItem> Children { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}

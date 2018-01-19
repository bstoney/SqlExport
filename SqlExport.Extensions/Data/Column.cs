using System;

namespace SqlExport.Data
{
    using System.Collections.Generic;

    using SqlExport.Common.Data;

    /// <summary>
    /// Describes a data column.
    /// </summary>
    public class Column : SchemaItem
    {
        /// <summary>
        /// Initializes a new instance of the Column class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="nullable">The nullable.</param>
        public Column(string name, string type, bool? nullable)
            : this(name, name, type, nullable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Column class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="queryText">The query text.</param>
        /// <param name="type">The type.</param>
        /// <param name="nullable">The null-able.</param>
        public Column(string name, string queryText, string type, bool? nullable)
            : base(name, SchemaItemType.Column)
        {
            this.Type = type;
            this.Nullable = nullable;
            this.QueryText = queryText;
            this.SetDisplayName();
        }

        /// <summary>
        /// Initializes a new instance of the Column class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="queryText">The query text.</param>
        /// <param name="type">The type.</param>
        /// <param name="underlyingType">Type of the underlying.</param>
        /// <param name="nullable">The null-able.</param>
        public Column(string name, string queryText, string type, Type underlyingType, bool? nullable)
            : this(name, name, type, nullable)
        {
            this.UnderlyingType = underlyingType;
        }

        /// <summary>
        /// Gets a description of the column type.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the column is null-able.
        /// </summary>
        public bool? Nullable { get; private set; }

        /// <summary>
        /// Gets the underlying type of the column.
        /// </summary>
        public Type UnderlyingType { get; private set; }

        /// <summary>
        /// Gets or sets the children items.
        /// </summary>
        private new IEnumerable<ISchemaItem> Children { get; set; }

        /// <summary>
        /// Sets the display name.
        /// </summary>
        private void SetDisplayName()
        {
            string value;
            if (this.Type != null)
            {
                if (this.Nullable.HasValue)
                {
                    value = string.Format("{0} ({1},{2})", this.Name, this.Type, this.Nullable.Value ? "null" : "not null");
                }
                else
                {
                    value = string.Format("{0} ({1})", this.Name, this.Type);
                }
            }
            else if (this.Nullable.HasValue)
            {
                value = string.Format("{0} ({1})", this.Name, this.Nullable.Value ? "null" : "not null");
            }
            else
            {
                value = this.Name;
            }

            this.DisplayName = value;
        }
    }
}

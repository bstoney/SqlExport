namespace SqlExport.Common.Data
{
    using System.Collections.Specialized;

    /// <summary>
    /// Statement Template collection.
    /// </summary>
    public sealed class StatementTemplateCollection : NameObjectCollectionBase
    {
        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>A list of statement templates.</returns>
        public IStatementTemplate[] this[string category]
        {
            get { return (IStatementTemplate[])this.BaseGet(category); }
            set { this.BaseSet(category, value); }
        }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A list of statement templates.</returns>
        public IStatementTemplate[] this[int index]
        {
            get { return (IStatementTemplate[])this.BaseGet(index); }
            set { this.BaseSet(index, value); }
        }

        /// <summary>
        /// Removes the entry with the specified key.
        /// </summary>
        /// <param name="category">The category.</param>
        public void Remove(string category)
        {
            this.BaseRemove(category);
        }

        /// <summary>
        /// Removes the entry at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        /// <summary>
        /// Adds the statement templates with the supplied key.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="statements">The statements.</param>
        public void Add(string category, IStatementTemplate[] statements)
        {
            this.BaseAdd(category, statements);
        }
    }
}


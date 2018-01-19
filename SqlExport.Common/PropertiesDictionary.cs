namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SqlExport.Common.Options;

    /// <summary>
    /// Defines the PropertiesDictionary type.
    /// </summary>
    public class PropertiesDictionary : IEnumerable<OptionProperty>
    {
        /// <summary>
        /// The properties.
        /// </summary>
        private readonly Dictionary<string, OptionProperty> properties;

        /// <summary>
        /// The option.
        /// </summary>
        private readonly Option option;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesDictionary" /> class.
        /// </summary>
        /// <param name="option">The option.</param>
        internal PropertiesDictionary(Option option)
        {
            this.option = option;
            this.properties = new Dictionary<string, OptionProperty>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get { return this.properties.Count; }
        }

        /// <summary>
        /// Gets or sets the <see cref="OptionProperty"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="OptionProperty"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns>A option property.</returns>
        public OptionProperty this[string name]
        {
            get
            {
                return this.properties.ContainsKey(name) ? this.properties[name] : null;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.properties[name] = value;
                value.Parent = this.option;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<OptionProperty> GetEnumerator()
        {
            return this.properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void SetPropertyValue(string name, string value)
        {
            if (this[name] == null)
            {
                this[name] = new OptionProperty(this.option, new OptionName(name, true));
            }

            this[name].Value = value;
        }

        /// <summary>
        /// Removes the value with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        internal void Remove(string name)
        {
            if (this[name] != null)
            {
                this[name].Parent = null;
                this.properties.Remove(name);
            }
        }
    }
}
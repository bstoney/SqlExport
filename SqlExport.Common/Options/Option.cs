namespace SqlExport.Common.Options
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the Option type.
    /// </summary>
    public class Option : OptionBase
    {
        /// <summary>
        /// The children.
        /// </summary>
        private readonly List<Option> children;

        /// <summary>
        /// Initializes a new instance of the <see cref="Option" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        internal Option(OptionName name)
            : base(name)
        {
            this.Properties = new PropertiesDictionary(this);
            this.children = new List<Option>();
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public PropertiesDictionary Properties { get; private set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public IEnumerable<Option> Children
        {
            get { return this.children; }
        }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="childOption">The child option.</param>
        public void AddChild(Option childOption)
        {
            childOption.Parent = this;
            this.children.Add(childOption);
        }

        /// <summary>
        /// Removes the child.
        /// </summary>
        /// <param name="childOption">The child option.</param>
        public void RemoveChild(Option childOption)
        {
            childOption.Parent = null;
            this.children.Remove(childOption);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "{0} = {1}, {2} {3}, {4} {5}",
                this.Name,
                this.Value ?? "(null)",
                this.Properties.Count,
                this.Properties.Count == 1 ? "Property" : "Properties",
                this.Children.Count(),
                this.Children.Count() == 1 ? "Child" : "Children");
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <returns>A path.</returns>
        public override OptionPath GetPath()
        {
            // The path is always relative the the root node.
            var name = this.Name;

            if (this.Parent != null)
            {
                // Find the option index.
                var siblings = this.Parent.Children.Where(o => o.Name == this.Name).ToList();
                var index = siblings.IndexOf(this);

                name = new OptionName(this.Name.Name, index);

                if (this.Parent.Parent != null)
                {
                    return this.Parent.GetPath() + name;
                }
            }

            return new OptionPath(new[] { name });
        }

        /// <summary>
        /// Gets the value by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The value at the given path if found; otherwise, null.</returns>
        public string GetValueByPath(string path)
        {
            // Grab the next node name in the xpath; or return parent if empty
            var partsOfXPath = new OptionPath(path);
            var name = partsOfXPath.First();
            if (name.IsEmpty)
            {
                return this.Value;
            }

            if (name.IsProperty)
            {
                var optionProperty = this.Properties[name.Name];
                return optionProperty != null ? optionProperty.Value : null;
            }

            var indexQuery = this.GetIndexQuery(name);

            return indexQuery.Select(c => c.GetValueByPath(string.Join("/", partsOfXPath.Skip(1))))
                .FirstOrDefault();
        }

        /// <summary>
        /// Sets the value of all occurrences of the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        public void SetValueByPath(string path, string value)
        {
            // Grab the next node name in the xpath; or return parent if empty
            var partsOfXPath = new OptionPath(path);
            var name = partsOfXPath.First();
            if (name.IsEmpty)
            {
                this.Value = value;
            }
            else if (name.IsProperty)
            {
                this.Properties.SetPropertyValue(name.Name, value);
            }
            else
            {
                var indexQueryList = this.GetIndexQuery(name).ToList();

                // If the path does not exist yet, then create it.
                if (!indexQueryList.Any())
                {
                    // Create enough new options to reach the index specified.
                    var optionsToCreate = (name.Index ?? 0) - this.Children.Count(o => o.Name == name) + 1;
                    Option newOption;
                    do
                    {
                        newOption = new Option(name);
                        this.AddChild(newOption);
                        optionsToCreate--;
                    }
                    while (optionsToCreate > 0);

                    // Add the last one to the query results;
                    indexQueryList.Add(newOption);
                }

                indexQueryList.ForEach(c => c.SetValueByPath(string.Join("/", partsOfXPath.Skip(1)), value));
            }
        }

        /// <summary>
        /// Gets the children by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>All the children with the matching path.</returns>
        public IEnumerable<Option> GetChildrenByPath(string path)
        {
            // Grab the next node name in the xpath; or return parent if empty
            var partsOfXPath = new OptionPath(path);
            var name = partsOfXPath.First();
            if (name.IsEmpty)
            {
                return new[] { this };
            }

            var indexQuery = this.GetIndexQuery(name);

            return indexQuery.SelectMany(c => c.GetChildrenByPath(string.Join("/", partsOfXPath.Skip(1))));
        }

        /// <summary>
        /// Gets all options by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>All the options or option properties matching the path.</returns>
        public IEnumerable<OptionBase> GetAllByPath(string path)
        {
            // Grab the next node name in the xpath; or return parent if empty
            var partsOfXPath = new OptionPath(path);
            var name = partsOfXPath.First();
            if (name.IsEmpty)
            {
                return new[] { this };
            }

            if (name.IsProperty)
            {
                return new[] { this.Properties[name.Name] };
            }

            var indexQuery = this.GetIndexQuery(name);

            return indexQuery.SelectMany(c => c.GetAllByPath(string.Join("/", partsOfXPath.Skip(1))));
        }

        /// <summary>
        /// Removes the by path.
        /// </summary>
        /// <param name="path">The path.</param>
        public IEnumerable<Option> RemoveByPath(string path)
        {
            // Grab the next node name in the xpath; or return parent if empty
            var partsOfXPath = new OptionPath(path);
            var name = partsOfXPath.First();
            if (name.IsProperty)
            {
                this.Properties.Remove(name.Name);
            }
            else if (name.IsEmpty)
            {
                yield return this;
            }
            else
            {
                var indexQuery = this.GetIndexQuery(name);

                indexQuery.SelectMany(c => c.RemoveByPath(string.Join("/", partsOfXPath.Skip(1))))
                          .ToList()
                          .ForEach(this.RemoveChild);
            }
        }

        /// <summary>
        /// Gets the option query ensuring indexing is treated correctly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// All the matching options.
        /// </returns>
        private IEnumerable<Option> GetIndexQuery(OptionName name)
        {
            var childrenQuery = from c in this.Children
                                where c.Name == name
                                select c;
            var indexQuery = childrenQuery.Skip(name.Index ?? 0)
                .TakeWhile((c, i) => name.Index == null || i == 0);
            return indexQuery;
        }
    }
}

namespace SqlExport.Common.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the OptionPath class.
    /// </summary>
    public class OptionPath : IEnumerable<OptionName>
    {
        /// <summary>
        /// The path
        /// </summary>
        private readonly IList<OptionName> path;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionPath"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        internal OptionPath(string path)
        {
            var partsOfXPath = from n in path.Trim('/').Split('/')
                               select GetOptionName(n);
            this.path = partsOfXPath.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionPath"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        internal OptionPath(IEnumerable<OptionName> path)
        {
            this.path = path.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionPath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        private OptionPath(IEnumerable<OptionName> path, OptionName name)
            : this(path)
        {
            this.path.Add(name);
        }

        /// <summary>
        /// Adds the option name to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        /// <returns>A new path.</returns>
        public static OptionPath operator +(OptionPath path, OptionName name)
        {
            return new OptionPath(path, name);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Join("/", this.path.Select(n => n.ToString()));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<OptionName> GetEnumerator()
        {
            return this.path.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.path.GetEnumerator();
        }

        /// <summary>
        /// Gets the name of the option.
        /// </summary>
        /// <param name="nextNodeInXPath">The next node in X path.</param>
        /// <returns>
        /// The option name details.
        /// </returns>
        private static OptionName GetOptionName(string nextNodeInXPath)
        {
            if (string.IsNullOrWhiteSpace(nextNodeInXPath))
            {
                return OptionName.Empty;
            }

            if (nextNodeInXPath.StartsWith("@"))
            {
                return new OptionName(nextNodeInXPath.Substring(1), true);
            }

            if (!string.IsNullOrEmpty(nextNodeInXPath))
            {
                var indexMatch = Regex.Match(nextNodeInXPath, @"^(?<name>.+)[[](?<index>\d+)[]]$");
                if (indexMatch.Success)
                {
                    // XPath indexing starts at 1.
                    return new OptionName(
                        indexMatch.Groups["name"].Value, int.Parse(indexMatch.Groups["index"].Value) - 1);
                }
            }

            return new OptionName(nextNodeInXPath);
        }
    }
}

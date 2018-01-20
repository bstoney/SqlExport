namespace SqlExport.Editor
{
    using System;

    /// <summary>
    /// Defines the TextFinder class.
    /// </summary>
    public class TextFinder
    {
        /// <summary>
        /// The comparison
        /// </summary>
        private readonly StringComparison comparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFinder"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="ignoreCase">if set to <c>true</c> ignore case.</param>
        public TextFinder(string match, bool ignoreCase)
        {
            this.Match = match;
            this.comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
        }

        /// <summary>
        /// Gets the match.
        /// </summary>
        public string Match { get; private set; }

        /// <summary>
        /// Finds the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>An integer.</returns>
        public int Find(string text, int startIndex)
        {
            var index = text.IndexOf(this.Match, startIndex, this.comparison);
            if (index < 0)
            {
                index = text.IndexOf(this.Match, 0, startIndex, this.comparison);
            }

            return index;
        }
    }
}

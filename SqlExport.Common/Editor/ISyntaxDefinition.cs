namespace SqlExport.Common.Editor
{
    /// <summary>
    /// Defines the ISyntaxDefinition interface.
    /// </summary>
    public interface ISyntaxDefinition
    {
        /// <summary>
        /// Gets the keyword capitalisation.
        /// </summary>
        Capitalisation KeywordCapitalisation { get; }

        CommentSyntax CommentSyntax { get; }

        /// <summary>
        /// Gets the keywords.
        /// </summary>
        string[] Keywords { get; }

        /// <summary>
        /// Gets the functions.
        /// </summary>
        string[] Functions { get; }

        /// <summary>
        /// Gets the data types.
        /// </summary>
        string[] DataTypes { get; }

        /// <summary>
        /// Gets the operators.
        /// </summary>
        string[] Operators { get; }
    }
}

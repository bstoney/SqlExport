using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Editor;

namespace SqlExport.Data.Adapters.Text
{
    using SqlExport.Common.Editor;

    internal class SyntaxDefinition : ISyntaxDefinition
    {
        internal readonly static string[] TextKeywords = new string[] { "FROM", "SELECT", "WHERE" };

        #region ISyntaxDefinition Members

        public Capitalisation KeywordCapitalisation
        {
            get { return Capitalisation.UpperCase; }
        }

        public CommentSyntax CommentSyntax
        {
            get { return CommentSyntax.TSql; }
        }

        public string[] Keywords
        {
            get { return TextKeywords; }
        }

        public string[] Functions
        {
            get { return new string[] { }; }
        }

        public string[] DataTypes
        {
            get
            {
                return new string[] { };
            }
        }

        public string[] Operators
        {
            get { return new string[] { }; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Editor;

namespace SqlExport.Data.Adapters.Odbc
{
    using SqlExport.Common.Editor;

    internal class SyntaxDefinition : ISyntaxDefinition
    {
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
            get
            {
                return new string[] { "FROM", "LIKE", "SELECT", "WHERE" };
            }
        }

        public string[] Functions
        {
            get
            {
                return new string[] { };
            }
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
            get { return new string[] { "NULL" }; }
        }

        #endregion
    }
}

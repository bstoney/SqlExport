using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Editor;

namespace SqlExport.Data.Adapters.Oracle
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
                return new string[] { 
					"SELECT", "FROM", "WHERE", "ORDER BY", "GROUP BY", "UNION", "INSERT", "INTO", "INNER", "OUTER",
					"CREATE", "TABLE", "DROP", "AS", "AND", "LEFT", "NOT", "IN", "OR", "DISTINCT",
					"VALUES", "CASE", "WHEN", "THEN", "HAVING", "ELSE", "END", "DUAL", "WITH", "ON", "ALL"
				};
            }
        }

        public string[] Functions
        {
            get
            {
                return new string[] { "DECODE", "TO_CHAR", "TO_DATE", "LENGTH", "MIN", "MAX", "COUNT", "SUM", "CAST",
					"SUBSTR", "NVL", "NULLIF", "TRIM", "RTRIM", "LTRIM", "ROUND", "TO_NUMBER", "ADD_MONTHS", 
					"ROWNUM", "TRUNC", "SYSDATE"
					};
            }
        }

        public string[] DataTypes
        {
            get
            {
                return new string[] { 
                    "CHAR", "NCHAR", "VARCHAR2", "NVARCHAR2", "NUMBER", "NUMERIC", "FLOAT", 
                    "DEC", "DECIMAL", "INTEGER", "INT", "SMALLINT", "REAL", "DATE", "TIMESTAMP", "BFILE", "BLOB",
                    "CLOB", "NCLOB", "ROWID", "UROWID", "XMLType"
                };
            }
        }

        public string[] Operators
        {
            get { return new string[] { "NULL", "JOIN", "IS" }; }
        }

        #endregion
    }
}

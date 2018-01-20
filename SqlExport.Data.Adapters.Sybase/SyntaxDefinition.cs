using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Editor;

namespace SqlExport.Data.Adapters.Sybase
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
                return new string[] { "ALL", "ALTER", "AND", "AS", "ASC", "AUTOINCREMENT", "BEGIN", "BETWEEN", "BY", "CLUSTERED", "CREATE", 
					"CROSS", "DECLARE", "DEFAULT", "DELETE", "DESC", "DISTINCT", "DROP", "ELSE", "END", "EXEC", "FROM", "FULL", 
					"FUNCTION", "GROUP", "HAVING", "IF", "IN", "INDEX", "INNER", "INSERT", "INTO", "KEY", "LIKE", "NOT", "ON", "OPTION", "OR", 
					"ORDER", "OUTER", "PARAMETER", "PRIMARY", "PROCEDURE", "RETURN", "RETURNS", "SELECT", "SET", "THEN", "TOP", "UNION", "UPDATE", 
					"USER", "VALUES", "WHEN", "WHERE", "WHILE" };
            }
        }

        public string[] Functions
        {
            get
            {
                return new string[] { "ASCII", "CASE", "CAST", "CEIL", "CONVERT", "COUNT", "CURRENT", "DATE", "DATEADD", 
					"DATEDIFF", "DATENAME", "DATEPART", "DAY", "EXISTS", "FLOOR", "GETDATE", "ISNULL", "LEFT", "LEN", "LTRIM", "MAX", 
					"MIN", "MONTH", "OPENQUERY", "PATINDEX", "RAISERROR", "RIGHT", "ROUND", "RTRIM", "SPACE", "SUBSTRING", "SUM", "TODAY", 
					"TRIM", "YEAR" };
            }
        }

        public string[] DataTypes
        {
            get
            {
                return new string[] { "BIGINT", "BINARY", "BIT", "CHAR", "DATE", "DATETIME", "DECIMAL", "DOUBLE", "FLOAT", "IMAGE", 
					"INT", "INTEGER", "LONG", "MONEY", "NUMERIC", "REAL", "SMALLDATETIME", "SMALLINT", "SMALLMONEY", 
					"TEXT", "TIME", "TIMESTAMP", "TINYINT", "VARBINARY", "VARCHAR" };
            }
        }

        public string[] Operators
        {
            get { return new string[] { "NULL", "JOIN", "IS" }; }
        }

        #endregion
    }
}

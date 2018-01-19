using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Editor;

namespace SqlExport.Data.Adapters.MsSql
{
    using SqlExport.Common.Editor;

    internal class SyntaxDefinition : ISyntaxDefinition
    {
        internal readonly static string[] MsSqlKeywords = new string[] { "ALL", "ALTER", "AND", "AS", "ASC", 
			"BEGIN", "BETWEEN", "BY", "CLUSTERED", "CREATE", 
			"CROSS", "CONSTRAINT", "DECLARE", "DELETE", "DESC", "DISTINCT", "DROP", 
			"ELSE", "END", "EXEC", "FOREIGN", "FROM", "FULL", "FUNCTION", "GO", "GROUP", 
			"HAVING", "IF", "IN", "INDEX", "INNER", "INSERT", "INTO", "KEY", "LIKE", "NOT", "ON", "OR", 
			"ORDER", "OUTER", "PRIMARY", "PROCEDURE", "REFERENCES", "RETURN", "RETURNS", 
			"SELECT", "SET", "THEN", "TOP", 
			"UNION", "UPDATE", "USE",
			"VALUES", "VIEW",
			"WHEN", "WHERE", "WHILE" };

        internal readonly static string[] MsSqlFunctions = new string[] { "ASCII", 
			"CASE", "CAST", "CEIL", "CONVERT", "COUNT", "CURRENT", 
			"DATE", "DATEADD", "DATEDIFF", "DATENAME", "DATEPART", "DAY", 
			"EXISTS", "FLOOR", "GETDATE", "ISNULL", "LEFT", "LEN", "LTRIM", 
			"MAX", "MIN", "MONTH", "OPENQUERY", "PATINDEX", "RIGHT", "ROUND", "RTRIM", 
			"SPACE", "SUBSTRING", "SUM", "TODAY", "TRIM", "YEAR" };

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
            get { return MsSqlKeywords; }
        }

        public string[] Functions
        {
            get { return MsSqlFunctions; }
        }

        public string[] DataTypes
        {
            get
            {
                return new string[] { "BIGINT", "BINARY", "BIT", "CHAR", "COLUMN", "DATETIME", "DECIMAL", "FLOAT", "IMAGE", 
					"INT", "MONEY", "NCHAR", "NTEXT", "NUMERIC", "NVARCHAR", "REAL", "SMALLDATETIME", "SMALLINT", 
					"SMALLMONEY", "SQL_VARIANT", "SYSNAME", "TABLE", "TEXT", "TIMESTAMP", "TINYINT", "UNIQUEIDENTIFIER", 
					"VARBINARY", "VARCHAR" };
            }
        }

        public string[] Operators
        {
            get { return new string[] { "NULL", "JOIN", "IS" }; }
        }

        #endregion
    }
}

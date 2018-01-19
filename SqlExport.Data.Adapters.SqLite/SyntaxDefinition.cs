using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Editor;

namespace SqlExport.Data.Adapters.SqLite
{
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
				return new string[] { "ALL", "ALTER", "AND", "AS", "ASC", "BEGIN", "BETWEEN", "BY", "CLUSTERED", "CREATE", 
					"CROSS", "DECLARE", "DELETE", "DESC", "DISTINCT", "DROP", "ELSE", "END", "EXEC", "FROM", "FULL", 
					"FUNCTION", "GROUP", "HAVING", "IF", "IN", "INDEX", "INNER", "INSERT", "INTO", "LIKE", "NOT", "ON", "OR", 
					"ORDER", "OUTER", "PROCEDURE", "RETURN", "RETURNS", "SELECT", "SET", "THEN", "TOP", "UNION", "UPDATE", 
					"WHEN", "WHERE", "WHILE" };
			}
		}

		public string[] Functions
		{
			get
			{
				return new string[] { "ASCII", "CASE", "CAST", "CEIL", "CONVERT", "COUNT", "CURRENT", "DATE", "DATEADD", 
					"DATEDIFF", "DATENAME", "DATEPART", "DAY", "FLOOR", "GETDATE", "ISNULL", "LEFT", "LEN", "LTRIM", "MAX", 
					"MIN", "MONTH", "OPENQUERY", "PATINDEX", "RIGHT", "ROUND", "RTRIM", "SPACE", "SUBSTRING", "SUM", "TODAY", 
					"TRIM", "YEAR" };
			}
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

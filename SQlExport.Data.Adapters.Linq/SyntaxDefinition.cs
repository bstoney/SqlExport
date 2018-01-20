using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;
using SqlExport.Editor;

namespace SqlExport.Data.Adapters.Linq
{
	internal class SyntaxDefinition : ISyntaxDefinition
	{
		internal static readonly string[] CSharpKeywords = new string[] { "abstract", "ascending", "as", "base", "break", "by", "case", "catch", "checked", "class", 
					"const", "continue", "default", "descending", "delegate", "do", "else", "enum", "event", "explicit", "extern", 
					"finally", "fixed", "for", "foreach",
					"from", "get", "goto", "group", "if", "implicit", "in", "interface", "internal", "into", "is", "join", "let",
					"lock", "namespace", "new", "operator", "orderby", "out", "override", "params", "partial", "partial", "private",
					"protected", "public", "readonly", "ref", "return", "sealed", "select", "set", "sizeof", "stackalloc", "static",
					"struct", "switch", "this", "throw", "try", "typeof", "unchecked", "unsafe", "using", "value", "virtual", "void",
					"volatile", "where", "while", "yield" };

		#region ISyntaxDefinition Members

		public Capitalisation KeywordCapitalisation
		{
			get { return Capitalisation.Default; }
		}

		public CommentSyntax CommentSyntax
		{
			get { return CommentSyntax.CSharp; }
		}

		public string[] Keywords
		{
			get { return CSharpKeywords; }
		}

		public string[] Functions
		{
			get
			{
				return new string[] { "Output", "Print", "PrintExpression", "PrintSql",
					"Aggregate", "Any", "Average", "Cast", "Concat", "Contains", "Count", "DefaultIfEmpty", "Distinct", "ElementAt",
					"ElementAtOrDefault", "Empty", "Except", "First", "FirstOrDefault", "GroupBy", "GroupJoin", "Intersect", "Join",
					"Last", "LastOrDefault", "LongCount", "Max", "Min", "OfType", "OrderBy", "DorderByDescending", "Range", "Repeat",
					"Reverse", "Select", "SelectMany", "Sequential", "SeuqenceEqual", "Single", "SingleOrDefault", "Skip", "SkipWhile",
					"Sum", "Take", "TakeWhile", "ThenBy", "ThenByDescending", "ToArray", "ToDictionary", "ToList", "ToLookup", "Union",
					"Where" };
			}
		}

		public string[] DataTypes
		{
			get
			{
				return new string[] { "bool", "byte", "char", "decimal", "double", "float", "int", "long", "object", "sbyte", "short",
					"string", "uint", "ulong", "ushort", "var" };
			}
		}

		public string[] Operators
		{
			get { return new string[] { "false", "null", "true" }; }
		}

		#endregion
	}
}

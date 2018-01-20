
namespace SqlExport.Editor
{
	public interface IEditorStyleConfiguration : IStyleConfiguration
	{
		int TabSize { get; set; }

		bool WordWrap { get; set; }

		bool LineNumbers { get; set; }

		IWordStyleConfiguration KeywordStyle { get; }

		IWordStyleConfiguration FunctionStyle { get; }

		IWordStyleConfiguration DataTypeStyle { get; }

		IWordStyleConfiguration OperatorStyle { get; }

		IStyleConfiguration ConstantStyle { get; }

		IStyleConfiguration VariableStyle { get; }

		ICommentStyleConfiguration CommentStyle { get; }
	}
}


namespace SqlExport.Editor
{
    using SqlExport.Common.Editor;

    public interface ICommentStyleConfiguration : IStyleConfiguration
	{
		CommentSyntax CommentSyntax { get; set; }
	}
}

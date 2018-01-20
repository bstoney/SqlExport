using System.Drawing;

namespace SqlExport.Editor
{
    using SqlExport.Common.Editor;

    public interface IStyleConfiguration
	{
		Font Font { get; set; }

		Color Colour { get; set; }

		Color BackColour { get; set; }

		Capitalisation Capitalisation { get; set; }
	}
}

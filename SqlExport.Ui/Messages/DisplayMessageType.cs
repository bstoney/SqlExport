using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages
{
	/// <summary>
	/// The available types of messages.
	/// </summary>
	public enum DisplayMessageType
	{
		None = -1,
		Error = 0,
		Warning = 1,
		Information = 2,
		Success = 4
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;

namespace SqlExport.Messages
{
	public class GetDataResultMessage
	{
		public GetDataResultMessage( Action<DataResult> callback )
		{
			GetDataResultCallback = callback;
		}

		public Action<DataResult> GetDataResultCallback { get; set; }
	}
}

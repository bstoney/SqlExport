using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;

namespace SqlExport.Messages
{
    using SqlExport.Common.Data;

    public class GetDataResultMessage
    {
        public GetDataResultMessage(Action<IDataResult> callback)
        {
            GetDataResultCallback = callback;
        }

        public Action<IDataResult> GetDataResultCallback { get; set; }
    }
}

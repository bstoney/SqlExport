using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Data;

namespace SqlExport.Messages
{
    using SqlExport.Common.Data;

    public class SetDataResultMessage
    {
        public IDataResult Data { get; set; }
    }
}

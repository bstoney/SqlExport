using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages
{
    using SqlExport.Common;

    public class GetSelectedDatabase
    {
        public DatabaseDetails SelectedDatabase { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages
{
    using SqlExport.Common;

    public class AddConnectionMessage
    {
        public AddConnectionMessage(DatabaseDetails databaseDetails)
        {
            this.DatabaseDetails = databaseDetails;
        }

        public DatabaseDetails DatabaseDetails { get; set; }
    }
}

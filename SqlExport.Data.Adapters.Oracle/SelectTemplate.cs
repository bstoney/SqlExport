using System;
using System.Collections.Generic;
using System.Text;

namespace SqlExport.Data.Adapters.Oracle
{
    using SqlExport.Common.Data;

    internal class SelectTemplate : IStatementTemplate
    {
        #region IStatementTemplate Members

        public string Name
        {
            get { return "Select"; }
        }

        public string Statement
        {
            get
            {
                return @"--:rows,Number,1000
			
SELECT * 
FROM <Table>
WHERE ROWNUM <= :rows";
            }
        }

        #endregion
    }
}

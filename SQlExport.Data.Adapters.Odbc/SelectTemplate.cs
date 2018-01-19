using System;
using System.Collections.Generic;
using System.Text;

namespace SqlExport.Data.Adapters.Odbc
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
            get { return "SELECT TOP 1000 * \r\nFROM \r\n"; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SqlExport.Data.Adapters.MsSql
{
    using SqlExport.Common.Data;

    internal class TemplateCreateView : IStatementTemplate
    {
        #region IStatementTemplate Members

        public string Name
        {
            get { return "Create View"; }
        }

        public string Statement
        {
            get
            {
                return @"
USE <DatabaseName>
GO

/****** Create Object:  View [dbo].[<ViewName>]    Script Date: " + DateTime.Now + @" ******/
IF EXISTS( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[<ViewName>]') AND OBJECTPROPERTY(id, N'IsUserView') = 1 )
	DROP VIEW [dbo].[<ViewName>]
GO

CREATE VIEW <ViewName>
AS

SELECT TOP 1000 * 
FROM [<TableName>]

GO
";
            }
        }

        #endregion
    }
}

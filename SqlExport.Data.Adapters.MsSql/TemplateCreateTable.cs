using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.MsSql
{
    using SqlExport.Common.Data;

    internal class TemplateCreateTable : IStatementTemplate
    {

        #region IStatementTemplate Members

        public string Name
        {
            get { return "Create Table"; }
        }

        public string Statement
        {
            get
            {
                return @"
USE <DatabaseName>
GO

/****** Create Object:  Table [dbo].[<TableName>]    Script Date: " + DateTime.Now + @" ******/
IF EXISTS( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[<TableName>]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1 )
	DROP TABLE [dbo].[<TableName>]
GO

CREATE TABLE [<TableName>] (
	[<TableName>ID] [int] IDENTITY (1, 1) NOT NULL ,
	[<StringColumnName>] VARCHAR(50) NOT NULL,
	[<DoubleColumnName>] DOUBLE NOT NULL
	CONSTRAINT [PK_<TableName>] PRIMARY KEY CLUSTERED 
	(
		[<TableName>ID]
	) ON [PRIMARY] 
) ON [PRIMARY]

GO
";
            }
        }

        #endregion
    }
}

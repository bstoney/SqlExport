using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace SqlExport.Data.Adapters.Linq
{
	public interface IGeneratedScript
	{
		void InitDataContext( IDbConnection connection, DbTransaction transaction, int commandTimeout );
		void Execute();
	}
}

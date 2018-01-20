using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace SqlExport.Data.Adapters.MsSql
{
    using SqlExport.Common.Data;

    internal static class DataColumnExtensions
    {
        public static int GetSize(this Column column, IDataResult data)
        {
            switch (column.UnderlyingType.FullName)
            {
                case "System.String":
                    int size = data.AsEnumerable().OfType<object>().Select(r => ((string)data.FetchValue(r, column.Name)).Length).Max();
                    int adjustedSize = size;

                    if (size < 50)
                    {
                        adjustedSize = 50;
                    }
                    else if (size < 250)
                    {
                        adjustedSize = 250;
                    }

                    return adjustedSize;
                case "System.DateTime":
                    return 8;
                default:
                    return Marshal.SizeOf(column.UnderlyingType);
            }
        }
    }
}

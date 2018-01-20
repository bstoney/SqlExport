namespace SqlExport.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    using SqlExport.Common.Data;

    /// <summary>
    /// Defines the IDataResultExtensions class.
    /// </summary>
    public static class DataResultHelper
    {
        /// <summary>
        /// Converts the data table to a data result..
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>A data result.</returns>
        public static IDataResult FromDataTable(DataTable table)
        {
            return new DataTableDataResult(table);
        }

        // TODO There are currently UI issues with handling IDataReader.
        ////public static DataResult FromDataReader( string name, System.Data.IDataReader reader )
        ////{
        ////    return new DataReaderDataResult( name, reader );
        ////}
    }
}

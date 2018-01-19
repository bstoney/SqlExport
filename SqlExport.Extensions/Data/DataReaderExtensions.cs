using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SqlExport.Data
{
	public static class DataReaderExtensions
	{
		public static IEnumerable<T> Select<T>( this IDataReader reader, Func<IDataReader, T> selector )
		{
			while( reader.Read() )
			{
				yield return selector( reader );
			}
		}

		public static IEnumerable<DataColumn> GetColumns( this IDataReader reader )
		{
			for( int i = 0; i < reader.FieldCount; i++ )
			{
				yield return new DataColumn( reader.GetName( i ), reader.GetFieldType( i ) );
			}
		}

		public static IEnumerable<IDataReader> Take( this IDataReader reader, int items )
		{
			while( reader.Read() && items > 0 )
			{
				yield return reader;
				items--;
			}
		}
	}
}

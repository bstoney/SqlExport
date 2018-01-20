using System;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// A sheet within a workbook (a collection of rows).
	/// </summary>
	public class Worksheet
	{
		/// <summary>
		/// Initializes a new instance of the Worksheet class.
		/// </summary>
		/// <param name="name">Title of the worksheet.</param>
		internal Worksheet( string name, int index )
		{
			Rows = new RowCollection( this );
			Columns = new ColumnCollection();
			Index = index;
			Name = name;
		}

		/// <summary>
		/// Gets or sets the title of this worksheet.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the index of this work sheet.
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Gets a Row at the y coordinate.
		/// </summary>
		public Row this[int row]
		{
			get { return Rows[row]; }
		}

		/// <summary>
		/// Gets a collection of all the rows in this worksheet.
		/// </summary>
		public RowCollection Rows { get; private set; }

		/// <summary>
		/// Gets a collection of all the columns in this worksheet.
		/// </summary>
		public ColumnCollection Columns { get; private set; }

		/// <summary>
		/// Gets or sets the zoom percentage of this worksheet.
		/// </summary>
		public int? Zoom { get; set; }

		/// <summary>
		/// Gets or sets the frozen x coordinate of this worksheet.
		/// </summary>
		public int? FrozenX { get; set; }

		/// <summary>
		/// Gets or sets the frozen y coordinate of this worksheet.
		/// </summary>
		public int? FrozenY { get; set; }

		#region Cell Functions

		/// <summary>
		/// Gets the cell at the given location.
		/// </summary>
		public Cell Cell( int column, int row )
		{
			return this[row][column];
		}

		private void GetArray( object[] aryValues, int column, int topRow, int bottomRow, object defaultValue )
		{
			for( int i = topRow; i <= bottomRow; i++ )
			{
				try
				{
					aryValues[i - topRow] = Convert.ToDouble( this[column][i].Value );
				}
				catch( Exception )
				{
					aryValues[i - topRow] = defaultValue;
				}
			}
		}

		/// <summary>
		/// Find the median value of set of cell values.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <param name="topRow">The index of the start row.</param>
		/// <param name="bottomRow">the index of the end row.</param>
		/// <returns>Median value of the cell values.</returns>
		public double Median( int column, int topRow, int bottomRow )
		{
			object[] aryValues = new object[bottomRow - topRow + 1];
			int offSet;
			GetArray( aryValues, column, topRow, bottomRow, (double)0 );
			Array.Sort( aryValues );
			if( aryValues.Length % 2 == 1 )
			{
				offSet = ((aryValues.Length + 1) / 2) - 1;
				return Convert.ToDouble( aryValues[offSet] );
			}
			else
			{
				offSet = (aryValues.Length / 2) - 1;
				return (Convert.ToDouble( aryValues[offSet] ) + Convert.ToDouble( aryValues[offSet + 1] )) / 2;
			}
		}

		/// <summary>
		/// Find the mean value of set of cell values.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <param name="topRow">The index of the start row.</param>
		/// <param name="bottomRow">the index of the end row.</param>
		/// <returns>Mean value of the cell values.</returns>
		public double Mean( int column, int topRow, int bottomRow )
		{
			double totalValue = 0;
			for( int i = topRow; i <= bottomRow; i++ )
			{
				try
				{
					totalValue += Convert.ToDouble( this[column][i].Value );
				}
				catch( Exception )
				{
				}
			}

			return totalValue / (bottomRow - topRow + 1);
		}

		/// <summary>
		/// Find the mimimum value of set of cell values.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <param name="topRow">The index of the start row.</param>
		/// <param name="bottomRow">the index of the end row.</param>
		/// <returns>Minimum value of the cell values.</returns>
		public double Minimum( int column, int topRow, int bottomRow )
		{
			double minValue = Convert.ToDouble( this[column][topRow].Value );
			for( int i = topRow + 1; i <= bottomRow; i++ )
			{
				try
				{
					if( minValue > Convert.ToDouble( this[column][i].Value ) )
					{
						minValue = Convert.ToDouble( this[column][i].Value );
					}
				}
				catch( Exception )
				{
				}
			}

			return minValue;
		}

		/// <summary>
		/// Find the maximum value of set of cell values.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <param name="topRow">The index of the start row.</param>
		/// <param name="bottomRow">the index of the end row.</param>
		/// <returns>Maximum value of the cell values.</returns>
		public object Maximum( int column, int topRow, int bottomRow )
		{
			double maxValue = Convert.ToDouble( this[column][topRow].Value );
			for( int i = topRow + 1; i <= bottomRow; i++ )
			{
				try
				{
					if( maxValue < Convert.ToDouble( this[column][i].Value ) )
					{
						maxValue = Convert.ToDouble( this[column][i].Value );
					}
				}
				catch( Exception )
				{
				}
			}

			return maxValue;
		}

		/// <summary>
		/// Find the standard deviation value of set of cell values.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <param name="topRow">The index of the start row.</param>
		/// <param name="bottomRow">the index of the end row.</param>
		/// <returns>Standard deviation of the cell values.</returns>
		public double StDev( int column, int topRow, int bottomRow )
		{
			double mean = Mean( column, topRow, bottomRow );
			int count = bottomRow - topRow + 1;
			double sumDiffSQR = 0;
			for( int i = topRow; i <= bottomRow; i++ )
			{
				try
				{
					sumDiffSQR += (Convert.ToDouble( this[column][i].Value ) - mean) * (Convert.ToDouble( this[column][i].Value ) - mean);
				}
				catch( Exception )
				{
				}
			}

			return Math.Sqrt( sumDiffSQR / count );
		}

		/// <summary>
		/// Find the percentage rank of Value within a set of cell values.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <param name="topRow">The index of the start row.</param>
		/// <param name="bottomRow">the index of the end row.</param>
		/// <param name="value">The value to seach for.</param>
		/// <returns>Percentage rank of Value within the cell values.</returns>
		public double PercentRank( int column, int topRow, int bottomRow, double value )
		{
			object[] values = new object[bottomRow - topRow + 1];
			GetArray( values, column, topRow, bottomRow, 0 );
			Array.Sort( values );

			double rank;
			if( value <= Convert.ToDouble( values[0] ) )
			{
				rank = 0;
			}
			else if( value >= Convert.ToDouble( values[values.Length - 1] ) )
			{
				rank = 1;
			}
			else
			{
				int i = 1;
				while( value > Convert.ToDouble( values[i] ) )
				{
					i += 1;
				}

				rank = i / (values.Length - 1);
				if( value != Convert.ToDouble( values[i] ) )
				{
					// The exact value wasn't found in the list, so the rank must be interpolated.
					double diff = Convert.ToDouble( values[i] ) - value;
					double range = Convert.ToDouble( values[i] ) - Convert.ToDouble( values[i - 1] );
					double maxRank = 1 / (values.Length - 1);
					rank -= diff / range * maxRank;
				}
			}

			return rank;
		}

		/// <summary>
		/// Find the percentile value within a set of cell values.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <param name="topRow">The index of the start row.</param>
		/// <param name="bottomRow">the index of the end row.</param>
		/// <param name="percentile">Percentile to search for.</param>
		/// <returns>Percentile value within the cell values.</returns>
		public double Percentile( int column, int topRow, int bottomRow, double percentile )
		{
			if( percentile < 0 || percentile > 1 )
			{
				throw new ArgumentOutOfRangeException( "percentile" );
			}

			object[] aryValues = new object[bottomRow - topRow + 1];
			int i;
			double interValue;
			GetArray( aryValues, column, topRow, bottomRow, 0 );
			Array.Sort( aryValues );
			i = (int)Math.Floor( percentile * (aryValues.Length - 1) );
			interValue = (percentile * (aryValues.Length - 1)) - i;
			if( interValue == 0 )
			{
				return Convert.ToDouble( aryValues[i] );
			}
			else
			{
				return Convert.ToDouble( aryValues[i] ) + ((Convert.ToDouble( aryValues[i + 1] ) - Convert.ToDouble( aryValues[i] )) * interValue);
			}
		}

		#endregion
	}
}

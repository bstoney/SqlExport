using System;
using System.Collections;
using System.Collections.Generic;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// Represents a collection of rows in a work sheet.
	/// </summary>
	public class RowCollection : ICollection<Row>
	{
		private Worksheet _worksheet;

		/// <summary>
		/// Initializes a new instance of the RowCollection class.
		/// </summary>
		internal RowCollection( Worksheet worksheet )
		{
			_worksheet = worksheet;
			InnerList = new List<Row>();
		}

		/// <summary>
		/// The list which contains the items.
		/// </summary>
		private List<Row> InnerList { get; set; }

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// If index is less than or equal to the collection count, the collection will be extended.
		/// </summary>
		public Row this[int index]
		{
			get
			{
				return GetItem( index );
			}

			set
			{
				if( value == null )
				{
					throw new ArgumentNullException();
				}

				EnsureCapacity( index );
				InnerList[index] = value;
			}
		}

		/// <summary>
		/// Gets the item at index, ensuring it exists.
		/// </summary>
		private Row GetItem( int index )
		{
			EnsureCapacity( index );

			Row item = InnerList[index] as Row;
			if( item == null )
			{
				InnerList[index] = item = new Row( _worksheet, index );
			}

			return item;
		}

		/// <summary>
		/// Ensures the collection is large enough for the index.
		/// </summary>
		private void EnsureCapacity( int index )
		{
			if( InnerList.Capacity < index )
			{
				InnerList.Capacity = index + 1;
			}

			// Adjust array size for new row
			for( int i = Count; i <= index; i++ )
			{
				InnerList.Add( null );
			}
		}

		#region ICollection<Row> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		void ICollection<Row>.Add( Row item )
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		public void Clear()
		{
			InnerList.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		public bool Contains( Row item )
		{
			return InnerList.Contains( item );
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		public void CopyTo( Row[] array, int arrayIndex )
		{
			InnerList.CopyTo( array, arrayIndex );
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		public int Count
		{
			get { return InnerList.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		bool ICollection<Row>.Remove( Row item )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<Row> Members

		/// <summary>
		/// Returns an enumerator that iterates through the instance.
		/// </summary>
		public IEnumerator<Row> GetEnumerator()
		{
			return InnerList.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through the instance.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return InnerList.GetEnumerator();
		}

		#endregion
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SqlExport.Export.Adapters.Common
{
	/// <summary>
	/// A worksheet row (a collection of cells).
	/// </summary>
	public class Row : ICollection<Cell>
	{
		private Worksheet _parent; 

		/// <summary>
		/// Initializes a new instance of the Row class.
		/// </summary>
		internal Row( Worksheet parent, int rowIndex )
		{
			_parent = parent;
			Index = rowIndex;
			InnerList = new List<Cell>();
		}

		/// <summary>
		/// The list which contains the items.
		/// </summary>
		private List<Cell> InnerList { get; set; }

		/// <summary>
		/// Gets the index of the row.
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Gets or sets the row name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the disply hight for the row.
		/// </summary>
		public int? Height { get; set; }

		/// <summary>
		/// Gets a cell at the index.
		/// </summary>
		public Cell this[int column]
		{
			get { return GetItem( column ); }
		}

		/// <summary>
		/// Gets or sets an array of raw cell values.
		/// </summary>
		public object[] ValueArray
		{
			get
			{
				return InnerList.OfType<Cell>().Select( c => c.Value ).ToArray();
			}

			set
			{
				value.Select( ( v, i ) => new { Index = i, Value = v } ).ToList()
					.ForEach( v => this[v.Index].Value = v.Value );
			}
		}

		/// <summary>
		/// Gets the item at index, ensuring it exists.
		/// </summary>
		private Cell GetItem( int index )
		{
			EnsureCapacity( index );

			Cell item = InnerList[index] as Cell;
			if( item == null )
			{
				InnerList[index] = item = new Cell( _parent.Columns[index], this );
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

		#region ICollection<Cell> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		public void Add( Cell item )
		{
			InnerList.Add( item );
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
		public bool Contains( Cell item )
		{
			return InnerList.Contains( item );
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		public void CopyTo( Cell[] array, int arrayIndex )
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
		bool ICollection<Cell>.Remove( Cell item )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<Cell> Members

		/// <summary>
		/// Returns an enumerator that iterates through the instance.
		/// </summary>
		public IEnumerator<Cell> GetEnumerator()
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

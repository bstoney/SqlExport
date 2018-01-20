using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Data.Common;
using System.Collections;

namespace SqlExport.Data.Adapters.Text.Query
{
	internal class Results : DbDataReader, IDataReader, ICustomTypeDescriptor, IEnumerable
	{
		private Query _query;
		private IEnumerator<object[]> _enumerator;
		private int _rowNumber;
		private object[] _current;
		private Dictionary<string, int> _columnLookUp;
		private DataTable _schemaTable;
		private bool _closed;

		public Results( Query query )
		{
			this._query = query;
			_enumerator = this._query.Lines.GetEnumerator();
			_columnLookUp = this._query.DestinationColumns.Select( ( n, i ) => new { N = n, I = i } ).ToDictionary( a => a.N, a => a.I );
			_rowNumber = 0;
			_closed = false;
		}

		public override IEnumerator GetEnumerator()
		{
			return new DbEnumerator( this );
		}

		public override bool HasRows
		{
			get { return true; }
		}

		#region IDataReader Members

		public override void Close()
		{
			if( !_closed )
			{
				_closed = true;
				_enumerator.Dispose();
				_enumerator = null;
			}
		}

		public override int Depth
		{
			get { return 0; }
		}

		public override DataTable GetSchemaTable()
		{
			if( _schemaTable == null )
			{
				_schemaTable = new DataTable();
				_schemaTable.Columns.Add( "ColumnName", typeof( string ) );
				_schemaTable.Columns.Add( "DataType", typeof( Type ) );

				foreach( var item in this._query.DestinationColumns )
				{
					DataRow row = _schemaTable.NewRow();
					row["ColumnName"] = item;
					row["DataType"] = SchemaAdapter.IsRowNumberColumnName( item ) ? typeof( int ) : typeof( string );
				}
			}

			return _schemaTable;
		}

		public override bool IsClosed
		{
			get { return _closed; }
		}

		public override bool NextResult()
		{
			throw new NotImplementedException();
		}

		public override bool Read()
		{
			if( !_closed )
			{
				while( _enumerator.MoveNext() )
				{
					var line = _enumerator.Current;
					_rowNumber++;
					var row = new object[this._query.DestinationColumns.Length];
					for( int i = 0; i < this._query.DestinationColumns.Length; i++ )
					{
					    row[i] = line[i]; // this._query.GetValue(this._query.DestinationColumns[i], line, _rowNumber) ?? DBNull.Value;
					}

					_current = row;
					return true;
				}

				_closed = true;
			}

			return false;
		}

		public override int RecordsAffected
		{
			get { return _rowNumber; }
		}

		#endregion

		#region IDataRecord Members

		public override int FieldCount
		{
			get { return this._query.DestinationColumns.Length; }
		}

		public override bool GetBoolean( int i )
		{
			throw new NotImplementedException();
		}

		public override byte GetByte( int i )
		{
			throw new NotImplementedException();
		}

		public override long GetBytes( int i, long fieldOffset, byte[] buffer, int bufferoffset, int length )
		{
			throw new NotImplementedException();
		}

		public override char GetChar( int i )
		{
			throw new NotImplementedException();
		}

		public override long GetChars( int i, long fieldoffset, char[] buffer, int bufferoffset, int length )
		{
			throw new NotImplementedException();
		}

		public override string GetDataTypeName( int i )
		{
			return GetFieldType( i ).Name;
		}

		public override DateTime GetDateTime( int i )
		{
			throw new NotImplementedException();
		}

		public override decimal GetDecimal( int i )
		{
			throw new NotImplementedException();
		}

		public override double GetDouble( int i )
		{
			throw new NotImplementedException();
		}

		public override Type GetFieldType( int i )
		{
			if( SchemaAdapter.IsRowNumberColumnName( GetName( i ) ) )
			{
				return typeof( int );
			}
			else
			{
				return typeof( string );
			}
		}

		public override float GetFloat( int i )
		{
			throw new NotImplementedException();
		}

		public override Guid GetGuid( int i )
		{
			throw new NotImplementedException();
		}

		public override short GetInt16( int i )
		{
			throw new NotImplementedException();
		}

		public override int GetInt32( int i )
		{
			throw new NotImplementedException();
		}

		public override long GetInt64( int i )
		{
			throw new NotImplementedException();
		}

		public override string GetName( int i )
		{
			return this._query.DestinationColumns[i];
		}

		public override int GetOrdinal( string name )
		{
			if( _columnLookUp.ContainsKey( name ) )
			{
				return _columnLookUp[name];
			}

			throw new IndexOutOfRangeException();
		}

		public override string GetString( int i )
		{
			throw new NotImplementedException();
		}

		public override object GetValue( int i )
		{
			return _current[i];
		}

		/// <summary>
		/// Gets all the attribute fields in the collection for the current record.
		/// </summary>
		public override int GetValues( object[] values )
		{
			int length = Math.Min( _current.Length, values.Length );
			Array.Copy( _current, values, length );
			return length;
		}

		/// <summary>
		/// Return whether the specified field is set to null.
		/// </summary>
		public override bool IsDBNull( int i )
		{
			return _current[i] is DBNull;
		}

		public override object this[string name]
		{
			get { return _current[GetOrdinal( name )]; }
		}

		public override object this[int i]
		{
			get { return _current[i]; }
		}

		#endregion

		#region ICustomTypeDescriptor Members

		public AttributeCollection GetAttributes()
		{
			throw new NotImplementedException();
		}

		public string GetClassName()
		{
			throw new NotImplementedException();
		}

		public string GetComponentName()
		{
			throw new NotImplementedException();
		}

		public TypeConverter GetConverter()
		{
			throw new NotImplementedException();
		}

		public EventDescriptor GetDefaultEvent()
		{
			throw new NotImplementedException();
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			throw new NotImplementedException();
		}

		public object GetEditor( Type editorBaseType )
		{
			throw new NotImplementedException();
		}

		public EventDescriptorCollection GetEvents( Attribute[] attributes )
		{
			throw new NotImplementedException();
		}

		public EventDescriptorCollection GetEvents()
		{
			throw new NotImplementedException();
		}

		public PropertyDescriptorCollection GetProperties( Attribute[] attributes )
		{
			return GetProperties();
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return new PropertyDescriptorCollection( this._query.DestinationColumns.Select( n => new CustomPropertyDescriptor( n, GetFieldType( GetOrdinal( n ) ) ) ).ToArray() );
		}

		public object GetPropertyOwner( PropertyDescriptor pd )
		{
			throw new NotImplementedException();
		}

		#endregion

		private class CustomPropertyDescriptor : PropertyDescriptor
		{
			private Type _propertyType;

			public CustomPropertyDescriptor( string name, Type type )
				: base( name, new Attribute[] { } )
			{
				_propertyType = type;
			}

			public override bool CanResetValue( object component )
			{
				return false;
			}

			public override Type ComponentType
			{
				get { return typeof( Results ); }
			}

			public override object GetValue( object component )
			{
				Results r = component as Results;
				if( r != null )
				{
					return r[Name];
				}

				return null;
			}

			public override bool IsReadOnly
			{
				get { return true; }
			}

			public override Type PropertyType
			{
				get { return _propertyType; }
			}

			public override void ResetValue( object component )
			{
				throw new NotImplementedException();
			}

			public override void SetValue( object component, object value )
			{
				throw new NotImplementedException();
			}

			public override bool ShouldSerializeValue( object component )
			{
				throw new NotImplementedException();
			}
		}
	}
}

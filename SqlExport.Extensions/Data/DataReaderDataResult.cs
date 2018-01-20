using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.ComponentModel;

namespace SqlExport.Data
{
    using SqlExport.Common.Data;

    public class DataReaderDataResult : IDataResult, IEnumerable
    {
        private DataTable _results;
        private IDataReader _reader;
        private bool _hasInitialised;
        private bool _hasLoaded;

        public DataReaderDataResult(string name, IDataReader reader)
        {
            this.Name = name;
            _results = new DataTable(name);
            _reader = reader;
        }

        public string Name { get; private set; }

        public IEnumerable AsEnumerable()
        {
            return this;
        }

        public int FetchCount()
        {
            if (_hasLoaded)
            {
                return _results.Rows.Count;
            }
            else
            {
                return AsEnumerable().OfType<object>().Count();
            }
        }

        public IEnumerable<ISchemaItem> FetchColumns()
        {
            lock (_results)
            {
                EnsureColumns();
            }

            return _results.Columns.OfType<DataColumn>()
                .Select(c => new Column(c.ColumnName, c.ColumnName, c.DataType.Name, c.DataType, c.AllowDBNull));
        }

        private void EnsureColumns()
        {
            if (!_hasInitialised)
            {
                _hasInitialised = true;

                foreach (var column in _reader.GetColumns())
                {
                    _results.Columns.Add(column);
                }
            }
        }

        public object FetchValue(object row, string column)
        {
            return ((DataRow)row)[column];
        }

        public object FetchValue(int columnIndex, int rowIndex)
        {
            return GetResultAt(rowIndex)[columnIndex];
        }

        private DataRow GetResultAt(int index)
        {
            lock (_results)
            {
                if (!_hasLoaded)
                {
                    EnsureColumns();

                    while (index >= _results.Rows.Count)
                    {
                        if (!_reader.Read())
                        {
                            _hasLoaded = true;
                            _reader.Dispose();
                            _reader = null;
                            break;
                        }
                        else
                        {
                            var row = _results.NewRow();
                            row.ItemArray = FetchColumns().Select(c => _reader[c.Name]).ToArray();
                            _results.Rows.Add(row);
                        }
                    }
                }
            }

            return index < _results.Rows.Count ? _results.Rows[index] : null;
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return new DataTableEnumerator(this);
        }

        #endregion

        private class DataTableEnumerator : IEnumerator
        {
            private int _currentIndex;
            private DataReaderDataResult _result;
            private DataRow _current;

            public DataTableEnumerator(DataReaderDataResult result)
            {
                _currentIndex = -1;
                _result = result;
            }

            #region IEnumerator Members

            public object Current
            {
                get { return _current; }
            }

            public bool MoveNext()
            {
                _currentIndex++;
                _current = _result.GetResultAt(_currentIndex);
                return _current != null;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }

            #endregion
        }
    }

    ////public class DataReaderDataResult : DataResult, IEnumerable
    ////{
    ////    private List<string> _columns;
    ////    private List<ResultType> _results;
    ////    private IDataReader _reader;
    ////    private bool _hasInitialised;
    ////    private bool _hasLoaded;

    ////    public DataReaderDataResult( string name, IDataReader reader ) :
    ////        base( name )
    ////    {
    ////        _results = new List<ResultType>();
    ////        _reader = reader;
    ////    }

    ////    public override IEnumerable AsEnumerable()
    ////    {
    ////        return this;
    ////    }

    ////    public override int FetchCount()
    ////    {
    ////        if( _hasLoaded )
    ////        {
    ////            return _results.Count;
    ////        }
    ////        else
    ////        {
    ////            return AsEnumerable().OfType<ResultType>().Count();
    ////        }
    ////    }

    ////    public override IEnumerable<string> FetchColumnNames()
    ////    {
    ////        lock( _results )
    ////        {
    ////            EnsureColumns();
    ////        }

    ////        return _columns;
    ////    }

    ////    private void EnsureColumns()
    ////    {
    ////        if( !_hasInitialised )
    ////        {
    ////            _hasInitialised = true;
    ////            _columns = _reader.GetColumns().Select( c => c.ColumnName ).ToList();
    ////        }
    ////    }

    ////    public override object FetchValue( object row, string column )
    ////    {
    ////        return ((ResultType)row).GetValue( column );
    ////    }

    ////    private ResultType GetResultAt( int index )
    ////    {
    ////        lock( _results )
    ////        {
    ////            if( !_hasLoaded )
    ////            {
    ////                EnsureColumns();

    ////                while( index >= _results.Count )
    ////                {
    ////                    if( !_reader.Read() )
    ////                    {
    ////                        _hasLoaded = true;
    ////                        break;
    ////                    }
    ////                    else
    ////                    {
    ////                        _results.Add( new ResultType( _columns, _reader ) );
    ////                    }
    ////                }
    ////            }
    ////        }

    ////        return index < _results.Count ? _results[index] : null;
    ////    }

    ////    #region IEnumerable Members

    ////    public IEnumerator GetEnumerator()
    ////    {
    ////        return new ResultTypeEnumerator( this );
    ////    }

    ////    #endregion

    ////    private class ResultTypeEnumerator : IEnumerator
    ////    {
    ////        private int _currentIndex;
    ////        private DataReaderDataResult _result;
    ////        private ResultType _current;

    ////        public ResultTypeEnumerator( DataReaderDataResult result )
    ////        {
    ////            _currentIndex = -1;
    ////            _result = result;
    ////        }

    ////        #region IEnumerator Members

    ////        public object Current
    ////        {
    ////            get { return _current; }
    ////        }

    ////        public bool MoveNext()
    ////        {
    ////            _currentIndex++;
    ////            _current = _result.GetResultAt( _currentIndex );
    ////            return _current != null;
    ////        }

    ////        public void Reset()
    ////        {
    ////            _currentIndex = -1;
    ////        }

    ////        #endregion
    ////    }

    ////    private class ResultType : CustomTypeDescriptor, ICustomTypeDescriptor
    ////    {
    ////        private PropertyDescriptorCollection _columns;
    ////        private Dictionary<string, object> _values;

    ////        public ResultType( IEnumerable<string> columns, IDataReader reader )
    ////        {
    ////            _columns = new PropertyDescriptorCollection( columns.Select( c =>
    ////                new ResultTypePropertyDescriptor( c, reader.GetFieldType( reader.GetOrdinal( c ) ) ) ).ToArray() );
    ////            _values = columns.ToDictionary( c => c, c => reader[c] );
    ////        }

    ////        public override PropertyDescriptorCollection GetProperties()
    ////        {
    ////            return _columns;
    ////        }

    ////        public override PropertyDescriptorCollection GetProperties( Attribute[] attributes )
    ////        {
    ////            return base.GetProperties( attributes );
    ////        }

    ////        public object GetValue( string column )
    ////        {
    ////            return _values[column];
    ////        }

    ////        private class ResultTypePropertyDescriptor : PropertyDescriptor
    ////        {
    ////            private Type _type;
    ////            public ResultTypePropertyDescriptor( string column, Type type )
    ////                : base( column, null )
    ////            {
    ////                _type = type;
    ////            }

    ////            public override bool CanResetValue( object component )
    ////            {
    ////                return false;
    ////            }

    ////            public override Type ComponentType
    ////            {
    ////                get { return typeof( ResultType ); }
    ////            }

    ////            public override object GetValue( object component )
    ////            {
    ////                var rt = component as ResultType;
    ////                if( rt != null )
    ////                {
    ////                    return rt.GetValue( Name );
    ////                }

    ////                return null;
    ////            }

    ////            public override bool IsReadOnly
    ////            {
    ////                get { return true; }
    ////            }

    ////            public override Type PropertyType
    ////            {
    ////                get { return _type; }
    ////            }

    ////            public override void ResetValue( object component )
    ////            {
    ////            }

    ////            public override void SetValue( object component, object value )
    ////            {
    ////            }

    ////            public override bool ShouldSerializeValue( object component )
    ////            {
    ////                return false;
    ////            }
    ////        }
    ////    }
    ////}
}

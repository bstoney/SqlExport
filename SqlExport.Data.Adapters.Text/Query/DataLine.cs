namespace SqlExport.Data.Adapters.Text.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DataLine
    {
        private readonly object[] _data;

        public DataLine(object[] data, int lineIndex)
        {
            _data = data;
            Index = lineIndex;
        }

        public int Index { get; private set; }

        public object this[int index]
        {
            get { return _data[index]; }
        }

        public int Length
        {
            get { return _data.Length; }
        }
    }
}

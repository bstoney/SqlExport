namespace SqlExport.Export.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    using SqlExport;
    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Common.Extensions;
    using SqlExport.Common.Options;
    using SqlExport.Data;

    /// <summary>
    /// Defines the CSV class.
    /// </summary>
    public class Csv : ExporterBase, IExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Csv"/> class.
        /// </summary>
        public Csv()
        {
            Configuration.SetOptionsOn(this);
        }

        /// <summary>
        /// Gets the name of the adapter.
        /// </summary>
        public override string Name
        {
            get { return "Csv"; }
        }

        /// <summary>
        /// Gets the dialog filename filter.
        /// </summary>
        public override string FileFilter
        {
            get { return "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"; }
        }

        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        [Option("Delimiter", DefaultValue = ",")]
        public string Delimiter { get; set; }

        /// <summary>
        /// Gets or sets the text encoding.
        /// </summary>
        [Option("TextEncoding", DisplayName = "Text Encoding")]
        public string TextEncoding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to export headers.
        /// </summary>
        [Option("ExportHeaders", DefaultValue = "True", DisplayName = "Include Field Names")]
        public bool ExportHeaders { get; set; }

        /// <summary>
        /// Exports the data to the supplied file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="filename">The filename.</param>
        public override void Export(IDataResult data, string filename)
        {
            Encoding encoding = EncodingInfoWrapper.GetEncoding(this.TextEncoding);
            using (StreamWriter writer = new StreamWriter(filename, false, encoding))
            {
                Export(data, writer);
            }
        }

        /// <summary>
        /// Exports the data to the supplied text writer.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="writer">The writer.</param>
        public override void Export(IDataResult data, TextWriter writer)
        {
            Status = "Exporting data as CSV";
            Maximum = data.FetchCount();
            int threadCount = Maximum < 100000 ? 1 : 2;
            RowWriter[] rws = new RowWriter[threadCount];
            int rowsPerThread = (int)Math.Ceiling(Maximum / (float)threadCount);

            string delimiter = this.Delimiter;
            Encoding encoding = EncodingInfoWrapper.GetEncoding(this.TextEncoding);

            var columns = data.FetchColumns().Select(c => c.Name).ToList();

            try
            {
                // Create the file and append the headers if necessary.
                if (this.ExportHeaders)
                {
                    for (int i = 0; i <= columns.Count - 1; i++)
                    {
                        if (i != 0)
                        {
                            writer.Write(delimiter);
                        }

                        writer.Write(columns[i]);
                    }

                    writer.WriteLine();
                }

                ManualResetEvent threadStart = new ManualResetEvent(false);
                ManualResetEvent[] threadsReady = new ManualResetEvent[threadCount];
                ManualResetEvent[] threadsComplete = new ManualResetEvent[threadCount];

                // Split work if we have lots of rows
                for (int i = 0; i < threadCount; i++)
                {
                    RowWriter rowWriter = new RowWriter(data, rowsPerThread * i, Math.Min(rowsPerThread * (i + 1),
                        data.FetchCount()) - 1, threadStart, delimiter);

                    if (i == 0)
                    {
                        rowWriter.Writer = writer;
                    }
                    else
                    {
                        rowWriter.Filename = Path.GetTempFileName();
                    }

                    threadsReady[i] = rowWriter.ThreadReady;
                    threadsComplete[i] = rowWriter.ThreadComplete;
                    Thread t = new Thread(new ParameterizedThreadStart(rowWriter.WriteRows));
                    t.Start(encoding);
                    rws[i] = rowWriter;
                }

                ManualResetEvent.WaitAll(threadsReady);
                threadStart.Set();
                ManualResetEvent.WaitAll(threadsComplete);

                // Append all additional sections to the main file.
                for (int i = 1; i < threadCount; i++)
                {
                    using (StreamReader reader = File.OpenText(rws[i].Filename))
                    {
                        char[] buf = new char[1024];
                        int chars = 0;
                        while ((chars = reader.Read(buf, 0, 1024)) > 0)
                        {
                            writer.Write(buf, 0, chars);
                        }
                    }
                }
            }
            finally
            {
                for (int i = 1; i < threadCount; i++)
                {
                    if (rws[i] != null && rws[i].Filename != null && File.Exists(rws[i].Filename))
                    {
                        File.Delete(rws[i].Filename);
                    }
                }
            }

            Status = "Export complete";
        }

        private class RowWriter
        {
            private int _startRow;
            private int _endRow;
            private IDataResult _data;
            private ManualResetEvent _threadStart;
            private ManualResetEvent _threadReady;
            private ManualResetEvent _threadComplete;
            private string _delimiter;

            public RowWriter(IDataResult data, int startRow, int endRow, ManualResetEvent threadStart, string delimiter)
            {
                _data = data;
                _startRow = startRow;
                _endRow = endRow;
                _threadStart = threadStart;
                _threadReady = new ManualResetEvent(false);
                _threadComplete = new ManualResetEvent(false);
                _delimiter = delimiter;
            }

            public ManualResetEvent ThreadReady
            {
                get { return _threadReady; }
            }

            public ManualResetEvent ThreadComplete
            {
                get { return _threadComplete; }
            }

            /// <summary>
            /// Gets or sets the stream to write to.
            /// </summary>
            public TextWriter Writer { get; set; }

            /// <summary>
            /// Gets or sets the stream to write to.
            /// </summary>
            public string Filename { get; set; }

            public void WriteRows(object state)
            {
                Encoding encoding = state as Encoding;

                _threadReady.Set();
                _threadStart.WaitOne();

                bool closeWriter = false;
                if (Writer == null)
                {
                    Writer = new StreamWriter(Filename, false, encoding);
                    closeWriter = true;
                }

                var columns = _data.FetchColumns().Select(c => c.Name).ToList();

                //int counter = 0;
                foreach (var row in _data.AsEnumerable().OfType<object>().Skip(_startRow).Take(_endRow - _startRow + 1))
                {
                    // TODO if( i % 10 == 0 )
                    //{
                    //    Value += counter;
                    //}
                    //else
                    //{
                    //    counter++;
                    //}
                    for (int j = 0; j <= columns.Count - 1; j++)
                    {
                        if (j != 0)
                        {
                            Writer.Write(_delimiter);
                        }

                        object value = _data.FetchValue(row, columns[j]);
                        if (!(value is DBNull))
                        {
                            if (value is string || value is char)
                            {
                                Writer.Write('"');
                                Writer.Write(EncodeString(value.ToString()));
                                Writer.Write('"');
                            }
                            else
                            {
                                Writer.Write(value);
                            }
                        }
                    }

                    Writer.WriteLine();
                }

                if (closeWriter)
                {
                    Writer.Flush();
                    Writer.Dispose();
                }

                _threadComplete.Set();
            }

            private static string EncodeString(string value)
            {
                return Regex.Replace(value, "[\\\"\r\n]", new MatchEvaluator(delegate(Match m)
                {
                    switch (m.Value[0])
                    {
                        case '\\':
                            return "\\\\";
                        case '"':
                            return "\"\"";
                        default:
                            return string.Concat(@"\x", ((int)m.Value[0]).ToString("X2"));
                    }
                }), RegexOptions.Compiled);
            }
        }
    }
}

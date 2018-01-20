using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SqlExport.Export.Adapters
{
	public class Confluence : ExporterBase
	{
		public override string Name
		{
			get { return "Confluence"; }
		}

		public override string FileFilter
		{
			get { return "Text File (*.txt)|*.txt|All Files (*.*)|*.*"; }
		}

		public override Control GetOptionPanel()
		{
			return null;
		}

		public void Export( DataTable data, string filename )
		{
			Status = "Exporting data as text for use in Confluence ";
			Maximum = data.Rows.Count;
			int threadCount = data.Rows.Count < 20000 ? 1 : 2;
			RowWriter[] rws = new RowWriter[threadCount];
			int rowsPerThread = (int)Math.Ceiling( data.Rows.Count / (float)threadCount );
			try
			{
				// Create the file and append the headers if necessary.
				using( StreamWriter writer = new StreamWriter( filename, false ) )
				{
					for( int i = 0; i <= data.Columns.Count - 1; i++ )
					{
						if( i == 0 )
						{
							writer.Write( "|| " );
						}
						else
						{
							writer.Write( " || " );
						}

						writer.Write( data.Columns[i].ColumnName );
					}

					writer.WriteLine( " ||" );
				}

				ManualResetEvent threadStart = new ManualResetEvent( false );
				ManualResetEvent[] threadsReady = new ManualResetEvent[threadCount];
				ManualResetEvent[] threadsComplete = new ManualResetEvent[threadCount];
				// Split work if we have lots of rows
				for( int i = 0; i < threadCount; i++ )
				{
					RowWriter rowWriter = new RowWriter( data, rowsPerThread * i, Math.Min( rowsPerThread * (i + 1),
						data.Rows.Count ) - 1, threadStart );
					if( i == 0 )
					{
						rowWriter.Filename = filename;
					}
					else
					{
						rowWriter.Filename = Path.GetTempFileName();
					}

					threadsReady[i] = rowWriter.ThreadReady;
					threadsComplete[i] = rowWriter.ThreadComplete;
					Thread t = new Thread( new ThreadStart( rowWriter.WriteRows ) );
					t.Start();
					rws[i] = rowWriter;
				}

				ManualResetEvent.WaitAll( threadsReady );
				threadStart.Set();
				ManualResetEvent.WaitAll( threadsComplete );

				// Append all additional sections to the main file.
				using( StreamWriter writer = new StreamWriter( filename, true ) )
				{
					for( int i = 1; i < threadCount; i++ )
					{
						using( StreamReader reader = File.OpenText( rws[i].Filename ) )
						{
							char[] buf = new char[1024];
							int chars = 0;
							while( (chars = reader.Read( buf, 0, 1024 )) > 0 )
							{
								writer.Write( buf, 0, chars );
							}
						}
					}
				}
			}
			finally
			{
				for( int i = 1; i < threadCount; i++ )
				{
					if( rws[i] != null && rws[i].Filename != null && File.Exists( rws[i].Filename ) )
					{
						File.Delete( rws[i].Filename );
					}
				}
			}

			Status = "Export complete";
		}

		public void Export( DataTable data, TextWriter writer )
		{
			throw new NotImplementedException();
		}

		private class RowWriter
		{
			private int _startRow;
			private int _endRow;
			private DataTable _data;
			private string _filename;
			private ManualResetEvent _threadStart;
			private ManualResetEvent _threadReady;
			private ManualResetEvent _threadComplete;

			public RowWriter( DataTable data, int startRow, int endRow, ManualResetEvent threadStart )
			{
				_data = data;
				_startRow = startRow;
				_endRow = endRow;
				_threadStart = threadStart;
				_threadReady = new ManualResetEvent( false );
				_threadComplete = new ManualResetEvent( false );
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
			/// Gets or sets the filename of the file to write to.
			/// </summary>
			public string Filename
			{
				get { return _filename; }
				set { _filename = value; }
			}


			public void WriteRows()
			{
				using( StreamWriter writer = new StreamWriter( _filename, true ) )
				{
					_threadReady.Set();
					_threadStart.WaitOne();

					//int counter = 0;
					for( int i = _startRow; i <= _endRow; i++ )
					{
						// TODO if( i % 10 == 0 )
						//{
						//    Value += counter;
						//}
						//else
						//{
						//    counter++;
						//}
						for( int j = 0; j <= _data.Columns.Count - 1; j++ )
						{
							if( j == 0 )
							{
								writer.Write( "| " );
							}
							else
							{
								writer.Write( " | " );
							}
							if( !(_data.Rows[i][j] is DBNull) )
							{
								if( _data.Rows[i][j] is string || _data.Rows[i][j] is char )
								{
									writer.Write( _data.Rows[i][j].ToString() );
								}
								else
								{
									writer.Write( _data.Rows[i][j] );
								}
							}
						}
						writer.WriteLine( " |" );
					}
				}
				_threadComplete.Set();
			}
		}
	}
}

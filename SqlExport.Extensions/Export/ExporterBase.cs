namespace SqlExport.Export
{
    using System;
    using System.Data;
    using System.IO;
    using System.Windows.Forms;

    using SqlExport.Common.Data;
    using SqlExport.Common.Extensions;
    using SqlExport.Data;

    /// <summary>
    /// The base class of a data exporter.
    /// </summary>
    public abstract class ExporterBase : IExtension
    {
        /// <summary>
        /// Occurs when the status has changed.
        /// </summary>
        public event EventHandler Update;

        /// <summary>
        /// Gets the name of the adapter.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the progress value when the export is 100% complete.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// Gets or sets the current progress value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets the dialog filename filter.
        /// </summary>
        public abstract string FileFilter { get; }

        /// <summary>
        /// Exports the data to the supplied file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="filename">The filename.</param>
        public virtual void Export(IDataResult data, string filename)
        {
            using (var writer = File.CreateText(filename))
            {
                this.Export(data, writer);
            }
        }

        /// <summary>
        /// Exports the data to the supplied text writer.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="writer">The writer.</param>
        public abstract void Export(IDataResult data, TextWriter writer);

        /// <summary>
        /// Invokes the Update event.
        /// </summary>
        protected virtual void OnUpdateProgress()
        {
            if (this.Update != null)
            {
                this.Update(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fills the data table.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>A data table.</returns>
        private DataTable FillDataTable(IDataReader dataReader)
        {
            DataTable dataTable = new DataTable();

            // Insert data reader schema into data table(dtCols)
            DataTable cols = dataReader.GetSchemaTable();
            if (cols != null)
            {
                int intNumCols = cols.Rows.Count - 1;

                // Loop thru dtCols, inserting columns into dataTable
                for (int i = 0; i < intNumCols; i++)
                {
                    dataTable.Columns.Add(cols.Rows[i]["ColumnName"] as string, (Type)cols.Rows[i]["DataType"]);
                }

                // Iterate thru data reader, adding rows to data table
                while (dataReader.Read())
                {
                    DataRow drow = dataTable.NewRow();

                    // Iterate thru columns data table
                    for (int i = 0; i < intNumCols; i++)
                    {
                        drow[i] = dataReader[(string)cols.Rows[i]["ColumnName"]];
                    }

                    dataTable.Rows.Add(drow);
                }

                dataReader.Close();
                cols.Rows.Clear(); 
            }

            return dataTable;
        }
    }
}

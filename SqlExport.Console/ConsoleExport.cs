namespace SqlExport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Data;
    using SqlExport.Export;

    /// <summary>
    /// Defines the ConsoleExport class.
    /// </summary>
    internal class ConsoleExport
    {
        /// <summary>
        /// The args
        /// </summary>
        private CommandLineArguments args;

        /// <summary>
        /// Executes a query on the console.
        /// </summary>
        public void ExecuteQuery(CommandLineArguments cla)
        {
            this.args = cla;
            bool optionsOk = true;

            ////Debugger.Break();

            // Load database settings.
            DatabaseDetails database = null;
            if (!string.IsNullOrEmpty(this.args.ConnectionName))
            {
                database = null; // TODO Configuration.Current.GetDatabase(this.args.ConnectionName);
                if (database == null)
                {
                    Console.Error.WriteLine("Undefined database name " + this.args.ConnectionName + ".");
                    optionsOk = false;
                }
            }
            else if (!string.IsNullOrEmpty(this.args.ConnectionType) && !string.IsNullOrEmpty(this.args.ConnectionString))
            {
                database = new DatabaseDetails();
                database.Name = "Console";
                database.Type = this.args.ConnectionType;
                database.ConnectionString = this.args.ConnectionString;
            }
            else
            {
                CommandLineArguments.ShowHelp(false, "Database details missing.");
                optionsOk = false;
            }

            if (database != null && (string.IsNullOrEmpty(database.Type) || !ConnectionAdapterHelper.IsConnectionAdapterRegistered(database.Type)))
            {
                Console.Error.WriteLine("Unrecognised database type " + database.Type + ".");
                optionsOk = false;
            }

            var connectionContext = database.CreateConnectionContext();

            if (!this.args.InputScript)
            {
                if (this.args.Files.Count() < 1)
                {
                    CommandLineArguments.ShowHelp(false, "No script file was specified.");
                    optionsOk = false;
                }
                else
                {
                    // Check files exist.
                    foreach (var file in this.args.Files)
                    {
                        if (!File.Exists(file))
                        {
                            Console.Error.WriteLine("Unable to find file " + file + ".");
                            optionsOk = false;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.args.OutputType) && !ExportAdapterHelper.IsExportAdapterRegistered(this.args.OutputType))
            {
                Console.Error.WriteLine("Unrecognised output type " + this.args.OutputType + ".");
                optionsOk = false;
            }

            if (optionsOk)
            {
                Controller c = new Controller();
                c.Message += new ConnectionMessageHandler(OnMessage);
                connectionContext.Message += new ConnectionMessageHandler(OnMessage);

                try
                {
                    if (!string.IsNullOrEmpty(this.args.OutputFile))
                    {
                        // Create a new new if necessary.
                        using (StreamWriter sw = File.CreateText(this.args.OutputFile))
                        {
                        }
                    }

                    Action<TextReader> run = reader =>
                    {
                        c.Run(connectionContext, reader);

                        if (string.IsNullOrEmpty(this.args.OutputType))
                        {
                            ExportResults(c.Data, new ConsoleExporter());
                        }
                        else
                        {
                            ExportResults(c.Data, ExportAdapterHelper.GetExportAdapter(this.args.OutputType));
                        }
                    };

                    if (this.args.InputScript)
                    {
                        run(Console.In);
                    }
                    else
                    {
                        foreach (var file in this.args.Files)
                        {
                            using (TextReader reader = new StreamReader(File.Open(file, FileMode.Open), Encoding.Default))
                            {
                                run(reader);
                            }
                        }
                    }

                }
                catch (Exception exp)
                {
                    Console.Error.WriteLine("Error exporting data: " + exp.Message);
                }
            }
        }

        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="lineNumber">The line number.</param>
        private void OnMessage(MessageType type, string message, int? lineNumber)
        {
            Console.Error.WriteLine("{0}: {1}", type, message);
        }

        /// <summary>
        /// Exports the results.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="exporter">The exporter.</param>
        private void ExportResults(IEnumerable<IDataResult> data, ExporterBase exporter)
        {
            // TODO
            ////var properties = new List<PropertyItem>();
            ////exporter.LoadPropertiesAndValues(properties);

            foreach (var dt in data)
            {
                if (!string.IsNullOrEmpty(this.args.OutputFile))
                {
                    using (StreamWriter sw = File.AppendText(this.args.OutputFile))
                    {
                        exporter.Export(dt, sw);
                    }
                }
                else
                {
                    Console.OutputEncoding = Encoding.Default;
                    exporter.Export(dt, Console.Out);
                }
            }
        }

        /// <summary>
        /// Defines the ConsoleExporter class.
        /// </summary>
        private class ConsoleExporter : ExporterBase
        {
            /// <summary>
            /// Gets the name of the adapter.
            /// </summary>
            public override string Name
            {
                get { throw new NotImplementedException(); }
            }

            /// <summary>
            /// Gets the dialog filename filter.
            /// </summary>
            public override string FileFilter
            {
                get { throw new NotImplementedException(); }
            }

            /// <summary>
            /// Exports the data to the supplied text writer.
            /// </summary>
            /// <param name="data">The data.</param>
            /// <param name="writer">The writer.</param>
            public override void Export(IDataResult data, TextWriter writer)
            {
                var columns = data.FetchColumns().Select(c => c.Name).ToArray();
                writer.WriteLine(string.Join(" | ", columns));
                writer.WriteLine(new string('-', 80));

                foreach (var row in data.AsEnumerable())
                {
                    writer.WriteLine(string.Join(" | ", columns.Select(c => Convert.ToString(data.FetchValue(row, c))).ToArray()));
                }
            }
        }
    }
}

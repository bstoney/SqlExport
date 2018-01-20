namespace SqlExport.Logic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Win32;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Export;
    using SqlExport.Messages;
    using SqlExport.View;
    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the ExportHelper class.
    /// </summary>
    internal class ExportHelper
    {
        /// <summary>
        /// The exporter
        /// </summary>
        private ExporterBase exporter;

        /// <summary>
        /// Occurs when the export is starting.
        /// </summary>
        public event EventHandler ExportStarted;

        /// <summary>
        /// Occurs when the export is complete.
        /// </summary>
        public event EventHandler ExportComplete;

        /// <summary>
        /// Gets or sets a value indicating whether to export to file.
        /// </summary>
        public bool ExportToFile { get; set; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public IDataResult Data { get; private set; }

        /// <summary>
        /// Gets the clipboard data.
        /// </summary>
        public StringBuilder ClipboardData { get; private set; }

        /// <summary>
        /// Exports the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="exporterType">Type of the exporter.</param>
        /// <exception cref="System.ArgumentNullException">The supplied data is null.</exception>
        public void ExportData(IDataResult data, string exporterType)
        {
            this.Data = data;
            if (this.Data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.exporter = ExportAdapterHelper.GetExportAdapter(exporterType);

            // Load properties.
            var properties = new List<PropertyItem>();

            var filenameProperty = new FilePropertyItem("General", "SqlExport.LastUsedFilename", this.exporter.FileFilter)
            {
                DisplayName = "Filename"
            };

            if (this.ExportToFile)
            {
                properties.Add(filenameProperty);
            }

            properties.AddRange(this.exporter.GetProperties());

            // Initialise and display dialog.
            var dialog = new ExportDialog();
            dialog.Title = string.Concat("Export to ", exporterType);

            Messenger.Default.Send(new SetPropertiesMessage(properties), dialog.DataContext);

            Messenger.Default.Register<DialogOkMessage>(
                this,
                dialog.DataContext,
                m => this.RunDataExport(filenameProperty));

            Messenger.Default.Register<DialogCancelMessage>(
                this,
                dialog.DataContext,
                m => this.OnExportComplete());

            // Show appropriate dialogs.
            if (this.ExportToFile)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = filenameProperty.Value;
                saveFile.Filter = this.exporter.FileFilter;

                try
                {
                    saveFile.InitialDirectory = Path.GetDirectoryName(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new UnhandledExceptionMessage { Exception = ex });
                }

                if (saveFile.ShowDialog() ?? false)
                {
                    filenameProperty.Value = saveFile.FileName;
                    if (properties.Count > 1)
                    {
                        dialog.ShowDialog();
                    }
                    else
                    {
                        this.RunDataExport(filenameProperty);
                    }
                }
            }
            else
            {
                if (properties.Count > 0)
                {
                    dialog.ShowDialog();
                }
                else
                {
                    this.RunDataExport(filenameProperty);
                }
            }
        }

        /// <summary>
        /// Runs the data export.
        /// </summary>
        /// <param name="filenameProperty">The filename property.</param>
        private void RunDataExport(FilePropertyItem filenameProperty)
        {
            // Save property values to configuration.
            Configuration.SaveOptionsFrom(this.exporter);

            // Do export.
            Thread t;
            if (this.ExportToFile)
            {
                this.Filename = filenameProperty.Value;
                try
                {
                    Configuration.Current.CurrentDirectory = Path.GetDirectoryName(this.Filename);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new UnhandledExceptionMessage { Exception = ex });
                }

                t = new Thread(this.StartThreadedExport);
            }
            else
            {
                t = new Thread(this.StartThreadedExportToClipboard);
                t.TrySetApartmentState(ApartmentState.STA);
            }

            t.Priority = ThreadPriority.BelowNormal;
            t.Name = "Export Data";
            t.Start();
        }

        /// <summary>
        /// Starts the threaded export.
        /// </summary>
        private void StartThreadedExport()
        {
            this.OnExportStarted();
            try
            {
                this.exporter.Export(this.Data, this.Filename);
            }
            catch (Exception exp)
            {
                this.Error = exp;
            }

            this.OnExportComplete();
        }

        /// <summary>
        /// Starts the threaded export to clipboard.
        /// </summary>
        private void StartThreadedExportToClipboard()
        {
            this.OnExportStarted();
            try
            {
                this.ClipboardData = new StringBuilder();
                using (var sw = new StringWriter(this.ClipboardData))
                {
                    this.exporter.Export(this.Data, sw);
                }
            }
            catch (Exception exp)
            {
                this.Error = exp;
            }

            this.OnExportComplete();
        }

        /// <summary>
        /// Called when the export is starting.
        /// </summary>
        private void OnExportStarted()
        {
            if (this.ExportStarted != null)
            {
                this.ExportStarted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the export is complete.
        /// </summary>
        private void OnExportComplete()
        {
            if (this.ExportComplete != null)
            {
                this.ExportComplete(this, EventArgs.Empty);
            }
        }
    }
}

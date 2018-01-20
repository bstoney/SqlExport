using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using SqlExport.Ui;
using System.IO;
using SqlExport.Export;
using SqlExport.Data;
using SqlExport.Ui.View;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Ui.Messages;
using SqlExport.Ui.ViewModel;
using SqlExport;
using Microsoft.Win32;

namespace SqlExport.Business
{
    using SqlExport.Common;
    using SqlExport.ViewModel;

    internal class ExportHelper
    {
        private ExporterBase _exporter;

        public event EventHandler ExportStarted;
        public event EventHandler ExportComplete;

        public bool ExportToFile { get; set; }

        public Exception Error { get; private set; }

        public string Filename { get; private set; }

        public DataResult Data { get; private set; }

        public StringBuilder ClipboardData { get; private set; }

        public void ExportData(DataResult data, string exporterType)
        {
            Data = data;
            if (Data == null)
            {
                throw new ArgumentNullException("data");
            }

            _exporter = ExportAdapterHelper.GetExportAdapter(exporterType);

            // Load properties.
            List<PropertyItem> properties = new List<PropertyItem>();

            var filenameProperty = new FilePropertyItem("General", "SqlExport.LastUsedFilename", _exporter.FileFilter)
            {
                DisplayName = "Filename"
            };

            if (ExportToFile)
            {
                properties.Add(filenameProperty);
            }

            _exporter.LoadPropertiesAndValues(properties);

            // Initialise and display dialog.
            ExportDialog dialog = new ExportDialog();
            dialog.Title = string.Concat("Export to ", exporterType);

            Messenger.Default.Send(new SetPropertiesMessage(properties), dialog.DataContext);

            Messenger.Default.Register<DialogOkMessage>(this, dialog.DataContext, m =>
            {
                RunDataExport(properties, filenameProperty);
            });

            Messenger.Default.Register<DialogCancelMessage>(this, dialog.DataContext, m =>
            {
                OnExportComplete();
            });

            // Show appropriate dialogs.
            if (ExportToFile)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = filenameProperty.Value;
                saveFile.Filter = _exporter.FileFilter;

                try
                {
                    saveFile.InitialDirectory = Path.GetDirectoryName(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new UnhandledExceptionMessage() { Exception = ex });
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
                        RunDataExport(properties, filenameProperty);
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
                    RunDataExport(properties, filenameProperty);
                }
            }
        }

        private void RunDataExport(List<PropertyItem> properties, FilePropertyItem filenameProperty)
        {
            // Save property values to configuration.
            foreach (var prop in properties)
            {
                Options.Current.SetExportOption(_exporter.Name, prop.Name, prop.Value);
            }
            
            Options.Save();

            // Do export.
            Thread t;
            if (ExportToFile)
            {
                Filename = filenameProperty.Value;
                try
                {
                    Options.Current.CurrentDirectory = Path.GetDirectoryName(Filename);
                    Options.Save();
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new UnhandledExceptionMessage() { Exception = ex });
                }

                t = new Thread(new ThreadStart(StartThreadedExport));
            }
            else
            {
                t = new Thread(new ThreadStart(StartThreadedExportToClipboard));
                t.TrySetApartmentState(ApartmentState.STA);
            }

            t.Priority = ThreadPriority.BelowNormal;
            t.Name = "Export Data";
            t.Start();
        }

        private void StartThreadedExport()
        {
            OnExportStarted();
            try
            {
                _exporter.Export(Data, Filename);
            }
            catch (Exception exp)
            {
                Error = exp;
            }

            OnExportComplete();
        }

        private void StartThreadedExportToClipboard()
        {
            OnExportStarted();
            try
            {
                ClipboardData = new StringBuilder();
                using (StringWriter sw = new StringWriter(ClipboardData))
                {
                    _exporter.Export(Data, sw);
                }
            }
            catch (Exception exp)
            {
                Error = exp;
            }

            OnExportComplete();
        }

        private void OnExportStarted()
        {
            if (ExportStarted != null)
            {
                ExportStarted(this, EventArgs.Empty);
            }
        }

        private void OnExportComplete()
        {
            if (ExportComplete != null)
            {
                ExportComplete(this, EventArgs.Empty);
            }
        }
    }
}

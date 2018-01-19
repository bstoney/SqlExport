namespace SqlExport.Logic
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Messages;
    using SqlExport.ViewModel;

    /// <summary>
    /// Defines the TemporaryFilesLogic class.
    /// </summary>
    internal static class TemporaryFilesLogic
    {
        /// <summary>
        /// The temp file extension.
        /// </summary>
        private const string TempFileExtension = ".sqle";

        /// <summary>
        /// Saves the query temp.
        /// </summary>
        /// <param name="queries">The queries.</param>
        public static void SaveQueryTemp(IEnumerable<QueryViewModel> queries)
        {
            Directory.GetFiles(Path.GetTempPath(), string.Concat("~*", TempFileExtension)).ToList().ForEach(File.Delete);

            var indexedQueries = queries.Select((q, i) => new { i, q });

            foreach (var query in indexedQueries)
            {
                using (var sw = File.CreateText(Path.Combine(Path.GetTempPath(), string.Concat("~", query.i, TempFileExtension))))
                {
                    sw.WriteLine(query.q.Filename);
                    sw.WriteLine(query.q.EditorViewDataContext.HasChanged);
                    sw.Write(query.q.EditorViewDataContext.AllText);
                }
            }
        }

        /// <summary>
        /// Loads the query temp.
        /// </summary>
        public static void LoadQueryTemp()
        {
            var tempFiles = from f in Directory.GetFiles(Path.GetTempPath(), string.Concat("~*", TempFileExtension))
                            orderby f
                            select f;

            foreach (var f in tempFiles)
            {
                using (StreamReader sr = File.OpenText(f))
                {
                    var firstLine = sr.ReadLine();
                    string filename = firstLine;

                    bool hasChanged;
                    bool.TryParse(sr.ReadLine(), out hasChanged);
                    var queryText = sr.ReadToEnd();

                    Messenger.Default.Send(new OpenQueryMessage(filename, null, queryText, hasChanged));
                }

                // Files are deleted only when the application exits successfully.
            }
        }

        /// <summary>
        /// Save temp files in an emergency.
        /// </summary>
        public static void EmegencySave()
        {
            var mainWindow = Application.Current.MainWindow;
            mainWindow.Dispatcher.Invoke(() => SaveQueryTemp(((MainWindowViewModel)mainWindow.DataContext).Queries));
        }
    }
}

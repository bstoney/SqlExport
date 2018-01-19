namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Documents;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Messages;

    /// <summary>
    /// Defines the AboutViewModel class.
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        public AboutViewModel()
        {
            this.ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;

            var infoDictionary = new Dictionary<string, List<string>>();

            var key = "Current Query";
            infoDictionary.Add(key, new List<string>());
            var message = new GetSelectedQueryMessage();
            Messenger.Default.Send(message);
            if (message.Query != null)
            {
                infoDictionary[key].Add(message.Query.DisplayText);
                infoDictionary[key].Add("Database: " + message.Query.Database);
                infoDictionary[key].Add("File: " + message.Query.Filename);
                infoDictionary[key].Add("Size: " + string.Format("{0:#,##0.0}kb", message.Query.EditorViewDataContext.AllText.Length / 1024f));
                infoDictionary[key].Add((message.Query.HasConnection ? "Is" : "Is not") + " connected");
                infoDictionary[key].Add((message.Query.HasTransaction ? "Has an" : "Has no") + " active transaction");
                infoDictionary[key].Add((message.Query.EditorViewDataContext.HasChanged ? "Has" : "Has no") + " modifications");
                infoDictionary[key].Add((message.Query.IsExecuting ? "Is" : "Is not") + " executing");
                infoDictionary[key].Add((message.Query.IsRunning ? "Is" : "Is not") + " running");
                infoDictionary[key].Add((message.Query.CanExport ? "Is" : "Is not") + " exportable");
            }
            else
            {
                infoDictionary[key].Add("No query selected.");
            }

            var info = new FlowDocument();
            info.FontFamily = SystemFonts.MessageFontFamily;
            info.FontWeight = SystemFonts.MessageFontWeight;
            info.FontSize = SystemFonts.MessageFontSize;
            foreach (var value in infoDictionary)
            {
                var paragraph = new Paragraph();
                paragraph.Margin = info.Blocks.Count > 0 ? new Thickness(0) : new Thickness(0, 10, 0, 0);
                paragraph.Inlines.Add(new Bold(new Run(value.Key)));

                var list = new List();
                list.Margin = new Thickness(0);
                list.Padding = new Thickness(10, 0, 0, 0);
                list.MarkerStyle = TextMarkerStyle.None;
                foreach (var detail in value.Value)
                {
                    list.ListItems.Add(new ListItem(new Paragraph(new Run(detail))));
                }

                info.Blocks.Add(paragraph);
                info.Blocks.Add(list);
            }

            this.ApplicationInformation = info;
        }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string ApplicationName { get; private set; }

        /// <summary>
        /// Gets the application information.
        /// </summary>
        /// <value>
        /// The application information.
        /// </value>
        public FlowDocument ApplicationInformation { get; private set; }
    }
}

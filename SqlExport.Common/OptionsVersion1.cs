namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Xml;

    using SqlExport.Common.Data;

    internal sealed class OptionsVersion1 : IOptionLoader, IOptions
    {
        private const int recentFileCount = 8;

        private Dictionary<string, NameValueCollection> exportOptions =
            new Dictionary<string, NameValueCollection>(EqualityComparer<string>.Default);
        private List<DatabaseDetails> databaseDetails = new List<DatabaseDetails>();
        private Dictionary<string, NameValueCollection> databaseOptions =
            new Dictionary<string, NameValueCollection>(EqualityComparer<string>.Default);
        private string[] files = new string[recentFileCount];

        private string exportType;
        private string currentDirectory;
        private bool editable;
        private bool transactional;
        private bool singleInstance;
        private string editorControl;
        private Font editorFont;
        private int tabWidth;
        private bool wordWrap;
        private bool lineNumbers;
        private WindowStateWrapper windowState;
        private int commandTimeout;
        private string currentDatabase;

        internal OptionsVersion1()
        {
            this.ExportType = "Csv";
            this.Transactional = true;
            this.SingleInstance = true;
            this.CommandTimeout = 30;
            this.EditorControl = "SqlExport.Editor.ScintillaEditor.ScintillaTextBox, SqlExport.Editor.Scintilla";
            this.EditorFont = new Font { Name = "Courier New", Size = 10, Style = FontStyle.Regular };
            this.TabWidth = 4;
            this.LineNumbers = true;
            this.WindowState = new WindowStateWrapper { X = 15, Y = 15, Width = 994, Height = 488, State = FormWindowState.Normal };
        }

        internal enum FormWindowState
        {
            Normal,
            Minimized,
            Maximized
        }

        internal class WindowStateWrapper
        {
            public int X { get; set; }

            public int Y { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public FormWindowState State { get; set; }
        }

        internal enum FontStyle
        {
            Regular,
            Bold,
            Italic,
            Strikeout,
            Underline
        }

        internal class Font
        {
            public string Name { get; set; }

            public float Size { get; set; }

            public FontStyle Style { get; set; }
        }

        public DateTime? LastLoaded { get; set; }

        /// <summary>
        /// Gets the options.
        /// </summary>
        public IOptions Options
        {
            get { return this; }
        }

        public IEnumerable<Option> AllOptions { get; private set; }

        public int RecentFileCount
        {
            get { return recentFileCount; }
            set { throw new NotImplementedException(); }
        }

        public string ExportType
        {
            get { return exportType; }
            set { exportType = value; }
        }

        public string CurrentDirectory
        {
            get { return currentDirectory; }
            set { currentDirectory = value; }
        }

        public bool Editable
        {
            get { return editable; }
            set { editable = value; }
        }

        public bool Transactional
        {
            get { return transactional; }
            set { transactional = value; }
        }

        public bool SingleInstance
        {
            get { return singleInstance; }
            set { singleInstance = value; }
        }

        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        public string EditorControl
        {
            get { return editorControl; }
            set { editorControl = value; }
        }

        public Font EditorFont
        {
            get { return editorFont; }
            set { editorFont = value; }
        }

        public int TabWidth
        {
            get { return tabWidth; }
            set { tabWidth = value; }
        }

        public bool WordWrap
        {
            get { return wordWrap; }
            set { wordWrap = value; }
        }

        public bool LineNumbers
        {
            get { return lineNumbers; }
            set { lineNumbers = value; }
        }

        public WindowStateWrapper WindowState
        {
            get { return windowState; }
            set { windowState = value; }
        }

        #region Databases

        public int DatabaseCount
        {
            get { return databaseDetails.Count; }
        }

        public string CurrentDatabase
        {
            get { return currentDatabase; }
            set { currentDatabase = value; }
        }

        public DatabaseDetails GetDatabase(int index)
        {
            return databaseDetails[index];
        }

        public DatabaseDetails GetDatabase(string name)
        {
            return databaseDetails.FirstOrDefault(d => d.Name == name);
        }

        public void SetDatabase(DatabaseDetails details, int index)
        {
            if (index >= 0 && index < databaseDetails.Count)
            {
                databaseDetails[index] = details;
            }
        }

        public void DeleteDatabase(int index)
        {
            if (index >= 0 && index < databaseDetails.Count)
            {
                databaseDetails.RemoveAt(index);
            }
        }

        public void AddDatabase(DatabaseDetails details)
        {
            databaseDetails.Add(details);
        }

        public string GetDatabaseOption(string type, string option)
        {
            string value = string.Empty;
            if (databaseOptions.ContainsKey(type) && databaseOptions[type][option] != null)
            {
                value = databaseOptions[type][option];
            }

            return value;
        }

        public void SetDatabaseOption(string type, string option, string value)
        {
            if (!databaseOptions.ContainsKey(type))
            {
                databaseOptions[type] = new NameValueCollection();
            }

            databaseOptions[type][option] = value;
        }

        #endregion

        #region Recent Files

        public string GetRecentFile(int index)
        {
            return files[index];
        }

        public void SetRecentFile(string filename, int index)
        {
            files[index] = filename;
        }

        #endregion

        public string GetExportOption(string type, string option)
        {
            string value = string.Empty;
            if (exportOptions.ContainsKey(type) && exportOptions[type][option] != null)
            {
                value = exportOptions[type][option];
            }

            return value;
        }

        public void SetExportOption(string type, string option, string value)
        {
            if (!exportOptions.ContainsKey(type))
            {
                exportOptions[type] = new NameValueCollection();
            }

            exportOptions[type][option] = value;
        }

        public void SaveConfig(XmlDocument document)
        {
            XmlNode node = document;

            node = node.WriteStartElement("SqlExport");
            node = node.WriteElementString("CurrentDirectory", CurrentDirectory);
            node = node.WriteElementString("Editable", Editable.ToString());
            node = node.WriteElementString("Transactional", Transactional.ToString());
            node = node.WriteElementString("SingleInstance", SingleInstance.ToString());
            node = node.WriteElementString("CommandTimeout", CommandTimeout.ToString());
            node = node.WriteElementString("EditorControl", EditorControl.ToString());
            node = node.WriteStartElement("EditorFont");
            node = node.WriteAttributeString("Name", EditorFont.Name);
            node = node.WriteAttributeString("Size", EditorFont.Size.ToString());
            node = node.WriteAttributeString("Weight", EditorFont.Style.ToString());
            node = node.WriteEndElement();
            node = node.WriteElementString("TabWidth", TabWidth.ToString());
            node = node.WriteElementString("WordWrap", WordWrap.ToString());
            node = node.WriteElementString("LineNumbers", LineNumbers.ToString());
            node = node.WriteStartElement("WindowState");
            node = node.WriteStartElement("WindowSize");
            node = node.WriteAttributeString("X", WindowState.X.ToString());
            node = node.WriteAttributeString("Y", WindowState.Y.ToString());
            node = node.WriteAttributeString("Width", WindowState.Width.ToString());
            node = node.WriteAttributeString("Height", WindowState.Height.ToString());
            node = node.WriteEndElement();
            node = node.WriteElementString("FormState", WindowState.State.ToString());
            node = node.WriteEndElement();

            node = node.WriteStartElement("DatabaseSettings");
            node = node.WriteElementString("CurrentDatabase", CurrentDatabase);
            node = node.WriteStartElement("Databases");
            for (int i = 0; i < DatabaseCount; i++)
            {
                node = node.WriteStartElement("Database");
                DatabaseDetails dbd = GetDatabase(i);
                node = node.WriteAttributeString("Name", dbd.Name);
                node = node.WriteAttributeString("ConnectionString", dbd.ConnectionString);
                node = node.WriteAttributeString("ConnectionType", dbd.Type);
                node = node.WriteEndElement();
            }

            node = node.WriteEndElement();
            node = node.WriteStartElement("Options");
            foreach (string key in databaseOptions.Keys)
            {
                node = node.WriteStartElement(key);
                foreach (string item in databaseOptions[key].AllKeys)
                {
                    node = node.WriteElementString(item, GetDatabaseOption(key, item));
                }

                node = node.WriteEndElement();
            }

            node = node.WriteEndElement();
            node = node.WriteEndElement();

            node = node.WriteStartElement("RecentFiles");
            for (int i = 0; i < RecentFileCount; i++)
            {
                node = node.WriteElementString("File", GetRecentFile(i));
            }

            node = node.WriteEndElement();

            node = node.WriteStartElement("Export");
            node = node.WriteElementString("Type", ExportType);
            node = node.WriteStartElement("Options");
            foreach (string key in exportOptions.Keys)
            {
                node = node.WriteStartElement(key);
                foreach (string item in exportOptions[key].AllKeys)
                {
                    node = node.WriteElementString(item, GetExportOption(key, item));
                }

                node = node.WriteEndElement();
            }

            node = node.WriteEndElement();
            node = node.WriteEndElement();

            ////node = node.WriteStartElement("Adapters");

            ////node = node.WriteStartElement("ConnectionAdapters");
            ////foreach (string key in ConnectionAdapterHelper.RegisteredConnectionAdapters)
            ////{
            ////    node = node.WriteElementString(key, ConnectionAdapterHelper.GetAdapterType(key));
            ////}

            ////node = node.WriteEndElement();

            ////node = node.WriteStartElement("ExportAdapters");
            ////foreach (string key in ExportAdapterHelper.RegisteredExportAdapters)
            ////{
            ////    node = node.WriteElementString(key, ExportAdapterHelper.GetAdapterType(key));
            ////}

            ////node = node.WriteEndElement();

            node = node.WriteEndElement();

            node = node.WriteEndElement();
        }

        public void LoadConfig(XmlDocument document)
        {
            XmlNode sqlExport = document.DocumentElement;

            CurrentDirectory = LoadSettingValue(sqlExport, "CurrentDirectory", CurrentDirectory);
            Editable = Convert.ToBoolean(LoadSettingValue(sqlExport, "Editable", Editable));
            Transactional = Convert.ToBoolean(LoadSettingValue(sqlExport, "Transactional", Transactional));
            SingleInstance = Convert.ToBoolean(LoadSettingValue(sqlExport, "SingleInstance", SingleInstance));
            CommandTimeout = Convert.ToInt32(LoadSettingValue(sqlExport, "CommandTimeout", CommandTimeout));
            EditorControl = LoadSettingValue(sqlExport, "EditorControl", EditorControl);

            string name = LoadSettingValue(sqlExport, "EditorFont/@Name", EditorFont.Name);
            float size = Convert.ToSingle(LoadSettingValue(sqlExport, "EditorFont/@Size", EditorFont.Size));
            FontStyle style = (FontStyle)Enum.Parse(typeof(FontStyle),
                LoadSettingValue(sqlExport, "EditorFont/@Weight", EditorFont.Style), true);
            EditorFont = new Font { Name = name, Size = size, Style = style };

            TabWidth = Convert.ToInt32(LoadSettingValue(sqlExport, "TabWidth", TabWidth));
            WordWrap = Convert.ToBoolean(LoadSettingValue(sqlExport, "WordWrap", WordWrap));
            LineNumbers = Convert.ToBoolean(LoadSettingValue(sqlExport, "LineNumbers", LineNumbers));

            var ws = new WindowStateWrapper();
            ws.X = Convert.ToInt32(LoadSettingValue(sqlExport, "WindowState/WindowSize/@X", WindowState.X));
            ws.Y = Convert.ToInt32(LoadSettingValue(sqlExport, "WindowState/WindowSize/@Y", WindowState.Y));
            ws.Width = Convert.ToInt32(LoadSettingValue(sqlExport, "WindowState/WindowSize/@Width", WindowState.Width));
            ws.Height = Convert.ToInt32(LoadSettingValue(sqlExport, "WindowState/WindowSize/@Height", WindowState.Height));
            ws.State = (FormWindowState)Enum.Parse(typeof(FormWindowState),
                LoadSettingValue(sqlExport, "WindowState/FormState", WindowState.State), true);
            WindowState = ws;

            CurrentDatabase = LoadSettingValue(sqlExport, "DatabaseSettings/CurrentDatabase", CurrentDatabase);
            databaseDetails.Clear();
            foreach (XmlNode node in sqlExport.SelectNodes("DatabaseSettings/Databases/Database"))
            {
                DatabaseDetails dbd = new DatabaseDetails();
                dbd.Name = LoadSettingValue(node, "@Name", null);
                dbd.ConnectionString = LoadSettingValue(node, "@ConnectionString", null);
                dbd.Type = LoadSettingValue(node, "@ConnectionType", null);
                databaseDetails.Add(dbd);
            }

            databaseOptions.Clear();
            foreach (XmlNode node in sqlExport.SelectNodes("DatabaseSettings/Options/*"))
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    SetDatabaseOption(node.Name, item.Name, item.InnerText);
                }
            }

            int i = 0;
            foreach (XmlNode node in sqlExport.SelectNodes("RecentFiles/File"))
            {
                files[i] = node.InnerText;
                i++;
            }

            for (; i < RecentFileCount; i++)
            {
                files[i] = null;
            }

            ExportType = LoadSettingValue(sqlExport, "Export/Type", ExportType);
            exportOptions.Clear();
            foreach (XmlNode node in sqlExport.SelectNodes("Export/Options/*"))
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    SetExportOption(node.Name, item.Name, item.InnerText);
                }
            }

            ////foreach (XmlNode node in sqlExport.SelectNodes("Adapters/ConnectionAdapters/*"))
            ////{
            ////    if (!ConnectionAdapterHelper.IsConnectionAdapterRegistered(node.Name))
            ////    {
            ////        ConnectionAdapterHelper.RegisterConnectionAdapter(node.Name, node.InnerText);
            ////    }
            ////}

            ////foreach (XmlNode node in sqlExport.SelectNodes("Adapters/ExportAdapters/*"))
            ////{
            ////    if (!ExportAdapterHelper.IsExportAdapterRegistered(node.Name))
            ////    {
            ////        ExportAdapterHelper.RegisterExportAdapter(node.Name, node.InnerText);
            ////    }
            ////}
        }

        private static string LoadSettingValue(XmlNode parent, string path, object defaultValue)
        {
            string value = Convert.ToString(defaultValue);
            XmlNode node = parent.SelectSingleNode(path);
            if (node != null)
            {
                value = node.InnerText;
            }

            return value;
        }


        /// <summary>
        /// Gets the value with the supplied name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public string GetOptionValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the option value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SetOptionValue(string name, object value)
        {
            throw new NotImplementedException();
        }
    }
}

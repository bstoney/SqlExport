namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using SqlExport.Common.Data;

    /// <summary>
    /// Defines the OptionsVersion2 type.
    /// </summary>
    public sealed class OptionsVersion2 : IOptionLoader, IOptions
    {
        /// <summary>
        /// The options.
        /// </summary>
        private readonly Option options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsVersion2"/> class.
        /// </summary>
        internal OptionsVersion2()
        {
            this.options = new Option("SqlExport");
            this.SetOptionValue("@Version", "2");

            this.RecentFileCount = 8;
            this.ExportType = "Csv";
            this.Transactional = true;
            this.SingleInstance = true;
            this.CommandTimeout = 30;
            this.SetOptionValue(KnownOptions.EditorFontName, "Courier New");
            this.SetOptionValue(KnownOptions.EditorFontSize, 10);
            this.SetOptionValue(KnownOptions.EditorFontStyle, "Regular");
            this.TabWidth = 4;
            this.LineNumbers = true;
            this.SetOptionValue(KnownOptions.WindowStateWindowSizeX, 15);
            this.SetOptionValue(KnownOptions.WindowStateWindowSizeY, 15);
            this.SetOptionValue(KnownOptions.WindowStateWindowSizeWidth, 994);
            this.SetOptionValue(KnownOptions.WindowStateWindowSizeHeight, 488);
            this.SetOptionValue(KnownOptions.WindowStateFormState, "Normal");
        }

        /// <summary>
        /// Gets or sets the last loaded.
        /// </summary>
        public DateTime? LastLoaded { get; set; }

        /// <summary>
        /// Gets the options.
        /// </summary>
        public IOptions Options
        {
            get { return this; }
        }

        /// <summary>
        /// Gets or sets the recent file count.
        /// </summary>
        public int RecentFileCount
        {
            get { return Convert.ToInt32(this.GetOptionValue("RecentFiles/@Count")); }
            set { this.SetOptionValue("RecentFiles/@Count", value); }
        }

        /// <summary>
        /// Gets or sets the type of the export.
        /// </summary>
        /// <value>
        /// The type of the export.
        /// </value>
        public string ExportType
        {
            get { return this.GetOptionValue("Export/Type"); }
            set { this.SetOptionValue("Export/Type", value); }
        }

        /// <summary>
        /// Gets or sets the current directory.
        /// </summary>
        public string CurrentDirectory
        {
            get { return this.GetOptionValue("CurrentDirectory"); }
            set { this.SetOptionValue("CurrentDirectory", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IOptions"/> is editable.
        /// </summary>
        public bool Editable
        {
            get { return Convert.ToBoolean(this.GetOptionValue("Editable")); }
            set { this.SetOptionValue("Editable", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IOptions"/> is transactional.
        /// </summary>
        public bool Transactional
        {
            get { return Convert.ToBoolean(this.GetOptionValue("Transactional")); }
            set { this.SetOptionValue("Transactional", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [single instance].
        /// </summary>
        public bool SingleInstance
        {
            get { return Convert.ToBoolean(this.GetOptionValue("SingleInstance")); }
            set { this.SetOptionValue("SingleInstance", value); }
        }

        /// <summary>
        /// Gets or sets the command timeout.
        /// </summary>
        public int CommandTimeout
        {
            get { return Convert.ToInt32(this.GetOptionValue("CommandTimeout")); }
            set { this.SetOptionValue("CommandTimeout", value); }
        }

        /// <summary>
        /// Gets or sets the width of the tab.
        /// </summary>
        /// <value>
        /// The width of the tab.
        /// </value>
        public int TabWidth
        {
            get { return Convert.ToInt32(this.GetOptionValue("TabWidth")); }
            set { this.SetOptionValue("TabWidth", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [word wrap].
        /// </summary>
        public bool WordWrap
        {
            get { return Convert.ToBoolean(this.GetOptionValue("WordWrap")); }
            set { this.SetOptionValue("WordWrap", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [line numbers].
        /// </summary>
        public bool LineNumbers
        {
            get { return Convert.ToBoolean(this.GetOptionValue("LineNumbers")); }
            set { this.SetOptionValue("LineNumbers", value); }
        }

        /// <summary>
        /// Gets the database count.
        /// </summary>
        public int DatabaseCount
        {
            get { return this.GetOptions("DatabaseSettings/Databases/Database").Count(); }
        }

        /// <summary>
        /// Gets or sets the current database.
        /// </summary>
        public string CurrentDatabase
        {
            get { return this.GetOptionValue("DatabaseSettings/CurrentDatabase"); }
            set { this.SetOptionValue("DatabaseSettings/CurrentDatabase", value); }
        }

        /// <summary>
        /// Gets all options.
        /// </summary>
        public IEnumerable<Option> AllOptions
        {
            get { return this.options.Children; }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The data base details.
        /// </returns>
        public DatabaseDetails GetDatabase(int index)
        {
            var databasePath = GetIndexedPath("DatabaseSettings/Databases/Database", index);
            var namePath = databasePath + "/@Name";
            var connectionStringPath = databasePath + "/@ConnectionString";
            var connectionTypePath = databasePath + "/@ConnectionType";

            return new DatabaseDetails
                {
                    Name = this.GetOptionValue(namePath),
                    ConnectionString = this.GetOptionValue(connectionStringPath),
                    Type = this.GetOptionValue(connectionTypePath)
                };
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The database details.
        /// </returns>
        public DatabaseDetails GetDatabase(string name)
        {
            var option = this.GetOptions("DatabaseSettings/Databases/Database").FirstOrDefault(o => o.Properties["Name"] == name);

            return new DatabaseDetails
            {
                Name = option.Properties["Name"],
                ConnectionString = option.Properties["ConnectionString"],
                Type = option.Properties["ConnectionType"]
            };
        }

        /// <summary>
        /// Sets the database.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <param name="index">The index.</param>
        public void SetDatabase(DatabaseDetails details, int index)
        {
            var databasePath = GetIndexedPath("DatabaseSettings/Databases/Database", index);
            var namePath = databasePath + "/@Name";
            var connectionStringPath = databasePath + "/@ConnectionString";
            var connectionTypePath = databasePath + "/@ConnectionType";

            this.SetOptionValue(namePath, details.Name);
            this.SetOptionValue(connectionStringPath, details.ConnectionString);
            this.SetOptionValue(connectionTypePath, details.Type);
        }

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="index">The index.</param>
        public void DeleteDatabase(int index)
        {
            var databasePath = GetIndexedPath("DatabaseSettings/Databases/Database", index);
            this.options.RemoveByPath(databasePath);
        }

        /// <summary>
        /// Adds the database.
        /// </summary>
        /// <param name="details">The details.</param>
        public void AddDatabase(DatabaseDetails details)
        {
            this.SetDatabase(details, this.DatabaseCount + 1);
        }

        /// <summary>
        /// Gets the database option.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        /// The option value.
        /// </returns>
        public string GetDatabaseOption(string type, string option)
        {
            var optionPath = string.Format("DatabaseSettings/Options/{0}/{1}", type, option);
            return this.GetOptionValue(optionPath);
        }

        /// <summary>
        /// Sets the database option.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="option">The option.</param>
        /// <param name="value">The value.</param>
        public void SetDatabaseOption(string type, string option, string value)
        {
            var optionPath = string.Format("DatabaseSettings/Options/{0}/{1}", type, option);
            this.SetOptionValue(optionPath, value);
        }

        /// <summary>
        /// Gets the recent file at the index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The recent file.</returns>
        public string GetRecentFile(int index)
        {
            return this.GetOptionValue(GetIndexedPath("RecentFiles/File", index));
        }

        /// <summary>
        /// Sets the recent file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="index">The index.</param>
        public void SetRecentFile(string filename, int index)
        {
            this.SetOptionValue(GetIndexedPath("RecentFiles/File", index), filename);
        }

        /// <summary>
        /// Gets the export option.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="option">The option.</param>
        /// <returns>the value.</returns>
        public string GetExportOption(string type, string option)
        {
            var optionPath = string.Format("Export/Options/{0}/{1}", type, option);
            return this.GetOptionValue(optionPath);
        }

        /// <summary>
        /// Sets the export option.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="option">The option.</param>
        /// <param name="value">The value.</param>
        public void SetExportOption(string type, string option, string value)
        {
            var optionPath = string.Format("Export/Options/{0}/{1}", type, option);
            this.SetOptionValue(optionPath, value);
        }

        /// <summary>
        /// Gets the option.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The first option value.
        /// </returns>
        public string GetOptionValue(string path)
        {
            return this.options.GetValueByPath(path);
        }

        /// <summary>
        /// Sets the option.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        public void SetOptionValue(string path, object value)
        {
            this.options.SetValueByPath(path, value == null ? null : value.ToString());
        }

        /// <summary>
        /// Saves the config.
        /// </summary>
        /// <param name="document">The document.</param>
        public void SaveConfig(XmlDocument document)
        {
            var root = document.DocumentElement;

            if (root == null)
            {
                root = document.CreateElement(this.options.Name);
                document.AppendChild(root);
            }

            SaveOption(this.options, root);
        }

        /// <summary>
        /// Loads the config.
        /// </summary>
        /// <param name="document">The document.</param>
        public void LoadConfig(XmlDocument document)
        {
            XmlNode sqlExport = document.DocumentElement;
            var versionNumber = sqlExport.Attributes["Version"];
            if (versionNumber != null && versionNumber.Value == "2")
            {
                LoadOptions(this.options, sqlExport);
            }
            else
            {
                var oldOptions = new OptionsVersion1();
                oldOptions.LoadConfig(document);

                this.CommandTimeout = oldOptions.CommandTimeout;
                this.CurrentDirectory = oldOptions.CurrentDirectory;
                this.Editable = oldOptions.Editable;
                this.SetOptionValue(KnownOptions.EditorFontName, oldOptions.EditorFont.Name);
                this.SetOptionValue(KnownOptions.EditorFontSize, oldOptions.EditorFont.Size);
                this.SetOptionValue(KnownOptions.EditorFontStyle, oldOptions.EditorFont.Style);
                this.ExportType = oldOptions.ExportType;
                this.LineNumbers = oldOptions.LineNumbers;
                this.SingleInstance = oldOptions.SingleInstance;
                this.TabWidth = oldOptions.TabWidth;
                this.Transactional = oldOptions.Transactional;
                this.SetOptionValue(KnownOptions.WindowStateWindowSizeX, oldOptions.WindowState.X);
                this.SetOptionValue(KnownOptions.WindowStateWindowSizeY, oldOptions.WindowState.Y);
                this.SetOptionValue(KnownOptions.WindowStateWindowSizeWidth, oldOptions.WindowState.Width);
                this.SetOptionValue(KnownOptions.WindowStateWindowSizeHeight, oldOptions.WindowState.Height);
                this.SetOptionValue(KnownOptions.WindowStateFormState, oldOptions.WindowState.State);
                this.WordWrap = oldOptions.WordWrap;

                this.CurrentDatabase = oldOptions.CurrentDatabase;
                for (int i = 0; i < oldOptions.DatabaseCount; i++)
                {
                    this.SetDatabase(oldOptions.GetDatabase(i), i);
                }

                // TODO database options

                this.RecentFileCount = oldOptions.RecentFileCount;
                for (int i = 0; i < oldOptions.RecentFileCount; i++)
                {
                    this.SetRecentFile(oldOptions.GetRecentFile(i), i);
                }

                // TODO export options
            }
        }

        /// <summary>
        /// Gets the indexed path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private static string GetIndexedPath(string path, int index)
        {
            return string.Format("{0}[{1}]", path, index + 1);
        }

        /// <summary>
        /// Gets the option.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The first option value.
        /// </returns>
        private Option GetOption(string path)
        {
            return this.GetOptions(path).FirstOrDefault();
        }

        /// <summary>
        /// Gets the options with the supplied path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>All the options.</returns>
        private IEnumerable<Option> GetOptions(string path)
        {
            return this.options.GetChildrenByPath(path);
        }

        /// <summary>
        /// Saves the option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="node">The node.</param>
        private static void SaveOption(Option option, XmlNode node)
        {
            var doc = node.OwnerDocument ?? node as XmlDocument;

            foreach (var property in option.Properties)
            {
                var attribute = node.Attributes[property.Key];
                if (attribute == null)
                {
                    attribute = doc.CreateAttribute(property.Key);
                    node.Attributes.Append(attribute);
                }

                attribute.Value = property.Value;
            }

            if (option.Value != null)
            {
                node.InnerText = option.Value;
            }
            else
            {
                for (int i = 0; i < option.Children.Count(); i++)
                {
                    var childOption = option.Children.ElementAt(i);
                    var childNode = node.ChildNodes.OfType<XmlNode>().ElementAtOrDefault(i);
                    if (childNode == null)
                    {
                        childNode = doc.CreateElement(childOption.Name);
                        node.AppendChild(childNode);
                    }

                    SaveOption(childOption, childNode);
                }
            }
        }

        /// <summary>
        /// Loads the options from the supplied node.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="node">The node.</param>
        private static void LoadOptions(Option option, XmlNode node)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                option.Properties[attribute.Name] = attribute.Value;
            }

            var elements = node.OfType<XmlElement>().ToList();
            if (elements.Any())
            {
                var elementQuery = elements.GroupBy(e => e.Name)
                    .SelectMany(g => g.Select((e, i) => new
                    {
                        Name = e.Name,
                        Path = GetIndexedPath(e.Name, i),
                        Element = e
                    }));

                foreach (var indexedElement in elementQuery)
                {
                    var childOption = option.GetChildrenByPath(indexedElement.Path).FirstOrDefault();
                    if (childOption == null)
                    {
                        childOption = new Option(indexedElement.Name);
                        option.AddChild(childOption);
                    }

                    LoadOptions(childOption, indexedElement.Element);
                }
            }
            else
            {
                option.Value = node.InnerText;
            }
        }
    }
}

namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Monads;
    using System.Text;
    using System.Xml;

    using SqlExport.Common.Extensions;
    using SqlExport.Common.Options;
    using SqlExport.Common.Util;

    /// <summary>
    /// Defines the OptionsLoader class.
    /// </summary>
    public class OptionsLoader : IOptionLoader
    {
        /// <summary>
        /// The options.
        /// </summary>
        private Option options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsLoader"/> class.
        /// </summary>
        public OptionsLoader()
        {
            this.LoadDefaultOptions();
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
            get { return new ApplicationOptions(); }
        }

        /// <summary>
        /// Gets all options.
        /// </summary>
        public IEnumerable<Option> AllOptions
        {
            get { return this.options.Children; }
        }

        /// <summary>
        /// Saves the config.
        /// </summary>
        /// <param name="document">The document.</param>
        public void SaveConfig(XmlDocument document)
        {
            var root = document.CreateElement(this.options.Name.Name);
            document.AppendChild(root);

            SaveOption(this.options, root);
        }

        /// <summary>
        /// Loads the config.
        /// </summary>
        /// <param name="document">The document.</param>
        public void LoadConfig(XmlDocument document)
        {
            this.LoadDefaultOptions();

            // Load options from file
            XmlNode sqlExport = document.DocumentElement;
            LoadOptions(this.options, sqlExport);
        }

        /// <summary>
        /// Gets the value with the supplied name.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public string GetOptionValue(string path)
        {
            return this.options.GetValueByPath(path);
        }

        /// <summary>
        /// Sets the option value.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        public void SetOptionValue(string path, object value)
        {
            this.options.SetValueByPath(path, value == null ? null : value.ToString());
        }

        /// <summary>
        /// Removes the option.
        /// </summary>
        /// <param name="path">The path.</param>
        public void RemoveOption(string path)
        {
            this.options.RemoveByPath(path);
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
                option.Properties.SetPropertyValue(attribute.Name, attribute.Value);
            }

            var elements = node.OfType<XmlElement>().ToList();
            if (elements.Any())
            {
                var elementQuery = elements.GroupBy(e => e.Name)
                    .SelectMany(g => g.Select((e, i) => new
                    {
                        e.Name,
                        Path = GetIndexedPath(e.Name, i),
                        Element = e
                    }));

                foreach (var indexedElement in elementQuery)
                {
                    var childOption = option.GetChildrenByPath(indexedElement.Path).FirstOrDefault();
                    if (childOption == null)
                    {
                        childOption = new Option(new OptionName(indexedElement.Name));
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

        /// <summary>
        /// Saves the option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="node">The node.</param>
        private static void SaveOption(Option option, XmlNode node)
        {
            var doc = node.OwnerDocument ?? node as XmlDocument;

            if (doc == null)
            {
                throw new ArgumentException("The node must have an owner document.", "node");
            }

            foreach (var property in option.Properties.OrderBy(p => p.DisplayName))
            {
                var attribute = node.Attributes[property.Name.Name];
                if (attribute == null)
                {
                    attribute = doc.CreateAttribute(property.Name.Name);
                    node.Attributes.Append(attribute);
                }

                attribute.Value = property.Value;
            }

            if (!string.IsNullOrEmpty(option.Value))
            {
                node.InnerText = option.Value;
            }
            else
            {
                foreach (var childOption in option.Children.OrderBy(o => o.DisplayName))
                {
                    var childNode = doc.CreateElement(childOption.Name.Name);
                    node.AppendChild(childNode);
                    SaveOption(childOption, childNode);
                }
            }
        }

        /// <summary>
        /// Gets the indexed path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="index">The index.</param>
        /// <returns>A xml path with index.</returns>
        private static string GetIndexedPath(string path, int index)
        {
            return string.Format("{0}[{1}]", path, index + 1);
        }

        /// <summary>
        /// Loads the default options.
        /// </summary>
        private void LoadDefaultOptions()
        {
            this.options = new Option(new OptionName("SqlExport"));
            this.SetOptionValue("@Version", "3");

            // Load option definitions from application
            var optionDefinitions = from t in ReflectionExtensions.GetExportedTypes(false)
                                    from o in t.GetOptionAccessors()
                                    select o;

            foreach (var optionAccessor in optionDefinitions)
            {
                this.options.SetValueByPath(optionAccessor.Path, optionAccessor.DefaultValue);

                var option = this.options.GetAllByPath(optionAccessor.Path).SingleOrDefault();
                if (option != null)
                {
                    option.OptionDefinition = optionAccessor.OptionAttribute;
                    option.DisplayName = optionAccessor.DisplayName;
                    option.Type = optionAccessor.GetOptionTypeFrom(null);
                }
            }
        }

        /// <summary>
        /// Defines the ApplicationOptions class.
        /// </summary>
        private class ApplicationOptions : IOptions, INotifyOptionChanged
        {
            /// <summary>
            /// The current database
            /// </summary>
            private string currentDatabase;

            /// <summary>
            /// The command timeout
            /// </summary>
            private int commandTimeout;

            /// <summary>
            /// Indicates whether the application runs in single instance mode.
            /// </summary>
            private bool singleInstance;

            /// <summary>
            /// Indicates whether queries are transactional.
            /// </summary>
            private bool transactional;

            /// <summary>
            /// Indicates whether the results are editable.
            /// </summary>
            private bool editable;

            /// <summary>
            /// The current directory
            /// </summary>
            private string currentDirectory;

            /// <summary>
            /// The export type
            /// </summary>
            private string exportType;

            /// <summary>
            /// The recent file count
            /// </summary>
            private int recentFileCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="ApplicationOptions"/> class.
            /// </summary>
            public ApplicationOptions()
            {
                Configuration.SetOptionsOn(this);
            }

            /// <summary>
            /// Gets or sets the recent file count.
            /// </summary>
            [Option("RecentFiles/@Count", DefaultValue = "8")]
            public int RecentFileCount
            {
                get
                {
                    return this.recentFileCount;
                }

                set
                {
                    this.recentFileCount = value;
                    this.OnOptionChanged(() => this.RecentFileCount);
                }
            }

            /// <summary>
            /// Gets or sets the type of the export.
            /// </summary>
            /// <value>
            /// The type of the export.
            /// </value>
            [Option("ExportType")]
            public string ExportType
            {
                get
                {
                    return this.exportType;
                }

                set
                {
                    this.exportType = value;
                    this.OnOptionChanged(() => this.ExportType);
                }
            }

            /// <summary>
            /// Gets or sets the current directory.
            /// </summary>
            [Option("CurrentDirectory")]
            public string CurrentDirectory
            {
                get
                {
                    return this.currentDirectory;
                }

                set
                {
                    this.currentDirectory = value;
                    this.OnOptionChanged(() => this.CurrentDirectory);
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether the results are editable.
            /// </summary>
            [Option("Editable", DefaultValue = "True")]
            public bool Editable
            {
                get
                {
                    return this.editable;
                }

                set
                {
                    this.editable = value;
                    this.OnOptionChanged(() => this.Editable);
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether queries should be run in a transaction.
            /// </summary>
            [Option("Transactional", DefaultValue = "True")]
            public bool Transactional
            {
                get
                {
                    return this.transactional;
                }

                set
                {
                    this.transactional = value;
                    this.OnOptionChanged(() => this.Transactional);
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether the application runs in single instance mode.
            /// </summary>
            [Option("SingleInstance", DefaultValue = "True")]
            public bool SingleInstance
            {
                get
                {
                    return this.singleInstance;
                }

                set
                {
                    this.singleInstance = value;
                    this.OnOptionChanged(() => this.SingleInstance);
                }
            }

            /// <summary>
            /// Gets or sets the command timeout.
            /// </summary>
            [Option("CommandTimeout", DefaultValue = "30")]
            public int CommandTimeout
            {
                get
                {
                    return this.commandTimeout;
                }

                set
                {
                    this.commandTimeout = value;
                    this.OnOptionChanged(() => this.CommandTimeout);
                }
            }

            /// <summary>
            /// Gets or sets the current database.
            /// </summary>
            [Option("CurrentDatabase")]
            public string CurrentDatabase
            {
                get
                {
                    return this.currentDatabase;
                }

                set
                {
                    this.currentDatabase = value;
                    this.OnOptionChanged(() => this.CurrentDatabase);
                }
            }
        }
    }
}

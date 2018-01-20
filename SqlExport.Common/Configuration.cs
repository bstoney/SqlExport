namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Monads;
    using System.Threading.Tasks;
    using System.Xml;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common.Options;
    using SqlExport.Messages;

    /// <summary>
    /// Defines the OptionsBase type.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// The settings filename
        /// </summary>
        private const string SettingsFilename = "Setting.3.xml";

        /// <summary>
        /// The current.
        /// </summary>
        private static readonly IOptionLoader OptionsLoader = new OptionsLoader();

        /// <summary>
        /// The settings path.
        /// </summary>
        private static string settingsPath;

        /// <summary>
        /// The auto apply options list.
        /// </summary>
        private static List<WeakReference> autoApplyOptionsList;

        /// <summary>
        /// The current action.
        /// </summary>
        private static ConfigAction currentAction = ConfigAction.None;

        /// <summary>
        /// Defines the ConfigAction enumeration.
        /// </summary>
        private enum ConfigAction
        {
            /// <summary>
            /// The none.
            /// </summary>
            None,

            /// <summary>
            /// The loading
            /// </summary>
            Loading,

            /// <summary>
            /// The saving
            /// </summary>
            Saving
        }

        /// <summary>
        /// Gets the configuration options.
        /// </summary>
        public static IOptions Current
        {
            get
            {
                EnsureOptionsLoaded();
                return OptionsLoader.Options;
            }
        }

        /// <summary>
        /// Gets the settings path.
        /// </summary>
        public static string SettingsPath
        {
            get
            {
                if (string.IsNullOrEmpty(settingsPath))
                {
                    try
                    {
                        settingsPath = Path.Combine(
                            Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Local Settings\Application Data"),
                            ApplicationEnvironment.Default.CompanyName,
                            ApplicationEnvironment.Default.ProductName,
                            ApplicationEnvironment.Default.ProductVersion,
                            SettingsFilename);
                    }
                    catch (Exception)
                    {
                        settingsPath = SettingsFilename;
                    }
                }

                return settingsPath;
            }
        }

        /// <summary>
        /// Gets the recent files.
        /// </summary>
        /// <returns>A list of filenames.</returns>
        public static IEnumerable<string> GetRecentFiles()
        {
            var files = from i in Enumerable.Range(0, Current.RecentFileCount)
                        select GetOptionValue(OptionExtensions.GetIndexedPath("RecentFiles/File", i));

            return files;
        }

        /// <summary>
        /// Sets the recent file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public static void SetRecentFile(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var files = GetRecentFiles().ToList();

                if (files.Contains(filename))
                {
                    files.RemoveAll(f => string.Equals(f, filename, StringComparison.OrdinalIgnoreCase));
                }

                files.Insert(0, filename);

                for (int i = 0; i < Current.RecentFileCount; i++)
                {
                    SetOptionValue(OptionExtensions.GetIndexedPath("RecentFiles/File", i), i >= files.Count ? string.Empty : files[i]);
                }

                Save();
            }
        }

        /// <summary>
        /// Gets the databases.
        /// </summary>
        /// <returns>A list of the databases.</returns>
        public static IEnumerable<DatabaseDetails> GetDatabases()
        {
            var databases = from i in Enumerable.Range(0, Convert.ToInt32(GetOptionValue("DatabaseSettings/Databases/@Count")))
                            let databasePath = OptionExtensions.GetIndexedPath("DatabaseSettings/Databases/Database", i)
                            let namePath = databasePath + "/@Name"
                            let connectionStringPath = databasePath + "/@ConnectionString"
                            let connectionTypePath = databasePath + "/@ConnectionType"
                            select new DatabaseDetails
                            {
                                Name = GetOptionValue(namePath),
                                ConnectionString = GetOptionValue(connectionStringPath),
                                Type = GetOptionValue(connectionTypePath)
                            };

            return databases;
        }

        /// <summary>
        /// Sets the databases.
        /// </summary>
        /// <param name="databases">The databases.</param>
        public static void SetDatabases(List<DatabaseDetails> databases)
        {
            OptionsLoader.RemoveOption("DatabaseSettings/Databases");
            SetOptionValue("DatabaseSettings/Databases/@Count", databases.Count);

            for (var i = 0; i < databases.Count; i++)
            {
                var databasePath = OptionExtensions.GetIndexedPath("DatabaseSettings/Databases/Database", i);
                SetOptionValue(databasePath + "/@Name", databases[i].Name);
                SetOptionValue(databasePath + "/@ConnectionString", databases[i].ConnectionString);
                SetOptionValue(databasePath + "/@ConnectionType", databases[i].Type);
            }
        }

        /// <summary>
        /// Gets the value with the supplied name.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public static string GetOptionValue(string path)
        {
            EnsureOptionsLoaded();
            return OptionsLoader.GetOptionValue(path);
        }

        /// <summary>
        /// Sets the option value.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        public static void SetOptionValue(string path, object value)
        {
            OptionsLoader.SetOptionValue(path, value);
        }

        /// <summary>
        /// Saves the options on the supplied object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void SaveOptionsFrom(object obj)
        {
            if (currentAction != ConfigAction.None)
            {
                return;
            }

            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var type = obj.GetType();
            var options = type.GetOptionAccessors();
            foreach (var option in options)
            {
                SetOptionValue(option.Path, option.GetOptionFrom(obj));
            }

            Task.Factory.StartNew(Save);
        }

        /// <summary>
        /// Saves the options.
        /// </summary>
        public static void SaveOptions()
        {
            Messenger.Default.Send(new OptionsChangedMessage());

            Task.Factory.StartNew(Save);
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        public static void Load()
        {
            EnsureOptionsLoaded();
        }

        /// <summary>
        /// Sets the options on the supplied object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="autoApplyOnOptionsChange">if set to <c>true</c> options changes will be automatically applied to the object.</param>
        /// <exception cref="System.ArgumentNullException">Throw if the object is null.</exception>
        public static void SetOptionsOn(object obj, bool autoApplyOnOptionsChange = true)
        {
            EnsureOptionsLoaded();
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var type = obj.GetType();
            var options = type.GetOptionAccessors();
            foreach (var option in options)
            {
                option.SetOptionOn(obj, GetOptionValue(option.Path));
            }

            if (autoApplyOnOptionsChange)
            {
                if (autoApplyOptionsList == null)
                {
                    autoApplyOptionsList = new List<WeakReference>();
                    Messenger.Default.Register<OptionsChangedMessage>(
                        autoApplyOptionsList,
                        m =>
                        {
                            autoApplyOptionsList.RemoveAll(r => !r.IsAlive);
                            var targets = autoApplyOptionsList.Select(r => r.Target).Distinct().ToArray();
                            foreach (var reference in targets)
                            {
                                reference.Do(r => SetOptionsOn(r, false));
                            }
                        });
                }

                autoApplyOptionsList.Add(new WeakReference(obj));
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        private static void Save()
        {
            if (currentAction != ConfigAction.None)
            {
                return;
            }

            currentAction = ConfigAction.Saving;
            try
            {
                var path = Path.GetDirectoryName(SettingsPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
                OptionsLoader.SaveConfig(doc);

                var settings = new XmlWriterSettings();
                settings.Indent = true;
                using (var writer = XmlWriter.Create(SettingsPath, settings))
                {
                    doc.Save(writer);
                }

                OptionsLoader.LastLoaded = DateTime.Now;
            }
            finally
            {
                currentAction = ConfigAction.None;
            }
        }

        /// <summary>
        /// Ensures the latest options have been loaded.
        /// </summary>
        private static void EnsureOptionsLoaded()
        {
            if (currentAction != ConfigAction.None)
            {
                return;
            }

            currentAction = ConfigAction.Loading;
            try
            {
                if (File.Exists(SettingsPath) && (OptionsLoader.LastLoaded == null || OptionsLoader.LastLoaded < File.GetLastWriteTime(SettingsPath)))
                {
                    if (File.Exists(SettingsPath))
                    {
                        using (var reader = File.OpenText(SettingsPath))
                        {
                            var doc = new XmlDocument();
                            doc.Load(reader);

                            OptionsLoader.LoadConfig(doc);
                        }
                    }

                    OptionsLoader.LastLoaded = DateTime.Now;
                }
            }
            finally
            {
                currentAction = ConfigAction.None;
            }
        }
    }
}

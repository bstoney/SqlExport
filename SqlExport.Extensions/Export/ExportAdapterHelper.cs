namespace SqlExport.Export
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using SqlExport.Common;

    /// <summary>
    /// Defines the ExportAdapterHelper class.
    /// </summary>
    public static class ExportAdapterHelper
    {
        private static Dictionary<string, string> _adapters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        private static Type _exportAdapterType = typeof(ExporterBase);

        static ExportAdapterHelper()
        {
            // Search the available libraries for all the available export adapters.
            LoadAvailableAdapters();
        }

        /// <summary>
        /// Search the available libraries for all the available export adapters.
        /// </summary>
        public static void LoadAvailableAdapters()
        {
            foreach (var path in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(path);
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        if (type != _exportAdapterType && _exportAdapterType.IsAssignableFrom(type))
                        {
                            ExporterBase adapter = GetExportAdapter(type);
                            if (adapter != null)
                            {
                                if (!IsExportAdapterRegistered(adapter.Name))
                                {
                                    RegisterExportAdapter(adapter.Name, type.AssemblyQualifiedName);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        #region Export Adapter Registration

        public static string[] RegisteredExportAdapters
        {
            get
            {
                string[] keys = new string[_adapters.Count];
                _adapters.Keys.CopyTo(keys, 0);
                return keys;
            }
        }

        public static void RegisterExportAdapter(string name, string type)
        {
            _adapters[name] = type;
        }

        public static bool IsExportAdapterRegistered(string name)
        {
            return _adapters.ContainsKey(name);
        }

        public static string GetAdapterType(string key)
        {
            return _adapters[key];
        }

        /// <summary>
        /// Gets the export adapter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A new exporter.</returns>
        public static ExporterBase GetExportAdapter(string name)
        {
            if (!IsExportAdapterRegistered(name))
            {
                throw new SqlExportException("Exported type is either invalid or not set.");
            }
            
            var type = Type.GetType(GetAdapterType(name));
            var adapter = GetExportAdapter(type);

            return adapter;
        }

        private static ExporterBase GetExportAdapter(Type type)
        {
            ExporterBase adapter = null;
            if (type != null)
            {
                adapter = (ExporterBase)Activator.CreateInstance(type);
            }

            return adapter;
        }

        #endregion
    }
}

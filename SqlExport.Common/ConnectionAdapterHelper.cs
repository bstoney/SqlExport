namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Helper class to manage connection adapters.
    /// </summary>
    public static class ConnectionAdapterHelper
    {
        /// <summary>
        /// A collection of connection adapters.
        /// </summary>
        private static readonly Dictionary<string, string> Adapters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// The connection adapter type.
        /// </summary>
        private static readonly Type ConnectionAdapterType = typeof(IConnectionAdapter);

        /// <summary>
        /// Initializes static members of the <see cref="ConnectionAdapterHelper"/> class.
        /// </summary>
        static ConnectionAdapterHelper()
        {
            // Search the available libraries for all the available connection adapters.
            LoadAvailableAdapters();
        }

        #region Connection Adapter Registration

        /// <summary>
        /// Gets a array containing all the registered connection adapters.
        /// </summary>
        public static string[] RegisteredConnectionAdapters
        {
            get
            {
                string[] keys = new string[Adapters.Count];
                Adapters.Keys.CopyTo(keys, 0);
                return keys;
            }
        }

        /// <summary>
        /// Registers a connection adapter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public static void RegisterConnectionAdapter(string name, string type)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            Adapters[name] = type;
        }

        /// <summary>
        /// Indicates whether or not an adapter is registered.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [is connection adapter registered] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsConnectionAdapterRegistered(string name)
        {
            return name != null && Adapters.ContainsKey(name);
        }

        /// <summary>
        /// Gets the type name of the registered adapter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The type of the adapter.</returns>
        public static string GetAdapterType(string name)
        {
            return IsConnectionAdapterRegistered(name) ? Adapters[name] : null;
        }

        /// <summary>
        /// Gets an instance of the registered adapter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The connection adapter.</returns>
        public static IConnectionAdapter GetConnectionAdapter(string name)
        {
            IConnectionAdapter adapter = null;
            if (IsConnectionAdapterRegistered(name))
            {
                Type type = Type.GetType(GetAdapterType(name));
                adapter = GetConnectionAdapter(type);
            }

            return adapter;
        }

        /// <summary>
        /// Gets the connection adapter for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The connection adapter.</returns>
        private static IConnectionAdapter GetConnectionAdapter(Type type)
        {
            IConnectionAdapter adapter = null;
            if (type != null)
            {
                adapter = (IConnectionAdapter)Activator.CreateInstance(type);
            }

            return adapter;
        }

        #endregion

        /// <summary>
        /// Search the available libraries for all the available connection adapters.
        /// </summary>
        private static void LoadAvailableAdapters()
        {
            foreach (var path in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(path);
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        if (type != ConnectionAdapterType && ConnectionAdapterType.IsAssignableFrom(type))
                        {
                            IConnectionAdapter adapter = GetConnectionAdapter(type);
                            if (adapter != null)
                            {
                                if (!IsConnectionAdapterRegistered(adapter.Name))
                                {
                                    RegisterConnectionAdapter(adapter.Name, type.AssemblyQualifiedName);
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
    }
}

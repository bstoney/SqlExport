namespace SqlExport.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using SqlExport.Common.Util;

    /// <summary>
    /// Defines the IExtensionExtensions class.
    /// </summary>
    public static class ExtensionExtensions
    {
        /// <summary>
        /// The extensions
        /// </summary>
        private static IEnumerable<Type> extensions;

        /// <summary>
        /// Gets the extensions.
        /// </summary>
        /// <returns>A list of the available extensions.</returns>
        public static IEnumerable<Type> GetExtensions()
        {
            return extensions
                   ?? (extensions = ReflectionExtensions.GetExportedTypes().WhereImplementsInterface<IExtension>());
        }
    }
}

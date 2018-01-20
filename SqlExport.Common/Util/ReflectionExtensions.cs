namespace SqlExport.Common.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines the ReflectionExtensions class.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Binding flags which define an instance or static public or non-public member.
        /// </summary>
        public static readonly BindingFlags NonPublic = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Binding flags which define an instance or static public member.
        /// </summary>
        public static readonly BindingFlags Public = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Gets the exported types.
        /// </summary>
        /// <param name="publicOnly">if set to <c>true</c> [public only].</param>
        /// <returns>
        /// A list of types.
        /// </returns>
        public static IEnumerable<Type> GetExportedTypes(bool publicOnly = true)
        {
            var libraries = from file in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                            where new[] { ".dll", ".exe" }.Contains(Path.GetExtension(file))
                            from type in file.GetExportedTypes(publicOnly)
                            select type;

            return libraries;
        }

        /// <summary>
        /// Filters the types, returning only those that implement the interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="types">The types.</param>
        /// <returns>A list of types.</returns>
        public static IEnumerable<Type> WhereImplementsInterface<TInterface>(this IEnumerable<Type> types)
        {
            var implementingTypes = from t in types
                                    where t.ImplementsInterface<TInterface>()
                                    select t;

            return implementingTypes;
        }

        /// <summary>
        /// Returns true if the supplied type implements the interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>
        /// true if the type implements the interface; otherwise, false.
        /// </returns>
        public static bool ImplementsInterface<TInterface>(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var interfaceType = typeof(TInterface);

            var interfaces = type.GetInterfaces();
            if (interfaceType.IsGenericTypeDefinition)
            {
                return interfaces.Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == interfaceType);
            }
            
            return interfaces.Any(i => i == interfaceType);
        }

        /// <summary>
        /// Gets a list of members that have the custom attribute.
        /// </summary>
        /// <typeparam name="TMember">The type of the member.</typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type to examine.</param>
        /// <param name="publicOnly">if set to <c>true</c> [public only].</param>
        /// <returns>
        /// The members of the type with the supplied attribute.
        /// </returns>
        public static IEnumerable<TypeMember<TMember, TAttribute>> GetMembersWithCustomAttribute<TMember, TAttribute>(this Type type, bool publicOnly = true)
            where TMember : MemberInfo
            where TAttribute : Attribute
        {
            var mis = type.GetMembers(publicOnly ? Public : NonPublic);

            var typeMembers = from m in mis.OfType<TMember>()
                              let ca = m.GetCustomAttributes(typeof(TAttribute), false).OfType<TAttribute>().ToArray()
                              where ca.Any()
                              select new TypeMember<TMember, TAttribute>(m, ca);

            return typeMembers;
        }

        /// <summary>
        /// Returns true if the supplied object is a numeric type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this object value)
        {
            return value != null && value.GetType().IsNumeric();
        }

        /// <summary>
        /// Returns true if the supplied object is a numeric type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this Type value)
        {
            return value != null
                   && (value == typeof(byte) || value == typeof(sbyte) || value == typeof(decimal)
                       || value == typeof(double) || value == typeof(float) || value == typeof(int)
                       || value == typeof(uint) || value == typeof(long) || value == typeof(ulong)
                       || value == typeof(short) || value == typeof(ushort));
        }


        /// <summary>
        /// Gets the exported types.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="publicOnly">if set to <c>true</c> [public only].</param>
        /// <returns>
        /// A list of types.
        /// </returns>
        private static IEnumerable<Type> GetExportedTypes(this string library, bool publicOnly = true)
        {
            try
            {
                var assembly = Assembly.LoadFile(library);
                return publicOnly ? assembly.GetExportedTypes() : assembly.GetTypes();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Type>();
            }
        }

        /// <summary>
        /// Defines a member returned from TypeHelper.GetMembersWithCustomAttribute method.
        /// </summary>
        /// <typeparam name="TMember">The type of member.</typeparam>
        /// <typeparam name="TAttribute">The type of attributes.</typeparam>
        public sealed class TypeMember<TMember, TAttribute>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TypeMember{TMember, TAttribute}"/> class.
            /// </summary>
            /// <param name="member">The member.</param>
            /// <param name="attributes">The attributes.</param>
            internal TypeMember(TMember member, TAttribute[] attributes)
            {
                this.Member = member;
                this.Attributes = attributes;
            }

            /// <summary>
            /// Gets the type member.
            /// </summary>
            public TMember Member { get; private set; }

            /// <summary>
            /// Gets an array of the member attributes.
            /// </summary>
            public TAttribute[] Attributes { get; private set; }
        }
    }
}

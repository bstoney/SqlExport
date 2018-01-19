namespace SqlExport.Common.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Monads;
    using System.Reflection;
    using System.Text;

    using SqlExport.Common.Extensions;
    using SqlExport.Common.Util;

    /// <summary>
    /// Defines the OptionExtensions class.
    /// </summary>
    public static class OptionExtensions
    {
        /// <summary>
        /// Gets the option accessors.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A list of option accessors.</returns>
        public static IEnumerable<OptionAccessor> GetOptionAccessors(this Type type)
        {
            var options = from m in GetOptionMembers(type)
                          select new OptionAccessor(type, m.Member, m.Attributes.Single());

            return options;
        }

        /// <summary>
        /// Called when an option is changed.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="optionContainer">The option container.</param>
        /// <param name="property">The property.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Expected property expression.
        /// or
        /// Invalid option property.
        /// </exception>
        public static void OnOptionChanged<TValue>(this INotifyOptionChanged optionContainer, Expression<Func<TValue>> property)
        {
            var prop = property.With(p => p.Body) as MemberExpression;
            if (prop == null)
            {
                throw new InvalidOperationException("Expected property expression.");
            }

            var option =
                optionContainer.With(c => c.GetType().GetOptionAccessors())
                               .FirstOrDefault(m => m.MemberName == prop.Member.Name);

            if (option == null)
            {
                throw new InvalidOperationException("Invalid option property.");
            }

            optionContainer.Do(c => Configuration.SetOptionValue(option.Path, option.GetOptionFrom(optionContainer)));
        }

        /// <summary>
        /// Gets the indexed path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="index">The index.</param>
        /// <returns>An option path.</returns>
        internal static string GetIndexedPath(string path, int index)
        {
            return string.Format("{0}[{1}]", path, index + 1);
        }

        /// <summary>
        /// Gets the option members.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A list of option members</returns>
        private static IEnumerable<ReflectionExtensions.TypeMember<MemberInfo, OptionAttribute>> GetOptionMembers(this Type type)
        {
            var options = from m in type.GetMembersWithCustomAttribute<MemberInfo, OptionAttribute>(false)
                          select m;

            return options;
        }

        /// <summary>
        /// Defines the OptionAccessor class.
        /// </summary>
        public class OptionAccessor
        {
            /// <summary>
            /// The member
            /// </summary>
            private readonly MemberInfo member;

            /// <summary>
            /// The option.
            /// </summary>
            private readonly OptionAttribute option;

            /// <summary>
            /// Initializes a new instance of the <see cref="OptionAccessor"/> class.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="member">The member.</param>
            /// <param name="option">The option.</param>
            public OptionAccessor(Type type, MemberInfo member, OptionAttribute option)
            {
                this.member = member;
                this.option = option;

                var path = option.Path;
                if (member.DeclaringType.ImplementsInterface<IExtension>())
                {
                    path = string.Format("Extensions/{0}/{1}", type.Name, option.Path);
                }

                this.Path = path;
            }

            /// <summary>
            /// Gets the path.
            /// </summary>
            public string Path { get; private set; }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            public string DisplayName
            {
                get { return this.option.DisplayName; }
            }

            /// <summary>
            /// Gets the default value.
            /// </summary>
            public string DefaultValue
            {
                get { return this.option.DefaultValue; }
            }

            /// <summary>
            /// Gets the option attribute.
            /// </summary>
            public OptionAttribute OptionAttribute
            {
                get { return this.option; }
            }

            /// <summary>
            /// Gets the name of the member.
            /// </summary>
            /// <value>
            /// The name of the member.
            /// </value>
            public string MemberName
            {
                get { return this.member.Name; }
            }

            /// <summary>
            /// Sets the option on the supplied object.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <param name="stringValue">The value.</param>
            public void SetOptionOn(object obj, string stringValue)
            {
                switch (this.member.MemberType)
                {
                    case MemberTypes.Field:
                        var fi = (FieldInfo)this.member;
                        fi.SetValue(obj, this.option.ConvertToValue(stringValue, fi.FieldType));
                        break;
                    case MemberTypes.Property:
                        var pi = (PropertyInfo)this.member;
                        pi.SetValue(obj, this.option.ConvertToValue(stringValue, pi.PropertyType), null);
                        break;
                }
            }

            /// <summary>
            /// Gets the option from.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>A string.</returns>
            public string GetOptionFrom(object obj)
            {
                object value = null;
                switch (this.member.MemberType)
                {
                    case MemberTypes.Field:
                        var fi = (FieldInfo)this.member;
                        value = fi.GetValue(obj);
                        break;
                    case MemberTypes.Property:
                        var pi = (PropertyInfo)this.member;
                        value = pi.GetValue(obj, null);
                        break;
                }

                return this.option.ConvertToString(value);
            }

            /// <summary>
            /// Gets the option type from.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// An option type.
            /// </returns>
            public virtual OptionType GetOptionTypeFrom(object obj)
            {
                var optionType = typeof(string);
                switch (this.member.MemberType)
                {
                    case MemberTypes.Field:
                        var fi = (FieldInfo)this.member;
                        optionType = fi.FieldType;
                        break;
                    case MemberTypes.Property:
                        var pi = (PropertyInfo)this.member;
                        optionType = pi.PropertyType;
                        break;
                }

                if (optionType == typeof(bool))
                {
                    return OptionType.Boolean;
                }

                if (optionType.IsNumeric())
                {
                    return OptionType.Numeric;
                }

                return this.option.OptionType;
            }
        }
    }
}

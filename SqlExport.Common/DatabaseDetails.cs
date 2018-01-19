namespace SqlExport.Common
{
    using System;

    using SqlExport.Common.Data;
    using SqlExport.Common.Editor;

    /// <summary>
    /// Defines the DatabaseDetails class.
    /// </summary>
    public class DatabaseDetails : IEquatable<DatabaseDetails>
    {
        /// <summary>
        /// The no connection.
        /// </summary>
        public static readonly DatabaseDetails NoConnection = new DatabaseDetails
            {
                Name = NoConnectionAdapter.NoConnectionAdapterName,
                ConnectionString = NoConnectionAdapter.NoConnectionAdapterName,
                Type = NoConnectionAdapter.NoConnectionAdapterName
            };

        /// <summary>
        /// Initializes static members of the <see cref="DatabaseDetails"/> class. 
        /// </summary>
        static DatabaseDetails()
        {
            if (!ConnectionAdapterHelper.IsConnectionAdapterRegistered(NoConnectionAdapter.NoConnectionAdapterName))
            {
                ConnectionAdapterHelper.RegisterConnectionAdapter(
                    NoConnectionAdapter.NoConnectionAdapterName, typeof(NoConnectionAdapter).AssemblyQualifiedName);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        public static bool operator ==(DatabaseDetails x, DatabaseDetails y)
        {
            return object.ReferenceEquals(x, y) || (object)x != null && x.Equals(y);
        }

        /// <summary>
        /// !=s the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool operator !=(DatabaseDetails x, DatabaseDetails y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as DatabaseDetails);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(DatabaseDetails other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other != null)
            {
                return this.Name == other.Name;
            }

            return false;
        }

        /// <summary>
        /// Defines the NoConnectionAdapter class.
        /// </summary>
        private class NoConnectionAdapter : IConnectionAdapter
        {
            /// <summary>
            /// The no connection adapter name.
            /// </summary>
            public const string NoConnectionAdapterName = "<No Connection>";

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get { return NoConnectionAdapterName; }
            }

            /// <summary>
            /// Gets the command adapter.
            /// </summary>
            /// <param name="connectionString">The connection string.</param>
            /// <param name="commandTimeout">The command timeout.</param>
            /// <returns>
            /// A command adapter.
            /// </returns>
            public ICommandAdapter GetCommandAdapter(string connectionString, int commandTimeout)
            {
                return null;
            }

            /// <summary>
            /// Gets the schema adapter.
            /// </summary>
            /// <param name="connectionString">The connection string.</param>
            /// <param name="commandTimeout">The command timeout.</param>
            /// <returns>
            /// A schema adapter.
            /// </returns>
            public ISchemaAdapter GetSchemaAdapter(string connectionString, int commandTimeout)
            {
                return null;
            }

            /// <summary>
            /// Gets the templates.
            /// </summary>
            /// <returns>
            /// A statement template collection.
            /// </returns>
            public StatementTemplateCollection GetTemplates()
            {
                return new StatementTemplateCollection();
            }

            /// <summary>
            /// Gets the syntax definition.
            /// </summary>
            /// <returns>
            /// A syntax definition.
            /// </returns>
            public ISyntaxDefinition GetSyntaxDefinition()
            {
                return new NoConnectionSyntaxDefinition();
            }

            /// <summary>
            /// Defines the NoConnectionSyntaxDefinition class.
            /// </summary>
            private class NoConnectionSyntaxDefinition : ISyntaxDefinition
            {
                /// <summary>
                /// Gets the keyword capitalisation.
                /// </summary>
                public Capitalisation KeywordCapitalisation
                {
                    get { return Capitalisation.Default; }
                }

                /// <summary>
                /// Gets the comment syntax.
                /// </summary>
                public CommentSyntax CommentSyntax
                {
                    get { return CommentSyntax.None; }
                }

                /// <summary>
                /// Gets the keywords.
                /// </summary>
                public string[] Keywords
                {
                    get { return new string[] { }; }
                }

                /// <summary>
                /// Gets the functions.
                /// </summary>
                public string[] Functions
                {
                    get { return new string[] { }; }
                }

                /// <summary>
                /// Gets the data types.
                /// </summary>
                public string[] DataTypes
                {
                    get { return new string[] { }; }
                }

                /// <summary>
                /// Gets the operators.
                /// </summary>
                public string[] Operators
                {
                    get { return new string[] { }; }
                }
            }
        }
    }
}

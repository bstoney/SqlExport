namespace SqlExport.Common.Options
{
    /// <summary>
    /// Defines the OptionBase class.
    /// </summary>
    public abstract class OptionBase
    {
        /// <summary>
        /// The display name
        /// </summary>
        private string displayName;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionBase"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected OptionBase(OptionName name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public OptionName Name { get; private set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName
        {
            get { return this.displayName ?? this.Name.Name; }
            set { this.displayName = value; }
        }

        /// <summary>
        /// Gets the type of the option.
        /// </summary>
        /// <value>
        /// The type of the option.
        /// </value>
        public OptionType Type { get; internal set; }

        /// <summary>
        /// Gets the option definition.
        /// </summary>
        public OptionAttribute OptionDefinition { get; internal set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        public Option Parent { get; internal set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <returns>A path string</returns>
        public abstract OptionPath GetPath();
    }
}
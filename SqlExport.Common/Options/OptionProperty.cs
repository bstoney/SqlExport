namespace SqlExport.Common.Options
{
    /// <summary>
    /// Defines the OptionProperty class.
    /// </summary>
    public class OptionProperty : OptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionProperty" /> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="name">The name.</param>
        internal OptionProperty(Option option, OptionName name)
            : base(name)
        {
            this.Parent = option;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <returns>
        /// A path string
        /// </returns>
        public override OptionPath GetPath()
        {
            return this.Parent.GetPath() + this.Name;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "{0} = {1}",
                this.Name,
                this.Value ?? "(null)");
        }
    }
}

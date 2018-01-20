namespace SqlExport.Common.Options
{
    using System;

    /// <summary>
    /// Defines the OptionName type.
    /// </summary>
    public class OptionName : IEquatable<OptionName>
    {
        /// <summary>
        /// The empty
        /// </summary>
        public static readonly OptionName Empty = new OptionName(string.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isProperty">if set to <c>true</c> [is property].</param>
        internal OptionName(string name, bool isProperty = false)
        {
            this.Name = name;
            this.IsProperty = isProperty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="index">The index.</param>
        internal OptionName(string name, int index)
        {
            this.Name = name;
            this.Index = index;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the index.
        /// </summary>
        public int? Index { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is property.
        /// </summary>
        public bool IsProperty { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(this.Name); }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">Option name A.</param>
        /// <param name="b">Option name B.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(OptionName a, OptionName b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">Option name A.</param>
        /// <param name="b">Option name B.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(OptionName a, OptionName b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.IsProperty
                       ? "@" + this.Name
                       : this.Index > 0 ? this.Name + "[" + (this.Index + 1) + "]" : this.Name;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Name != null ? this.Name.GetHashCode() : 0;
                ////hashCode = (hashCode * 397) ^ this.Index.GetHashCode();
                hashCode = (hashCode * 397) ^ this.IsProperty.GetHashCode();
                return hashCode;
            }
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
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((OptionName)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(OptionName other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Name, other.Name)
                ////&& this.Index == other.Index 
                && this.IsProperty.Equals(other.IsProperty);
        }
    }
}
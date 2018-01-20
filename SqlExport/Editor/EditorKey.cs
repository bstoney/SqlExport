namespace SqlExport.Editor
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the EditorKey class.
    /// </summary>
    public struct EditorKey : IEquatable<EditorKey>
    {
        /// <summary>
        /// The run key
        /// </summary>
        public static readonly EditorKey RunKey = Keys.F5;

        /// <summary>
        /// The alternate run key
        /// </summary>
        public static readonly EditorKey AlternateRunKey = Keys.Control | Keys.T;

        /// <summary>
        /// The save key
        /// </summary>
        public static EditorKey SaveKey = Keys.Control | Keys.S;

        /// <summary>
        /// The next query key
        /// </summary>
        public static EditorKey NextQueryKey = Keys.Control | Keys.Tab;

        /// <summary>
        /// The previous query key
        /// </summary>
        public static EditorKey PreviousQueryKey = Keys.Control | Keys.Shift | Keys.Tab;

        /// <summary>
        /// The find key
        /// </summary>
        public static EditorKey FindKey = Keys.Control | Keys.F;

        /// <summary>
        /// The find key next
        /// </summary>
        public static EditorKey FindKeyNext = Keys.F3;

        /// <summary>
        /// The comment key
        /// </summary>
        public static EditorKey CommentKey = new EditorKey(Keys.Control | Keys.K, Keys.Control | Keys.C);

        /// <summary>
        /// The uncomment key
        /// </summary>
        public static EditorKey UncommentKey = new EditorKey(Keys.Control | Keys.K, Keys.Control | Keys.U);

        /// <summary>
        /// The cancel key
        /// </summary>
        public static EditorKey CancelKey = Keys.Escape;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorKey"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        public EditorKey(Keys key)
            : this(Keys.None, key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorKey"/> struct.
        /// </summary>
        /// <param name="commandKey">The command key.</param>
        /// <param name="key">The key.</param>
        public EditorKey(Keys commandKey, Keys key)
            : this()
        {
            this.CommandKey = commandKey;
            this.Key = key;
        }

        /// <summary>
        /// Gets the command key.
        /// </summary>
        public Keys CommandKey { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public Keys Key { get; private set; }

        /// <summary>
        /// Defines an implicit conversion from the specified keys to a EditorKey.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// A new EditorKey.
        /// </returns>
        public static implicit operator EditorKey(Keys key)
        {
            return new EditorKey(key);
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
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((EditorKey)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(EditorKey other)
        {
            return this.CommandKey == other.CommandKey && this.Key == other.Key;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.CommandKey == Keys.None ? this.Key.ToString() : string.Format("{0} + {1}", this.CommandKey, this.Key);
        }
    }
}

namespace SqlExport.Editor
{
    using System;

    /// <summary>
    /// Defines the TextLine class.
    /// </summary>
    public class TextLine : IEquatable<TextLine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLine" /> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public TextLine(int index, int start, int end)
        {
            this.Index = index;
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the start.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Gets the end.
        /// </summary>
        public int End { get; private set; }

        /// <summary>
        /// Determines if the two TextLines are equal.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A Boolean.</returns>
        public static bool operator ==(TextLine x, TextLine y)
        {
            return object.ReferenceEquals(x, y) || ((object)x != null && x.Equals(y));
        }

        /// <summary>
        /// Determines if the two TextLines are not equal.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A Boolean.</returns>
        public static bool operator !=(TextLine x, TextLine y)
        {
            return !(x == y);
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
            return this.Equals(obj as TextLine);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TextLine other)
        {
            return other != null && (this.Index == other.Index && this.Start == other.Start && this.End == other.End);
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
                var hashCode = this.Index;
                hashCode = (hashCode * 397) ^ this.Start;
                hashCode = (hashCode * 397) ^ this.End;
                return hashCode;
            }
        }
    }
}
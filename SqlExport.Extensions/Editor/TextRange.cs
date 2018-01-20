namespace SqlExport.Editor
{
    using System;
    using System.Linq;
    using System.Monads;

    /// <summary>
    /// Defines the TextRange class.
    /// </summary>
    public class TextRange : IEquatable<TextRange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextRange" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <param name="lines">The lines.</param>
        public TextRange(int start, int length, TextLine[] lines)
        {
            this.Start = start;
            this.Length = length;
            this.Lines = lines ?? new TextLine[] { };
        }

        /// <summary>
        /// Gets the start.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the lines.
        /// </summary>
        public TextLine[] Lines { get; private set; }

        /// <summary>
        /// Determines if the two TextRanges are equal.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A Boolean.</returns>
        public static bool operator ==(TextRange x, TextRange y)
        {
            return object.ReferenceEquals(x, y) || ((object)x != null && x.Equals(y));
        }

        /// <summary>
        /// Determines if the two TextRanges are not equal.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A Boolean.</returns>
        public static bool operator !=(TextRange x, TextRange y)
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
            return this.Equals(obj as TextRange);
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
                var hashCode = this.Start;
                hashCode = (hashCode * 397) ^ this.Length;
                return hashCode;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TextRange other)
        {
            return other != null
                   && (this.Start == other.Start && this.Length == other.Length
                       && this.Lines.Length == other.Lines.Length
                       && Enumerable.Range(0, this.Lines.Length).All(i => this.Lines[i] == other.Lines[i]));
        }
    }
}
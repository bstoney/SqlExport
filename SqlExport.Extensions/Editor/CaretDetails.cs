namespace SqlExport.Editor
{
    using System;
    using System.Linq;
    using System.Monads;

    /// <summary>
    /// Defines the CaretDetails class.
    /// </summary>
    public struct CaretDetails : IEquatable<CaretDetails>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaretDetails" /> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="range">The range.</param>
        /// <param name="horizontalScroll">The horizontal scroll.</param>
        /// <param name="verticalScroll">The vertical scroll.</param>
        public CaretDetails(int location, TextRange range, int horizontalScroll, int verticalScroll)
            : this()
        {
            this.Location = location;
            this.Range = range;
            this.HorizontalScroll = horizontalScroll;
            this.VerticalScroll = verticalScroll;
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public int Location { get; private set; }

        /// <summary>
        /// Gets the range.
        /// </summary>
        public TextRange Range { get; private set; }

        /// <summary>
        /// Gets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public int Start
        {
            get
            {
                return this.Range.With(r => r.Start);
            }
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length
        {
            get
            {
                return this.Range.With(r => r.Length);
            }
        }


        /// <summary>
        /// Gets the start line.
        /// </summary>
        /// <value>
        /// The start line.
        /// </value>
        public int StartLine
        {
            get
            {
                return this.Range.With(r => r.Lines.First().With(l => l.Index));
            }
        }

        /// <summary>
        /// Gets the end line.
        /// </summary>
        /// <value>
        /// The end line.
        /// </value>
        public int EndLine
        {
            get
            {
                return this.Range.With(r => r.Lines.Last().With(l => l.Index));
            }
        }


        /// <summary>
        /// Gets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public int End
        {
            get
            {
                return this.Start + this.Length;
            }
        }

        /// <summary>
        /// Gets the horizontal scroll.
        /// </summary>
        public int HorizontalScroll { get; private set; }

        /// <summary>
        /// Gets the vertical scroll.
        /// </summary>
        public int VerticalScroll { get; private set; }

        /// <summary>
        /// Determines if the two CaretDetails are equal.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A Boolean.</returns>
        public static bool operator ==(CaretDetails x, CaretDetails y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines if the two CaretDetails are not equal.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A Boolean.</returns>
        public static bool operator !=(CaretDetails x, CaretDetails y)
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
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CaretDetails && this.Equals((CaretDetails)obj);
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
                var hashCode = this.Location;
                hashCode = (hashCode * 397) ^ this.Location;
                hashCode = (hashCode * 397) ^ this.Range.With(r => r.GetHashCode());
                hashCode = (hashCode * 397) ^ this.HorizontalScroll;
                hashCode = (hashCode * 397) ^ this.VerticalScroll;
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
        public bool Equals(CaretDetails other)
        {
            return this.Location == other.Location && this.Range == other.Range
                   && this.HorizontalScroll == other.HorizontalScroll && this.VerticalScroll == other.VerticalScroll;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Export.Adapters
{
	public class EncodingInfoWrapper : IComparable
	{
		EncodingInfo _encodingInfo;

		public EncodingInfoWrapper( EncodingInfo encodingInfo )
		{
			_encodingInfo = encodingInfo;
		}

		public string Name
		{
			get { return _encodingInfo.Name; }
		}

		public string DisplayName
		{
			get { return _encodingInfo.DisplayName; }
		}

		public int CodePage
		{
			get { return _encodingInfo.CodePage; }
		}

		public override string ToString()
		{
			return _encodingInfo.DisplayName;
		}

		public static implicit operator EncodingInfoWrapper( string value )
		{
			return new EncodingInfoWrapper( Encoding.GetEncodings().Where( e => string.Compare( e.Name, value, true) == 0 ).First() );
		}

		public static implicit operator string( EncodingInfoWrapper value )
		{
			return value.Name;
		}

		internal static Encoding GetEncoding( EncodingInfoWrapper textEncoding )
		{
			Encoding encoding = null;
			try
			{
				encoding = Encoding.GetEncoding( textEncoding.Name );
			}
			catch( ArgumentException )
			{
				encoding = Encoding.Default;
			}

			return encoding;
		}

		#region IComparable Members

		public int CompareTo( object obj )
		{
			return string.Compare( this.ToString(), obj.ToString(), true );
		}

		#endregion
	}
}

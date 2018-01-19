#define Win32
using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;


namespace SqlExport
{
	[Serializable()]
	public class Colour : System.Runtime.Serialization.ISerializable
	{

		private int _R;
		private int _G;
		private int _B;
		private double _H;
		private double _S;
		private double _V;

		// now red = 0, yellow = PI/3, green = 2*PI/3, cyan = PI, blue = 4*PI/3, purple = 5*PI/3, red = 2PI
		private object[] redmap = new object[] { 1, "-", 0, 0, "+", 1 };
		private object[] greenmap = new object[] { "+", 1, 1, "-", 0, 0 };
		private object[] bluemap = new object[] { 0, 0, "+", 1, 1, "-" };

		public string Name;

		public Colour()
		{
			_R = 0;
			_G = 0;
			_B = 0;

			_H = 0;
			_S = 0;
			_V = 0;
		}
		public Colour( Color color )
		{
			_R = color.R;
			_G = color.G;
			_B = color.B;
			RGBtoHSV();
		}
		public Colour( int Red, int Green, int Blue )
		{
			if( Red < 0 || Red > 255 || Green < 0 || Green > 255 || Blue < 0 || Blue > 255 )
			{
				throw (new ArgumentOutOfRangeException( "Colour values must be between 0 and 255" ));
			}
			this._R = Red;

			this._G = Green;
			this._B = Blue;
			this.RGBtoHSV();
		}
		public Colour( double Hue, double Saturation, double Value )
		{
			if( Saturation < 0 || Saturation > 1 || Value < 0 || Value > 1 )
			{
				throw (new ArgumentOutOfRangeException( "Saturation and Value must be between 0 and 1" ));
			}
			Hue = Hue % 360;
			if( Hue < 0 )
			{
				Hue += 360;
			}
			this._H = Hue;

			this._S = Saturation;
			this._V = Value;
			this.HSVtoRGB();
		}

		public int R
		{
			get { return _R; }
			set
			{
				if( value < 0 || value > 255 )
				{
					throw (new ArgumentOutOfRangeException( "Colour values must be between 0 and 255" ));
				}
				this._R = value;
				this.RGBtoHSV();
			}
		}
		public int G
		{
			get { return _G; }
			set
			{
				if( value < 0 || value > 255 )
				{
					throw (new ArgumentOutOfRangeException( "Colour values must be between 0 and 255" ));
				}
				this._G = value;
				this.RGBtoHSV();
			}
		}
		public int B
		{
			get { return _B; }
			set
			{
				if( value < 0 || value > 255 )
				{
					throw (new ArgumentOutOfRangeException( "Colour values must be between 0 and 255" ));
				}
				this._B = value;
				this.RGBtoHSV();
			}
		}
		public double H
		{
			get
			{
				return this._H;
			}
			set
			{
				value = value % 360;
				if( value < 0 )
				{
					value += 360;
				}
				this._H = value;
				this.HSVtoRGB();
			}
		}
		public double S
		{
			get
			{
				return this._S;
			}
			set
			{
				if( value < 0 || value > 1 )
				{
					throw (new ArgumentOutOfRangeException( "Saturation must be between 0 and 1" ));
				}
				this._S = value;
				this.HSVtoRGB();
			}
		}
		public double V
		{
			get
			{
				return this._V;
			}
			set
			{
				if( value < 0 || value > 1 )
				{
					throw (new ArgumentOutOfRangeException( "Value must be between 0 and 1" ));
				}
				this._V = value;
				this.HSVtoRGB();
			}
		}
		public Color Color
		{
			get { return Color.FromArgb( _R, _G, _B ); }
			set
			{
				_R = value.R;
				_G = value.G;
				_B = value.B;
				RGBtoHSV();
			}
		}
		public string Hex
		{
			get { return "#" + this.R.ToString( "X2" ) + this.G.ToString( "X2" ) + this.B.ToString( "X2" ); }
			set
			{
				value = value.Replace( "#", "" );
				if( value.Length != 6 )
				{
					throw (new ArgumentOutOfRangeException( "Must be 6 characters long." ));
				}
				R = Int32.Parse( value.Substring( 0, 2 ), System.Globalization.NumberStyles.AllowHexSpecifier );
				G = Int32.Parse( value.Substring( 2, 2 ), System.Globalization.NumberStyles.AllowHexSpecifier );
				B = Int32.Parse( value.Substring( 4, 2 ), System.Globalization.NumberStyles.AllowHexSpecifier );
			}
		}

		// copied from http://www.cs.yorku.ca/~yongjian/SAVI/Colour_8cpp-source.html
		private void RGBtoHSV()
		{
			int maxCol = Math.Max( this._R, Math.Max( this._G, this._B ) );
			int minCol = Math.Min( this._R, Math.Min( this._G, this._B ) );

			// Brightness Value
			this._V = maxCol / 255;

			// Calculate saturation
			this._S = maxCol != 0 ? ((maxCol - minCol) / maxCol) : 0;

			// Calculate hue
			if( this._S == 0 )
			{
				this._H = 0;
			}
			else
			{
				double delta = maxCol - minCol;
				double tmpHue;

				if( this._R == maxCol ) // Resulting colour is between yellow and magenta
				{
					tmpHue = (this._G - this._B) / delta;
				}
				else if( this._G == maxCol ) // Resulting colour is between cyan and yellow
				{
					tmpHue = 2.0 + (this._B - this._R) / delta;
				}
				else // Resulting colour is between magenta and cyan
				{
					tmpHue = 4.0 + (this._R - this._G) / delta;
				}

				this._H = tmpHue * 60.0;

				// Make sure the hue is +ve
				if( this._H < 0 )
				{
					this._H += 360;
				}
			}
		}

		private void HSVtoRGB()
		{
			int maxCol = (int)(_V * 255);
			int minCol = (int)(maxCol - _S * maxCol);

			double a = this._H >= 360 ? 0 : this._H / 180 * Math.PI;
			int i = 0;

			while( a > (Math.PI / 3) )
			{
				a -= Math.PI / 3;
				i++;
			}
			double aColour = (maxCol - minCol) / (Math.PI / 3) * a;

			if( redmap[i] is int )
			{
				_R = ((int)redmap[i] == 1) ? maxCol : minCol;
			}
			else
			{
				this._R = System.Convert.ToInt32( ((string)redmap[i] == "+") ? minCol + aColour : maxCol - aColour );
			}
			if( greenmap[i] is int )
			{
				this._G = ((int)greenmap[i] == 1) ? maxCol : minCol;
			}
			else
			{
				this._G = System.Convert.ToInt32( ((string)greenmap[i] == "+") ? minCol + aColour : maxCol - aColour );
			}
			if( bluemap[i] is int )
			{
				this._B = ((int)bluemap[i] == 1) ? maxCol : minCol;
			}
			else
			{
				this._B = System.Convert.ToInt32( ((string)bluemap[i] == "+") ? minCol + aColour : maxCol - aColour );
			}
		}

		//Deserialization constructor.
		public Colour( SerializationInfo info, StreamingContext context )
		{
			_R = System.Convert.ToInt32( info.GetValue( "Red", typeof( int ) ) );
			_G = System.Convert.ToInt32( info.GetValue( "Green", typeof( int ) ) );
			_B = System.Convert.ToInt32( info.GetValue( "Blue", typeof( int ) ) );
			RGBtoHSV();
		}

		//Serialization function.
		public void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "Red", this._R );
			info.AddValue( "Green", this._G );
			info.AddValue( "Blue", this._B );
		}

		public Colour( TextReader reader )
		{
			string str;
			str = reader.ReadLine();
			if( str.IndexOf( "Name=\"", StringComparison.InvariantCultureIgnoreCase ) >= 0 )
			{
				Name = str.Split( '"' )[1];
			}
			R = System.Convert.ToInt32( reader.ReadLine().Split( '"' )[1] );
			G = System.Convert.ToInt32( reader.ReadLine().Split( '"' )[1] );
			B = System.Convert.ToInt32( reader.ReadLine().Split( '"' )[1] );
			reader.ReadLine();
			RGBtoHSV();
		}

		public void WriteObject( TextWriter writer )
		{
			writer.WriteLine( "<Colour" + (Name == "" ? "" : " Name=\"" + Name + "\"") + ">" );
			writer.WriteLine( "<Red value=\"" + _R + "\" />" );
			writer.WriteLine( "<Green value=\"" + _G + "\" />" );
			writer.WriteLine( "<Blue value=\"" + _B + "\" />" );
			writer.WriteLine( "</Colour>" );
		}
	}

}

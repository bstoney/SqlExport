using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SqlExport.Data.Adapters.Linq
{
	/// <summary>
	/// Represents a large integer value.
	/// </summary>
	public class BigInt
	{
		private static readonly Type BIT = Type.GetType( "System.Numeric.BigInteger, System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", true );
		private static readonly MethodInfo _parse = BIT.GetMethod( "Parse", new[] { typeof( string ) } );
		private static readonly MethodInfo _negate = BIT.GetMethod( "Negate" );
		private static readonly MethodInfo _multiply = BIT.GetMethod( "Multiply" );
		private static readonly MethodInfo _divide = BIT.GetMethod( "Divide" );
		private static readonly MethodInfo _add = BIT.GetMethod( "Add" );
		private static readonly MethodInfo _subtract = BIT.GetMethod( "Subtract" );
		private static readonly MethodInfo _mod = BIT.GetMethod( "Remainder" );
		private static readonly MethodInfo _compare = BIT.GetMethod( "Compare" );
		private static readonly MethodInfo _getHashCode = BIT.GetMethod( "GetHashCode" );
		private static readonly MethodInfo _castToLong = BIT.GetMethods()
			.Where( mi => mi.Name == "op_Explicit" && mi.ReturnType.Equals( typeof( long ) ) ).First();

		private static readonly object _one = Activator.CreateInstance( BIT, 1 );

		private object _value;

		public BigInt() : this( 0 ) { }

		public BigInt( long value )
		{
			_value = Activator.CreateInstance( BIT, value );
		}

		private BigInt( object value )
		{
			_value = value;
		}

		public static BigInt Parse( string value )
		{
			return new BigInt( _parse.Invoke( null, new[] { value } ) );
		}

		public static BigInt operator *( BigInt bi1, BigInt bi2 )
		{
			return new BigInt( _multiply.Invoke( null, new[] { bi1._value, bi2._value } ) );
		}

		public static BigInt operator /( BigInt bi1, BigInt bi2 )
		{
			return new BigInt( _divide.Invoke( null, new[] { bi1._value, bi2._value } ) );
		}

		public static BigInt operator %( BigInt bi1, BigInt bi2 )
		{
			return new BigInt( _mod.Invoke( null, new[] { bi1._value, bi2._value } ) );
		}

		public static BigInt operator +( BigInt bi1, BigInt bi2 )
		{
			return new BigInt( _add.Invoke( null, new[] { bi1._value, bi2._value } ) );
		}

		public static BigInt operator -( BigInt bi1, BigInt bi2 )
		{
			return new BigInt( _subtract.Invoke( null, new[] { bi1._value, bi2._value } ) );
		}

		public static BigInt operator ++( BigInt bi1 )
		{
			return new BigInt( _add.Invoke( null, new[] { bi1._value, _one } ) );
		}

		public static BigInt operator --( BigInt bi1 )
		{
			return new BigInt( _subtract.Invoke( null, new[] { bi1._value, _one } ) );
		}

		public static BigInt operator -( BigInt bi1 )
		{
			return new BigInt( _negate.Invoke( null, new[] { bi1._value } ) );
		}

		public static bool operator ==( BigInt bi1, BigInt bi2 )
		{
			return (int)_compare.Invoke( null, new[] { bi1._value, bi2._value } ) == 0;
		}

		public static bool operator !=( BigInt bi1, BigInt bi2 )
		{
			return (int)_compare.Invoke( null, new[] { bi1._value, bi2._value } ) != 0;
		}

		public static bool operator <( BigInt bi1, BigInt bi2 )
		{
			return (int)_compare.Invoke( null, new[] { bi1._value, bi2._value } ) < 0;
		}

		public static bool operator >( BigInt bi1, BigInt bi2 )
		{
			return (int)_compare.Invoke( null, new[] { bi1._value, bi2._value } ) > 0;
		}

		public static bool operator <=( BigInt bi1, BigInt bi2 )
		{
			return (int)_compare.Invoke( null, new[] { bi1._value, bi2._value } ) <= 0;
		}

		public static bool operator >=( BigInt bi1, BigInt bi2 )
		{
			return (int)_compare.Invoke( null, new[] { bi1._value, bi2._value } ) >= 0;
		}

		public static explicit operator long( BigInt bi1 )
		{
			return (long)_castToLong.Invoke( null, new [] { bi1._value } );
		}

		public static implicit operator BigInt( long value )
		{
			return new BigInt( value );
		}

		public override bool Equals( object obj )
		{
			return _value.Equals( obj );
		}

		public override int GetHashCode()
		{
			return (int)_getHashCode.Invoke( _value, new object[] { } );
		}

		public override string ToString()
		{
			return _value.ToString();
		}
	}
}

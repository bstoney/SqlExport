using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace SqlExport.Data.Adapters.Linq
{
	/// <summary>
	/// Extension methods for outputing linq results.
	/// </summary>
	public static class OutputExtensions
	{
		private static List<QueryClient> _clients = new List<QueryClient>();

		/// <summary>
		/// Registers a client to accept output.
		/// </summary>
		internal static void RegisterClient( QueryClient client )
		{
			if( !_clients.Contains( client ) )
			{
				_clients.Add( client );
			}
		}

		/// <summary>
		/// Un registers a client.
		/// </summary>
		internal static void UnregisterClient( QueryClient client )
		{
			if( _clients.Contains( client ) )
			{
				_clients.Remove( client );
			}
		}

		private static void DistributeMessage( string message )
		{
			foreach( QueryClient client in _clients )
			{
				client.AddMessage( message );
			}
		}

		private static void DistributeResult( DataTable result )
		{
			foreach( QueryClient client in _clients )
			{
				client.AddResult( result );
			}
		}

		/// <summary>
		/// Formats an object for output as result data.
		/// </summary>
		public static void Output<T>( this T data ) where T : IEnumerable
		{
			Type elementType = null;
			DataTable result = new DataTable();
			foreach( var value in data )
			{
				if( elementType == null )
				{
					elementType = value.GetType();
					if( elementType.IsGenericType &&
						elementType.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) )
					{
						elementType = Nullable.GetUnderlyingType( elementType );
					}

					if( elementType.IsValueType || elementType == typeof( string ) )
					{
						result.Columns.Add( data.GetType().Name, elementType );
					}
					else
					{
						foreach( PropertyDescriptor prop in TypeDescriptor.GetProperties( value ) )
						{
							Type propertyType = prop.PropertyType;
							if( propertyType.IsGenericType &&
								propertyType.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) )
							{
								propertyType = Nullable.GetUnderlyingType( propertyType );
							}

							result.Columns.Add( prop.Name, propertyType );
						}
					}
				}

				DataRow row = result.NewRow();
				if( elementType.IsValueType || elementType == typeof( string ) )
				{
					row[0] = value ?? DBNull.Value;
				}
				else
				{
					foreach( PropertyDescriptor prop in TypeDescriptor.GetProperties( value ) )
					{
						row[prop.Name] = prop.GetValue( value ) ?? DBNull.Value;
					}
				}

				result.Rows.Add( row );
			}

			DistributeResult( result );
		}

		/// <summary>
		/// Outputs an object as a message.
		/// </summary>
		public static void Print<T>( this T o )
		{
			DistributeMessage( (o != null ? o.ToString() : "(null)") );
		}

		/// <summary>
		/// Outputs an object as a message.
		/// </summary>
		public static void Print<T>( this T o, string formatString, params object[] parameters )
		{
			DistributeMessage( String.Format( formatString, new object[] { o }.Concat( parameters ).ToArray() ) );
		}

		/// <summary>
		/// Outputs a query expression as a message.
		/// </summary>
		public static void PrintExpression<T>( this T query ) where T : IQueryable
		{
			query.Expression.Print();
		}
	}
}

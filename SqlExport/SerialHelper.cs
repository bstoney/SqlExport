using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SqlExport
{
	public class SerialHelper
	{

		// Class mercilessly ripped-off from Dr GUI.Net #3 :)
		public static string SerializeToBase64String( object o )
		{
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream serialMemoryStream = new MemoryStream();
			formatter.Serialize( serialMemoryStream, o );
			byte[] bytes = serialMemoryStream.ToArray();
			return Convert.ToBase64String( bytes ).Trim();
		}

		public static object DeserializeFromBase64String( string base64String )
		{
			BinaryFormatter formatter = new BinaryFormatter();
			base64String = base64String.Trim( '\0' );
			byte[] bytes = Convert.FromBase64String( base64String );
			MemoryStream serialMemoryStream = new MemoryStream( bytes );
			return formatter.Deserialize( serialMemoryStream );
		}
	}
}

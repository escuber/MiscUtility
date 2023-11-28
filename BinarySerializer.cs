using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace CardCells.utility
{
	public class BinarySerializer
	{
		private static readonly BinaryFormatter _serializer = new BinaryFormatter();

		public static BinaryFormatter Instance
		{
			get { return _serializer; }
		}

		public static byte[] BigCompress(object data)
		{
			
			using (var stream = new MemoryStream(340666695))
			{
				Compress(data, stream);
				//BinaryFormatter _serializer = instance;
				_serializer.Serialize(stream, data);
				return stream.ToArray();
			}
			
		}
		public static byte[] Compress(object data)
		{

			using (var stream = new MemoryStream())
			{
				Compress(data, stream);
				//BinaryFormatter _serializer = instance;
				_serializer.Serialize(stream, data);
				return stream.ToArray();
			}

		}
/*		private static void Compress(object data, Stream stream)
		{
			using (var zipStream = new GZipStream(stream, CompressionMode.Compress, true))
			{
				Instance.Serialize(zipStream, data);
			}
		}
		*/
		private static void Compress(object data, Stream stream)
		{
			//using (var zipStream = new GZipStream(stream, CompressionMode.Compress, true))
			//{
			Instance.Serialize(stream, data);
			//}
		}
		//public static object Expand(byte[] data)
		//{
		//	//byte[] compressedFile;
		//	//using (var stream = new MemoryStream())
		//	//{
		//	//    //Compress(data, stream);
		//	//    compressedFile = stream.ToArray();
		//	//}

		//	object xo;
		//	using (var stream = new MemoryStream(data))
		//	{
		//		using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
		//		{
		//			xo = Instance.Deserialize(zipStream);
		//		}
		//	}
		//	return xo;
		//}
		public static object Expand(byte[] data)
		{
			//byte[] compressedFile;
			//using (var stream = new MemoryStream())
			//{
			//    //Compress(data, stream);
			//    compressedFile = stream.ToArray();
			//}

			object xo;
			using (var stream = new MemoryStream(data))
			{
			//	using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
				//{
					xo = Instance.Deserialize(stream);
				//}
			}
			return xo;
		}
		//public static object Expand(Stream stream)
		//{
		//	object data;
		//	using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
		//	{
		//		data = Instance.Deserialize(zipStream);
		//	}
		//	return data;
		//}
	
		public static object Expand(Stream stream)
		{
			object data;
		//	using (var zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
			//{
				data = Instance.Deserialize(stream);
			//}
			return data;
		}
	}
}
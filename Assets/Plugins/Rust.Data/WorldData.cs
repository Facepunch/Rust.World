using System.IO;

namespace ProtoBuf
{
	public partial class WorldData
	{
		public static void Serialize(Stream stream, WorldData data)
		{
			Serializer.Serialize(stream, data);
		}

		public static WorldData Deserialize(Stream stream)
		{
			return Serializer.Deserialize<WorldData>(stream);
		}
	}
}

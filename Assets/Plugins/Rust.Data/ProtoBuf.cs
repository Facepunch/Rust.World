using System.Collections.Generic;

namespace ProtoBuf
{
	[ProtoContract]
	public partial class WorldData
	{
		[ProtoMember(1)] public uint size;
		[ProtoMember(2)] public List<MapData> maps;
		[ProtoMember(3)] public List<PrefabData> prefabs;
		[ProtoMember(4)] public List<PathData> paths;
	}

	[ProtoContract]
	public partial class MapData
	{
		[ProtoMember(1)] public string name;
		[ProtoMember(2)] public byte[] data;
	}

	[ProtoContract]
	public partial class PrefabData
	{
		[ProtoMember(1)] public string category;
		[ProtoMember(2)] public uint id;
		[ProtoMember(3)] public VectorData position;
		[ProtoMember(4)] public VectorData rotation;
		[ProtoMember(5)] public VectorData scale;
	}

	[ProtoContract]
	public partial class PathData
	{
		[ProtoMember(1)] public string name;
		[ProtoMember(2)] public bool spline;
		[ProtoMember(3)] public bool start;
		[ProtoMember(4)] public bool end;
		[ProtoMember(5)] public float width;
		[ProtoMember(6)] public float innerPadding;
		[ProtoMember(7)] public float outerPadding;
		[ProtoMember(8)] public float innerFade;
		[ProtoMember(9)] public float outerFade;
		[ProtoMember(10)] public float randomScale;
		[ProtoMember(11)] public float meshOffset;
		[ProtoMember(12)] public float terrainOffset;
		[ProtoMember(13)] public int splat;
		[ProtoMember(14)] public int topology;
		[ProtoMember(15)] public VectorData[] nodes;
	}

	[ProtoContract]
	public partial class VectorData
	{
		[ProtoMember(1)] public float x;
		[ProtoMember(2)] public float y;
		[ProtoMember(3)] public float z;
	}
}

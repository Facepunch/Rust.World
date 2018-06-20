using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.IO;
using ProtoBuf;
using LZ4;

public class WorldSerialization
{
	public const uint CurrentVersion = 8;

	public uint Version
	{
		get; private set;
	}

	public string Checksum
	{
		get; private set;
	}

	public WorldData world = new WorldData();

	public WorldSerialization()
	{
		Version = CurrentVersion;
		Checksum = null;
	}

	[ProtoContract]
	public class WorldData
	{
		[ProtoMember(1)] public uint size = 4000;
		[ProtoMember(2)] public List<MapData> maps = new List<MapData>();
		[ProtoMember(3)] public List<PrefabData> prefabs = new List<PrefabData>();
		[ProtoMember(4)] public List<PathData> paths = new List<PathData>();
	}

	[ProtoContract]
	public class MapData
	{
		[ProtoMember(1)] public string name;
		[ProtoMember(2)] public byte[] data;
	}

	[ProtoContract]
	public class PrefabData
	{
		[ProtoMember(1)] public string category;
		[ProtoMember(2)] public uint id;
		[ProtoMember(3)] public VectorData position;
		[ProtoMember(4)] public VectorData rotation;
		[ProtoMember(5)] public VectorData scale;
	}

	[ProtoContract]
	public class PathData
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
	public class VectorData
	{
		[ProtoMember(1)] public float x;
		[ProtoMember(2)] public float y;
		[ProtoMember(3)] public float z;

		public VectorData()
		{
		}

		public VectorData(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static implicit operator VectorData(Vector3 v)
		{
			return new VectorData(v.x, v.y, v.z);
		}

		public static implicit operator VectorData(Quaternion q)
		{
			return q.eulerAngles;
		}

		public static implicit operator Vector3(VectorData v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		public static implicit operator Quaternion(VectorData v)
		{
			return Quaternion.Euler(v);
		}
	}

	public MapData GetMap(string name)
	{
		for (int i = 0; i < world.maps.Count; i++)
		{
			if (world.maps[i].name == name) return world.maps[i];
		}
		return null;
	}

	public void AddMap(string name, byte[] data)
	{
		var map = new MapData();

		map.name = name;
		map.data = data;

		world.maps.Add(map);
	}

	public IEnumerable<PrefabData> GetPrefabs(string category)
	{
		return world.prefabs.Where(p => p.category == category);
	}

	public void AddPrefab(string category, uint id, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		var prefab = new PrefabData();

		prefab.category = category;
		prefab.id = id;
		prefab.position = position;
		prefab.rotation = rotation;
		prefab.scale = scale;

		world.prefabs.Add(prefab);
	}

	public IEnumerable<PathData> GetPaths(string name)
	{
		return world.paths.Where(p => p.name.Contains(name));
	}

	public PathData GetPath(string name)
	{
		for (int i = 0; i < world.paths.Count; i++)
		{
			if (world.paths[i].name == name) return world.paths[i];
		}
		return null;
	}

	public void AddPath(PathData path)
	{
		world.paths.Add(path);
	}

	public void Clear()
	{
		world.maps.Clear();
		world.prefabs.Clear();
		world.paths.Clear();

		Version = CurrentVersion;
		Checksum = null;
	}

	public void Save(string fileName)
	{
		try
		{
			using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (var binaryWriter = new BinaryWriter(fileStream))
				{
					binaryWriter.Write(Version);

					using (var compressionStream = new LZ4Stream(fileStream, LZ4StreamMode.Compress))
					{
						Serializer.Serialize(compressionStream, world);
					}
				}
			}

			Checksum = Hash();
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}
	}

	public void Load(string fileName)
	{
		try
		{
			using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var binaryReader = new BinaryReader(fileStream))
				{
					Version = binaryReader.ReadUInt32();

					if (Version == CurrentVersion)
					{
						using (var compressionStream = new LZ4Stream(fileStream, LZ4StreamMode.Decompress))
						{
							world = Serializer.Deserialize<WorldData>(compressionStream);
						}
					}
				}
			}

			Checksum = Hash();
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}
	}

	public void CalculateChecksum()
	{
		Checksum = Hash();
	}

	private string Hash()
	{
		var checksum = new Checksum();

		var heights = GetMap("terrain");
		if (heights != null)
		{
			for (int i = 0; i < heights.data.Length; i++)
			{
				checksum.Add(heights.data[i]);
			}
		}

		var prefabs = world.prefabs;
		if (prefabs != null)
		{
			for (int i = 0; i < prefabs.Count; i++)
			{
				var prefab = prefabs[i];

				checksum.Add(prefab.id);

				// Include the 3 most significant bytes as an approximation
				checksum.Add(prefab.position.x, 3);
				checksum.Add(prefab.position.y, 3);
				checksum.Add(prefab.position.z, 3);
				checksum.Add(prefab.scale.x, 3);
				checksum.Add(prefab.scale.y, 3);
				checksum.Add(prefab.scale.z, 3);
			}
		}

		return checksum.MD5();
	}
}

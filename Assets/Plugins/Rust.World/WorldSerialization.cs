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
	public const uint CurrentVersion = 9;

	public uint Version
	{
		get; private set;
	}

	public WorldData world = new WorldData()
	{
		size = 4000,
		maps = new List<MapData>(),
		prefabs = new List<PrefabData>(),
		paths = new List<PathData>()
	};

	public WorldSerialization()
	{
		Version = CurrentVersion;
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
						WorldData.Serialize(compressionStream, world);
					}
				}
			}
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
							world = WorldData.Deserialize(compressionStream);
						}
					}
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}
	}
}

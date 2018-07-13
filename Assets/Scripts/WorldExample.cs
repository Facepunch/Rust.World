using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;

public class WorldExample : MonoBehaviour
{
	private string bundlename = string.Empty;
	private string filename = string.Empty;
	private string result = string.Empty;

	private WorldSerialization world;
	private PrefabLookup prefabs;

	private WorldSerialization LoadWorld(string filename)
	{
		var blob = new WorldSerialization();

		blob.Load(filename);

		return blob;
	}

	private string GetInfo(WorldSerialization blob)
	{
		// Resolution of the terrain height and water maps
		var meshResolution = Mathf.NextPowerOfTwo((int)(blob.world.size * 0.50f)) + 1;

		// Resolution of the terrain splat, topology, biome and alpha maps
		var textureResolution = Mathf.NextPowerOfTwo((int)(blob.world.size * 0.50f));

		// The dimensions of the terrain object, Y always goes from -500 to +500, X and Z from -extents to +extents
		var terrainSize = new Vector3(blob.world.size, 1000, blob.world.size);

		// The position of the terrain object, chosen so world origin is always at the center of the terrain bounds
		var terrainPosition = -0.5f * terrainSize;

		// Terrain mesh height values (16 bit)
		// Indexed [z, x]
		var terrainMap = new TerrainMap<short>(blob.GetMap("terrain").data, 1);

		// World height values (16 bit)
		// Indexed [z, x]
		// Used to sample the height at which to spawn grass and decor at
		// Can include both terrain and other meshes like for example cliffs
		var heightMap = new TerrainMap<short>(blob.GetMap("height").data, 1);

		// Water map (16 bit)
		// Indexed [z, x]
		// Includes both the ocean plane at zero level and any rivers
		var waterMap = new TerrainMap<short>(blob.GetMap("water").data, 1);

		// Alpha map (8 bit)
		// Indexed [z, x]
		// Zero to render parts of the terrain invisible
		var alphaMap = new TerrainMap<byte>(blob.GetMap("alpha").data, 1);

		// Splat map (8 bit, 8 channels)
		// Indexed [c, z, x] (see TerrainSplat class)
		// Sum of all channels should be normalized to 255
		var splatMap = new TerrainMap<byte>(blob.GetMap("splat").data, 8);

		// Biome map (8 bit, 4 channels)
		// Indexed [c, z, x] (see TerrainBiome class)
		// Sum of all channels should be normalized to 255
		var biomeMap = new TerrainMap<byte>(blob.GetMap("biome").data, 4);

		// Topology map (32 bit)
		// Indexed [z, x] (see TerrainTopology class)
		// Used as a bit mask, multiple topologies can be set in one location
		var topologyMap = new TerrainMap<int>(blob.GetMap("topology").data, 1);

		int x = 0;
		int z = 0;

		var sb = new StringBuilder();

		sb.AppendLine("Info");
		sb.Append("\tPosition: ");
		sb.AppendLine(terrainPosition.ToString());
		sb.Append("\tSize: ");
		sb.AppendLine(terrainSize.ToString());
		sb.Append("\tMesh Resolution: ");
		sb.AppendLine(meshResolution.ToString());
		sb.Append("\tTexture Resolution: ");
		sb.AppendLine(textureResolution.ToString());

		sb.AppendLine();

		sb.AppendLine("Terrain Map");
		sb.Append("\t");
		sb.AppendLine(BitUtility.Short2Float(terrainMap[z, x]).ToString());

		sb.AppendLine();

		sb.AppendLine("Height Map");
		sb.Append("\t");
		sb.AppendLine(BitUtility.Short2Float(heightMap[z, x]).ToString());

		sb.AppendLine();

		sb.AppendLine("Water Map");
		sb.Append("\t");
		sb.AppendLine(BitUtility.Short2Float(waterMap[z, x]).ToString());

		sb.AppendLine();

		sb.AppendLine("Alpha Map");
		sb.Append("\t");
		sb.AppendLine(BitUtility.Byte2Float(alphaMap[z, x]).ToString());

		sb.AppendLine();

		sb.AppendLine("Splat Map");
		sb.Append("\tDirt: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.DIRT_IDX, z, x]).ToString());
		sb.Append("\tSnow: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.SNOW_IDX, z, x]).ToString());
		sb.Append("\tSand: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.SAND_IDX, z, x]).ToString());
		sb.Append("\tRock: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.ROCK_IDX, z, x]).ToString());
		sb.Append("\tGrass: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.GRASS_IDX, z, x]).ToString());
		sb.Append("\tForest: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.FOREST_IDX, z, x]).ToString());
		sb.Append("\tStones: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.STONES_IDX, z, x]).ToString());
		sb.Append("\tGravel: ");
		sb.AppendLine(BitUtility.Byte2Float(splatMap[TerrainSplat.GRAVEL_IDX, z, x]).ToString());

		sb.AppendLine();

		sb.AppendLine("Biome Map");
		sb.Append("\tArid: ");
		sb.AppendLine(BitUtility.Byte2Float(biomeMap[TerrainBiome.ARID_IDX, z, x]).ToString());
		sb.Append("\tTemperate: ");
		sb.AppendLine(BitUtility.Byte2Float(biomeMap[TerrainBiome.TEMPERATE_IDX, z, x]).ToString());
		sb.Append("\tTundra: ");
		sb.AppendLine(BitUtility.Byte2Float(biomeMap[TerrainBiome.TUNDRA_IDX, z, x]).ToString());
		sb.Append("\tArctic: ");
		sb.AppendLine(BitUtility.Byte2Float(biomeMap[TerrainBiome.ARCTIC_IDX, z, x]).ToString());

		sb.AppendLine();

		sb.AppendLine("Topology Map");
		sb.Append("\tField: ");
		sb.AppendLine((topologyMap[z, x] & TerrainTopology.FIELD) != 0 ? "yes" : "no");
		sb.Append("\tBeach: ");
		sb.AppendLine((topologyMap[z, x] & TerrainTopology.BEACH) != 0 ? "yes" : "no");
		sb.Append("\tForest: ");
		sb.AppendLine((topologyMap[z, x] & TerrainTopology.FOREST) != 0 ? "yes" : "no");
		sb.Append("\tOcean: ");
		sb.AppendLine((topologyMap[z, x] & TerrainTopology.OCEAN) != 0 ? "yes" : "no");
		sb.Append("\tLake: ");
		sb.AppendLine((topologyMap[z, x] & TerrainTopology.LAKE) != 0 ? "yes" : "no");

		sb.AppendLine();
		sb.AppendLine("Paths");
		sb.Append("\t");
		sb.Append(blob.world.paths.Count);

		return sb.ToString();
	}

	private void SpawnPrefabs(WorldSerialization blob, PrefabLookup prefabs)
	{
		foreach (var prefab in blob.world.prefabs)
		{
			var go = GameObject.Instantiate(prefabs[prefab.id], prefab.position, prefab.rotation);

			if (go)
			{
				go.transform.localScale = prefab.scale;
				go.SetActive(true);
			}
		}
	}

	protected void OnGUI()
	{
		const float padding = 10;

		GUILayout.BeginArea(new Rect(padding, padding, Screen.width - padding - padding, Screen.height - padding - padding));
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Map File");

			filename = GUILayout.TextField(filename, GUILayout.MinWidth(100));

			#if UNITY_EDITOR
			if (GUILayout.Button("Browse"))
			{
				filename = UnityEditor.EditorUtility.OpenFilePanel("Select Map File", filename, "map");
				world = LoadWorld(filename);
			}
			#endif

			if (GUILayout.Button("Load"))
			{
				world = LoadWorld(filename);
			}

			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (world != null)
		{
			GUILayout.Label("Bundle File");

			bundlename = GUILayout.TextField(bundlename, GUILayout.MinWidth(100));

			#if UNITY_EDITOR
			if (GUILayout.Button("Browse"))
			{
				bundlename = UnityEditor.EditorUtility.OpenFilePanel("Select Bundle File", bundlename, "");

				if (prefabs != null)
				{
					prefabs.Dispose();
					prefabs = null;
				}

				prefabs = new PrefabLookup(bundlename);
			}
			#endif

			if (GUILayout.Button("Load"))
			{
				if (prefabs != null)
				{
					prefabs.Dispose();
					prefabs = null;
				}

				prefabs = new PrefabLookup(bundlename);
			}

			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (world != null && prefabs != null)
		{
			if (prefabs.isLoaded)
			{
				GUILayout.Label("Tools");

				if (GUILayout.Button("Print Map Info")) result = GetInfo(world);
				if (GUILayout.Button("Clear Map Info")) result = string.Empty;
				if (GUILayout.Button("Spawn Map Prefabs")) SpawnPrefabs(world, prefabs);
			}
			else
			{
				GUILayout.Label("Loading Prefabs...");
			}

			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();

		if (!string.IsNullOrEmpty(result)) GUILayout.TextArea(result);

		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}

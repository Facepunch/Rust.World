using System.Collections.Generic;

public static class TerrainSplat
{
	public enum Enum
	{
		Dirt   = 1 << DIRT_IDX,
		Snow   = 1 << SNOW_IDX,
		Sand   = 1 << SAND_IDX,
		Rock   = 1 << ROCK_IDX,
		Grass  = 1 << GRASS_IDX,
		Forest = 1 << FOREST_IDX,
		Stones = 1 << STONES_IDX,
		Gravel = 1 << GRAVEL_IDX,
	}

	public const int COUNT = 8;
	public const int EVERYTHING = ~0;
	public const int NOTHING = 0;

	public const int DIRT   = (int)Enum.Dirt;
	public const int SNOW   = (int)Enum.Snow;
	public const int SAND   = (int)Enum.Sand;
	public const int ROCK   = (int)Enum.Rock;
	public const int GRASS  = (int)Enum.Grass;
	public const int FOREST = (int)Enum.Forest;
	public const int STONES = (int)Enum.Stones;
	public const int GRAVEL = (int)Enum.Gravel;

	public const int DIRT_IDX   = 0;
	public const int SNOW_IDX   = 1;
	public const int SAND_IDX   = 2;
	public const int ROCK_IDX   = 3;
	public const int GRASS_IDX  = 4;
	public const int FOREST_IDX = 5;
	public const int STONES_IDX = 6;
	public const int GRAVEL_IDX = 7;

	private static Dictionary<int, int> type2index = new Dictionary<int, int>() {
		{ ROCK,   ROCK_IDX   },
		{ GRASS,  GRASS_IDX  },
		{ SAND,   SAND_IDX   },
		{ DIRT,   DIRT_IDX   },
		{ FOREST, FOREST_IDX },
		{ STONES, STONES_IDX },
		{ SNOW,   SNOW_IDX   },
		{ GRAVEL, GRAVEL_IDX },
	};

	public static int TypeToIndex(int id)
	{
		return type2index[id];
	}

	public static int IndexToType(int idx)
	{
		return 1 << idx;
	}
}

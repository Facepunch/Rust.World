using System.Collections.Generic;

public static class TerrainBiome
{
	public enum Enum
	{
		Arid      = 1 << ARID_IDX,
		Temperate = 1 << TEMPERATE_IDX,
		Tundra    = 1 << TUNDRA_IDX,
		Arctic    = 1 << ARCTIC_IDX,
	}

	public const int COUNT = 4;
	public const int EVERYTHING = ~0;
	public const int NOTHING = 0;

	public const int ARID      = (int)Enum.Arid;
	public const int TEMPERATE = (int)Enum.Temperate;
	public const int TUNDRA    = (int)Enum.Tundra;
	public const int ARCTIC    = (int)Enum.Arctic;

	public const int ARID_IDX      = 0;
	public const int TEMPERATE_IDX = 1;
	public const int TUNDRA_IDX    = 2;
	public const int ARCTIC_IDX    = 3;

	private static Dictionary<int, int> type2index = new Dictionary<int, int>() {
		{ ARID,      ARID_IDX      },
		{ TEMPERATE, TEMPERATE_IDX },
		{ TUNDRA,    TUNDRA_IDX    },
		{ ARCTIC,    ARCTIC_IDX    },
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

using System.Collections.Generic;

public static class TerrainTopology
{
	public enum Enum
	{
		Field      = 1 << 0,
		Cliff      = 1 << 1,
		Summit     = 1 << 2,
		Beachside  = 1 << 3,
		Beach      = 1 << 4,
		Forest     = 1 << 5,
		Forestside = 1 << 6,
		Ocean      = 1 << 7,
		Oceanside  = 1 << 8,
		Decor      = 1 << 9,
		Monument   = 1 << 10,
		Road       = 1 << 11,
		Roadside   = 1 << 12,
		Swamp      = 1 << 13,
		River      = 1 << 14,
		Riverside  = 1 << 15,
		Lake       = 1 << 16,
		Lakeside   = 1 << 17,
		Offshore   = 1 << 18,
		Powerline  = 1 << 19,
		Runway     = 1 << 20,
		Building   = 1 << 21,
		Cliffside  = 1 << 22,
		Mountain   = 1 << 23,
		Clutter    = 1 << 24,
		Alt        = 1 << 25,
		Tier0      = 1 << 26,
		Tier1      = 1 << 27,
		Tier2      = 1 << 28,
		Mainland   = 1 << 29,
		Hilltop    = 1 << 30,
	}

	public const int COUNT = 31;
	public const int EVERYTHING = ~0;
	public const int NOTHING = 0;

	public const int FIELD      = (int)Enum.Field;
	public const int CLIFF      = (int)Enum.Cliff;
	public const int SUMMIT     = (int)Enum.Summit;
	public const int BEACHSIDE  = (int)Enum.Beachside;
	public const int BEACH      = (int)Enum.Beach;
	public const int FOREST     = (int)Enum.Forest;
	public const int FORESTSIDE = (int)Enum.Forestside;
	public const int OCEAN      = (int)Enum.Ocean;
	public const int OCEANSIDE  = (int)Enum.Oceanside;
	public const int DECOR      = (int)Enum.Decor;
	public const int MONUMENT   = (int)Enum.Monument;
	public const int ROAD       = (int)Enum.Road;
	public const int ROADSIDE   = (int)Enum.Roadside;
	public const int SWAMP      = (int)Enum.Swamp;
	public const int RIVER      = (int)Enum.River;
	public const int RIVERSIDE  = (int)Enum.Riverside;
	public const int LAKE       = (int)Enum.Lake;
	public const int LAKESIDE   = (int)Enum.Lakeside;
	public const int OFFSHORE   = (int)Enum.Offshore;
	public const int POWERLINE  = (int)Enum.Powerline;
	public const int RUNWAY     = (int)Enum.Runway;
	public const int BUILDING   = (int)Enum.Building;
	public const int CLIFFSIDE  = (int)Enum.Cliffside;
	public const int MOUNTAIN   = (int)Enum.Mountain;
	public const int CLUTTER    = (int)Enum.Clutter;
	public const int ALT        = (int)Enum.Alt;
	public const int TIER0      = (int)Enum.Tier0;
	public const int TIER1      = (int)Enum.Tier1;
	public const int TIER2      = (int)Enum.Tier2;
	public const int MAINLAND   = (int)Enum.Mainland;
	public const int HILLTOP    = (int)Enum.Hilltop;

	public const int WATER     = OCEAN | RIVER | LAKE;
	public const int WATERSIDE = OCEANSIDE | RIVERSIDE | LAKESIDE;
	public const int SAND      = OCEAN | OCEANSIDE | LAKE | LAKESIDE | BEACH | BEACHSIDE;
}

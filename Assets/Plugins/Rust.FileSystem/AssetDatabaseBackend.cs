using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR

public class AssetDatabaseBackend : FileSystemBackend
{
	protected override T LoadAsset<T>(string filePath)
	{
		return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(filePath);
	}

	protected override string[] LoadAssetList(string folder, string search)
	{
		var list = new List<string>();

		if (!System.IO.Directory.Exists(folder))
			return list.ToArray();

		var assets = UnityEditor.AssetDatabase.FindAssets("", new[] { folder.TrimEnd( '/' ) }).Select(guid => UnityEditor.AssetDatabase.GUIDToAssetPath(guid)).Distinct();

		foreach (var path in assets)
		{
			if (!string.IsNullOrEmpty(search) && path.IndexOf(search, System.StringComparison.InvariantCultureIgnoreCase) == -1)
				continue;

			list.Add(path);
		}

		return list.ToArray();
	}
}

#endif

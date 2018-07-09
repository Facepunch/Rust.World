using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class FileSystemBackend
{
	public bool isError = false;
	public string loadingError = "";

	public Dictionary<string, Object> cache = new Dictionary<string, Object>();

	public GameObject[] LoadPrefabs(string folder)
	{
		if (!folder.EndsWith("/", System.StringComparison.CurrentCultureIgnoreCase))
		{
			Debug.LogWarning("LoadPrefabs - folder should end in '/' - " + folder);
		}

		if (!folder.StartsWith("assets/", System.StringComparison.CurrentCultureIgnoreCase))
		{
			Debug.LogWarning("LoadPrefabs - should start with assets/ - " + folder);
		}

		return LoadAll<GameObject>(folder, ".prefab");
	}

	public GameObject LoadPrefab(string filePath)
	{
		if (!filePath.StartsWith("assets/", System.StringComparison.CurrentCultureIgnoreCase))
		{
			Debug.LogWarning("LoadPrefab - should start with assets/ - " + filePath);
		}

		return Load<GameObject>(filePath);
	}

	public string[] FindAll(string folder, string search = "")
	{
		return LoadAssetList(folder, search);
	}

	public T[] LoadAll<T>(string folder, string search = "") where T : Object
	{
		var files = new List<T>();

		foreach (var filename in FindAll(folder, search))
		{
			var obj = Load<T>(filename);
			if (obj != null)
				files.Add(obj);
		}

		return files.ToArray();
	}

	public T Load<T>(string filePath) where T : Object
	{
		var val = default(T);

		if (cache.ContainsKey(filePath))
		{
			val = cache[filePath] as T;
		}
		else
		{
			val = LoadAsset<T>(filePath);

			if (val != null)
			{
				cache.Add(filePath, val);
			}
		}

		return val;
	}

	protected void LoadError(string err)
	{
		Debug.LogError(err);
		loadingError = err;
		isError = true;
	} 

	protected abstract T LoadAsset<T>(string filePath) where T : Object;
	protected abstract string[] LoadAssetList(string folder, string search);
}

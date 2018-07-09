using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class HashLookup
{
	private Dictionary<uint, string> uid2str = new Dictionary<uint, string>();
	private Dictionary<string, uint> str2uid = new Dictionary<string, uint>();

	public HashLookup(string text)
	{
		using (var reader = new StringReader(text))
		{
		    string line;
		    while ((line = reader.ReadLine()) != null)
		    {
		        var parts = line.Split(',');

		        if (parts.Length != 3) continue;

		        // 0: Unity GUID
		        // 1: Rust UID
		        // 2: Asset path

		        var uid = uint.Parse(parts[1]);
		        var str = parts[2];

		        uid2str.Add(uid, str);
		        str2uid.Add(str, uid);
		    }
		}
	}

	public string this[uint uid]
	{
		get { return uid2str[uid]; }
	}

	public uint this[string str]
	{
		get { return str2uid[str]; }
	}
}

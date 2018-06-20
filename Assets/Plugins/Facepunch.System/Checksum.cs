using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

public class Checksum
{
	private List<byte> values = new List<byte>();

	public void Add(float f, int bytes)
	{
		var v = new Union32();
		v.f = f;
		if (bytes >= 4) values.Add(v.b1);
		if (bytes >= 3) values.Add(v.b2);
		if (bytes >= 2) values.Add(v.b3);
		if (bytes >= 1) values.Add(v.b4);
	}

	public void Add(float f)
	{
		var v = new Union32();
		v.f = f;
		values.Add(v.b1);
		values.Add(v.b2);
		values.Add(v.b3);
		values.Add(v.b4);
	}

	public void Add(int i)
	{
		var v = new Union32();
		v.i = i;
		values.Add(v.b1);
		values.Add(v.b2);
		values.Add(v.b3);
		values.Add(v.b4);
	}

	public void Add(uint u)
	{
		var v = new Union32();
		v.u = u;
		values.Add(v.b1);
		values.Add(v.b2);
		values.Add(v.b3);
		values.Add(v.b4);
	}

	public void Add(short i)
	{
		var v = new Union16();
		v.i = i;
		values.Add(v.b1);
		values.Add(v.b2);
	}

	public void Add(ushort u)
	{
		var v = new Union16();
		v.u = u;
		values.Add(v.b1);
		values.Add(v.b2);
	}

	public void Add(byte b)
	{
		values.Add(b);
	}

	public void Clear()
	{
		values.Clear();
	}

	public string MD5()
	{
		var hashFunc = new MD5CryptoServiceProvider();
		var hashBytes = hashFunc.ComputeHash(values.ToArray());
		return BytesToString(hashBytes);
	}

	public string SHA1()
	{
		var hashFunc = new SHA1CryptoServiceProvider();
		var hashBytes = hashFunc.ComputeHash(values.ToArray());
		return BytesToString(hashBytes);
	}

	public override string ToString()
	{
		return BytesToString(values.ToArray());
	}

	private string BytesToString(byte[] bytes)
	{
		var sb = new StringBuilder();

		for (int x = 0; x < bytes.Length; x++)
		{
			sb.Append(bytes[x].ToString("x2"));
		}

		return sb.ToString();
	}
}

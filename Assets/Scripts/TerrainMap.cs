using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class TerrainMap<T> where T : struct
{
	public int res;
	public T[] src;
	public T[] dst;

	public T this[int z, int x]
	{
		get { return src[z * res + x]; }
		set { dst[z * res + x] = value; }
	}

	public T this[int c, int z, int x]
	{
		get { return src[(c * res + z) * res + x]; }
		set { dst[(c * res + z) * res + x] = value; }
	}

	public TerrainMap(byte[] data, int channels)
	{
		res = Mathf.RoundToInt(Mathf.Sqrt(data.Length / BytesPerElement() / channels));
		src = dst = new T[channels * res * res];

		FromByteArray(data);
	}

	public TerrainMap(int size, int channels)
	{
		res = size;
		src = dst = new T[channels * res * res];
	}

	public void Push()
	{
		if (src != dst) return;

		dst = (T[])src.Clone();
	}

	public void Pop()
	{
		if (src == dst) return;

		Array.Copy(dst, src, src.Length);
		dst = src;
	}

	public int BytesPerElement()
	{
		return Marshal.SizeOf(typeof(T));
	}

	public int BytesTotal()
	{
		return BytesPerElement() * src.Length;
	}

	public byte[] ToByteArray()
	{
		var dat = new byte[BytesTotal()];
		ToByteArray(dat);
		return dat;
	}

	public void ToByteArray(byte[] dat)
	{
		Buffer.BlockCopy(src, 0, dat, 0, dat.Length);
	}

	public void FromByteArray(byte[] dat)
	{
		Buffer.BlockCopy(dat, 0, dst, 0, dat.Length);
	}
}

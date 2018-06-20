using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct Union64
{
	[FieldOffset(0)]
	public long i;
	[FieldOffset(0)]
	public ulong u;
	[FieldOffset(0)]
	public double f;
	[FieldOffset(0)]
	public byte b1;
	[FieldOffset(1)]
	public byte b2;
	[FieldOffset(2)]
	public byte b3;
	[FieldOffset(3)]
	public byte b4;
	[FieldOffset(4)]
	public byte b5;
	[FieldOffset(5)]
	public byte b6;
	[FieldOffset(6)]
	public byte b7;
	[FieldOffset(7)]
	public byte b8;
}

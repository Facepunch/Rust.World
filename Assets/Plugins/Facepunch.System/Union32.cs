using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct Union32
{
	[FieldOffset(0)]
	public int i;
	[FieldOffset(0)]
	public uint u;
	[FieldOffset(0)]
	public float f;
	[FieldOffset(0)]
	public byte b1;
	[FieldOffset(1)]
	public byte b2;
	[FieldOffset(2)]
	public byte b3;
	[FieldOffset(3)]
	public byte b4;
}

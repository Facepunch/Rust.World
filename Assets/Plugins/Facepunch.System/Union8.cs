using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct Union8
{
	[FieldOffset(0)]
	public sbyte i;
	[FieldOffset(0)]
	public byte u;
	[FieldOffset(0)]
	public byte b1;
}

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct Union16
{
	[FieldOffset(0)]
	public short i;
	[FieldOffset(0)]
	public ushort u;
	[FieldOffset(0)]
	public byte b1;
	[FieldOffset(1)]
	public byte b2;
}

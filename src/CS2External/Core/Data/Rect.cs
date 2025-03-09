using System.Runtime.InteropServices;

namespace CS2External.Core.Data;

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    public int Left, Top, Right, Bottom;
}
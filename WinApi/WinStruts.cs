using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CSharpLib.WinApi
{
    public class WinStruts
    {
        public struct INPUT
        {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }
        public struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
#pragma warning restore 649

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public Rect(Rect Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
            {
            }
            public Rect(int Left, int Top, int Right, int Bottom)
            {
                X = Left;
                Y = Top;
                this.Right = Right;
                this.Bottom = Bottom;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Left { get => X; set => X = value; }

            public int Top { get => Y; set => Y = value; }

            public int Right { get; set; }
            public int Bottom { get; set; }

            public int Height
            {
                get { return Bottom - Y; }
                set { Bottom = value + Y; }
            }
            public int Width
            {
                get { return Right - X; }
                set { Right = value + X; }
            }
            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }
            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    Right = value.Width + X;
                    Bottom = value.Height + Y;
                }
            }

            public static implicit operator Rectangle(Rect Rectangle)
            {
                return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
            }
            public static implicit operator Rect(Rectangle Rectangle)
            {
                return new Rect(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            }
            public static bool operator ==(Rect Rectangle1, Rect Rectangle2)
            {
                return Rectangle1.Equals(Rectangle2);
            }
            public static bool operator !=(Rect Rectangle1, Rect Rectangle2)
            {
                return !Rectangle1.Equals(Rectangle2);
            }

            public override string ToString()
            {
                return "{Left: " + X + "; " + "Top: " + Y + "; Right: " + Right + "; Bottom: " + Bottom + "}";
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public bool Equals(Rect Rectangle)
            {
                return Rectangle.Left == X && Rectangle.Top == Y && Rectangle.Right == Right && Rectangle.Bottom == Bottom;
            }

            public override bool Equals(object Object)
            {
                if (Object is Rect)
                {
                    return Equals((Rect)Object);
                }
                else if (Object is Rectangle)
                {
                    return Equals(new Rect((Rectangle)Object));
                }

                return false;
            }
        }
    }
}

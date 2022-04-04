using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public struct ColorInt
    {
        public int r;
        public int g;
        public int b;
        public int a;

        public ColorInt(int r, int g, int b, int a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public override string ToString()
        {
            return $"({r}, {g}, {b}, {a})";
        }
    }
}

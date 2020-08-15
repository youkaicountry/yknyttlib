using System;

namespace YKnyttLib
{
    public struct KnyttPoint
    {
        public readonly int x, y;
        public int Area 
        { 
            get { return x * y; }
        }

        public KnyttPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public KnyttPoint min(KnyttPoint point)
        {
            return new KnyttPoint(Math.Min(point.x, x), Math.Min(point.y, y));
        }

        public KnyttPoint max(KnyttPoint point)
        {
            return new KnyttPoint(Math.Max(point.x, x), Math.Max(point.y, y));
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", this.x, this.y);
        }
    }
}

using System;
using System.Collections.Generic;

namespace FactoryLogic
{



    public class Box : IComparable<Box>
    {
        public BoxSize BoxSize { get; set; }
        public int Count { get; set; }
        public DateTime LastPurshaceDate { get; set; }

        public Box(double width, double height, int count = 0)
        {
            BoxSize = new BoxSize(width, height);
            Count = count;
            LastPurshaceDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Width: {BoxSize.Width}, Height: {BoxSize.Height}, Total Count: {this.Count}";
        }

        public int CompareTo(Box other)
        {
            if (this.BoxSize.Width > other.BoxSize.Width)
            {
                return 1;
            }
            if (this.BoxSize.Width < other.BoxSize.Width)
            {
                return -1;
            }
            else // both boxes have the same width 
                return this.BoxSize.Height.CompareTo(other.BoxSize.Height);
        }

    }

}

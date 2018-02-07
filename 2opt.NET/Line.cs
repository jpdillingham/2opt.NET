using System;
using System.Collections.Generic;
using System.Text;

namespace _2opt.NET
{
    public class Line
    {
        public Point[] Points { get; set; }

        public Line()
        {
            Points = new Point[2];
        }

        public override string ToString()
        {
            return $"{Points[0]} {Points[1]}";
        }
    }
}

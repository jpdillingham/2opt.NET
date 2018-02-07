using System;

namespace _2opt.NET
{
    class Program
    {
        Random rng = new Random();

        private static int PointCount { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine(PointCount);
        }
    }

    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point()
        {
            X = 0.0;
            Y = 0.0;
        }
    }

    class Line
    {
        public Point[] Points { get; set; }

        public Line()
        {
            Points = new Point[2];
        }
    }
}

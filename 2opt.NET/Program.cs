using System;
using System.Collections.Generic;

namespace _2opt.NET
{
    class Program
    {
        private static Random rng = new Random();

        private static int PointCount = 5;
        private static int XLim = 10;
        private static int YLim = 10;
    
        private static List<Point> Points { get; set; }
        private static List<Line> Lines { get; set; }

        static void Main(string[] args)
        {
            ParseArgs(args);

            Console.WriteLine($"Generating polygon with {PointCount} points and bounded size ({XLim}, {YLim})");

            Points = new List<Point>();

            for (int i = 0; i < PointCount; i++)
            {
                var point = new Point()
                {
                    X = rng.NextDouble() * XLim,
                    Y = rng.NextDouble() * YLim,
                };

                Points.Add(point);

                Console.WriteLine($"Added point {point}");
            }

            Console.WriteLine($"Un-2opted SQL: {GetSQL(Points)}");

            Lines = new List<Line>();

            for (int i = 0; i < Points.Count; i++)
            {
                var line = new Line()
                {
                    Points = new[] { Points[i], Points[i + 1 == Points.Count ? 0 : i + 1] }
                };

                Lines.Add(line);

                Console.WriteLine($"Added line {line}");
            }
        }

        private static string GetSQL(List<Point> points)
        {
            string sql = "SELECT geometry::STGeomFromText('POLYGON((";

            foreach (Point point in points)
            {
                sql += $"{point.X} {point.Y}, ";
            }

            sql += $"{points[0].X} {points[0].Y}))', 4326)";

            return sql;
        }

        private static void ParseArgs(string[] args)
        {
            ParseArg(args, 0, out PointCount, nameof(PointCount), 5);
            ParseArg(args, 1, out XLim, nameof(XLim), 10);
            ParseArg(args, 2, out YLim, nameof(YLim), 10);
        }

        private static void ParseArg(string[] args, int index, out int target, string targetName, int def)
        {
            try
            {
                target = Int32.Parse(args[index]);
            }
            catch
            {
                Console.WriteLine($"WARN: invalid parameter for {targetName}; defaulting to {def}.");
                target = def;
            }
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

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    class Line
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

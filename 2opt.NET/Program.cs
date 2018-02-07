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

            Console.WriteLine($"Un-2opted SQL: {Utility.GetSQL(Points)}");

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
        
        private static void ParseArgs(string[] args)
        {
            Utility.ParseArg(args, 0, out PointCount, nameof(PointCount), 5);
            Utility.ParseArg(args, 1, out XLim, nameof(XLim), 10);
            Utility.ParseArg(args, 2, out YLim, nameof(YLim), 10);
        }
    }
}

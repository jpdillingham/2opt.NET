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

        private static List<Tuple<Line, Line>> IntersectingLines { get; set; }

        static void Main(string[] args)
        {
            ParseArgs(args);

            Console.WriteLine($"Generating polygon with {PointCount} points and bounded size ({XLim}, {YLim})\n");

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

            Console.WriteLine($"\nUn-2opted SQL: {Utility.GetSQL(Points)}\n");

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

            Console.WriteLine("\nChecking for intersecting lines...");

            IntersectingLines = new List<Tuple<Line, Line>>();

            for (int i = 0; i < Lines.Count; i++)
            {
                for (int j = i + 1; j < Lines.Count; j++)
                {

                    if (Lines[i].Points[0] == Lines[j].Points[0] || Lines[i].Points[0] == Lines[j].Points[1] || Lines[i].Points[1] == Lines[j].Points[0] || Lines[i].Points[1] == Lines[j].Points[1])
                    {
                        continue;
                    }

                    if (Utility.LineSegementsIntersect(Lines[i].Points[0], Lines[i].Points[1], Lines[j].Points[0], Lines[j].Points[1]))
                    {
                        var intersectingLines = new Tuple<Line, Line>(Lines[i], Lines[j]);
                        IntersectingLines.Add(intersectingLines);

                        Console.WriteLine($"Intersecting lines: {Lines[i]} {Lines[j]}");
                    }
                }
            }

            Console.WriteLine($"\n{IntersectingLines.Count} intersecting line pair(s) found.");

            if (IntersectingLines.Count > 0)
            {
                Console.WriteLine("\nChoosing random line pair and mutating...");

                // do stuff
            }
            else
            {
                Console.WriteLine("Done!");
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

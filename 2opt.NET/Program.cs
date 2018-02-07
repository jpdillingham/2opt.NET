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

            Points = GetRandomizedPoints(PointCount, XLim, YLim);

            string originalSQL = Utility.GetSQL(Points);

            int maxIterations = (int)Math.Pow(PointCount, 3) + 1; // double check this

            while (maxIterations > 0)
            {
                Lines = GetLinesFromPoints(Points);
                IntersectingLines = GetIntersectingLines(Lines);

                if (IntersectingLines.Count == 0)
                {
                    break;
                }

                Points = MutateIntersectingLines(Points, IntersectingLines);
                maxIterations--;
            }

            if (maxIterations == 0)
            {
                Console.WriteLine("\nOperation broke; maximum number of iterations reached.");
            }
            else
            {
                Console.WriteLine($"\n{originalSQL}\n{Utility.GetSQL(Points)}\n");
                Console.WriteLine("\nDone!");
            }
        }

        private static List<Point> MutateIntersectingLines(List<Point> points, List<Tuple<Line, Line>> intersectingLines)
        {
            Console.WriteLine("\nChoosing random line pair and mutating...");
            var pairIndex = rng.Next(intersectingLines.Count);

            Tuple<Line, Line> pair = intersectingLines[pairIndex];

            Point left = pair.Item1.Points[0];
            Point right = pair.Item2.Points[1];

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == left)
                {
                    points[i] = right;
                }
                else if (points[i] == right)
                {
                    points[i] = left;
                }
            }

            return points;
        }

        private static List<Tuple<Line, Line>> GetIntersectingLines(List<Line> lines)
        {
            Console.WriteLine("\nChecking for intersecting lines...");

            var intersectingLines = new List<Tuple<Line, Line>>();

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = i + 1; j < lines.Count; j++)
                {

                    if (lines[i].Points[0] == lines[j].Points[0] || lines[i].Points[0] == lines[j].Points[1] || lines[i].Points[1] == lines[j].Points[0] || lines[i].Points[1] == lines[j].Points[1])
                    {
                        continue;
                    }

                    if (Utility.LineSegementsIntersect(Lines[i].Points[0], Lines[i].Points[1], Lines[j].Points[0], Lines[j].Points[1]))
                    {
                        var intersectingPair = new Tuple<Line, Line>(Lines[i], Lines[j]);
                        intersectingLines.Add(intersectingPair);

                        Console.WriteLine($"Intersecting lines: {Lines[i]} {Lines[j]}");
                    }
                }
            }

            Console.WriteLine($"\n{intersectingLines.Count} intersecting line pair(s) found.");

            return intersectingLines;
        }

        private static List<Point> GetRandomizedPoints(int count, int xlim, int ylim)
        {
            Console.WriteLine($"Generating polygon with {count} points and bounded size ({xlim}, {ylim})\n");

            var points = new List<Point>();

            for (int i = 0; i < count; i++)
            {
                var point = new Point()
                {
                    X = rng.NextDouble() * xlim,
                    Y = rng.NextDouble() * ylim,
                };

                points.Add(point);

                Console.WriteLine($"Added point {point}");
            }

            return points;
        }

        private static List<Line> GetLinesFromPoints(List<Point> points)
        {
            var lines = new List<Line>();

            for (int i = 0; i < points.Count; i++)
            {
                var line = new Line()
                {
                    Points = new[] { points[i], points[i + 1 == points.Count ? 0 : i + 1] }
                };

                lines.Add(line);

                Console.WriteLine($"Added line {line}");
            }

            return lines;
        }
        
        private static void ParseArgs(string[] args)
        {
            Utility.ParseArg(args, 0, out PointCount, nameof(PointCount), 5);
            Utility.ParseArg(args, 1, out XLim, nameof(XLim), 10);
            Utility.ParseArg(args, 2, out YLim, nameof(YLim), 10);
        }
    }
}

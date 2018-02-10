﻿using System;
using System.Collections.Generic;

namespace _2opt.NET
{
    class Program
    {
        private static Random rng = new Random();

        private static int pointCount = 5;
        private static int xLim = 10;
        private static int yLim = 10;
        private static int maxUnproductiveIterations = 1000;

        private static int previousIntersectionCount = 0;
        private static int unproductiveIterationCount = 0;
        private static int iterationCount = 0;
    
        private static List<Point> Points { get; set; }
        private static List<Line> Lines { get; set; }

        private static List<Tuple<Line, Line>> IntersectingLines { get; set; }

        static void Main(string[] args)
        {
            ParseArgs(args);

            Points = GetRandomizedPoints(pointCount, xLim, yLim);

            string originalSQL = Utility.GetSQL(Points);

            while (true)
            {
                Lines = GetLinesFromPoints(Points);
                IntersectingLines = GetIntersectingLines(Lines);
                
                if (IntersectingLines.Count == 0 || unproductiveIterationCount > maxUnproductiveIterations)
                {
                    break;
                }

                if (IntersectingLines.Count >= previousIntersectionCount)
                {
                    unproductiveIterationCount++;
                }

                previousIntersectionCount = IntersectingLines.Count;
                iterationCount++;

                Points = MutateIntersectingLines(Points, IntersectingLines);
            }

            if (IntersectingLines.Count > 0)
            {
                Console.WriteLine($"\nUnable to remove all intersections after {iterationCount} iterations.");
            }
            else
            {
                Console.WriteLine($"\nIntersections removed! Completed in {iterationCount} iterations.");
            }

            Console.WriteLine($"\n{originalSQL}\n{Utility.GetSQL(Points)}\n");
        }

        private static List<Point> MutateIntersectingLines(List<Point> points, List<Tuple<Line, Line>> intersectingLines)
        {
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
                    }
                }
            }

            return intersectingLines;
        }

        private static List<Point> GetRandomizedPoints(int count, int xlim, int ylim)
        {
            var points = new List<Point>();

            while (points.Count < count)
            {
                var point = new Point()
                {
                    X = rng.NextDouble() * xlim,
                    Y = rng.NextDouble() * ylim,
                };

                if (!points.Contains(point))
                {
                    points.Add(point);
                }
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
            }

            return lines;
        }
        
        private static void ParseArgs(string[] args)
        {
            Utility.ParseArg(args, 0, out pointCount, nameof(pointCount), 5);
            Utility.ParseArg(args, 1, out xLim, nameof(xLim), 10);
            Utility.ParseArg(args, 2, out yLim, nameof(yLim), 10);
            Utility.ParseArg(args, 3, out maxUnproductiveIterations, nameof(maxUnproductiveIterations), 1000);
        }
    }
}

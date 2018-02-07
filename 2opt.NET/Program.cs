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

        /// <summary>
        /// Test whether two line segments intersect. If so, calculate the intersection point.
        /// <see cref="http://stackoverflow.com/a/14143738/292237"/>
        /// </summary>
        /// <param name="p">Vector to the start point of p.</param>
        /// <param name="p2">Vector to the end point of p.</param>
        /// <param name="q">Vector to the start point of q.</param>
        /// <param name="q2">Vector to the end point of q.</param>
        /// <param name="intersection">The point of intersection, if any.</param>
        /// <param name="considerOverlapAsIntersect">Do we consider overlapping lines as intersecting?
        /// </param>
        /// <returns>True if an intersection point was found.</returns>
        public static bool LineSegementsIntersect(Point p, Point p2, Point q, Point q2,
            out Point intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Point();

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross(s);
            var qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                    if ((0 <= (q - p) * r && (q - p) * r <= r * r) || (0 <= (p - q) * s && (p - q) * s <= s * s))
                        return true;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero() && !qpxr.IsZero())
                return false;

            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross(s) / rxs;

            // u = (q - p) x r / (r x s)

            var u = (q - p).Cross(r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + t * r;

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }
    }


    public class Point
    {
        public double X;
        public double Y;

        public Point(double x, double y) { X = x; Y = y; }
        public Point() : this(double.NaN, double.NaN) { }

        public static Point operator -(Point v, Point w)
        {
            return new Point(v.X - w.X, v.Y - w.Y);
        }

        public static Point operator +(Point v, Point w)
        {
            return new Point(v.X + w.X, v.Y + w.Y);
        }

        public static double operator *(Point v, Point w)
        {
            return v.X * w.X + v.Y * w.Y;
        }

        public static Point operator *(Point v, double mult)
        {
            return new Point(v.X * mult, v.Y * mult);
        }

        public static Point operator *(double mult, Point v)
        {
            return new Point(v.X * mult, v.Y * mult);
        }

        public double Cross(Point v)
        {
            return X * v.Y - Y * v.X;
        }

        public override bool Equals(object obj)
        {
            var v = (Point)obj;
            return (X - v.X).IsZero() && (Y - v.Y).IsZero();
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public static class Extensions
    {
        private const double Epsilon = 1e-10;

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
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

using System;
using System.Collections.Generic;
using System.Text;

namespace _2opt.NET
{
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
}

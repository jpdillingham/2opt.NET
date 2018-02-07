using System;
using Xunit;

namespace _2opt.NET.Tests
{
    public class Tests
    {
        [Fact]
        public void One()
        {
            var p1 = new Point() { X = 0.0, Y = 0.0 };
            var p2 = new Point() { X = 5.0, Y = 5.0 };
            var p3 = new Point() { X = 0.0, Y = 5.0 };
            var p4 = new Point() { X = 5.0, Y = 0.0 };

            var result = Utility.LineSegementsIntersect(p1, p2, p3, p4, true);

            Assert.True(result);
        }
    }
}

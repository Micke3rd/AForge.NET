using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AForge.Math.Geometry.Tests
{
    [TestClass]
    public class LineTest
    {
        private const float Error = 0.00001f;

        [DataTestMethod]
        [DataRow(1, 1, 45, 1.41421356f, -1, 2)]
        [DataRow(-2, 2, 135, 2*1.41421356f, 1, 4)]
        [DataRow(-0.5f, -1.73205081f/2, 240, 1, -1/1.73205081f, -2/1.73205081f)]
        [DataRow(1, 0, 0, 1, float.NegativeInfinity, 1)]
        [DataRow(0, -1, 270, 1, 0, -1)]
        public void RThetaTest(float x, float y, float theta, float expectedRadius, float expectedSlope, float expectedIntercept)
        {
            var pt = new Point(x, y);

            // test Point-Theta factory
            var line = Line.FromPointTheta(pt, theta);
            Assert.AreEqual(expectedSlope, line.Slope, Error);
            Assert.AreEqual(expectedIntercept, line.Intercept, Error);

            // calculate radius
            var radius = pt.EuclideanNorm();
            Assert.AreEqual(expectedRadius, radius, Error);

            // test R-Theta factory
            line=Line.FromRTheta(radius, theta);
            Assert.AreEqual(expectedSlope, line.Slope, Error);
            Assert.AreEqual(expectedIntercept, line.Intercept, Error);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 10, true, float.PositiveInfinity, 0)]
        [DataRow(0, 0, 0, -10, true, float.NegativeInfinity, 0)]
        [DataRow(0, 0, 10, 10, false, 1, 0)]
        [DataRow(0, 0, 10, 0, false, 0, 0)]
        public void IsVerticalTest(float sx, float sy, float ex, float ey, bool expectedResult, float expectedSlope, float expectedIntercept)
        {
            var line = Line.FromPoints(new Point(sx, sy), new Point(ex, ey));

            Assert.AreEqual(expectedResult, line.IsVertical);
            Assert.AreEqual(expectedSlope, line.Slope);
            Assert.AreEqual(expectedIntercept, line.Intercept);
        }

        [DataTestMethod]
        [DataRow(0, 0, 10, 0, true, 0, 0)]
        [DataRow(0, 0, -10, 0, true, 0, 0)]
        [DataRow(0, 0, 10, 10, false, 1, 0)]
        [DataRow(0, 0, 0, 10, false, float.PositiveInfinity, 0)]
        public void IsHorizontalTest(float sx, float sy, float ex, float ey, bool expectedResult, float expectedSlope, float expectedIntercept)
        {
            var line = Line.FromPoints(new Point(sx, sy), new Point(ex, ey));

            Assert.AreEqual(expectedResult, line.IsHorizontal);
            Assert.AreEqual(expectedSlope, line.Slope);
            Assert.AreEqual(expectedIntercept, line.Intercept);
        }

        [DataTestMethod]
        [DataRow(0, 0, 10, 0, 0, 10, 10, 10, 0)]
        [DataRow(0, 0, 10, 0, 0, 10, 0, 20, 90)]
        [DataRow(0, 0, 10, 0, 1, 1, 10, 10, 45)]
        [DataRow(0, 0, 10, 0, 1, 1, -8, 10, 45)]
        [DataRow(0, 0, 10, 10, 0, 0, -100, 100, 90)]
        public void GetAngleBetweenLinesTest(float sx1, float sy1, float ex1, float ey1, float sx2, float sy2, float ex2, float ey2, float expectedAngle)
        {
            var line1 = Line.FromPoints(new Point(sx1, sy1), new Point(ex1, ey1));
            var line2 = Line.FromPoints(new Point(sx2, sy2), new Point(ex2, ey2));

            var angle = line1.GetAngleBetweenLines(line2);

            Assert.AreEqual(expectedAngle, angle, Error);
        }

        [DataTestMethod]
        [DataRow(0, 0, 1, 0, 0, 1, 1, 1, 0, 0, false)]
        [DataRow(0, 0, 0, 1, 1, 0, 1, 1, 0, 0, false)]
        //[DataRow( 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, false, ExpectedException = typeof( InvalidOperationException ) )]        [DataRow( 0, 0, 1, 1, 0, 1, 1, 2, 0, 0, false )]        [DataRow( 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, true )]        [DataRow( 0, 0, 1, 0, 0, 1, 1, 2, -1, 0, true )]        [DataRow( 0, 0, 1, 0, 1, 1, 1, 2, 1, 0, true )]        [DataRow( 0, 0, 0, 1, 0, 1, -1, 1, 0, 1, true )]        [DataRow( -1, -1, 1, 1, 1, -1, -1, 1, 0, 0, true )]
        public void GetIntersectionPointTest(float sx1, float sy1, float ex1, float ey1,
            float sx2, float sy2, float ex2, float ey2, float xRet, float yRet, bool hasResult)
        {
            var line1 = Line.FromPoints(new Point(sx1, sy1), new Point(ex1, ey1));
            var line2 = Line.FromPoints(new Point(sx2, sy2), new Point(ex2, ey2));

            var result = line1.GetIntersectionWith(line2);

            if (hasResult)
            {
                Assert.IsTrue(result==new Point(xRet, yRet));
            }
            else
            {
                Assert.AreEqual(null, result);
            }
        }

        [DataTestMethod]
        [DataRow(0, 0, 5, 0, 8, 0, 0)]
        [DataRow(6, 2, 5, 0, 8, 0, 2)]
        [DataRow(2, 6, 0, 5, 0, 8, 2)]
        [DataRow(9, 0, 5, 0, 8, 0, 0)]
        [DataRow(3, 0, 0, 0, 3, 4, 2.4f)]
        public void DistanceToPointTest(float x, float y, float x1, float y1, float x2, float y2, float expectedDistance)
        {
            var pt = new Point(x, y);
            var pt1 = new Point(x1, y1);
            var pt2 = new Point(x2, y2);
            var line = Line.FromPoints(pt1, pt2);

            Assert.AreEqual(expectedDistance, line.DistanceToPoint(pt), Error);
        }
    }
}

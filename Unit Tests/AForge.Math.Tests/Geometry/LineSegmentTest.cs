using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AForge.Math.Geometry.Tests
{
	[TestClass]
	public class LineSegmentTest
	{
		[DataTestMethod]
		[DataRow(0, 0, 10, 0, 10)]
		[DataRow(0, 0, 0, 10, 10)]
		[DataRow(0, 0, 3, 4, 5)]
		[DataRow(0, 0, -3, 4, 5)]
		[DataRow(0, 0, -3, -4, 5)]
		public void LengthTest(float sx, float sy, float ex, float ey, float expectedResult)
		{
			var segment = new LineSegment(new Point(sx, sy), new Point(ex, ey));

			Assert.AreEqual(expectedResult, segment.Length);
		}

		[DataTestMethod]
		[DataRow(0, 0, 5, 0, 8, 0, 5)]
		[DataRow(6f, 2.5f, 5f, 0f, 8f, 0f, 2.5f)]
		[DataRow(2.5f, 6f, 0f, 5f, 0f, 8f, 2.5f)]
		[DataRow(9, 0, 5, 0, 8, 0, 1)]
		[DataRow(3, 4, 0, 0, -10, 0, 5)]
		public void DistanceToPointTest(float x, float y, float x1, float y1, float x2, float y2, float expectedDistance)
		{
			var pt = new Point(x, y);
			var pt1 = new Point(x1, y1);
			var pt2 = new Point(x2, y2);
			var segment = new LineSegment(pt1, pt2);

			Assert.AreEqual(expectedDistance, segment.DistanceToPoint(pt));
		}

		// Denotes which versions of the test are supposed to return non-null values:
		// SegmentA means that the segment A1-A2 intersects with the line B1-B2, but not
		// with the segment B1-B2.
		public enum IntersectionType { None, LinesOnly, SegmentA, SegmentB, AllFour };

		[DataTestMethod]
		[DataRow(0, 0, 4, 4, 0, 4, 4, 0, 2, 2, IntersectionType.AllFour)]
		[DataRow(0, 0, 4, 0, 0, 0, 0, 4, 0, 0, IntersectionType.AllFour)]
		[DataRow(0, 0, 4, 4, 4, 8, 8, 4, 6, 6, IntersectionType.SegmentB)]
		[DataRow(-4, -4, 0, 0, 4, 0, 8, -4, 2, 2, IntersectionType.LinesOnly)]
		[DataRow(0, 0, 6, 0, 5, 1, 5, 5, 5, 0, IntersectionType.SegmentA)]
		[DataRow(0, 0, 0, 5, 1, 0, 1, 5, 0, 0, IntersectionType.None)]

		//[DataRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, IntersectionType.LinesOnly, ExpectedException = typeof(ArgumentException), ExpectedMessage = "Start point of the line cannot be the same as its end point.")]
		//[DataRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, IntersectionType.LinesOnly)]
		//[ExpectedException(typeof(ArgumentException), "Start point of the line cannot be the same as its end point.")]
		public void IntersectionPointTest(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2, float ix, float iy, IntersectionType type)
		{
			var segA = new LineSegment(new Point(ax1, ay1), new Point(ax2, ay2));
			var segB = new LineSegment(new Point(bx1, by1), new Point(bx2, by2));
			var expectedIntersection = new Point(ix, iy);

			// Assert.DoesNotThrow(() =>
			//{
			var segSeg = segA.GetIntersectionWith(segB);
			var segLine = segA.GetIntersectionWith((Line)segB);
			var lineSeg = ((Line)segA).GetIntersectionWith(segB);

			if (type == IntersectionType.AllFour)
			{
				Assert.AreEqual(expectedIntersection, segSeg);
			}
			else
			{
				Assert.AreEqual(null, segSeg);
			}

			if ((type == IntersectionType.AllFour) || (type == IntersectionType.SegmentA))
			{
				Assert.AreEqual(expectedIntersection, segLine);
			}
			else
			{
				Assert.AreEqual(null, segLine);
			}

			if ((type == IntersectionType.AllFour) || (type == IntersectionType.SegmentB))
			{
				Assert.AreEqual(expectedIntersection, lineSeg);
			}
			else
			{
				Assert.AreEqual(null, lineSeg);
			}
			//});

			var lineLine = ((Line)segA).GetIntersectionWith((Line)segB);

			if (type != IntersectionType.None)
			{
				Assert.AreEqual(expectedIntersection, lineLine);
			}
			else
			{
				Assert.AreEqual(null, lineLine);
			}
		}

		[DataTestMethod]
		[DataRow(0, 0, 0, 1, 1, 1, 1, 2)]
		[DataRow(0, 0, 4, 4, 3, -1, 7, 3)]
		[DataRow(0, 0, 1, 0, 1, 1, 2, 1)]
		public void ParallelIntersectionPointTest(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2)
		{
			var segA = new LineSegment(new Point(ax1, ay1), new Point(ax2, ay2));
			var segB = new LineSegment(new Point(bx1, by1), new Point(bx2, by2));

			// are we really parallel?
			Assert.AreEqual(null, ((Line)segA).GetIntersectionWith((Line)segB));

			Assert.AreEqual(null, segA.GetIntersectionWith((Line)segB));
			Assert.AreEqual(null, ((Line)segA).GetIntersectionWith(segB));
			Assert.AreEqual(null, segB.GetIntersectionWith((Line)segA));
			Assert.AreEqual(null, ((Line)segB).GetIntersectionWith(segA));
			Assert.AreEqual(null, segB.GetIntersectionWith(segA));
			Assert.AreEqual(null, segA.GetIntersectionWith(segB));
		}

		[DataTestMethod]
		[DataRow(0, 0, 1, 1, 2, 2, 3, 3)]
		[DataRow(0, 1, 0, 2, 0, 3, 0, 4)]
		[DataRow(0, 0, -1, 1, -2, 2, -3, 3)]
		[DataRow(1, 0, 2, 0, 3, 0, 4, 0)]
		[DataRow(0, 0, 0, 1, 0, 2, 0, 3)]
		public void CollinearIntersectionPointTest(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2)
		{
			var segA = new LineSegment(new Point(ax1, ay1), new Point(ax2, ay2));
			var segB = new LineSegment(new Point(bx1, by1), new Point(bx2, by2));

			// are we really collinear?
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segA).GetIntersectionWith((Line)segB));

			Assert.ThrowsException<InvalidOperationException>(() => segA.GetIntersectionWith((Line)segB));
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segA).GetIntersectionWith(segB));
			Assert.ThrowsException<InvalidOperationException>(() => segB.GetIntersectionWith((Line)segA));
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segB).GetIntersectionWith(segA));

			Assert.AreEqual(null, segB.GetIntersectionWith(segA));
			Assert.AreEqual(null, segA.GetIntersectionWith(segB));
		}

		[DataTestMethod]
		[DataRow(0, 0, 1, 1, 1, 1, 3, 3, 1, 1)]
		[DataRow(0, 0, 1, 1, 3, 3, 1, 1, 1, 1)]
		[DataRow(0, 0, 1, 1, 0, 0, -3, -3, 0, 0)]
		[DataRow(0, 0, 1, 1, -1, -1, 0, 0, 0, 0)]
		[DataRow(0, 1, 0, 2, 0, 1, 0, 0, 0, 1)]
		[DataRow(0, 1, 0, 2, 0, 2, 0, 4, 0, 2)]
		[DataRow(0, 1, 0, 2, 0, 0, 0, 1, 0, 1)]
		[DataRow(0, 1, 0, 2, 0, 3, 0, 2, 0, 2)]
		public void CommonIntersectionPointTest(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2, float ix, float iy)
		{
			var segA = new LineSegment(new Point(ax1, ay1), new Point(ax2, ay2));
			var segB = new LineSegment(new Point(bx1, by1), new Point(bx2, by2));
			var expectedIntersection = new Point(ix, iy);

			// are we really collinear?
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segA).GetIntersectionWith((Line)segB));

			Assert.ThrowsException<InvalidOperationException>(() => segA.GetIntersectionWith((Line)segB));
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segA).GetIntersectionWith(segB));
			Assert.ThrowsException<InvalidOperationException>(() => segB.GetIntersectionWith((Line)segA));
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segB).GetIntersectionWith(segA));
			Assert.AreEqual(expectedIntersection, segB.GetIntersectionWith(segA));
			Assert.AreEqual(expectedIntersection, segA.GetIntersectionWith(segB));
		}

		[DataTestMethod]
		[DataRow(0, 0, 0, 2, 0, 1, 0, 3)]
		[DataRow(1, 2, 3, 4, 2, 3, 4, 5)]
		[DataRow(0, 0, 2, 0, 3, 0, 1, 0)]
		public void OverlappingSegmentIntersectionPointTest(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2)
		{
			var segA = new LineSegment(new Point(ax1, ay1), new Point(ax2, ay2));
			var segB = new LineSegment(new Point(bx1, by1), new Point(bx2, by2));

			// are we really collinear?
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segA).GetIntersectionWith((Line)segB));

			Assert.ThrowsException<InvalidOperationException>(() => segA.GetIntersectionWith((Line)segB));
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segA).GetIntersectionWith(segB));
			Assert.ThrowsException<InvalidOperationException>(() => segB.GetIntersectionWith((Line)segA));
			Assert.ThrowsException<InvalidOperationException>(() => ((Line)segB).GetIntersectionWith(segA));
			Assert.ThrowsException<InvalidOperationException>(() => segB.GetIntersectionWith(segA));
			Assert.ThrowsException<InvalidOperationException>(() => segA.GetIntersectionWith(segB));
		}
	}
}

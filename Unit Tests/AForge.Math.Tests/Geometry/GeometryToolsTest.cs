using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AForge.Math.Geometry.Tests
{
	[TestClass]
	public class GeometryToolsTest
	{
		[DataTestMethod]
		[DataRow(0, 0, 10, 0, 100, 0, 0)]
		[DataRow(0, 0, 10, 10, 100, 100, 0)]
		[DataRow(0, 0, 10, 0, 0, 100, 90)]
		[DataRow(0, 0, 10, 0, 100, 100, 45)]
		[DataRow(0, 0, 10, 10, -100, 100, 90)]
		[DataRow(0, 0, 10, 0, -100, 100, 135)]
		[DataRow(0, 0, 10, 0, -100, 0, 180)]
		[DataRow(0, 0, 10, 0, -100, -100, 135)]
		public void GetAngleBetweenVectorsTest(int sx, int sy, int ex1, int ey1, int ex2, int ey2, float expectedAngle)
		{
			var startPoint = new IntPoint(sx, sy);
			var vector1end = new IntPoint(ex1, ey1);
			var vector2end = new IntPoint(ex2, ey2);

			var angle = GeometryTools.GetAngleBetweenVectors(startPoint, vector1end, vector2end);

			Assert.AreEqual(expectedAngle, angle, 0.00001f);
		}

		[DataTestMethod]
		[DataRow(0, 0, 10, 0, 0, 10, 10, 10, 0)]
		[DataRow(0, 0, 10, 0, 0, 10, 0, 20, 90)]
		[DataRow(0, 0, 10, 0, 1, 1, 10, 10, 45)]
		[DataRow(0, 0, 10, 0, 1, 1, -8, 10, 45)]
		[DataRow(0, 0, 10, 10, 0, 0, -100, 100, 90)]
		public void GetAngleBetweenLinesTest(int sx1, int sy1, int ex1, int ey1, int sx2, int sy2, int ex2, int ey2, float expectedAngle)
		{
			var line1start = new IntPoint(sx1, sy1);
			var line1end = new IntPoint(ex1, ey1);
			var line2start = new IntPoint(sx2, sy2);
			var line2end = new IntPoint(ex2, ey2);

			var angle = GeometryTools.GetAngleBetweenLines(line1start, line1end, line2start, line2end);

			Assert.AreEqual(expectedAngle, angle, 0.00001f);
		}
	}
}

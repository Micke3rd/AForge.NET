using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AForge.Math.Geometry.Tests
{
    [TestClass]
    public class FlatAnglesOptimizerTest
    {
        private IShapeOptimizer optimizer = new FlatAnglesOptimizer(160);

        [DataTestMethod]
        [DataRow(new int[] { 0, 0, 10, 0, 10, 10 }, new int[] { 0, 0, 10, 0, 10, 10 })]
        [DataRow(new int[] { 0, 0, 20, 0, 10, 1 }, new int[] { 0, 0, 20, 0, 10, 1 })]
        [DataRow(new int[] { 0, 0, 10, 1, 20, 0, 20, 20 }, new int[] { 0, 0, 20, 0, 20, 20 })]
        [DataRow(new int[] { 0, 0, 5, 1, 10, 0, 10, 10 }, new int[] { 0, 0, 5, 1, 10, 0, 10, 10 })]
        [DataRow(new int[] { 0, 0, 20, 0, 20, 20, 11, 9 }, new int[] { 0, 0, 20, 0, 20, 20 })]
        [DataRow(new int[] { 0, 0, 20, 0, 20, 20, 9, 11 }, new int[] { 0, 0, 20, 0, 20, 20 })]
        [DataRow(new int[] { 9, 11, 0, 0, 10, 1, 20, 0, 21, 10, 20, 20 }, new int[] { 0, 0, 20, 0, 20, 20 })]
        [DataRow(new int[] { 11, 9, 0, 0, 10, -1, 20, 0, 19, 10, 20, 20 }, new int[] { 0, 0, 20, 0, 20, 20 })]
        public void OptimizationTest(int[] coordinates, int[] expectedCoordinates)
        {
            ShapeOptimizerTestBase.TestOptimizer(coordinates, expectedCoordinates, optimizer);
        }
    }
}

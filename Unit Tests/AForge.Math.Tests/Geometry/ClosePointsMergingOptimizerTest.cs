using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AForge.Math.Geometry.Tests
{
	[TestClass]
	public class ClosePointsMergingOptimizerTest
	{
		private IShapeOptimizer optimizer = new ClosePointsMergingOptimizer(3);

		[DataTestMethod]
		[DataRow(new int[] { 0,0,10,0,10,10 },new int[] { 0,0,10,0,10,10 })]
		[DataRow(new int[] { 0,0,10,0,1,1 },new int[] { 0,0,10,0,1,1 })]
		[DataRow(new int[] { 0,0,10,0,10,10,2,2 },new int[] { 1,1,10,0,10,10 })]
		[DataRow(new int[] { 0,0,10,0,10,10,3,3 },new int[] { 0,0,10,0,10,10,3,3 })]
		[DataRow(new int[] { 0,0,8,0,10,2,10,10 },new int[] { 0,0,9,1,10,10 })]
		[DataRow(new int[] { 2,0,8,0,10,2,10,8,8,10,0,2 },new int[] { 1,1,9,1,9,9 })]

		public void OptimizationTest(int[] coordinates,int[] expectedCoordinates)
		{
			ShapeOptimizerTestBase.TestOptimizer(coordinates,expectedCoordinates,optimizer);
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Drawing.Imaging;

namespace AForge.Imaging.Tests
{
	[TestClass]
	public class IntegralImageTest
	{
		private IntegralImage integralImage = null;

		public IntegralImageTest()
		{
			var uImage = UnmanagedImage.Create(10,10,PixelFormat.Format8bppIndexed);

			for (var y = 0; y < 10; y++)
			{
				for (var x = 0; x < 10; x++)
				{
					uImage.SetPixel(x,y,((x + y) % 2 == 0) ? Color.FromArgb(0,0,0) : Color.FromArgb(1,1,1));
				}
			}

			integralImage = IntegralImage.FromBitmap(uImage);
		}

		[DataTestMethod]
		[DataRow(0,0,0,0,0u)]
		[DataRow(0,0,1,0,1u)]
		[DataRow(0,0,0,1,1u)]
		[DataRow(0,0,1,1,2u)]
		[DataRow(-1,-1,1,1,2u)]
		[DataRow(0,0,9,9,50u)]
		[DataRow(9,9,9,9,0u)]
		[DataRow(9,9,10,10,0u)]
		public void GetRectangleSumTest(int x1,int y1,int x2,int y2,uint expectedSum)
		{
			var sum = integralImage.GetRectangleSum(x1,y1,x2,y2);
			Assert.AreEqual(sum,expectedSum);
		}

		[DataTestMethod]
		[DataRow(0,0,0,0,0u)]
		[DataRow(0,0,1,0,1u)]
		[DataRow(0,0,0,1,1u)]
		[DataRow(0,0,1,1,2u)]
		[DataRow(0,0,9,9,50u)]
		[DataRow(9,9,9,9,0u)]
		public void GetRectangleSumUnsafeTest(int x1,int y1,int x2,int y2,uint expectedSum)
		{
			var sum = integralImage.GetRectangleSum(x1,y1,x2,y2);
			Assert.AreEqual(sum,expectedSum);
		}

		[DataTestMethod]
		[DataRow(0,0,1,2u)]
		[DataRow(1,1,1,4u)]
		[DataRow(9,9,1,2u)]
		[DataRow(8,8,1,4u)]
		[DataRow(2,1,1,5u)]
		public void GetRectangleSumTest(int x,int y,int radius,uint expectedSum)
		{
			var sum = integralImage.GetRectangleSum(x,y,radius);
			Assert.AreEqual(sum,expectedSum);
		}

		[DataTestMethod]
		[DataRow(1,1,1,4u)]
		[DataRow(8,8,1,4u)]
		[DataRow(2,1,1,5u)]
		public void GetRectangleSumUnsafeTest(int x,int y,int radius,uint expectedSum)
		{
			var sum = integralImage.GetRectangleSum(x,y,radius);
			Assert.AreEqual(sum,expectedSum);
		}

		[DataTestMethod]
		[DataRow(0,0,0,0,0)]
		[DataRow(0,0,1,0,0.5f)]
		[DataRow(0,0,0,1,0.5f)]
		[DataRow(0,0,1,1,0.5f)]
		[DataRow(-1,-1,1,1,0.5f)]
		[DataRow(0,0,9,9,0.5f)]
		[DataRow(9,9,9,9,0)]
		[DataRow(9,9,10,10,0)]
		[DataRow(9,0,9,0,1)]
		public void GetRectangleMeanTest(int x1,int y1,int x2,int y2,float expectedMean)
		{
			var mean = integralImage.GetRectangleMean(x1,y1,x2,y2);
			Assert.AreEqual(mean,expectedMean);
		}

		[DataTestMethod]
		[DataRow(1,1,1,0)]
		[DataRow(1,2,1,0)]
		[DataRow(2,2,1,0)]
		[DataRow(2,2,2,0)]
		[DataRow(8,8,1,0)]
		[DataRow(5,5,5,0)]
		[DataRow(0,1,1,1)]
		[DataRow(10,9,1,-1)]
		public void GetHaarXWavelet(int x,int y,int radius,int expectedValue)
		{
			var value = integralImage.GetHaarXWavelet(x,y,radius);
			Assert.AreEqual(value,expectedValue);
		}

		[DataTestMethod]
		[DataRow(1,1,1,0)]
		[DataRow(1,2,1,0)]
		[DataRow(2,2,1,0)]
		[DataRow(2,2,2,0)]
		[DataRow(8,8,1,0)]
		[DataRow(5,5,5,0)]
		[DataRow(1,0,1,1)]
		[DataRow(9,10,1,-1)]
		public void GetHaarYWavelet(int x,int y,int radius,int expectedValue)
		{
			var value = integralImage.GetHaarYWavelet(x,y,radius);
			Assert.AreEqual(value,expectedValue);
		}
	}
}

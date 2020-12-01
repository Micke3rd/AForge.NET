﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace AForge.Imaging.Tests
{
	[TestClass]
	public class UnmanagedImageTest
	{
		[TestMethod]
		public void Collect8bppPixelValuesTest_Grayscale()
		{
			// create grayscale image
			var image = UnmanagedImage.Create(320,240,PixelFormat.Format8bppIndexed);

			// draw vertical and horizontal lines
			Drawing.Line(image,new IntPoint(10,10),new IntPoint(20,10),Color.FromArgb(128,128,128));
			Drawing.Line(image,new IntPoint(20,20),new IntPoint(20,30),Color.FromArgb(64,64,64));

			// prepare lists with coordinates
			var horizontal = new List<IntPoint>();
			var horizontalU = new List<IntPoint>();
			var horizontalD = new List<IntPoint>();

			for (var x = 10; x <= 20; x++)
			{
				horizontal.Add(new IntPoint(x,10));  // on the line
				horizontalU.Add(new IntPoint(x,9));  // above
				horizontalD.Add(new IntPoint(x,11)); // below
			}

			var vertical = new List<IntPoint>();
			var verticalL = new List<IntPoint>();
			var verticalR = new List<IntPoint>();

			for (var y = 20; y <= 30; y++)
			{
				vertical.Add(new IntPoint(20,y));    // on the line
				verticalL.Add(new IntPoint(19,y));   // left
				verticalR.Add(new IntPoint(21,y));   // right
			}

			// collect all pixel's values
			var horizontalValues = image.Collect8bppPixelValues(horizontal);
			var horizontalUValues = image.Collect8bppPixelValues(horizontalU);
			var horizontalDValues = image.Collect8bppPixelValues(horizontalD);
			var verticalValues = image.Collect8bppPixelValues(vertical);
			var verticalLValues = image.Collect8bppPixelValues(verticalL);
			var verticalRValues = image.Collect8bppPixelValues(verticalR);

			Assert.AreEqual(horizontal.Count,horizontalValues.Length);
			Assert.AreEqual(vertical.Count,verticalValues.Length);

			// check all pixel values
			for (int i = 0, n = horizontalValues.Length; i < n; i++)
			{
				Assert.AreEqual(128,horizontalValues[i]);
				Assert.AreEqual(0,horizontalUValues[i]);
				Assert.AreEqual(0,horizontalDValues[i]);
			}

			for (int i = 0, n = verticalValues.Length; i < n; i++)
			{
				Assert.AreEqual(64,verticalValues[i]);
				Assert.AreEqual(0,verticalLValues[i]);
				Assert.AreEqual(0,verticalRValues[i]);
			}
		}

		[TestMethod]
		public void Collect8bppPixelValuesTest_RGB()
		{
			// create grayscale image
			var image = UnmanagedImage.Create(320,240,PixelFormat.Format24bppRgb);

			// draw vertical and horizontal lines
			Drawing.Line(image,new IntPoint(10,10),new IntPoint(20,10),Color.FromArgb(128,129,130));
			Drawing.Line(image,new IntPoint(20,20),new IntPoint(20,30),Color.FromArgb(64,65,66));

			// prepare lists with coordinates
			var horizontal = new List<IntPoint>();
			var horizontalU = new List<IntPoint>();
			var horizontalD = new List<IntPoint>();

			for (var x = 10; x <= 20; x++)
			{
				horizontal.Add(new IntPoint(x,10));  // on the line
				horizontalU.Add(new IntPoint(x,9));  // above
				horizontalD.Add(new IntPoint(x,11)); // below
			}

			var vertical = new List<IntPoint>();
			var verticalL = new List<IntPoint>();
			var verticalR = new List<IntPoint>();

			for (var y = 20; y <= 30; y++)
			{
				vertical.Add(new IntPoint(20,y));    // on the line
				verticalL.Add(new IntPoint(19,y));   // left
				verticalR.Add(new IntPoint(21,y));   // right
			}

			// collect all pixel's values
			var horizontalValues = image.Collect8bppPixelValues(horizontal);
			var horizontalUValues = image.Collect8bppPixelValues(horizontalU);
			var horizontalDValues = image.Collect8bppPixelValues(horizontalD);
			var verticalValues = image.Collect8bppPixelValues(vertical);
			var verticalLValues = image.Collect8bppPixelValues(verticalL);
			var verticalRValues = image.Collect8bppPixelValues(verticalR);

			Assert.AreEqual(horizontal.Count * 3,horizontalValues.Length);
			Assert.AreEqual(vertical.Count * 3,verticalValues.Length);

			// check all pixel values
			for (int i = 0, n = horizontalValues.Length; i < n; i += 3)
			{
				Assert.AreEqual(128,horizontalValues[i]);
				Assert.AreEqual(129,horizontalValues[i + 1]);
				Assert.AreEqual(130,horizontalValues[i + 2]);

				Assert.AreEqual(0,horizontalUValues[i]);
				Assert.AreEqual(0,horizontalUValues[i + 1]);
				Assert.AreEqual(0,horizontalUValues[i + 2]);

				Assert.AreEqual(0,horizontalDValues[i]);
				Assert.AreEqual(0,horizontalDValues[i + 1]);
				Assert.AreEqual(0,horizontalDValues[i + 2]);
			}

			for (int i = 0, n = verticalValues.Length; i < n; i += 3)
			{
				Assert.AreEqual(64,verticalValues[i]);
				Assert.AreEqual(65,verticalValues[i + 1]);
				Assert.AreEqual(66,verticalValues[i + 2]);

				Assert.AreEqual(0,verticalLValues[i]);
				Assert.AreEqual(0,verticalLValues[i + 1]);
				Assert.AreEqual(0,verticalLValues[i + 2]);

				Assert.AreEqual(0,verticalRValues[i]);
				Assert.AreEqual(0,verticalRValues[i + 1]);
				Assert.AreEqual(0,verticalRValues[i + 2]);
			}
		}

		[TestMethod]
		public void CollectActivePixelsTest()
		{
			// create grayscale image
			var image24 = UnmanagedImage.Create(7,7,PixelFormat.Format24bppRgb);
			var image8 = UnmanagedImage.Create(7,7,PixelFormat.Format8bppIndexed);

			Drawing.FillRectangle(image24,new Rectangle(1,1,5,5),Color.FromArgb(255,255,255));
			Drawing.FillRectangle(image24,new Rectangle(2,2,3,3),Color.FromArgb(1,1,1));
			Drawing.FillRectangle(image24,new Rectangle(3,3,1,1),Color.FromArgb(0,0,0));

			Drawing.FillRectangle(image8,new Rectangle(1,1,5,5),Color.FromArgb(255,255,255));
			Drawing.FillRectangle(image8,new Rectangle(2,2,3,3),Color.FromArgb(1,1,1));
			Drawing.FillRectangle(image8,new Rectangle(3,3,1,1),Color.FromArgb(0,0,0));

			var pixels24 = image24.CollectActivePixels();
			var pixels8 = image8.CollectActivePixels();

			Assert.AreEqual(pixels24.Count,24);
			Assert.AreEqual(pixels8.Count,24);

			for (var i = 1; i < 6; i++)
			{
				for (var j = 1; j < 6; j++)
				{
					if ((i == 3) && (j == 3))
						continue;

					Assert.IsTrue(pixels24.Contains(new IntPoint(j,i)));
					Assert.IsTrue(pixels8.Contains(new IntPoint(j,i)));
				}
			}

			pixels24 = image24.CollectActivePixels(new Rectangle(1,0,5,4));
			pixels8 = image8.CollectActivePixels(new Rectangle(1,0,5,4));

			Assert.AreEqual(pixels24.Count,14);
			Assert.AreEqual(pixels8.Count,14);

			for (var i = 1; i < 4; i++)
			{
				for (var j = 1; j < 6; j++)
				{
					if ((i == 3) && (j == 3))
						continue;

					Assert.IsTrue(pixels24.Contains(new IntPoint(j,i)));
					Assert.IsTrue(pixels8.Contains(new IntPoint(j,i)));
				}
			}
		}
		[DataTestMethod]
		[DataRow(PixelFormat.Format8bppIndexed)]
		[DataRow(PixelFormat.Format24bppRgb)]
		[DataRow(PixelFormat.Format32bppArgb)]
		[DataRow(PixelFormat.Format32bppRgb)]
		[DataRow(PixelFormat.Format16bppGrayScale)]
		[DataRow(PixelFormat.Format48bppRgb)]
		[DataRow(PixelFormat.Format64bppArgb)]
		//[DataRow(PixelFormat.Format32bppPArgb, ExpectedException = typeof(UnsupportedImageFormatException))]
		public void SetPixelTest(PixelFormat pixelFormat)
		{
			var image = UnmanagedImage.Create(320,240,pixelFormat);
			var color = Color.White;
			byte value = 255;

			image.SetPixel(0,0,color);
			image.SetPixel(319,0,color);
			image.SetPixel(0,239,color);
			image.SetPixel(319,239,value);
			image.SetPixel(160,120,value);

			image.SetPixel(-1,-1,color);
			image.SetPixel(320,0,color);
			image.SetPixel(0,240,value);
			image.SetPixel(320,240,value);

			var pixels = image.CollectActivePixels();

			Assert.AreEqual(5,pixels.Count);

			Assert.IsTrue(pixels.Contains(new IntPoint(0,0)));
			Assert.IsTrue(pixels.Contains(new IntPoint(319,0)));
			Assert.IsTrue(pixels.Contains(new IntPoint(0,239)));
			Assert.IsTrue(pixels.Contains(new IntPoint(319,239)));
			Assert.IsTrue(pixels.Contains(new IntPoint(160,120)));
		}

		[TestMethod]
		public void SetGetPixelGrayscale()
		{
			var image = UnmanagedImage.Create(320,240,PixelFormat.Format8bppIndexed);

			image.SetPixel(0,0,255);
			image.SetPixel(319,0,127);
			image.SetPixel(0,239,Color.FromArgb(64,64,64));

			var color1 = image.GetPixel(0,0);
			var color2 = image.GetPixel(319,0);
			var color3 = image.GetPixel(0,239);

			Assert.AreEqual(255,color1.R);
			Assert.AreEqual(255,color1.G);
			Assert.AreEqual(255,color1.B);

			Assert.AreEqual(127,color2.R);
			Assert.AreEqual(127,color2.G);
			Assert.AreEqual(127,color2.B);

			Assert.AreEqual(64,color3.R);
			Assert.AreEqual(64,color3.G);
			Assert.AreEqual(64,color3.B);
		}
		[DataTestMethod]
		[DataRow(PixelFormat.Format24bppRgb)]
		[DataRow(PixelFormat.Format32bppArgb)]
		[DataRow(PixelFormat.Format32bppRgb)]
		public void SetGetPixelColor(PixelFormat pixelFormat)
		{
			var image = UnmanagedImage.Create(320,240,pixelFormat);

			image.SetPixel(0,0,Color.FromArgb(255,10,20,30));
			image.SetPixel(319,0,Color.FromArgb(127,110,120,130));
			image.SetPixel(0,239,Color.FromArgb(64,210,220,230));

			var color1 = image.GetPixel(0,0);
			var color2 = image.GetPixel(319,0);
			var color3 = image.GetPixel(0,239);

			Assert.AreEqual(10,color1.R);
			Assert.AreEqual(20,color1.G);
			Assert.AreEqual(30,color1.B);

			Assert.AreEqual(110,color2.R);
			Assert.AreEqual(120,color2.G);
			Assert.AreEqual(130,color2.B);

			Assert.AreEqual(210,color3.R);
			Assert.AreEqual(220,color3.G);
			Assert.AreEqual(230,color3.B);

			if (pixelFormat == PixelFormat.Format32bppArgb)
			{
				Assert.AreEqual(255,color1.A);
				Assert.AreEqual(127,color2.A);
				Assert.AreEqual(64,color3.A);
			}
		}
		[DataTestMethod]
		[DataRow(PixelFormat.Format8bppIndexed)]
		[DataRow(PixelFormat.Format24bppRgb)]
		[DataRow(PixelFormat.Format32bppArgb)]
		[DataRow(PixelFormat.Format32bppRgb)]
		[DataRow(PixelFormat.Format16bppGrayScale)]
		[DataRow(PixelFormat.Format48bppRgb)]
		[DataRow(PixelFormat.Format64bppArgb)]
		//[DataRow(PixelFormat.Format32bppPArgb, ExpectedException = typeof(UnsupportedImageFormatException))]
		public void SetPixelsTest(PixelFormat pixelFormat)
		{
			var image = UnmanagedImage.Create(320,240,pixelFormat);
			var color = Color.White;
			var points = new List<IntPoint>
			{
				new IntPoint(0, 0),
				new IntPoint(319, 0),
				new IntPoint(0, 239),
				new IntPoint(319, 239),
				new IntPoint(160, 120),

				new IntPoint(-1, -1),
				new IntPoint(320, 0),
				new IntPoint(0, 240),
				new IntPoint(320, 240)
			};

			image.SetPixels(points,color);

			var pixels = image.CollectActivePixels();

			Assert.AreEqual(5,pixels.Count);

			Assert.IsTrue(pixels.Contains(new IntPoint(0,0)));
			Assert.IsTrue(pixels.Contains(new IntPoint(319,0)));
			Assert.IsTrue(pixels.Contains(new IntPoint(0,239)));
			Assert.IsTrue(pixels.Contains(new IntPoint(319,239)));
			Assert.IsTrue(pixels.Contains(new IntPoint(160,120)));
		}
		[DataTestMethod]
		[DataRow(PixelFormat.Format24bppRgb,1,1,240,0,0)]
		[DataRow(PixelFormat.Format24bppRgb,318,1,0,240,0)]
		[DataRow(PixelFormat.Format24bppRgb,318,238,240,240,0)]
		[DataRow(PixelFormat.Format24bppRgb,1,238,0,0,240)]
		[DataRow(PixelFormat.Format24bppRgb,160,120,240,240,240)]
		[DataRow(PixelFormat.Format32bppArgb,1,1,240,0,0)]
		[DataRow(PixelFormat.Format32bppArgb,318,1,0,240,0)]
		[DataRow(PixelFormat.Format32bppArgb,318,238,240,240,0)]
		[DataRow(PixelFormat.Format32bppArgb,1,238,0,0,240)]
		[DataRow(PixelFormat.Format32bppArgb,160,120,240,240,240)]
		[DataRow(PixelFormat.Format32bppRgb,1,1,240,0,0)]
		[DataRow(PixelFormat.Format32bppRgb,318,1,0,240,0)]
		[DataRow(PixelFormat.Format32bppRgb,318,238,240,240,0)]
		[DataRow(PixelFormat.Format32bppRgb,1,238,0,0,240)]
		[DataRow(PixelFormat.Format32bppRgb,160,120,240,240,240)]
		[DataRow(PixelFormat.Format8bppIndexed,1,1,128,128,128)]
		[DataRow(PixelFormat.Format8bppIndexed,318,1,96,96,96)]
		[DataRow(PixelFormat.Format8bppIndexed,318,238,192,192,192)]
		[DataRow(PixelFormat.Format8bppIndexed,1,238,32,32,32)]
		[DataRow(PixelFormat.Format8bppIndexed,160,120,255,255,255)]

		public void ToManagedImageTest(PixelFormat pixelFormat,int x,int y,byte red,byte green,byte blue)
		{
			var image = UnmanagedImage.Create(320,240,pixelFormat);

			image.SetPixel(new IntPoint(x,y),Color.FromArgb(255,red,green,blue));

			var bitmap = image.ToManagedImage();

			// check colors of pixels
			Assert.AreEqual(Color.FromArgb(255,red,green,blue),bitmap.GetPixel(x,y));

			// make sure there are only 1 pixel
			var temp = UnmanagedImage.FromManagedImage(bitmap);

			var pixels = temp.CollectActivePixels();
			Assert.AreEqual(1,pixels.Count);

			image.Dispose();
			bitmap.Dispose();
			temp.Dispose();
		}
	}
}

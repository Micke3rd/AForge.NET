// Sample of IPPrototyper usage
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2007-2011
// contacts@aforgenet.com
//

using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.IPPrototyper;
using AForge.Math.Geometry;
using System.Drawing;

namespace IPPrototyperSample
{
	public class CoinsFinder : IImageProcessingRoutine
	{
		// Image processing routine's name
		public string Name
		{
			get { return "IPPrototyper Sample"; }
		}

		// Process specified image trying to recognize counter's image
		public void Process(Bitmap image, IImageProcessingLog log)
		{
			log.AddMessage("Image size: " + image.Width + " x " + image.Height);

			// 1- grayscale image
			var grayImage = Grayscale.CommonAlgorithms.BT709.Apply(image);
			log.AddImage("Grayscale", grayImage);

			// 2 - Otsu thresholding
			var threshold = new OtsuThreshold();
			var binaryImage = threshold.Apply(grayImage);
			log.AddImage("Binary", binaryImage);
			log.AddMessage("Otsu threshold: " + threshold.ThresholdValue);

			// 3 - Blob counting
			var blobCounter = new BlobCounter
			{
				FilterBlobs = true,
				MinWidth = 24
			};
			blobCounter.MinWidth = 24;

			blobCounter.ProcessImage(binaryImage);
			var blobs = blobCounter.GetObjectsInformation();

			log.AddMessage("Found blobs (min width/height = 24): " + blobs.Length);

			// 4 - check shape of each blob
			var shapeChecker = new SimpleShapeChecker();

			log.AddMessage("Found coins: ");
			var count = 0;

			// create graphics object for drawing on image
			var g = Graphics.FromImage(image);
			var pen = new Pen(Color.Red, 3);

			foreach (var blob in blobs)
			{
				var edgePoint = blobCounter.GetBlobsEdgePoints(blob);

				// check if shape looks like a circle
				AForge.Point center;
				float radius;

				if (shapeChecker.IsCircle(edgePoint, out center, out radius))
				{
					count++;

					log.AddMessage(string.Format("  {0}: center = ({1}, {2}), radius = {3}",
						count, center.X, center.Y, radius));

					// highlight coin
					g.DrawEllipse(pen, (int)(center.X - radius), (int)(center.Y - radius),
						(int)(radius * 2), (int)(radius * 2));
				}
			}

			g.Dispose();
			pen.Dispose();

			log.AddMessage("Total coins: " + count);
			log.AddImage("Coins", image);
		}
	}
}

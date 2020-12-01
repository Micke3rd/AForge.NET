// AForge Image Processing Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Andrew Kirillov, 2005-2010
// andrew.kirillov@aforgenet.com
//

namespace AForge.Imaging.Filters
{
	using System.Collections.Generic;
	using System.Drawing;
	using System.Drawing.Imaging;

	/// <summary>
	/// Simple water wave effect filter.
	/// </summary>
	/// 
	/// <remarks><para>The image processing filter implements simple water wave effect. Using
	/// properties of the class, it is possible to set number of vertical/horizontal waves,
	/// as well as their amplitude.</para>
	/// 
	/// <para>Bilinear interpolation is used to create smooth effect.</para>
	/// 
	/// <para>The filter accepts 8 bpp grayscale images and 24/32
	/// color images for processing.</para>
	/// 
	/// <para>Sample usage:</para>
	/// <code>
	/// // create filter
	/// WaterWave filter = new WaterWave( );
	/// filter.HorizontalWavesCount     = 10;
	/// filter.HorizontalWavesAmplitude = 5;
	/// filter.VerticalWavesCount       = 3;
	/// filter.VerticalWavesAmplitude   = 15;
	/// // apply the filter
	/// Bitmap newImage = filter.Apply( image );
	/// </code>
	/// 
	/// <para><b>Initial image:</b></para>
	/// <img src="img/imaging/sample1.jpg" width="480" height="361" />
	/// <para><b>Result image:</b></para>
	/// <img src="img/imaging/water_wave.jpg" width="480" height="361" />
	/// </remarks>
	/// 
	public class WaterWave: BaseFilter
	{
		// private format translation dictionary
		private Dictionary<PixelFormat,PixelFormat> formatTranslations = new Dictionary<PixelFormat,PixelFormat>();

		private int xWavesCount = 5;
		private int yWavesCount = 5;
		private int xWavesAmplitude = 10;
		private int yWavesAmplitude = 10;

		/// <summary>
		/// Number of horizontal waves, [1, 10000].
		/// </summary>
		/// 
		/// <remarks><para>Default value is set to <b>5</b>.</para></remarks>
		/// 
		public int HorizontalWavesCount
		{
			get { return xWavesCount; }
			set { xWavesCount = System.Math.Max(1,System.Math.Min(10000,value)); }
		}

		/// <summary>
		/// Number of vertical waves, [1, 10000].
		/// </summary>
		/// 
		/// <remarks><para>Default value is set to <b>5</b>.</para></remarks>
		/// 
		public int VerticalWavesCount
		{
			get { return yWavesCount; }
			set { yWavesCount = System.Math.Max(1,System.Math.Min(10000,value)); }
		}

		/// <summary>
		/// Amplitude of horizontal waves measured in pixels, [0, 10000].
		/// </summary>
		/// 
		/// <remarks><para>Default value is set to <b>10</b>.</para></remarks>
		/// 
		public int HorizontalWavesAmplitude
		{
			get { return xWavesAmplitude; }
			set { xWavesAmplitude = System.Math.Max(0,System.Math.Min(10000,value)); }
		}

		/// <summary>
		/// Amplitude of vertical waves measured in pixels, [0, 10000].
		/// </summary>
		/// 
		/// <remarks><para>Default value is set to <b>10</b>.</para></remarks>
		/// 
		public int VerticalWavesAmplitude
		{
			get { return yWavesAmplitude; }
			set { yWavesAmplitude = System.Math.Max(0,System.Math.Min(10000,value)); }
		}

		/// <summary>
		/// Format translations dictionary.
		/// </summary>
		/// 
		/// <remarks><para>See <see cref="IFilterInformation.FormatTranslations"/>
		/// documentation for additional information.</para></remarks>
		/// 
		public override Dictionary<PixelFormat,PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="WaterWave"/> class.
		/// </summary>
		public WaterWave()
		{
			formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
		}

		/// <summary>
		/// Process the filter on the specified image.
		/// </summary>
		/// 
		/// <param name="source">Source image data.</param>
		/// <param name="destination">Destination image data.</param>
		/// 
		protected override unsafe void ProcessFilter(UnmanagedImage source,UnmanagedImage destination)
		{
			var pixelSize = Image.GetPixelFormatSize(source.PixelFormat) / 8;

			// processing start and stop X,Y positions
			var width = source.Width;
			var height = source.Height;

			var srcStride = source.Stride;
			var dstStride = destination.Stride;
			var dstOffset = dstStride - width * pixelSize;

			// coordinates of source points
			double ox, oy, dx1, dy1, dx2, dy2;
			int ox1, oy1, ox2, oy2;

			// width and height decreased by 1
			var ymax = height - 1;
			var xmax = width - 1;

			var src = (byte*)source.ImageData.ToPointer();
			var dst = (byte*)destination.ImageData.ToPointer();
			byte* p1, p2, p3, p4;

			var xFactor = 2 * System.Math.PI * xWavesCount / width;
			var yFactor = 2 * System.Math.PI * yWavesCount / height;

			// for each line
			for (var y = 0; y < height; y++)
			{
				var yPart = System.Math.Sin(yFactor * y) * yWavesAmplitude;

				// for each pixel
				for (var x = 0; x < width; x++)
				{
					ox = x + yPart;
					oy = y + System.Math.Cos(xFactor * x) * xWavesAmplitude;

					// check if the source pixel is inside of image
					if ((ox >= 0) && (oy >= 0) && (ox < width) && (oy < height))
					{
						// perform bilinear interpolation
						oy1 = (int)oy;
						oy2 = (oy1 == ymax) ? oy1 : oy1 + 1;
						dy1 = oy - oy1;
						dy2 = 1.0 - dy1;

						ox1 = (int)ox;
						ox2 = (ox1 == xmax) ? ox1 : ox1 + 1;
						dx1 = ox - ox1;
						dx2 = 1.0 - dx1;

						p1 = src + oy1 * srcStride + ox1 * pixelSize;
						p2 = src + oy1 * srcStride + ox2 * pixelSize;
						p3 = src + oy2 * srcStride + ox1 * pixelSize;
						p4 = src + oy2 * srcStride + ox2 * pixelSize;

						for (var i = 0; i < pixelSize; i++, dst++, p1++, p2++, p3++, p4++)
						{
							*dst = (byte)(
								dy2 * (dx2 * (*p1) + dx1 * (*p2)) +
								dy1 * (dx2 * (*p3) + dx1 * (*p4)));
						}
					}
					else
					{
						for (var i = 0; i < pixelSize; i++, dst++)
						{
							*dst = 0;
						}
					}
				}
				dst += dstOffset;
			}
		}
	}
}


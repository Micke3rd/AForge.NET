// AForge Image Processing Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2005-2011
// contacts@aforgenet.com
//

namespace AForge.Imaging.Filters
{
	using System.Collections.Generic;
	using System.Drawing;
	using System.Drawing.Imaging;

	/// <summary>
	/// Difference filter - get the difference between overlay and source images.
	/// </summary>
	/// 
	/// <remarks><para>The difference filter takes two images (source and
	/// <see cref="BaseInPlaceFilter2.OverlayImage">overlay</see> images)
	/// of the same size and pixel format and produces an image, where each pixel equals
	/// to absolute difference between corresponding pixels from provided images.</para>
	/// 
	/// <para>The filter accepts 8 and 16 bpp grayscale images and 24, 32, 48 and 64 bpp
	/// color images for processing.</para>
	/// 
	/// <para><note>In the case if images with alpha channel are used (32 or 64 bpp), visualization
	/// of the result image may seem a bit unexpected - most probably nothing will be seen
	/// (in the case if image is displayed according to its alpha channel). This may be
	/// caused by the fact that after differencing the entire alpha channel will be zeroed
	/// (zero difference between alpha channels), what means that the resulting image will be
	/// 100% transparent.</note></para>
	/// 
	/// <para>Sample usage:</para>
	/// <code>
	/// // create filter
	/// Difference filter = new Difference( overlayImage );
	/// // apply the filter
	/// Bitmap resultImage = filter.Apply( sourceImage );
	/// </code>
	///
	/// <para><b>Source image:</b></para>
	/// <img src="img/imaging/sample6.png" width="320" height="240" />
	/// <para><b>Overlay image:</b></para>
	/// <img src="img/imaging/sample7.png" width="320" height="240" />
	/// <para><b>Result image:</b></para>
	/// <img src="img/imaging/difference.png" width="320" height="240" />
	/// </remarks>
	///
	/// <seealso cref="Intersect"/>
	/// <seealso cref="Merge"/>
	/// <seealso cref="Add"/>
	/// <seealso cref="Subtract"/>
	/// 
	public sealed class Difference: BaseInPlaceFilter2
	{
		// private format translation dictionary
		private Dictionary<PixelFormat,PixelFormat> formatTranslations = new Dictionary<PixelFormat,PixelFormat>();

		/// <summary>
		/// Format translations dictionary.
		/// </summary>
		public override Dictionary<PixelFormat,PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Difference"/> class.
		/// </summary>
		public Difference()
		{
			InitFormatTranslations();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Difference"/> class.
		/// </summary>
		/// 
		/// <param name="overlayImage">Overlay image.</param>
		/// 
		public Difference(Bitmap overlayImage)
			: base(overlayImage)
		{
			InitFormatTranslations();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Difference"/> class.
		/// </summary>
		/// 
		/// <param name="unmanagedOverlayImage">Unmanaged overlay image.</param>
		/// 
		public Difference(UnmanagedImage unmanagedOverlayImage)
			: base(unmanagedOverlayImage)
		{
			InitFormatTranslations();
		}

		// Initialize format translation dictionary
		private void InitFormatTranslations()
		{
			formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
			formatTranslations[PixelFormat.Format16bppGrayScale] = PixelFormat.Format16bppGrayScale;
			formatTranslations[PixelFormat.Format48bppRgb] = PixelFormat.Format48bppRgb;
			formatTranslations[PixelFormat.Format64bppArgb] = PixelFormat.Format64bppArgb;
		}

		/// <summary>
		/// Process the filter on the specified image.
		/// </summary>
		/// 
		/// <param name="image">Source image data.</param>
		/// <param name="overlay">Overlay image data.</param>
		///
		protected override unsafe void ProcessFilter(UnmanagedImage image,UnmanagedImage overlay)
		{
			var pixelFormat = image.PixelFormat;
			// get image dimension
			var width = image.Width;
			var height = image.Height;
			// pixel value
			int v;

			if (
				(pixelFormat == PixelFormat.Format8bppIndexed) ||
				(pixelFormat == PixelFormat.Format24bppRgb) ||
				(pixelFormat == PixelFormat.Format32bppRgb) ||
				(pixelFormat == PixelFormat.Format32bppArgb))
			{
				// initialize other variables
				var pixelSize = (pixelFormat == PixelFormat.Format8bppIndexed) ? 1 :
					(pixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
				var lineSize = width * pixelSize;
				var srcOffset = image.Stride - lineSize;
				var ovrOffset = overlay.Stride - lineSize;

				// do the job
				var ptr = (byte*)image.ImageData.ToPointer();
				var ovr = (byte*)overlay.ImageData.ToPointer();

				// for each line
				for (var y = 0; y < height; y++)
				{
					// for each pixel
					for (var x = 0; x < lineSize; x++, ptr++, ovr++)
					{
						// abs(sub)
						v = *ptr - *ovr;
						*ptr = (v < 0) ? (byte)-v : (byte)v;
					}
					ptr += srcOffset;
					ovr += ovrOffset;
				}
			}
			else
			{
				// initialize other variables
				var pixelSize = (pixelFormat == PixelFormat.Format16bppGrayScale) ? 1 :
					(pixelFormat == PixelFormat.Format48bppRgb) ? 3 : 4;
				var lineSize = width * pixelSize;
				var srcStride = image.Stride;
				var ovrStride = overlay.Stride;

				// do the job
				var basePtr = (byte*)image.ImageData.ToPointer();
				var baseOvr = (byte*)overlay.ImageData.ToPointer();

				// for each line
				for (var y = 0; y < height; y++)
				{
					var ptr = (ushort*)(basePtr + y * srcStride);
					var ovr = (ushort*)(baseOvr + y * ovrStride);

					// for each pixel
					for (var x = 0; x < lineSize; x++, ptr++, ovr++)
					{
						// abs(sub)
						v = *ptr - *ovr;
						*ptr = (v < 0) ? (ushort)-v : (ushort)v;
					}
				}
			}
		}
	}
}

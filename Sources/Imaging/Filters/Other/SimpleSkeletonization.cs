// AForge Image Processing Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Andrew Kirillov, 2005-2009
// andrew.kirillov@aforgenet.com
//

namespace AForge.Imaging.Filters
{
	using System.Collections.Generic;
	using System.Drawing;
	using System.Drawing.Imaging;

	/// <summary>
	/// Simple skeletonization filter.
	/// </summary>
	/// 
	/// <remarks><para>The filter build simple objects' skeletons by thinning them until
	/// they have one pixel wide "bones" horizontally and vertically. The filter uses
	/// <see cref="Background"/> and <see cref="Foreground"/> colors to distinguish
	/// between object and background.</para>
	/// 
	/// <para>The filter accepts 8 bpp grayscale images for processing.</para>
	/// 
	/// <para>Sample usage:</para>
	/// <code>
	/// // create filter
	/// SimpleSkeletonization filter = new SimpleSkeletonization( );
	/// // apply the filter
	/// filter.ApplyInPlace( image );
	/// </code>
	/// 
	/// <para><b>Initial image:</b></para>
	/// <img src="img/imaging/sample14.png" width="150" height="150" />
	/// <para><b>Result image:</b></para>
	/// <img src="img/imaging/simple_skeletonization.png" width="150" height="150" />
	/// </remarks>
	/// 
	public class SimpleSkeletonization: BaseUsingCopyPartialFilter
	{
		private byte bg = 0;
		private byte fg = 255;

		// private format translation dictionary
		private Dictionary<PixelFormat,PixelFormat> formatTranslations = new Dictionary<PixelFormat,PixelFormat>();

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
		/// Background pixel color.
		/// </summary>
		/// 
		/// <remarks><para>The property sets background (none object) color to look for.</para>
		/// 
		/// <para>Default value is set to <b>0</b> - black.</para></remarks>
		/// 
		public byte Background
		{
			get { return bg; }
			set { bg = value; }
		}

		/// <summary>
		/// Foreground pixel color.
		/// </summary>
		/// 
		/// <remarks><para>The property sets objects' (none background) color to look for.</para>
		/// 
		/// <para>Default value is set to <b>255</b> - white.</para></remarks>
		/// 
		public byte Foreground
		{
			get { return fg; }
			set { fg = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleSkeletonization"/> class.
		/// </summary>
		public SimpleSkeletonization()
		{
			formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleSkeletonization"/> class.
		/// </summary>
		/// 
		/// <param name="bg">Background pixel color.</param>
		/// <param name="fg">Foreground pixel color.</param>
		/// 
		public SimpleSkeletonization(byte bg,byte fg) : this()
		{
			this.bg = bg;
			this.fg = fg;
		}

		/// <summary>
		/// Process the filter on the specified image.
		/// </summary>
		/// 
		/// <param name="source">Source image data.</param>
		/// <param name="destination">Destination image data.</param>
		/// <param name="rect">Image rectangle for processing by the filter.</param>
		/// 
		protected override unsafe void ProcessFilter(UnmanagedImage source,UnmanagedImage destination,Rectangle rect)
		{
			// processing start and stop X,Y positions
			var startX = rect.Left;
			var startY = rect.Top;
			var stopX = startX + rect.Width;
			var stopY = startY + rect.Height;

			var srcStride = source.Stride;
			var dstStride = destination.Stride;
			var srcOffset = srcStride - rect.Width;

			int start;

			// do the job
			var src0 = (byte*)source.ImageData.ToPointer();
			var dst0 = (byte*)destination.ImageData.ToPointer();
			var src = src0;
			var dst = dst0;

			// horizontal pass

			// allign pointers to the first pixel to process
			src += (startY * srcStride + startX);
			dst += (startY * dstStride);

			// for each line
			for (var y = startY; y < stopY; y++)
			{
				// make destination image filled with background color
				AForge.SystemTools.SetUnmanagedMemory(dst + startX,bg,stopX - startX);

				start = -1;
				// for each pixel
				for (var x = startX; x < stopX; x++, src++)
				{
					// looking for foreground pixel
					if (start == -1)
					{
						if (*src == fg)
							start = x;
						continue;
					}

					// looking for non foreground pixel
					if (*src != fg)
					{
						dst[start + ((x - start) >> 1)] = fg;
						start = -1;
					}
				}
				if (start != -1)
				{
					dst[start + ((stopX - start) >> 1)] = fg;
				}
				src += srcOffset;
				dst += dstStride;
			}

			// vertical pass

			// allign pointer to the first line to process
			src0 += (startY * srcStride);

			// for each column
			for (var x = startX; x < stopX; x++)
			{
				src = src0 + x;
				dst = dst0 + x;

				start = -1;
				// for each row
				for (var y = startY; y < stopY; y++, src += srcStride)
				{
					// looking for foreground pixel
					if (start == -1)
					{
						if (*src == fg)
							start = y;
						continue;
					}

					// looking for non foreground pixel
					if (*src != fg)
					{
						dst[dstStride * (start + ((y - start) >> 1))] = fg;
						start = -1;
					}
				}
				if (start != -1)
				{
					dst[dstStride * (start + ((stopY - start) >> 1))] = fg;
				}
			}
		}
	}
}

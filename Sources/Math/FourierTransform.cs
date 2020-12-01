// AForge Math Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Andrew Kirillov, 2005-2009
// andrew.kirillov@aforgenet.com
//
// FFT idea from Exocortex.DSP library
// http://www.exocortex.org/dsp/
//

namespace AForge.Math
{
	using System;

	/// <summary>
	/// Fourier transformation.
	/// </summary>
	/// 
	/// <remarks>The class implements one dimensional and two dimensional
	/// Discrete and Fast Fourier Transformation.</remarks>
	/// 
	public static class FourierTransform
	{
		/// <summary>
		/// Fourier transformation direction.
		/// </summary>
		public enum Direction
		{
			/// <summary>
			/// Forward direction of Fourier transformation.
			/// </summary>
			Forward = 1,

			/// <summary>
			/// Backward direction of Fourier transformation.
			/// </summary>
			Backward = -1
		};

		/// <summary>
		/// One dimensional Discrete Fourier Transform.
		/// </summary>
		/// 
		/// <param name="data">Data to transform.</param>
		/// <param name="direction">Transformation direction.</param>
		/// 
		public static void DFT(Complex[] data,Direction direction)
		{
			var n = data.Length;
			double arg, cos, sin;
			var dst = new Complex[n];

			// for each destination element
			for (var i = 0; i < n; i++)
			{
				dst[i] = Complex.Zero;

				arg = -(int)direction * 2.0 * System.Math.PI * i / n;

				// sum source elements
				for (var j = 0; j < n; j++)
				{
					cos = System.Math.Cos(j * arg);
					sin = System.Math.Sin(j * arg);

					dst[i].Re += (data[j].Re * cos - data[j].Im * sin);
					dst[i].Im += (data[j].Re * sin + data[j].Im * cos);
				}
			}

			// copy elements
			if (direction == Direction.Forward)
			{
				// devide also for forward transform
				for (var i = 0; i < n; i++)
				{
					data[i].Re = dst[i].Re / n;
					data[i].Im = dst[i].Im / n;
				}
			}
			else
			{
				for (var i = 0; i < n; i++)
				{
					data[i].Re = dst[i].Re;
					data[i].Im = dst[i].Im;
				}
			}
		}

		/// <summary>
		/// Two dimensional Discrete Fourier Transform.
		/// </summary>
		/// 
		/// <param name="data">Data to transform.</param>
		/// <param name="direction">Transformation direction.</param>
		/// 
		public static void DFT2(Complex[,] data,Direction direction)
		{
			var n = data.GetLength(0);  // rows
			var m = data.GetLength(1);  // columns
			double arg, cos, sin;
			var dst = new Complex[System.Math.Max(n,m)];

			// process rows
			for (var i = 0; i < n; i++)
			{
				for (var j = 0; j < m; j++)
				{
					dst[j] = Complex.Zero;

					arg = -(int)direction * 2.0 * System.Math.PI * j / m;

					// sum source elements
					for (var k = 0; k < m; k++)
					{
						cos = System.Math.Cos(k * arg);
						sin = System.Math.Sin(k * arg);

						dst[j].Re += (data[i,k].Re * cos - data[i,k].Im * sin);
						dst[j].Im += (data[i,k].Re * sin + data[i,k].Im * cos);
					}
				}

				// copy elements
				if (direction == Direction.Forward)
				{
					// devide also for forward transform
					for (var j = 0; j < m; j++)
					{
						data[i,j].Re = dst[j].Re / m;
						data[i,j].Im = dst[j].Im / m;
					}
				}
				else
				{
					for (var j = 0; j < m; j++)
					{
						data[i,j].Re = dst[j].Re;
						data[i,j].Im = dst[j].Im;
					}
				}
			}

			// process columns
			for (var j = 0; j < m; j++)
			{
				for (var i = 0; i < n; i++)
				{
					dst[i] = Complex.Zero;

					arg = -(int)direction * 2.0 * System.Math.PI * i / n;

					// sum source elements
					for (var k = 0; k < n; k++)
					{
						cos = System.Math.Cos(k * arg);
						sin = System.Math.Sin(k * arg);

						dst[i].Re += (data[k,j].Re * cos - data[k,j].Im * sin);
						dst[i].Im += (data[k,j].Re * sin + data[k,j].Im * cos);
					}
				}

				// copy elements
				if (direction == Direction.Forward)
				{
					// devide also for forward transform
					for (var i = 0; i < n; i++)
					{
						data[i,j].Re = dst[i].Re / n;
						data[i,j].Im = dst[i].Im / n;
					}
				}
				else
				{
					for (var i = 0; i < n; i++)
					{
						data[i,j].Re = dst[i].Re;
						data[i,j].Im = dst[i].Im;
					}
				}
			}
		}


		/// <summary>
		/// One dimensional Fast Fourier Transform.
		/// </summary>
		/// 
		/// <param name="data">Data to transform.</param>
		/// <param name="direction">Transformation direction.</param>
		/// 
		/// <remarks><para><note>The method accepts <paramref name="data"/> array of 2<sup>n</sup> size
		/// only, where <b>n</b> may vary in the [1, 14] range.</note></para></remarks>
		/// 
		/// <exception cref="ArgumentException">Incorrect data length.</exception>
		/// 
		public static void FFT(Complex[] data,Direction direction)
		{
			var n = data.Length;
			var m = Tools.Log2(n);

			// reorder data first
			ReorderData(data);

			// compute FFT
			int tn = 1, tm;

			for (var k = 1; k <= m; k++)
			{
				var rotation = FourierTransform.GetComplexRotation(k,direction);

				tm = tn;
				tn <<= 1;

				for (var i = 0; i < tm; i++)
				{
					var t = rotation[i];

					for (var even = i; even < n; even += tn)
					{
						var odd = even + tm;
						var ce = data[even];
						var co = data[odd];

						var tr = co.Re * t.Re - co.Im * t.Im;
						var ti = co.Re * t.Im + co.Im * t.Re;

						data[even].Re += tr;
						data[even].Im += ti;

						data[odd].Re = ce.Re - tr;
						data[odd].Im = ce.Im - ti;
					}
				}
			}

			if (direction == Direction.Forward)
			{
				for (var i = 0; i < n; i++)
				{
					data[i].Re /= n;
					data[i].Im /= n;
				}
			}
		}

		/// <summary>
		/// Two dimensional Fast Fourier Transform.
		/// </summary>
		/// 
		/// <param name="data">Data to transform.</param>
		/// <param name="direction">Transformation direction.</param>
		/// 
		/// <remarks><para><note>The method accepts <paramref name="data"/> array of 2<sup>n</sup> size
		/// only in each dimension, where <b>n</b> may vary in the [1, 14] range. For example, 16x16 array
		/// is valid, but 15x15 is not.</note></para></remarks>
		/// 
		/// <exception cref="ArgumentException">Incorrect data length.</exception>
		/// 
		public static void FFT2(Complex[,] data,Direction direction)
		{
			var k = data.GetLength(0);
			var n = data.GetLength(1);

			// check data size
			if (
				(!Tools.IsPowerOf2(k)) ||
				(!Tools.IsPowerOf2(n)) ||
				(k < minLength) || (k > maxLength) ||
				(n < minLength) || (n > maxLength)
				)
			{
				throw new ArgumentException("Incorrect data length.");
			}

			// process rows
			var row = new Complex[n];

			for (var i = 0; i < k; i++)
			{
				// copy row
				for (var j = 0; j < n; j++)
					row[j] = data[i,j];
				// transform it
				FourierTransform.FFT(row,direction);
				// copy back
				for (var j = 0; j < n; j++)
					data[i,j] = row[j];
			}

			// process columns
			var col = new Complex[k];

			for (var j = 0; j < n; j++)
			{
				// copy column
				for (var i = 0; i < k; i++)
					col[i] = data[i,j];
				// transform it
				FourierTransform.FFT(col,direction);
				// copy back
				for (var i = 0; i < k; i++)
					data[i,j] = col[i];
			}
		}

		#region Private Region

		private const int minLength = 2;
		private const int maxLength = 16384;
		private const int minBits = 1;
		private const int maxBits = 14;
		private static int[][] reversedBits = new int[maxBits][];
		private static Complex[,][] complexRotation = new Complex[maxBits,2][];

		// Get array, indicating which data members should be swapped before FFT
		private static int[] GetReversedBits(int numberOfBits)
		{
			if ((numberOfBits < minBits) || (numberOfBits > maxBits))
				throw new ArgumentOutOfRangeException();

			// check if the array is already calculated
			if (reversedBits[numberOfBits - 1] == null)
			{
				var n = Tools.Pow2(numberOfBits);
				var rBits = new int[n];

				// calculate the array
				for (var i = 0; i < n; i++)
				{
					var oldBits = i;
					var newBits = 0;

					for (var j = 0; j < numberOfBits; j++)
					{
						newBits = (newBits << 1) | (oldBits & 1);
						oldBits = (oldBits >> 1);
					}
					rBits[i] = newBits;
				}
				reversedBits[numberOfBits - 1] = rBits;
			}
			return reversedBits[numberOfBits - 1];
		}

		// Get rotation of complex number
		private static Complex[] GetComplexRotation(int numberOfBits,Direction direction)
		{
			var directionIndex = (direction == Direction.Forward) ? 0 : 1;

			// check if the array is already calculated
			if (complexRotation[numberOfBits - 1,directionIndex] == null)
			{
				var n = 1 << (numberOfBits - 1);
				var uR = 1.0;
				var uI = 0.0;
				var angle = System.Math.PI / n * (int)direction;
				var wR = System.Math.Cos(angle);
				var wI = System.Math.Sin(angle);
				double t;
				var rotation = new Complex[n];

				for (var i = 0; i < n; i++)
				{
					rotation[i] = new Complex(uR,uI);
					t = uR * wI + uI * wR;
					uR = uR * wR - uI * wI;
					uI = t;
				}

				complexRotation[numberOfBits - 1,directionIndex] = rotation;
			}
			return complexRotation[numberOfBits - 1,directionIndex];
		}

		// Reorder data for FFT using
		private static void ReorderData(Complex[] data)
		{
			var len = data.Length;

			// check data length
			if ((len < minLength) || (len > maxLength) || (!Tools.IsPowerOf2(len)))
				throw new ArgumentException("Incorrect data length.");

			var rBits = GetReversedBits(Tools.Log2(len));

			for (var i = 0; i < len; i++)
			{
				var s = rBits[i];

				if (s > i)
				{
					var t = data[i];
					data[i] = data[s];
					data[s] = t;
				}
			}
		}

		#endregion
	}
}

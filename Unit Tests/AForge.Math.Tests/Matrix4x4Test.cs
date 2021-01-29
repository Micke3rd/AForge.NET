using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AForge.Math.Tests
{
	[TestClass]
	public class Matrix4x4Test
	{
		private const float Epsilon = 0.000001f;

		private Matrix4x4 a1 = new Matrix4x4();
		private Matrix4x4 a2 = new Matrix4x4();

		public Matrix4x4Test()
		{
			// prepare 1st argument
			a1.V00 = 1;
			a1.V01 = 2;
			a1.V02 = 3;
			a1.V03 = 4;

			a1.V10 = 4;
			a1.V11 = 3;
			a1.V12 = 2;
			a1.V13 = 1;

			a1.V20 = 3;
			a1.V21 = 1;
			a1.V22 = 4;
			a1.V23 = 2;

			a1.V30 = 2;
			a1.V31 = 4;
			a1.V32 = 1;
			a1.V33 = 3;

			// prepare 2nd argument
			a2.V00 = 2;
			a2.V01 = 1;
			a2.V02 = 4;
			a2.V03 = 3;

			a2.V10 = 4;
			a2.V11 = 2;
			a2.V12 = 3;
			a2.V13 = 1;

			a2.V20 = 3;
			a2.V21 = 4;
			a2.V22 = 1;
			a2.V23 = 2;

			a2.V30 = 1;
			a2.V31 = 3;
			a2.V32 = 2;
			a2.V33 = 4;
		}

		[TestMethod]
		public void ToArrayTest()
		{
			var matrix = new Matrix4x4
			{
				V00 = 1,
				V01 = 2,
				V02 = 3,
				V03 = 4,

				V10 = 5,
				V11 = 6,
				V12 = 7,
				V13 = 8,

				V20 = 9,
				V21 = 10,
				V22 = 11,
				V23 = 12,

				V30 = 13,
				V31 = 14,
				V32 = 15,
				V33 = 16
			};

			var array = matrix.ToArray();

			for (var i = 0; i < 16; i++)
			{
				Assert.AreEqual(array[i],i + 1);
			}
		}

		[TestMethod]
		public void CreateFromRowsTest()
		{
			var row0 = new Vector4(1,2,3,4);
			var row1 = new Vector4(5,6,7,8);
			var row2 = new Vector4(9,10,11,12);
			var row3 = new Vector4(13,14,15,16);
			var matrix = Matrix4x4.CreateFromRows(row0,row1,row2,row3);

			var array = matrix.ToArray();

			for (var i = 0; i < 16; i++)
			{
				Assert.AreEqual(array[i],i + 1);
			}

			Assert.AreEqual(row0,matrix.GetRow(0));
			Assert.AreEqual(row1,matrix.GetRow(1));
			Assert.AreEqual(row2,matrix.GetRow(2));
			Assert.AreEqual(row3,matrix.GetRow(3));


			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetRow(-1);
		   }
			);

			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetRow(4);
		   }
			);
		}

		[TestMethod]
		public void CreateFromColumnsTest()
		{
			var column0 = new Vector4(1,5,9,13);
			var column1 = new Vector4(2,6,10,14);
			var column2 = new Vector4(3,7,11,15);
			var column3 = new Vector4(4,8,12,16);
			var matrix = Matrix4x4.CreateFromColumns(column0,column1,column2,column3);

			var array = matrix.ToArray();

			for (var i = 0; i < 16; i++)
			{
				Assert.AreEqual(array[i],i + 1);
			}

			Assert.AreEqual(column0,matrix.GetColumn(0));
			Assert.AreEqual(column1,matrix.GetColumn(1));
			Assert.AreEqual(column2,matrix.GetColumn(2));
			Assert.AreEqual(column3,matrix.GetColumn(3));

			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetColumn(-1);
		   }
			);

			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetColumn(4);
		   }
			);
		}

		[DataTestMethod]
		[DataRow(0)]
		[DataRow(30)]
		[DataRow(45)]
		[DataRow(60)]
		[DataRow(90)]
		[DataRow(-30)]
		[DataRow(-90)]
		[DataRow(-180)]
		public void CreateRotationYTest(float angle)
		{
			var radians = (float)(angle * System.Math.PI / 180);
			var matrix = Matrix4x4.CreateRotationY(radians);

			var sin = (float)System.Math.Sin(radians);
			var cos = (float)System.Math.Cos(radians);

			var expectedArray = new float[16]
			{
				cos, 0, sin, 0,
				0, 1, 0, 0,
				-sin, 0, cos, 0,
				0, 0, 0, 1
			};

			CompareMatrixWithArray(matrix,expectedArray);
		}

		[DataTestMethod]
		[DataRow(0)]
		[DataRow(30)]
		[DataRow(45)]
		[DataRow(60)]
		[DataRow(90)]
		[DataRow(-30)]
		[DataRow(-90)]
		[DataRow(-180)]
		public void CreateRotationXTest(float angle)
		{
			var radians = (float)(angle * System.Math.PI / 180);
			var matrix = Matrix4x4.CreateRotationX(radians);

			var sin = (float)System.Math.Sin(radians);
			var cos = (float)System.Math.Cos(radians);

			var expectedArray = new float[16]
			{
				1, 0, 0, 0,
				0, cos, -sin, 0,
				0, sin, cos, 0,
				0, 0, 0, 1
			};

			CompareMatrixWithArray(matrix,expectedArray);
		}

		[DataTestMethod]
		[DataRow(0)]
		[DataRow(30)]
		[DataRow(45)]
		[DataRow(60)]
		[DataRow(90)]
		[DataRow(-30)]
		[DataRow(-90)]
		[DataRow(-180)]
		public void CreateRotationZTest(float angle)
		{
			var radians = (float)(angle * System.Math.PI / 180);
			var matrix = Matrix4x4.CreateRotationZ(radians);

			var sin = (float)System.Math.Sin(radians);
			var cos = (float)System.Math.Cos(radians);

			var expectedArray = new float[16]
			{
				cos, -sin, 0, 0,
				sin, cos, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1,
			};

			CompareMatrixWithArray(matrix,expectedArray);
		}

		[DataTestMethod]
		[DataRow(0,0,0)]
		[DataRow(30,45,60)]
		[DataRow(45,60,30)]
		[DataRow(60,30,45)]
		[DataRow(90,90,90)]
		[DataRow(-30,-60,-90)]
		[DataRow(-90,-135,-180)]
		[DataRow(-180,-30,-60)]
		public void CreateFromYawPitchRollTest(float yaw,float pitch,float roll)
		{
			var radiansYaw = (float)(yaw * System.Math.PI / 180);
			var radiansPitch = (float)(pitch * System.Math.PI / 180);
			var radiansRoll = (float)(roll * System.Math.PI / 180);

			var matrix = Matrix4x4.CreateFromYawPitchRoll(radiansYaw,radiansPitch,radiansRoll);

			var xMatrix = Matrix4x4.CreateRotationX(radiansPitch);
			var yMatrix = Matrix4x4.CreateRotationY(radiansYaw);
			var zMatrix = Matrix4x4.CreateRotationZ(radiansRoll);

			var rotationMatrix = (yMatrix * xMatrix) * zMatrix;

			CompareMatrixWithArray(matrix,rotationMatrix.ToArray());
		}

		[DataTestMethod]
		[DataRow(0,0,0)]
		[DataRow(30,45,60)]
		[DataRow(45,60,30)]
		[DataRow(60,30,45)]
		[DataRow(-30,-60,-90)]
		public void ExtractYawPitchRollTest(float yaw,float pitch,float roll)
		{
			var radiansYaw = (float)(yaw * System.Math.PI / 180);
			var radiansPitch = (float)(pitch * System.Math.PI / 180);
			var radiansRoll = (float)(roll * System.Math.PI / 180);

			var matrix = Matrix4x4.CreateFromYawPitchRoll(radiansYaw,radiansPitch,radiansRoll);

			float extractedYaw;
			float extractedPitch;
			float extractedRoll;

			matrix.ExtractYawPitchRoll(out extractedYaw,out extractedPitch,out extractedRoll);

			Assert.AreEqual(radiansYaw,extractedYaw,Epsilon);
			Assert.AreEqual(radiansPitch,extractedPitch,Epsilon);
			Assert.AreEqual(radiansRoll,extractedRoll,Epsilon);
		}

		[DataTestMethod]
		[DataRow(1,2,3,4)]
		[DataRow(-1,-2,-3,-4)]
		public void CreateDiagonalTest(float v00,float v11,float v22,float v33)
		{
			var diagonal = new Vector4(v00,v11,v22,v33);
			var matrix = Matrix4x4.CreateDiagonal(diagonal);

			var expectedArray = new float[16] { v00,0,0,0,0,v11,0,0,0,0,v22,0,0,0,0,v33 };

			CompareMatrixWithArray(matrix,expectedArray);
		}

		[TestMethod]
		public void AddMatricesTest()
		{
			var expectedResult = new Matrix4x4
			{
				V00 = 3,
				V01 = 3,
				V02 = 7,
				V03 = 7,

				V10 = 8,
				V11 = 5,
				V12 = 5,
				V13 = 2,

				V20 = 6,
				V21 = 5,
				V22 = 5,
				V23 = 4,

				V30 = 3,
				V31 = 7,
				V32 = 3,
				V33 = 7
			};

			var result = a1 + a2;

			Assert.AreEqual(true, ApproximateEquals(result,expectedResult));
		}

		[TestMethod]
		public void SubtractMatricesTest()
		{
			var expectedResult = new Matrix4x4
			{
				V00 = -1,
				V01 = 1,
				V02 = -1,
				V03 = 1,

				V10 = 0,
				V11 = 1,
				V12 = -1,
				V13 = 0,

				V20 = 0,
				V21 = -3,
				V22 = 3,
				V23 = 0,

				V30 = 1,
				V31 = 1,
				V32 = -1,
				V33 = -1
			};

			var result = a1 - a2;

			Assert.AreEqual(true, ApproximateEquals(result,expectedResult));
		}

		[TestMethod]
		public void MultiplyMatricesTest()
		{
			var expectedResult = new Matrix4x4
			{
				V00 = 23,
				V01 = 29,
				V02 = 21,
				V03 = 27,

				V10 = 27,
				V11 = 21,
				V12 = 29,
				V13 = 23,

				V20 = 24,
				V21 = 27,
				V22 = 23,
				V23 = 26,

				V30 = 26,
				V31 = 23,
				V32 = 27,
				V33 = 24
			};

			var result = a1 * a2;

			Assert.AreEqual(true, ApproximateEquals(result,expectedResult));
		}

		private static void CompareMatrixWithArray(Matrix4x4 matrix,float[] array)
		{
			var matrixArray = matrix.ToArray();

			for (var i = 0; i < 16; i++)
			{
				Assert.AreEqual(matrixArray[i],array[i]);
			}
		}

		private static bool ApproximateEquals(Matrix4x4 matrix1,Matrix4x4 matrix2)
		{
			// TODO: better algorithm should be put into the framework actually
			return (
				(System.Math.Abs(matrix1.V00 - matrix2.V00) <= Epsilon) &&
				(System.Math.Abs(matrix1.V01 - matrix2.V01) <= Epsilon) &&
				(System.Math.Abs(matrix1.V02 - matrix2.V02) <= Epsilon) &&
				(System.Math.Abs(matrix1.V03 - matrix2.V03) <= Epsilon) &&

				(System.Math.Abs(matrix1.V10 - matrix2.V10) <= Epsilon) &&
				(System.Math.Abs(matrix1.V11 - matrix2.V11) <= Epsilon) &&
				(System.Math.Abs(matrix1.V12 - matrix2.V12) <= Epsilon) &&
				(System.Math.Abs(matrix1.V13 - matrix2.V13) <= Epsilon) &&

				(System.Math.Abs(matrix1.V20 - matrix2.V20) <= Epsilon) &&
				(System.Math.Abs(matrix1.V21 - matrix2.V21) <= Epsilon) &&
				(System.Math.Abs(matrix1.V22 - matrix2.V22) <= Epsilon) &&
				(System.Math.Abs(matrix1.V23 - matrix2.V23) <= Epsilon) &&

				(System.Math.Abs(matrix1.V30 - matrix2.V30) <= Epsilon) &&
				(System.Math.Abs(matrix1.V31 - matrix2.V31) <= Epsilon) &&
				(System.Math.Abs(matrix1.V32 - matrix2.V32) <= Epsilon) &&
				(System.Math.Abs(matrix1.V33 - matrix2.V33) <= Epsilon)
			);
		}
	}
}

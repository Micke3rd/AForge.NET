using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AForge.Math.Tests
{
	[TestClass]
	public class Matrix3x3Test
	{
		private const float Epsilon = 0.000001f;

		private Matrix3x3 a1 = new Matrix3x3();
		private Matrix3x3 a2 = new Matrix3x3();

		public Matrix3x3Test()
		{
			// prepare 1st argument
			a1.V00 = 1;
			a1.V01 = 2;
			a1.V02 = 3;

			a1.V10 = 3;
			a1.V11 = 2;
			a1.V12 = 1;

			a1.V20 = 1;
			a1.V21 = 3;
			a1.V22 = 2;

			// prepare 2nd argument
			a2.V00 = 2;
			a2.V01 = 1;
			a2.V02 = 3;

			a2.V10 = 1;
			a2.V11 = 3;
			a2.V12 = 2;

			a2.V20 = 3;
			a2.V21 = 2;
			a2.V22 = 1;
		}

		[TestMethod]
		public void ToArrayTest()
		{
			var matrix = new Matrix3x3
			{
				V00 = 1,
				V01 = 2,
				V02 = 3,

				V10 = 4,
				V11 = 5,
				V12 = 6,

				V20 = 7,
				V21 = 8,
				V22 = 9
			};

			var array = matrix.ToArray();

			for (var i = 0; i < 9; i++)
			{
				Assert.AreEqual(array[i],i + 1);
			}
		}

		[TestMethod]
		public void CreateFromRowsTest()
		{
			var row0 = new Vector3(1,2,3);
			var row1 = new Vector3(4,5,6);
			var row2 = new Vector3(7,8,9);
			var matrix = Matrix3x3.CreateFromRows(row0,row1,row2);

			var array = matrix.ToArray();

			for (var i = 0; i < 9; i++)
			{
				Assert.AreEqual(array[i],i + 1);
			}

			Assert.AreEqual(row0,matrix.GetRow(0));
			Assert.AreEqual(row1,matrix.GetRow(1));
			Assert.AreEqual(row2,matrix.GetRow(2));

			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetRow(-1);
		   }
			);

			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetRow(3);
		   }
			);
		}

		[TestMethod]
		public void CreateFromColumnsTest()
		{
			var column0 = new Vector3(1,4,7);
			var column1 = new Vector3(2,5,8);
			var column2 = new Vector3(3,6,9);
			var matrix = Matrix3x3.CreateFromColumns(column0,column1,column2);

			var array = matrix.ToArray();

			for (var i = 0; i < 9; i++)
			{
				Assert.AreEqual(array[i],i + 1);
			}

			Assert.AreEqual(column0,matrix.GetColumn(0));
			Assert.AreEqual(column1,matrix.GetColumn(1));
			Assert.AreEqual(column2,matrix.GetColumn(2));

			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetColumn(-1);
		   }
			);

			Assert.ThrowsException<ArgumentException>(() =>
		   {
			   matrix.GetColumn(3);
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
			var matrix = Matrix3x3.CreateRotationY(radians);

			var sin = (float)System.Math.Sin(radians);
			var cos = (float)System.Math.Cos(radians);

			var expectedArray = new float[9]
			{
				cos, 0, sin, 0, 1, 0, -sin, 0, cos
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
			var matrix = Matrix3x3.CreateRotationX(radians);

			var sin = (float)System.Math.Sin(radians);
			var cos = (float)System.Math.Cos(radians);

			var expectedArray = new float[9]
			{
				1, 0, 0, 0, cos, -sin, 0, sin, cos
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
			var matrix = Matrix3x3.CreateRotationZ(radians);

			var sin = (float)System.Math.Sin(radians);
			var cos = (float)System.Math.Cos(radians);

			var expectedArray = new float[9]
			{
				cos, -sin, 0, sin, cos, 0, 0, 0, 1
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

			var matrix = Matrix3x3.CreateFromYawPitchRoll(radiansYaw,radiansPitch,radiansRoll);

			var xMatrix = Matrix3x3.CreateRotationX(radiansPitch);
			var yMatrix = Matrix3x3.CreateRotationY(radiansYaw);
			var zMatrix = Matrix3x3.CreateRotationZ(radiansRoll);

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

			var matrix = Matrix3x3.CreateFromYawPitchRoll(radiansYaw,radiansPitch,radiansRoll);

			float extractedYaw;
			float extractedPitch;
			float extractedRoll;

			matrix.ExtractYawPitchRoll(out extractedYaw,out extractedPitch,out extractedRoll);

			Assert.AreEqual(radiansYaw,extractedYaw,Epsilon);
			Assert.AreEqual(radiansPitch,extractedPitch,Epsilon);
			Assert.AreEqual(radiansRoll,extractedRoll,Epsilon);
		}

		[DataTestMethod]
		[DataRow(1,2,3)]
		[DataRow(-1,-2,-3)]
		public void CreateDiagonalTest(float v00,float v11,float v22)
		{
			var diagonal = new Vector3(v00,v11,v22);
			var matrix = Matrix3x3.CreateDiagonal(diagonal);

			var expectedArray = new float[9] { v00,0,0,0,v11,0,0,0,v22 };

			CompareMatrixWithArray(matrix,expectedArray);
		}

		[DataTestMethod]
		[DataRow(1,1,0,0,0,1,0,0,0,1)]
		[DataRow(0,1,0,0,0,1,0,1,0,0)]
		[DataRow(0,1,1,1,1,1,1,1,1,1)]
		[DataRow(-3,1,3,2,2,2,1,2,1,1)]
		public void DeterminantTest(float expectedDeterminant,
			float v00,float v01,float v02,
			float v10,float v11,float v12,
			float v20,float v21,float v22)
		{
			var matrix = new Matrix3x3
			{
				V00 = v00,
				V01 = v01,
				V02 = v02,

				V10 = v10,
				V11 = v11,
				V12 = v12,

				V20 = v20,
				V21 = v21,
				V22 = v22
			};

			Assert.AreEqual(expectedDeterminant,matrix.Determinant);
		}

		[DataTestMethod]
		[DataRow(1,0,0,0,1,0,0,0,1)]
		//[DataRow( 1, 0, 0, 0, 1, 0, 1, 0, 0, ExpectedException = typeof( ArgumentException ) )]        [DataRow( 2, 0, 0, 0, 4, 0, 0, 0, 3 )]        [DataRow( 1, 4, 2, 2, 2, 1, 2, 1, 1 )]
		public void InverseTest(float v00,float v01,float v02,float v10,float v11,float v12,float v20,float v21,float v22)
		{
			var matrix = new Matrix3x3
			{
				V00 = v00,
				V01 = v01,
				V02 = v02,

				V10 = v10,
				V11 = v11,
				V12 = v12,

				V20 = v20,
				V21 = v21,
				V22 = v22
			};

			var inverse = matrix.Inverse();
			var identity = matrix * inverse;

			Assert.AreEqual(true,ApproximateEquals(identity,Matrix3x3.Identity));
		}

		[TestMethod]
		public void AddMatricesTest()
		{
			var expectedResult = new Matrix3x3
			{
				V00 = 3,
				V01 = 3,
				V02 = 6,

				V10 = 4,
				V11 = 5,
				V12 = 3,

				V20 = 4,
				V21 = 5,
				V22 = 3
			};

			var result = a1 + a2;

			Assert.AreEqual(true,ApproximateEquals(result,expectedResult));
		}

		[TestMethod]
		public void SubtractMatricesTest()
		{
			var expectedResult = new Matrix3x3
			{
				V00 = -1,
				V01 = 1,
				V02 = 0,

				V10 = 2,
				V11 = -1,
				V12 = -1,

				V20 = -2,
				V21 = 1,
				V22 = 1
			};

			var result = a1 - a2;

			Assert.AreEqual(true,ApproximateEquals(result,expectedResult));
		}

		[TestMethod]
		public void MultiplyMatricesTest()
		{
			var expectedResult = new Matrix3x3
			{
				V00 = 13,
				V01 = 13,
				V02 = 10,

				V10 = 11,
				V11 = 11,
				V12 = 14,

				V20 = 11,
				V21 = 14,
				V22 = 11
			};

			var result = a1 * a2;

			Assert.AreEqual(true,ApproximateEquals(result,expectedResult));
		}

		private void CompareMatrixWithArray(Matrix3x3 matrix,float[] array)
		{
			var matrixArray = matrix.ToArray();

			for (var i = 0; i < 9; i++)
			{
				Assert.AreEqual(matrixArray[i],array[i]);
			}
		}

		private bool ApproximateEquals(Matrix3x3 matrix1,Matrix3x3 matrix2)
		{
			// TODO: better algorithm should be put into the framework actually
			return (
				(System.Math.Abs(matrix1.V00 - matrix2.V00) <= Epsilon) &&
				(System.Math.Abs(matrix1.V01 - matrix2.V01) <= Epsilon) &&
				(System.Math.Abs(matrix1.V02 - matrix2.V02) <= Epsilon) &&

				(System.Math.Abs(matrix1.V10 - matrix2.V10) <= Epsilon) &&
				(System.Math.Abs(matrix1.V11 - matrix2.V11) <= Epsilon) &&
				(System.Math.Abs(matrix1.V12 - matrix2.V12) <= Epsilon) &&

				(System.Math.Abs(matrix1.V20 - matrix2.V20) <= Epsilon) &&
				(System.Math.Abs(matrix1.V21 - matrix2.V21) <= Epsilon) &&
				(System.Math.Abs(matrix1.V22 - matrix2.V22) <= Epsilon)
			);
		}
	}
}

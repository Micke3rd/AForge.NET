using AForge.Math;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PoseEstimation
{
	public partial class WorldRendererControl: UserControl
	{
		private Vector3[] objectPoints = null;
		private Vector3[] objectScenePoints = null;
		private AForge.Point[] projectedPoints = null;
		private Color[] colors = null;
		private int[,] lines = null;


		private Matrix4x4 worldMatrix = new Matrix4x4();
		private Matrix4x4 viewMatrix = new Matrix4x4();
		private Matrix4x4 perspectiveMatrix = new Matrix4x4();

		public Matrix4x4 WorldMatrix
		{
			get { return worldMatrix; }
			set
			{
				worldMatrix = value;
				Recalculate();
			}
		}

		public Matrix4x4 ViewMatrix
		{
			get { return viewMatrix; }
			set
			{
				viewMatrix = value;
				Recalculate();
			}
		}

		public AForge.Point[] ProjectedPoints
		{
			get { return projectedPoints; }
		}

		public WorldRendererControl()
		{
			InitializeComponent();

			objectPoints = new Vector3[]
			{
				new Vector3( 0, 0, 0 ),
			};

			colors = new Color[]
			{
				Color.White,
			};

			lines = new int[0,2];

			// create default matrices
			worldMatrix = Matrix4x4.Identity;
			viewMatrix = Matrix4x4.CreateLookAt(new Vector3(0,0,5),new Vector3(0,0,0));
			perspectiveMatrix = Matrix4x4.CreatePerspective(1,1,1,1000);

			Recalculate();
		}

		public void SetObject(Vector3[] vertices,Color[] colors,int[,] ribs)
		{
			if (vertices.Length != colors.Length)
			{
				throw new ArgumentException("Number of colors must be equal to number of vertices.");
			}

			if (ribs.GetLength(1) != 2)
			{
				throw new ArgumentException("Ribs array must have 2 coordinates per rib.");
			}

			this.objectPoints = (Vector3[])vertices.Clone();
			this.colors = (Color[])colors.Clone();
			this.lines = (int[,])ribs.Clone();

			Recalculate();
		}

		private void Recalculate()
		{
			var pointsCount = objectPoints.Length;
			objectScenePoints = new Vector3[pointsCount];
			projectedPoints = new AForge.Point[pointsCount];

			var cx = ClientRectangle.Width / 2;
			var cy = ClientRectangle.Height / 2;

			for (var i = 0; i < pointsCount; i++)
			{
				objectScenePoints[i] = (perspectiveMatrix *
									   (viewMatrix *
									   (worldMatrix * objectPoints[i].ToVector4()))).ToVector3();

				projectedPoints[i] = new AForge.Point(
					(int)(cx * objectScenePoints[i].X),
					(int)(cy * objectScenePoints[i].Y));
			}

			Invalidate();
		}

		private void WorldRendererControl_Paint(object sender,PaintEventArgs e)
		{
			var g = e.Graphics;
			var linesPen = new Pen(Color.White);

			using (var brush = new SolidBrush(Color.Black))
			{
				g.FillRectangle(brush,this.ClientRectangle);
			}

			if (projectedPoints != null)
			{
				var cx = ClientRectangle.Width / 2;
				var cy = ClientRectangle.Height / 2;

				var screenPoints = new Point[projectedPoints.Length];

				for (int i = 0, n = projectedPoints.Length; i < n; i++)
				{
					screenPoints[i] = new Point((int)(cx + projectedPoints[i].X),
												 (int)(cy - projectedPoints[i].Y));
				}

				for (var i = 0; i < lines.GetLength(0); i++)
				{
					var lineStart = lines[i,0];
					var lineEnd = lines[i,1];

					if ((lineStart < projectedPoints.Length) && (lineEnd < projectedPoints.Length))
					{
						g.DrawLine(linesPen,screenPoints[lineStart],screenPoints[lineEnd]);
					}
				}

				for (var i = 0; i < projectedPoints.Length; i++)
				{
					using (var pointsBrush = new SolidBrush(colors[i]))
					{
						g.FillRectangle(pointsBrush,screenPoints[i].X - 2,screenPoints[i].Y - 2,5,5);
					}
				}
			}

			linesPen.Dispose();
		}
	}
}

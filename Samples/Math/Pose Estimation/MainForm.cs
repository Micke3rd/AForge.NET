﻿// 3D Pose Estimation sample application
// AForge.NET Framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2009-2011
// contacts@aforgenet.com
//

using AForge.Math;
using AForge.Math.Geometry;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PoseEstimation
{
    public partial class MainForm : Form
    {
        private int updating = 0;

        // object rotation
        private float yaw = 0.0f, pitch = 0.0f, roll = 0.0f;
        // object position
        private float xObject = 0.0f, yObject = 0.0f, zObject = 0.0f;
        // camera position
        private float xCamera = 0.0f, yCamera = 0.0f, zCamera = 5.0f;
        // the point camera looks at
        private float xLookAt = 0.0f, yLookAt = 0.0f, zLookAt = 0.0f;

        // object points for POSIT case
        private readonly Vector3[] positObject = new Vector3[4]
        {
            new Vector3( -0.5f, -0.5f,  0 ),
            new Vector3(  0.5f, -0.5f,  0 ),
            new Vector3( -0.5f,  0.5f,  0 ),
            new Vector3( -0.5f, -0.5f, -1 ),
        };

        // object points for CoPOSIT case
        private readonly Vector3[] copositObject = new Vector3[4]
        {
            new Vector3( -0.5f, -0.5f, 0 ),
            new Vector3(  0.5f, -0.5f, 0 ),
            new Vector3(  0.5f,  0.5f, 0 ),
            new Vector3( -0.5f,  0.5f, 0 ),
        };

        // colors for object's vertices
        private readonly Color[] vertexColors = new Color[]
        {
            Color.Yellow,
            Color.Blue,
            Color.Red,
            Color.Green
        };

        // list of ribs for the POSIT object
        private readonly int[,] positRibs = new int[3, 2]
        {
            { 0, 1 },
            { 0, 2 },
            { 0, 3 },
        };

        // list of ribs for the CoPOSIT object
        private readonly int[,] copositRibs = new int[4, 2]
        {
            { 0, 1 },
            { 1, 2 },
            { 2, 3 },
            { 3, 0 },
        };

        private Posit posit = null;
        private CoplanarPosit coposit = null;

        private Matrix4x4 transformationMatrix = Matrix4x4.Identity;
        private Matrix4x4 viewMatrix = Matrix4x4.Identity;

        public MainForm()
        {
            InitializeComponent();

            posit=new Posit(positObject, -200);
            coposit=new CoplanarPosit(copositObject, 200);
        }

        // Form got loaded
        private void MainForm_Load(object sender, EventArgs e)
        {
            StartUpdating();

            screenPoint1ColorLabel.BackColor=objectPoint1ColorLabel.BackColor=vertexColors[0];
            screenPoint2ColorLabel.BackColor=objectPoint2ColorLabel.BackColor=vertexColors[1];
            screenPoint3ColorLabel.BackColor=objectPoint3ColorLabel.BackColor=vertexColors[2];
            screenPoint4ColorLabel.BackColor=objectPoint4ColorLabel.BackColor=vertexColors[3];

            objectTypeCombo.SelectedIndex=0;

            yawBox.Text=yaw.ToString();
            pitchBox.Text=pitch.ToString();
            rollBox.Text=roll.ToString();

            xObjectBox.Text=xObject.ToString();
            yObjectBox.Text=yObject.ToString();
            zObjectBox.Text=zObject.ToString();

            xCameraBox.Text=xCamera.ToString();
            yCameraBox.Text=yCamera.ToString();
            zCameraBox.Text=zCamera.ToString();

            xLookAtBox.Text=xLookAt.ToString();
            yLookAtBox.Text=yLookAt.ToString();
            zLookAtBox.Text=zLookAt.ToString();

            EndUpdating();

            UpdateTransformationMatrix();
            UpdateViewMatrix();
        }


        private void StartUpdating()
        {
            updating++;
        }

        private void EndUpdating()
        {
            updating--;
        }

        private bool IsUpdating
        {
            get { return updating>0; }
        }

        // Parse float value from the text in the specified text box
        private bool GetFloatValue(TextBox textBox, ref float floatValue)
        {
            var ret = false;

            if (float.TryParse(textBox.Text, out floatValue))
            {
                errorProvider.Clear();
                ret=true;
            }
            else
            {
                errorProvider.SetError(textBox, "Failed parsing float value");
            }

            return ret;
        }

        // Yaw edit box has changed
        private void yawBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(yawBox, ref yaw))
                {
                    UpdateTransformationMatrix();
                }
            }
        }

        // Pitch edit box has changed
        private void pitchBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(pitchBox, ref pitch))
                {
                    UpdateTransformationMatrix();
                }
            }
        }

        // Roll edit box has changed
        private void rollBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(rollBox, ref roll))
                {
                    UpdateTransformationMatrix();
                }
            }
        }

        // Object X position edit box has changed
        private void xObjectBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(xObjectBox, ref xObject))
                {
                    UpdateTransformationMatrix();
                }
            }
        }

        // Object Y position edit box has changed
        private void yObjectBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(yObjectBox, ref yObject))
                {
                    UpdateTransformationMatrix();
                }
            }
        }

        // Object Z position edit box has changed
        private void zObjectBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(zObjectBox, ref zObject))
                {
                    UpdateTransformationMatrix();
                }
            }
        }

        // Camera's X position edit box has changed
        private void xCameraBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(xCameraBox, ref xCamera))
                {
                    UpdateViewMatrix();
                }
            }
        }

        // Camera's Y position edit box has changed
        private void yCameraBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(yCameraBox, ref yCamera))
                {
                    UpdateViewMatrix();
                }
            }
        }

        // Camera's Z position edit box has changed
        private void zCameraBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(zCameraBox, ref zCamera))
                {
                    UpdateViewMatrix();
                }
            }
        }

        // Camera's look at point's X position edit box has changed
        private void xLookAtBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(xLookAtBox, ref xLookAt))
                {
                    UpdateViewMatrix();
                }
            }
        }

        // Camera's look at point's Y position edit box has changed
        private void yLookAtBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(yLookAtBox, ref yLookAt))
                {
                    UpdateViewMatrix();
                }
            }
        }

        // Camera's look at point's Z position edit box has changed
        private void zLookAtBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (GetFloatValue(zLookAtBox, ref zLookAt))
                {
                    UpdateViewMatrix();
                }
            }
        }

        // Calculate new world transformation matrix
        private void UpdateTransformationMatrix()
        {
            var rYaw = (float)(yaw/180*Math.PI);
            var rPitch = (float)(pitch/180*Math.PI);
            var rRoll = (float)(roll/180*Math.PI);

            transformationMatrix=
                Matrix4x4.CreateTranslation(new Vector3(xObject, yObject, zObject))*
                Matrix4x4.CreateFromYawPitchRoll(rYaw, rPitch, rRoll);

            worldRendererControl.WorldMatrix=transformationMatrix;
            transformationMatrixControl.SetMatrix(transformationMatrix);
            targetTransformationMatrixControl.SetMatrix(viewMatrix*transformationMatrix);

            UpdateProjectedPoints();
            EstimatePose();
        }

        // Calculate new view transformation matrix
        private void UpdateViewMatrix()
        {
            viewMatrix=Matrix4x4.CreateLookAt(
                new Vector3(xCamera, yCamera, zCamera),
                new Vector3(xLookAt, yLookAt, zLookAt));

            worldRendererControl.ViewMatrix=viewMatrix;
            viewMatrixControl.SetMatrix(viewMatrix);
            targetTransformationMatrixControl.SetMatrix(viewMatrix*transformationMatrix);

            UpdateProjectedPoints();
            EstimatePose();
        }

        // Show current coordinates of object's projected points
        private void UpdateProjectedPoints()
        {
            var projectedPoints = worldRendererControl.ProjectedPoints;

            screenPoint1Box.Text=projectedPoints[0].ToString();
            screenPoint2Box.Text=projectedPoints[1].ToString();
            screenPoint3Box.Text=projectedPoints[2].ToString();
            screenPoint4Box.Text=projectedPoints[3].ToString();
        }

        // Object type has changed
        private void objectTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Vector3[] vertices = null;

            if (objectTypeCombo.SelectedIndex==0)
            {
                vertices=positObject;
                worldRendererControl.SetObject(vertices=positObject, vertexColors, positRibs);
            }
            else
            {
                vertices=copositObject;
                worldRendererControl.SetObject(vertices=copositObject, vertexColors, copositRibs);
            }

            objectPoint1Box.Text=vertices[0].ToString();
            objectPoint2Box.Text=vertices[1].ToString();
            objectPoint3Box.Text=vertices[2].ToString();
            objectPoint4Box.Text=vertices[3].ToString();

            UpdateProjectedPoints();
            EstimatePose();
        }

        // Estimate pose of the object from its screen (projected) points
        private void EstimatePose()
        {
            var projectedPoints = worldRendererControl.ProjectedPoints;
            var rotationMatrix = Matrix3x3.Identity;
            var translationVector = new Vector3(0);

            if (objectTypeCombo.SelectedIndex==0)
            {
                posit.EstimatePose(projectedPoints, out rotationMatrix, out translationVector);
            }
            else
            {
                coposit.EstimatePose(projectedPoints, out rotationMatrix, out translationVector);
            }

            estimatedTransformationMatrixControl.SetMatrix(
                Matrix4x4.CreateTranslation(translationVector)*
                Matrix4x4.CreateFromRotation(rotationMatrix));

            float estimatedYaw;
            float estimatedPitch;
            float estimatedRoll;

            rotationMatrix.ExtractYawPitchRoll(out estimatedYaw, out estimatedPitch, out estimatedRoll);

            estimatedYaw*=(float)(180.0/Math.PI);
            estimatedPitch*=(float)(180.0/Math.PI);
            estimatedRoll*=(float)(180.0/Math.PI);

            estimatedYawBox.Text=estimatedYaw.ToString("F3");
            estimatedPitchBox.Text=estimatedPitch.ToString("F3");
            estimatedRollBox.Text=estimatedRoll.ToString("F3");

            estimatedXObjectBox.Text=translationVector.X.ToString("F3");
            estimatedYObjectBox.Text=translationVector.Y.ToString("F3");
            estimatedZObjectBox.Text=translationVector.Z.ToString("F3");
        }
    }
}

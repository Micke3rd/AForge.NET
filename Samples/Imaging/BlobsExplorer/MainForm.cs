// Blobs Browser sample application
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2006-2011
// contacts@aforgenet.com
//

using AForge.Imaging;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlobsExplorer
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			highlightTypeCombo.SelectedIndex = 0;
			showRectangleAroundSelectionCheck.Checked = blobsBrowser.ShowRectangleAroundSelection;
		}

		// On loading of the form
		private void MainForm_Load(object sender, EventArgs e)
		{
			LoadDemo();
		}

		// Exit from application
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		// Open file
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					ProcessImage((Bitmap)Bitmap.FromFile(openFileDialog.FileName));
				}
				catch
				{
					MessageBox.Show("Failed loading selected image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		// Process image
		private void ProcessImage(Bitmap image)
		{
			var foundBlobsCount = blobsBrowser.SetImage(image);

			blobsCountLabel.Text = string.Format("Found blobs' count: {0}", foundBlobsCount);
			propertyGrid.SelectedObject = null;
		}

		// Blob was selected - display its information
		private void blobsBrowser_BlobSelected(object sender, Blob blob)
		{
			propertyGrid.SelectedObject = blob;
			propertyGrid.ExpandAllGridItems();
		}

		// Load demo image
		private void loaddemoImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LoadDemo();
		}

		private void LoadDemo()
		{
			// load arrow bitmap
			var assembly = this.GetType().Assembly;
			var image = new Bitmap(assembly.GetManifestResourceStream("BlobsExplorer.demo.png"));
			ProcessImage(image);
		}

		// Show about form
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var form = new AboutForm();

			form.ShowDialog();
		}

		// Change type of blobs' highlighting
		private void highlightTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			blobsBrowser.Highlighting = (BlobsBrowser.HightlightType)highlightTypeCombo.SelectedIndex;
		}

		// Toggle displaying of rectangle around selection
		private void showRectangleAroundSelectionCheck_CheckedChanged(object sender, EventArgs e)
		{
			blobsBrowser.ShowRectangleAroundSelection = showRectangleAroundSelectionCheck.Checked;
		}
	}
}

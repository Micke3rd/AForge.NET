using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Snapshot_Maker
{
    public partial class SnapshotForm : Form
    {
        public SnapshotForm()
        {
            InitializeComponent();
        }

        public void SetImage(Bitmap bitmap)
        {
            timeBox.Text=DateTime.Now.ToLongTimeString();

            lock (this)
            {
                var old = (Bitmap)pictureBox.Image;
                pictureBox.Image=bitmap;

                if (old!=null)
                {
                    old.Dispose();
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                var ext = Path.GetExtension(saveFileDialog.FileName).ToLower();
                var format = ImageFormat.Jpeg;

                if (ext==".bmp")
                {
                    format=ImageFormat.Bmp;
                }
                else if (ext==".png")
                {
                    format=ImageFormat.Png;
                }

                try
                {
                    lock (this)
                    {
                        var image = (Bitmap)pictureBox.Image;

                        image.Save(saveFileDialog.FileName, format);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed saving the snapshot.\n"+ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

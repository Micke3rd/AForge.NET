// Textures demo
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2006-2011
// contacts@aforgenet.com
//

using AForge.Imaging.Textures;
using System;
using System.Windows.Forms;

namespace TexturesDemo
{
    public partial class MainForm : Form
    {
        ITextureGenerator textureGenerator = null;

        public MainForm()
        {
            InitializeComponent();

            // show first texture
            texturesCombo.SelectedIndex=0;
        }

        // Texture changed
        private void texturesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // create texture generator
            switch (texturesCombo.SelectedIndex)
            {
                case 0:     // clouds
                    textureGenerator=new CloudsTexture();
                    break;
                case 1:     // marble
                    textureGenerator=new MarbleTexture();
                    break;
                case 2:     // wood
                    textureGenerator=new WoodTexture(7);
                    break;
                case 3:     // labyrinth
                    textureGenerator=new LabyrinthTexture();
                    break;
                case 4:     // textile
                    textureGenerator=new TextileTexture();
                    break;
                default:
                    textureGenerator=null;
                    break;
            }

            // show texture
            ShowTexture();
        }

        // Generate and show texture
        private void ShowTexture()
        {
            // check generator
            if (textureGenerator==null)
            {
                pictureBox.Image=null;
                return;
            }

            var width = pictureBox.ClientRectangle.Width;
            var height = pictureBox.ClientRectangle.Height;

            // generate texture
            var texture = textureGenerator.Generate(width, height);

            // create bitmap from the texture
            var image = TextureTools.ToBitmap(texture);

            // show image
            pictureBox.Image=image;
        }

        // Regenerate texture
        private void regenerateButton_Click(object sender, EventArgs e)
        {
            if (textureGenerator!=null)
            {
                textureGenerator.Reset();
                ShowTexture();
            }
        }
    }
}
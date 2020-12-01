// AForge debugging visualizers
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2011
// contacts@aforgenet.com
//

using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Drawing;

[assembly: System.Diagnostics.DebuggerVisualizer(
	typeof(AForge.DebuggerVisualizers.ImageVisualizer),
	typeof(VisualizerObjectSource),
	Target = typeof(System.Drawing.Image),
	Description = "Image Visualizer")]

namespace AForge.DebuggerVisualizers
{
	public class ImageVisualizer: DialogDebuggerVisualizer
	{
		override protected void Show(IDialogVisualizerService windowService,IVisualizerObjectProvider objectProvider)
		{
			var image = (Image)objectProvider.GetObject();

			var imageViewer = new ImageView();
			imageViewer.SetImage(image);

			windowService.ShowDialog(imageViewer);
		}
	}
}

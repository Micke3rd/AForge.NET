﻿// AForge debugging visualizers
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2011
// contacts@aforgenet.com
//

using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[assembly: System.Diagnostics.DebuggerVisualizer(
	typeof(AForge.DebuggerVisualizers.BitmapDataVisualizer),
	typeof(AForge.DebuggerVisualizers.BitmapDataObjectSource),
	Target = typeof(System.Drawing.Imaging.BitmapData),
	Description = "Bitmap Data Visualizer")]

namespace AForge.DebuggerVisualizers
{
	class BitmapDataVisualizer: DialogDebuggerVisualizer
	{
		override protected void Show(IDialogVisualizerService windowService,IVisualizerObjectProvider objectProvider)
		{
			var image = (Image)objectProvider.GetObject();

			var imageViewer = new ImageView();
			imageViewer.SetImage(image);

			windowService.ShowDialog(imageViewer);
		}
	}

	public class BitmapDataObjectSource: VisualizerObjectSource
	{
		public override void GetData(object target,Stream outgoingData)
		{
			var bf = new BinaryFormatter();
			bf.Serialize(outgoingData,
				(new AForge.Imaging.UnmanagedImage((BitmapData)target)).ToManagedImage());
		}
	}
}

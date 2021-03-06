﻿// AForge debugging visualizers
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2011
// contacts@aforgenet.com
//

using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[assembly: System.Diagnostics.DebuggerVisualizer(
	typeof(AForge.DebuggerVisualizers.UnmanagedImageVisualizer),
	typeof(AForge.DebuggerVisualizers.UnmanagedImageObjectSource),
	Target = typeof(AForge.Imaging.UnmanagedImage),
	Description = "Unmanaged Image Visualizer")]

namespace AForge.DebuggerVisualizers
{
	public class UnmanagedImageVisualizer: DialogDebuggerVisualizer
	{
		override protected void Show(IDialogVisualizerService windowService,IVisualizerObjectProvider objectProvider)
		{
			var image = (Image)objectProvider.GetObject();

			var imageViewer = new ImageView();
			imageViewer.SetImage(image);

			windowService.ShowDialog(imageViewer);
		}
	}

	public class UnmanagedImageObjectSource: VisualizerObjectSource
	{
		public override void GetData(object target,Stream outgoingData)
		{
			var bf = new BinaryFormatter();
			bf.Serialize(outgoingData,((AForge.Imaging.UnmanagedImage)target).ToManagedImage());
		}
	}
}

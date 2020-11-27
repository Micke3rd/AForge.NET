﻿// AForge debugging visualizers
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2011
// contacts@aforgenet.com
//

using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(
	typeof(AForge.DebuggerVisualizers.HistogramVisualizer),
	typeof(VisualizerObjectSource),
	Target = typeof(AForge.Math.Histogram),
	Description = "Histogram Visualizer")]

namespace AForge.DebuggerVisualizers
{
	public class HistogramVisualizer : DialogDebuggerVisualizer
	{
		override protected void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
		{
			var histogram = (AForge.Math.Histogram)objectProvider.GetObject();

			var histogramView = new HistogramView();
			histogramView.SetHistogram(histogram);

			windowService.ShowDialog(histogramView);
		}
	}
}

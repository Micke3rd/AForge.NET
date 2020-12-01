// AForge debugging visualizers
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2011
// contacts@aforgenet.com
//

using AForge.Math;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AForge.DebuggerVisualizers
{
	public partial class HistogramView: Form
	{
		private const int HistogramHeight = 100;
		private Histogram histogram;

		public HistogramView()
		{
			InitializeComponent();
		}

		public void SetHistogram(Histogram histogram)
		{
			this.histogram = histogram;

			histogramControl.Values = histogram.Values;
			var length = histogram.Values.Length;

			Text = string.Format("Histogram - {0} values",length);
			statsBox.Text = string.Format("Min: {0}   Max: {1}   Mean: {2:F2}   Std.Dev.: {3:F2}",
				histogram.Min,histogram.Max,histogram.Mean,histogram.StdDev);

			// set form size
			var formWidth = System.Math.Min(800,length) + 40;
			var formHeight = HistogramHeight + 120;

			this.Size = new Size(formWidth,formHeight);

			// set histogram control size
			var width = length + 2;
			var height = HistogramHeight + 2;

			var x = (width > mainPanel.ClientSize.Width) ? 0 : (mainPanel.ClientSize.Width - width) / 2;
			var y = (height > mainPanel.ClientSize.Height) ? 0 : (mainPanel.ClientSize.Height - height) / 2;

			histogramControl.SuspendLayout();
			histogramControl.Size = new Size(width,height);
			histogramControl.Location = new System.Drawing.Point(x,y);
			histogramControl.ResumeLayout();
		}

		// Mouse cursor's position has changed within histogram control
		private void histogramControl_PositionChanged(object sender,AForge.Controls.HistogramEventArgs e)
		{
			if (histogram != null)
			{
				var pos = e.Position;

				if (pos != -1)
				{
					textBox.Text = string.Format("Value: {0}   Count: {1}   Percent: {2:F2}",
						pos,histogram.Values[pos],((float)histogram.Values[pos] * 100 / histogram.TotalCount));
				}
				else
				{
					textBox.Text = string.Empty;
				}
			}
		}

		// Selection has changed within histogram control
		private void histogramControl_SelectionChanged(object sender,AForge.Controls.HistogramEventArgs e)
		{
			if (histogram != null)
			{
				var min = e.Min;
				var max = e.Max;
				var count = 0;

				// count pixels
				for (var i = min; i <= max; i++)
				{
					count += histogram.Values[i];
				}

				textBox.Text = string.Format("Values: {0}...{1}   Count: {2}   Percent: {3:F2}",
					min,max,count,((float)count * 100 / histogram.TotalCount));
			}
		}

		private void logCheck_CheckedChanged(object sender,EventArgs e)
		{
			histogramControl.IsLogarithmicView = logCheck.Checked;
		}
	}
}

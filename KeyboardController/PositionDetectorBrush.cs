using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using System.Drawing;
using System.Diagnostics;
using CUE.NET.Devices.Generic.Enums;

namespace KeyboardController
{
	class PositionDetectorBrush : SolidColorBrush
	{
		public CorsairLedId Led { get; set; }

		public PositionDetectorBrush(CorsairLedId led, CorsairColor color) : base(color)
		{
			this.Led = led;
		}

		protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
		{
			if (renderTarget.LedId == Led)
			{
				Debug.WriteLine(Led);
				Debug.WriteLine(new PointF(renderTarget.Point.X / rectangle.Width, renderTarget.Point.Y / rectangle.Height));
			}
			return base.GetColorAtPoint(rectangle, renderTarget);
		}
	}
}

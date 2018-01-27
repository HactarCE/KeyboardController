using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Effects;
using CUE.NET.Gradients;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardController.Effects
{
	class MoveLinearGradientEffect : AbstractBrushEffect<IGradientBrush>
	{

		#region Properties & Fields
		
		public bool Direction { get; set; }
		public float Speed { get; set; }

		#endregion

		#region Constructors

		// speed = keys/s
		// direction = true for LTR or outward; false for RTL or inward
		public MoveLinearGradientEffect(float speed = 1f, bool direction = true)
		{
			this.Speed = speed;
			this.Direction = direction;
		}

		#endregion

		#region Methods

		public override void OnAttach(IBrush target)
		{
			base.OnAttach(target);
			if (!Direction)
			{
				if (target is LinearGradientBrush && ((IGradientBrush)target).Gradient is LinearGradient)
				{
					foreach (GradientStop gradientStop in ((LinearGradient)((IGradientBrush)target).Gradient).GradientStops)
					{
						gradientStop.Offset = 1 - gradientStop.Offset;
					}
				}
			}
		}

		public override void Update(float deltaTime)
		{
			float movement = Speed * deltaTime * (Direction ? 1 : -1);
			if (Brush.Gradient is LinearGradient)
			{
				LinearGradient linearGradient = (LinearGradient)Brush.Gradient;
				float max = -2f;
				float min = 2f;
				foreach (GradientStop gradientStop in linearGradient.GradientStops)
				{
					gradientStop.Offset += movement;
					if (gradientStop.Offset > max) max = gradientStop.Offset;
					if (gradientStop.Offset < min) min = gradientStop.Offset;
				}
				if (min > 1f || max < 0f)
				{
					IsDone = true;
				}
			}
		}

		#endregion

	}
}

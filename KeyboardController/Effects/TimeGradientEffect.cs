using CUE.NET.Brushes;
using CUE.NET.Effects;
using CUE.NET.Gradients;

namespace KeyboardController.Effects
{
	class TimeGradientEffect : AbstractBrushEffect<SolidColorBrush>
	{

		public LinearGradient Gradient;
		public float Duration;
		public float Speed;
		public int Repetitions;

		private float CurrentOffset = 0;

		public TimeGradientEffect(LinearGradient gradient, float duration, float speed = 1, int repetitions = 0)
		{
			this.Gradient = gradient;
			this.Duration = duration;
			this.Speed = speed;
			this.Repetitions = repetitions;
		}

		public override void Update(float deltaTime)
		{
			CurrentOffset += deltaTime;
			if (CurrentOffset > Duration) {
				CurrentOffset -= Duration;
				IsDone = Repetitions != 0 && --Repetitions == 0;
			}
			Brush.Color = Gradient.GetColor(CurrentOffset * Speed);
		}

	}
}

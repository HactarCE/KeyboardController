using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Effects;
using CUE.NET.Groups;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardController.Default
{
	class TypeFlashKeyManager : KeyManager
	{

		public IEnumerable<CorsairLed> AppliedKeys = new List<CorsairLed>();
		
		public CorsairColor FlashColor { get; set; } = Color.White;

		public float Attack { get; set; } = 0;
		public float Decay { get; set; } = 0;
		public float Sustain { get; set; } = 0.5f;
		public float Release { get; set; } = 0.5f;
		public float AttackValue { get; set; } = 1f;
		public float SustainValue { get; set; } = 1f;

		public override bool OnKeypress(CorsairLedId ledId, bool pressed)
		{
			if (AppliedKeys.Contains(CueSDK.KeyboardSDK[ledId]))
			{
				SolidColorBrush brush = new SolidColorBrush(FlashColor);
				if (!pressed)
				{
					brush.AddEffect(new FlashEffect()
					{
						Attack = this.Attack,
						Decay = this.Decay,
						Sustain = this.Sustain,
						Release = this.Release,
						AttackValue = this.AttackValue,
						SustainValue = this.SustainValue,
						Repetitions = 1
					});
				}
				Profile.GetSingleLedGroup(ledId).Brush = brush;
			}
			return false;
		}

	}
}

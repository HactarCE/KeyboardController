using System.Linq;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Brushes;
using System.Drawing;
using CUE.NET.Groups;
using CUE.NET.Effects;

namespace KeyboardController.KeyManagers
{
	class ModifierKeyManager : KeyManager
	{

		public CorsairLedId[] Leds = {
				CorsairLedId.LeftShift, CorsairLedId.LeftCtrl, CorsairLedId.LeftGui, CorsairLedId.LeftAlt,
				CorsairLedId.RightShift, CorsairLedId.RightCtrl, CorsairLedId.RightGui, CorsairLedId.RightAlt,
		};

		public override bool OnKeypress(CorsairLedId ledId, bool pressed)
		{
			if (Leds.Contains(ledId))
			{
				ListLedGroup ledGroup = Profile.GetSingleLedGroup(ledId);
				if (pressed)
				{
					ledGroup.Brush = new SolidColorBrush(Color.White);
					ledGroup.Brush.AddEffect(new FlashEffect()
					{
						Attack = 0.5f,
						Sustain = 0,
						Release = 0.5f,
						Interval = 0f,
					});
				}
				else
				{
					ledGroup.Brush = null;
				}
				return true;
			}
			return false;
		}

	}
}

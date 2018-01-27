using CUE.NET.Groups;
using CUE.NET.Devices.Generic.Enums;
using System.Diagnostics;
using CUE.NET.Brushes;
using KeyboardController.KeyManagers;

namespace KeyboardController.Profiles
{
	class Tis100 : Profile
	{

		ListLedGroup AllKeys;

		public override void Init()
		{
			base.Init();
			AllKeys = AddGroup(Keyboard.Leds);
			KeyManagers.Add(new MediaKeyManager());
			ListLedGroup FlashyKeysGroup = AddFlashyKeysGroup();
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = FlashyKeysGroup.GetLeds(),
				FlashColor = FromArgb(0xFFFFFFFF),
			});
			KeyManagers.Add(new ModifierKeyManager());
		}

		public override void Start()
		{
			base.Start();
			AllKeys.Brush = new SolidColorBrush(FromArgb(0x3FFFFFFF));
		}

		public override bool MatchesProcess(Process process)
		{
			return process.ProcessName == "tis100";
		}

		protected override bool OnKeyPress(CorsairLedId ledId, bool pressed)
		{
			return false;
		}

	}
}

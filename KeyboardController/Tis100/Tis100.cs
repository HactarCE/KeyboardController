using CUE.NET.Groups;
using CUE.NET.Devices.Generic.Enums;
using System.Diagnostics;
using CUE.NET.Brushes;
using KeyboardController.Default;

namespace KeyboardController.Tis100
{
	class Tis100 : Profile
	{

		ListLedGroup AllKeys;

		public override void Init()
		{
			base.Init();
			AllKeys = AddGroup(Keyboard.Leds);
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = Keyboard.Leds,
				FlashColor = FromArgb(0xFFFFFFFF),
			});
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

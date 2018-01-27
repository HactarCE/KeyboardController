using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Groups;
using CUE.NET.Brushes;
using System.Drawing;
using KeyboardController.Profiles;

namespace KeyboardController.KeyManagers
{
	class SpecialKeyManager : KeyManager
	{

		private static ListLedGroup SpecialKeysGroup;

		public override void OnInit(Profile profile)
		{
			base.OnInit(profile);
			if (SpecialKeysGroup == null) SpecialKeysGroup = profile.AddGroup(CorsairLedId.Brightness, CorsairLedId.WinLock);
		}

		public override void OnStart()
		{
			base.OnStart();
			SpecialKeysGroup.Brush = new SolidColorBrush(Color.White);
		}

		public override bool OnKeypress(CorsairLedId ledId, bool pressed)
		{
			return false;
		}

	}
}

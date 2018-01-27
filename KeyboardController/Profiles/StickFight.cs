using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Gradients;
using CUE.NET.Groups;
using KeyboardController.Effects;
using KeyboardController.KeyManagers;
using System.Diagnostics;

namespace KeyboardController.Profiles
{
	class StickFight : Profile
	{
		
		ListLedGroup MovementGroup;
		ListLedGroup ThrowGroup;

		public override void Init()
		{
			base.Init();
			MovementGroup = AddGroup(CorsairLedId.W, CorsairLedId.A, CorsairLedId.S, CorsairLedId.D);
			ThrowGroup = AddGroup(CorsairLedId.F);
			KeyManagers.Add(new MediaKeyManager());
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = MovementGroup.GetLeds(),
				FlashColor = FromArgb(0xFF00FFFF),
			});
			ListLedGroup FlashyKeysGroup = AddFlashyKeysGroup();
			FlashyKeysGroup.RemoveLeds(MovementGroup.GetLeds());
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = FlashyKeysGroup.GetLeds()
			});
			KeyManagers.Add(new ModifierKeyManager());
		}

		public override void Start()
		{
			base.Start();
			MovementGroup.Brush = new SolidColorBrush(FromArgb(0x00000000));
			MovementGroup.Brush.AddEffect(new TimeGradientEffect(new LinearGradient(new GradientStop[] {
				GradStop(0xFF009999, 0f),
				GradStop(0xFF003333, 0.5f),
				GradStop(0xFF009999, 1f),
			}), 5f, 0.2f));
			ThrowGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF7700));
		}

		public override bool MatchesProcess(Process process)
		{
			return process.ProcessName == "StickFight";
		}

		protected override bool OnKeyPress(CorsairLedId ledId, bool pressed)
		{
			return false;
		}

	}
}

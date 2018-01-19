using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Brushes;
using CUE.NET.Groups;
using CUE.NET.Effects;
using CUE.NET.Gradients;
using System.Drawing;

namespace KeyboardController.Default
{
	class MediaKeyManager : KeyManager
	{

		LockKeyManager MuteKeyManager;
		ListLedGroup MediaGroup;
		ListLedGroup MediaGroup2;
		ListLedGroup MediaGroup3;

		public override void OnInit(Profile profile)
		{
			base.OnInit(profile);
			MuteKeyManager = new LockKeyManager()
			{
				BrushDisabled = new SolidColorBrush(Profile.FromArgb(0x00000000)),
				BrushEnabled = new SolidColorBrush(Profile.FromArgb(0xFFBB0000)),
				ManageCapsLock = false,
				ManageMuteKey = true,
			};
			MuteKeyManager.OnInit(profile);
			MediaGroup = profile.AddGroup(CorsairLedId.Stop, CorsairLedId.ScanPreviousTrack,
				CorsairLedId.PlayPause, CorsairLedId.ScanNextTrack);
			MediaGroup2 = profile.AddGroup(MediaGroup.GetLeds());
			MediaGroup3 = profile.AddGroup(MediaGroup.GetLeds());
		}

		public override void OnStart()
		{
			base.OnStart();
			MuteKeyManager.OnStart();
		}

		public override void OnStop()
		{
			base.OnStop();
			MuteKeyManager.OnStop();
		}

		public override bool OnKeypress(CorsairLedId ledId, bool pressed)
		{
			if (pressed)
			{
				IBrush Brush;
				switch (ledId)
				{
					case CorsairLedId.ScanNextTrack:
					case CorsairLedId.ScanPreviousTrack:
						Brush = new LinearGradientBrush(new LinearGradient(new GradientStop[] {
								Profile.GradStop(0x00FFFFFF, 0f),
								Profile.GradStop(0xFFFFFFFF, -0.5f),
								Profile.GradStop(0x00FFFFFF, -1.0f),
							}));
						Brush.AddEffect(new MoveLinearGradientEffect(2, ledId == CorsairLedId.ScanNextTrack));
						MediaGroup2.Brush = Brush;
						return true;
					case CorsairLedId.PlayPause:
					case CorsairLedId.Stop:
						Brush = new SolidColorBrush(ledId == CorsairLedId.PlayPause ? Color.Green : Color.Red);
						FlashEffect Effect = new FlashEffect()
						{
							Attack = 0,
							Sustain = 0.5f,
							Release = 0.5f,
							Repetitions = 1
						};
						Brush.AddEffect(Effect);
						(ledId == CorsairLedId.PlayPause ? MediaGroup : MediaGroup3).Brush = Brush;
						return true;
				}
			}
			return false;
		}

	}
}

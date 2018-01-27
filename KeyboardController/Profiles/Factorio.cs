using System.Diagnostics;
using CUE.NET.Devices.Generic.Enums;
using KeyboardController.KeyManagers;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Groups;
using CUE.NET.Effects;
using CUE.NET.Gradients;
using KeyboardController.Effects;

namespace KeyboardController.Profiles
{
	class Factorio : Profile
	{

		ListLedGroup FunctionGroup;
		ListLedGroup HotbarGroup;
		ListLedGroup BrushSizeGroup;
		ListLedGroup MovementGroup;
		ListLedGroup ItemsGroup;
		ListLedGroup WeaponsGroup;
		ListLedGroup VehiclesGroup;
		ListLedGroup TechMapProdGroup;
		ListLedGroup RotateGroup;
		ListLedGroup ShootingEffectGroup;

		public Factorio()
		{
			KeyManagers.Add(new LockKeyManager()
			{
				BrushDisabled = new SolidColorBrush(Color.Transparent),
				BrushEnabled = new SolidColorBrush(Color.White),
			});
		}

		public override void Init()
		{
			base.Init();
			FunctionGroup = AddGroup(CorsairLedId.Escape, CorsairLedId.GraveAccentAndTilde, CorsairLedId.Tab,
				CorsairLedId.F4, CorsairLedId.F5, CorsairLedId.F6, CorsairLedId.F7, CorsairLedId.F9, CorsairLedId.F12,
				CorsairLedId.LeftShift, CorsairLedId.LeftCtrl, CorsairLedId.LeftAlt);
			HotbarGroup = AddGroup(CorsairLedId.D1, CorsairLedId.D2, CorsairLedId.D3, CorsairLedId.D4, CorsairLedId.D5, CorsairLedId.X);
			BrushSizeGroup = AddGroup(CorsairLedId.KeypadPlus, CorsairLedId.KeypadMinus);
			MovementGroup = AddGroup(CorsairLedId.W, CorsairLedId.A, CorsairLedId.S, CorsairLedId.D);
			ItemsGroup = AddGroup(CorsairLedId.E, CorsairLedId.F, CorsairLedId.Z);
			WeaponsGroup = AddGroup(CorsairLedId.Q, CorsairLedId.C, CorsairLedId.Space);
			VehiclesGroup = AddGroup(CorsairLedId.G, CorsairLedId.V, CorsairLedId.Enter);
			TechMapProdGroup = AddGroup(CorsairLedId.T, CorsairLedId.M, CorsairLedId.P, CorsairLedId.K, CorsairLedId.L);
			RotateGroup = AddGroup(CorsairLedId.R);
			ShootingEffectGroup = AddGroup(Keyboard.GetLeds());
			ShootingEffectGroup.RemoveLed(CorsairLedId.Brightness, CorsairLedId.WinLock, CorsairLedId.Mute,
				CorsairLedId.Stop, CorsairLedId.ScanPreviousTrack, CorsairLedId.PlayPause, CorsairLedId.ScanNextTrack);
			KeyManagers.Add(new MediaKeyManager());
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = MovementGroup.GetLeds(),
				FlashColor = FromArgb(0xFF00FFFF),
			});
			//KeyManagers.Add(new ModifierKeyManager());
			ListLedGroup FlashyKeysGroup = AddFlashyKeysGroup();
			FlashyKeysGroup.RemoveLeds(MovementGroup.GetLeds());
			FlashyKeysGroup.AddLed(CorsairLedId.LeftAlt);
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = FlashyKeysGroup.GetLeds()
			});
			KeyManagers.Add(new ModifierKeyManager()
			{
				Leds = new CorsairLedId[]
				{
					CorsairLedId.LeftShift, CorsairLedId.LeftCtrl, CorsairLedId.LeftGui,
					CorsairLedId.RightShift, CorsairLedId.RightCtrl, CorsairLedId.RightAlt, CorsairLedId.RightGui
				}
			});
		}

		public override void Start()
		{
			base.Start();
			FunctionGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF00FF));
			HotbarGroup.Brush = new SolidColorBrush(FromArgb(0xFF00FF00));
			BrushSizeGroup.Brush = new SolidColorBrush(FromArgb(0xFFFFFF00));
			MovementGroup.Brush = new SolidColorBrush(FromArgb(0x00000000));
			MovementGroup.Brush.AddEffect(new TimeGradientEffect(new LinearGradient(new GradientStop[] {
				GradStop(0xFF009999, 0f),
				GradStop(0xFF003333, 0.5f),
				GradStop(0xFF009999, 1f),
			}), 5f, 0.2f));
			ItemsGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF7700));
			WeaponsGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF0000));
			VehiclesGroup.Brush = new SolidColorBrush(FromArgb(0xFF00FF77));
			TechMapProdGroup.Brush = new SolidColorBrush(FromArgb(0xFF0000FF));
			RotateGroup.Brush = new SolidColorBrush(FromArgb(0xFFFFFF00));
		}

		public override bool MatchesProcess(Process process)
		{
			return process.ProcessName == "factorio";
		}

		protected override bool OnKeyPress(CorsairLedId ledId, bool pressed)
		{
			if (ledId == CorsairLedId.C || ledId == CorsairLedId.Space)
			{
				SolidColorBrush Brush = new SolidColorBrush(FromArgb(0xFFFF0000));
				if (!pressed)
				{
					Brush.AddEffect(new FlashEffect()
					{
						Attack = 0,
						Sustain = 0,
						Release = 1,
						Repetitions = 1,
					});
				}
				else
				{
					//Brush.AddEffect(new FlashEffect()
					//{
					//	Attack = 0.01f,
					//	Sustain = 0.02f,
					//	Release = 0.01f,
					//	Repetitions = 0,
					//	Interval = 0.02f,
					//});
				}
				ShootingEffectGroup.Brush = Brush;
			}
			return false;
		}

	}
}

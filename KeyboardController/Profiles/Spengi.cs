using System.Diagnostics;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Groups;
using KeyboardController.KeyManagers;
using CUE.NET.Brushes;
using CUE.NET.Gradients;
using KeyboardController.Effects;

namespace KeyboardController.Profiles
{
	class Spengi : Profile
	{

		ListLedGroup FunctionGroup;
		ListLedGroup HotbarGroup;
		ListLedGroup RotateGroup;
		ListLedGroup MovementGroup;
		ListLedGroup BuildingGroup;
		ListLedGroup InteractGroup;
		ListLedGroup ViewGroup;
		ListLedGroup ModifierGroup;

		public override void Init()
		{
			base.Init();
			FunctionGroup = AddGroup(CorsairLedId.Escape, CorsairLedId.F1,
				CorsairLedId.F4, CorsairLedId.F6, CorsairLedId.F7, CorsairLedId.F8, CorsairLedId.F9, CorsairLedId.F10, CorsairLedId.Tab);
			HotbarGroup = AddGroup(CorsairLedId.D1, CorsairLedId.D2, CorsairLedId.D3, 
				CorsairLedId.D4, CorsairLedId.D5, CorsairLedId.D6, CorsairLedId.D7, 
				CorsairLedId.D8, CorsairLedId.D9, CorsairLedId.D0);
			RotateGroup = AddGroup(CorsairLedId.Insert, CorsairLedId.Home, CorsairLedId.PageUp,
				CorsairLedId.Delete, CorsairLedId.End, CorsairLedId.PageDown);
			MovementGroup = AddGroup(CorsairLedId.Q, CorsairLedId.W, CorsairLedId.E, 
				CorsairLedId.A, CorsairLedId.S, CorsairLedId.D, CorsairLedId.C);
			BuildingGroup = AddGroup(CorsairLedId.P, CorsairLedId.BracketLeft, 
				CorsairLedId.BracketRight, CorsairLedId.N, CorsairLedId.M);
			InteractGroup = AddGroup(CorsairLedId.Backspace, CorsairLedId.Y, CorsairLedId.CapsLock, CorsairLedId.F,
				CorsairLedId.I, CorsairLedId.G, CorsairLedId.K, CorsairLedId.L, CorsairLedId.Z, CorsairLedId.X);
			ViewGroup = AddGroup(CorsairLedId.V, CorsairLedId.LeftAlt, CorsairLedId.UpArrow,
				CorsairLedId.LeftArrow, CorsairLedId.DownArrow, CorsairLedId.RightArrow);
			ModifierGroup = AddGroup(CorsairLedId.LeftShift, CorsairLedId.LeftCtrl);
			KeyManagers.Add(new MediaKeyManager());
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = MovementGroup.GetLeds(),
				FlashColor = FromArgb(0xFF00FFFF),
			});
			KeyManagers.Add(new ModifierKeyManager());
			ListLedGroup FlashyKeys = AddFlashyKeysGroup();
			FlashyKeys.AddLed(CorsairLedId.CapsLock);
			FlashyKeys.RemoveLeds(MovementGroup.GetLeds());
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = FlashyKeys.GetLeds()
			});
		}

		public override void Start()
		{
			base.Start();
			FunctionGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF00FF));
			HotbarGroup.Brush = new SolidColorBrush(FromArgb(0xFF00FF00));
			RotateGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF00FF));
			MovementGroup.Brush = new SolidColorBrush(FromArgb(0x00000000));
			MovementGroup.Brush.AddEffect(new TimeGradientEffect(new LinearGradient(new GradientStop[] {
				GradStop(0xFF00FFFF, 0f),
				GradStop(0xFF003333, 0.5f),
				GradStop(0xFF00FFFF, 1f),
			}), 5f, 0.2f));
			BuildingGroup.Brush = new SolidColorBrush(FromArgb(0xFF00FF77));
			InteractGroup.Brush = new SolidColorBrush(FromArgb(0xFF0000FF));
			ViewGroup.Brush = new SolidColorBrush(FromArgb(0xFFFFFF00));
			ModifierGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF00FF));
		}

		public override bool MatchesProcess(Process process)
		{
			return process.ProcessName == "SpaceEngineers";
		}

		protected override bool OnKeyPress(CorsairLedId ledId, bool pressed)
		{
			return false;
		}

	}
}

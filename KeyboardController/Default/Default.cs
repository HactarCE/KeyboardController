using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Brushes;
using CUE.NET.Groups;
using System.Drawing;
using CUE.NET.Gradients;
using CUE.NET.Effects;
using CUE.NET.Devices.Generic;

namespace KeyboardController.Default
{
	class Default : Profile
	{

		public Default()
		{
		}

		ListLedGroup MediaGroup;
		ListLedGroup NumpadGroup;
		ListLedGroup NumpadGroup2;
		ListLedGroup ModifierGroup;
		ListLedGroup PseudoModifierGroup;
		ListLedGroup FunctionGroup;
		ListLedGroup ArrowGroup;
		ListLedGroup NavigationGroup;
		ListLedGroup SpecialGroup;
		ListLedGroup TypingGroup;
		//ListLedGroup AllButNumpad;

		public override void Init()
		{
			base.Init();
			#region MediaGroup
			MediaGroup = AddGroup(CorsairLedId.Mute, CorsairLedId.Stop,
				CorsairLedId.ScanPreviousTrack, CorsairLedId.PlayPause, CorsairLedId.ScanNextTrack);
			#endregion
			#region NumpadGroup
			NumpadGroup = AddGroup(
				CorsairLedId.NumLock, CorsairLedId.KeypadSlash, CorsairLedId.KeypadAsterisk, CorsairLedId.KeypadMinus,
				CorsairLedId.Keypad7, CorsairLedId.Keypad8, CorsairLedId.Keypad9, CorsairLedId.KeypadPlus,
				CorsairLedId.Keypad4, CorsairLedId.Keypad5, CorsairLedId.Keypad6,
				CorsairLedId.Keypad1, CorsairLedId.Keypad2, CorsairLedId.Keypad3, CorsairLedId.KeypadEnter,
				CorsairLedId.Keypad0, CorsairLedId.KeypadPeriodAndDelete);
			NumpadGroup2 = AddGroup(NumpadGroup.GetLeds());
			#endregion
			#region ModifierGroup
			ModifierGroup = AddModifiersGroup();
			#endregion
			#region PseudoModifierGroup
			PseudoModifierGroup = AddGroup(CorsairLedId.Application);
			#endregion
			#region FunctionGroup
			FunctionGroup = AddGroup(CorsairLedId.Escape,
				CorsairLedId.F1, CorsairLedId.F2, CorsairLedId.F3,
				CorsairLedId.F4, CorsairLedId.F5, CorsairLedId.F6,
				CorsairLedId.F7, CorsairLedId.F8, CorsairLedId.F9,
				CorsairLedId.F10, CorsairLedId.F11, CorsairLedId.F12);
			#endregion
			#region ArrowGroup
			ArrowGroup = AddGroup(CorsairLedId.UpArrow,
				CorsairLedId.LeftArrow, CorsairLedId.DownArrow, CorsairLedId.RightArrow);
			#endregion
			#region NavigationGroup
			NavigationGroup = AddGroup(
				CorsairLedId.PrintScreen, CorsairLedId.ScrollLock, CorsairLedId.PauseBreak,
				CorsairLedId.Insert, CorsairLedId.Home, CorsairLedId.PageUp,
				CorsairLedId.Delete, CorsairLedId.End, CorsairLedId.PageDown);
			#endregion
			#region SpecialGroup
			SpecialGroup = AddGroup(CorsairLedId.Brightness, CorsairLedId.WinLock);
			#endregion
			#region TypingGroup
			TypingGroup = AddGroup(Keyboard.GetLeds());
			TypingGroup.RemoveLeds(MediaGroup.GetLeds());
			TypingGroup.RemoveLeds(NumpadGroup.GetLeds());
			TypingGroup.RemoveLeds(ModifierGroup.GetLeds());
			TypingGroup.RemoveLeds(PseudoModifierGroup.GetLeds());
			TypingGroup.RemoveLeds(FunctionGroup.GetLeds());
			TypingGroup.RemoveLeds(ArrowGroup.GetLeds());
			TypingGroup.RemoveLeds(NavigationGroup.GetLeds());
			TypingGroup.RemoveLeds(SpecialGroup.GetLeds());
			TypingGroup.RemoveLed(CorsairLedId.CapsLock);
			#endregion
			KeyManagers.Add(new LockKeyManager()
			{
				BrushDisabled = new SolidColorBrush(FromArgb(0xFF0077FF)),
				BrushEnabled = new SolidColorBrush(FromArgb(0xFFFFFFFF)),
			});
			KeyManagers.Add(new MediaKeyManager());
			KeyManagers.Add(new TypeFlashKeyManager()
			{
				AppliedKeys = AddFlashyKeysGroup().GetLeds()
			});
			KeyManagers.Add(new ModifierKeyManager());
			//AllButNumpad = AddGroup(Keyboard.GetLeds());
			//AllButNumpad.RemoveLeds(NumpadGroup.GetLeds());
		}

		public override void Start()
		{
			base.Start();
			MediaGroup.Brush = new SolidColorBrush(FromArgb(0xFF0000BB));
			ModifierGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF00FF));
			PseudoModifierGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF00FF));
			FunctionGroup.Brush = new SolidColorBrush(FromArgb(0xFF0000FF));
			ArrowGroup.Brush = new SolidColorBrush(FromArgb(0xFF0000FF));
			NavigationGroup.Brush = new SolidColorBrush(FromArgb(0xFFFF00FF));
			SpecialGroup.Brush = new SolidColorBrush(FromArgb(0xFFFFFFFF));
			TypingGroup.Brush = new SolidColorBrush(FromArgb(0xFF0077FF));
			//AllButNumpad.Brush = new SolidColorBrush(FromArgb(0xFF000000));
		}

		public override bool MatchesProcess(Process process)
		{
			return process.ProcessName == "explorer";
		}

		protected override bool OnKeyPress(CorsairLedId ledId, bool pressed) { return false; }

		private bool SpeedCrunchActive = false;

		public override void NotifyActiveWindowChanged(Process process)
		{
			CorsairColor CalculatorColor = FromArgb(0xFF77FFFF);
			CorsairColor NormalColor = FromArgb(0xFFFF7700);
			if ((process.ProcessName == "SpeedCrunch" && !SpeedCrunchActive)
				|| (process.ProcessName != "SpeedCrunch" && SpeedCrunchActive))
			{
				SpeedCrunchActive = !SpeedCrunchActive;
				NumpadGroup.Brush = new SolidColorBrush(SpeedCrunchActive ? NormalColor : CalculatorColor);
				PointF NumlockWithinNumpad = new PointF(0.08695652f, 0.07142857f);
				NumpadGroup2.Brush = new RadialGradientBrush(NumlockWithinNumpad, new LinearGradient(new GradientStop[] {
							GradStop(0x00FFFFFF, 0f),
							GradStop(SpeedCrunchActive ? CalculatorColor : NormalColor, -0.1f),
						}));
				NumpadGroup2.Brush.AddEffect(new MoveLinearGradientEffect(4));
				//AllButNumpad.Brush = new RadialGradientBrush(new LinearGradient(new GradientStop[] {
				//	GradStop(SpeedCrunchActive ? (uint)0x00000000 : 0x77000000, -0.2f),
				//	GradStop(SpeedCrunchActive ? (uint)0x77000000 : 0x00000000, 0),
				//}));
				//AllButNumpad.Brush.AddEffect(new MoveLinearGradientEffect(3, false));
			}
			else
			{
				NumpadGroup.Brush = new SolidColorBrush(SpeedCrunchActive ? CalculatorColor : NormalColor);
				//AllButNumpad.Brush = new SolidColorBrush(FromArgb(SpeedCrunchActive ? (uint)0x77000000 : 0x00000000));
			}
		}

	}
}

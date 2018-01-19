using System;
using CUE.NET;
using CUE.NET.Devices.Keyboard;
using System.Diagnostics;
using CUE.NET.Exceptions;
using CUE.NET.Devices.Generic.Enums;
using System.Drawing;
using CUE.NET.Groups;
using CUE.NET.Brushes;
using CUE.NET.Input;
using CUE.NET.Gradients;
using CUE.NET.Effects;
using CUE.NET.Devices.Generic;
using KeyboardController.Default;
using CUE.NET.Input.Enums;
using System.Collections.Generic;
using CUE.NET.Input.EventArgs;

namespace KeyboardController
{
	abstract class Profile
	{

		protected CorsairKeyboard Keyboard;
		protected List<KeyManager> KeyManagers = new List<KeyManager>();
		protected List<ILedGroup> LedGroups = new List<ILedGroup>();

		public abstract bool MatchesProcess(Process process);

		private EventHandler<OnInputEventArgs> OnInput;

		private static bool CtrlDown = false;
		private static bool ShiftDown = false;
		private static bool AltDown = false;

		// Define your groups here
		public virtual void Init()
		{
			try
			{
				Keyboard = CueSDK.KeyboardSDK;
				if (Keyboard == null) throw new WrapperException("No keyboard found");
				OnInput = (sender, args) =>
				{
					CorsairLedId ledId = args.LedId;
					bool pressed = args.Action == InputAction.Pressed;
					//Console.WriteLine("Key pressed: {0}", ledId);
					if (ledId == CorsairLedId.RightCtrl) CtrlDown = pressed;
					else if (ledId == CorsairLedId.RightShift) ShiftDown = pressed;
					else if (ledId == CorsairLedId.RightAlt) AltDown = pressed;
					else if (CtrlDown && AltDown && ledId == CorsairLedId.ScrollLock && pressed) Environment.Exit(0);
					else if (CtrlDown && ShiftDown && ledId == CorsairLedId.ScrollLock && pressed)
					{
						fadeFromBlack();
					}
					if (!OnKeyPress(ledId, pressed))
						foreach (KeyManager keyManager in KeyManagers)
							if (keyManager.OnKeypress(ledId, pressed)) return;
				};

			}
			catch (CUEException ex)
			{
				Debug.WriteLine("CUE Exception! ErrorCode: " + Enum.GetName(typeof(CorsairError), ex.Error));
			}
			catch (WrapperException ex)
			{
				Debug.WriteLine("Wrapper Exception! Message:" + ex.Message);
			}
		}

		// Set up brushes/effects here (also do RegisterOnInput)
		public virtual void Start()
		{
			if (Keyboard == null)
			{
				Init();
				foreach (KeyManager keyManager in KeyManagers)
					keyManager.OnInit(this);
			}
			Keyboard.RegisterOnInput(OnInput);
			Keyboard.Brush = new SolidColorBrush(Color.Black);
			foreach (KeyManager keyManager in KeyManagers)
				keyManager.OnStart();
		}

		// Set all brushes to null and do UnregisterOnInput (automagically handled)
		public void Stop()
		{
			foreach (KeyManager keyManager in KeyManagers)
				keyManager.OnStop();
			foreach (ILedGroup ledGroup in LedGroups)
				ledGroup.Brush = null;
			Keyboard.UnregisterOnInput(OnInput);
		}

		protected abstract bool OnKeyPress(CorsairLedId ledId, bool pressed);

		// Called at Start()
		// Also called if this is the default program and the window changes (but no other program is taking over)
		public virtual void NotifyActiveWindowChanged(Process process) { }

		#region LED Groups
		public ListLedGroup AddGroup() { return AddGroupInternal(new ListLedGroup(Keyboard)); }
		public ListLedGroup AddGroup(params CorsairLed[] leds) { return AddGroupInternal(new ListLedGroup(Keyboard, leds)); }
		public ListLedGroup AddGroup(params CorsairLedId[] leds) { return AddGroupInternal(new ListLedGroup(Keyboard, leds)); }
		public ListLedGroup AddGroup(IEnumerable<CorsairLed> leds) { return AddGroupInternal(new ListLedGroup(Keyboard, leds)); }
		public ListLedGroup AddGroup(IEnumerable<CorsairLedId> leds) { return AddGroupInternal(new ListLedGroup(Keyboard, leds)); }
		private ListLedGroup AddGroupInternal(ListLedGroup group)
		{
			LedGroups.Add(group);
			return group;
		}

		public ListLedGroup GetSingleLedGroup(CorsairLed led) { return AddGroupInternal(KeyboardController.GetSingleLedGroup(Keyboard, led)); }
		public ListLedGroup GetSingleLedGroup(CorsairLedId led) { return AddGroupInternal(KeyboardController.GetSingleLedGroup(Keyboard, led)); }

		public ListLedGroup AddFlashyKeysGroup()
		{
			ListLedGroup group = AddGroup(Keyboard.GetLeds());
			group.RemoveLed(CorsairLedId.Brightness, CorsairLedId.WinLock, CorsairLedId.Mute, CorsairLedId.CapsLock,
				CorsairLedId.Stop, CorsairLedId.ScanPreviousTrack, CorsairLedId.PlayPause, CorsairLedId.ScanNextTrack,
				CorsairLedId.LeftShift, CorsairLedId.LeftCtrl, CorsairLedId.LeftGui, CorsairLedId.LeftAlt,
				CorsairLedId.RightShift, CorsairLedId.RightCtrl, CorsairLedId.RightGui, CorsairLedId.RightAlt);
			return group;
		}
		public ListLedGroup AddModifiersGroup()
		{
			return AddGroup(
				CorsairLedId.LeftShift, CorsairLedId.LeftCtrl, CorsairLedId.LeftGui, CorsairLedId.LeftAlt,
				CorsairLedId.RightShift, CorsairLedId.RightCtrl, CorsairLedId.RightGui, CorsairLedId.RightAlt);
		}
		#endregion

		#region Helper methods
		public static GradientStop GradStop(UInt32 argb, float offset)
		{ return GradStop(FromArgb(argb), offset); }

		public static GradientStop GradStop(CorsairColor color, float offset)
		{ return new GradientStop(offset, color); }

		public static Color FromArgb(UInt32 argb)
		{ return Color.FromArgb(unchecked((int)argb)); }

		protected static LinearGradient WhiteFlash()
		{
			return new LinearGradient(new GradientStop[]{
				new GradientStop(-10f, new CorsairColor(000, 255, 255, 255)),
				new GradientStop(-0.1f, new CorsairColor(255, 255, 255, 255)),
				new GradientStop(0f, new CorsairColor(000, 255, 255, 255))
			});
		}
		#endregion

		public void fadeFromBlack()
		{
			SolidColorBrush brush = new SolidColorBrush(Color.Black);
			brush.AddEffect(new FlashEffect()
			{
				Attack = 0,
				Decay = 0.5f,
				Sustain = 0,
				Repetitions = 1
			});
			new ListLedGroup(Keyboard, Keyboard.Leds).Brush = brush;
		}

	}
}

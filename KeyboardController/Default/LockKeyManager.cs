using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;
using System.Runtime.InteropServices;
using CUE.NET.Groups;
using CUE.NET.Brushes;
using System.Drawing;
using System.Diagnostics;
using CUE.NET;
using NAudio.CoreAudioApi;

namespace KeyboardController.Default
{
	class LockKeyManager : KeyManager
	{

		public static bool CapsLockState { get { return (((ushort)GetKeyState(0x14)) & 0xffff) != 0; } }   // shamelessly
		public static bool NumLockState { get { return (((ushort)GetKeyState(0x90)) & 0xffff) != 0; } }    // stolen from
		public static bool ScrollLockState { get { return (((ushort)GetKeyState(0x91)) & 0xffff) != 0; } } // StackOverflow
		public static bool MuteState { get {
				// also stolen from StackOverflow
				return new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioEndpointVolume.Mute;
			} }

		public IBrush BrushEnabled { get; set; } = new SolidColorBrush(Color.Green);
		public IBrush BrushDisabled { get; set; } = new SolidColorBrush(Color.Red);

		public bool ManageCapsLock { get; set; } = true;
		public bool ManageNumLock { get; set; } = false;
		public bool ManageScrollLock { get; set; } = false;
		public bool ManageMuteKey { get; set; } = false;

		private static ListLedGroup CapsLockKey;
		private static ListLedGroup NumLockKey;
		private static ListLedGroup ScrollLockKey;
		private static ListLedGroup MuteKey;

		public override void OnInit(Profile profile)
		{
			base.OnInit(profile);
			CapsLockKey = profile.GetSingleLedGroup(CorsairLedId.CapsLock);
			NumLockKey = profile.GetSingleLedGroup(CorsairLedId.NumLock);
			ScrollLockKey = profile.GetSingleLedGroup(CorsairLedId.ScrollLock);
			MuteKey = profile.GetSingleLedGroup(CorsairLedId.Mute);
		}

		public override void OnStart()
		{
			base.OnStart();
			Update();
		}

		public override bool OnKeypress(CorsairLedId ledId, bool pressed)
		{
			Update();
			switch (ledId)
			{
				case CorsairLedId.CapsLock:
					return ManageCapsLock;
				case CorsairLedId.NumLock:
					return ManageNumLock;
				case CorsairLedId.ScrollLock:
					return ManageScrollLock;
				case CorsairLedId.VolumeDown:
				case CorsairLedId.VolumeUp:
				case CorsairLedId.Mute:
					return ManageMuteKey;
			}
			return false;
		}

		private void Update()
		{
			if (ManageCapsLock) CapsLockKey.Brush = CapsLockState ? BrushEnabled : BrushDisabled;
			if (ManageNumLock) NumLockKey.Brush = NumLockState ? BrushEnabled : BrushDisabled;
			if (ManageScrollLock) ScrollLockKey.Brush = ScrollLockState ? BrushEnabled : BrushDisabled;
			if (ManageMuteKey) MuteKey.Brush = MuteState ? BrushEnabled : BrushDisabled;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
		public static extern short GetKeyState(int keyCode);

		[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint WaveOutGetVolume(IntPtr hwo, uint dwVolume);

	}
}

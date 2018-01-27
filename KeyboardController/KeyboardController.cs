using CUE.NET;
using CUE.NET.Devices;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Groups;
using KeyboardController.Profiles;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace KeyboardController
{
	class KeyboardController
	{

		private static Dictionary<CorsairLedId, ListLedGroup> SingleLedGroups = new Dictionary<CorsairLedId, ListLedGroup>();
		static Profile activeProfile = null;

		private static Profile[] Profiles =
		{
			new Default(),
			new Factorio(),
			new Spengi(),
			new StickFight(),
			new Tis100(),
		};

		public static void Main(string[] args)
		{
			SystemEvents.PowerModeChanged += OnPowerChange;
			CueSDK.Initialize(true);
			Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");
			CueSDK.UpdateMode = UpdateMode.Continuous;
			IntPtr previousActiveWindow = new IntPtr();
			while (true)
			{
				IntPtr activeWindow = GetForegroundWindow();
				if (activeWindow != previousActiveWindow)
				{
					previousActiveWindow = activeWindow;
					uint pid;
					GetWindowThreadProcessId(activeWindow, out pid);
					Process process = Process.GetProcessById((int)pid);
					Profile newActiveProfile = Profiles[0];
					foreach (Profile p in Profiles)
						if (p.MatchesProcess(process))
							newActiveProfile = p;
					if (newActiveProfile != activeProfile)
					{
						activeProfile?.Stop();
						(activeProfile = newActiveProfile).Start();
						activeProfile.NotifyActiveWindowChanged(process);
					}
					else
					{
						activeProfile.NotifyActiveWindowChanged(process);
					}
				}
				Thread.Sleep(250);
			}
		}

		public static ListLedGroup GetSingleLedGroup(ICueDevice device, CorsairLedId ledId)
		{
			if (!SingleLedGroups.ContainsKey(ledId)) SingleLedGroups.Add(ledId, new ListLedGroup(device, ledId));
			return SingleLedGroups[ledId];
		}

		private static void OnPowerChange(object s, PowerModeChangedEventArgs e)
		{
			if (e.Mode == PowerModes.Resume)
			{
				Task.Delay(3000).ContinueWith(_ => activeProfile.fadeFromBlack());
			}
		}

		[DllImport("user32.dll")]
		static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true)]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

	}
}

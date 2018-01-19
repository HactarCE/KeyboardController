﻿using CUE.NET.Devices.Generic.Enums;

namespace KeyboardController.Default
{
	abstract class KeyManager
	{
		protected Profile Profile;
		public virtual void OnInit(Profile profile) { this.Profile = profile; }
		public virtual void OnStart() { }
		public virtual void OnStop() { }
		public abstract bool OnKeypress(CorsairLedId ledId, bool pressed);
	}
}

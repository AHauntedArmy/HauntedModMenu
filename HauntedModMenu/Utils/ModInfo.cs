using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using BepInEx;

namespace HauntedModMenu.Utils
{
	public class ModInfo
	{
		public bool Enabled {
			get => Enabled;
			set {
				Enabled = value;
				ModMenuEnable?.Invoke(Mod, new object[] { value });
			} 
		}

		public string Name { get; private set; } = null;
		public BaseUnityPlugin Mod { get; private set; } = null;
		public MethodInfo ModMenuEnable { get; private set; } = null;

		public ModInfo(BaseUnityPlugin mod, string modName, MethodInfo enableFunc)
		{
			Mod = mod;
			Name = modName;
			ModMenuEnable = enableFunc;

			Enabled = false;
		}
	}
}

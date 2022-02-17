using BepInEx;

namespace HauntedModMenu.Utils
{
	public class ModInfo
	{
		private BaseUnityPlugin mod = null;

		public bool Enabled {
			get {
				if (mod == null)
					return false;

				return mod.enabled;
			}

			set {
				if (mod != null)
					mod.enabled = value;
			} 
		}

		public string Name { get; private set; } = null;

		public ModInfo(BaseUnityPlugin plugin, string modName)
		{
			mod = plugin;
			Name = modName;
		}
	}
}

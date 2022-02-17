using System.Collections.Generic;

using UnityEngine;

namespace HauntedModMenu.Utils
{
	// static class to cache references
	public static class RefCache
	{
		public static Font CustomFont { get; set; } = null;
		public static List<ModInfo> ModList { get; private set; } = new List<ModInfo>();

		// left hand references
		public static GameObject LeftHandRig { get; set; } = null;
		public static GameObject LeftHandFollower { get; set; } = null;

		// right hand references
		public static GameObject RightHandRig { get; set; } = null;
		public static GameObject RightHandFollower { get; set; } = null;

		public static Transform CameraTransform { get; set; } = null;
	}
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using BepInEx;
using BepInEx.Configuration;

namespace HauntedModMenu.Utils
{
	internal static class Config
	{
		public static ConfigFile File { get; private set; } = new ConfigFile(Path.Combine(Paths.ConfigPath, "HauntedModMenu.cfg"), true);

		public static T LoadData<T>(string tag, string key, string description, T defaultValue)
		{
			if (File == null)
				return defaultValue;

			T data;
			ConfigEntry<T> fileData = File.Bind(tag, key, defaultValue, description);

			if (fileData != null) {
				data = fileData.Value;

			} else {
				data = defaultValue;
			}

			return data;
		}

		public static void SaveData<T>(string tag, string key, T data)
		{
			if (File == null)
				return;

			ConfigEntry<T> fileData = File.Bind(tag, key, data);

			if (fileData != null)
				fileData.Value = data;

			File.Save();
		}
	}
}

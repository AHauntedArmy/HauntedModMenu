using System;
using System.Collections.Generic;
using System.Reflection;

using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;

using UnityEngine;

using Utilla;

namespace HauntedModMenu
{
	/// <summary>
	/// This is your mod's main class.
	/// </summary>

	/* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class HauntedModMenuPlugin : BaseUnityPlugin
	{
		private bool inRoom;
		private GameObject menuObject = null;

		private void Start()
		{
			Type[] funcParem = new Type[] { typeof(bool) };

			foreach(BepInEx.PluginInfo plugin in Chainloader.PluginInfos.Values) {

				BaseUnityPlugin modPlugin = plugin.Instance;
				MethodInfo modImpl = AccessTools.Method(modPlugin.GetType(), "HauntedModMenuEnable", funcParem);

				if (modImpl != null) {
					Utils.RefCache.ModList.Add(new Utils.ModInfo(modPlugin, plugin.Metadata.Name, modImpl));
				}
			}
		}

		private void OnEnable()
		{
			Utilla.Events.GameInitialized += OnGameInitialized;
		}

		private void OnDisable()
		{
			
		}

		private void OnGameInitialized(object sender, EventArgs e)
		{
			Utils.RefCache.LeftHandFollower = GorillaLocomotion.Player.Instance.leftHandFollower.gameObject;
			Utils.RefCache.RightHandFollower = GorillaLocomotion.Player.Instance.rightHandFollower.gameObject;
			Utils.RefCache.CameraTransform = GorillaLocomotion.Player.Instance.headCollider.transform;

			Utils.RefCache.LeftHandRig = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L");
			Utils.RefCache.RightHandRig = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R");

			OnJoin(null);
		}

		private void Update()
		{
			/* Code here runs every frame when the mod is enabled */
		}

		/* This attribute tells Utilla to call this method when a modded room is joined */
		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			inRoom = true;

			menuObject = CreateTrigger();
			if (menuObject != null)
				menuObject.AddComponent<Menu.MenuController>();
		}

		/* This attribute tells Utilla to call this method when a modded room is left */
		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			inRoom = false;
			UnityEngine.Object.Destroy(menuObject);
		}

		private GameObject CreateTrigger()
		{
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			if (go == null)
				return null;

			Collider col = go.GetComponent<Collider>();
			if (col != null)
				col.isTrigger = true;

			return go;
		}
	}
}

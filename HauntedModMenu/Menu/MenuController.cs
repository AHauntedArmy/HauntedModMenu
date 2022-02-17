using System.IO;
using System.Reflection;
using System.Collections.Generic;

using BepInEx.Configuration;

using UnityEngine;

using HauntedModMenu.Utils;

namespace HauntedModMenu.Menu
{
	class MenuController : Buttons.HandTrigger
	{
		private static readonly string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.cfg");

		private ConfigFile config = null;
		private ConfigEntry<bool> leftHandConfig = null;
		private ConfigEntry<float> handSensitivityConfig = null;
		private Dictionary<string, ConfigEntry<bool>> modConfigs = null;

		private Collider menuTrigger = null;
		private GameObject menu = null;

		protected override void Awake()
		{
			base.Awake();

			LoadConfig();
			SetParent();
			CreateMenuView();

			menuTrigger = this.gameObject.GetComponent<Collider>();
			if (menuTrigger != null)
				menuTrigger.enabled = false;
		}

		private void OnEnable()
		{
			if (leftHandTracker != null)
				leftHandTracker.enabled = true;

			if (rightHandTracker != null)
				rightHandTracker.enabled = true;
		}

		private void OnDisable()
		{
			if (leftHandTracker != null)
				leftHandTracker.enabled = false;

			if (rightHandTracker != null)
				rightHandTracker.enabled = false;

			if (menuTrigger != null)
				menuTrigger.enabled = false;

			SaveConfig();
		}

		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(leftHandTracker);
			UnityEngine.Object.Destroy(rightHandTracker);

			config = null;
		}

		private void Update()
		{
			if (RefCache.CameraTransform == null)
				return;

			Vector3 handDir = Vector3.Normalize(this.gameObject.transform.position - RefCache.CameraTransform.position);
			Vector3 lookDir = RefCache.CameraTransform.forward;

			float dotAngle = Vector3.Dot(handDir, lookDir);
			if (dotAngle < 0.8f)
				return;

			
			if (!menuTrigger.enabled) {
				dotAngle = Vector3.Dot(this.gameObject.transform.forward, lookDir);

				if (dotAngle > 0.8f)
					menuTrigger.enabled = true;

			} else if (menuTrigger.enabled) {
				menuTrigger.enabled = false;
			}
		}

		private void LoadConfig()
		{
			leftHand = Config.LoadData("Hand Config", "LeftHand", "which hand the menu is on, true = left hand, false = right hand.", true);
			handSensitivity = Config.LoadData("Hand Config", "Sensitivity", "how sensitive the button is to activate, higher number = more sensitive.", 0.8f);

			// load mod status
			if (RefCache.ModList?.Count > 0) {
				foreach (ModInfo modInfo in RefCache.ModList) {
					modInfo.Enabled = Config.LoadData("Mod Status", modInfo.Name, "", modInfo.Enabled);
				}
			}

			// just in case
			Config.File?.Save();
		}

		private void SaveConfig()
		{
			if (RefCache.ModList?.Count < 1)
				return;

			foreach(ModInfo modInfo in RefCache.ModList) {
				Config.SaveData("Mod Status", modInfo.Name, modInfo.Enabled);
			}
		}

		private void SetParent()
		{
			// Transform parent = leftHand ? RefCache.LeftHandRig?.transform : RefCache.RightHandRig?.transform;
			Transform parent = RefCache.LeftHandRig?.transform;

			if (parent != null) {
				Transform myTransform = this.gameObject.transform;
				myTransform.SetParent(parent);

				myTransform.localPosition = new Vector3(-0.0315f, 0.035f, 0f);
				myTransform.localRotation = Quaternion.Euler(-30f, 120f, 75f);
				myTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			}
		}

		public void SetHand(bool lHand)
		{
			if (leftHandConfig != null) {
				leftHand = lHand;
				leftHandConfig.Value = lHand;
				SetParent();
			}
		}

		protected override void HandTriggered()
		{
			if (menu == null)
				return;

			menu.SetActive(!menu.activeSelf);

			if (!menu.activeSelf)
				SaveConfig();
		}

		private void CreateMenuView()
		{
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			if (go == null)
				return;

			Collider col = go.GetComponent<Collider>();
			if (col != null)
				UnityEngine.Object.Destroy(col);

			go.transform.SetParent(this.gameObject.transform);
			go.transform.localPosition = new Vector3(1.05f, 1.75f, -1.35f);
			go.transform.localRotation = Quaternion.Euler(-25f, 17f, -18.5f);
			go.transform.localScale = new Vector3(1.8f, 3.5f, 0.05f);

			go.AddComponent<MenuView>();

			go.SetActive(false);
			menu = go;
		}
	}
}

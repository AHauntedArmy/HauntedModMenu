using UnityEngine;

using HauntedModMenu.Utils;

namespace HauntedModMenu.Menu
{
	class MenuController : Buttons.HandTrigger
	{
		private bool autoClose;
		private float lookSensitivty;
		private Collider menuTrigger = null;
		private GameObject menu = null;

		private struct PositionOffset
		{
			public Vector3 leftHandPosition;
			public Vector3 rightHandPosition;
			public Quaternion leftHandRotation;
			public Quaternion rightHandRotation;
		}

		private PositionOffset triggerOffset = new PositionOffset {
			leftHandPosition = new Vector3(-0.0315f, 0.035f, 0f),
			rightHandPosition = new Vector3(0.0315f, 0.035f, 0f),
			leftHandRotation = Quaternion.Euler(-30f, 120f, 75f),
			rightHandRotation = Quaternion.Euler(-30f, -120f, -75f)
		};

		private PositionOffset menuOffset = new PositionOffset {
			leftHandPosition = new Vector3(1.05f, 1.75f, -1.35f),
			rightHandPosition = new Vector3(-1.05f, 1.75f, -1.35f),
			leftHandRotation = Quaternion.Euler(-25f, 17f, -18.5f),
			rightHandRotation = Quaternion.Euler(-25f, -17f, 18.5f)
		};

		protected override void Awake()
		{
			base.Awake();

			LoadConfig();
			CreateMenuView();
			SetParent();

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

		protected override void OnDisable()
		{
			base.OnDisable();

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
		}

		private void Update()
		{
			if (RefCache.CameraTransform == null)
				return;

			Vector3 handDir = Vector3.Normalize(this.gameObject.transform.position - RefCache.CameraTransform.position);
			Vector3 lookDir = RefCache.CameraTransform.forward;

			float dotAngle = Vector3.Dot(handDir, lookDir);
			if (dotAngle < lookSensitivty) {
				if (autoClose && menu.activeSelf)
					HandTriggered();

				return;
			}

			dotAngle = Vector3.Dot(this.gameObject.transform.forward, lookDir);
			
			if (!menuTrigger.enabled) {
				if (dotAngle > lookSensitivty)
					menuTrigger.enabled = true;

			} else if (menuTrigger.enabled && dotAngle < 0.3f) {
				if (autoClose && menu.activeSelf)
					HandTriggered();

				menuTrigger.enabled = false;
			}
		}

		private void LoadConfig()
		{
			autoClose = Config.LoadData("Hand Config", "Auto Close", "whether or not to automatically close the menu when its out of view", true);
			leftHand = Config.LoadData("Hand Config", "LeftHand", "which hand the menu is on, true = left hand, false = right hand.", true);
			handSensitivity = Config.LoadData("Hand Config", "Hand Speed Sensitivty", "how slow the hand has to be moving to activate the trigger. higher number = more sensitive", 0.8f);
			lookSensitivty = Config.LoadData("Hand Config", "Look Sensitivity", "the angle threshold between the camera and the hand need to acivate, value between -1 and 1, -1 = 180 offset (always on), 1 being prefectly inline with the camera. reccomneded 0.7", 0.7f);

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
			Transform parent = leftHand ? RefCache.LeftHandRig?.transform : RefCache.RightHandRig?.transform;

			if (parent != null) {
				Transform myTransform = this.gameObject.transform;
				myTransform.SetParent(parent);

				myTransform.localPosition = leftHand ? triggerOffset.leftHandPosition : triggerOffset.rightHandPosition;
				myTransform.localRotation = leftHand ? triggerOffset.leftHandRotation : triggerOffset.rightHandRotation;
				myTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

				if(menu != null) {
					menu.transform.localPosition = leftHand ? menuOffset.leftHandPosition : menuOffset.rightHandPosition;
					menu.transform.localRotation = leftHand ? menuOffset.leftHandRotation : menuOffset.rightHandRotation;
				}
			}
		}

		public void SetHand(bool lHand)
		{
			leftHand = lHand;
			Config.SaveData("Hand Config", "LeftHand", lHand);
			SetParent();
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
			go.transform.localScale = new Vector3(1.8f, 3.5f, 0.05f);

			go.AddComponent<MenuView>();

			go.SetActive(false);
			menu = go;
		}
	}
}

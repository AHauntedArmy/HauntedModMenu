using UnityEngine;

using HauntedModMenu.Utils;

namespace HauntedModMenu.Menu
{
	class MenuController : Buttons.HandTrigger
	{
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

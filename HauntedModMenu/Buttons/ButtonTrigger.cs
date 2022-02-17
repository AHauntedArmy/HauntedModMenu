using UnityEngine;
using UnityEngine.UI;

namespace HauntedModMenu.Buttons
{
	class ButtonTrigger : HandTrigger
	{
		public Color EnabledColor { get; set; }
		public Color DisabledColor { get; set; }
		public Text ButtonText { get; private set; }
		public Material ButtonMaterial { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			ButtonText = this.gameObject.GetComponentInChildren<Text>();
			ButtonMaterial = this.gameObject.GetComponent<Renderer>()?.material;
		}

		public void SetColour(bool enabled)
		{
			Debug.Log("set colour called");
			if (ButtonMaterial == null)
				return;

			Debug.Log("attempting to set colour");
			ButtonMaterial.SetColor("_Color", enabled ? EnabledColor : DisabledColor);
		}
	}
}

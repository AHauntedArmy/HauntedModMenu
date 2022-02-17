using UnityEngine;

namespace HauntedModMenu.Buttons
{
	class ModButtonTrigger : ButtonTrigger
	{
		private Utils.ModInfo modTarget = null;
		public Utils.ModInfo ModTarget { 
			get => modTarget; 
			set {
				modTarget = value;

				if (value != null) {
					SetColour(value.Enabled);
				
				} else {
					SetColour(false);
				}
			} 
		}

		protected override void HandTriggered()
		{
			if (modTarget == null) return;

			bool toEnable = !modTarget.Enabled;
			modTarget.Enabled = toEnable;
			SetColour(toEnable);
		}
	}
}

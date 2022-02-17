
namespace HauntedModMenu.Buttons
{
	class ModButtonTrigger : ButtonTrigger
	{
		public Utils.ModInfo ModTarget { 
			get => ModTarget; 
			set {
				ModTarget = value;

				if (value != null) {
					SetColour(value.Enabled);
				
				} else {
					SetColour(false);
				}
			} 
		}

		protected override void HandTriggered()
		{
			if (ModTarget == null) return;

			bool toEnable = !ModTarget.Enabled;
			ModTarget.Enabled = toEnable;
			SetColour(toEnable);
		}
	}
}

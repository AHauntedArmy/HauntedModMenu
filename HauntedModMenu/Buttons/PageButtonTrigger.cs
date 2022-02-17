using System;

namespace HauntedModMenu.Buttons
{
	class PageButtonTrigger : ButtonTrigger
	{
		public Action PageUpdate { get; set; }

		protected override void HandTriggered()
		{
			PageUpdate();
		}
	}
}

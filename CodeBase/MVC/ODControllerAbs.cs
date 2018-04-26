using System;

namespace CodeBase.MVC {
	///<summary>Base controller class that should be used by every form and control that uses the MVC paradigm.</summary>
	public class ODControllerAbs<V> {
		protected V _view;

		public ODControllerAbs(V view) {
			_view=view;
		}

		///<summary>Invoked just after the view has been initialized but before the view has been shown to the user.
		///Any logic within this method will happen before the Load event of the view.
		///Because this gets invoked prior to the Load event, do not attempt to call _view.TryGetModelFromView() here.</summary>
		public virtual void OnPostInit() {
		}

	}
}

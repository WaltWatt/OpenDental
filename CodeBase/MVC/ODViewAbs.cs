using System;
using System.Windows.Forms;

namespace CodeBase.MVC {
	///<summary>Base view class that provides methods that are required to be invoked in order to wire the model and controller to the view.
	///Most views will not directly implement this abstract class but will instead </summary>
	///<typeparam name="M">Model - Two deep copies of this model are made on Init and will be available at all times to the view.</typeparam>
	///<typeparam name="V">View - This will seem redundant but is necessary so that the controller can utilize the view as needed.</typeparam>
	///<typeparam name="C">Controller - Required so that Init can correctly wire up this view to the controller passed in.</typeparam>
	///<typeparam name="P">Processor Type - Typically set to Signalod but can be set to any object type that gets processed by the view.</typeparam>
	public class ODViewAbs<M,V,C,P> : ODFormAbs<P> where M : ODModelAbs<M> where C : ODControllerAbs<V> {
		///<summary>Only set to true after Init has been invoked.  It is imparitive that Init get invoked prior to utilizing this view.</summary>
		private bool _isInitialized=false;
		///<summary>The controller associated to this view.</summary>
		protected C _controller;
		///<summary>Deep copy instance of the original model. Used to compare our changes.</summary>
		protected M _modelOld;
		///<summary>Deep copy instance of the original model that will be modified throughout the lifetime of the view.</summary>
		protected M _model;

		///<summary>Build a deep copy model from the current values entered into the view.  Not abstract due to Visual Studio designer error.
		///All views must override this method otherwise they will get an exception at runtime.</summary>
		public virtual bool TryGetModelFromView(out M model) {
			throw new NotImplementedException();
		}

		///<summary>Marry the model and controller to this view. 
		///This cannot be moved to the constructor because we do not have the model and controller at that point.
		///This will be called by Show(M,C) and ShowDialog(M,C) but can also be called manually before calling one of the Show overloads.
		///If called a second time on the same instance, this method returns immediately and does nothing.</summary>
		public void Init(M model,C controller) {
			if(_isInitialized) {
				return;
			}
			_model=model.Copy();
			_modelOld=_model.Copy();
			_controller=controller;
			_isInitialized=true;
			controller.OnPostInit();
		}

		///<summary>Connects the MVC wires before calling Form.ShowDialog().</summary>
		public DialogResult ShowDialog(M model,C controller,IWin32Window owner=null) {
			Init(model,controller);
			if(owner==null) {
				return base.ShowDialog();
			}
			else {
				return base.ShowDialog(owner);
			}
		}

		///<summary>Connects the MVC wires before calling Form.Show().</summary>
		public void Show(M model,C controller,IWin32Window owner=null) {
			Init(model,controller);
			if(owner==null) {
				base.Show();
			}
			else {
				base.Show(owner);
			}
		}
	}
}

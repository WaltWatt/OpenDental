using System;

namespace CodeBase.MVC {
	///<summary>Base model class that should be used by every model (based off of table types) that uses the MVC paradigm.</summary>
	public class ODModelAbs<M> where M : ODModelAbs<M> {
		///<summary>Returns a deep copy of the entire model.  This will typically entail more than just a deep copy of a simple table type.
		///Not abstract due to Visual Studio designer error.  All models must override this method otherwise they will get an exception at runtime.</summary>
		public virtual M Copy() {
			throw new NotImplementedException();
		}
	}
}

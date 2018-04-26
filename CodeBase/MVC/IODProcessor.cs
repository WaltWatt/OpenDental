using System.Collections.Generic;

namespace CodeBase.MVC {
	///<summary>An interface meant to be implemented by forms or views that process things (typically signals).</summary>
	public interface IODProcessor<T> {
		void ProcessObjects(List<T> listObjs);
	}
}

using CodeBase.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDental {
	public class BasicTemplateModel : ODModelAbs<BasicTemplateModel> {
		///<summary>Models can be filled with any type of object.  Make sure to make deep copies of every object type correctly.</summary>
		public override BasicTemplateModel Copy() {
			return (BasicTemplateModel)this.MemberwiseClone();
		}
	}
}

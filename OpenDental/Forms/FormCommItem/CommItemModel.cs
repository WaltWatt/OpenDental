using OpenDentBusiness;
using System.Collections.Generic;
using System;
using System.Linq;
using CodeBase.MVC;

namespace OpenDental {
	public class CommItemModel : ODModelAbs<CommItemModel> {
		public Commlog CommlogCur;
		private List<Def> _listCommlogTypeDefs;

		///<summary>The list of non-hidden CommLogTypes defs.  
		///This property is mainly used as an example to show how we might use business logic within models.</summary>
		public List<Def> ListCommlogTypeDefs {
			get {
				if(_listCommlogTypeDefs==null) {
					_listCommlogTypeDefs=Defs.GetDefsForCategory(DefCat.CommLogTypes,true);
					if(!PrefC.IsODHQ) {
						_listCommlogTypeDefs.RemoveAll(x => x.ItemValue==CommItemTypeAuto.ODHQ.ToString());
					}
				}
				return _listCommlogTypeDefs;
			}
		}
		
		public override CommItemModel Copy() {
			return new CommItemModel() {
				CommlogCur=this.CommlogCur.Copy(),
				_listCommlogTypeDefs=ListCommlogTypeDefs.Select(x => x.Copy()).ToList(),
			};
		}
	}
}

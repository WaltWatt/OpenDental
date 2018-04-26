using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsCore {
	public class DefLinkT {

		public static DefLink CreateDefLink(long defNum,long fKey,DefLinkType linkType) {
			DefLink defLink=new DefLink() {
				DefNum=defNum,
				FKey=fKey,
				LinkType=linkType,
			};
			DefLinks.Insert(defLink);
			return defLink;
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;

namespace OpenDentBusiness {
	public interface IODEvent {
		void FireEvent(ODEventArgs e);
	}
}

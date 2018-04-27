using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.WebServices {
	public class OpenDentalServerMockIIS : IOpenDentalServer {

		public new string ProcessRequest(string dtoString) {
			return RunWebMethod(() => DtoProcessor.ProcessDto(dtoString));
		}

		///<summary></summary>
		private static string RunWebMethod(Func<string> funcWebMethod) {
			Exception ex=null;
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Change the thread static remoting role to ServerWeb and then execute the func that was passed in.
				RemotingClient.SetRemotingRoleT(RemotingRole.ServerWeb);
				o.Tag=funcWebMethod();
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => {
				ex=e;//Cannot throw exception here due to still being in a threaded context.
			}));
			if(ex!=null) {
				throw ex;//Should never happen because this would be a "web exception" as in the ProcessRequest could not even invoke or something like that.
			}
			odThread.Name="threadMiddleTierUnitTestRunWebMethod";
			odThread.Start(true);
			odThread.Join(System.Threading.Timeout.Infinite);
			return (string)odThread.Tag;
		}

	}
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeBase{
  public class ODEnvironment{
		///<summary>Constant that represnets number of milliseconds that can elapse between a first click and a second click 
		///for OD to consider the mouse action a double-click.
		///Currently used in ODGrid when HasDistinctClickEvents is true.
		///Eventually we may want to add pref to allow the user to set this.</summary>
		public const int DoubleClickTime=200;

		//public static bool Is64BitOperatingSystem(){
		//  string arch="";
		//  try{
		//      arch=Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
		//  }catch{
		//      //May fail if the environemnt variable is not present on the target machine (i.e. Unix).
		//  }
		//  bool retVal=Regex.IsMatch(arch,".*64.*");
		//  return retVal; 
		//}

		///<summary>Will return true if the provided id matches the local computer name or a local IPv4 or IPv6 address. Will return false if id is 'localhost' or '127.0.0.1'. Returns false in all other cases.</summary>
		public static bool IdIsThisComputer(string id){
			id=id.ToLower();
			//Compare ID against the local host name.
			if(Environment.MachineName.ToLower()==id){
			  return true;
			}
			IPHostEntry iphostentry;
			try {
				iphostentry=Dns.GetHostEntry(Environment.MachineName);
			}
			catch {
				return false;
			}
			//Check against the local computer's IP addresses (does not include 127.0.0.1). Includes IPv4 and IPv6.
			foreach(IPAddress ipaddress in iphostentry.AddressList){
			  if(ipaddress.ToString()==id){
			    return true;
			  }
			}
			return false;
		}

		///<summary>Will return true if the provided servername matches the local computer name or a local IPv4 or IPv6 address.  Will return true if servername is 'localhost' or '127.0.0.1'.  Returns false in all other cases.</summary>
		public static bool IsRunningOnDbServer(string servername) {
			servername=servername.ToLower();
			//Compare servername against the local host name.  Also check if the servername is "localhost".
			if(Environment.MachineName.ToLower()==servername || servername=="localhost") {
				return true;
			}
			//Check to see if the servername is an ipaddress that is a loopback (127.XXX.XXX.XXX).  Catches failure in parsing.
			try {
				if(IPAddress.IsLoopback(IPAddress.Parse(servername))) {
					return true;
				}
			}
			catch { }	//not a valid IP address
			IPHostEntry iphostentry;
			try {
				iphostentry=Dns.GetHostEntry(Environment.MachineName);
			}
			catch {
				return false;
			}
			//Check against the local computer's IP addresses (does not include 127.0.0.1). Includes IPv4 and IPv6.
			foreach(IPAddress ipaddress in iphostentry.AddressList) {
				if(ipaddress.ToString()==servername) {
					return true;
				}
			}
			return false;
		}

		///<summary>Returns an IPv4 address for the local machine. Returns an empty string if one cannot be found.</summary>
		public static string GetLocalIPAddress() {
			IPHostEntry iphostentry;
			try {
				iphostentry=Dns.GetHostEntry(Environment.MachineName);
			}
			catch {
				return "";
			}
			foreach(IPAddress ip in iphostentry.AddressList) {
				if(ip.AddressFamily==AddressFamily.InterNetwork) {
					return ip.ToString();
				}
			}
			return "";
		}
	}
}

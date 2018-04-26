using System;
using System.Windows.Forms;

namespace OpenDental
{
	///<summary>This is a more efficient version of the MS MessageBox.</summary>
	public class MsgBox{
		

		///<summary>This is a more efficient version of the MS MessageBox. It also automates the language translation. Do NOT use if the text is variable in any way, because it will mess up the translation feature.</summary>
		public static void Show(System.Object sender,string text){
			MessageBox.Show(Lan.g(sender.GetType().Name,text));
		}

		///<summary>This is a more efficient version of the MS MessageBox. It also automates the language translation. 
		///Do NOT use if the text or titleBarText is variable in any way, because it will mess up the translation feature.</summary>
		public static void Show(System.Object sender,string text,string titleBarText) {
			MessageBox.Show(Lan.g(sender.GetType().Name,text),Lan.g(sender.GetType().Name,titleBarText));
		}

		///<summary>This is a more efficient version of the MS MessageBox. It also automates the language translation. Returns true if result is OK or Yes.</summary>
		public static bool Show(System.Object sender,MsgBoxButtons buttons,string question) {
			if(buttons==MsgBoxButtons.OKCancel) {
				if(MessageBox.Show(Lan.g(sender.GetType().Name,question),"",MessageBoxButtons.OKCancel)==DialogResult.OK) {
					return true;
				}
				else {
					return false;
				}
			}
			else if(buttons==MsgBoxButtons.YesNo) {
				if(MessageBox.Show(Lan.g(sender.GetType().Name,question),"",MessageBoxButtons.YesNo)==DialogResult.Yes) {
					return true;
				}
				else {
					return false;
				}
			}
			return false;
		}

		///<summary>This is a more efficient version of the MS MessageBox. It also automates the language translation. 
		///Returns true if result is OK or Yes.</summary>
		public static bool Show(System.Object sender,MsgBoxButtons buttons,string question,string titleBarText) {
			if(buttons==MsgBoxButtons.OKCancel) {
				if(MessageBox.Show(Lan.g(sender.GetType().Name,question),
					Lan.g(sender.GetType().Name,titleBarText),MessageBoxButtons.OKCancel)==DialogResult.OK) 
				{
					return true;
				}
				else {
					return false;
				}
			}
			else if(buttons==MsgBoxButtons.YesNo) {
				if(MessageBox.Show(Lan.g(sender.GetType().Name,question),
					Lan.g(sender.GetType().Name,titleBarText),MessageBoxButtons.YesNo)==DialogResult.Yes) 
				{
					return true;
				}
				else {
					return false;
				}
			}
			return false;
		}

		///<summary>deprecated</summary>
		public static bool Show(System.Object sender,bool okCancel,string question){
			return Show(sender,MsgBoxButtons.OKCancel,question);
		}

		


	}

	///<summary></summary>
	public enum MsgBoxButtons {
		OKCancel,
		YesNo
	}

}

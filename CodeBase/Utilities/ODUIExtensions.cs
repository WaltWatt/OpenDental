using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeBase {
	public static class ODUIExtensions {

		[DllImport("user32.dll")]
		private static extern int ShowWindow(IntPtr hWnd,uint Msg);
		private const uint SW_RESTORE=0x09;

		///<summary>This will restore a minimized window.  If the window is not minimized, this does nothing.  This behaves as though the user clicked on
		///the task bar to restore the window.  The window will return to the state prior to being minimized, i.e. if it was maximized and then minimized,
		///this will restore the window to the maximized state; and if its state was FormWindowState.Normal but resized it will be restored to the Normal
		///state with the resized dimensions.</summary>
		public static void Restore(this Form f) {
			if(f.WindowState==FormWindowState.Minimized) {
				ShowWindow(f.Handle,SW_RESTORE);
			}
		}

		///<summary>Does nothing to the object except reference it. Usefull for handling the Exception ex declared but never used warning.</summary>
		public static void DoNothing(this Exception ex) {}

		///<summary>Attempts to select the index provided for the combo box. If idx is -1 or otherwise invalid, then getOverrideText() will be used to get displayed text instead.</summary>
		/// <param name="combo">This is an extension method. This is the control that will be affected.</param>
		/// <param name="idx">Instead of providing an idx directly, use a function that determines the index to select. i.e. _listObj.FindIndex(x=>x.Num==SelectedNum).</param>
		/// <param name="getOverrideText">This should be a function that takes no parameters and returns a string. Linq statements and anonymous functions work best. 
		/// I.E. comboBox.SelectIndex(myCombo,_listObj.FindIndex(x=>x.Num==SelectedNum),</param>
		public static void IndexSelectOrSetText(this ComboBox combo,int idx,Func<string> getOverrideText) {
			combo.SelectedIndexChanged-=ComboSelectedIndex;//Reset visual style when user selects from dropdown.
			combo.KeyPress-=NullKeyPressHandler;//re-enable editing if neccesary
			combo.KeyDown-=NullKeyEventHandler;//re-enable editing if neccesary
			combo.KeyUp-=NullKeyEventHandler;//re-enable editing if neccesary
			//Normal index selection. No special logic needed.
			if(idx>-1 && idx<combo.Items.Count) {
				combo.DropDownStyle=ComboBoxStyle.DropDownList;
				combo.SelectedIndex=idx;
				return;
			}
			//Selected index is not part of the selected list, use the getOverrideText function provided.
			combo.SelectedIndexChanged+=ComboSelectedIndex;//Reset visual style when user selects from dropdown.
			combo.KeyPress+=NullKeyPressHandler;//disable editing.
			combo.KeyDown+=NullKeyEventHandler;//disable editing.
			combo.KeyUp+=NullKeyEventHandler;//disable editing.
			combo.SelectedIndex=-1;
			combo.DropDownStyle=ComboBoxStyle.DropDown;
			combo.Text=getOverrideText();
			if(string.IsNullOrEmpty(combo.Text)) {
				//In case the getOverrideText function returns an empty string
				combo.DropDownStyle=ComboBoxStyle.DropDownList;
			}
		}

		///<summary>Only use this overload if you already have a display string available. 
		///Use SelectIndex(int idx,Func&lt;string> getOverrideText) instead to delay execution of the delegate if it is not needed.</summary>
		public static void SelectIndex(this ComboBox combo,int idx,string OverrideText) {
			combo.IndexSelectOrSetText(idx,() => { return OverrideText; });
		}

		///<summary>Sets the selected item that matches the func passed in. Will only work if the Items in the combo box are ODBoxItems.</summary>
		///<param name="fSelectItem">A func that takes an object that is the same type as the ODBoxItems Tags and returns a bool, i.e.
		///x => x.ClinicNum==0.</param>
		///<param name="overrideText">The text to display in the combo box if a matching item cannot be found.</param>
		public static void SetSelectedItem<T>(this ComboBox combo,Func<T,bool> fSelectItem,string overrideText) {
			int idx=-1;
			for(int i=0;i<combo.Items.Count;i++) {
				ODBoxItem<T> odBoxItem=combo.Items[i] as ODBoxItem<T>;
				if(odBoxItem==null) {
					continue;
				}
				if(fSelectItem(odBoxItem.Tag)) {
					idx=i;
					break;
				}
			}
			combo.SelectIndex(idx,overrideText);
		}

		///<summary>Sets e.Handled to true to prevent event from firing.</summary>
		private static void NullKeyPressHandler(object sender,KeyPressEventArgs e) {
			e.Handled=true;
		}

		///<summary>Sets e.Handled to true to prevent event from firing.</summary>
		private static void NullKeyEventHandler(object sender,KeyEventArgs e) {
			e.Handled=true;
		}

		///<summary>Used to call comboBox.SelectIndex after user manually selects an item from the list. Should reset visual style to dropdown.</summary>
		private static void ComboSelectedIndex(object sender,EventArgs e) {
			ComboBox box = (ComboBox)sender;//capturing variable for readability
			box.IndexSelectOrSetText(box.SelectedIndex,() => { return box.Text; });
		}

		///<summary>Invoke an action on a control.</summary>
		public static void Invoke(this Control control,Action action) {
			control.Invoke((Delegate)action);
		}

		///<summary>BeginInvoke an action on a control.</summary>
		public static void BeginInvoke(this Control control,Action action) {
			control.BeginInvoke((Delegate)action);
		}

		///<summary>Returns the tag of the selected item. The items in the ComboBox must be ODBoxItems.</summary>
		public static T SelectedTag<T>(this ComboBox comboBox) {
			if(comboBox.SelectedItem is ODBoxItem<T>) {
				return (comboBox.SelectedItem as ODBoxItem<T>).Tag;
			}
			return default(T);
		}

		///<summary>Returns the tag of the selected item. The items in the ComboBox must be ODBoxItems.</summary>
		public static T SelectedTag<T>(this ListBox listBox) {
			if(listBox.SelectedItem is ODBoxItem<T>) {
				return (listBox.SelectedItem as ODBoxItem<T>).Tag;
			}
			return default(T);
		}
		
		///<summary>Returns the tags of the selected items. The items in the ComboBox must be ODBoxItems.</summary>
		public static List<T> SelectedTags<T>(this ListBox listBox) {
			List<T> listSelected=new List<T>();
			foreach(object selectedItem in listBox.SelectedItems) {
				if(selectedItem is ODBoxItem<T>) {
					listSelected.Add((selectedItem as ODBoxItem<T>).Tag);
				}
			}
			return listSelected;
		}
	}
}

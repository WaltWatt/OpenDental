using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;

namespace OpenDental {
	public partial class FormEServicesSetup {
	
		///<summary>The fake clinic num used for the "Default" clinic option in the WebSchedVerify clinic combo box.</summary>
		private const int DEF_CLINIC_NUM=-1;
		///<summary>The in-memory list of updated ClinicPrefs related to WebSchedVerify, used to track changes made while the window is open.</summary>
		private List<ClinicPref> _listWebSchedVerifyClinicPrefs=new List<ClinicPref>();
		///<summary>The in-memory list of original ClinicPrefs related to WebSchedVerify, used to compare changes when saving on window close.</summary>
		private List<ClinicPref> _listWebSchedVerifyClinicPrefs_Old;
		///<summary>The list of prefNames modified by the WebSched Verify tab.</summary>
		private List<PrefName> _listWebSchedVerifyPrefNames=new List<PrefName>() {
				PrefName.WebSchedVerifyRecallType,
				PrefName.WebSchedVerifyRecallText,
				PrefName.WebSchedVerifyRecallEmailSubj,
				PrefName.WebSchedVerifyRecallEmailBody,
				PrefName.WebSchedVerifyNewPatType,
				PrefName.WebSchedVerifyNewPatText,
				PrefName.WebSchedVerifyNewPatEmailSubj,
				PrefName.WebSchedVerifyNewPatEmailBody,
				PrefName.WebSchedVerifyASAPType,
				PrefName.WebSchedVerifyASAPText,
				PrefName.WebSchedVerifyASAPEmailSubj,
				PrefName.WebSchedVerifyASAPEmailBody,
			};

		private bool IsTabValidWebSchedVerify() {
			if(Patients.DoesContainPHIField(textRecallTextTemplate.Text)) {
				MsgBox.Show(this,"Web Sched Verify Recall Text Template is not allowed to contain Protected Health Information.");
				return false;
			}
			if(Patients.DoesContainPHIField(textNewPatTextTemplate.Text)) {
				MsgBox.Show(this,"Web Sched Verify New Patient Text Template is not allowed to contain Protected Health Information.");
				return false;
			}
			if(Patients.DoesContainPHIField(textASAPTextTemplate.Text)) {
				MsgBox.Show(this,"Web Sched Verify ASAP Text Template is not allowed to contain Protected Health Information.");
				return false;
			}
			return true;
		}

		#region Load-in
		///<summary>Loading routine for the WebSchedVerify tab.</summary>
		private void FillTabWebSchedVerify() {
			//Load in an existing list of clinicprefs so we can keep in in-memory record of changes
			ClinicPrefs.RefreshCache();
			foreach(PrefName prefName in _listWebSchedVerifyPrefNames) {
				_listWebSchedVerifyClinicPrefs.AddRange(ClinicPrefs.GetPrefAllClinics(prefName));
				Pref pref=Prefs.GetPref(prefName.ToString());
				_listWebSchedVerifyClinicPrefs.Add(new ClinicPref() { ClinicNum=DEF_CLINIC_NUM, PrefName=prefName, ValueString=pref.ValueString });
			}
			_listWebSchedVerifyClinicPrefs_Old=_listWebSchedVerifyClinicPrefs.Select(x => x.Clone()).ToList();
			//Fill in the UI
			WebSchedVerifyFillClinics();
			WebSchedVerifyFillTemplates();
		}

		///<summary>Fill in the ClinicComboBox if applicable.</summary>
		private void WebSchedVerifyFillClinics() {
			if(PrefC.HasClinicsEnabled) {
				labelClinicVerify.Visible=true;
				comboClinicVerify.Visible=true;
				checkUseDefaultsVerify.Visible=true;
				if(!Security.CurUser.ClinicIsRestricted) {
					Clinic clinic=new Clinic { Abbr=Lan.g("ComboBoxClinic","Default"),Description=Lan.g("ComboBoxClinic","Default"),ClinicNum=DEF_CLINIC_NUM };
					comboClinicVerify.Items.Insert(0,new ODBoxItem<Clinic>(Lan.g("ComboBoxClinic","Default"),clinic));
				}
				if(Clinics.ClinicNum==0) {
					comboClinicVerify.SelectedIndex=0;
				}
			}
			else {
				labelClinicVerify.Visible=false;
				comboClinicVerify.Visible=false;
				checkUseDefaultsVerify.Visible=false;
			}
		}

		///<summary>Fill in the template data for the current clinic.</summary>
		private void WebSchedVerifyFillTemplates() {
			//Load Recall values
			WebSchedVerify_SetRadioButtonVal(PrefName.WebSchedVerifyRecallType,groupBoxRadioRecall);
			textRecallTextTemplate.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyRecallText);
			textRecallEmailSubj.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyRecallEmailSubj);
			textRecallEmailBody.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyRecallEmailBody);
			//Load NewPat values
			WebSchedVerify_SetRadioButtonVal(PrefName.WebSchedVerifyNewPatType,groupBoxRadioNewPat);
			textNewPatTextTemplate.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyNewPatText);
			textNewPatEmailSubj.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyNewPatEmailSubj);
			textNewPatEmailBody.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyNewPatEmailBody);
			//Load ASAP values
			WebSchedVerify_SetRadioButtonVal(PrefName.WebSchedVerifyASAPType,groupBoxRadioASAP);
			textASAPTextTemplate.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyASAPText);
			textASAPEmailSubj.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyASAPEmailSubj);
			textASAPEmailBody.Text=WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyASAPEmailBody);
		}

		#endregion Load-in

		///<summary>Save template changes made in WebSchedVerify.</summary>
		private void SaveTabWebSchedVerify() {
			List<long> listClinics=Clinics.GetForUserod(Security.CurUser).Select(x => x.ClinicNum).ToList();
			foreach(PrefName prefName in _listWebSchedVerifyPrefNames) {
				foreach(long clinicNum in listClinics) {
					ClinicPref newClinicPref=_listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.PrefName==prefName && x.ClinicNum==clinicNum);
					ClinicPref oldClinicPref=_listWebSchedVerifyClinicPrefs_Old.FirstOrDefault(x => x.PrefName==prefName && x.ClinicNum==clinicNum);
					if(oldClinicPref==null && newClinicPref==null) { //skip items not in either list
						continue;
					}
					else if(oldClinicPref==null && newClinicPref!=null) { //insert items in the new list and not the old list
						ClinicPrefs.Insert(newClinicPref);
					}
					else if(oldClinicPref!=null && newClinicPref==null) { //delete items in the old list and not the new list
						ClinicPrefs.Delete(oldClinicPref.ClinicPrefNum);
					}
					else { //update items that have changed
						ClinicPrefs.Update(newClinicPref,oldClinicPref);
					}
				}
				ClinicPref newPref=_listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.PrefName==prefName && x.ClinicNum==DEF_CLINIC_NUM);
				if(newPref!=null) {
					Prefs.UpdateString(prefName,newPref.ValueString);
				}
			}
		}

		#region Event handlers

		///<summary>Event handler for CheckUseDefaults check changed.</summary>
		private void WebSchedVerify_CheckUseDefaultsChanged(object sender,EventArgs e) {
			if(checkUseDefaultsVerify.Checked) {
				groupBoxRecall.Enabled=false;
				groupBoxNewPat.Enabled=false;
				groupBoxASAP.Enabled=false;
				_listWebSchedVerifyClinicPrefs.RemoveAll(x => x.ClinicNum==comboClinicVerify.SelectedClinicNum);		
			}
			else {
				groupBoxRecall.Enabled=true;
				groupBoxNewPat.Enabled=true;
				groupBoxASAP.Enabled=true;
				//Only do this logic if the check change result from the user manually checking the box, not from changing clinics
				if(!_listWebSchedVerifyClinicPrefs.Any(x => x.ClinicNum==comboClinicVerify.SelectedClinicNum)) {
					foreach(PrefName prefName in _listWebSchedVerifyPrefNames) {
						WebSchedVerify_TryRestoreClinicPrefOld(prefName);
					}
				}
			}
			WebSchedVerifyFillTemplates();
		}

		///<summary>Event handler for ComboClinics index changed.</summary>
		private void WebSchedVerify_ComboClinicSelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinicVerify.SelectedClinicNum==DEF_CLINIC_NUM) {//'Default' is selected.
				checkUseDefaultsVerify.Visible=false;
				checkUseDefaultsVerify.Checked=false;
			}
			else {
				checkUseDefaultsVerify.Visible=true;
				if(!_listWebSchedVerifyClinicPrefs.Exists(x => x.ClinicNum==comboClinicVerify.SelectedClinicNum)) {
					checkUseDefaultsVerify.Checked=true;
				}
				else {
					checkUseDefaultsVerify.Checked=false;
				}
			}
			WebSchedVerifyFillTemplates();
		}

		///<summary>Event handler for RadioButtons check changed.</summary>
		private void WebSchedVerify_RadioButtonCheckChanged(object sender,EventArgs e) {
			RadioButton buttonCur=(RadioButton)sender;
			GroupBox groupBox=(GroupBox)buttonCur.Parent;
			if(buttonCur.Checked) {
				PrefName prefName=(PrefName)groupBox.Tag;
				WebSchedVerifyType verifyType=(WebSchedVerifyType)buttonCur.Tag;
				if(_listWebSchedVerifyClinicPrefs.Any(x => x.ClinicNum==comboClinicVerify.SelectedClinicNum)) {
					//We only want to do this part when the user manually checked this, not when the check-defaults forced it to change
					WebSchedVerify_UpdateClinicPref(prefName,POut.Int((int)verifyType));
				}
			}
		}

		///<summary>Event handler for TextBox leave.</summary>
		private void WebSchedVerify_TextLeave(object sender, EventArgs e) {
			TextBox textBox=(TextBox)sender;
			PrefName prefName=(PrefName)textBox.Tag;
			WebSchedVerify_UpdateClinicPref(prefName,textBox.Text);
		}

		/// <summary>All the user to undo all changes they have made to the currently selected clinic.</summary>
		private void WebSchedVerify_butUndoClick(object sender,EventArgs e) {
			bool isAccepted=MsgBox.Show(this,MsgBoxButtons.YesNo,"Undo all changes to templates in this clinic?");
			if(isAccepted) {
				foreach(PrefName prefName in _listWebSchedVerifyPrefNames) {
					WebSchedVerify_TryRestoreClinicPrefOld(prefName);
				}
				WebSchedVerifyFillTemplates();
			}
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuUndoClick(object sender,EventArgs e) {
			ToolStripItem item=(ToolStripItem)sender;
			ContextMenuStrip menu=(ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Undo();
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuCutClick(object sender,EventArgs e) {
			ToolStripItem item=(ToolStripItem)sender;
			ContextMenuStrip menu=(ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Cut();
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuCopyClick(object sender,EventArgs e) {
			ToolStripItem item=(ToolStripItem)sender;
			ContextMenuStrip menu=(ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Copy();
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuPasteClick(object sender,EventArgs e) {
			ToolStripItem item=(ToolStripItem)sender;
			ContextMenuStrip menu=(ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Paste();
		}
		
		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuSelectAllClick(object sender,EventArgs e) {
			ToolStripItem item=(ToolStripItem)sender;
			ContextMenuStrip menu=(ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).SelectAll();
		}

		/// <summary>Opens FormMessageReplacements to allow the user to select from replaceable tags to include in the templates.</summary>
		private void WebSchedVerify_ContextMenuReplacementsClick(object sender,EventArgs e) {
			ToolStripItem item=(ToolStripItem)sender;
			ContextMenuStrip menu=(ContextMenuStrip)item.Owner;
			TextBox textBox=((TextBox)menu.SourceControl);
			//PHI is not supposed to be communicated via text message.
			bool allowPHI=(!textBox.Name.In(textRecallTextTemplate.Name,textNewPatTextTemplate.Name,textASAPTextTemplate.Name));
			FormMessageReplacements FormMR=new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient,allowPHI);
			FormMR.IsSelectionMode=true;
			FormMR.ShowDialog();
			if(FormMR.DialogResult==DialogResult.OK) {
				textBox.SelectedText=FormMR.Replacement;
			}
		}

		#endregion Event handlers

		#region Helpers

		///<summary>Returns the clinic pref value for the currently selected clinic and provided PrefName, or the default pref if there is none.</summary>
		private string WebSchedVerify_GetTemplateVal(PrefName prefName) {
			ClinicPref clinicPref=_listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.ClinicNum==comboClinicVerify.SelectedClinicNum && x.PrefName==prefName);
			ClinicPref defaultPref=_listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.ClinicNum==DEF_CLINIC_NUM && x.PrefName==prefName);
			//ClinicPref won't be available if it has not been created previously.
			return clinicPref!=null ? clinicPref.ValueString : defaultPref.ValueString;
		}

		///<summary>Checks the currently selected radio button for the given PrefName and groupBox, based on the radio button tags.</summary>
		private void WebSchedVerify_SetRadioButtonVal(PrefName prefName, GroupBox groupBox) {
			WebSchedVerifyType type=(WebSchedVerifyType)PIn.Int(WebSchedVerify_GetTemplateVal(prefName));
			RadioButton buttonMatch=groupBox.Controls.OfType<RadioButton>().FirstOrDefault(x => (WebSchedVerifyType)x.Tag==type);
			buttonMatch.Checked=true;
		}

		///<summary>Updates the in-memory clinic pref list with the given valueString for the provided prefName and currently selected clinic.</summary>
		private void WebSchedVerify_UpdateClinicPref(PrefName prefName,string valueString) {
			ClinicPref clinicPref=_listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.ClinicNum==comboClinicVerify.SelectedClinicNum && x.PrefName==prefName);
			if(clinicPref==null) {
				_listWebSchedVerifyClinicPrefs.Add(new ClinicPref(){ PrefName=prefName, ClinicNum=comboClinicVerify.SelectedClinicNum, ValueString=valueString });
			}
			else {
				clinicPref.ValueString=valueString;
			}
		}

		/// <summary>Tries to get the original clinic pref that was loaded in when the form first opened, and reload it into the in-memory clinic pref 
		/// list. If there is no old pref, this loads the default pref value for that clinic into the in-memory list.</summary>
		private void WebSchedVerify_TryRestoreClinicPrefOld(PrefName prefName) {
			ClinicPref pref=_listWebSchedVerifyClinicPrefs_Old.FindAll(x => x.ClinicNum==comboClinicVerify.SelectedClinicNum && x.PrefName==prefName).FirstOrDefault();
			if(pref==null) {
				pref=_listWebSchedVerifyClinicPrefs.FindAll(x => x.ClinicNum==DEF_CLINIC_NUM && x.PrefName==prefName).First();
			}
			WebSchedVerify_UpdateClinicPref(prefName,pref.ValueString);
		}
		#endregion

	}
}

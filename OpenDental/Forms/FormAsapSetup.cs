using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormAsapSetup:ODForm {
		///<summary>Returns the ClinicNum of the selected clinic. Returns 0 if 'Default' is selected or if clinics are not enabled.</summary>
		private long _selectedClinicNum {
			get {
				if(!PrefC.HasClinicsEnabled) {
					return 0;
				}
				return ((ComboClinicItem)comboClinic.SelectedItem)?.ClinicNum??-1;
			}
		}

		public FormAsapSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormAsapSetup_Load(object sender,EventArgs e) {
			if(PrefC.HasClinicsEnabled) {
				FillClinics();
			}
			else {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
				checkUseDefaults.Visible=false;
			}
			FillPrefs();
		}

		private void FillClinics() {
			comboClinic.Items.Clear();
			List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
			if(!Security.CurUser.ClinicIsRestricted) {
				comboClinic.Items.Add(new ComboClinicItem(Lan.g(this,"Default"),0));
				comboClinic.SelectedIndex=0;
			}
			for(int i=0;i<listClinics.Count;i++) {
				int addedIdx=comboClinic.Items.Add(new ComboClinicItem(listClinics[i].Abbr,listClinics[i].ClinicNum));
				if(listClinics[i].ClinicNum==Clinics.ClinicNum) {
					comboClinic.SelectedIndex=addedIdx;
				}
			}
		}

		private void FillPrefs() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,""),250);//Random tidbits regarding the template
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Template"),500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			checkUseDefaults.Checked=true;
			string baseVars=Lan.g(this,"Available variables:")+" [NameF], [Date], [Time], [OfficeName], [OfficePhone]";
			ODGridRow row;
			row=BuildRowForTemplate(PrefName.ASAPTextTemplate,"Text manual",baseVars);
			gridMain.Rows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapTextTemplate,"Web Sched Text",baseVars+", [AsapURL]");
			gridMain.Rows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapEmailTemplate,"Web Sched Email Body",baseVars+", [AsapURL]");
			gridMain.Rows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapEmailSubj,"Web Sched Email Subject",baseVars);
			gridMain.Rows.Add(row);
			gridMain.EndUpdate();
			if(_selectedClinicNum==0) {
				textWebSchedPerDay.Text=PrefC.GetString(PrefName.WebSchedAsapTextLimit);
				checkUseDefaults.Checked=false;
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(PrefName.WebSchedAsapTextLimit,_selectedClinicNum);
				if(clinicPref==null || clinicPref.ValueString==null) {
					textWebSchedPerDay.Text=PrefC.GetString(PrefName.WebSchedAsapTextLimit);
				}
				else {
					textWebSchedPerDay.Text=clinicPref.ValueString;
					checkUseDefaults.Checked=false;
				}
			}
		}

		///<summary>Creates a row for the passed in template type. Unchecks checkUseDefaults if a clinic-level template is in use.</summary>
		private ODGridRow BuildRowForTemplate(PrefName prefName,string templateName,string availableVars) {
			string templateText;
			bool doShowDefault=false;
			if(_selectedClinicNum==0) {
				templateText=PrefC.GetString(prefName);
				checkUseDefaults.Checked=false;
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,_selectedClinicNum);
				if(clinicPref==null || clinicPref.ValueString==null) {
					templateText=PrefC.GetString(prefName);
					doShowDefault=true;
				}
				else {
					templateText=clinicPref.ValueString;
					checkUseDefaults.Checked=false;
				}
			}
			ODGridRow row=new ODGridRow();
			row.Cells.Add(Lan.g(this,templateName)+(doShowDefault ? " "+Lan.g(this,"(Default)") : ""));
			row.Cells.Add(availableVars);
			row.Cells.Add(templateText);
			row.Tag=prefName;
			return row;
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			ComboClinicItem selectedItem=(ComboClinicItem)comboClinic.SelectedItem;
			if(selectedItem.ClinicNum==0) {//'Default' is selected.
				checkUseDefaults.Visible=false;
			}
			else {
				checkUseDefaults.Visible=true;
			}
			FillPrefs();
		}

		private void checkUseDefaults_Click(object sender,EventArgs e) {
			List<PrefName> listPrefs=new List<PrefName> {
				PrefName.ASAPTextTemplate,
				PrefName.WebSchedAsapTextTemplate,
				PrefName.WebSchedAsapEmailTemplate,
				PrefName.WebSchedAsapEmailSubj,
				PrefName.WebSchedAsapTextLimit,
			};
			if(checkUseDefaults.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete custom templates for this clinic and switch to using defaults? This cannot be undone.")) {
					ClinicPrefs.DeletePrefs(_selectedClinicNum,listPrefs);
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
				else {
					checkUseDefaults.Checked=false;
				}
			}
			else {//Was checked, now user is unchecking it.
				bool wasChanged=false;
				foreach(PrefName pref in listPrefs) {
					if(ClinicPrefs.Upsert(pref,_selectedClinicNum,PrefC.GetString(pref))) {
						wasChanged=true;
					}
				}
				if(wasChanged) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
			FillPrefs();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PrefName prefName=(PrefName)gridMain.Rows[e.Row].Tag;
			FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
			if(_selectedClinicNum==0) {
				FormR.MessageVal=PrefC.GetString(prefName);
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,_selectedClinicNum);
				if(clinicPref==null || string.IsNullOrEmpty(clinicPref.ValueString)) {
					FormR.MessageVal=PrefC.GetString(prefName);
				}
				else {
					FormR.MessageVal=clinicPref.ValueString;
				}
			}
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			if(_selectedClinicNum==0) {
				if(Prefs.UpdateString(prefName,FormR.MessageVal)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				if(ClinicPrefs.Upsert(prefName,_selectedClinicNum,FormR.MessageVal)) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
			FillPrefs();
		}

		private void textWebSchedPerDay_Leave(object sender,EventArgs e) {
			if(!textWebSchedPerDay.IsValid) {
				return;
			}
			if(_selectedClinicNum==0) {
				if(Prefs.UpdateString(PrefName.WebSchedAsapTextLimit,textWebSchedPerDay.Text)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				if(ClinicPrefs.Upsert(PrefName.WebSchedAsapTextLimit,_selectedClinicNum,textWebSchedPerDay.Text)) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private class ComboClinicItem {
			public string DisplayName;
			public long ClinicNum;
			public ComboClinicItem(string displayName,long clinicNum) {
				DisplayName=displayName;
				ClinicNum=clinicNum;
			}
			public override string ToString() {
				return DisplayName;
			}
		}

	}
}
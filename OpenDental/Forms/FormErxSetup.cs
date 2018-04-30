using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDental.UI;

namespace OpenDental {
	///<summary>This form is used to make changes for the eRx program link.
	///With the integration of DoseSpot, the default program link form is no longer sufficient.</summary>
	public partial class FormErxSetup:ODForm {

		private Program _progCur;
		private ErxOption _eRxOption;
		private List<ProgramProperty> _listProgramProperties=new List<ProgramProperty>();

		private ProgramProperty ErxOptionPP {
			get {
				ProgramProperty retVal=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Erx.PropertyDescs.ErxOption);
				if(retVal==null) {
					throw new Exception("The database is missing an eRx option program property.");
				}
				return retVal;
			}
			set {
				int pos=_listProgramProperties.IndexOf(ErxOptionPP);
				_listProgramProperties[pos]=value;
			}
		}

		public FormErxSetup() {
			InitializeComponent();
			Lan.F(this);
			//We only show the tabs in the designer for development purposes.  We want to hide them for our users.
			//Because the tab control is in "flat buttons" appearance and "fixed size" style the tabs will not show even if they are one pixel tall.
			//0,0 does not work because some size is required.
			tabControlErxSoftware.ItemSize=new Size(0,1);
		}

		private void FormErxSetup_Load(object sender,EventArgs e) {
			try {
				_progCur=Programs.GetCur(ProgramName.eRx);
				if(_progCur==null) {
					throw new Exception("The eRx bridge is missing from the database.");
				}
				_listProgramProperties=ProgramProperties.GetForProgram(_progCur.ProgramNum);
				checkEnabled.Checked=_progCur.Enabled;
				_eRxOption=PIn.Enum<ErxOption>(ErxOptionPP.PropertyValue);
				if(_eRxOption==ErxOption.Legacy) {
					radioNewCrop.Checked=true;
				}
				else if(_eRxOption==ErxOption.DoseSpot) {
					radioDoseSpot.Checked=true;
					RelayoutForm();
				}
				textNewCropAccountID.Text=PrefC.GetString(PrefName.NewCropAccountId);
				List<ProgramProperty> listClinicIDs=_listProgramProperties.FindAll(x => x.PropertyDesc==Erx.PropertyDescs.ClinicID);
				List<ProgramProperty> listClinicKeys=_listProgramProperties.FindAll(x => x.PropertyDesc==Erx.PropertyDescs.ClinicKey);
				//Always make sure clinicnum 0 (HQ) exists, regardless of if clinics are enabled
				if(!listClinicIDs.Exists(x => x.ClinicNum==0)) {
					ProgramProperty ppClinicID=new ProgramProperty();
					ppClinicID.ProgramNum=_progCur.ProgramNum;
					ppClinicID.ClinicNum=0;
					ppClinicID.PropertyDesc=Erx.PropertyDescs.ClinicID;
					ppClinicID.PropertyValue="";
					_listProgramProperties.Add(ppClinicID);
				}
				if(!listClinicKeys.Exists(x => x.ClinicNum==0)) {
					ProgramProperty ppClinicKey=new ProgramProperty();
					ppClinicKey.ProgramNum=_progCur.ProgramNum;
					ppClinicKey.ClinicNum=0;
					ppClinicKey.PropertyDesc=Erx.PropertyDescs.ClinicKey;
					ppClinicKey.PropertyValue="";
					_listProgramProperties.Add(ppClinicKey);
				}
				if(PrefC.HasClinicsEnabled) {
					foreach(Clinic clinicCur in Clinics.GetAllForUserod(Security.CurUser)) {
						if(!listClinicIDs.Exists(x => x.ClinicNum==clinicCur.ClinicNum)) {//Only add a program property if it doesn't already exist.
							ProgramProperty ppClinicID=new ProgramProperty();
							ppClinicID.ProgramNum=_progCur.ProgramNum;
							ppClinicID.ClinicNum=clinicCur.ClinicNum;
							ppClinicID.PropertyDesc=Erx.PropertyDescs.ClinicID;
							ppClinicID.PropertyValue="";
							_listProgramProperties.Add(ppClinicID);
						}
						if(!listClinicKeys.Exists(x => x.ClinicNum==clinicCur.ClinicNum)) {//Only add a program property if it doesn't already exist.
							ProgramProperty ppClinicKey=new ProgramProperty();
							ppClinicKey.ProgramNum=_progCur.ProgramNum;
							ppClinicKey.ClinicNum=clinicCur.ClinicNum;
							ppClinicKey.PropertyDesc=Erx.PropertyDescs.ClinicKey;
							ppClinicKey.PropertyValue="";
							_listProgramProperties.Add(ppClinicKey);
						}
					}
				}
				else {
					checkShowHiddenClinics.Visible=false;
				}
				FillGridDoseSpot();
				SetRadioButtonChecked(radioNewCrop.Checked);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error loading the eRx program: ")+ex.Message);
				DialogResult=DialogResult.Cancel;
				return;
			}
		}
		
		private void FillGridDoseSpot() {
			gridProperties.BeginUpdate();
			gridProperties.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Clinic"),120);
			gridProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Clinic ID"),160);
			gridProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Clinic Key"),160);
			gridProperties.Columns.Add(col);
			gridProperties.Rows.Clear();
			DoseSpotGridRowModel clinicHqModel=new DoseSpotGridRowModel();
			clinicHqModel.Clinic=new Clinic();
			clinicHqModel.Clinic.ClinicNum=0;
			clinicHqModel.Clinic.Abbr=Lan.g(this,"Headquarters");
			clinicHqModel.ClinicIDProperty=GetPropertyForClinic(0,Erx.PropertyDescs.ClinicID);
			clinicHqModel.ClinicKeyProperty=GetPropertyForClinic(0,Erx.PropertyDescs.ClinicKey);
			gridProperties.Rows.Add(CreateDoseSpotGridRow(clinicHqModel));//If clinics isn't enabled, this will be the only row in the grid.
			if(PrefC.HasClinicsEnabled) {
				foreach(Clinic clinicCur in Clinics.GetAllForUserod(Security.CurUser)) {
					if(!checkShowHiddenClinics.Checked && clinicCur.IsHidden) {
						continue;
					}
					DoseSpotGridRowModel model=new DoseSpotGridRowModel();
					model.Clinic=clinicCur.Copy();
					model.ClinicIDProperty=GetPropertyForClinic(clinicCur.ClinicNum,Erx.PropertyDescs.ClinicID);
					model.ClinicKeyProperty=GetPropertyForClinic(clinicCur.ClinicNum,Erx.PropertyDescs.ClinicKey);
					gridProperties.Rows.Add(CreateDoseSpotGridRow(model));
				}
			}
			gridProperties.EndUpdate();
		}

		private ODGridRow CreateDoseSpotGridRow(DoseSpotGridRowModel model) {
			ODGridRow row=new ODGridRow();
			row.Cells.Add(model.Clinic.Abbr);
			row.Cells.Add(model.ClinicIDProperty==null ? "" : model.ClinicIDProperty.PropertyValue);
			row.Cells.Add(model.ClinicKeyProperty==null ? "" : model.ClinicKeyProperty.PropertyValue);
			row.Tag=model;
			return row;
		}

		private ProgramProperty GetPropertyForClinic(long clinicNum,string propDesc) {
			return _listProgramProperties.FindAll(x => x.ClinicNum==clinicNum)
					.FirstOrDefault(x => x.PropertyDesc==propDesc);
		}

		private void RelayoutForm() {
			//If we ever introduce more eRx options, this will need to change to just hiding the NewCrop option
			// (per Nathan in TaskNum 1108825 on 10/11/2017 at 4:16PM) 
			groupErxOptions.Visible=false;
			//The following modifications to tablControl and this form are intended to use the wasted space that hiding groupErxOptions created.
			tabControlErxSoftware.Location=new Point(12,36);
			tabControlErxSoftware.Size=new Size(tabControlErxSoftware.Size.Width,tabControlErxSoftware.Size.Height+groupErxOptions.Size.Height);
			this.Size=new Size(this.Size.Width,this.Size.Height-groupErxOptions.Size.Height);
		}

		private void SetRadioButtonChecked(bool isNewCrop) {
			if(isNewCrop) {
				tabControlErxSoftware.SelectedTab=tabNewCrop;
				_eRxOption=ErxOption.Legacy;
			}
			else {
				tabControlErxSoftware.SelectedTab=tabDoseSpot;
				_eRxOption=ErxOption.DoseSpot;
			}
		}

		private void checkShowHiddenClinics_CheckedChanged(object sender,EventArgs e) {
			FillGridDoseSpot();
		}

		private void radioNewCrop_Click(object sender,EventArgs e) {
			SetRadioButtonChecked(true);
		}

		private void radioDoseSpot_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This enables the DoseSpot program link only.  You must contact support to cancel current eRx Legacy charges and sign up for DoseSpot.");
			SetRadioButtonChecked(false);
		}

		private void gridProperties_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DoseSpotGridRowModel model=(DoseSpotGridRowModel)gridProperties.Rows[e.Row].Tag;
			FormDoseSpotPropertyEdit FormDPE=new FormDoseSpotPropertyEdit(model.Clinic,model.ClinicIDProperty.PropertyValue,model.ClinicKeyProperty.PropertyValue,_listProgramProperties);
			FormDPE.ShowDialog();
			if(FormDPE.DialogResult==DialogResult.OK) {
				int clinicIdPos=_listProgramProperties.IndexOf(GetPropertyForClinic(model.Clinic.ClinicNum,Erx.PropertyDescs.ClinicID));
				_listProgramProperties[clinicIdPos].PropertyValue=FormDPE.ClinicIdVal;
				int clinicKeyPos=_listProgramProperties.IndexOf(GetPropertyForClinic(model.Clinic.ClinicNum,Erx.PropertyDescs.ClinicKey));
				_listProgramProperties[clinicKeyPos].PropertyValue=FormDPE.ClinicKeyVal;
			}
			FillGridDoseSpot();//Always fill grid because clinics could have been editted in FormDoseSpotPropertyEdit.
		}

		private void butOK_Click(object sender,EventArgs e) {
			ErxOptionPP.PropertyValue=POut.Int((int)_eRxOption);
			_progCur.Enabled=checkEnabled.Checked;
			Programs.Update(_progCur);
			ProgramProperties.Sync(_listProgramProperties,_progCur.ProgramNum);
			DataValid.SetInvalid(InvalidType.Programs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private class DoseSpotGridRowModel {
			public Clinic Clinic;
			public ProgramProperty ClinicIDProperty;
			public ProgramProperty ClinicKeyProperty;

			public DoseSpotGridRowModel() {
			}
		}
	}
}
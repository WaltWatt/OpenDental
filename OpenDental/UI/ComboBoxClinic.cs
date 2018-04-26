using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Collections;

namespace OpenDental.UI {
	public partial class ComboBoxClinic:ComboBox {

		private const long CLINIC_NUM_ALL=-2;
		private const long CLINIC_NUM_UNASSIGNED=0;
		private bool _doIncludeAll;
		private bool _doIncludeUnassigned;
		private string _hqDescription;

		[Category("Behavior"), Description("The display value for ClinicNum 0."), DefaultValue("Unassigned")]
		public string HqDescription {
			get {
				return _hqDescription;
			}
			set {
				_hqDescription=value;
				FillClinics();
			}
		}

		///<summary>Getter returns -1 if no clinic is selected. Setter sets the SelectedIndex to -1 if the clinicNum has not been added to the 
		///combobox.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long SelectedClinicNum {
			get {			
				if(SelectedIndex==-1) {
					return -1;
				}	
				return ((ODBoxItem<Clinic>)SelectedItem).Tag.ClinicNum;
			}
			set {
				SelectedIndex=-1;
				for(int i = 0;i<Items.Count;i++) {
					if(value==((ODBoxItem<Clinic>)Items[i]).Tag.ClinicNum) {
						SelectedIndex=i;
					}
				}
			}
		}

		///<summary>Getter returns null if no clinic is selected. Setter sets the SelectedIndex to -1 if the clinic has not been added to the 
		///combobox.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Clinic SelectedClinic {
			get {
				if(SelectedIndex==-1) {
					return null;
				}
				return ((ODBoxItem<Clinic>)SelectedItem).Tag;
			}
			private set {
				SelectedIndex=-1;
				for(int i = 0;i<Items.Count;i++) {
					if(value.ClinicNum==((ODBoxItem<Clinic>)Items[i]).Tag.ClinicNum) {
						SelectedIndex=i;
					}
				}
			}
		}

		///<summary>Getter returns empty string if no clinic is selected. Setter sets the SelectedIndex to -1 if the clinic has not been added to the 
		///combobox.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedAbbr {
			get {
				if(SelectedIndex==-1) {
					return "";
				}
				return ((ODBoxItem<Clinic>)SelectedItem).Tag.Abbr;
			}
			private set {
				SelectedIndex=-1;
				for(int i = 0;i<Items.Count;i++) {
					if(value==((ODBoxItem<Clinic>)Items[i]).Tag.Abbr) {
						SelectedIndex=i;
					}
				}
			}
		}

		[Category("Behavior"), Description("Set to true to include 'All' as a selection option."), DefaultValue(false)]
		public bool DoIncludeAll {
			get {
				return _doIncludeAll;
			}
			set {
				_doIncludeAll=value;
				FillClinics();
			}
		}

		[Category("Behavior"), Description("Set to true to include 'Unassigned' as a selection option."), DefaultValue(false)]
		public bool DoIncludeUnassigned {
			get {
				return _doIncludeUnassigned;
			}
			set {
				_doIncludeUnassigned=value;
				FillClinics();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsAllSelected {
			get {
				return SelectedClinicNum==CLINIC_NUM_ALL;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsUnassignedSelected {
			get {
				return SelectedClinicNum==CLINIC_NUM_UNASSIGNED;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsNothingSelected {
			get {
				return SelectedIndex==-1;
			}
		}

		///<summary>Returns the clinic nums that are selected. Will be an empty list if nothing is selected. Will be a list of one clinic num unless
		///'All' is selected.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<long> ListSelectedClinicNums {
			get {
				if(SelectedIndex==-1) {
					return new List<long> { };
				}
				if(!IsAllSelected) {
					return new List<long> { SelectedClinicNum };
				}
				//'All' is selected
				List<long> listClinicNums=new List<long>();
				foreach(object item in Items) {
					if(((ODBoxItem<Clinic>)item).Tag.ClinicNum==CLINIC_NUM_ALL) {
						continue;
					}
					listClinicNums.Add(((ODBoxItem<Clinic>)item).Tag.ClinicNum);
				}
				return listClinicNums;
			}
		}

		public ComboBoxClinic() {
			InitializeComponent();
			Size=new Size(160,21);
			HqDescription="Unassigned";
			FillClinics();
		}

		private void FillClinics() {
			if(!Db.HasDatabaseConnection && !Security.IsUserLoggedIn) {
				return;
			}
			try {
				if(!PrefC.HasClinicsEnabled) {
					Visible=false;
					return;
				}
				Items.Clear();
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser,true,Lan.g("ComboBoxClinic",HqDescription));
				if(!DoIncludeUnassigned) {
					listClinics.RemoveAll(x => x.ClinicNum==CLINIC_NUM_UNASSIGNED);
				}
				//Always include the 'All' option regardless of how many clinics are in our list.
				//This is for the windows that treat 'All' as a way to include items that do not have a clinic set (e.g. Operatories windows).
				if(DoIncludeAll) {//&& listClinics.Count > 1
					Items.Add(new ODBoxItem<Clinic>(Lan.g("ComboBoxClinic","All"),new Clinic {
						Abbr=Lan.g("ComboBoxClinic","All"),
						Description=Lan.g("ComboBoxClinic","All"),
						ClinicNum=CLINIC_NUM_ALL
					}));
				}
				foreach(Clinic clinic in listClinics.OrderBy(x => x.Abbr)) {
					Items.Add(new ODBoxItem<Clinic>(clinic.Abbr,clinic));
				}
				if(Clinics.ClinicNum==0) {
					if(DoIncludeUnassigned) {
						SelectedClinicNum=CLINIC_NUM_UNASSIGNED;
					}
					else if(DoIncludeAll) {
						SelectedClinicNum=CLINIC_NUM_ALL;
					}
				}
				else {
					SelectedClinicNum=Clinics.ClinicNum;
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
		}
	}

}

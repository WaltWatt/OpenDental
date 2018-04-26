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
	public partial class ComboBoxClinicMulti:ComboBoxMulti {

		private const long CLINIC_NUM_ALL=-2;
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

		///<summary>Overriding this property so that it doesn't show in the designer.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override ArrayList Items {
			get {
				return base.Items;
			}
			set {
				base.Items=value;
			}
		}

		///<summary>Overriding this property so that it doesn't show in the designer.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override ArrayList SelectedIndices {
			get {
				return base.SelectedIndices;
			}
			set {
				base.SelectedIndices=value;
			}
		}

		///<summary>Overriding this property so that it doesn't show in the designer.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int[] ArraySelectedIndices {
			get {
				return base.ArraySelectedIndices;
			}
			set {
				base.ArraySelectedIndices=value;
			}
		}

		///<summary>Getter returns -1 if no clinic is selected. Setter sets the SelectedIndex to -1 if the clinicNum has not been added to the 
		///combobox.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<long> ListSelectedClinicNums {
			get {
				List<long> listSelectedClinicNums=ListSelectedIndices.Select(x => ((ODBoxItem<Clinic>)Items[x]).Tag.ClinicNum).ToList();
				if(listSelectedClinicNums.Contains(CLINIC_NUM_ALL)) {
					return Items.ToArray().ToList().Select(x => ((ODBoxItem<Clinic>)x).Tag.ClinicNum).Where(x => x!=CLINIC_NUM_ALL).ToList();
				}
				return listSelectedClinicNums;
			}
			private set {
				SelectedIndicesClear();
				ArrayList indices=new ArrayList();
				for(int i=0;i<Items.Count;i++) {
					if(value.Contains(((ODBoxItem<Clinic>)Items[i]).Tag.ClinicNum)) {
						SetSelected(i,true);
					}
				}
			}
		}
		
		///<summary>Returns the selected Clinics.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<Clinic> ListSelectedClinics {
			get {
				List<Clinic> listSelectedClinic=ListSelectedIndices.Select(x => ((ODBoxItem<Clinic>)Items[x]).Tag).ToList();
				if(listSelectedClinic.Any(x => x.ClinicNum==CLINIC_NUM_ALL)) {
					return Items.ToArray().OfType<ODBoxItem<Clinic>>().Select(x => x.Tag).Where(x => x.ClinicNum!=CLINIC_NUM_ALL).ToList();
				}
				return listSelectedClinic;
			}
		}
		
		///<summary>Returns true if 'All' is selected.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsAllSelected {
			get {
				return ListSelectedClinicNums.Contains(CLINIC_NUM_ALL);
			}
		}

		public ComboBoxClinicMulti() {
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
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted || listClinics.Count > 1) {
					Items.Add(new ODBoxItem<Clinic>(Lan.g("ComboBoxClinicMulti","All"),new Clinic { Abbr=Lan.g("ComboBoxClinicMulti","All"),
						Description=Lan.g("ComboBoxClinicMulti","All"),ClinicNum=CLINIC_NUM_ALL }));
				}
				if(!Security.CurUser.ClinicIsRestricted) {
					Items.Add(new ODBoxItem<Clinic>(Lan.g("ComboBoxClinicMulti",HqDescription),new Clinic { Abbr=Lan.g("ComboBoxClinicMulti",HqDescription),
						Description=Lan.g("ComboBoxClinicMulti",HqDescription),ClinicNum=0 }));
				}
				foreach(Clinic clinic in listClinics.OrderBy(x => x.Abbr)) {
					Items.Add(new ODBoxItem<Clinic>(clinic.Abbr,clinic));
				}
				if(Items.Count > 0) {
					ListSelectedClinicNums=new List<long> { Clinics.ClinicNum };
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
		}
		
	}

}

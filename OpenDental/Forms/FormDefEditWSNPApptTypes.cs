using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	public partial class FormDefEditWSNPApptTypes:ODForm {
		private Def _defCur;
		private AppointmentType _apptTypeCur;

		public bool IsDeleted {
			get;
			private set;
		}

		public FormDefEditWSNPApptTypes(Def defCur) {
			InitializeComponent();
			Lan.F(this);
			_defCur=defCur;
			checkHidden.Checked=_defCur.IsHidden;
			textName.Text=_defCur.ItemName;
			//Look for an associated appointment type.
			List<DefLink> listDefLinks=DefLinks.GetDefLinksByType(DefLinkType.AppointmentType);
			DefLink defLink=listDefLinks.FirstOrDefault(x => x.DefNum==_defCur.DefNum);
			if(defLink!=null) {
				_apptTypeCur=AppointmentTypes.GetFirstOrDefault(x => x.AppointmentTypeNum==defLink.FKey);
			}
			FillTextValue();
		}

		private void FillTextValue() {
			textValue.Clear();
			if(_apptTypeCur!=null) {
				textValue.Text=_apptTypeCur.AppointmentTypeName;
			}
		}

		private void butSelect_Click(object sender,EventArgs e) {
			FormApptTypes FormAT=new FormApptTypes();
			FormAT.IsSelectionMode=true;
			FormAT.SelectedAptType=_apptTypeCur;
			if(FormAT.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_apptTypeCur=FormAT.SelectedAptType;
			FillTextValue();
		}

		private void butColor_Click(object sender,EventArgs e) {
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(string.IsNullOrEmpty(textName.Text.Trim())) {
				MsgBox.Show(this,"Reason required.");
				return;
			}
			if(_apptTypeCur==null) {
				MsgBox.Show(this,"Appointment Type required.");
				return;
			}
			_defCur.ItemName=PIn.String(textName.Text);
			if(_defCur.IsNew) {
				Defs.Insert(_defCur);
			}
			else {
				Defs.Update(_defCur);
			}
			DefLinks.SetFKeyForDef(_defCur.DefNum,_apptTypeCur.AppointmentTypeNum,DefLinkType.AppointmentType);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			try {
				Defs.Delete(_defCur);
				//Web Sched New Pat Appt appointment type defs are associated to appointment type and operatory deflinks.  Clean them up.
				DefLinks.DeleteAllForDef(_defCur.DefNum,DefLinkType.AppointmentType);
				DefLinks.DeleteAllForDef(_defCur.DefNum,DefLinkType.Operatory);
				IsDeleted=true;
				DialogResult=DialogResult.OK;
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
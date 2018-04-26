using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using OpenDental.UI;
using System.Drawing;

namespace OpenDental {
	public partial class FormDiscountPlanEdit:ODForm {
		public DiscountPlan DiscountPlanCur;
		///<summary>FeeSched for the current DiscountPlan.  May be null if the DiscountPlan is new.</summary>
		private FeeSched _feeSchedCur;
		private List<Def> _listAdjTypeDefs;
		private List<string> _listPatNames;
		///<summary>IsSelectionMode is true if this window is opened with the intent of selecting a plan for a user</summary>
		public bool IsSelectionMode;

		public FormDiscountPlanEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormDiscountPlanEdit_Load(object sender,EventArgs e) {
			textDescript.Text=DiscountPlanCur.Description;
			_listPatNames=DiscountPlans.GetPatsForPlan(DiscountPlanCur.DiscountPlanNum)
				.Select(x => x.LName+", "+x.FName)
				.Distinct()
				.OrderBy(x => x)
				.ToList();
			_feeSchedCur=FeeScheds.GetFirstOrDefault(x => x.FeeSchedNum==DiscountPlanCur.FeeSchedNum,true);
			textFeeSched.Text=_feeSchedCur!=null ? _feeSchedCur.Description : "";
			_listAdjTypeDefs=Defs.GetDiscountPlanAdjTypes().ToList();
			for(int i=0;i<_listAdjTypeDefs.Count;i++) {
				comboBoxAdjType.Items.Add(_listAdjTypeDefs[i].ItemName);
				if(_listAdjTypeDefs[i].DefNum==DiscountPlanCur.DefNum) {
					comboBoxAdjType.SelectedIndex=i;
				}
			}
			//populate patient information
			int countPats=_listPatNames.Count;
			textNumPatients.Text=countPats.ToString();
			if(countPats>10000) {//10,000 per Nathan. copied from FormInsPlan.cs
				comboPatient.Visible=false;
				butListPatients.Visible=true;
				butListPatients.Location=comboPatient.Location;
			}
			else {
				comboPatient.Visible=true;
				butListPatients.Visible=false;
				comboPatient.Items.Clear();
				comboPatient.Items.AddRange(_listPatNames.ToArray());
				if(_listPatNames.Count>0) {
					comboPatient.SelectedIndex=0;
				}
			}
			checkHidden.Checked=DiscountPlanCur.IsHidden;
			if(!Security.IsAuthorized(Permissions.InsPlanEdit,true)) {//User may be able to get here if FormDiscountPlans is not in selection mode.
				textDescript.ReadOnly=true;
				comboBoxAdjType.Enabled=false;
				butFeeSched.Enabled=false;
				butOK.Enabled=false;
				checkHidden.Enabled=false;
			}
			if(IsSelectionMode) {
				butDrop.Visible=true;
			}
		}

		private void butFeeSched_Click(object sender,EventArgs e) {
			//No need to check security because we are launching the form in selection mode.
			FormFeeScheds FormFS=new FormFeeScheds(true);
			FormFS.SelectedFeeSchedNum=(_feeSchedCur==null ? 0 : _feeSchedCur.FeeSchedNum);
			if(FormFS.ShowDialog()==DialogResult.OK) {
				_feeSchedCur=FeeScheds.GetFirst(x => x.FeeSchedNum==FormFS.SelectedFeeSchedNum);
				textFeeSched.Text=_feeSchedCur.Description;
			}
		}
		
		private void butDrop_Click(object sender,EventArgs e) {
			DiscountPlans.DropForPatient(FormOpenDental.CurPatNum);
			string logText="The discount plan "+DiscountPlanCur.Description+" was dropped.";
			SecurityLogs.MakeLogEntry(Permissions.DiscountPlanAddDrop,FormOpenDental.CurPatNum,logText);
			DialogResult=DialogResult.OK;
		}

		private void checkHidden_Click(object sender,EventArgs e) {
			if(checkHidden.Checked) {
				List<Patient> listPatsForPlan=DiscountPlans.GetPatsForPlan(DiscountPlanCur.DiscountPlanNum);
				if(listPatsForPlan.Count!=0) {
					string msgText=Lan.g(this,"Specified Discount Plan will be hidden.  "+
						"It will no longer be available for assigning, but existing patients on plan will remain");
					if(MessageBox.Show(this,msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						checkHidden.Checked=false;
					}
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDescript.Text.Trim()=="") {
				MsgBox.Show(this,"Please enter a description.");
				return;
			}
			if(_feeSchedCur==null) {
				MsgBox.Show(this,"Please select a fee schedule.");
				return;
			}
			if(comboBoxAdjType.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select an adjustment type.\r\nYou may need to create discount plan adjustment types within definition setup.");
				return;
			}
			DiscountPlanCur.Description=textDescript.Text;
			DiscountPlanCur.FeeSchedNum=_feeSchedCur.FeeSchedNum;
			DiscountPlanCur.DefNum=_listAdjTypeDefs[comboBoxAdjType.SelectedIndex].DefNum;
			DiscountPlanCur.IsHidden=checkHidden.Checked;
			if(DiscountPlanCur.IsNew) {
				DiscountPlans.Insert(DiscountPlanCur);
			}
			else {
				DiscountPlans.Update(DiscountPlanCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butListPatients_Click(object sender,EventArgs e) {
			ODForm form=new ODForm() {
				Size=new Size(500,400),
				Text="Other Patients List",
				FormBorderStyle=FormBorderStyle.FixedSingle
			};
			ODGrid grid=new ODGrid() {
				Size=new Size(475,300),
				Location=new Point(5,5),
				Title="Patients",
				TranslationName=""
			};
			UI.Button butClose=new UI.Button() {
				Size=new Size(75,23),
				Text="Close",
				Location=new Point(form.ClientSize.Width-80,form.ClientSize.Height-28),//Subtract the button's size plus 5 pixel buffer.
			};
			butClose.Click+=(s,ex) => form.Close();//When butClose is pressed, simply close the form.  If more functionality is needed, make a method below.
			form.Controls.Add(grid);
			form.Controls.Add(butClose);
			grid.BeginUpdate();
			grid.Columns.Clear();
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Name"),0));
			grid.Rows.Clear();
			foreach(string patName in _listPatNames) {
				grid.Rows.Add(new ODGridRow(patName));
			}
			grid.EndUpdate();
			form.ShowDialog();
		}
	}
}
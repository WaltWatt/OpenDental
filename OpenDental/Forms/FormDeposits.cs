using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormDeposits : ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid grid;
		private Deposit[] DList;
		private OpenDental.UI.Button butOK;
		///<summary>Use this from Transaction screen when attaching a source document.</summary>
		public bool IsSelectionMode;
		private Label labelClinic;
		private ComboBoxMulti comboClinic;
		///<summary>List of Clinics the user has access to.</summary>
		private List<Clinic> _listClinics=new List<Clinic>();
		///<summary>In selection mode, when closing form with OK, this contains selected deposit.</summary>
		public Deposit SelectedDeposit;

		///<summary></summary>
		public FormDeposits()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeposits));
			this.butClose = new OpenDental.UI.Button();
			this.grid = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboClinic = new OpenDental.UI.ComboBoxMulti();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(365, 574);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.HasAddButton = false;
			this.grid.HasMultilineHeaders = false;
			this.grid.HeaderHeight = 15;
			this.grid.HScrollVisible = false;
			this.grid.Location = new System.Drawing.Point(18, 34);
			this.grid.Name = "grid";
			this.grid.ScrollValue = 0;
			this.grid.Size = new System.Drawing.Size(339, 567);
			this.grid.TabIndex = 1;
			this.grid.Title = "Deposit Slips";
			this.grid.TitleHeight = 18;
			this.grid.TranslationName = "TableDepositSlips";
			this.grid.WrapText = false;
			this.grid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(363, 293);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(77, 26);
			this.butAdd.TabIndex = 0;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(365, 542);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(26, 6);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(58, 21);
			this.labelClinic.TabIndex = 129;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.ArraySelectedIndices = new int[0];
			this.comboClinic.BackColor = System.Drawing.SystemColors.Window;
			this.comboClinic.Items = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.Items")));
			this.comboClinic.Location = new System.Drawing.Point(90, 6);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.SelectedIndices")));
			this.comboClinic.Size = new System.Drawing.Size(166, 24);
			this.comboClinic.TabIndex = 130;
			this.comboClinic.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// FormDeposits
			// 
			this.AcceptButton = this.butOK;
			this.ClientSize = new System.Drawing.Size(454, 613);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(370, 250);
			this.Name = "FormDeposits";
			this.ShowInTaskbar = false;
			this.Text = "Deposit Slips";
			this.Load += new System.EventHandler(this.FormDeposits_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDeposits_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode){
				butAdd.Visible=false;
			}
			else{
				butOK.Visible=false;
			}
			if(PrefC.HasClinicsEnabled) {//clinics
				List <int> listSelectedItems=new List<int>();
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted) {
					comboClinic.Items.Add(Lan.g(this,"All"));
					listSelectedItems.Add(0);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=comboClinic.Items.Add(_listClinics[i].Abbr);
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						listSelectedItems.Clear();
						listSelectedItems.Add(curIndex);
					}
				}
				//We set the selections after when using ComboBoxMulti
				foreach(int index in listSelectedItems) {
					comboClinic.SetSelected(index,true);
				}
			}
			else {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
			}
			FillGrid();
		}

		private void FillGrid(){
			if(!PrefC.HasClinicsEnabled) {
				if(IsSelectionMode) {
					DList=Deposits.GetUnattached();
				}
				else {
					DList=Deposits.Refresh();
				}
			}
			else {
				List<long> listSelectedClinicNums=new List<long>();
				if(!Security.CurUser.ClinicIsRestricted && comboClinic.SelectedIndices.Contains(0)) {//All is an option in comboClinic and is selected.
					//Blank by design for Deposits.GetForClinics(...).
				}
				else {
					int listStep=(Security.CurUser.ClinicIsRestricted?0:1);//If the All option is available, then all indices are offset by 1.
					comboClinic.ListSelectedIndices.ForEach(x => listSelectedClinicNums.Add(_listClinics[x-listStep].ClinicNum));
				}
				DList=Deposits.GetForClinics(listSelectedClinicNums,IsSelectionMode);
			}
			grid.BeginUpdate();
			grid.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableDepositSlips","Date"),80);
			grid.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlips","Amount"),90,HorizontalAlignment.Right);
			grid.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g("TableDepositSlips","Clinic"),150);
				grid.Columns.Add(col);
			}
			grid.Rows.Clear();
			OpenDental.UI.ODGridRow row;
			for(int i=0;i<DList.Length;i++){
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(DList[i].DateDeposit.ToShortDateString());
				row.Cells.Add(DList[i].Amount.ToString("F"));
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(" "+DList[i].ClinicAbbr);//padding left with space to add separation between amount and clinic abbr
				}
				grid.Rows.Add(row);
			}
			grid.EndUpdate();
			grid.ScrollToEnd();
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(!Security.CurUser.ClinicIsRestricted //All is in comboClinic
				&& comboClinic.SelectedIndices.Contains(0)//All is selected
				&& comboClinic.SelectedIndices.Count>1)//Other clinics are also selected
			{
				comboClinic.SelectedIndicesClear();//Force selection to just All
				comboClinic.SetSelected(0,true);
			}
			FillGrid();
		}

		private void grid_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(IsSelectionMode){
				SelectedDeposit=DList[e.Row];
				DialogResult=DialogResult.OK;
				return;
			}
			//not selection mode.
			FormDepositEdit FormD=new FormDepositEdit(DList[e.Row]);
			FormD.ShowDialog();
			if(FormD.DialogResult==DialogResult.Cancel){
				return;
			}
			FillGrid();
		}

		///<summary>Not available in selection mode.</summary>
		private void butAdd_Click(object sender, System.EventArgs e) {
			Deposit deposit=new Deposit();
			deposit.DateDeposit=DateTime.Today;
			deposit.BankAccountInfo=PrefC.GetString(PrefName.PracticeBankNumber);
			FormDepositEdit FormD=new FormDepositEdit(deposit);
			FormD.IsNew=true;
			FormD.ShowDialog();
			if(FormD.DialogResult==DialogResult.Cancel){
				return;
			}
			FillGrid();
		}

		///<summary>Only available in selection mode.</summary>
		private void butOK_Click(object sender,EventArgs e) {
			if(grid.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a deposit first.");
				return;
			}
			SelectedDeposit=DList[grid.GetSelectedIndex()];
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		

		


	}
}






















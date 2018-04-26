using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormApptViews : ODForm {
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.ListBox listViews;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioTen;
		private System.Windows.Forms.RadioButton radioFifteen;
		private System.Windows.Forms.CheckBox checkTwoRows;
		private OpenDental.UI.Button butCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private RadioButton radioFive;
		private OpenDental.UI.Button butProcColors;
		private ComboBox comboClinic;
		private Label labelClinic;
		private bool viewChanged;
		private long _clinicNum;
		private List<Clinic> _listClinics;
		private List<ApptView> _listApptViews;

		///<summary></summary>
		public FormApptViews()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptViews));
			this.butCancel = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.listViews = new System.Windows.Forms.ListBox();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioFive = new System.Windows.Forms.RadioButton();
			this.radioFifteen = new System.Windows.Forms.RadioButton();
			this.radioTen = new System.Windows.Forms.RadioButton();
			this.checkTwoRows = new System.Windows.Forms.CheckBox();
			this.butProcColors = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(447, 433);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(57, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(158, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "Views";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// listViews
			// 
			this.listViews.Location = new System.Drawing.Point(56, 60);
			this.listViews.Name = "listViews";
			this.listViews.Size = new System.Drawing.Size(183, 329);
			this.listViews.TabIndex = 2;
			this.listViews.DoubleClick += new System.EventHandler(this.listViews_DoubleClick);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(151, 437);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(89, 24);
			this.butDown.TabIndex = 38;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(151, 399);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(89, 24);
			this.butUp.TabIndex = 39;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(55, 399);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(89, 24);
			this.butAdd.TabIndex = 36;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioFive);
			this.groupBox1.Controls.Add(this.radioFifteen);
			this.groupBox1.Controls.Add(this.radioTen);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(279, 54);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(169, 82);
			this.groupBox1.TabIndex = 40;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Time Increments";
			// 
			// radioFive
			// 
			this.radioFive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioFive.Location = new System.Drawing.Point(23, 19);
			this.radioFive.Name = "radioFive";
			this.radioFive.Size = new System.Drawing.Size(100, 18);
			this.radioFive.TabIndex = 2;
			this.radioFive.Text = "5 Min";
			// 
			// radioFifteen
			// 
			this.radioFifteen.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioFifteen.Location = new System.Drawing.Point(23, 57);
			this.radioFifteen.Name = "radioFifteen";
			this.radioFifteen.Size = new System.Drawing.Size(100, 18);
			this.radioFifteen.TabIndex = 1;
			this.radioFifteen.Text = "15 Min";
			// 
			// radioTen
			// 
			this.radioTen.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioTen.Location = new System.Drawing.Point(23, 38);
			this.radioTen.Name = "radioTen";
			this.radioTen.Size = new System.Drawing.Size(100, 18);
			this.radioTen.TabIndex = 0;
			this.radioTen.Text = "10 Min";
			// 
			// checkTwoRows
			// 
			this.checkTwoRows.Location = new System.Drawing.Point(0, 0);
			this.checkTwoRows.Name = "checkTwoRows";
			this.checkTwoRows.Size = new System.Drawing.Size(104, 24);
			this.checkTwoRows.TabIndex = 0;
			// 
			// butProcColors
			// 
			this.butProcColors.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProcColors.Autosize = true;
			this.butProcColors.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProcColors.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProcColors.CornerRadius = 4F;
			this.butProcColors.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butProcColors.Location = new System.Drawing.Point(279, 159);
			this.butProcColors.Name = "butProcColors";
			this.butProcColors.Size = new System.Drawing.Size(82, 24);
			this.butProcColors.TabIndex = 41;
			this.butProcColors.Text = "Proc Colors";
			this.butProcColors.Click += new System.EventHandler(this.butProcColors_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(88, 12);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(152, 21);
			this.comboClinic.TabIndex = 135;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(1, 12);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(87, 17);
			this.labelClinic.TabIndex = 134;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// FormApptViews
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(546, 485);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.butProcColors);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.listViews);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormApptViews";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Appointment Views";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptViews_FormClosing);
			this.Load += new System.EventHandler(this.FormApptViews_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormApptViews_Load(object sender, System.EventArgs e) {
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			else {
				_clinicNum=Clinics.ClinicNum;
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				comboClinic.Items.Clear();
				if(!Security.CurUser.ClinicIsRestricted) {
					comboClinic.Items.Add(Lan.g(this,"Headquarters"));//in this form, the Headquarters list is the list of views that are not assigned to a clinic
					comboClinic.SelectedIndex=0;
				}
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Abbr);
					if(_clinicNum==_listClinics[i].ClinicNum) {
						comboClinic.SelectedIndex=Security.CurUser.ClinicIsRestricted?i:i+1;
					}
				}
			}
			FillViewList();
			if(PrefC.GetInt(PrefName.AppointmentTimeIncrement)==5){
				radioFive.Checked=true;
			}
			else if(PrefC.GetInt(PrefName.AppointmentTimeIncrement)==10) {
				radioTen.Checked=true;
			}
			else{
				radioFifteen.Checked=true;
			}
		}

		private void FillViewList(){
			Cache.Refresh(InvalidType.Views);
			listViews.Items.Clear();
			_listApptViews=new List<ApptView>();
			List<ApptView> listApptViewsTemp=ApptViews.GetDeepCopy();
			string F;
			for(int i=0;i<listApptViewsTemp.Count;i++){
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && _clinicNum!=listApptViewsTemp[i].ClinicNum) {
					continue;//only add views assigned to the clinic selected
				}
				if(listViews.Items.Count<12)
					F="F"+(listViews.Items.Count+1).ToString()+"-";
				else
					F="";
				listViews.Items.Add(F+listApptViewsTemp[i].Description);
				_listApptViews.Add(listApptViewsTemp[i]);
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(Security.CurUser.ClinicIsRestricted) {
				_clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			else {
				if(comboClinic.SelectedIndex==0) {
					_clinicNum=0;
				}
				else {
					_clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
				}
			}
			FillViewList();
		}
		
		private void butAdd_Click(object sender, System.EventArgs e) {
			ApptView ApptViewCur=new ApptView();
			if(_listApptViews.Count==0) {
				ApptViewCur.ItemOrder=0;
			}
			else {
				ApptViewCur.ItemOrder=_listApptViews[_listApptViews.Count-1].ItemOrder+1;
			}
			ApptViewCur.ApptTimeScrollStart=DateTime.Parse("08:00:00").TimeOfDay;//default to 8 AM
			ApptViews.Insert(ApptViewCur);//this also gets the primary key
			FormApptViewEdit FormAVE=new FormApptViewEdit();
			FormAVE.ApptViewCur=ApptViewCur;
			FormAVE.IsNew=true;
			FormAVE.ClinicNum=_clinicNum;
			FormAVE.ShowDialog();
			if(FormAVE.DialogResult!=DialogResult.OK){
				return;
			}
			viewChanged=true;
			FillViewList();
			listViews.SelectedIndex=listViews.Items.Count-1;//this works even if no items
		}

		private void listViews_DoubleClick(object sender, System.EventArgs e) {
			if(listViews.SelectedIndex==-1){
				return;
			}
			int selected=listViews.SelectedIndex;
			ApptView ApptViewCur=_listApptViews[listViews.SelectedIndex];
			FormApptViewEdit FormAVE=new FormApptViewEdit();
			FormAVE.ApptViewCur=ApptViewCur;
			FormAVE.ClinicNum=_clinicNum;
			FormAVE.ShowDialog();
			if(FormAVE.DialogResult!=DialogResult.OK){
				return;
			}
			viewChanged=true;
			FillViewList();
			if(selected<listViews.Items.Count) {
				listViews.SelectedIndex=selected;
			}
			else {
				listViews.SelectedIndex=-1;
			}
		}

		private void butUp_Click(object sender, System.EventArgs e) {
			if(listViews.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select a category first."));
				return;
			}
			if(listViews.SelectedIndex==0){
				return;//can't go up any more
			}
			//it will flip flop with the one above it
			ApptView ApptViewCur=_listApptViews[listViews.SelectedIndex-1];
			ApptViewCur.ItemOrder=listViews.SelectedIndex;
			ApptViews.Update(ApptViewCur);
			//now the other
			ApptViewCur=_listApptViews[listViews.SelectedIndex];
			ApptViewCur.ItemOrder=listViews.SelectedIndex-1;
			ApptViews.Update(ApptViewCur);
			viewChanged=true;
			FillViewList();
			listViews.SelectedIndex=_listApptViews.FindIndex(x => x.ApptViewNum==ApptViewCur.ApptViewNum);
		}

		private void butDown_Click(object sender, System.EventArgs e) {
			if(listViews.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select a category first."));
				return;
			}
			if(listViews.SelectedIndex==listViews.Items.Count-1){
				return;//can't go down any more
			}
			//it will flip flop with the one below it
			ApptView ApptViewCur=_listApptViews[listViews.SelectedIndex+1];
			ApptViewCur.ItemOrder=listViews.SelectedIndex;
			ApptViews.Update(ApptViewCur);
			//now the other
			ApptViewCur=_listApptViews[listViews.SelectedIndex];
			ApptViewCur.ItemOrder=listViews.SelectedIndex+1;
			ApptViews.Update(ApptViewCur);
			viewChanged=true;
			FillViewList();
			listViews.SelectedIndex=_listApptViews.FindIndex(x => x.ApptViewNum==ApptViewCur.ApptViewNum);
		}

		private void butProcColors_Click(object sender,EventArgs e) {
			FormProcApptColors formProcColors=new FormProcApptColors();
			formProcColors.ShowDialog();
			DialogResult=DialogResult.None;//This is required to prevent FormApptViews from closing.
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormApptViews_FormClosing(object sender,FormClosingEventArgs e) {
			int newIncrement=15;
			if(radioFive.Checked) {
				newIncrement=5;
			}
			if(radioTen.Checked) {
				newIncrement=10;
			}
			if(Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,newIncrement)){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			if(viewChanged){
				DataValid.SetInvalid(InvalidType.Views);
			}
		}

		


	

		


	}
}






















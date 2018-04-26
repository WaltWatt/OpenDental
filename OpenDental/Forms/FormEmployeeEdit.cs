using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormEmployeeEdit : ODForm {
		private System.Windows.Forms.TextBox textLName;
		private System.Windows.Forms.TextBox textFName;
		private System.Windows.Forms.TextBox textMI;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkIsHidden;
		///<summary></summary>
		public bool IsNew;
		private OpenDental.UI.Button butDelete;
		private TextBox textPhoneExt;
		private Label labelPhoneExt;
		private TextBox textPayrollID;
		private Label label1;
		public Employee EmployeeCur;

		///<summary></summary>
		public FormEmployeeEdit(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmployeeEdit));
			this.textLName = new System.Windows.Forms.TextBox();
			this.textFName = new System.Windows.Forms.TextBox();
			this.textMI = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.textPhoneExt = new System.Windows.Forms.TextBox();
			this.labelPhoneExt = new System.Windows.Forms.Label();
			this.textPayrollID = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textLName
			// 
			this.textLName.Location = new System.Drawing.Point(103, 58);
			this.textLName.MaxLength = 100;
			this.textLName.Name = "textLName";
			this.textLName.Size = new System.Drawing.Size(174, 20);
			this.textLName.TabIndex = 23;
			// 
			// textFName
			// 
			this.textFName.Location = new System.Drawing.Point(103, 88);
			this.textFName.MaxLength = 100;
			this.textFName.Name = "textFName";
			this.textFName.Size = new System.Drawing.Size(174, 20);
			this.textFName.TabIndex = 24;
			// 
			// textMI
			// 
			this.textMI.Location = new System.Drawing.Point(103, 118);
			this.textMI.MaxLength = 100;
			this.textMI.Name = "textMI";
			this.textMI.Size = new System.Drawing.Size(88, 20);
			this.textMI.TabIndex = 25;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(29, 62);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(70, 14);
			this.label10.TabIndex = 31;
			this.label10.Text = "Last Name";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(29, 92);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(70, 14);
			this.label8.TabIndex = 29;
			this.label8.Text = "First Name";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(25, 122);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 14);
			this.label7.TabIndex = 28;
			this.label7.Text = "MI";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHidden.Location = new System.Drawing.Point(103, 32);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(70, 18);
			this.checkIsHidden.TabIndex = 32;
			this.checkIsHidden.Text = "Hidden";
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
			this.butCancel.Location = new System.Drawing.Point(406, 223);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 35;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(406, 191);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 34;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(45, 223);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(81, 25);
			this.butDelete.TabIndex = 36;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textPhoneExt
			// 
			this.textPhoneExt.Location = new System.Drawing.Point(103, 178);
			this.textPhoneExt.MaxLength = 100;
			this.textPhoneExt.Name = "textPhoneExt";
			this.textPhoneExt.Size = new System.Drawing.Size(70, 20);
			this.textPhoneExt.TabIndex = 39;
			// 
			// labelPhoneExt
			// 
			this.labelPhoneExt.Location = new System.Drawing.Point(29, 182);
			this.labelPhoneExt.Name = "labelPhoneExt";
			this.labelPhoneExt.Size = new System.Drawing.Size(70, 14);
			this.labelPhoneExt.TabIndex = 40;
			this.labelPhoneExt.Text = "Phone Ext.";
			this.labelPhoneExt.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPayrollID
			// 
			this.textPayrollID.Location = new System.Drawing.Point(103, 148);
			this.textPayrollID.MaxLength = 100;
			this.textPayrollID.Name = "textPayrollID";
			this.textPayrollID.Size = new System.Drawing.Size(88, 20);
			this.textPayrollID.TabIndex = 41;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(29, 152);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 14);
			this.label1.TabIndex = 42;
			this.label1.Text = "Payroll ID";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormEmployeeEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(500, 265);
			this.Controls.Add(this.textPayrollID);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textPhoneExt);
			this.Controls.Add(this.labelPhoneExt);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.checkIsHidden);
			this.Controls.Add(this.textLName);
			this.Controls.Add(this.textFName);
			this.Controls.Add(this.textMI);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEmployeeEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Employee Edit";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormEmployeeEdit_Closing);
			this.Load += new System.EventHandler(this.FormEmployeeEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEmployeeEdit_Load(object sender, System.EventArgs e) {
			textLName.Text=EmployeeCur.LName;
			textFName.Text=EmployeeCur.FName;
			textMI.Text=EmployeeCur.MiddleI;
			textPayrollID.Text=EmployeeCur.PayrollID;
			if(!PrefC.GetBool(PrefName.DockPhonePanelShow)){
				labelPhoneExt.Visible=false;
				textPhoneExt.Visible=false;
			}
			textPhoneExt.Text=EmployeeCur.PhoneExt.ToString();
			checkIsHidden.Checked=EmployeeCur.IsHidden;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			//not new:
			try{
				Employees.Delete(EmployeeCur.EmployeeNum);
				DialogResult=DialogResult.OK;
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			EmployeeCur.LName=textLName.Text;
			EmployeeCur.FName=textFName.Text;
			EmployeeCur.MiddleI=textMI.Text;
			EmployeeCur.PayrollID=textPayrollID.Text;
			try{
				EmployeeCur.PhoneExt=PIn.Int(textPhoneExt.Text);
			}
			catch{
				EmployeeCur.PhoneExt=0;
			}
			EmployeeCur.IsHidden=checkIsHidden.Checked;
			try {
				if(IsNew) {
					Employees.Insert(EmployeeCur);
				}
				else {
					Employees.Update(EmployeeCur);
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormEmployeeEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			
		}

		

		
		
	}

	
	
}


























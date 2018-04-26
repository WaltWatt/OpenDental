using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormApptLists : ODForm {
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private OpenDental.UI.Button butRecall;
		private OpenDental.UI.Button butConfirm;
		private OpenDental.UI.Button butPlanned;
		private OpenDental.UI.Button butUnsched;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butASAP;
		private Label label4;
		private UI.Button butRadOrders;
		private Label label7;
        private UI.Button butInsVerify;
        private Label label8;

        ///<summary>After this window closes, if dialog result is OK, this will contain which list was selected.</summary>
        public ApptListSelection SelectionResult;

		///<summary></summary>
		public FormApptLists()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptLists));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.butASAP = new OpenDental.UI.Button();
			this.butUnsched = new OpenDental.UI.Button();
			this.butPlanned = new OpenDental.UI.Button();
			this.butConfirm = new OpenDental.UI.Button();
			this.butRecall = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butRadOrders = new OpenDental.UI.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.butInsVerify = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(130, 67);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(479, 34);
			this.label1.TabIndex = 2;
			this.label1.Text = "A list of due dates for patients who have previously been in for an exam or clean" +
    "ing";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(130, 118);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(479, 44);
			this.label2.TabIndex = 3;
			this.label2.Text = "A list of scheduled appointments.  These patients need to be reminded about their" +
    " appointments.";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(130, 172);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(479, 47);
			this.label3.TabIndex = 4;
			this.label3.Text = "Planned appointments are created in the Chart module.  Every patient should have " +
    "one or be marked done.  This list is work that has been planned but never schedu" +
    "led.";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(130, 226);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(479, 47);
			this.label5.TabIndex = 6;
			this.label5.Text = "A short list of appointments that need to be followed up on.   Keep this list sho" +
    "rt by deleting any that don\'t get scheduled quickly.  You would then track them " +
    " using the Planned Appointment Tracker";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(27, 19);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(519, 31);
			this.label6.TabIndex = 7;
			this.label6.Text = "These lists may be used for calling patients, sending postcards, running reports," +
    " etc..  Make sure to make good Comm Log entries for everything.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(130, 280);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(479, 54);
			this.label4.TabIndex = 12;
			this.label4.Text = "The short call list. A list of appointments and recalls where the patient is avai" +
    "lable on short notice. To show this list, the \"ASAP\" checkbox must be checked on" +
    " the appointment or the recall";
			// 
			// butASAP
			// 
			this.butASAP.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butASAP.Autosize = true;
			this.butASAP.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butASAP.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butASAP.CornerRadius = 4F;
			this.butASAP.Location = new System.Drawing.Point(28, 278);
			this.butASAP.Name = "butASAP";
			this.butASAP.Size = new System.Drawing.Size(100, 26);
			this.butASAP.TabIndex = 13;
			this.butASAP.Text = "ASAP";
			this.butASAP.Click += new System.EventHandler(this.butASAP_Click);
			// 
			// butUnsched
			// 
			this.butUnsched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnsched.Autosize = true;
			this.butUnsched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnsched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnsched.CornerRadius = 4F;
			this.butUnsched.Location = new System.Drawing.Point(28, 224);
			this.butUnsched.Name = "butUnsched";
			this.butUnsched.Size = new System.Drawing.Size(100, 26);
			this.butUnsched.TabIndex = 11;
			this.butUnsched.Text = "Unscheduled";
			this.butUnsched.Click += new System.EventHandler(this.butUnsched_Click);
			// 
			// butPlanned
			// 
			this.butPlanned.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPlanned.Autosize = true;
			this.butPlanned.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPlanned.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPlanned.CornerRadius = 4F;
			this.butPlanned.Location = new System.Drawing.Point(28, 170);
			this.butPlanned.Name = "butPlanned";
			this.butPlanned.Size = new System.Drawing.Size(100, 26);
			this.butPlanned.TabIndex = 10;
			this.butPlanned.Text = "Planned Tracker";
			this.butPlanned.Click += new System.EventHandler(this.butPlanned_Click);
			// 
			// butConfirm
			// 
			this.butConfirm.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butConfirm.Autosize = true;
			this.butConfirm.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butConfirm.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butConfirm.CornerRadius = 4F;
			this.butConfirm.Location = new System.Drawing.Point(28, 116);
			this.butConfirm.Name = "butConfirm";
			this.butConfirm.Size = new System.Drawing.Size(100, 26);
			this.butConfirm.TabIndex = 9;
			this.butConfirm.Text = "Confirmations";
			this.butConfirm.Click += new System.EventHandler(this.butConfirm_Click);
			// 
			// butRecall
			// 
			this.butRecall.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecall.Autosize = true;
			this.butRecall.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecall.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecall.CornerRadius = 4F;
			this.butRecall.Location = new System.Drawing.Point(28, 62);
			this.butRecall.Name = "butRecall";
			this.butRecall.Size = new System.Drawing.Size(100, 26);
			this.butRecall.TabIndex = 8;
			this.butRecall.Text = "Recall";
			this.butRecall.Click += new System.EventHandler(this.butRecall_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(579, 456);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butRadOrders
			// 
			this.butRadOrders.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRadOrders.Autosize = true;
			this.butRadOrders.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRadOrders.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRadOrders.CornerRadius = 4F;
			this.butRadOrders.Location = new System.Drawing.Point(28, 332);
			this.butRadOrders.Name = "butRadOrders";
			this.butRadOrders.Size = new System.Drawing.Size(100, 26);
			this.butRadOrders.TabIndex = 15;
			this.butRadOrders.Text = "Radiology";
			this.butRadOrders.Click += new System.EventHandler(this.butRadOrders_Click);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(130, 334);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(479, 47);
			this.label7.TabIndex = 14;
			this.label7.Text = "A list of radiology orders associated to appointments that have not been marked a" +
    "s CPOE.  This list allows providers to quickly approve radiology orders which wi" +
    "ll help meet EHR measures.";
			// 
			// butInsVerify
			// 
			this.butInsVerify.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsVerify.Autosize = true;
			this.butInsVerify.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsVerify.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsVerify.CornerRadius = 4F;
			this.butInsVerify.Location = new System.Drawing.Point(28, 386);
			this.butInsVerify.Name = "butInsVerify";
			this.butInsVerify.Size = new System.Drawing.Size(100, 26);
			this.butInsVerify.TabIndex = 17;
			this.butInsVerify.Text = "Ins Verify";
			this.butInsVerify.Click += new System.EventHandler(this.butInsVerify_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(130, 388);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(479, 47);
			this.label8.TabIndex = 16;
			this.label8.Text = "A list of insurance verifications for upcoming appointments.";
			// 
			// FormApptLists
			// 
			this.ClientSize = new System.Drawing.Size(666, 494);
			this.Controls.Add(this.butInsVerify);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.butRadOrders);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.butASAP);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butUnsched);
			this.Controls.Add(this.butPlanned);
			this.Controls.Add(this.butConfirm);
			this.Controls.Add(this.butRecall);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormApptLists";
			this.ShowInTaskbar = false;
			this.Text = "Appointment Lists";
			this.Load += new System.EventHandler(this.FormApptLists_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormApptLists_Load(object sender, System.EventArgs e) {
			Plugins.HookAddCode(this,"FormApptLists.Load_start");
		}

		private void butRecall_Click(object sender, System.EventArgs e) {
			SelectionResult=ApptListSelection.Recall;
			DialogResult=DialogResult.OK;
		}

		private void butConfirm_Click(object sender, System.EventArgs e) {
			SelectionResult=ApptListSelection.Confirm;
			DialogResult=DialogResult.OK;
		}

		private void butPlanned_Click(object sender, System.EventArgs e) {
			SelectionResult=ApptListSelection.Planned;
			DialogResult=DialogResult.OK;
		}

		private void butUnsched_Click(object sender, System.EventArgs e) {
			SelectionResult=ApptListSelection.Unsched;
			DialogResult=DialogResult.OK;
		}

		private void butASAP_Click(object sender,EventArgs e) {
			SelectionResult=ApptListSelection.ASAP;
			DialogResult=DialogResult.OK;
		}

		private void butRadOrders_Click(object sender,EventArgs e) {
			SelectionResult=ApptListSelection.Radiology;
			DialogResult=DialogResult.OK;
		}

        private void butInsVerify_Click(object sender,EventArgs e) {
            SelectionResult=ApptListSelection.InsVerify;
			DialogResult=DialogResult.OK;
        }

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
    }

    ///<summary>Used in FormApptLists as the selection result.</summary>
    public enum ApptListSelection{
		///<summary></summary>
		Recall,
		///<summary></summary>
		Confirm,
		///<summary></summary>
		Planned,
		///<summary></summary>
		Unsched,
		///<summary></summary>
		ASAP,
		///<summary></summary>
		Radiology,
		///<summary></summary>
        InsVerify
    }

}






















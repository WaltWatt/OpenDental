using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace OpenDental{
	/// <summary></summary>
	public class FormRegistrationKeyEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private TextBox textKey;
		private TextBox textNote;
		private Label label1;
		private Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox checkForeign;
		private ValidDate textDateStarted;
		private Label label3;
		private Label label4;
		private ValidDate textDateDisabled;
		private Label label5;
		private ValidDate textDateEnded;
		private Label label6;
		private Label label7;
		private Label label8;
		private OpenDental.UI.Button butNow;
		private CheckBox checkFree;
		private Label label9;
		private Label label10;
		private CheckBox checkTesting;
		private Label label11;
		private TextBox textVotesAllotted;
		private Label label12;
		private UI.Button butMoveTo;
		private Label label13;
		private CheckBox checkResellerCustomer;
		private UI.Button butPracticeTitleReset;
		private Label label14;
		private TextBox textPatNum;
		public RegistrationKey RegKey;

		///<summary></summary>
		public FormRegistrationKeyEdit(){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegistrationKeyEdit));
			this.textKey = new System.Windows.Forms.TextBox();
			this.textNote = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkForeign = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.checkFree = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.checkTesting = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textVotesAllotted = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.checkResellerCustomer = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textPatNum = new System.Windows.Forms.TextBox();
			this.butMoveTo = new OpenDental.UI.Button();
			this.butNow = new OpenDental.UI.Button();
			this.textDateEnded = new OpenDental.ValidDate();
			this.textDateDisabled = new OpenDental.ValidDate();
			this.textDateStarted = new OpenDental.ValidDate();
			this.butPracticeTitleReset = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// textKey
			// 
			this.textKey.Location = new System.Drawing.Point(134, 10);
			this.textKey.Name = "textKey";
			this.textKey.ReadOnly = true;
			this.textKey.Size = new System.Drawing.Size(269, 20);
			this.textKey.TabIndex = 2;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(134, 326);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(269, 124);
			this.textNote.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(110, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Registration Key";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(59, 329);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Note";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkForeign
			// 
			this.checkForeign.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkForeign.Location = new System.Drawing.Point(44, 61);
			this.checkForeign.Name = "checkForeign";
			this.checkForeign.Size = new System.Drawing.Size(104, 18);
			this.checkForeign.TabIndex = 7;
			this.checkForeign.Text = "Foreign";
			this.checkForeign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkForeign.UseVisualStyleBackColor = true;
			this.checkForeign.Click += new System.EventHandler(this.checkForeign_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(23, 155);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "Date Started";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(23, 188);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(110, 16);
			this.label4.TabIndex = 11;
			this.label4.Text = "Date Disabled";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(23, 256);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(110, 16);
			this.label5.TabIndex = 13;
			this.label5.Text = "Date Ended";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(236, 155);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(207, 16);
			this.label6.TabIndex = 14;
			this.label6.Text = "Not accurate before 11/07";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(236, 186);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(578, 62);
			this.label7.TabIndex = 15;
			this.label7.Text = resources.GetString("label7.Text");
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(304, 253);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(419, 32);
			this.label8.TabIndex = 16;
			this.label8.Text = "If a customer drops monthly service, this date should reflect their last date of " +
    "coverage.  They will still be able to download bug fixes.";
			// 
			// checkFree
			// 
			this.checkFree.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFree.Location = new System.Drawing.Point(44, 82);
			this.checkFree.Name = "checkFree";
			this.checkFree.Size = new System.Drawing.Size(104, 18);
			this.checkFree.TabIndex = 18;
			this.checkFree.Text = "Free Version";
			this.checkFree.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFree.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(150, 82);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(572, 16);
			this.label9.TabIndex = 19;
			this.label9.Text = "Only for very poor countries.  Must make absolutely sure that the country meets t" +
    "he requirements.";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(150, 103);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(572, 16);
			this.label10.TabIndex = 21;
			this.label10.Text = "Only for approved developers.  Cannot be used with live patient data.";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkTesting
			// 
			this.checkTesting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTesting.Location = new System.Drawing.Point(12, 103);
			this.checkTesting.Name = "checkTesting";
			this.checkTesting.Size = new System.Drawing.Size(136, 18);
			this.checkTesting.TabIndex = 20;
			this.checkTesting.Text = "For Testing";
			this.checkTesting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTesting.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(23, 292);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(110, 16);
			this.label11.TabIndex = 23;
			this.label11.Text = "Votes Allotted";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textVotesAllotted
			// 
			this.textVotesAllotted.Location = new System.Drawing.Point(134, 291);
			this.textVotesAllotted.Name = "textVotesAllotted";
			this.textVotesAllotted.Size = new System.Drawing.Size(100, 20);
			this.textVotesAllotted.TabIndex = 24;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(240, 292);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(533, 16);
			this.label12.TabIndex = 25;
			this.label12.Text = "Typically 100, although it can be more for multilocation offices.  Sometimes 0.";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.Enabled = false;
			this.label13.Location = new System.Drawing.Point(150, 124);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(655, 16);
			this.label13.TabIndex = 28;
			this.label13.Text = "Only for customers of resellers.  Helps keep track of customers that we bill via " +
    "passthrough to a reseller.";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkResellerCustomer
			// 
			this.checkResellerCustomer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkResellerCustomer.Enabled = false;
			this.checkResellerCustomer.Location = new System.Drawing.Point(12, 124);
			this.checkResellerCustomer.Name = "checkResellerCustomer";
			this.checkResellerCustomer.Size = new System.Drawing.Size(136, 18);
			this.checkResellerCustomer.TabIndex = 27;
			this.checkResellerCustomer.Text = "Reseller Customer";
			this.checkResellerCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkResellerCustomer.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(23, 37);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(110, 16);
			this.label14.TabIndex = 30;
			this.label14.Text = "PatNum";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatNum
			// 
			this.textPatNum.Location = new System.Drawing.Point(134, 36);
			this.textPatNum.Name = "textPatNum";
			this.textPatNum.ReadOnly = true;
			this.textPatNum.Size = new System.Drawing.Size(100, 20);
			this.textPatNum.TabIndex = 29;
			// 
			// butMoveTo
			// 
			this.butMoveTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMoveTo.Autosize = true;
			this.butMoveTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMoveTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMoveTo.CornerRadius = 4F;
			this.butMoveTo.Location = new System.Drawing.Point(239, 33);
			this.butMoveTo.Name = "butMoveTo";
			this.butMoveTo.Size = new System.Drawing.Size(75, 24);
			this.butMoveTo.TabIndex = 26;
			this.butMoveTo.Text = "Move To";
			this.butMoveTo.Click += new System.EventHandler(this.butMoveTo_Click);
			// 
			// butNow
			// 
			this.butNow.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNow.Autosize = true;
			this.butNow.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNow.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNow.CornerRadius = 4F;
			this.butNow.Location = new System.Drawing.Point(240, 255);
			this.butNow.Name = "butNow";
			this.butNow.Size = new System.Drawing.Size(62, 21);
			this.butNow.TabIndex = 17;
			this.butNow.Text = "Now";
			this.butNow.Click += new System.EventHandler(this.butNow_Click);
			// 
			// textDateEnded
			// 
			this.textDateEnded.Location = new System.Drawing.Point(134, 255);
			this.textDateEnded.Name = "textDateEnded";
			this.textDateEnded.Size = new System.Drawing.Size(100, 20);
			this.textDateEnded.TabIndex = 12;
			// 
			// textDateDisabled
			// 
			this.textDateDisabled.Location = new System.Drawing.Point(134, 187);
			this.textDateDisabled.Name = "textDateDisabled";
			this.textDateDisabled.Size = new System.Drawing.Size(100, 20);
			this.textDateDisabled.TabIndex = 10;
			// 
			// textDateStarted
			// 
			this.textDateStarted.Location = new System.Drawing.Point(134, 154);
			this.textDateStarted.Name = "textDateStarted";
			this.textDateStarted.Size = new System.Drawing.Size(100, 20);
			this.textDateStarted.TabIndex = 8;
			// 
			// butPracticeTitleReset
			// 
			this.butPracticeTitleReset.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPracticeTitleReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPracticeTitleReset.Autosize = true;
			this.butPracticeTitleReset.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPracticeTitleReset.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPracticeTitleReset.CornerRadius = 4F;
			this.butPracticeTitleReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPracticeTitleReset.Location = new System.Drawing.Point(134, 485);
			this.butPracticeTitleReset.Name = "butPracticeTitleReset";
			this.butPracticeTitleReset.Size = new System.Drawing.Size(131, 26);
			this.butPracticeTitleReset.TabIndex = 6;
			this.butPracticeTitleReset.Text = "Reset Practice Title";
			this.butPracticeTitleReset.Visible = false;
			this.butPracticeTitleReset.Click += new System.EventHandler(this.butPracticeTitleReset_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(690, 444);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(690, 485);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormRegistrationKeyEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(817, 536);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.textPatNum);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.checkResellerCustomer);
			this.Controls.Add(this.butMoveTo);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.textVotesAllotted);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.checkTesting);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.checkFree);
			this.Controls.Add(this.butNow);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textDateEnded);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textDateDisabled);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDateStarted);
			this.Controls.Add(this.checkForeign);
			this.Controls.Add(this.butPracticeTitleReset);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.textKey);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRegistrationKeyEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Registration Key";
			this.Load += new System.EventHandler(this.FormRegistrationKeyEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRegistrationKeyEdit_Load(object sender,EventArgs e) {
			if(RegKey.RegKey.Length==16){
				textKey.Text=RegKey.RegKey.Substring(0,4)+"-"+RegKey.RegKey.Substring(4,4)+"-"
					+RegKey.RegKey.Substring(8,4)+"-"+RegKey.RegKey.Substring(12,4);
			}
			else{
				textKey.Text=RegKey.RegKey;
			}
			textPatNum.Text=RegKey.PatNum.ToString();
			checkForeign.Checked=RegKey.IsForeign;
			checkFree.Checked=RegKey.IsFreeVersion;
			checkTesting.Checked=RegKey.IsOnlyForTesting;
			checkResellerCustomer.Checked=RegKey.IsResellerCustomer;
			//checkServerVersion.Checked=RegKey.UsesServerVersion;
			textDateStarted.Text=RegKey.DateStarted.ToShortDateString();
			if(RegKey.DateDisabled.Year>1880){
				textDateDisabled.Text=RegKey.DateDisabled.ToShortDateString();
			}
			if(RegKey.DateEnded.Year>1880){
				textDateEnded.Text=RegKey.DateEnded.ToShortDateString();
			}
			textVotesAllotted.Text=RegKey.VotesAllotted.ToString();
			textNote.Text=RegKey.Note;
			//Make the practice title reset button visible for HQ.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				butPracticeTitleReset.Visible=true;
			}
		}

		private void butMoveTo_Click(object sender,EventArgs e) {
			FormPatientSelect form=new FormPatientSelect();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			RegKey.PatNum=form.SelectedPatNum;
			RegistrationKeys.Update(RegKey);
			MessageBox.Show("Registration key moved successfully");
			DialogResult=DialogResult.OK;//Chart module grid will refresh after closing this form, showing that the key is no longer in the ptinfo grid of the chart.
		}

		private void checkForeign_Click(object sender,EventArgs e) {
			checkForeign.Checked=RegKey.IsForeign;//don't allow user to change
		}

		private void butNow_Click(object sender,EventArgs e) {
			textDateEnded.Text=DateTime.Today.ToShortDateString();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			try{
				RegistrationKeys.Delete(RegKey.RegistrationKeyNum);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
			}
			Close();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDateStarted.errorProvider1.GetError(textDateStarted)!=""
				|| textDateDisabled.errorProvider1.GetError(textDateDisabled)!=""
				|| textDateEnded.errorProvider1.GetError(textDateEnded)!=""
				) 
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			try {
				PIn.Int(textVotesAllotted.Text);
			}
			catch {
				MsgBox.Show(this,"Votes Allotted is invalid.");
				return;
			}
			//RegKey.RegKey=textKey.Text;//It's read only.
			RegKey.DateStarted=PIn.Date(textDateStarted.Text);
			RegKey.DateDisabled=PIn.Date(textDateDisabled.Text);
			RegKey.DateEnded=PIn.Date(textDateEnded.Text);
			RegKey.IsFreeVersion=checkFree.Checked;
			RegKey.IsOnlyForTesting=checkTesting.Checked;
			RegKey.IsResellerCustomer=checkResellerCustomer.Checked;
			//RegKey.UsesServerVersion=checkServerVersion.Checked;
			RegKey.VotesAllotted=PIn.Int(textVotesAllotted.Text);
			RegKey.Note=textNote.Text;
			RegistrationKeys.Update(RegKey);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		/// <summary>Inserts RegistrationKey with blank PracticeTitle into bugs database so next time cusotmer hits the update service it will reset their PracticeTitle.</summary>
		private void butPracticeTitleReset_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you sure you want to reset the Practice Title associated with this Registration Key? This should only be done if they are getting a message saying, \"Practice title given does not match the practice title on record,\" when connecting to the Patient Portal. It will be cleared out of the database and filled in with the appropriate Practice Title next time they connect using this Registration Key.")) {
				return;
			}
			//prepare the xml document to send--------------------------------------------------------------------------------------
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars=("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("PracticeTitleReset");
					writer.WriteStartElement("RegistrationKey");
						writer.WriteString(RegKey.RegKey);
					writer.WriteEndElement();
				writer.WriteEndElement();
			}
			#if DEBUG
				OpenDental.localhost.Service1 updateService=new OpenDental.localhost.Service1();
			#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
				updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
			#endif
			if(PrefC.GetString(PrefName.UpdateWebProxyAddress)!="") {
				IWebProxy proxy=new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
				ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
				proxy.Credentials=cred;
				updateService.Proxy=proxy;
			}
			updateService.PracticeTitleReset(strbuild.ToString());//may throw error
		}

		

		


	}
}






















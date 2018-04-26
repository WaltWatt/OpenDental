using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTreatPlanEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private OpenDental.ValidDate textDateTP;
		private System.Windows.Forms.TextBox textHeading;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TreatPlan PlanCur;
		private OpenDental.UI.Button butClearResponsParty;
		private OpenDental.UI.Button butPickResponsParty;
		private TextBox textResponsParty;
		private Label labelResponsParty;
		private UI.Button butSigClear;
		private TextBox textDocument;
		private Label labelDocument;
		private UI.Button butDocumentDetach;
		private UI.Button butDocumentView;
		private ODtextBox textNote;
		private TextBox textUserEntry;
		private Label label4;
		private UI.Button butPickPresenter;
		private TextBox textPresenter;
		private Label label5;
		private OpenDental.UI.Button butDelete;
		private Userod _presenterCur;
		private Userod _presenterOld;

		///<summary></summary>
		public FormTreatPlanEdit(TreatPlan planCur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			PlanCur=planCur.Copy();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTreatPlanEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textDateTP = new OpenDental.ValidDate();
			this.label1 = new System.Windows.Forms.Label();
			this.textHeading = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.butClearResponsParty = new OpenDental.UI.Button();
			this.butPickResponsParty = new OpenDental.UI.Button();
			this.textResponsParty = new System.Windows.Forms.TextBox();
			this.labelResponsParty = new System.Windows.Forms.Label();
			this.butSigClear = new OpenDental.UI.Button();
			this.textDocument = new System.Windows.Forms.TextBox();
			this.labelDocument = new System.Windows.Forms.Label();
			this.butDocumentDetach = new OpenDental.UI.Button();
			this.butDocumentView = new OpenDental.UI.Button();
			this.textNote = new OpenDental.ODtextBox();
			this.textUserEntry = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butPickPresenter = new OpenDental.UI.Button();
			this.textPresenter = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
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
			this.butCancel.Location = new System.Drawing.Point(589, 385);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
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
			this.butOK.Location = new System.Drawing.Point(589, 347);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textDateTP
			// 
			this.textDateTP.Location = new System.Drawing.Point(138, 22);
			this.textDateTP.Name = "textDateTP";
			this.textDateTP.Size = new System.Drawing.Size(85, 20);
			this.textDateTP.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(47, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHeading
			// 
			this.textHeading.Location = new System.Drawing.Point(138, 44);
			this.textHeading.Name = "textHeading";
			this.textHeading.Size = new System.Drawing.Size(489, 20);
			this.textHeading.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(47, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Heading";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(47, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(89, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Note";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 384);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(86, 24);
			this.butDelete.TabIndex = 8;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butClearResponsParty
			// 
			this.butClearResponsParty.AdjustImageLocation = new System.Drawing.Point(1, 1);
			this.butClearResponsParty.Autosize = true;
			this.butClearResponsParty.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearResponsParty.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearResponsParty.CornerRadius = 4F;
			this.butClearResponsParty.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butClearResponsParty.Location = new System.Drawing.Point(385, 257);
			this.butClearResponsParty.Name = "butClearResponsParty";
			this.butClearResponsParty.Size = new System.Drawing.Size(25, 23);
			this.butClearResponsParty.TabIndex = 71;
			this.butClearResponsParty.TabStop = false;
			this.butClearResponsParty.Click += new System.EventHandler(this.butClearResponsParty_Click);
			// 
			// butPickResponsParty
			// 
			this.butPickResponsParty.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickResponsParty.Autosize = true;
			this.butPickResponsParty.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickResponsParty.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickResponsParty.CornerRadius = 4F;
			this.butPickResponsParty.Location = new System.Drawing.Point(337, 257);
			this.butPickResponsParty.Name = "butPickResponsParty";
			this.butPickResponsParty.Size = new System.Drawing.Size(48, 23);
			this.butPickResponsParty.TabIndex = 70;
			this.butPickResponsParty.TabStop = false;
			this.butPickResponsParty.Text = "Pick";
			this.butPickResponsParty.Click += new System.EventHandler(this.butPickResponsParty_Click);
			// 
			// textResponsParty
			// 
			this.textResponsParty.AcceptsReturn = true;
			this.textResponsParty.Location = new System.Drawing.Point(137, 259);
			this.textResponsParty.Name = "textResponsParty";
			this.textResponsParty.ReadOnly = true;
			this.textResponsParty.Size = new System.Drawing.Size(198, 20);
			this.textResponsParty.TabIndex = 69;
			// 
			// labelResponsParty
			// 
			this.labelResponsParty.Location = new System.Drawing.Point(-2, 259);
			this.labelResponsParty.Name = "labelResponsParty";
			this.labelResponsParty.Size = new System.Drawing.Size(138, 17);
			this.labelResponsParty.TabIndex = 68;
			this.labelResponsParty.Text = "Responsible Party";
			this.labelResponsParty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSigClear
			// 
			this.butSigClear.AdjustImageLocation = new System.Drawing.Point(1, 1);
			this.butSigClear.Autosize = true;
			this.butSigClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSigClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSigClear.CornerRadius = 4F;
			this.butSigClear.Location = new System.Drawing.Point(275, 385);
			this.butSigClear.Name = "butSigClear";
			this.butSigClear.Size = new System.Drawing.Size(110, 23);
			this.butSigClear.TabIndex = 75;
			this.butSigClear.TabStop = false;
			this.butSigClear.Text = "Clear Signature";
			this.butSigClear.Click += new System.EventHandler(this.butSigClear_Click);
			// 
			// textDocument
			// 
			this.textDocument.AcceptsReturn = true;
			this.textDocument.Location = new System.Drawing.Point(137, 285);
			this.textDocument.Name = "textDocument";
			this.textDocument.ReadOnly = true;
			this.textDocument.Size = new System.Drawing.Size(198, 20);
			this.textDocument.TabIndex = 77;
			// 
			// labelDocument
			// 
			this.labelDocument.Location = new System.Drawing.Point(-2, 285);
			this.labelDocument.Name = "labelDocument";
			this.labelDocument.Size = new System.Drawing.Size(138, 17);
			this.labelDocument.TabIndex = 76;
			this.labelDocument.Text = "Saved Document";
			this.labelDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDocumentDetach
			// 
			this.butDocumentDetach.AdjustImageLocation = new System.Drawing.Point(1, 1);
			this.butDocumentDetach.Autosize = true;
			this.butDocumentDetach.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDocumentDetach.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDocumentDetach.CornerRadius = 4F;
			this.butDocumentDetach.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDocumentDetach.Location = new System.Drawing.Point(385, 283);
			this.butDocumentDetach.Name = "butDocumentDetach";
			this.butDocumentDetach.Size = new System.Drawing.Size(25, 23);
			this.butDocumentDetach.TabIndex = 78;
			this.butDocumentDetach.TabStop = false;
			this.butDocumentDetach.Click += new System.EventHandler(this.butDocumentDetach_Click);
			// 
			// butDocumentView
			// 
			this.butDocumentView.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDocumentView.Autosize = true;
			this.butDocumentView.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDocumentView.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDocumentView.CornerRadius = 4F;
			this.butDocumentView.Location = new System.Drawing.Point(337, 283);
			this.butDocumentView.Name = "butDocumentView";
			this.butDocumentView.Size = new System.Drawing.Size(48, 23);
			this.butDocumentView.TabIndex = 79;
			this.butDocumentView.TabStop = false;
			this.butDocumentView.Text = "View";
			this.butDocumentView.Click += new System.EventHandler(this.butDocumentView_Click);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(138, 66);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.TreatPlan;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(489, 181);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 80;
			this.textNote.Text = "";
			// 
			// textUserEntry
			// 
			this.textUserEntry.AcceptsReturn = true;
			this.textUserEntry.Location = new System.Drawing.Point(138, 337);
			this.textUserEntry.Name = "textUserEntry";
			this.textUserEntry.ReadOnly = true;
			this.textUserEntry.Size = new System.Drawing.Size(198, 20);
			this.textUserEntry.TabIndex = 86;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(-1, 337);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(138, 17);
			this.label4.TabIndex = 85;
			this.label4.Text = "User Entry";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickPresenter
			// 
			this.butPickPresenter.AdjustImageLocation = new System.Drawing.Point(1, 1);
			this.butPickPresenter.Autosize = true;
			this.butPickPresenter.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickPresenter.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickPresenter.CornerRadius = 4F;
			this.butPickPresenter.Location = new System.Drawing.Point(337, 310);
			this.butPickPresenter.Name = "butPickPresenter";
			this.butPickPresenter.Size = new System.Drawing.Size(23, 21);
			this.butPickPresenter.TabIndex = 84;
			this.butPickPresenter.TabStop = false;
			this.butPickPresenter.Text = "...";
			this.butPickPresenter.Click += new System.EventHandler(this.butPickPresenter_Click);
			// 
			// textPresenter
			// 
			this.textPresenter.AcceptsReturn = true;
			this.textPresenter.Location = new System.Drawing.Point(138, 311);
			this.textPresenter.Name = "textPresenter";
			this.textPresenter.ReadOnly = true;
			this.textPresenter.Size = new System.Drawing.Size(198, 20);
			this.textPresenter.TabIndex = 82;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(-1, 311);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(138, 17);
			this.label5.TabIndex = 81;
			this.label5.Text = "Presenter";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormTreatPlanEdit
			// 
			this.ClientSize = new System.Drawing.Size(676, 420);
			this.Controls.Add(this.textUserEntry);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butPickPresenter);
			this.Controls.Add(this.textPresenter);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butDocumentView);
			this.Controls.Add(this.butDocumentDetach);
			this.Controls.Add(this.textDocument);
			this.Controls.Add(this.labelDocument);
			this.Controls.Add(this.butSigClear);
			this.Controls.Add(this.butClearResponsParty);
			this.Controls.Add(this.butPickResponsParty);
			this.Controls.Add(this.textResponsParty);
			this.Controls.Add(this.labelResponsParty);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textHeading);
			this.Controls.Add(this.textDateTP);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTreatPlanEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Treatment Plan";
			this.Load += new System.EventHandler(this.FormTreatPlanEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTreatPlanEdit_Load(object sender, System.EventArgs e) {
			//this window never comes up for new TP.  Always saved ahead of time.
			if(!Security.IsAuthorized(Permissions.TreatPlanEdit,PlanCur.DateTP)) {
				butOK.Enabled=false;
				butDelete.Enabled=false;
				butPickResponsParty.Enabled=false;
				butClearResponsParty.Enabled=false;
				butSigClear.Enabled=false;
				butDocumentDetach.Enabled=false;
				textHeading.ReadOnly=true;
				textDateTP.ReadOnly=true;
				textNote.ReadOnly=true;
				if(Security.IsAuthorized(Permissions.TreatPlanSign,PlanCur.DateTP)) {//User has permission to edit the heading field.
					textHeading.ReadOnly=false;
					butOK.Enabled=true;
				}
			}
			if(!Security.IsAuthorized(Permissions.TreatPlanPresenterEdit,true)) {
				butPickPresenter.Visible=false;
			}
			if(PlanCur.UserNumPresenter>0) {
				_presenterCur=Userods.GetUser(PlanCur.UserNumPresenter);
				_presenterOld=_presenterCur.Copy();
				textPresenter.Text=_presenterCur.UserName;
			}
			textUserEntry.Text=Userods.GetName(PlanCur.SecUserNumEntry);
			textDateTP.Text=PlanCur.DateTP.ToShortDateString();
			textHeading.Text=PlanCur.Heading;
			textNote.Text=PlanCur.Note;
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				labelResponsParty.Visible=false;
				textResponsParty.Visible=false;
				butPickResponsParty.Visible=false;
				butClearResponsParty.Visible=false;
			}
			if(PlanCur.ResponsParty!=0){
				textResponsParty.Text=Patients.GetLim(PlanCur.ResponsParty).GetNameLF();
			}
			if(PlanCur.Signature!="") { //Per Nathan 01 OCT 2015: In addition to invalidating signature (old behavior) we will also block editing signed TPs.
				butOK.Enabled=false;
				textHeading.ReadOnly=true;
				textDateTP.ReadOnly=true;
				textNote.ReadOnly=true;
				butClearResponsParty.Enabled=false;
				butPickResponsParty.Enabled=false;
				butSigClear.Visible=true;
				butDocumentDetach.Enabled=false;
			}
			else {
				butSigClear.Visible=false;
				butSigClear.Enabled=false;
			}
			if(PlanCur.DocNum>0) {//Was set at some point in the past.
				Document doc=Documents.GetByNum(PlanCur.DocNum);
				if(doc.DocNum==0) {
					textDocument.Text="("+Lan.g(this,"Missing Document")+")";//Invalid Fkey to document.DocNum
					butDocumentView.Enabled=false;
				}
				else {
					textDocument.Text=doc.Description;
					if(!Documents.DocExists(doc.DocNum)) {
						textDocument.Text+=" ("+Lan.g(this,"Unreachable File")+")";//Document points to unreachable file
						butDocumentView.Enabled=false;
					}
				}
			}
			else {//hide document controls because there is no attached document
				labelDocument.Visible=false;
				textDocument.Visible=false;
				butDocumentView.Visible=false;
				butDocumentDetach.Visible=false;
			}
		}

		private void butPickResponsParty_Click(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK){
				return;
			}
			PlanCur.ResponsParty=FormPS.SelectedPatNum;
			textResponsParty.Text=Patients.GetLim(PlanCur.ResponsParty).GetNameLF();
		}

		private void butClearResponsParty_Click(object sender,EventArgs e) {
			PlanCur.ResponsParty=0;
			textResponsParty.Text="";
		}

		private void butDocumentView_Click(object sender,EventArgs e) {
			Document doc=Documents.GetByNum(PlanCur.DocNum);
			if(doc.DocNum==0) {
				MsgBox.Show(this,"Error locating document.");
				return;
			}
			if(!Documents.DocExists(doc.DocNum)) {
				MsgBox.Show(this,"Unable to open document.");
				return;
			}
			Documents.OpenDoc(doc.DocNum);
		}

		private void butDocumentDetach_Click(object sender,EventArgs e) {
			PlanCur.DocNum=0;
			butDocumentView.Enabled=false;
			butDocumentDetach.Enabled=false;
			textDocument.Text="";
		}

		private void butSigClear_Click(object sender,EventArgs e) {
			//Cannot click this button if you are not authorized to edit, so it is safe to re-enable edit controls below.
			//Disable signature buttons
			if(!Security.IsAuthorized(Permissions.TreatPlanEdit,PlanCur.DateTP)) {
				butOK.Enabled=false;
			}
			else {
				butOK.Enabled=true;
			}
			PlanCur.Signature="";
			PlanCur.SigIsTopaz=false;
			butSigClear.Enabled=false;
			//Re-enable controls to edit. 
			textHeading.ReadOnly=false;
			textDateTP.ReadOnly=false;
			textNote.ReadOnly=false;
			butClearResponsParty.Enabled=true;
			butPickResponsParty.Enabled=true;
			butDocumentDetach.Enabled=true;
		}

		private void butPickPresenter_Click(object sender,EventArgs e) {
			FormUserPick FormUP=new FormUserPick();
			List<Userod> listUsers=Userods.GetWhere(x => x.ClinicIsRestricted == false || x.ClinicNum == Clinics.ClinicNum,true);
			FormUP.ListUserodsFiltered=listUsers;
			if(_presenterCur!=null) {
				FormUP.SuggestedUserNum=_presenterCur.UserNum;
			}
			FormUP.IsPickNoneAllowed=true;
			FormUP.ShowDialog();
			if(FormUP.DialogResult!=DialogResult.OK) {
				return;
			}
			_presenterCur=Userods.GetUser(FormUP.SelectedUserNum);//can be null
			if(_presenterCur!=null) {
				textPresenter.Text=_presenterCur.UserName;
			}
			else {
				textPresenter.Text="";
			}
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			//if(IsNew){
			//	DialogResult=DialogResult.Cancel;
			//	return;
			//}
			ProcTPs.DeleteForTP(PlanCur.TreatPlanNum);
			try{
				TreatPlans.Delete(PlanCur);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			TreatPlans.Delete(PlanCur);
			SecurityLogs.MakeLogEntry(Permissions.TreatPlanEdit,PlanCur.PatNum,"Delete TP: "+PlanCur.DateTP.ToShortDateString());
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if( textDateTP.errorProvider1.GetError(textDateTP)!=""
				){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textDateTP.Text==""){
				MsgBox.Show(this,"Please enter a date first.");
				return;
			}
			PlanCur.DateTP=PIn.Date(textDateTP.Text);
			PlanCur.Heading=textHeading.Text;
			PlanCur.Note=textNote.Text;
			if(_presenterCur!=null) {
				PlanCur.UserNumPresenter=_presenterCur.UserNum;
			}
			else {
				PlanCur.UserNumPresenter=0;
			}
			//PlanCur.SecUserNumEntry is updated automatically by MySQL.
			TreatPlans.Update(PlanCur);
			SecurityLogs.MakeLogEntry(Permissions.TreatPlanEdit,PlanCur.PatNum,"Edit TP: "+PlanCur.DateTP.ToShortDateString());
			if(_presenterCur!=null && (_presenterOld==null || _presenterCur.UserNum != _presenterOld.UserNum)) {
				SecurityLogs.MakeLogEntry(Permissions.TreatPlanPresenterEdit,PlanCur.PatNum,
					"TP Presenter Changed from "+(_presenterOld==null?"\"null\"":_presenterOld.UserName)+" to "+_presenterCur.UserName+".");
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}






















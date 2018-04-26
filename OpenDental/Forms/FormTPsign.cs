using Microsoft.CSharp;
//using Microsoft.Vsa;
using System.CodeDom.Compiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	public delegate List<Document> SaveFileAsDocDelegate(bool isSigSave,Sheet sheetTP);

	///<summary></summary>
	public class FormTPsign : ODForm {
		private System.ComponentModel.IContainer components;
		///<summary></summary>
		public int TotalPages;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.PrintPreviewControl previewContr;
		///<summary></summary>
		public PrintDocument Document;
		private Panel panelSig;
		private Label label1;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private bool SigChanged;
		public TreatPlan TPcur;
		///<summary>Must be sorted by primary key.</summary>
		private List<ProcTP> proctpList;
				//private bool allowTopaz;
		///<summary>Should be set to ContrTreat.SaveTPAsDocument(). Can save multiple copies if multiple TP image categories are defined.</summary>
		public SaveFileAsDocDelegate SaveDocDelegate;
		private SignatureBoxWrapper signatureBoxWrapper;
		public Sheet SheetTP;
		///<summary>True if printing with sheets, false if printing with classic view.</summary>
		public bool DoPrintUsingSheets;

		///<summary></summary>
		public FormTPsign(){
			InitializeComponent();//Required for Windows Form Designer support
		}

		/// <summary>Clean up any resources being used.</summary>
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTPsign));
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.previewContr = new System.Windows.Forms.PrintPreviewControl();
			this.panelSig = new System.Windows.Forms.Panel();
			this.signatureBoxWrapper = new OpenDental.UI.SignatureBoxWrapper();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.panelSig.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "");
			this.imageListMain.Images.SetKeyName(1, "");
			this.imageListMain.Images.SetKeyName(2, "");
			// 
			// previewContr
			// 
			this.previewContr.AutoZoom = false;
			this.previewContr.Location = new System.Drawing.Point(10, 41);
			this.previewContr.Name = "previewContr";
			this.previewContr.Size = new System.Drawing.Size(806, 423);
			this.previewContr.TabIndex = 6;
			// 
			// panelSig
			// 
			this.panelSig.Controls.Add(this.signatureBoxWrapper);
			this.panelSig.Controls.Add(this.butCancel);
			this.panelSig.Controls.Add(this.butOK);
			this.panelSig.Controls.Add(this.label1);
			this.panelSig.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelSig.Location = new System.Drawing.Point(0, 562);
			this.panelSig.Name = "panelSig";
			this.panelSig.Size = new System.Drawing.Size(842, 92);
			this.panelSig.TabIndex = 92;
			// 
			// signatureBoxWrapper
			// 
			this.signatureBoxWrapper.BackColor = System.Drawing.SystemColors.ControlDark;
			this.signatureBoxWrapper.Location = new System.Drawing.Point(162, 3);
			this.signatureBoxWrapper.Name = "signatureBoxWrapper";
			this.signatureBoxWrapper.Size = new System.Drawing.Size(362, 79);
			this.signatureBoxWrapper.TabIndex = 182;
			this.signatureBoxWrapper.SignatureChanged += new System.EventHandler(this.signatureBoxWrapper_SignatureChanged);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(741, 57);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 94;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(741, 25);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 93;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(153, 41);
			this.label1.TabIndex = 92;
			this.label1.Text = "Please Sign Here --->";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(842, 25);
			this.ToolBarMain.TabIndex = 5;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// FormTPsign
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(842, 654);
			this.Controls.Add(this.panelSig);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.previewContr);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormTPsign";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Report";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTPsign_FormClosing);
			this.Load += new System.EventHandler(this.FormTPsign_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormReport_Layout);
			this.panelSig.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormTPsign_Load(object sender, System.EventArgs e) {
			//this window never comes up for new TP.  Always saved ahead of time.
			if(!Security.IsAuthorized(Permissions.TreatPlanSign,TPcur.DateTP)) {
				butOK.Enabled=false;
				signatureBoxWrapper.Enabled=false;
			}
			LayoutToolBar();
			ToolBarMain.Buttons["FullPage"].Pushed=true;
			previewContr.Location=new Point(0,ToolBarMain.Bottom);
			previewContr.Size=new Size(ClientRectangle.Width,ClientRectangle.Height-ToolBarMain.Height-panelSig.Height);
			if(Document.DefaultPageSettings.PrintableArea.Height==0) {
				Document.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			SetSize();
			previewContr.Document=Document;
			ToolBarMain.Buttons["PageNum"].Text=(previewContr.StartPage+1).ToString()
				+" / "+TotalPages.ToString();
			proctpList=ProcTPs.RefreshForTP(TPcur.TreatPlanNum);
			signatureBoxWrapper.SignatureMode=UI.SignatureBoxWrapper.SigMode.TreatPlan;
			string keyData= TreatPlans.GetKeyDataForSignatureHash(TPcur,proctpList);
			signatureBoxWrapper.FillSignature(TPcur.SigIsTopaz,keyData,TPcur.Signature);
		}

		private void SetSize(){
			if(ToolBarMain.Buttons["FullPage"].Pushed){
				//if document fits within window, then don't zoom it bigger; leave it at 100%
				if(Document.DefaultPageSettings.PaperSize.Height<previewContr.ClientSize.Height
					&& Document.DefaultPageSettings.PaperSize.Width<previewContr.ClientSize.Width) {
					previewContr.Zoom=1;
				}
				//if document ratio is taller than screen ratio, shrink by height.
				else if(Document.DefaultPageSettings.PaperSize.Height
					/Document.DefaultPageSettings.PaperSize.Width
					> previewContr.ClientSize.Height / previewContr.ClientSize.Width) {
					previewContr.Zoom=((double)previewContr.ClientSize.Height
						/(double)Document.DefaultPageSettings.PaperSize.Height);
				}
				//otherwise, shrink by width
				else {
					previewContr.Zoom=((double)previewContr.ClientSize.Width
						/(double)Document.DefaultPageSettings.PaperSize.Width);
				}
			}
			else{//100%
				previewContr.Zoom=1;
			}
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print"),0,"","Print"));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",1,"Go Back One Page","Back"));
			ODToolBarButton button=new ODToolBarButton("",-1,"","PageNum");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton("",2,"Go Forward One Page","Fwd"));
			button=new ODToolBarButton(Lan.g(this,"FullPage"),-1,Lan.g(this,"FullPage"),"FullPage");
			button.Style=ODToolBarButtonStyle.ToggleButton;
			ToolBarMain.Buttons.Add(button);
			button=new ODToolBarButton(Lan.g(this,"100%"),-1,Lan.g(this,"100%"),"100");
			button.Style=ODToolBarButtonStyle.ToggleButton;
			ToolBarMain.Buttons.Add(button);
			//ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,"Close This Window","Close"));
		}

		private void FormReport_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			previewContr.Width=ClientSize.Width;	
			previewContr.Height=ClientSize.Height-panelSig.Height-ToolBarMain.Height;
		}

		/*//I don't think we need this:
		///<summary></summary>
		private void FillSignature() {
			textNote.Text="";
			sigBox.ClearTablet();
			if(!panelNote.Visible) {
				return;
			}
			DataRow obj=(DataRow)TreeDocuments.SelectedNode.Tag;
			textNote.Text=DocSelected.Note;
			sigBox.Visible=true;
			sigBox.SetTabletState(0);//never accepts input here
			labelInvalidSig.Visible=false;
			//Topaz box is not supported in Unix, since the required dll is Windows native.
			if(Environment.OSVersion.Platform!=PlatformID.Unix) {
				sigBoxTopaz.Location=sigBox.Location;//this puts both boxes in the same spot.
				sigBoxTopaz.Visible=false;
				((Topaz.SigPlusNET)sigBoxTopaz).SetTabletState(0);
			}
			//A machine running Unix will have DocSelected.SigIsTopaz set to false here, because the visibility of the panelNote
			//will be set to false in the case of Unix and SigIsTopaz. Therefore, the else part of this if-else clause is always
			//run on Unix systems.
			if(DocSelected.SigIsTopaz) {
				if(DocSelected.Signature!=null && DocSelected.Signature!="") {
					sigBox.Visible=false;
					sigBoxTopaz.Visible=true;
					((Topaz.SigPlusNET)sigBoxTopaz).ClearTablet();
					((Topaz.SigPlusNET)sigBoxTopaz).SetSigCompressionMode(0);
					((Topaz.SigPlusNET)sigBoxTopaz).SetEncryptionMode(0);
					((Topaz.SigPlusNET)sigBoxTopaz).SetKeyString(GetHashString(DocSelected));
					((Topaz.SigPlusNET)sigBoxTopaz).SetEncryptionMode(2);//high encryption
					((Topaz.SigPlusNET)sigBoxTopaz).SetSigCompressionMode(2);//high compression
					((Topaz.SigPlusNET)sigBoxTopaz).SetSigString(DocSelected.Signature);
					if(((Topaz.SigPlusNET)sigBoxTopaz).NumberOfTabletPoints() == 0) {
						labelInvalidSig.Visible=true;
					}
				}
			}
			else {
				sigBox.ClearTablet();
				if(DocSelected.Signature!=null && DocSelected.Signature!="") {
					sigBox.Visible=true;
					sigBoxTopaz.Visible=false;
					sigBox.SetKeyString(GetHashString(DocSelected));
					sigBox.SetSigString(DocSelected.Signature);
					if(sigBox.NumberOfTabletPoints()==0) {
						labelInvalidSig.Visible=true;
					}
					sigBox.SetTabletState(0);//not accepting input.
				}
			}
		}*/

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			//MessageBox.Show(e.Button.Tag.ToString());
			switch(e.Button.Tag.ToString()){
				//case "Print":
				//	ToolBarPrint_Click();
				//	break;
				case "Back":
					OnBack_Click();
					break;
				case "Fwd":
					OnFwd_Click();
					break;
				case "FullPage":
					OnFullPage_Click();
					break;
				case "100":
					On100_Click();
					break;
				//case "Close":
				//	OnClose_Click();
				//	break;
			}
		}
		
		private void OnPrint_Click() {
			if(!PrinterL.SetPrinter(Document,PrintSituation.TPPerio,TPcur.PatNum,"Signed treatment plan from "+TPcur.DateTP.ToShortDateString()+" printed")){
				return;
			}
			if(Document.OriginAtMargins){
				//In the sheets framework,we had to set margins to 20 because of a bug in their preview control.
				//We now need to set it back to 0 for the actual printing.
				//Hopefully, this doesn't break anything else.
				Document.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			}
			try{
				Document.Print();
			}
			catch(Exception e){
				MessageBox.Show(Lan.g(this,"Error: ")+e.Message);
			}
			DialogResult=DialogResult.OK;
		}

		private void OnClose_Click() {
			this.Close();
		}

		private void OnBack_Click(){
			if(previewContr.StartPage==0) return;
			previewContr.StartPage--;
			ToolBarMain.Buttons["PageNum"].Text=(previewContr.StartPage+1).ToString()
				+" / "+TotalPages.ToString();
			ToolBarMain.Invalidate();
		}

		private void OnFwd_Click(){
			//if(printPreviewControl2.StartPage==totalPages-1) return;
			previewContr.StartPage++;
			ToolBarMain.Buttons["PageNum"].Text=(previewContr.StartPage+1).ToString()
				+" / "+TotalPages.ToString();
			ToolBarMain.Invalidate();
		}

		private void OnFullPage_Click(){
			ToolBarMain.Buttons["100"].Pushed=!ToolBarMain.Buttons["FullPage"].Pushed;
			ToolBarMain.Invalidate();
			SetSize();
		}

		private void On100_Click(){
			ToolBarMain.Buttons["FullPage"].Pushed=!ToolBarMain.Buttons["100"].Pushed;
			ToolBarMain.Invalidate();
			SetSize();
		}

		private void signatureBoxWrapper_SignatureChanged(object sender,EventArgs e) {
			SigChanged=true;
		}

		private void SaveSignature() {
			if(SigChanged) {
				string keyData = TreatPlans.GetKeyDataForSignatureSaving(TPcur,proctpList);
				TPcur.Signature=signatureBoxWrapper.GetSignature(keyData);
				TPcur.SigIsTopaz=signatureBoxWrapper.GetSigIsTopaz();
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			SaveSignature();//"saves" signature to TPCur, does not save to DB.
			TreatPlans.Update(TPcur);//save signature to DB.
			TPcur.ListProcTPs=ProcTPs.RefreshForTP(TPcur.TreatPlanNum);
			if(DoPrintUsingSheets) {
				SheetParameter.SetParameter(SheetTP,"TreatPlan",TPcur); //update TP on sheet to have new signature for generating pdfs
			}
			if(TPcur.Signature.Length>0 && TPcur.DocNum==0 && PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf)) {
				SigChanged=true;
			}
			else if(TPcur.DocNum>0 && !Documents.DocExists(TPcur.DocNum) && PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf)) {
				//Setting SigChanged to True will resave document below.
				SigChanged=MsgBox.Show(this,MsgBoxButtons.YesNo,"Cannot find saved copy of signed PDF, would you like to resave the document?");
			}
			if(PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf) && SaveDocDelegate!=null && SigChanged && TPcur.Signature.Length>0) {
				List<Document> docs=SaveDocDelegate(true,SheetTP);
				if(docs.Count>0) {
					TPcur.DocNum=docs[0].DocNum;//attach first Doc to TP.
					TreatPlans.Update(TPcur); //update docnum. must be called after signature is updated.
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.TreatPlanEdit,TPcur.PatNum,"Sign TP");
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormTPsign_FormClosing(object sender,FormClosingEventArgs e) {
			//if(allowTopaz){
			//  sigBoxTopaz.Dispose();
			//}
		}
	}
}

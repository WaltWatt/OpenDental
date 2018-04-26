/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormDocInfo : ODForm {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ListBox listCategory;
		private System.Windows.Forms.TextBox textDescript;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label labelFileName;
		private OpenDental.ValidDate textDate;
		private System.ComponentModel.Container components = null;//required by designer
		private System.Windows.Forms.TextBox textFileName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListBox listType;
		//<summary></summary>
		//public bool IsNew;
		private System.Windows.Forms.TextBox textSize;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textToothNumbers;
		private System.Windows.Forms.Label labelToothNums;
		private Patient PatCur;
		private Document DocCur;
		private Document DocOld;
		private UI.Button butOpen;
		private UI.Button butAudit;
		private Label label4;
		private TextBox textTime;
		private string initialSelection;
		private bool _isOkDisabled;
		private List<Def> _listImageCatDefs;

		///<summary>ALWAYS save docCur before loading this form.</summary>
		public FormDocInfo(Patient patCur,Document docCur,string pInitialSelection,bool isOkDisabled=false){
			InitializeComponent();
			PatCur=patCur;
			DocCur=docCur;
			DocOld=DocCur.Copy();
			_isOkDisabled=isOkDisabled;
			initialSelection=pInitialSelection;
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDocInfo));
			this.listCategory = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textDescript = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.labelFileName = new System.Windows.Forms.Label();
			this.textFileName = new System.Windows.Forms.TextBox();
			this.textDate = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.listType = new System.Windows.Forms.ListBox();
			this.textSize = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textToothNumbers = new System.Windows.Forms.TextBox();
			this.labelToothNums = new System.Windows.Forms.Label();
			this.butOpen = new OpenDental.UI.Button();
			this.butAudit = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.textTime = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// listCategory
			// 
			this.listCategory.Location = new System.Drawing.Point(12, 30);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(104, 342);
			this.listCategory.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Category";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(122, 267);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(127, 18);
			this.label2.TabIndex = 2;
			this.label2.Text = "Optional Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDescript
			// 
			this.textDescript.Location = new System.Drawing.Point(252, 264);
			this.textDescript.MaxLength = 255;
			this.textDescript.Multiline = true;
			this.textDescript.Name = "textDescript";
			this.textDescript.Size = new System.Drawing.Size(364, 77);
			this.textDescript.TabIndex = 2;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(665, 382);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "OK";
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(763, 382);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(149, 95);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 18);
			this.label3.TabIndex = 6;
			this.label3.Text = "Date";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFileName
			// 
			this.labelFileName.Location = new System.Drawing.Point(149, 33);
			this.labelFileName.Name = "labelFileName";
			this.labelFileName.Size = new System.Drawing.Size(100, 18);
			this.labelFileName.TabIndex = 8;
			this.labelFileName.Text = "File Name";
			this.labelFileName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textFileName
			// 
			this.textFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textFileName.Location = new System.Drawing.Point(252, 30);
			this.textFileName.Name = "textFileName";
			this.textFileName.ReadOnly = true;
			this.textFileName.Size = new System.Drawing.Size(586, 20);
			this.textFileName.TabIndex = 9;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(252, 92);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(104, 20);
			this.textDate.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(149, 153);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 18);
			this.label5.TabIndex = 11;
			this.label5.Text = "Type";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listType
			// 
			this.listType.Location = new System.Drawing.Point(252, 153);
			this.listType.Name = "listType";
			this.listType.Size = new System.Drawing.Size(104, 69);
			this.listType.TabIndex = 10;
			// 
			// textSize
			// 
			this.textSize.Location = new System.Drawing.Point(252, 61);
			this.textSize.Name = "textSize";
			this.textSize.ReadOnly = true;
			this.textSize.Size = new System.Drawing.Size(134, 20);
			this.textSize.TabIndex = 13;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(149, 64);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 18);
			this.label6.TabIndex = 12;
			this.label6.Text = "File Size";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textToothNumbers
			// 
			this.textToothNumbers.Location = new System.Drawing.Point(252, 233);
			this.textToothNumbers.Name = "textToothNumbers";
			this.textToothNumbers.Size = new System.Drawing.Size(240, 20);
			this.textToothNumbers.TabIndex = 15;
			// 
			// labelToothNums
			// 
			this.labelToothNums.Location = new System.Drawing.Point(149, 234);
			this.labelToothNums.Name = "labelToothNums";
			this.labelToothNums.Size = new System.Drawing.Size(100, 18);
			this.labelToothNums.TabIndex = 14;
			this.labelToothNums.Text = "Tooth Numbers";
			this.labelToothNums.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butOpen
			// 
			this.butOpen.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butOpen.Autosize = true;
			this.butOpen.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOpen.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOpen.CornerRadius = 4F;
			this.butOpen.Location = new System.Drawing.Point(763, 55);
			this.butOpen.Name = "butOpen";
			this.butOpen.Size = new System.Drawing.Size(75, 25);
			this.butOpen.TabIndex = 16;
			this.butOpen.Text = "Open Folder";
			this.butOpen.Click += new System.EventHandler(this.butOpen_Click);
			// 
			// butAudit
			// 
			this.butAudit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAudit.Autosize = true;
			this.butAudit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAudit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAudit.CornerRadius = 4F;
			this.butAudit.Location = new System.Drawing.Point(249, 383);
			this.butAudit.Name = "butAudit";
			this.butAudit.Size = new System.Drawing.Size(92, 24);
			this.butAudit.TabIndex = 126;
			this.butAudit.Text = "Audit Trail";
			this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(154, 125);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 16);
			this.label4.TabIndex = 128;
			this.label4.Text = "Time";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textTime
			// 
			this.textTime.Location = new System.Drawing.Point(252, 122);
			this.textTime.Name = "textTime";
			this.textTime.Size = new System.Drawing.Size(104, 20);
			this.textTime.TabIndex = 129;
			// 
			// FormDocInfo
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(868, 419);
			this.Controls.Add(this.textTime);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butAudit);
			this.Controls.Add(this.butOpen);
			this.Controls.Add(this.textToothNumbers);
			this.Controls.Add(this.labelToothNums);
			this.Controls.Add(this.textSize);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.listType);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.textDescript);
			this.Controls.Add(this.textFileName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listCategory);
			this.Controls.Add(this.labelFileName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDocInfo";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Item Info";
			this.Load += new System.EventHandler(this.FormDocInfo_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		///<summary></summary>
		public void FormDocInfo_Load(object sender, System.EventArgs e){
			if(_isOkDisabled) {
				butOK.Enabled=false;
			}
			//if (Docs.Cur.FileName.Equals(null))
			listCategory.Items.Clear();
			_listImageCatDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			for(int i=0;i<_listImageCatDefs.Count;i++){
				string folderName=_listImageCatDefs[i].ItemName;
				listCategory.Items.Add(folderName);
				if(i==0 || _listImageCatDefs[i].DefNum==DocCur.DocCategory || folderName==initialSelection){
					listCategory.SelectedIndex=i;
				}
			}
			listType.Items.Clear();
			listType.Items.AddRange(Enum.GetNames(typeof(ImageType)));
			listType.SelectedIndex=(int)DocCur.ImgType;
			textToothNumbers.Text=Tooth.FormatRangeForDisplay(DocCur.ToothNumbers);
			textDate.Text=DocCur.DateCreated.ToShortDateString();
			textTime.Text=DocCur.DateCreated.ToLongTimeString();
			textDescript.Text=DocCur.Description;
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				string patFolder=ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath());
				textFileName.Text=ODFileUtils.CombinePaths(patFolder,DocCur.FileName);
				if(File.Exists(textFileName.Text)) {
					FileInfo fileInfo=new FileInfo(textFileName.Text);
					textSize.Text=fileInfo.Length.ToString("n0");
				}
			}
			else if(CloudStorage.IsCloudStorage) {
				string patFolder=ImageStore.GetPatientFolder(PatCur,"");
				textFileName.Text=ODFileUtils.CombinePaths(patFolder,DocCur.FileName,'/');
			}
			else {
				labelFileName.Visible=false;
				textFileName.Visible=false;
				butOpen.Visible=false;
				textSize.Text=DocCur.RawBase64.Length.ToString("n0");
			}
			textToothNumbers.Text=Tooth.FormatRangeForDisplay(DocCur.ToothNumbers);
			//textNote.Text=DocCur.Note;
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				labelToothNums.Visible=false;
				textToothNumbers.Visible=false;
			}
		}

		private void butOpen_Click(object sender,EventArgs e) {
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				System.Diagnostics.Process.Start("Explorer",Path.GetDirectoryName(textFileName.Text));
			}
			else if(CloudStorage.IsCloudStorage) {//First download, then open
				FormProgress FormP=new FormProgress();
				FormP.DisplayText="Downloading...";
				FormP.NumberFormat="F";
				FormP.NumberMultiplication=1;
				FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
				FormP.TickMS=1000;
				OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.DownloadAsync(ImageStore.GetPatientFolder(PatCur,"")
					,DocCur.FileName
					,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.Cancel) {
					state.DoCancel=true;
					return;
				}
				//Create temp file here or create the file with the actual name?  Changes made when opening the file won't be saved, so I think temp file is best.
				string tempFile=PrefC.GetRandomTempFile(Path.GetExtension(DocCur.FileName));
				File.WriteAllBytes(tempFile,state.FileContent);
				System.Diagnostics.Process.Start(tempFile);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e){
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			if(textDate.Text=="") {
				MsgBox.Show(this,"Please enter a date.");
				return;
			}
			if(textTime.Text=="") {
				MsgBox.Show(this,"Please enter a time.");
				return;
			}
			DateTime time;
			if(!DateTime.TryParse(textTime.Text,out time)) {
				MsgBox.Show(this,"Please enter a valid time.");
				return;
			}
			//We had a security bug where users could change the date to a more recent date, and then subsequently delete.
			//The code below is for that specific scenario.
			DateTime dateTimeEntered=PIn.DateT(textDate.Text+" "+textTime.Text);
			if(dateTimeEntered>DocCur.DateCreated) {
				//user is trying to change the date to some date after the previously linked date
				//is the new doc date allowed?
				if(!Security.IsAuthorized(Permissions.ImageDelete,DocCur.DateCreated,true)) {
					//suppress the default security message above (it's too confusing for this case) and generate our own here
					MessageBox.Show(this,Lan.g(this,"Not allowed to future date this image from")+": "
						+"\r\n"+DocCur.DateCreated.ToString()+" to "+dateTimeEntered.ToString()
						+"\r\n\r\n"+Lan.g(this,"A user with the SecurityAdmin permission must grant you access for")
						+":\r\n"+GroupPermissions.GetDesc(Permissions.ImageDelete));
					return;
				}
			}
			try{
				DocCur.ToothNumbers=Tooth.FormatRangeForDb(textToothNumbers.Text);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DocCur.DocCategory=_listImageCatDefs[listCategory.SelectedIndex].DefNum;
			DocCur.ImgType=(ImageType)listType.SelectedIndex;
			DocCur.Description=textDescript.Text;			
			DocCur.DateCreated=dateTimeEntered;	
			try{//incomplete
				DocCur.ToothNumbers=Tooth.FormatRangeForDb(textToothNumbers.Text);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			//DocCur.Note=textNote.Text;
      //Docs.Cur.LastAltered=DateTime.Today;
			//if(IsNew){
			//	DocCur.Insert(PatCur);
			//}
			//else{
			if(Documents.Update(DocCur,DocOld)) {
				ImageStore.LogDocument(Lan.g(this,"Document Edited")+": ",Permissions.ImageEdit,DocCur,DocOld.DateTStamp);
			}
			//}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butAudit_Click(object sender,EventArgs e) {
			List<Permissions> listPermissions=new List<Permissions>();
			listPermissions.Add(Permissions.ImageEdit);
			listPermissions.Add(Permissions.ImageDelete);
			FormAuditOneType formA=new FormAuditOneType(0,listPermissions,Lan.g(this,"Audit Trail for Document"),DocCur.DocNum);
			formA.ShowDialog();
		}

		
		
	}
}
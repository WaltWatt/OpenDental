using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormLetterMerges : ODForm {
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		//private bool localChanged;
		private System.Drawing.Printing.PrintDocument pd2;
		//private int pagesPrinted=0;
		private System.Windows.Forms.ListBox listCategories;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox listLetters;
		private OpenDental.UI.Button butEditCats;
		private Patient PatCur;
		private List<LetterMerge> ListForCat;
		private bool changed;
		private string mergePath;
#if !DISABLE_MICROSOFT_OFFICE
		//private Word.Application wrdApp;
		private Word._Document wrdDoc;
		private Object oMissing = System.Reflection.Missing.Value;
		private Object oFalse = false;
#endif
		private OpenDental.UI.Button butMerge;
		private OpenDental.UI.Button butCreateData;
		private OpenDental.UI.Button butEditTemplate;
		private System.Windows.Forms.GroupBox groupBox1;
		private OpenDental.UI.Button butViewData;
		private OpenDental.UI.Button butPreview;
		private ComboBox comboImageCategory;
		private List<Def> _listLetterMergeCatDefs;
		private Label labelImageCategory;

		///<summary></summary>
		public FormLetterMerges(Patient patCur){
			InitializeComponent();// Required for Windows Form Designer support
			PatCur=patCur;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLetterMerges));
			this.butCancel = new OpenDental.UI.Button();
			this.listCategories = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.butMerge = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.listLetters = new System.Windows.Forms.ListBox();
			this.butEditCats = new OpenDental.UI.Button();
			this.butCreateData = new OpenDental.UI.Button();
			this.butEditTemplate = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboImageCategory = new System.Windows.Forms.ComboBox();
			this.butViewData = new OpenDental.UI.Button();
			this.butPreview = new OpenDental.UI.Button();
			this.labelImageCategory = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(462,405);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(79,24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// listCategories
			// 
			this.listCategories.Location = new System.Drawing.Point(15,33);
			this.listCategories.Name = "listCategories";
			this.listCategories.Size = new System.Drawing.Size(164,368);
			this.listCategories.TabIndex = 2;
			this.listCategories.Click += new System.EventHandler(this.listCategories_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(14,14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124,14);
			this.label1.TabIndex = 3;
			this.label1.Text = "Categories";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(206,408);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79,24);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butMerge
			// 
			this.butMerge.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butMerge.Autosize = true;
			this.butMerge.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMerge.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMerge.CornerRadius = 4F;
			this.butMerge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butMerge.Location = new System.Drawing.Point(38,84);
			this.butMerge.Name = "butMerge";
			this.butMerge.Size = new System.Drawing.Size(79,24);
			this.butMerge.TabIndex = 17;
			this.butMerge.Text = "Print";
			this.butMerge.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(205,14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(124,14);
			this.label3.TabIndex = 19;
			this.label3.Text = "Letters";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listLetters
			// 
			this.listLetters.Location = new System.Drawing.Point(206,33);
			this.listLetters.Name = "listLetters";
			this.listLetters.Size = new System.Drawing.Size(164,368);
			this.listLetters.TabIndex = 18;
			this.listLetters.DoubleClick += new System.EventHandler(this.listLetters_DoubleClick);
			this.listLetters.Click += new System.EventHandler(this.listLetters_Click);
			// 
			// butEditCats
			// 
			this.butEditCats.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butEditCats.Autosize = true;
			this.butEditCats.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditCats.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditCats.CornerRadius = 4F;
			this.butEditCats.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditCats.Location = new System.Drawing.Point(14,408);
			this.butEditCats.Name = "butEditCats";
			this.butEditCats.Size = new System.Drawing.Size(98,24);
			this.butEditCats.TabIndex = 20;
			this.butEditCats.Text = "Edit Categories";
			this.butEditCats.Click += new System.EventHandler(this.butEditCats_Click);
			// 
			// butCreateData
			// 
			this.butCreateData.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCreateData.Autosize = true;
			this.butCreateData.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreateData.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreateData.CornerRadius = 4F;
			this.butCreateData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCreateData.Location = new System.Drawing.Point(38,22);
			this.butCreateData.Name = "butCreateData";
			this.butCreateData.Size = new System.Drawing.Size(79,24);
			this.butCreateData.TabIndex = 21;
			this.butCreateData.Text = "Data File";
			this.butCreateData.Click += new System.EventHandler(this.butCreateData_Click);
			// 
			// butEditTemplate
			// 
			this.butEditTemplate.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butEditTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEditTemplate.Autosize = true;
			this.butEditTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditTemplate.CornerRadius = 4F;
			this.butEditTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditTemplate.Location = new System.Drawing.Point(449,348);
			this.butEditTemplate.Name = "butEditTemplate";
			this.butEditTemplate.Size = new System.Drawing.Size(92,24);
			this.butEditTemplate.TabIndex = 22;
			this.butEditTemplate.Text = "Edit Template";
			this.butEditTemplate.Click += new System.EventHandler(this.butEditTemplate_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboImageCategory);
			this.groupBox1.Controls.Add(this.labelImageCategory);
			this.groupBox1.Controls.Add(this.butViewData);
			this.groupBox1.Controls.Add(this.butPreview);
			this.groupBox1.Controls.Add(this.butMerge);
			this.groupBox1.Controls.Add(this.butCreateData);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(415,128);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(158,197);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Create";
			// 
			// comboImageCategory
			// 
			this.comboImageCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboImageCategory.Location = new System.Drawing.Point(15, 164);
			this.comboImageCategory.MaxDropDownItems = 30;
			this.comboImageCategory.Name = "comboImageCategory";
			this.comboImageCategory.Size = new System.Drawing.Size(137, 21);
			this.comboImageCategory.TabIndex = 37;
			// 
			// butViewData
			// 
			this.butViewData.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butViewData.Autosize = true;
			this.butViewData.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butViewData.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butViewData.CornerRadius = 4F;
			this.butViewData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butViewData.Location = new System.Drawing.Point(38, 53);
			this.butViewData.Name = "butViewData";
			this.butViewData.Size = new System.Drawing.Size(79, 24);
			this.butViewData.TabIndex = 23;
			this.butViewData.Text = "View Data";
			this.butViewData.Click += new System.EventHandler(this.butViewData_Click);
			// 
			// butPreview
			// 
			this.butPreview.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butPreview.Autosize = true;
			this.butPreview.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPreview.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPreview.CornerRadius = 4F;
			this.butPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPreview.Location = new System.Drawing.Point(38,115);
			this.butPreview.Name = "butPreview";
			this.butPreview.Size = new System.Drawing.Size(79,24);
			this.butPreview.TabIndex = 22;
			this.butPreview.Text = "Preview";
			this.butPreview.Click += new System.EventHandler(this.butPreview_Click);
			// 
			// labelImageCategory
			// 
			this.labelImageCategory.Location = new System.Drawing.Point(13, 147);
			this.labelImageCategory.Name = "labelImageCategory";
			this.labelImageCategory.Size = new System.Drawing.Size(124, 14);
			this.labelImageCategory.TabIndex = 38;
			this.labelImageCategory.Text = "Image Folder";
			this.labelImageCategory.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormLetterMerges
			// 
			this.ClientSize = new System.Drawing.Size(579,446);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butEditTemplate);
			this.Controls.Add(this.butEditCats);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.listLetters);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listCategories);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormLetterMerges";
			this.ShowInTaskbar = false;
			this.Text = "Letter Merge";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormLetterMerges_Closing);
			this.Load += new System.EventHandler(this.FormLetterMerges_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		
		private void FormLetterMerges_Load(object sender, System.EventArgs e) {
			mergePath=PrefC.GetString(PrefName.LetterMergePath);
			FillCats();
			if(listCategories.Items.Count>0){
				listCategories.SelectedIndex=0;
			}
			FillLetters();
			if(listLetters.Items.Count>0){
				listLetters.SelectedIndex=0;
			}
			List<Def> listImageCatDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			comboImageCategory.Items.Clear();
			//Create None image category
			comboImageCategory.Items.Add(new ODBoxItem<Def>(Lan.g(this,"None"),new Def() { DefNum=0 }));
			foreach(Def imageCat in listImageCatDefs) {
				comboImageCategory.Items.Add(new ODBoxItem<Def>(imageCat.ItemName,imageCat));
			}
			SelectImageCat();
		}

		private void FillCats() {
			_listLetterMergeCatDefs=Defs.GetDefsForCategory(DefCat.LetterMergeCats,true);
			listCategories.Items.Clear();
			for(int i=0;i<_listLetterMergeCatDefs.Count;i++){
				listCategories.Items.Add(_listLetterMergeCatDefs[i].ItemName);
			}
		}

		private void FillLetters(){
			listLetters.Items.Clear();
			if(listCategories.SelectedIndex==-1){
				ListForCat=new List<LetterMerge>();
				return;
			}
			LetterMergeFields.RefreshCache();
			LetterMerges.RefreshCache();
			ListForCat=LetterMerges.GetListForCat(listCategories.SelectedIndex);
			for(int i=0;i<ListForCat.Count;i++){
				listLetters.Items.Add(ListForCat[i].Description);
			}
		}

		private void SelectImageCat() {
			long defNumLetter=0;
			if(listLetters.Items.Count>0) {
				LetterMerge letterMergeSelected=ListForCat[listLetters.SelectedIndex];
				if(letterMergeSelected!=null) {
					defNumLetter=letterMergeSelected.ImageFolder;
				}
			}
			if(defNumLetter!=0) {
				comboImageCategory.SetSelectedItem<Def>(x => x.DefNum==defNumLetter,Defs.GetName(DefCat.ImageCats,defNumLetter)+" "+Lan.g(this,"(hidden)"));
			}
			else {
				comboImageCategory.SelectedIndex=0;
			}
		}

		private void listCategories_Click(object sender, System.EventArgs e) {
			//selectedIndex already changed.
			FillLetters();
			if(listLetters.Items.Count>0){
				listLetters.SelectedIndex=0;
			}
			SelectImageCat();
		}

		private void butEditCats_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.LetterMergeCats);
			FormD.ShowDialog();
			FillCats();
		}

		private void listLetters_DoubleClick(object sender, System.EventArgs e) {
			if(listLetters.SelectedIndex==-1){
				return;
			}
			int selectedRow=listLetters.SelectedIndex;
			LetterMerge letter=ListForCat[listLetters.SelectedIndex];
			FormLetterMergeEdit FormL=new FormLetterMergeEdit(letter);
			FormL.ShowDialog();
			FillLetters();
			if(listLetters.Items.Count>selectedRow){
				listLetters.SetSelected(selectedRow,true);
			}
			SelectImageCat();
			changed=true;
		}

		private void listLetters_Click(object sender,EventArgs e) {
			SelectImageCat();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			if(listCategories.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a category first.");
				return;
			}
			LetterMerge letter=new LetterMerge();
			letter.Category=_listLetterMergeCatDefs[listCategories.SelectedIndex].DefNum;
			letter.Fields=new List<string>();
			FormLetterMergeEdit FormL=new FormLetterMergeEdit(letter);
			FormL.IsNew=true;
			FormL.ShowDialog();
			FillLetters();
			changed=true;
		}

		///<summary>Shows and error message and returns false if there is a problem creating the data file; Otherwise true.</summary>
		private bool CreateDataFile(string fileName,LetterMerge letter){
 			DataTable table;
			try {
				table=LetterMergesQueries.GetLetterMergeInfo(PatCur,letter);
			}
			catch(Exception ex) {
				string message=Lan.g(this,"There was a error getting letter merge info:");
				MessageBox.Show(message+"\r\n"+ex.Message);
				return false;
			}
			table=FormQuery.MakeReadable(table,null,false);
			try{
			  using(StreamWriter sw=new StreamWriter(fileName,false)){
					string line="";  
					for(int i=0;i<letter.Fields.Count;i++){
						if(letter.Fields[i].StartsWith("referral.")){
							line+="Ref"+letter.Fields[i].Substring(9);
						}
						else{
							line+=letter.Fields[i];
						}
						if(i<letter.Fields.Count-1){
							line+="\t";
						}
					}
					sw.WriteLine(line);
					string cell;
					for(int i=0;i<table.Rows.Count;i++){
						line="";
						for(int j=0;j<table.Columns.Count;j++){
							cell=table.Rows[i][j].ToString();
							cell=cell.Replace("\r","");
							cell=cell.Replace("\n","");
							cell=cell.Replace("\t","");
							cell=cell.Replace("\"","");
							line+=cell;
							if(j<table.Columns.Count-1){
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
      }
      catch{
        MsgBox.Show(this,"File in use by another program.  Close and try again.");
				return false;
			}
			return true;
		}

		private void butCreateData_Click(object sender, System.EventArgs e) {
			if(!CreateData()){
				return;
			}
			MsgBox.Show(this,"done");
		}

		private void butViewData_Click(object sender,EventArgs e) {
			if(!CreateData()){
				return;
			}
			LetterMerge letterCur=ListForCat[listLetters.SelectedIndex];
			string dataFile=PrefC.GetString(PrefName.LetterMergePath)+letterCur.DataFileName;
			Process.Start(dataFile);
		}

		private bool CreateData(){
			if(listLetters.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a letter first.");
				return false;
			}
			LetterMerge letterCur=ListForCat[listLetters.SelectedIndex];
			string dataFile=PrefC.GetString(PrefName.LetterMergePath)+letterCur.DataFileName;
			if(!Directory.Exists(PrefC.GetString(PrefName.LetterMergePath))){
				MsgBox.Show(this,"Letter merge path not valid.");
				return false;
			}
			Cursor=Cursors.WaitCursor;
			if(!CreateDataFile(dataFile,letterCur)){
				Cursor=Cursors.Default;
				return false;
			}
			Cursor=Cursors.Default;
			return true;
		}

		private void butPrint_Click(object sender, System.EventArgs e) {
#if DISABLE_MICROSOFT_OFFICE
			MessageBox.Show(this, "This version of Open Dental does not support Microsoft Word.");
			return;
#endif
			if(listLetters.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a letter first.");
				return;
			}
			LetterMerge letterCur=ListForCat[listLetters.SelectedIndex];
			letterCur.ImageFolder=comboImageCategory.SelectedTag<Def>().DefNum;
			string templateFile=ODFileUtils.CombinePaths(PrefC.GetString(PrefName.LetterMergePath),letterCur.TemplateName);
			string dataFile=ODFileUtils.CombinePaths(PrefC.GetString(PrefName.LetterMergePath),letterCur.DataFileName);
			if(!File.Exists(templateFile)){
				MsgBox.Show(this,"Template file does not exist.");
				return;
			}
			PrintDocument pd=new PrintDocument();
			if(!PrinterL.SetPrinter(pd,PrintSituation.Default,PatCur.PatNum,"Letter merge "+letterCur.Description+" printed")) {
				return;
			}
			if(!CreateDataFile(dataFile,letterCur)){
				return;
			}
			Word.MailMerge wrdMailMerge;
			//Create an instance of Word.
			Word.Application WrdApp;
			try {
				WrdApp=LetterMerges.WordApp;
			}
			catch {
				MsgBox.Show(this,"Error.  Is MS Word installed?");
				return;
			}
			//Open a document.
			Object oName=templateFile;
			wrdDoc=WrdApp.Documents.Open(ref oName,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing);
			wrdDoc.Select();
			wrdMailMerge=wrdDoc.MailMerge;
			//Attach the data file.
			wrdDoc.MailMerge.OpenDataSource(dataFile,ref oMissing,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing);
			wrdMailMerge.Destination = Word.WdMailMergeDestination.wdSendToPrinter;
			//WrdApp.ActivePrinter=pd.PrinterSettings.PrinterName;
			//replaced with following 4 lines due to MS bug that changes computer default printer
			object oWBasic = WrdApp.WordBasic;
			object[] oWBValues = new object[] { pd.PrinterSettings.PrinterName, 1 };
			String[] sWBNames = new String[] { "Printer", "DoNotSetAsSysDefault" };
			oWBasic.GetType().InvokeMember("FilePrintSetup", BindingFlags.InvokeMethod, null, oWBasic, oWBValues, null, null, sWBNames);
			wrdMailMerge.Execute(ref oFalse);
			if(letterCur.ImageFolder!=0) {//if image folder exist for this letter, save to AtoZ folder
				try {
					wrdDoc.Select();
					wrdMailMerge.Destination = Word.WdMailMergeDestination.wdSendToNewDocument;
					wrdMailMerge.Execute(ref oFalse);
					WrdApp.Activate();
					string tempFilePath=ODFileUtils.CreateRandomFile(Path.GetTempPath(),GetFileExtensionForWordDoc(templateFile));
					Object oFileName=tempFilePath;
					WrdApp.ActiveDocument.SaveAs(oFileName);//save the document 
					WrdApp.ActiveDocument.Close();
					SaveToImageFolder(tempFilePath,letterCur);
				}
				catch(Exception ex) {
					FriendlyException.Show(Lan.g(this,"Error saving file to the Image module:")+"\r\n"+ex.Message,ex);
				}
			}
			//Close the original form document since just one record.
			wrdDoc.Saved=true;
			wrdDoc.Close(ref oFalse,ref oMissing,ref oMissing);
			//At this point, Word remains open with no documents.
			WrdApp.WindowState=Word.WdWindowState.wdWindowStateMinimize;
			wrdMailMerge=null;
			wrdDoc=null;
			Commlog CommlogCur=new Commlog();
			CommlogCur.CommDateTime=DateTime.Now;
			CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			CommlogCur.Mode_=CommItemMode.Mail;
			CommlogCur.SentOrReceived=CommSentOrReceived.Sent;
			CommlogCur.PatNum=PatCur.PatNum;
			CommlogCur.Note="Letter sent: "+letterCur.Description+". ";
			CommlogCur.UserNum=Security.CurUser.UserNum;
			Commlogs.Insert(CommlogCur);
			DialogResult=DialogResult.OK;
		}

		private void butPreview_Click(object sender, System.EventArgs e) {
#if !DISABLE_MICROSOFT_OFFICE
			if(listLetters.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a letter first.");
				return;
			}
			LetterMerge letterCur=ListForCat[listLetters.SelectedIndex];
			letterCur.ImageFolder=comboImageCategory.SelectedTag<Def>().DefNum;
			string templateFile=ODFileUtils.CombinePaths(PrefC.GetString(PrefName.LetterMergePath),letterCur.TemplateName);
			string dataFile=ODFileUtils.CombinePaths(PrefC.GetString(PrefName.LetterMergePath),letterCur.DataFileName);
			if(!File.Exists(templateFile)){
				MsgBox.Show(this,"Template file does not exist.");
				return;
			}
			if(!CreateDataFile(dataFile,letterCur)){
				return;
			}
			Word.MailMerge wrdMailMerge;
			//Create an instance of Word.
			Word.Application WrdApp;
			try{
				WrdApp=LetterMerges.WordApp;
			}
			catch{
				MsgBox.Show(this,"Error. Is Word installed?");
				return;
			}
			string errorMessage="";
			//Open a document.
			try {
				Object oName=templateFile;
				wrdDoc=WrdApp.Documents.Open(ref oName,ref oMissing,ref oMissing,
					ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,
					ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing);
				wrdDoc.Select();
			}
			catch(Exception ex) {
				errorMessage=Lan.g(this,"Error opening document:")+"\r\n"+ex.Message;
				MessageBox.Show(errorMessage);
				return;
			}
			//Attach the data file.
			try {
				wrdMailMerge=wrdDoc.MailMerge;
				wrdDoc.MailMerge.OpenDataSource(dataFile,ref oMissing,ref oMissing,ref oMissing,
					ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,
					ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing);
				wrdMailMerge.Destination = Word.WdMailMergeDestination.wdSendToNewDocument;
				wrdMailMerge.Execute(ref oFalse);
			}
			catch(Exception ex) {
				errorMessage=Lan.g(this,"Error attaching data file:")+"\r\n"+ex.Message;
				MessageBox.Show(errorMessage);
				return;
			}
			if(letterCur.ImageFolder!=0) {//if image folder exist for this letter, save to AtoZ folder
				//Open document from the atoz folder.
				try {
					WrdApp.Activate();
					string tempFilePath=ODFileUtils.CreateRandomFile(Path.GetTempPath(),GetFileExtensionForWordDoc(templateFile));
					Object oFileName=tempFilePath;
					WrdApp.ActiveDocument.SaveAs(oFileName);//save the document to temp location
					Document doc=SaveToImageFolder(tempFilePath,letterCur);
					string patFolder=ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath());
					string fileName=ImageStore.GetFilePath(doc,patFolder);
					if(!FileAtoZ.Exists(fileName)) {
						throw new ApplicationException(Lans.g("LetterMerge","Error opening document"+" "+doc.FileName));
					}
					FileAtoZ.StartProcess(fileName);
					WrdApp.ActiveDocument.Close();//Necessary since we created an extra document
					try {
						File.Delete(tempFilePath);//Clean up the temp file
					}
					catch(Exception ex) {
						ex.DoNothing();
					}
				}
				catch(Exception ex) {
					FriendlyException.Show(Lan.g(this,"Error saving file to the Image module:")+"\r\n"+ex.Message,ex);
				}
			}
			//Close the original form document since just one record.
			try {
				wrdDoc.Saved=true;
				wrdDoc.Close(ref oFalse,ref oMissing,ref oMissing);
			}
			catch(Exception ex) {
				errorMessage=Lan.g(this,"Error closing document:")+"\r\n"+ex.Message;
				MessageBox.Show(errorMessage);
				return;
			}
			//At this point, Word remains open with just one new document.
			try {
				WrdApp.Activate();
				if(WrdApp.WindowState==Word.WdWindowState.wdWindowStateMinimize) {
					WrdApp.WindowState=Word.WdWindowState.wdWindowStateMaximize;
				}
			}
			catch(Exception ex) {
				errorMessage=Lan.g(this,"Error showing Microsoft Word:")+"\r\n"+ex.Message;
				MessageBox.Show(errorMessage);
				return;
			}
			wrdMailMerge=null;
			wrdDoc=null;
			Commlog CommlogCur=new Commlog();
			CommlogCur.CommDateTime=DateTime.Now;
			CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			CommlogCur.Mode_=CommItemMode.Mail;
			CommlogCur.SentOrReceived=CommSentOrReceived.Sent;
			CommlogCur.PatNum=PatCur.PatNum;
			CommlogCur.Note="Letter sent: "+letterCur.Description+". ";
			CommlogCur.UserNum=Security.CurUser.UserNum;
			Commlogs.Insert(CommlogCur);
#else
			MessageBox.Show(this, "This version of Open Dental does not support Microsoft Word.");
#endif
			//this window now closes regardless of whether the user saved the comm item.
			DialogResult=DialogResult.OK;
		}

		///<summary>Returns default Microsoft Word extension of .docx. Returns extension .doc If the file passed in has an extension of .dot,.doc,or .dotm.</summary>
		private string GetFileExtensionForWordDoc(string filePath) {
			string retVal=".docx";//default file extension
			string ext=Path.GetExtension(filePath).ToLower();
			List<string> listBackwardCompat=new List<string> { ".dot",".doc",".dotm" };
			if(listBackwardCompat.Contains(ext)) {
				retVal=".doc";
			}
			return retVal;
		}

		private Document SaveToImageFolder(string fileSourcePath,LetterMerge letterCur) {
			if(letterCur.ImageFolder==0) {//This shouldn't happen
				return new Document();
			}
			string rawBase64="";
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				rawBase64=Convert.ToBase64String(File.ReadAllBytes(fileSourcePath));
			}
			Document docSave=new Document();
			docSave.ImgType=ImageType.Document;
			docSave.DateCreated=DateTime.Now;
			docSave.PatNum=PatCur.PatNum;
			docSave.DocCategory=letterCur.ImageFolder;
			docSave.Description=letterCur.Description;//no extension.
			docSave.RawBase64=rawBase64;//blank if using AtoZfolder
			docSave.FileName=GetFileExtensionForWordDoc(fileSourcePath);//file extension used for both DB images and AtoZ images. The insert will generate the file name.
			docSave=Documents.InsertAndGet(docSave,PatCur);
			string fileDestPath=ImageStore.GetFilePath(docSave,ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath()));
			FileAtoZ.Copy(fileSourcePath,fileDestPath,FileAtoZSourceDestination.LocalToAtoZ);
			return docSave;
		}

		private void butEditTemplate_Click(object sender, System.EventArgs e) {
#if !DISABLE_MICROSOFT_OFFICE
			if(listLetters.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a letter first.");
				return;
			}
			LetterMerge letterCur=ListForCat[listLetters.SelectedIndex];
			string templateFile=ODFileUtils.CombinePaths(PrefC.GetString(PrefName.LetterMergePath),letterCur.TemplateName);
			string dataFile=ODFileUtils.CombinePaths(PrefC.GetString(PrefName.LetterMergePath),letterCur.DataFileName);
			if(!File.Exists(templateFile)){
				MessageBox.Show(Lan.g(this,"Template file does not exist:")+"  "+templateFile);
				return;
			}
			if(!CreateDataFile(dataFile,letterCur)){
				return;
			}
			//Create an instance of Word.
			Word.Application WrdApp;
			try {
				WrdApp=LetterMerges.WordApp;
			}
			catch {
				MsgBox.Show(this,"Error.  Is MS Word installed?");
				return;
			}
			//Open a document.
			Object oName=templateFile;
			wrdDoc=WrdApp.Documents.Open(ref oName,ref oMissing,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing);
			wrdDoc.Select();
			//Attach the data file.
			wrdDoc.MailMerge.OpenDataSource(dataFile,ref oMissing,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,
				ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing);
			//At this point, Word remains open with just one new document.
			if(WrdApp.WindowState==Word.WdWindowState.wdWindowStateMinimize){
				WrdApp.WindowState=Word.WdWindowState.wdWindowStateMaximize;
			}
			wrdDoc=null;
#else
			MessageBox.Show(this, "This version of Open Dental does not support Microsoft Word.");
#endif
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormLetterMerges_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.LetterMerge);
			}
		}

		

		

		

		

		

		

		

		

		


		

		

		

		

		

		

		

		


	}
}






















using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Net;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClaimReports : ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelRetrieving;
		private System.Windows.Forms.ComboBox comboClearhouse;
		private OpenDental.UI.Button butRetrieve;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary>If true, then reports will be automatically retrieved for default clearinghouse.  Then this form will close.</summary>
		public bool AutomaticMode;
		private List<Clearinghouse> _listHqClearinghouses;

		///<summary></summary>
		public FormClaimReports()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimReports));
			this.labelRetrieving = new System.Windows.Forms.Label();
			this.comboClearhouse = new System.Windows.Forms.ComboBox();
			this.butRetrieve = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// labelRetrieving
			// 
			this.labelRetrieving.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelRetrieving.Location = new System.Drawing.Point(12,72);
			this.labelRetrieving.Name = "labelRetrieving";
			this.labelRetrieving.Size = new System.Drawing.Size(366,20);
			this.labelRetrieving.TabIndex = 1;
			this.labelRetrieving.Text = "Retrieving reports from selected clearinghouse.";
			this.labelRetrieving.Visible = false;
			// 
			// comboClearhouse
			// 
			this.comboClearhouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClearhouse.Location = new System.Drawing.Point(18,32);
			this.comboClearhouse.Name = "comboClearhouse";
			this.comboClearhouse.Size = new System.Drawing.Size(187,21);
			this.comboClearhouse.TabIndex = 2;
			// 
			// butRetrieve
			// 
			this.butRetrieve.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butRetrieve.Autosize = true;
			this.butRetrieve.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRetrieve.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRetrieve.CornerRadius = 4F;
			this.butRetrieve.Location = new System.Drawing.Point(222,29);
			this.butRetrieve.Name = "butRetrieve";
			this.butRetrieve.Size = new System.Drawing.Size(90,26);
			this.butRetrieve.TabIndex = 5;
			this.butRetrieve.Text = "Retrieve";
			this.butRetrieve.Click += new System.EventHandler(this.butRetrieve_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(289,152);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75,26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormClaimReports
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(401,202);
			this.Controls.Add(this.butRetrieve);
			this.Controls.Add(this.comboClearhouse);
			this.Controls.Add(this.labelRetrieving);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClaimReports";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "E-claim Reports";
			this.Load += new System.EventHandler(this.FormClaimReports_Load);
			this.Shown += new System.EventHandler(this.FormClaimReports_Shown);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClaimReports_Load(object sender, System.EventArgs e) {
			_listHqClearinghouses=Clearinghouses.GetDeepCopy();
			for(int i=0;i<_listHqClearinghouses.Count;i++){
				comboClearhouse.Items.Add(_listHqClearinghouses[i].Description);
				if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==_listHqClearinghouses[i].ClearinghouseNum){
					comboClearhouse.SelectedIndex=i;
				}
			}
			if(comboClearhouse.Items.Count>0 && comboClearhouse.SelectedIndex==-1){
				comboClearhouse.SelectedIndex=0;
			}
		}

		private void FormClaimReports_Shown(object sender,EventArgs e) {
			if(AutomaticMode) {
				labelRetrieving.Visible=true;
				Cursor=Cursors.WaitCursor;
				Clearinghouse clearinghouseHq=_listHqClearinghouses[comboClearhouse.SelectedIndex];
				Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
				string errorMessage=Clearinghouses.RetrieveAndImport(clearinghouseClin,AutomaticMode);
				if(errorMessage!="") {
					MessageBox.Show(errorMessage);
				}
				Cursor=Cursors.Default;
				Close();
			}
		}

		private void butRetrieve_Click(object sender,EventArgs e) {
			if(comboClearhouse.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a clearinghouse first.");
				return;
			}
			if(!MsgBox.Show(this,true,"Connect to clearinghouse to retrieve reports?")) {
				return;
			}
			Clearinghouse clearhouseHq=_listHqClearinghouses[comboClearhouse.SelectedIndex];
			Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearhouseHq,Clinics.ClinicNum);
			if(!Directory.Exists(clearinghouseClin.ResponsePath)) {
				MsgBox.Show(this,"Clearinghouse does not have a valid Report Path set.");
				return;
			}
			ODProgressExtended progressbar=new ODProgressExtended(ClearinghouseEvent.Name,new ClearinghouseEvent(),this
				,new ProgressBarHelper((Lans.g(this,"Clearinghouse Progress")),progressBarEventType:ProgBarEventType.Header),lanThis: this.Name);
			//For Tesia, user wouldn't normally manually retrieve.
			if(clearhouseHq.ISA08=="113504607") {
				if((PrefC.RandomKeys && !PrefC.GetBool(PrefName.EasyNoClinics))//See FormClaimsSend_Load
					|| PrefC.GetLong(PrefName.ClearinghouseDefaultDent)!=clearhouseHq.ClearinghouseNum) //default
				{
					//But they might need to in these situations.
					string errorMessage=Clearinghouses.RetrieveAndImport(clearinghouseClin,false,progressbar);
					progressbar.UpdateProgressDetailed("",tagString:"reports",percentVal:"100%",barVal:100);
					if(errorMessage=="") {
						progressbar.UpdateProgress(Lan.g(this,"Retrieval and import successful"));
					}
					else {
						progressbar.UpdateProgress(errorMessage);
					}
					progressbar.UpdateProgress(Lan.g(this,"Done"));
				}
				else{
					progressbar.UpdateProgress(Lan.g(this,"No need to retrieve. Available reports are automatically downloaded every three minutes."));
				}
				progressbar.OnProgressDone();
				return;
			}
			if(clearhouseHq.CommBridge==EclaimsCommBridge.None || clearhouseHq.CommBridge==EclaimsCommBridge.Renaissance 
				|| clearhouseHq.CommBridge==EclaimsCommBridge.RECS) 
			{
				progressbar.UpdateProgress(Lan.g(this,"No built-in functionality for retrieving reports from this clearinghouse."));
				progressbar.OnProgressDone();
				return;
			}
			labelRetrieving.Visible=true;
			string errorMesssage=Clearinghouses.RetrieveAndImport(clearinghouseClin,false,progressbar);
			progressbar.UpdateProgressDetailed("",tagString:"reports",percentVal:"100%",barVal:100);
			if(clearhouseHq.CommBridge==EclaimsCommBridge.ClaimConnect && errorMesssage=="" && Directory.Exists(clearinghouseClin.ResponsePath)) {
				//Since the dentalxchange website was successfully launched in this scenario, the user does not need any further notification.
			}
			else if(errorMesssage=="") {
				progressbar.UpdateProgress(Lan.g(this,"Retrieve and import successful."));
			}
			else {
				progressbar.UpdateProgress(Lans.g(progressbar.LanThis,"Error Log:")+"\r\n"+errorMesssage);
			}
			labelRetrieving.Visible=false;
			progressbar.OnProgressDone();
			if(progressbar.IsCanceled) {//close
				progressbar.Close();
			}
		}


		/*private void listMain_DoubleClick(object sender, System.EventArgs e) {
			if(listMain.SelectedIndices.Count==0){
				return;
			}
			string messageText=File.ReadAllText((string)listMain.SelectedItem);
			if(X12object.IsX12(messageText)){
				X12object xobj=new X12object(messageText);
				if(X277U.Is277U(xobj)){
					MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(X277U.MakeHumanReadable(xobj));
					msgbox.ShowDialog();
				}
				else if(X997.Is997(xobj)) {
					//MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(X997.MakeHumanReadable(xobj));
					//msgbox.ShowDialog();
				}
			}
			else{
				MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(messageText);
				msgbox.ShowDialog();
			}
			
			//if the file is an X12 file (277 for now), then display it differently
			if(Path.GetExtension((string)listMain.SelectedItem)==".txt"){
				//List<string> messageLines=new List<string>();
				//X12object xObj=new X12object(File.ReadAllText(fileName));
				string firstLine="";
				using(StreamReader sr=new StreamReader((string)listMain.SelectedItem)){
					firstLine=sr.ReadLine();
				}
				if(firstLine!=null && firstLine.Length==106 && firstLine.Substring(0,3)=="ISA"){
					//try{
						string humanText=X277U.MakeHumanReadable((string)listMain.SelectedItem);
						ArchiveFile((string)listMain.SelectedItem);
						//now the file will be gone
						//create a new file from humanText with same name as original file
						StreamWriter sw=File.CreateText((string)listMain.SelectedItem);
						sw.Write(humanText);
						sw.Close();
						//now, it will try to launch the new text file
					//}
					//catch(Exception ex){
					//	MessageBox.Show(ex.Message);
					//	return;
					//}
				}
			}
			try{
				Process.Start((string)listMain.SelectedItem);
			}
			catch{
				MsgBox.Show(this,"Could not open the item. You could try open it directly from the folder where it is located.");
			}
			//FillGrid();
		}*/

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

	

		

		

		

		


	}
}






















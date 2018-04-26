using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormSigElementDefEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private Label label1;
		private ListBox listType;
		private Label label2;
		private TextBox textSigText;
		private Label label3;
		private Label label5;
		private Label label6;
		private ValidNum textLightRow;
		private Button butColor;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Label label7;
		///<summary>Required to be set before opening this form.</summary>
		public SigElementDef ElementCur;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butPlay;
		private OpenDental.UI.Button butImport;
		private GroupBox groupBox1;
		private OpenDental.UI.Button butRecord;
		private OpenDental.UI.Button butExport;
		private OpenFileDialog openFileDialog1;
		private SaveFileDialog saveFileDialog1;
		private OpenDental.UI.Button butDeleteSound;
		private UI.Button butRecordNew;
		///<summary></summary>
		public bool IsNew;

		///<summary></summary>
		public FormSigElementDefEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSigElementDefEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.listType = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textSigText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.butColor = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butRecordNew = new OpenDental.UI.Button();
			this.butDeleteSound = new OpenDental.UI.Button();
			this.butRecord = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butPlay = new OpenDental.UI.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.butDelete = new OpenDental.UI.Button();
			this.textLightRow = new OpenDental.ValidNum();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.label1.Location = new System.Drawing.Point(63, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Type";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listType
			// 
			this.listType.FormattingEnabled = true;
			this.listType.Location = new System.Drawing.Point(168, 12);
			this.listType.Name = "listType";
			this.listType.Size = new System.Drawing.Size(106, 43);
			this.listType.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.label2.Location = new System.Drawing.Point(63, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "Text";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textSigText
			// 
			this.textSigText.Location = new System.Drawing.Point(169, 67);
			this.textSigText.Name = "textSigText";
			this.textSigText.Size = new System.Drawing.Size(105, 20);
			this.textSigText.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.label3.Location = new System.Drawing.Point(281, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(304, 40);
			this.label3.TabIndex = 6;
			this.label3.Text = "This is the text as it should show in the list or the username.  Typed messages a" +
    "re handled separately.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.label5.Location = new System.Drawing.Point(62, 253);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 20);
			this.label5.TabIndex = 8;
			this.label5.Text = "Light Row";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.label6.Location = new System.Drawing.Point(62, 293);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 20);
			this.label6.TabIndex = 9;
			this.label6.Text = "Light Color";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(168, 289);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 11;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// label7
			// 
			this.label7.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.label7.Location = new System.Drawing.Point(281, 275);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(278, 73);
			this.label7.TabIndex = 12;
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butRecordNew);
			this.groupBox1.Controls.Add(this.butDeleteSound);
			this.groupBox1.Controls.Add(this.butRecord);
			this.groupBox1.Controls.Add(this.butExport);
			this.groupBox1.Controls.Add(this.butImport);
			this.groupBox1.Controls.Add(this.butPlay);
			this.groupBox1.Location = new System.Drawing.Point(121, 93);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(464, 141);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sound";
			// 
			// butRecordNew
			// 
			this.butRecordNew.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecordNew.Autosize = true;
			this.butRecordNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecordNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecordNew.CornerRadius = 4F;
			this.butRecordNew.Location = new System.Drawing.Point(142, 50);
			this.butRecordNew.Name = "butRecordNew";
			this.butRecordNew.Size = new System.Drawing.Size(105, 23);
			this.butRecordNew.TabIndex = 20;
			this.butRecordNew.Text = "Record New";
			this.butRecordNew.Click += new System.EventHandler(this.butRecordNew_Click);
			// 
			// butDeleteSound
			// 
			this.butDeleteSound.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeleteSound.Autosize = true;
			this.butDeleteSound.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeleteSound.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeleteSound.CornerRadius = 4F;
			this.butDeleteSound.Location = new System.Drawing.Point(47, 49);
			this.butDeleteSound.Name = "butDeleteSound";
			this.butDeleteSound.Size = new System.Drawing.Size(55, 23);
			this.butDeleteSound.TabIndex = 19;
			this.butDeleteSound.Text = "Delete";
			this.butDeleteSound.Click += new System.EventHandler(this.butDeleteSound_Click);
			// 
			// butRecord
			// 
			this.butRecord.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecord.Autosize = true;
			this.butRecord.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecord.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecord.CornerRadius = 4F;
			this.butRecord.Location = new System.Drawing.Point(142, 21);
			this.butRecord.Name = "butRecord";
			this.butRecord.Size = new System.Drawing.Size(105, 23);
			this.butRecord.TabIndex = 18;
			this.butRecord.Text = "Windows Recorder";
			this.butRecord.Click += new System.EventHandler(this.butRecord_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.Location = new System.Drawing.Point(47, 107);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(55, 23);
			this.butExport.TabIndex = 17;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butImport
			// 
			this.butImport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImport.Autosize = true;
			this.butImport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImport.CornerRadius = 4F;
			this.butImport.Location = new System.Drawing.Point(47, 78);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(55, 23);
			this.butImport.TabIndex = 16;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butPlay
			// 
			this.butPlay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPlay.Autosize = true;
			this.butPlay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPlay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPlay.CornerRadius = 4F;
			this.butPlay.Location = new System.Drawing.Point(47, 20);
			this.butPlay.Name = "butPlay";
			this.butPlay.Size = new System.Drawing.Size(55, 23);
			this.butPlay.TabIndex = 15;
			this.butPlay.Text = "Play";
			this.butPlay.Click += new System.EventHandler(this.butPlay_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(46, 351);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(82, 26);
			this.butDelete.TabIndex = 14;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textLightRow
			// 
			this.textLightRow.Location = new System.Drawing.Point(168, 250);
			this.textLightRow.MaxVal = 255;
			this.textLightRow.MinVal = 0;
			this.textLightRow.Name = "textLightRow";
			this.textLightRow.Size = new System.Drawing.Size(51, 20);
			this.textLightRow.TabIndex = 1;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(442, 351);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 2;
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
			this.butCancel.Location = new System.Drawing.Point(544, 351);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormSigElementDefEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(671, 402);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.textLightRow);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textSigText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSigElementDefEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Message Element";
			this.Load += new System.EventHandler(this.FormSigElementDefEdit_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormSigElementDefEdit_Load(object sender,EventArgs e) {
			listType.Items.Clear();
			listType.Items.Add(Lan.g("enumSignalElementType","User"));
			listType.Items.Add(Lan.g("enumSignalElementType","Extra"));
			listType.Items.Add(Lan.g("enumSignalElementType","Message"));
			listType.SelectedIndex=(int)ElementCur.SigElementType;
			textSigText.Text=ElementCur.SigText;
			SetSoundButtons();
			textLightRow.Text=ElementCur.LightRow.ToString();
			butColor.BackColor=ElementCur.LightColor;
		}

		private void SetSoundButtons(){
			if(String.IsNullOrEmpty(ElementCur.Sound)){
				butPlay.Enabled=false;
				butExport.Enabled=false;
			}
			else{
				butPlay.Enabled=true;
				butExport.Enabled=true;
			}
		}

		private void butPlay_Click(object sender,EventArgs e) {
			try {
				byte[] rawData=Convert.FromBase64String(ElementCur.Sound);
				MemoryStream stream=new MemoryStream(rawData);
				SoundPlayer simpleSound = new SoundPlayer(stream);
				simpleSound.Play();
			}
			catch {
				MsgBox.Show(this,"Invalid sound");
			}
		}

		private void butDeleteSound_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"Delete sound?")){
				return;
			}
			ElementCur.Sound="";
			SetSoundButtons();
		}

		private void butRecord_Click(object sender,EventArgs e) {
			//The following article was used to figure out how to launch the appropriate executable:
			//http://blogs.microsoft.co.il/blogs/tamir/archive/2007/12/04/seek-and-hide-x64-or-where-my-sound-recoder.aspx
			try{
				//Try to launch the sound recorder program within the Windows operating system
				//for all versions of Windows prior to Windows Vista.
				Process.Start("sndrec32.exe");
			}
			catch{
				//We are on a Windows Vista or later Operating System.
				//The path to the SoundRecorder.exe changes depending on if the Operating System
				//is 32 bit or 64 bit.
				try{
					//First try to launch the SoundRecorder.exe for 32 bit Operating Systems.
					Process.Start("SoundRecorder.exe","/file outputfile.wav");
				}
				catch{
					//This is a 64 bit Operating System. A special environment variable path must be used to indirectly access
					//the SoundRecoder.exe file. The resulting path inside of the soundRecoderVirtualPath variable will only
					//exist inside of this program and does not actually exist if one tries to browse to it.
					string soundRecorderVirtualPath=Environment.ExpandEnvironmentVariables(@"%systemroot%\Sysnative")+"\\SoundRecorder.exe";
					try {
						Process.Start(soundRecorderVirtualPath,"/file outputfile.wav");
					}
					catch {
						//Windows 10 does not have this sound recorder program anymore.
						MsgBox.Show(this,"Cannot find Windows Sound Recorder. Use the 'Record New' button to record a message sound.");
					}
				}
			}
		}

		private void butImport_Click(object sender,EventArgs e) {
			openFileDialog1.FileName="";
			openFileDialog1.DefaultExt="wav";
			if(openFileDialog1.ShowDialog() !=DialogResult.OK){
				return;
			}
			try{
				ElementCur.Sound=POut.Sound(openFileDialog1.FileName);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			SetSoundButtons();
		}

		private void butExport_Click(object sender,EventArgs e) {
			saveFileDialog1.FileName="";
			saveFileDialog1.DefaultExt="wav";
			if(saveFileDialog1.ShowDialog() !=DialogResult.OK) {
				return;
			}
			try {
				PIn.Sound(ElementCur.Sound,saveFileDialog1.FileName);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butColor_Click(object sender,EventArgs e) {
			ColorDialog colorDialog1=new ColorDialog();
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
			}
			else {
				if(!MsgBox.Show(this,true,"Delete?")) {
					return;
				}
				SigElementDefs.Delete(ElementCur);
				DialogResult=DialogResult.OK;
			}
		}

		private void butRecordNew_Click(object sender,EventArgs e) {
			FormRecordAudio FormRA=new FormRecordAudio();
			if(FormRA.ShowDialog()==DialogResult.OK) {
				ElementCur.Sound=FormRA.Sound;
			}
			SetSoundButtons();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textLightRow.errorProvider1.GetError(textLightRow)!=""
				) {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textSigText.Text==""){
				MsgBox.Show(this,"Please enter a text description first.");
				return;
			}
			ElementCur.SigElementType=(SignalElementType)listType.SelectedIndex;
			ElementCur.SigText=textSigText.Text;
			ElementCur.LightRow=PIn.Byte(textLightRow.Text);
			ElementCur.LightColor=butColor.BackColor;
			if(IsNew){
				SigElementDefs.Insert(ElementCur);
			}
			else{
				SigElementDefs.Update(ElementCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		

		


		




	}
}






















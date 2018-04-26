using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormScheduleBlockEdit : ODForm	{
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private OpenDental.ODtextBox textNote;
		private System.Windows.Forms.Label label4;
		private OpenDental.UI.Button butDelete;
		///<summary></summary>
    public bool IsNew;
		private System.Windows.Forms.ListBox listOp;
		private System.Windows.Forms.Label labelOp;
		private System.Windows.Forms.ListBox listType;
		private System.Windows.Forms.Label labelType;
		private ComboBox comboStart;
		private ComboBox comboStop;
		private Schedule _schedCur;
		private long _clinicNum;
		private List<Operatory> _listOps;
		private List<Def> _listBlockoutCatDefs;

		///<summary>Setting clinicNum to 0 will show all operatories, otherwise only operatories for the clinic passed in will show.  
		///If a list of defs is passed in it will fill the blockout type select box.  If no list is passed in, it will show all defs.</summary>
		public FormScheduleBlockEdit(Schedule schedCur, long clinicNum, List<Def> listDefsToShow=null) {
			InitializeComponent();
			_schedCur=schedCur;
			_clinicNum=clinicNum;
			_listBlockoutCatDefs=listDefsToShow;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScheduleBlockEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textNote = new OpenDental.ODtextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.listOp = new System.Windows.Forms.ListBox();
			this.labelOp = new System.Windows.Forms.Label();
			this.listType = new System.Windows.Forms.ListBox();
			this.labelType = new System.Windows.Forms.Label();
			this.comboStart = new System.Windows.Forms.ComboBox();
			this.comboStop = new System.Windows.Forms.ComboBox();
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
			this.butCancel.Location = new System.Drawing.Point(539, 382);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 14;
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
			this.butOK.Location = new System.Drawing.Point(539, 348);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 12;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(5, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Stop Time";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Start Time";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(75, 64);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Schedule;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(220, 113);
			this.textNote.TabIndex = 15;
			this.textNote.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(9, 65);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 16);
			this.label4.TabIndex = 16;
			this.label4.Text = "Note";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
			this.butDelete.Location = new System.Drawing.Point(18, 382);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 26);
			this.butDelete.TabIndex = 17;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// listOp
			// 
			this.listOp.Location = new System.Drawing.Point(443, 43);
			this.listOp.Name = "listOp";
			this.listOp.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listOp.Size = new System.Drawing.Size(171, 290);
			this.listOp.TabIndex = 21;
			// 
			// labelOp
			// 
			this.labelOp.Location = new System.Drawing.Point(443, 23);
			this.labelOp.Name = "labelOp";
			this.labelOp.Size = new System.Drawing.Size(128, 16);
			this.labelOp.TabIndex = 20;
			this.labelOp.Text = "Operatories";
			this.labelOp.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listType
			// 
			this.listType.Location = new System.Drawing.Point(313, 43);
			this.listType.Name = "listType";
			this.listType.Size = new System.Drawing.Size(115, 134);
			this.listType.TabIndex = 19;
			// 
			// labelType
			// 
			this.labelType.Location = new System.Drawing.Point(313, 23);
			this.labelType.Name = "labelType";
			this.labelType.Size = new System.Drawing.Size(127, 16);
			this.labelType.TabIndex = 18;
			this.labelType.Text = "Blockout Type";
			this.labelType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboStart
			// 
			this.comboStart.FormattingEnabled = true;
			this.comboStart.Location = new System.Drawing.Point(75, 11);
			this.comboStart.MaxDropDownItems = 48;
			this.comboStart.Name = "comboStart";
			this.comboStart.Size = new System.Drawing.Size(120, 21);
			this.comboStart.TabIndex = 22;
			// 
			// comboStop
			// 
			this.comboStop.FormattingEnabled = true;
			this.comboStop.Location = new System.Drawing.Point(75, 37);
			this.comboStop.MaxDropDownItems = 48;
			this.comboStop.Name = "comboStop";
			this.comboStop.Size = new System.Drawing.Size(120, 21);
			this.comboStop.TabIndex = 23;
			// 
			// FormScheduleBlockEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(638, 426);
			this.Controls.Add(this.comboStop);
			this.Controls.Add(this.comboStart);
			this.Controls.Add(this.listOp);
			this.Controls.Add(this.labelOp);
			this.Controls.Add(this.listType);
			this.Controls.Add(this.labelType);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormScheduleBlockEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Blockout";
			this.Load += new System.EventHandler(this.FormScheduleBlockEdit_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormScheduleBlockEdit_Load(object sender,System.EventArgs e) {
			listType.Items.Clear();
			//This list will be null if there isn't a passed in list.  We pass in lists if we want to show a special modified list.
			if(_listBlockoutCatDefs==null) {
				_listBlockoutCatDefs=Defs.GetDefsForCategory(DefCat.BlockoutTypes,true);
			}
			for(int i=0;i<_listBlockoutCatDefs.Count;i++){
				listType.Items.Add(_listBlockoutCatDefs[i].ItemName);
				if(_schedCur.BlockoutType==_listBlockoutCatDefs[i].DefNum){
					listType.SelectedIndex=i;
				}
			}
			if(listType.Items.Count==0){
				MsgBox.Show(this,"You must setup blockout types first in Setup-Definitions.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(listType.SelectedIndex==-1){
				listType.SelectedIndex=0;
			}
			listOp.Items.Clear();
			//Filter clinics by the clinic passed in.
			List<Operatory> listOpsShort=Operatories.GetDeepCopy(true);
			_listOps=new List<Operatory>();
			for(int i=0;i<listOpsShort.Count;i++) {
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && _clinicNum!=0) {//Using clinics and a clinic filter was passed in.
					if(listOpsShort[i].ClinicNum!=_clinicNum) {
						continue;
					}
				}
				listOp.Items.Add(listOpsShort[i].OpName);
				_listOps.Add(listOpsShort[i]);
				if(_schedCur.Ops.Contains(listOpsShort[i].OperatoryNum)) {
					listOp.SetSelected(listOp.Items.Count-1,true);//Select the item that was just added.
				}
			}
			DateTime time;
			for(int i=0;i<24;i++){
				time=DateTime.Today+TimeSpan.FromHours(7)+TimeSpan.FromMinutes(30*i);
				comboStart.Items.Add(time.ToShortTimeString());
				comboStop.Items.Add(time.ToShortTimeString());
			}
			comboStart.Text=_schedCur.StartTime.ToShortTimeString();
			comboStop.Text=_schedCur.StopTime.ToShortTimeString();
			textNote.Text=_schedCur.Note;
			comboStart.Select();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(MessageBox.Show(Lan.g(this,"Delete Blockout?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
			  return;   
			}
      if(IsNew){
        DialogResult=DialogResult.Cancel; 
      }
      else{ 
        Schedules.Delete(_schedCur);
				SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout delete.");
			}
      DialogResult=DialogResult.Cancel;
		}

    private void butOK_Click(object sender, System.EventArgs e) { 
			if(listOp.SelectedIndices.Count==0){
				MsgBox.Show(this,"Please select at least one operatory first.");
				return;
			}
		  try{
				_schedCur.StartTime=DateTime.Parse(comboStart.Text).TimeOfDay;
				_schedCur.StopTime=DateTime.Parse(comboStop.Text).TimeOfDay;
			}
			catch{
				MsgBox.Show(this,"Incorrect time format");
				return;
			}
      _schedCur.Note=textNote.Text;
			_schedCur.BlockoutType=_listBlockoutCatDefs[listType.SelectedIndex].DefNum;
			_schedCur.Ops=new List<long>();
			for(int i=0;i<listOp.SelectedIndices.Count;i++){
				_schedCur.Ops.Add(_listOps[listOp.SelectedIndices[i]].OperatoryNum);
			}
			if(Schedules.Overlaps(_schedCur)) {
				MsgBox.Show(this,"Blockouts not allowed to overlap.");
				return;
			}
			try{
				if(IsNew) {
					Schedules.Insert(_schedCur,true);
					SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout add.");
				}
				else {
					Schedules.Update(_schedCur);
					SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout edit.");
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;		  
    }

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}   

	}
}







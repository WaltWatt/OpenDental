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
	public class FormTimeAdjustEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private Label label1;
		private Label label2;
		private TextBox textTimeEntry;
		private Label label4;
		private TextBox textNote;
		private CheckBox checkOvertime;
		private OpenDental.UI.Button butDelete;
		private TextBox textHours;
		private Label label3;
		private RadioButton radioAuto;
		private RadioButton radioManual;
		private Label label5;
		private TimeAdjust TimeAdjustCur;

		///<summary></summary>
		public FormTimeAdjustEdit(TimeAdjust timeAdjustCur){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			TimeAdjustCur=timeAdjustCur.Copy();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeAdjustEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textTimeEntry = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.checkOvertime = new System.Windows.Forms.CheckBox();
			this.textHours = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.radioAuto = new System.Windows.Forms.RadioButton();
			this.radioManual = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11,49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(126,20);
			this.label1.TabIndex = 11;
			this.label1.Text = "Date/Time Entry";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(11,76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126,20);
			this.label2.TabIndex = 13;
			this.label2.Text = "Hours";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTimeEntry
			// 
			this.textTimeEntry.Location = new System.Drawing.Point(137,50);
			this.textTimeEntry.Name = "textTimeEntry";
			this.textTimeEntry.Size = new System.Drawing.Size(155,20);
			this.textTimeEntry.TabIndex = 17;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(10,123);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(126,20);
			this.label4.TabIndex = 18;
			this.label4.Text = "Note";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(137,124);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(377,96);
			this.textNote.TabIndex = 21;
			// 
			// checkOvertime
			// 
			this.checkOvertime.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOvertime.Location = new System.Drawing.Point(12,103);
			this.checkOvertime.Name = "checkOvertime";
			this.checkOvertime.Size = new System.Drawing.Size(139,17);
			this.checkOvertime.TabIndex = 22;
			this.checkOvertime.Text = "Overtime Adjustment";
			this.checkOvertime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOvertime.UseVisualStyleBackColor = true;
			// 
			// textHours
			// 
			this.textHours.Location = new System.Drawing.Point(137,77);
			this.textHours.Name = "textHours";
			this.textHours.Size = new System.Drawing.Size(68,20);
			this.textHours.TabIndex = 23;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(152,101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(300,18);
			this.label3.TabIndex = 24;
			this.label3.Text = "(the hours will be shifted from regular time to overtime)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(37,269);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(79,24);
			this.butDelete.TabIndex = 16;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(439,237);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,24);
			this.butOK.TabIndex = 8;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(439,269);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,24);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// radioAuto
			// 
			this.radioAuto.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAuto.Location = new System.Drawing.Point(12,10);
			this.radioAuto.Name = "radioAuto";
			this.radioAuto.Size = new System.Drawing.Size(139,18);
			this.radioAuto.TabIndex = 25;
			this.radioAuto.TabStop = true;
			this.radioAuto.Text = "Automatically entered";
			this.radioAuto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAuto.UseVisualStyleBackColor = true;
			// 
			// radioManual
			// 
			this.radioManual.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioManual.Location = new System.Drawing.Point(12,27);
			this.radioManual.Name = "radioManual";
			this.radioManual.Size = new System.Drawing.Size(139,18);
			this.radioManual.TabIndex = 26;
			this.radioManual.TabStop = true;
			this.radioManual.Text = "Manually entered";
			this.radioManual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioManual.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(152,27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(170,18);
			this.label5.TabIndex = 27;
			this.label5.Text = "(protected from auto deletion)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormTimeAdjustEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(540,313);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.radioManual);
			this.Controls.Add(this.radioAuto);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textHours);
			this.Controls.Add(this.checkOvertime);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textTimeEntry);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTimeAdjustEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Time Adjustment";
			this.Load += new System.EventHandler(this.FormTimeAdjustEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTimeAdjustEdit_Load(object sender, System.EventArgs e) {
			if(TimeAdjustCur.IsAuto) {
				radioAuto.Checked=true;
			}
			else {
				radioManual.Checked=true;
			}
			textTimeEntry.Text=TimeAdjustCur.TimeEntry.ToString();
			if(TimeAdjustCur.OTimeHours.TotalHours==0){
				textHours.Text=ClockEvents.Format(TimeAdjustCur.RegHours);
			}
			else{
				checkOvertime.Checked=true;
				textHours.Text=ClockEvents.Format(TimeAdjustCur.OTimeHours);
			}
			textNote.Text=TimeAdjustCur.Note;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			TimeAdjusts.Delete(TimeAdjustCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			try {
				DateTime.Parse(textTimeEntry.Text);
			}
			catch {
				MsgBox.Show(this,"Please enter a valid Date/Time.");
				return;
			}
			try{
				if(textHours.Text.Contains(":")){
					ClockEvents.ParseHours(textHours.Text);
				}
				else{
					Double.Parse(textHours.Text);
				}
			}
			catch{
				MsgBox.Show(this,"Please enter valid Hours and Minutes.");
				return;
			}
			//end of validation
			TimeAdjustCur.IsAuto=radioAuto.Checked;
			TimeAdjustCur.TimeEntry=DateTime.Parse(textTimeEntry.Text);
			TimeSpan hoursEntered;
			if(textHours.Text.Contains(":")){
				hoursEntered=ClockEvents.ParseHours(textHours.Text);//we know this will work because we tested ParseHours above.
			}
			else{
				hoursEntered=TimeSpan.FromHours(Double.Parse(textHours.Text));
			}
			if(checkOvertime.Checked){
				TimeAdjustCur.RegHours=-hoursEntered;
				TimeAdjustCur.OTimeHours=hoursEntered;
			}
			else{
				TimeAdjustCur.RegHours=hoursEntered;
				TimeAdjustCur.OTimeHours=TimeSpan.FromHours(0);
			}
			TimeAdjustCur.Note=textNote.Text;
			if(IsNew){
				TimeAdjusts.Insert(TimeAdjustCur);
			}
			else{
				TimeAdjusts.Update(TimeAdjustCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	

	

		

		

		


	}
}






















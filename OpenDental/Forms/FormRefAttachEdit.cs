using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormRefAttachEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label labelRefDate;
		///<summary></summary>
    public bool IsNew;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.Label labelRefType;
		private OpenDental.ValidNumber textOrder;
		private System.Windows.Forms.Label labelOrder;
		private OpenDental.UI.Button butEditReferral;
		private OpenDental.ValidDate textRefDate;
		private System.Windows.Forms.TextBox textReferralNotes;
		private System.Windows.Forms.Label labelPatient;
		private System.Windows.Forms.Label labelReferralNotes;
		private System.ComponentModel.Container components = null;
		private Label labelNote;
		private TextBox textNote;
		private ComboBox comboRefToStatus;
		private Label labelRefToStatus;
		private OpenDental.UI.Button butDetach;
		private ListBox listSheets;
		///<summary></summary>
		public RefAttach RefAttachCur;
		private Label labelSheets;
		private ListBox listRefType;
		private CheckBox checkIsTransitionOfCare;
		private Label labelIsTransitionOfCare;
		private TextBox textProc;
		private Label labelProc;
		private ValidDate textDateProcCompleted;
		private Label labelDateProcComplete;
		private UI.Button butChangeReferral;
		private UI.Button butNoneProv;
		private ComboBox comboProvNum;
		private UI.Button butPickProv;
		private Label labelProv;
		///<summary>List of referral slips for this pat/ref combo.</summary>
		private List<Sheet> SheetList; 
		///<summary>Select a referring provider for referals to other providers.</summary>
		private long _provNumSelected;
		private RefAttach _refAttachOld;
		private List<Provider> _listProviders;

		///<summary></summary>
		public FormRefAttachEdit(){
			InitializeComponent();
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

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRefAttachEdit));
			this.labelRefDate = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.labelRefType = new System.Windows.Forms.Label();
			this.labelOrder = new System.Windows.Forms.Label();
			this.textReferralNotes = new System.Windows.Forms.TextBox();
			this.labelPatient = new System.Windows.Forms.Label();
			this.labelReferralNotes = new System.Windows.Forms.Label();
			this.labelNote = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.comboRefToStatus = new System.Windows.Forms.ComboBox();
			this.labelRefToStatus = new System.Windows.Forms.Label();
			this.listSheets = new System.Windows.Forms.ListBox();
			this.labelSheets = new System.Windows.Forms.Label();
			this.listRefType = new System.Windows.Forms.ListBox();
			this.checkIsTransitionOfCare = new System.Windows.Forms.CheckBox();
			this.labelIsTransitionOfCare = new System.Windows.Forms.Label();
			this.textProc = new System.Windows.Forms.TextBox();
			this.labelProc = new System.Windows.Forms.Label();
			this.labelDateProcComplete = new System.Windows.Forms.Label();
			this.comboProvNum = new System.Windows.Forms.ComboBox();
			this.labelProv = new System.Windows.Forms.Label();
			this.butNoneProv = new OpenDental.UI.Button();
			this.butPickProv = new OpenDental.UI.Button();
			this.textDateProcCompleted = new OpenDental.ValidDate();
			this.butDetach = new OpenDental.UI.Button();
			this.textRefDate = new OpenDental.ValidDate();
			this.butChangeReferral = new OpenDental.UI.Button();
			this.butEditReferral = new OpenDental.UI.Button();
			this.textOrder = new OpenDental.ValidNumber();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// labelRefDate
			// 
			this.labelRefDate.Location = new System.Drawing.Point(6, 188);
			this.labelRefDate.Name = "labelRefDate";
			this.labelRefDate.Size = new System.Drawing.Size(143, 17);
			this.labelRefDate.TabIndex = 16;
			this.labelRefDate.Text = "Date";
			this.labelRefDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(6, 58);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(143, 17);
			this.labelName.TabIndex = 17;
			this.labelName.Text = "Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(151, 56);
			this.textName.Name = "textName";
			this.textName.ReadOnly = true;
			this.textName.Size = new System.Drawing.Size(258, 20);
			this.textName.TabIndex = 1;
			// 
			// labelRefType
			// 
			this.labelRefType.Location = new System.Drawing.Point(6, 14);
			this.labelRefType.Name = "labelRefType";
			this.labelRefType.Size = new System.Drawing.Size(143, 17);
			this.labelRefType.TabIndex = 20;
			this.labelRefType.Text = "Referral Type";
			this.labelRefType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelOrder
			// 
			this.labelOrder.Location = new System.Drawing.Point(6, 209);
			this.labelOrder.Name = "labelOrder";
			this.labelOrder.Size = new System.Drawing.Size(143, 17);
			this.labelOrder.TabIndex = 73;
			this.labelOrder.Text = "Order";
			this.labelOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textReferralNotes
			// 
			this.textReferralNotes.Location = new System.Drawing.Point(151, 97);
			this.textReferralNotes.Multiline = true;
			this.textReferralNotes.Name = "textReferralNotes";
			this.textReferralNotes.ReadOnly = true;
			this.textReferralNotes.Size = new System.Drawing.Size(454, 66);
			this.textReferralNotes.TabIndex = 78;
			// 
			// labelPatient
			// 
			this.labelPatient.Location = new System.Drawing.Point(150, 78);
			this.labelPatient.Name = "labelPatient";
			this.labelPatient.Size = new System.Drawing.Size(98, 17);
			this.labelPatient.TabIndex = 80;
			this.labelPatient.Text = "(a patient)";
			// 
			// labelReferralNotes
			// 
			this.labelReferralNotes.Location = new System.Drawing.Point(6, 99);
			this.labelReferralNotes.Name = "labelReferralNotes";
			this.labelReferralNotes.Size = new System.Drawing.Size(143, 38);
			this.labelReferralNotes.TabIndex = 81;
			this.labelReferralNotes.Text = "Notes about referral source";
			this.labelReferralNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelNote
			// 
			this.labelNote.Location = new System.Drawing.Point(6, 252);
			this.labelNote.Name = "labelNote";
			this.labelNote.Size = new System.Drawing.Size(143, 38);
			this.labelNote.TabIndex = 83;
			this.labelNote.Text = "Patient note";
			this.labelNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(151, 250);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(454, 66);
			this.textNote.TabIndex = 1;
			// 
			// comboRefToStatus
			// 
			this.comboRefToStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRefToStatus.FormattingEnabled = true;
			this.comboRefToStatus.Location = new System.Drawing.Point(151, 228);
			this.comboRefToStatus.MaxDropDownItems = 20;
			this.comboRefToStatus.Name = "comboRefToStatus";
			this.comboRefToStatus.Size = new System.Drawing.Size(180, 21);
			this.comboRefToStatus.TabIndex = 84;
			// 
			// labelRefToStatus
			// 
			this.labelRefToStatus.Location = new System.Drawing.Point(6, 230);
			this.labelRefToStatus.Name = "labelRefToStatus";
			this.labelRefToStatus.Size = new System.Drawing.Size(143, 17);
			this.labelRefToStatus.TabIndex = 85;
			this.labelRefToStatus.Text = "Status (if referred out)";
			this.labelRefToStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listSheets
			// 
			this.listSheets.FormattingEnabled = true;
			this.listSheets.Location = new System.Drawing.Point(151, 317);
			this.listSheets.Name = "listSheets";
			this.listSheets.Size = new System.Drawing.Size(120, 69);
			this.listSheets.TabIndex = 90;
			this.listSheets.DoubleClick += new System.EventHandler(this.listSheets_DoubleClick);
			// 
			// labelSheets
			// 
			this.labelSheets.Location = new System.Drawing.Point(6, 319);
			this.labelSheets.Name = "labelSheets";
			this.labelSheets.Size = new System.Drawing.Size(143, 40);
			this.labelSheets.TabIndex = 91;
			this.labelSheets.Text = "Referral Slips\r\n(double click to view)";
			this.labelSheets.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listRefType
			// 
			this.listRefType.FormattingEnabled = true;
			this.listRefType.Location = new System.Drawing.Point(151, 12);
			this.listRefType.Name = "listRefType";
			this.listRefType.Size = new System.Drawing.Size(65, 43);
			this.listRefType.TabIndex = 92;
			this.listRefType.SelectedIndexChanged += new System.EventHandler(this.listRefType_SelectedIndexChanged);
			// 
			// checkIsTransitionOfCare
			// 
			this.checkIsTransitionOfCare.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsTransitionOfCare.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsTransitionOfCare.Location = new System.Drawing.Point(6, 387);
			this.checkIsTransitionOfCare.Name = "checkIsTransitionOfCare";
			this.checkIsTransitionOfCare.Size = new System.Drawing.Size(158, 18);
			this.checkIsTransitionOfCare.TabIndex = 93;
			this.checkIsTransitionOfCare.Text = "Transition of Care";
			this.checkIsTransitionOfCare.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsTransitionOfCare.UseVisualStyleBackColor = true;
			// 
			// labelIsTransitionOfCare
			// 
			this.labelIsTransitionOfCare.Location = new System.Drawing.Point(166, 387);
			this.labelIsTransitionOfCare.Name = "labelIsTransitionOfCare";
			this.labelIsTransitionOfCare.Size = new System.Drawing.Size(195, 17);
			this.labelIsTransitionOfCare.TabIndex = 94;
			this.labelIsTransitionOfCare.Text = "(From or To another doctor)";
			this.labelIsTransitionOfCare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textProc
			// 
			this.textProc.BackColor = System.Drawing.SystemColors.Control;
			this.textProc.ForeColor = System.Drawing.Color.DarkRed;
			this.textProc.Location = new System.Drawing.Point(151, 406);
			this.textProc.Name = "textProc";
			this.textProc.ReadOnly = true;
			this.textProc.Size = new System.Drawing.Size(232, 20);
			this.textProc.TabIndex = 171;
			this.textProc.Text = "test";
			// 
			// labelProc
			// 
			this.labelProc.Location = new System.Drawing.Point(6, 408);
			this.labelProc.Name = "labelProc";
			this.labelProc.Size = new System.Drawing.Size(143, 17);
			this.labelProc.TabIndex = 170;
			this.labelProc.Text = "Procedure";
			this.labelProc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDateProcComplete
			// 
			this.labelDateProcComplete.Location = new System.Drawing.Point(6, 429);
			this.labelDateProcComplete.Name = "labelDateProcComplete";
			this.labelDateProcComplete.Size = new System.Drawing.Size(143, 17);
			this.labelDateProcComplete.TabIndex = 172;
			this.labelDateProcComplete.Text = "Date Proc Completed";
			this.labelDateProcComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProvNum
			// 
			this.comboProvNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvNum.Location = new System.Drawing.Point(151, 164);
			this.comboProvNum.MaxDropDownItems = 30;
			this.comboProvNum.Name = "comboProvNum";
			this.comboProvNum.Size = new System.Drawing.Size(258, 21);
			this.comboProvNum.TabIndex = 280;
			this.comboProvNum.SelectionChangeCommitted += new System.EventHandler(this.comboProvNum_SelectionChangeCommitted);
			// 
			// labelProv
			// 
			this.labelProv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProv.Location = new System.Drawing.Point(6, 166);
			this.labelProv.Name = "labelProv";
			this.labelProv.Size = new System.Drawing.Size(143, 17);
			this.labelProv.TabIndex = 279;
			this.labelProv.Text = "Referring Provider";
			this.labelProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butNoneProv
			// 
			this.butNoneProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneProv.Autosize = false;
			this.butNoneProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneProv.CornerRadius = 2F;
			this.butNoneProv.Location = new System.Drawing.Point(433, 164);
			this.butNoneProv.Name = "butNoneProv";
			this.butNoneProv.Size = new System.Drawing.Size(44, 21);
			this.butNoneProv.TabIndex = 282;
			this.butNoneProv.Text = "None";
			this.butNoneProv.Click += new System.EventHandler(this.butNoneProv_Click);
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(412, 164);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 21);
			this.butPickProv.TabIndex = 281;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// textDateProcCompleted
			// 
			this.textDateProcCompleted.Location = new System.Drawing.Point(151, 427);
			this.textDateProcCompleted.Name = "textDateProcCompleted";
			this.textDateProcCompleted.Size = new System.Drawing.Size(100, 20);
			this.textDateProcCompleted.TabIndex = 173;
			// 
			// butDetach
			// 
			this.butDetach.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDetach.Autosize = true;
			this.butDetach.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetach.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetach.CornerRadius = 4F;
			this.butDetach.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDetach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDetach.Location = new System.Drawing.Point(14, 477);
			this.butDetach.Name = "butDetach";
			this.butDetach.Size = new System.Drawing.Size(81, 24);
			this.butDetach.TabIndex = 86;
			this.butDetach.Text = "Detach";
			this.butDetach.Click += new System.EventHandler(this.butDetach_Click);
			// 
			// textRefDate
			// 
			this.textRefDate.Location = new System.Drawing.Point(151, 186);
			this.textRefDate.Name = "textRefDate";
			this.textRefDate.Size = new System.Drawing.Size(100, 20);
			this.textRefDate.TabIndex = 75;
			// 
			// butChangeReferral
			// 
			this.butChangeReferral.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangeReferral.Autosize = true;
			this.butChangeReferral.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangeReferral.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangeReferral.CornerRadius = 4F;
			this.butChangeReferral.Location = new System.Drawing.Point(510, 54);
			this.butChangeReferral.Name = "butChangeReferral";
			this.butChangeReferral.Size = new System.Drawing.Size(95, 24);
			this.butChangeReferral.TabIndex = 74;
			this.butChangeReferral.Text = "Change Referral";
			this.butChangeReferral.Click += new System.EventHandler(this.butChangeReferral_Click);
			// 
			// butEditReferral
			// 
			this.butEditReferral.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditReferral.Autosize = true;
			this.butEditReferral.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditReferral.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditReferral.CornerRadius = 4F;
			this.butEditReferral.Location = new System.Drawing.Point(412, 54);
			this.butEditReferral.Name = "butEditReferral";
			this.butEditReferral.Size = new System.Drawing.Size(95, 24);
			this.butEditReferral.TabIndex = 74;
			this.butEditReferral.Text = "&Edit Referral";
			this.butEditReferral.Click += new System.EventHandler(this.butEditReferral_Click);
			// 
			// textOrder
			// 
			this.textOrder.Location = new System.Drawing.Point(151, 207);
			this.textOrder.MaxVal = 255;
			this.textOrder.MinVal = 0;
			this.textOrder.Name = "textOrder";
			this.textOrder.Size = new System.Drawing.Size(36, 20);
			this.textOrder.TabIndex = 72;
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
			this.butCancel.Location = new System.Drawing.Point(602, 477);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 6;
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
			this.butOK.Location = new System.Drawing.Point(521, 477);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 0;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormRefAttachEdit
			// 
			this.ClientSize = new System.Drawing.Size(689, 513);
			this.Controls.Add(this.butNoneProv);
			this.Controls.Add(this.comboProvNum);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.labelProv);
			this.Controls.Add(this.textDateProcCompleted);
			this.Controls.Add(this.labelDateProcComplete);
			this.Controls.Add(this.textProc);
			this.Controls.Add(this.labelProc);
			this.Controls.Add(this.labelIsTransitionOfCare);
			this.Controls.Add(this.checkIsTransitionOfCare);
			this.Controls.Add(this.listRefType);
			this.Controls.Add(this.labelSheets);
			this.Controls.Add(this.listSheets);
			this.Controls.Add(this.butDetach);
			this.Controls.Add(this.labelRefToStatus);
			this.Controls.Add(this.comboRefToStatus);
			this.Controls.Add(this.labelNote);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.labelReferralNotes);
			this.Controls.Add(this.labelPatient);
			this.Controls.Add(this.textReferralNotes);
			this.Controls.Add(this.textRefDate);
			this.Controls.Add(this.butChangeReferral);
			this.Controls.Add(this.butEditReferral);
			this.Controls.Add(this.textOrder);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.labelOrder);
			this.Controls.Add(this.labelRefType);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.labelRefDate);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRefAttachEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Edit Referral Attachment";
			this.Load += new System.EventHandler(this.FormRefAttachEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRefAttachEdit_Load(object sender,EventArgs e) {
			if(Plugins.HookMethod(this,"FormRefAttachEdit.Load",RefAttachCur,IsNew)) {
				return;
			}
			if(IsNew){
				Text=Lan.g(this,"Add Referral Attachment");
      }
      else{
				_refAttachOld=RefAttachCur.Copy();
				Text=Lan.g(this,"Edit Referral Attachment");
      }
			string referralDescript=DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
				.FirstOrDefault(x => x.InternalName=="Referrals")?.Description;
			if(string.IsNullOrWhiteSpace(referralDescript)) {//either not displaying the Referral field or no description entered, default to 'Referral (other)'
				referralDescript=Lan.g(this,"Referral (other)");
			}
			listRefType.Items.AddRange(new[] { Lan.g(this,"To"),Lan.g(this,"From"),referralDescript });
			FillData();
			FillSheets();
			_provNumSelected=RefAttachCur.ProvNum;
			comboProvNum.Items.Clear();
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				comboProvNum.Items.Add(_listProviders[i].GetLongDesc());//Only visible provs added to combobox.
				if(_listProviders[i].ProvNum==RefAttachCur.ProvNum) {
					comboProvNum.SelectedIndex=i;//Sets combo text too.
				}
			}
			if(comboProvNum.SelectedIndex==-1) {//The provider exists but is hidden
				comboProvNum.Text=Providers.GetLongDesc(_provNumSelected);//Appends "(hidden)" to the end of the long description.
			}
			if(RefAttachCur.RefType==ReferralType.RefFrom) {
				butNoneProv.Visible=false;
				butPickProv.Visible=false;
				comboProvNum.Visible=false;
				labelProv.Visible=false;
			}
			else {
				butNoneProv.Visible=true;
				butPickProv.Visible=true;
				comboProvNum.Visible=true;
				labelProv.Visible=true;
			}
		}

		private void FillData(){
			Referral referral=ReferralL.GetReferral(RefAttachCur.ReferralNum);
			if(referral==null) {
				return;
			}
			textName.Text=referral.GetNameFL();
			labelPatient.Visible=referral.PatNum>0;
			textReferralNotes.Text=referral.Note;
			listRefType.SelectedIndex=(int)RefAttachCur.RefType;
			if(RefAttachCur.RefDate.Year<1880) {
				textRefDate.Text="";
			}
			else{
				textRefDate.Text=RefAttachCur.RefDate.ToShortDateString();
			}
			textOrder.Text=RefAttachCur.ItemOrder.ToString();
			textOrder.ReadOnly=true;//It can be reordered by the Up/Down buttons on FormReferralsPatient.
			comboRefToStatus.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(ReferralToStatus)).Length;i++){
				comboRefToStatus.Items.Add(Lan.g("enumReferralToStatus",Enum.GetNames(typeof(ReferralToStatus))[i]));
				if((int)RefAttachCur.RefToStatus==i){
					comboRefToStatus.SelectedIndex=i;
				}
			}
			textNote.Text=RefAttachCur.Note;
			checkIsTransitionOfCare.Checked=RefAttachCur.IsTransitionOfCare;
			textProc.Text="";
			if(RefAttachCur.ProcNum!=0) {
				Procedure proc=Procedures.GetOneProc(RefAttachCur.ProcNum,false);
				textProc.Text=Procedures.GetDescription(proc);
			}
			if(RefAttachCur.DateProcComplete.Year<1880) {
				textDateProcCompleted.Text="";
			}
			else {
				textDateProcCompleted.Text=RefAttachCur.DateProcComplete.ToShortDateString();
			}
		}

		private void FillSheets(){
			SheetList=Sheets.GetReferralSlips(RefAttachCur.PatNum,RefAttachCur.ReferralNum);
			listSheets.Items.Clear();
			listSheets.Items.AddRange(SheetList.Select(x => x.DateTimeSheet.ToShortDateString()).ToArray());
		}

		private void butEditReferral_Click(object sender,EventArgs e) {
			try{
				DataToCur();
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			Referral referral=ReferralL.GetReferral(RefAttachCur.ReferralNum);
			if(referral==null) {
				return;
			}
			FormReferralEdit FormRE=new FormReferralEdit(referral);
			FormRE.ShowDialog();
			Referrals.RefreshCache();
			FillData();
		}

		private void butChangeReferral_Click(object sender,EventArgs e) {
			FormReferralSelect FormRS=new FormReferralSelect();
			FormRS.IsSelectionMode=true;
			FormRS.ShowDialog();
			if(FormRS.DialogResult!=DialogResult.OK) {
				return;
			}
			RefAttachCur.ReferralNum=FormRS.SelectedReferral.ReferralNum;
			FillData();
		}

		private void listSheets_DoubleClick(object sender,EventArgs e) {
			if(listSheets.SelectedIndex==-1){
				return;
			}
			Sheet sheet=SheetList[listSheets.SelectedIndex];
			SheetFields.GetFieldsAndParameters(sheet);
			FormSheetFillEdit.ShowForm(sheet,delegate { FillSheets(); });
		}

		private void comboProvNum_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumSelected=_listProviders[comboProvNum.SelectedIndex].ProvNum;
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick formP=new FormProviderPick();
			if(comboProvNum.SelectedIndex > -1) {//Initial formP selection if selected prov is not hidden.
				formP.SelectedProvNum=_provNumSelected;
			}
			formP.ShowDialog();
			if(formP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProvNum.SelectedIndex=Providers.GetIndex(formP.SelectedProvNum);
			_provNumSelected=formP.SelectedProvNum;
		}

		private void butNoneProv_Click(object sender,EventArgs e) {
			_provNumSelected=0;
			comboProvNum.SelectedIndex=-1;
		}

		private void listRefType_SelectedIndexChanged(object sender,EventArgs e) {
			//show referring provider only if referring to
			butNoneProv.Visible=((ReferralType)listRefType.SelectedIndex==ReferralType.RefTo);
			butPickProv.Visible=((ReferralType)listRefType.SelectedIndex==ReferralType.RefTo);
			comboProvNum.Visible=((ReferralType)listRefType.SelectedIndex==ReferralType.RefTo);
			labelProv.Visible=((ReferralType)listRefType.SelectedIndex==ReferralType.RefTo);
		}

		///<summary>Surround with try-catch.  Attempts to take the data on the form and set the values of RefAttachCur.</summary>
		private void DataToCur(){
			if(textOrder.errorProvider1.GetError(textOrder)!=""
				|| textRefDate.errorProvider1.GetError(textRefDate)!=""
				|| textDateProcCompleted.errorProvider1.GetError(textDateProcCompleted)!="") 
			{
				throw new ApplicationException(Lan.g(this,"Please fix data entry errors first."));
			}
			RefAttachCur.RefType=(ReferralType)listRefType.SelectedIndex;
			RefAttachCur.ProvNum=(listRefType.SelectedIndex==0?_provNumSelected:0);//If the Referral Type is 'To', use the selected ProvNum.
			//(Optional) Also Set ProvNum on RefType.Other??
			RefAttachCur.RefDate=PIn.Date(textRefDate.Text);
			RefAttachCur.ItemOrder=PIn.Int(textOrder.Text);
			RefAttachCur.RefToStatus=(ReferralToStatus)comboRefToStatus.SelectedIndex;
			RefAttachCur.Note=textNote.Text;
			RefAttachCur.IsTransitionOfCare=checkIsTransitionOfCare.Checked;
			RefAttachCur.DateProcComplete=PIn.Date(textDateProcCompleted.Text);
		}

		private void butDetach_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.RefAttachDelete)) {
				return;
			}
			if(!MsgBox.Show(this,true,"Detach Referral?")) {
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.RefAttachDelete,RefAttachCur.PatNum,"Referral attachment deleted for "+Referrals.GetNameFL(RefAttachCur.ReferralNum));
			RefAttaches.Delete(RefAttachCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//We want to help EHR users meet their summary of care measure.  So all outgoing patient referrals should warn them if they didn't enter data correctly.
			if((ReferralType)listRefType.SelectedIndex==ReferralType.RefTo && PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				string warning="";
				if(comboProvNum.SelectedIndex<0) {
					warning+=Lans.g(this,"Selected patient referral does not have a referring provider set.");
				}
				if(!checkIsTransitionOfCare.Checked) {
					if(warning!="") {
						warning+="\r\n";
					}
					warning+=Lans.g(this,"Selected patient referral is not flagged as a transition of care.");
				}
				if(warning!="") {
					warning+="\r\n"+Lans.g(this,"It will not meet the EHR summary of care requirements.")+"  "+Lans.g(this,"Continue anyway?");
					if(MessageBox.Show(warning,Lans.g(this,"EHR Measure Warning"),MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						return;
					}
				}
			}
			//this is an old pattern
			try{
				DataToCur();
				if(IsNew){
					RefAttaches.Insert(RefAttachCur);
				}
				else{
					RefAttaches.Update(RefAttachCur,_refAttachOld);
				}
			}
			catch(ApplicationException ex){
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









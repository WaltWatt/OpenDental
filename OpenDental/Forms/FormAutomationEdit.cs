using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormAutomationEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private Label label1;
		private TextBox textDescription;
		private Label label2;
		private TextBox textProcCodes;
		private Label labelProcCodes;
		private Label label4;
		private Label labelActionObject;
		private Label labelMessage;
		private TextBox textMessage;
		private ComboBox comboTrigger;
		private ComboBox comboAction;
		private ComboBox comboActionObject;
		private OpenDental.UI.Button butProcCode;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butAdd;
		private Automation AutoCur;
		private List<AutomationCondition> autoList;
		///<summary>List of actions currently in the drop down.  Some actions are only available for specific triggers, so this is possibly a sub-set of
		///all AutomationAction enum values.</summary>
		private List<AutomationAction> _listAutoActions;
		///<summary>Matches list of appointments in comboAppointmentType. Does not include hidden types unless current automation already has that type set.</summary>
		private List<AppointmentType> _listAptTypes;

		///<summary>subset of allowed appointment statuses, in the same order as enum.</summary>
		private List<ApptStatus> _listApptStatuses=new List<ApptStatus> {
			ApptStatus.None,
			//ApptStatus.Scheduled,
			//ApptStatus.Complete,
			//ApptStatus.UnschedList,
			//ApptStatus.ASAP //Deprecated
			//ApptStatus.Broken
		};
		private List<Def> _listCommLogTypeDefs;

		///<summary></summary>
		public FormAutomationEdit(Automation autoCur)
		{
			//
			// Required for Windows Form Designer support
			//
			AutoCur=autoCur.Copy();
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
			OpenDental.UI.Button butDelete;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutomationEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textProcCodes = new System.Windows.Forms.TextBox();
			this.labelProcCodes = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.labelActionObject = new System.Windows.Forms.Label();
			this.labelMessage = new System.Windows.Forms.Label();
			this.textMessage = new System.Windows.Forms.TextBox();
			this.comboTrigger = new System.Windows.Forms.ComboBox();
			this.comboAction = new System.Windows.Forms.ComboBox();
			this.comboActionObject = new System.Windows.Forms.ComboBox();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butProcCode = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			butDelete = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butDelete
			// 
			butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			butDelete.Autosize = true;
			butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			butDelete.CornerRadius = 4F;
			butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			butDelete.Location = new System.Drawing.Point(48, 393);
			butDelete.Name = "butDelete";
			butDelete.Size = new System.Drawing.Size(75, 24);
			butDelete.TabIndex = 16;
			butDelete.Text = "&Delete";
			butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(111, 20);
			this.label1.TabIndex = 11;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(161, 25);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(316, 20);
			this.textDescription.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 20);
			this.label2.TabIndex = 18;
			this.label2.Text = "Trigger";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcCodes
			// 
			this.textProcCodes.Location = new System.Drawing.Point(161, 77);
			this.textProcCodes.Name = "textProcCodes";
			this.textProcCodes.Size = new System.Drawing.Size(316, 20);
			this.textProcCodes.TabIndex = 2;
			// 
			// labelProcCodes
			// 
			this.labelProcCodes.Location = new System.Drawing.Point(13, 76);
			this.labelProcCodes.Name = "labelProcCodes";
			this.labelProcCodes.Size = new System.Drawing.Size(146, 29);
			this.labelProcCodes.TabIndex = 20;
			this.labelProcCodes.Text = "Procedure Code(s)\r\n(separated with commas)";
			this.labelProcCodes.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 256);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(143, 17);
			this.label4.TabIndex = 21;
			this.label4.Text = "Action";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelActionObject
			// 
			this.labelActionObject.Location = new System.Drawing.Point(16, 282);
			this.labelActionObject.Name = "labelActionObject";
			this.labelActionObject.Size = new System.Drawing.Size(143, 17);
			this.labelActionObject.TabIndex = 22;
			this.labelActionObject.Text = "Action Object";
			this.labelActionObject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMessage
			// 
			this.labelMessage.Location = new System.Drawing.Point(16, 307);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(143, 17);
			this.labelMessage.TabIndex = 25;
			this.labelMessage.Text = "Message";
			this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMessage
			// 
			this.textMessage.Location = new System.Drawing.Point(161, 308);
			this.textMessage.Multiline = true;
			this.textMessage.Name = "textMessage";
			this.textMessage.Size = new System.Drawing.Size(316, 73);
			this.textMessage.TabIndex = 26;
			// 
			// comboTrigger
			// 
			this.comboTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTrigger.FormattingEnabled = true;
			this.comboTrigger.Location = new System.Drawing.Point(161, 50);
			this.comboTrigger.Name = "comboTrigger";
			this.comboTrigger.Size = new System.Drawing.Size(183, 21);
			this.comboTrigger.TabIndex = 27;
			this.comboTrigger.SelectedIndexChanged += new System.EventHandler(this.comboTrigger_SelectedIndexChanged);
			// 
			// comboAction
			// 
			this.comboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAction.FormattingEnabled = true;
			this.comboAction.Location = new System.Drawing.Point(161, 255);
			this.comboAction.Name = "comboAction";
			this.comboAction.Size = new System.Drawing.Size(183, 21);
			this.comboAction.TabIndex = 28;
			this.comboAction.SelectedIndexChanged += new System.EventHandler(this.comboAction_SelectedIndexChanged);
			// 
			// comboActionObject
			// 
			this.comboActionObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboActionObject.FormattingEnabled = true;
			this.comboActionObject.Location = new System.Drawing.Point(161, 281);
			this.comboActionObject.Name = "comboActionObject";
			this.comboActionObject.Size = new System.Drawing.Size(183, 21);
			this.comboActionObject.TabIndex = 31;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(677, 225);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(65, 24);
			this.butAdd.TabIndex = 35;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(161, 103);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridMain.Size = new System.Drawing.Size(510, 146);
			this.gridMain.TabIndex = 34;
			this.gridMain.Title = "Conditions";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableConditions";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butProcCode
			// 
			this.butProcCode.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProcCode.Autosize = true;
			this.butProcCode.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProcCode.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProcCode.CornerRadius = 4F;
			this.butProcCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProcCode.Location = new System.Drawing.Point(479, 75);
			this.butProcCode.Name = "butProcCode";
			this.butProcCode.Size = new System.Drawing.Size(23, 24);
			this.butProcCode.TabIndex = 32;
			this.butProcCode.Text = "...";
			this.butProcCode.Click += new System.EventHandler(this.butProcCode_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(589, 393);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 4;
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
			this.butCancel.Location = new System.Drawing.Point(677, 393);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormAutomationEdit
			// 
			this.ClientSize = new System.Drawing.Size(778, 437);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butProcCode);
			this.Controls.Add(this.comboActionObject);
			this.Controls.Add(this.comboAction);
			this.Controls.Add(this.comboTrigger);
			this.Controls.Add(this.textMessage);
			this.Controls.Add(this.labelMessage);
			this.Controls.Add(this.labelActionObject);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textProcCodes);
			this.Controls.Add(this.labelProcCodes);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(butDelete);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAutomationEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Automation";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAutomationEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormAutomationEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAutomationEdit_Load(object sender, System.EventArgs e) {
			_listCommLogTypeDefs=Defs.GetDefsForCategory(DefCat.CommLogTypes,true);
			textDescription.Text=AutoCur.Description;
			_listAptTypes=new List<AppointmentType>() { new AppointmentType() { AppointmentTypeName="none" } };
			AppointmentTypes.GetWhere(x => !x.IsHidden || x.AppointmentTypeNum==AutoCur.AppointmentTypeNum)
				.ForEach(x => _listAptTypes.Add(x));
			_listAptTypes=_listAptTypes.OrderBy(x => x.AppointmentTypeNum>0).ThenBy(x => x.ItemOrder).ToList();
			Enum.GetNames(typeof(AutomationTrigger)).ToList().ForEach(x => comboTrigger.Items.Add(x));
			comboTrigger.SelectedIndex=(int)AutoCur.Autotrigger;
			textProcCodes.Text=AutoCur.ProcCodes;//although might not be visible.
			textMessage.Text=AutoCur.MessageContent;
			FillGrid();
		}

		private void FillGrid() {
			AutomationConditions.RefreshCache();
			autoList=AutomationConditions.GetListByAutomationNum(AutoCur.AutomationNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g("AutomationCondition","Field"),200));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("AutomationCondition","Comparison"),75));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("AutomationCondition","Text"),100));
			gridMain.Rows.Clear();
			autoList.ForEach(x => gridMain.Rows.Add(new ODGridRow(x.CompareField.ToString(),x.Comparison.ToString(),x.CompareString)));
			gridMain.EndUpdate();
		}

		private void comboTrigger_SelectedIndexChanged(object sender,EventArgs e) {
			comboAction.Items.Clear();
			_listAutoActions=Enum.GetValues(typeof(AutomationAction)).OfType<AutomationAction>().ToList();
			//only add the SetApptStatus and SetApptType actions if the triggers CreateAppt or CreateApptNewPat are selected
			if(!new[] { (int)AutomationTrigger.CreateAppt,(int)AutomationTrigger.CreateApptNewPat }.Contains(comboTrigger.SelectedIndex)) {
				_listAutoActions.Remove(AutomationAction.SetApptStatus);
				_listAutoActions.Remove(AutomationAction.SetApptType);
			}
			_listAutoActions.ForEach(x => comboAction.Items.Add(x.ToString()));
			if((int)AutoCur.Autotrigger==comboTrigger.SelectedIndex) {
				comboAction.SelectedIndex=_listAutoActions.IndexOf(AutoCur.AutoAction);
			}
			else {
				comboAction.SelectedIndex=0;//default to first in the list
			}
			if(new[] { (int)AutomationTrigger.CompleteProcedure,(int)AutomationTrigger.ScheduleProcedure }.Contains(comboTrigger.SelectedIndex)) {
				labelProcCodes.Visible=true;
				textProcCodes.Visible=true;
				butProcCode.Visible=true;
			}
			else{
				labelProcCodes.Visible=false;
				textProcCodes.Visible=false;
				butProcCode.Visible=false;
			}
		}

		///<summary>Fills comboActionObject with the correct type of items based on the comboAction selection and sets labelActionObject text.
		///Also handles setting combos/labels/texts visibility based on selected action.</summary>
		private void comboAction_SelectedIndexChanged(object sender,EventArgs e) {
			labelActionObject.Text="Action Object";//user should never see this text, just to help with troubleshooting in case of bug
			labelActionObject.Visible=false;
			comboActionObject.Visible=false;
			labelMessage.Visible=false;
			textMessage.Visible=false;
			if(comboAction.SelectedIndex<0 || comboAction.SelectedIndex>=_listAutoActions.Count) {
				return;
			}
			comboActionObject.Items.Clear();
			switch(_listAutoActions[comboAction.SelectedIndex]) {
				case AutomationAction.CreateCommlog:
					labelActionObject.Visible=true;
					labelActionObject.Text=Lan.g(this,"Commlog Type");
					comboActionObject.Visible=true;
					_listCommLogTypeDefs.ForEach(x => comboActionObject.Items.Add(x.ItemName));
					comboActionObject.SelectedIndex=_listCommLogTypeDefs.FindIndex(x => x.DefNum==AutoCur.CommType);
					labelMessage.Visible=true;
					textMessage.Visible=true;
					return;
				case AutomationAction.PopUp:
				case AutomationAction.PopUpThenDisable10Min:
					labelMessage.Visible=true;
					textMessage.Visible=true;
					return;
				case AutomationAction.SetApptStatus:
					labelActionObject.Visible=true;
					labelActionObject.Text=Lan.g(this,"Appointment Status");
					comboActionObject.Visible=true;
					_listApptStatuses.ForEach(x => comboActionObject.Items.Add(x.ToString()));
					comboActionObject.SelectedIndex=_listApptStatuses.FindIndex(x => x==AutoCur.AptStatus);//can be -1;
					return;
				case AutomationAction.SetApptType:
					labelActionObject.Visible=true;
					labelActionObject.Text=Lan.g(this,"Appointment Type");
					comboActionObject.Visible=true;
					//_listAppointmentType contains 'none' with AppointmentTypeNum of 0 at index 0, just add list to combo and FindIndex will always be valid
					_listAptTypes.ForEach(x => comboActionObject.Items.Add(x.AppointmentTypeName));
					comboActionObject.SelectedIndex=_listAptTypes.FindIndex(x => AutoCur.AppointmentTypeNum==x.AppointmentTypeNum);//should always be >=0
					return;
				case AutomationAction.PrintPatientLetter:
				case AutomationAction.PrintReferralLetter:
				case AutomationAction.ShowConsentForm:
				case AutomationAction.ShowExamSheet:
					labelActionObject.Visible=true;
					labelActionObject.Text=Lan.g(this,"Sheet Definition");
					comboActionObject.Visible=true;
					List<SheetDef> listSheetDefs=SheetDefs.GetDeepCopy();
					listSheetDefs.ForEach(x => comboActionObject.Items.Add(x.Description));
					comboActionObject.SelectedIndex=listSheetDefs.FindIndex(x => AutoCur.SheetDefNum==x.SheetDefNum);//can be -1
					return;
			}
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			FormAutomationConditionEdit FormACE=new FormAutomationConditionEdit();
			FormACE.ConditionCur=autoList[e.Row];
			FormACE.ShowDialog();
			FillGrid();
		}

		private void butProcCode_Click(object sender,EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			textProcCodes.Text=string.Join(",",new[] { textProcCodes.Text,ProcedureCodes.GetStringProcCode(FormP.SelectedCodeNum) }.Where(x => !string.IsNullOrEmpty(x)));
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormAutomationConditionEdit FormACE=new FormAutomationConditionEdit();
			FormACE.IsNew=true;
			FormACE.ConditionCur=new AutomationCondition();
			FormACE.ConditionCur.AutomationNum=AutoCur.AutomationNum;
			FormACE.ShowDialog();
			if(FormACE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;//delete takes place in FormClosing
			}
			else {
				AutomationConditions.DeleteByAutomationNum(AutoCur.AutomationNum);
				Automations.Delete(AutoCur);
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDescription.Text==""){
				MsgBox.Show(this,"Description not allowed to be blank.");
				return;
			}
			AutoCur.Description=textDescription.Text;
			AutoCur.Autotrigger=(AutomationTrigger)comboTrigger.SelectedIndex;//should never be <0
			#region ProcCodes
			AutoCur.ProcCodes="";//set to correct proc code string below if necessary
			if(new[] { AutomationTrigger.CompleteProcedure,AutomationTrigger.ScheduleProcedure }.Contains(AutoCur.Autotrigger)) {
				if(textProcCodes.Text.Contains(" ")){
					MsgBox.Show(this,"Procedure codes cannot contain any spaces.");
					return;
				}
				if(textProcCodes.Text=="") {
					MsgBox.Show(this,"Please enter valid procedure code(s) first.");
					return;
				}
				string strInvalidCodes=string.Join(", ",textProcCodes.Text.Split(',').Where(x => !ProcedureCodes.IsValidCode(x)));
				if(!string.IsNullOrEmpty(strInvalidCodes)) {
					MessageBox.Show(Lan.g(this,"The following procedure code(s) are not valid")+": "+strInvalidCodes);
					return;
				}
				AutoCur.ProcCodes=textProcCodes.Text;
			}
			#endregion ProcCodes
			#region Automation Action
			//Dictionary linking actions to their associated sheet types and the string to add to the message box text.
			//Only valid for actions PrintPatientLetter, PrintReferralLetter, ShowExamSheet, and ShowConsentForm.
			Dictionary<AutomationAction,Tuple<SheetTypeEnum,string>> dictAutoActionSheetType=new Dictionary<AutomationAction,Tuple<SheetTypeEnum,string>>() {
				{ AutomationAction.PrintPatientLetter,Tuple.Create(SheetTypeEnum.PatientLetter,"a patient letter") },
				{ AutomationAction.PrintReferralLetter,Tuple.Create(SheetTypeEnum.ReferralLetter,"a referral letter") },
				{ AutomationAction.ShowExamSheet,Tuple.Create(SheetTypeEnum.ExamSheet,"an exam sheet") },
				{ AutomationAction.ShowConsentForm,Tuple.Create(SheetTypeEnum.Consent,"a consent form") }
			};
			AutoCur.AutoAction=_listAutoActions[comboAction.SelectedIndex];
			AutoCur.SheetDefNum=0;
			AutoCur.CommType=0;
			AutoCur.MessageContent="";
			AutoCur.AptStatus=ApptStatus.None;
			AutoCur.AppointmentTypeNum=0;
			switch(AutoCur.AutoAction) {
				case AutomationAction.CreateCommlog:
					if(comboActionObject.SelectedIndex==-1) {
						MsgBox.Show(this,"A commlog type must be selected.");
						return;
					}
					AutoCur.CommType=_listCommLogTypeDefs[comboActionObject.SelectedIndex].DefNum;
					AutoCur.MessageContent=textMessage.Text;
					break;
				case AutomationAction.PopUp:
				case AutomationAction.PopUpThenDisable10Min:
					if(string.IsNullOrEmpty(textMessage.Text.Trim())) {
						MsgBox.Show(this,"The message cannot be blank.");
						return;
					}
					AutoCur.MessageContent=textMessage.Text;
					break;
				case AutomationAction.PrintPatientLetter:
				case AutomationAction.PrintReferralLetter:
				case AutomationAction.ShowExamSheet:
				case AutomationAction.ShowConsentForm:
					if(comboActionObject.SelectedIndex==-1) {
						MsgBox.Show(this,"A sheet definition must be selected.");
						return;
					}
					if(SheetDefs.GetDeepCopy()[comboActionObject.SelectedIndex].SheetType!=dictAutoActionSheetType[AutoCur.AutoAction].Item1) {
						MessageBox.Show(this,Lan.g(this,"The selected sheet type must be")+" "+dictAutoActionSheetType[AutoCur.AutoAction].Item2+".");
						return;
					}
					AutoCur.SheetDefNum=SheetDefs.GetDeepCopy()[comboActionObject.SelectedIndex].SheetDefNum;
					break;
				case AutomationAction.SetApptStatus:
					if(comboActionObject.SelectedIndex==-1) {
						MsgBox.Show(this,"An appointment status must be selected.");
						return;
					}
					AutoCur.AptStatus=_listApptStatuses[comboActionObject.SelectedIndex];
					break;
				case AutomationAction.SetApptType:
					if(comboActionObject.SelectedIndex==-1) {
						MsgBox.Show(this,"An appointment type must be selected.");
						return;
					}
					AutoCur.AppointmentTypeNum=_listAptTypes[comboActionObject.SelectedIndex].AppointmentTypeNum;
					break;
			}
			#endregion Automation Action
			Automations.Update(AutoCur);//Because always inserted before opening this form.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormAutomationEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				return;
			}
			//this happens if cancel or if user deletes a new automation
			if(IsNew) {
				AutomationConditions.DeleteByAutomationNum(AutoCur.AutomationNum);
				Automations.Delete(AutoCur);
			}
		}

	}
}






















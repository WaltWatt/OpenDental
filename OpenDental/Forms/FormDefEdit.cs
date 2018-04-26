/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public class FormDefEdit : ODForm {
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.Label labelValue;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.TextBox textValue;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butSelect;
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.Label labelColor;
		private System.Windows.Forms.CheckBox checkHidden;
		private Def DefCur;
		//private Def 
		private OpenDental.UI.Button butDelete;
		private CheckBox checkExcludeSend;
		private CheckBox checkExcludeConfirm;
		private GroupBox groupEConfirm;
		private List<long> _listExcludeSendNums;
		private List<long> _listExcludeConfirmNums;
		///<summary>A list of DefNums that represent all of the Confirmation Statuses that should skip sending eReminders.</summary>
		private List<long> _listExcludeRemindNums;
		private UI.Button butClearValue;
		private DefCatOptions _defCatOptions;
		private string _selectedValueString;
		public bool IsDeleted = false;
		private GroupBox groupBoxEReminders;
		private CheckBox checkExcludeRemind;

		///<summary>The list of definitions that is showing in FormDefinitions.  This list will typically be out of synch with the cache.  Gets set in the constructor.</summary>
		private List<Def> _defsList;
		
		///<summary>defCur should be the currently selected def from FormDefinitions.  defList is going to be the in-memory list of definitions currently displaying to the user.  defList typically is out of synch with the cache which is why we need to pass it in.</summary>
		public FormDefEdit(Def defCur,List<Def> defsList,DefCatOptions defCatOptions){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
			DefCur=defCur;
			_defCatOptions=defCatOptions;
			_defsList=defsList;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefEdit));
			this.labelName = new System.Windows.Forms.Label();
			this.labelValue = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.textValue = new System.Windows.Forms.TextBox();
			this.butColor = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.labelColor = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.butDelete = new OpenDental.UI.Button();
			this.checkExcludeSend = new System.Windows.Forms.CheckBox();
			this.checkExcludeConfirm = new System.Windows.Forms.CheckBox();
			this.groupEConfirm = new System.Windows.Forms.GroupBox();
			this.butSelect = new OpenDental.UI.Button();
			this.butClearValue = new OpenDental.UI.Button();
			this.groupBoxEReminders = new System.Windows.Forms.GroupBox();
			this.checkExcludeRemind = new System.Windows.Forms.CheckBox();
			this.groupEConfirm.SuspendLayout();
			this.groupBoxEReminders.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(12, 36);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(178, 17);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelValue
			// 
			this.labelValue.Location = new System.Drawing.Point(190, 25);
			this.labelValue.Name = "labelValue";
			this.labelValue.Size = new System.Drawing.Size(178, 28);
			this.labelValue.TabIndex = 1;
			this.labelValue.Text = "Value";
			this.labelValue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(12, 55);
			this.textName.Multiline = true;
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(178, 64);
			this.textName.TabIndex = 0;
			// 
			// textValue
			// 
			this.textValue.Location = new System.Drawing.Point(190, 55);
			this.textValue.MaxLength = 256;
			this.textValue.Multiline = true;
			this.textValue.Name = "textValue";
			this.textValue.Size = new System.Drawing.Size(178, 64);
			this.textValue.TabIndex = 1;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(371, 55);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 2;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// colorDialog1
			// 
			this.colorDialog1.FullOpen = true;
			// 
			// labelColor
			// 
			this.labelColor.Location = new System.Drawing.Point(371, 37);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(74, 16);
			this.labelColor.TabIndex = 5;
			this.labelColor.Text = "Color";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(416, 134);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(497, 134);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkHidden
			// 
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(12, 12);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(157, 18);
			this.checkHidden.TabIndex = 3;
			this.checkHidden.Text = "Hidden";
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 134);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(79, 25);
			this.butDelete.TabIndex = 6;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// checkExcludeSend
			// 
			this.checkExcludeSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeSend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeSend.Location = new System.Drawing.Point(6, 19);
			this.checkExcludeSend.Name = "checkExcludeSend";
			this.checkExcludeSend.Size = new System.Drawing.Size(141, 18);
			this.checkExcludeSend.TabIndex = 7;
			this.checkExcludeSend.Text = "Exclude when sending";
			// 
			// checkExcludeConfirm
			// 
			this.checkExcludeConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeConfirm.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeConfirm.Location = new System.Drawing.Point(6, 40);
			this.checkExcludeConfirm.Name = "checkExcludeConfirm";
			this.checkExcludeConfirm.Size = new System.Drawing.Size(141, 18);
			this.checkExcludeConfirm.TabIndex = 8;
			this.checkExcludeConfirm.Text = "Exclude when confirming";
			// 
			// groupEConfirm
			// 
			this.groupEConfirm.Controls.Add(this.checkExcludeSend);
			this.groupEConfirm.Controls.Add(this.checkExcludeConfirm);
			this.groupEConfirm.Location = new System.Drawing.Point(419, 12);
			this.groupEConfirm.Name = "groupEConfirm";
			this.groupEConfirm.Size = new System.Drawing.Size(153, 64);
			this.groupEConfirm.TabIndex = 9;
			this.groupEConfirm.TabStop = false;
			this.groupEConfirm.Text = "eConfirmations";
			// 
			// butSelect
			// 
			this.butSelect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelect.Autosize = true;
			this.butSelect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSelect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSelect.CornerRadius = 4F;
			this.butSelect.Location = new System.Drawing.Point(371, 97);
			this.butSelect.Name = "butSelect";
			this.butSelect.Size = new System.Drawing.Size(21, 22);
			this.butSelect.TabIndex = 200;
			this.butSelect.Text = "...";
			this.butSelect.Click += new System.EventHandler(this.butSelect_Click);
			// 
			// butClearValue
			// 
			this.butClearValue.AdjustImageLocation = new System.Drawing.Point(1, 0);
			this.butClearValue.Autosize = true;
			this.butClearValue.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearValue.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearValue.CornerRadius = 4F;
			this.butClearValue.Image = global::OpenDental.Properties.Resources.deleteX18;
			this.butClearValue.Location = new System.Drawing.Point(395, 97);
			this.butClearValue.Name = "butClearValue";
			this.butClearValue.Size = new System.Drawing.Size(21, 22);
			this.butClearValue.TabIndex = 201;
			this.butClearValue.Click += new System.EventHandler(this.butClearValue_Click);
			// 
			// groupBoxEReminders
			// 
			this.groupBoxEReminders.Controls.Add(this.checkExcludeRemind);
			this.groupBoxEReminders.Location = new System.Drawing.Point(419, 82);
			this.groupBoxEReminders.Name = "groupBoxEReminders";
			this.groupBoxEReminders.Size = new System.Drawing.Size(153, 46);
			this.groupBoxEReminders.TabIndex = 202;
			this.groupBoxEReminders.TabStop = false;
			this.groupBoxEReminders.Text = "eReminders";
			// 
			// checkExcludeRemind
			// 
			this.checkExcludeRemind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeRemind.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeRemind.Location = new System.Drawing.Point(6, 19);
			this.checkExcludeRemind.Name = "checkExcludeRemind";
			this.checkExcludeRemind.Size = new System.Drawing.Size(141, 18);
			this.checkExcludeRemind.TabIndex = 7;
			this.checkExcludeRemind.Text = "Exclude when sending";
			// 
			// FormDefEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(584, 171);
			this.Controls.Add(this.groupBoxEReminders);
			this.Controls.Add(this.butClearValue);
			this.Controls.Add(this.groupEConfirm);
			this.Controls.Add(this.butSelect);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.textValue);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.labelValue);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.labelColor);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDefEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Definition";
			this.Load += new System.EventHandler(this.FormDefEdit_Load);
			this.groupEConfirm.ResumeLayout(false);
			this.groupBoxEReminders.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDefEdit_Load(object sender, System.EventArgs e) {
			if(DefCur.Category==DefCat.ApptConfirmed) {
				_listExcludeSendNums=PrefC.GetString(PrefName.ApptConfirmExcludeESend).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				_listExcludeConfirmNums=PrefC.GetString(PrefName.ApptConfirmExcludeEConfirm).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				_listExcludeRemindNums=PrefC.GetString(PrefName.ApptConfirmExcludeERemind).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				//0 will get automatically added to the list when this is the first of its kind.  We never want 0 inserted.
				_listExcludeSendNums.Remove(0);
				_listExcludeConfirmNums.Remove(0);
				_listExcludeRemindNums.Remove(0);
				checkExcludeSend.Checked=_listExcludeSendNums.Contains(DefCur.DefNum);
				checkExcludeConfirm.Checked=_listExcludeConfirmNums.Contains(DefCur.DefNum);
				checkExcludeRemind.Checked=_listExcludeRemindNums.Contains(DefCur.DefNum);
			}
			else {
				groupEConfirm.Visible=false;
				groupBoxEReminders.Visible=false;
			}
			if(DefCur.DefNum.In(PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger),PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger),
				PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger))) 
			{
				//We never want to send confirmation or reminders to an appointment when it is in a triggered confirm status.
				checkExcludeConfirm.Enabled=false;
				checkExcludeRemind.Enabled=false;
				checkExcludeSend.Enabled=false;
				checkExcludeConfirm.Checked=true;
				checkExcludeRemind.Checked=true;
				checkExcludeSend.Checked=true;
			}
			string itemName=DefCur.ItemName;
			_selectedValueString=DefCur.ItemValue;
			if(!_defCatOptions.CanEditName) {
				//Allow foreign users to translate definitions that they do not have access to translate.
				//Use FormDefinitions instead of 'this' because the users will have already translated the item names in that form and no need to duplicate.
				itemName=Lan.g("FormDefinitions",DefCur.ItemName);
				textName.ReadOnly=true;
				if(!DefCur.IsHidden || Defs.IsDefDeprecated(DefCur)) {
					checkHidden.Enabled=false;//prevent hiding defs that are hard-coded into OD. Prevent unhiding defs that are deprecated.
				}
			}
			labelValue.Text=_defCatOptions.ValueText;
			if(DefCur.Category==DefCat.AdjTypes && !IsNew){
				labelValue.Text="Not allowed to change type after an adjustment is created.";
				textValue.Visible=false;
			}
			if(!_defCatOptions.EnableValue){
				labelValue.Visible=false;
				textValue.Visible=false;
			}
			if(!_defCatOptions.EnableColor){
				labelColor.Visible=false;
				butColor.Visible=false;
			}
			if(!_defCatOptions.CanHide){
				checkHidden.Visible=false;
			}
			if(!_defCatOptions.CanDelete){
				butDelete.Visible=false;
			}
			if(_defCatOptions.IsValueDefNum) {
				textValue.ReadOnly=true;
				textValue.BackColor=SystemColors.Control;
				labelValue.Text=Lan.g("FormDefinitions","Use the select button to choose a definition from the list.");
				long defNumCur=PIn.Long(DefCur.ItemValue??"");
				if(defNumCur>0) {
					textValue.Text=_defsList.FirstOrDefault(x => defNumCur==x.DefNum)?.ItemName??"";
				}
				butSelect.Visible=true;
				butClearValue.Visible=true;
			}
			else if(_defCatOptions.DoShowItemOrderInValue) {
				labelValue.Text=Lan.g(this,"Internal Priority");
				textValue.Text=DefCur.ItemOrder.ToString();
				textValue.ReadOnly=true;
				butSelect.Visible=false;
				butClearValue.Visible=false;
			}
			else {
				textValue.Text=DefCur.ItemValue;
				butSelect.Visible=false;
				butClearValue.Visible=false;
			}
			textName.Text=itemName;
			butColor.BackColor=DefCur.ItemColor;
			checkHidden.Checked=DefCur.IsHidden;
		}

		///<summary>Check to make sure that DefCur is not the last showing def in the list of defs that was passed in.  Only helpful for definitions that require at least one def be present.</summary>
		private bool IsDefCurLastShowing() {
			int countShowing=0;
			for(int i=0;i<_defsList.Count;i++) {
				if(_defsList[i].DefNum==DefCur.DefNum) {
					continue;
				}
				if(_defsList[i].IsHidden) {
					continue;
				}
				countShowing++;
			}
			return countShowing==0;
		}

		private void butColor_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
			//textColor.Text=colorDialog1.Color.Name;
		}

		private void butSelect_Click(object sender,EventArgs e) {
			long defNumParent=PIn.Long(DefCur.ItemValue);//ItemValue could be blank, in which case defNumCur will be 0
			FormDefinitionPicker FormDP=new FormDefinitionPicker(DefCur.Category,_defsList.ToList().FindAll(x => x.DefNum==defNumParent),DefCur.DefNum);
			FormDP.IsMultiSelectionMode=false;
			FormDP.HasShowHiddenOption=false;
			FormDP.ShowDialog();
			if(FormDP.DialogResult!=DialogResult.OK) {
				return;
			}
			Def selectedDef=FormDP.ListSelectedDefs.DefaultIfEmpty(new Def() { ItemName="" }).First();
			_selectedValueString=selectedDef.DefNum==0?"":selectedDef.DefNum.ToString();//list should have exactly one def in it, but this is safe
			textValue.Text=selectedDef.ItemName;
		}

		private void butClearValue_Click(object sender,EventArgs e) {
			_selectedValueString="";
			textValue.Clear();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			//This is VERY new.  Only allowed and visible for three categories so far: supply cats, claim payment types, and claim custom tracking.
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(DefCur.Category==DefCat.ClaimCustomTracking && _defsList.Count(x => x.Category==DefCat.ClaimCustomTracking)==1
				|| DefCur.Category==DefCat.InsurancePaymentType && _defsList.Count(x => x.Category==DefCat.InsurancePaymentType)==1
				|| DefCur.Category==DefCat.SupplyCats && _defsList.Count(x => x.Category==DefCat.SupplyCats)==1) 
			{
				MsgBox.Show(this,"Cannot delete the last definition from this category.");
				return;
			}
			bool isAutoNoteRefresh=false;
			if(DefCur.Category==DefCat.AutoNoteCats && AutoNotes.GetExists(x => x.Category==DefCur.DefNum)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Deleting this Auto Note Category will uncategorize some auto notes.  Delete anyway?")) {
					return;
				}
				isAutoNoteRefresh=true;
			}
			try{
				Defs.Delete(DefCur);
				IsDeleted=true;
				if(isAutoNoteRefresh) {//deleting an auto note category currently in use will uncategorize those auto notes, refresh cache
					DataValid.SetInvalid(InvalidType.AutoNotes);
				}
				DialogResult=DialogResult.OK;
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(Defs.IsHidable(DefCur.Category) && checkHidden.Checked) {
				if(Defs.IsDefinitionInUse(DefCur)) {
					if(DefCur.DefNum==PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.BillingChargeAdjustmentType)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.FinanceChargeAdjustmentType)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.PrepaymentUnearnedType)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.PracticeDefaultBillType)
						|| DefCur.DefNum==PrefC.GetLong(PrefName.SalesTaxAdjustmentType))
					{
						MsgBox.Show(this,"You cannot hide a definition if it is in use within Module Preferences.");
						return;
					}
					else if(DefCur.DefNum.In(
						PrefC.GetLong(PrefName.RecallStatusMailed),
						PrefC.GetLong(PrefName.RecallStatusTexted),
						PrefC.GetLong(PrefName.RecallStatusEmailed),
						PrefC.GetLong(PrefName.RecallStatusEmailedTexted))) 
					{
						MsgBox.Show(this,"You cannot hide a definition that is used as a status in the Setup Recall window.");
						return;
					}
					else if(DefCur.DefNum==PrefC.GetLong(PrefName.WebSchedNewPatConfirmStatus)) {
						MsgBox.Show(this,"You cannot hide a definition that is used as an appointment confirmation status in Web Sched New Pat Appt.");
						return;
					}
					else if(DefCur.DefNum==PrefC.GetLong(PrefName.WebSchedRecallConfirmStatus)) {
						MsgBox.Show(this,"You cannot hide a definition that is used as an appointment confirmation status in Web Sched Recall Appt.");
						return;
					}
					else {
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning: This definition is currently in use within the program.")) {
							return;
						}
					}
				}
				//Stop users from hiding the last definition in categories that must have at least one def in them.
				if(!_defsList.Any(x=>x.DefNum!=DefCur.DefNum && !x.IsHidden)) {
					MsgBox.Show(this,"You cannot hide the last definition in this category.");
					return;
				}
			}
			if(textName.Text==""){
				MsgBox.Show(this,"Name required.");
				return;
			}
			switch(DefCur.Category){
				case DefCat.AccountQuickCharge:
				case DefCat.ApptProcsQuickAdd:
					string[] procCodes=textValue.Text.Split(',');
					List<string> listProcCodes=new List<string>();
					for(int i=0;i<procCodes.Length;i++) {
						ProcedureCode procCode=ProcedureCodes.GetProcCode(procCodes[i]);
						if(procCode.CodeNum==0) {
							//Now check to see if the trimmed version of the code does not exist either.
							procCode=ProcedureCodes.GetProcCode(procCodes[i].Trim());
							if(procCode.CodeNum==0) {
								MessageBox.Show(Lan.g(this,"Invalid procedure code entered")+": "+procCodes[i]);
								return;
							}
						}
						listProcCodes.Add(procCode.ProcCode);
					}
					textValue.Text=String.Join(",",listProcCodes);
					break;
				case DefCat.AdjTypes:
					if(textValue.Text!="+" && textValue.Text!="-" && textValue.Text!="dp"){
						MessageBox.Show(Lan.g(this,"Valid values are +, -, or dp."));
						return;
					}
					break;
				case DefCat.BillingTypes:
					if(textValue.Text!="" && textValue.Text!="E" && textValue.Text!="C") 
					{
						MsgBox.Show(this,"Valid values are blank, E, or C.");
						return;
					}
					if(checkHidden.Checked && Patients.IsBillingTypeInUse(DefCur.DefNum)) {
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning: Billing type is currently in use by patients, insurance plans, or preferences.")) {
							return;
						}
					}
					break;
				case DefCat.ClaimCustomTracking:
					int value=0;
					if(!Int32.TryParse(textValue.Text,out value) || value<0) {
						MsgBox.Show(this,"Days Suppressed must be a valid non-negative number.");
						return;
					}
					break;
				case DefCat.CommLogTypes:
					if(textValue.Text!="" && textValue.Text!="MISC" && textValue.Text!="APPT"
						&& textValue.Text!="FIN" && textValue.Text!="RECALL" && textValue.Text!="TEXT") {
						MessageBox.Show(Lan.g(this,"Valid values are blank,APPT,FIN,RECALL,MISC,or TEXT."));
						return;
					}
					break;
				case DefCat.DiscountTypes:
					int discVal;
					if(textValue.Text=="") break;
					try {
						discVal=System.Convert.ToInt32(textValue.Text);
					}
					catch {
						MessageBox.Show(Lan.g(this,"Not a valid number"));
						return;
					}
					if(discVal < 0 || discVal > 100) {
						MessageBox.Show(Lan.g(this,"Valid values are between 0 and 100"));
						return;
					}
					textValue.Text=discVal.ToString();
					break;
				/*case DefCat.FeeSchedNames:
					if(textValue.Text=="C" || textValue.Text=="c") {
						textValue.Text="C";
					}
					else if(textValue.Text=="A" || textValue.Text=="a") {
						textValue.Text="A";
					}
					else textValue.Text="";
					break;*/
				case DefCat.ImageCats:
					textValue.Text=textValue.Text.ToUpper().Replace(",","");
					if(!Regex.IsMatch(textValue.Text,@"^[XPS]*$")){
						textValue.Text="";
					}
					break;
				case DefCat.InsurancePaymentType:
					if(textValue.Text!="" && textValue.Text!="N") {
						MsgBox.Show(this,"Valid values are blank or N.");
						return;
					}
					break;
				case DefCat.OperatoriesOld:
					if(textValue.Text.Length > 5){
						MessageBox.Show(Lan.g(this,"Maximum length of abbreviation is 5."));
						return;
					}
					break;
				case DefCat.ProcCodeCats:
					if(checkHidden.Checked) {
						if(IsDefCurLastShowing()) {
							MsgBox.Show(this,"At least one procedure code category must be enabled.");
							return;
						}
					}
					break;
				case DefCat.ProviderSpecialties:
					if(checkHidden.Checked
						&& (Providers.IsSpecialtyInUse(DefCur.DefNum)
						|| Referrals.IsSpecialtyInUse(DefCur.DefNum)))
					{
						MsgBox.Show(this,"You cannot hide a specialty if it is in use by a provider or a referral source.");
						checkHidden.Checked=false;
						return;
					}
					break;
				case DefCat.RecallUnschedStatus:
					if(textValue.Text.Length > 7){
						MessageBox.Show(Lan.g(this,"Maximum length is 7."));
						return;
					}
					break;
				case DefCat.TaskPriorities:
					if(checkHidden.Checked) {
						if(IsDefCurLastShowing()) {
							MsgBox.Show(this,"You cannot hide the last priority.");
							return;
						}
					}
					break;
				case DefCat.TxPriorities:
					if(textValue.Text.Length > 7){
						MessageBox.Show(Lan.g(this,"Maximum length of abbreviation is 7."));
						return;
					}
					break;
				default:
					break;
			}//end switch DefCur.Category
			DefCur.ItemName=textName.Text;
			DefCur.ItemValue=_selectedValueString;
			if(_defCatOptions.EnableValue && !_defCatOptions.IsValueDefNum) {
				DefCur.ItemValue=textValue.Text;
			}
			if(_defCatOptions.EnableColor) {
				DefCur.ItemColor=butColor.BackColor;
			}
			DefCur.IsHidden=checkHidden.Checked;
			if(IsNew){
				Defs.Insert(DefCur);
			}
			else{
				Defs.Update(DefCur);
			}
			//Must be after the upsert so that we have access to the DefNum for new Defs.
			if(DefCur.Category==DefCat.ApptConfirmed) {
				//==================== EXCLUDE SEND ====================
				if(checkExcludeSend.Checked) {
					_listExcludeSendNums.Add(DefCur.DefNum);
				}
				else {
					_listExcludeSendNums.RemoveAll(x => x==DefCur.DefNum);
				}
				string sendString=string.Join(",",_listExcludeSendNums.Distinct().OrderBy(x => x));
				Prefs.UpdateString(PrefName.ApptConfirmExcludeESend,sendString);
				//==================== EXCLUDE CONFIRM ====================
				if(checkExcludeConfirm.Checked) {
					_listExcludeConfirmNums.Add(DefCur.DefNum);
				}
				else {
					_listExcludeConfirmNums.RemoveAll(x => x==DefCur.DefNum);
				}
				string confirmString = string.Join(",",_listExcludeConfirmNums.Distinct().OrderBy(x => x));
				Prefs.UpdateString(PrefName.ApptConfirmExcludeEConfirm,confirmString);
				//==================== EXCLUDE REMIND ====================
				if(checkExcludeRemind.Checked) {
					_listExcludeRemindNums.Add(DefCur.DefNum);
				}
				else {
					_listExcludeRemindNums.RemoveAll(x => x==DefCur.DefNum);
				}
				string remindString=string.Join(",",_listExcludeRemindNums.Distinct().OrderBy(x => x));
				Prefs.UpdateString(PrefName.ApptConfirmExcludeERemind,remindString);
				Signalods.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

	

	}
}

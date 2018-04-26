using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTimeCardSetup:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private CheckBox checkUseDecimal;
		private ODGrid gridRules;
		private GroupBox groupBox1;
		private OpenDental.UI.Button butAddRule;
		private CheckBox checkAdjOverBreaks;
		private Label label2;
		private TextBox textADPCompanyCode;
		private CheckBox checkShowSeconds;
		private UI.Button butGenerate;
		private UI.Button butDelete;
		private bool changed;
		private CheckBox checkHideOlder;
		///<summary>Locally cached list of pay periods.</summary>
		private List<PayPeriod> _listPayPeriods;

		///<summary></summary>
		public FormTimeCardSetup() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeCardSetup));
			this.checkUseDecimal = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkShowSeconds = new System.Windows.Forms.CheckBox();
			this.checkAdjOverBreaks = new System.Windows.Forms.CheckBox();
			this.butAddRule = new OpenDental.UI.Button();
			this.gridRules = new OpenDental.UI.ODGrid();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textADPCompanyCode = new System.Windows.Forms.TextBox();
			this.butGenerate = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.checkHideOlder = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkUseDecimal
			// 
			this.checkUseDecimal.Location = new System.Drawing.Point(12, 19);
			this.checkUseDecimal.Name = "checkUseDecimal";
			this.checkUseDecimal.Size = new System.Drawing.Size(362, 18);
			this.checkUseDecimal.TabIndex = 12;
			this.checkUseDecimal.Text = "Use decimal format rather than colon format";
			this.checkUseDecimal.UseVisualStyleBackColor = true;
			this.checkUseDecimal.Click += new System.EventHandler(this.checkUseDecimal_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.checkShowSeconds);
			this.groupBox1.Controls.Add(this.checkAdjOverBreaks);
			this.groupBox1.Controls.Add(this.checkUseDecimal);
			this.groupBox1.Location = new System.Drawing.Point(305, 524);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(380, 74);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// checkShowSeconds
			// 
			this.checkShowSeconds.Location = new System.Drawing.Point(12, 51);
			this.checkShowSeconds.Name = "checkShowSeconds";
			this.checkShowSeconds.Size = new System.Drawing.Size(362, 18);
			this.checkShowSeconds.TabIndex = 14;
			this.checkShowSeconds.Text = "Use seconds on time card when using colon format";
			this.checkShowSeconds.UseVisualStyleBackColor = true;
			this.checkShowSeconds.Click += new System.EventHandler(this.checkShowSeconds_Click);
			// 
			// checkAdjOverBreaks
			// 
			this.checkAdjOverBreaks.Location = new System.Drawing.Point(12, 35);
			this.checkAdjOverBreaks.Name = "checkAdjOverBreaks";
			this.checkAdjOverBreaks.Size = new System.Drawing.Size(362, 18);
			this.checkAdjOverBreaks.TabIndex = 13;
			this.checkAdjOverBreaks.Text = "Calc Daily button makes adjustments if breaks over 30 minutes.";
			this.checkAdjOverBreaks.UseVisualStyleBackColor = true;
			this.checkAdjOverBreaks.Click += new System.EventHandler(this.checkAdjOverBreaks_Click);
			// 
			// butAddRule
			// 
			this.butAddRule.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddRule.Autosize = true;
			this.butAddRule.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddRule.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddRule.CornerRadius = 4F;
			this.butAddRule.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddRule.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddRule.Location = new System.Drawing.Point(305, 494);
			this.butAddRule.Name = "butAddRule";
			this.butAddRule.Size = new System.Drawing.Size(80, 24);
			this.butAddRule.TabIndex = 15;
			this.butAddRule.Text = "Add";
			this.butAddRule.Click += new System.EventHandler(this.butAddRule_Click);
			// 
			// gridRules
			// 
			this.gridRules.AllowSortingByColumn = true;
			this.gridRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRules.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridRules.HasAddButton = false;
			this.gridRules.HasDropDowns = false;
			this.gridRules.HasMultilineHeaders = false;
			this.gridRules.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridRules.HeaderHeight = 15;
			this.gridRules.HScrollVisible = false;
			this.gridRules.Location = new System.Drawing.Point(305, 27);
			this.gridRules.Name = "gridRules";
			this.gridRules.ScrollValue = 0;
			this.gridRules.Size = new System.Drawing.Size(616, 463);
			this.gridRules.TabIndex = 13;
			this.gridRules.Title = "Rules";
			this.gridRules.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridRules.TitleHeight = 18;
			this.gridRules.TranslationName = "FormTimeCardSetup";
			this.gridRules.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridRules_CellDoubleClick);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(19, 27);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(272, 463);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Pay Periods";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TablePayPeriods";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(19, 494);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(88, 24);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add One";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(846, 604);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(14, 608);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(119, 16);
			this.label2.TabIndex = 17;
			this.label2.Text = "ADP Company Code";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textADPCompanyCode
			// 
			this.textADPCompanyCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textADPCompanyCode.Location = new System.Drawing.Point(133, 606);
			this.textADPCompanyCode.Name = "textADPCompanyCode";
			this.textADPCompanyCode.Size = new System.Drawing.Size(97, 20);
			this.textADPCompanyCode.TabIndex = 16;
			// 
			// butGenerate
			// 
			this.butGenerate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGenerate.Autosize = true;
			this.butGenerate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGenerate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGenerate.CornerRadius = 4F;
			this.butGenerate.Image = global::OpenDental.Properties.Resources.Add;
			this.butGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGenerate.Location = new System.Drawing.Point(19, 524);
			this.butGenerate.Name = "butGenerate";
			this.butGenerate.Size = new System.Drawing.Size(114, 23);
			this.butGenerate.TabIndex = 18;
			this.butGenerate.Text = "Generate Many";
			this.butGenerate.UseVisualStyleBackColor = true;
			this.butGenerate.Click += new System.EventHandler(this.butGenerate_Click);
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
			this.butDelete.Location = new System.Drawing.Point(180, 494);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(111, 24);
			this.butDelete.TabIndex = 19;
			this.butDelete.Text = "Delete Selected";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// checkHideOlder
			// 
			this.checkHideOlder.Checked = true;
			this.checkHideOlder.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkHideOlder.Location = new System.Drawing.Point(20, 7);
			this.checkHideOlder.Name = "checkHideOlder";
			this.checkHideOlder.Size = new System.Drawing.Size(288, 18);
			this.checkHideOlder.TabIndex = 20;
			this.checkHideOlder.Text = "Hide pay periods older than 6 months";
			this.checkHideOlder.UseVisualStyleBackColor = true;
			this.checkHideOlder.CheckedChanged += new System.EventHandler(this.checkHideOlder_CheckedChanged);
			// 
			// FormTimeCardSetup
			// 
			this.ClientSize = new System.Drawing.Size(949, 636);
			this.Controls.Add(this.checkHideOlder);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butGenerate);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textADPCompanyCode);
			this.Controls.Add(this.butAddRule);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gridRules);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTimeCardSetup";
			this.ShowInTaskbar = false;
			this.Text = "Time Card Setup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPayPeriods_FormClosing);
			this.Load += new System.EventHandler(this.FormPayPeriods_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPayPeriods_Load(object sender,System.EventArgs e) {
			checkUseDecimal.Checked=PrefC.GetBool(PrefName.TimeCardsUseDecimalInsteadOfColon);
			checkAdjOverBreaks.Checked=PrefC.GetBool(PrefName.TimeCardsMakesAdjustmentsForOverBreaks);
			checkShowSeconds.Checked=PrefC.GetBool(PrefName.TimeCardShowSeconds);
			Employees.RefreshCache();
			FillGrid();
			FillRules();
			textADPCompanyCode.Text=PrefC.GetString(PrefName.ADPCompanyCode);
		}

		///<summary>Does not refresh the cached list.  Make sure any updates to _listPayPeriods are done before calling this method.</summary>
		private void FillGrid() {
			PayPeriods.RefreshCache();
			_listPayPeriods=PayPeriods.GetDeepCopy().OrderBy(x => x.DateStart).ToList();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Start Date",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("End Date",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Paycheck Date",100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			UI.ODGridRow row;
			foreach(PayPeriod payPeriodCur in _listPayPeriods) {
				if(checkHideOlder.Checked && payPeriodCur.DateStart < DateTimeOD.Today.AddMonths(-6)) {
					continue;
				}
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(payPeriodCur.DateStart.ToShortDateString());
				row.Cells.Add(payPeriodCur.DateStop.ToShortDateString());
				if(payPeriodCur.DatePaycheck.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(payPeriodCur.DatePaycheck.ToShortDateString());
				}
				row.Tag=payPeriodCur;
				if(payPeriodCur.DateStart<=DateTimeOD.Today && payPeriodCur.DateStop >=DateTimeOD.Today) {
					row.ColorBackG=Color.LightCyan;
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillRules() {
			TimeCardRules.RefreshCache();
			//Start with a convenient sorting of all employees on top, followed by a last name sort.
			List<TimeCardRule> listSorted=TimeCardRules.GetDeepCopy().OrderBy(x => x.IsOvertimeExempt)
				.ThenBy(x => x.EmployeeNum!=0)
				.ThenBy(x => (Employees.GetEmp(x.EmployeeNum)??new Employee()).LName)
				.ToList();
			gridRules.BeginUpdate();
			gridRules.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Employee",150,GridSortingStrategy.StringCompare);
			gridRules.Columns.Add(col);
			col=new ODGridColumn("OT before x Time",105,GridSortingStrategy.TimeParse);
			gridRules.Columns.Add(col);
			col=new ODGridColumn("OT after x Time",100,GridSortingStrategy.TimeParse);
			gridRules.Columns.Add(col);
			col=new ODGridColumn("OT after x Hours",110,GridSortingStrategy.TimeParse);
			gridRules.Columns.Add(col);
			col=new ODGridColumn("Is Overtime Exempt",120,HorizontalAlignment.Center,GridSortingStrategy.StringCompare);
			gridRules.Columns.Add(col);
			gridRules.Rows.Clear();
			UI.ODGridRow row;
			for(int i=0;i<listSorted.Count;i++) {
				row=new ODGridRow();
				if(listSorted[i].EmployeeNum==0) {
					row.Cells.Add(Lan.g(this,"All Employees"));
				}
				else {
					Employee emp=Employees.GetEmp(listSorted[i].EmployeeNum);
					row.Cells.Add(emp.FName+" "+emp.LName);
				}
				row.Cells.Add(listSorted[i].BeforeTimeOfDay.ToStringHmm());
				row.Cells.Add(listSorted[i].AfterTimeOfDay.ToStringHmm());
				row.Cells.Add(listSorted[i].OverHoursPerDay.ToStringHmm());
				row.Cells.Add(listSorted[i].IsOvertimeExempt ? "X" : "");
				row.Tag=listSorted[i];
				gridRules.Rows.Add(row);
			}
			gridRules.EndUpdate();
		}

		///<summary>Makes sure that the pay periods that the user has selected are safe to delete.
		///A pay period cannot be deleted in bulk if: 
		///a) It is in the past OR 
		///b) There are clockevents tied to it and there are no other pay periods for the date of the clockevent.</summary>
		private bool IsSafeToDelete(List<PayPeriod> listSelectedPayPeriods,out List<PayPeriod> retListToDelete) {
			if(listSelectedPayPeriods.Where(x => x.DateStop < DateTimeOD.Today).Count() > 0) {
				MsgBox.Show(this,"You may not delete past pay periods from here. Delete them individually by double clicking them instead.");
				retListToDelete=new List<PayPeriod>();
				return false;
			}
			List<PayPeriod> listPayPeriodsToDelete = new List<PayPeriod>();
			List<ClockEvent> listClockEventsAll = ClockEvents.GetAllForPeriod(listSelectedPayPeriods.Min(x => x.DateStart), listSelectedPayPeriods.Max(x => x.DateStop));
			foreach(PayPeriod payPeriod in listSelectedPayPeriods) {
				List<ClockEvent> listClockEventsForPeriod = listClockEventsAll.Where(x => x.TimeDisplayed1 >= payPeriod.DateStart && x.TimeDisplayed2 <= payPeriod.DateStop).ToList();
				if(listClockEventsForPeriod.Count == 0) {
					//there are no clock events for this period.
					listPayPeriodsToDelete.Add(payPeriod);
					continue;
				}
				//there ARE clock events for this period. now are there other periods that are *not* in the selected list?
				foreach(ClockEvent clockEvent in listClockEventsForPeriod) {
					if(_listPayPeriods.Where(x => x.DateStart <= clockEvent.TimeDisplayed1 && x.DateStop >= clockEvent.TimeDisplayed1 && !listSelectedPayPeriods.Contains(x)).Count() < 1) {
						//if no, then kick out.
						MsgBox.Show(this,"You may not delete all pay periods where a clock event exists.");
						retListToDelete=new List<PayPeriod>();
						return false;
					}
					//otherwise, the add this pay period to the list to delete and continue
					listPayPeriodsToDelete.Add(payPeriod);
				}
			}
			retListToDelete=listPayPeriodsToDelete;
			return true;
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			FormPayPeriodEdit FormP=new FormPayPeriodEdit((PayPeriod)gridMain.Rows[e.Row].Tag);
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel) {
				return;
			}
			FillGrid();
			changed=true;
		}

		private void butAdd_Click(object sender,System.EventArgs e) {
			PayPeriod payPeriodCur=new PayPeriod();
			if(PayPeriods.GetCount()==0) {
				payPeriodCur.DateStart=DateTime.Today;
			}
			else {
				payPeriodCur.DateStart=PayPeriods.GetLast().DateStop.AddDays(1);
			}
			payPeriodCur.DateStop=payPeriodCur.DateStart.AddDays(13);//payPeriodCur.DateStop is inclusive, this is effectively a 14 day default pay period. This only affects default date of newly created pay periods.
			payPeriodCur.DatePaycheck=payPeriodCur.DateStop.AddDays(4);
			FormPayPeriodEdit FormP=new FormPayPeriodEdit(payPeriodCur);
			FormP.IsNew=true;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel) {
				return;
			}
			FillGrid();
			changed=true;
		}

		private void butGenerate_Click(object sender,EventArgs e) {
			//Automatically generate payperiods based on settings.
			FormPayPeriodManager FormPPM=new FormPayPeriodManager();
			FormPPM.ShowDialog();
			if(FormPPM.DialogResult == DialogResult.Cancel) {
				return;
			}
			FillGrid();
		}

		private void checkUseDecimal_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.TimeCardsUseDecimalInsteadOfColon,checkUseDecimal.Checked)) {
				changed=true;
			}
		}

		private void checkAdjOverBreaks_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.TimeCardsMakesAdjustmentsForOverBreaks,checkAdjOverBreaks.Checked)) {
				changed=true;
			}
		}

		private void checkShowSeconds_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.TimeCardShowSeconds,checkShowSeconds.Checked)) {
				changed=true;
			}
		}

		private void butAddRule_Click(object sender,EventArgs e) {
			TimeCardRule rule=new TimeCardRule();
			rule.IsNew=true;
			FormTimeCardRuleEdit FormT=new FormTimeCardRuleEdit();
			FormT.timeCardRule=rule;
			FormT.ShowDialog();
			FillRules();
			changed=true;
		}

		private void gridRules_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormTimeCardRuleEdit FormT=new FormTimeCardRuleEdit();
			FormT.timeCardRule=(TimeCardRule)gridRules.Rows[e.Row].Tag;
			FormT.ShowDialog();
			FillRules();
			changed=true;
		}

		private void checkHideOlder_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
			if(checkHideOlder.Checked) {
				gridMain.ScrollToEnd();
			}
		}

		///<summary>Deletes all the selected pay periods. Performs validation to make sure the delete is safe.</summary>
		private void butDelete_Click(object sender,EventArgs e) {
			//validation
			if(gridMain.SelectedIndices.Length == 0) {
				MsgBox.Show(this,"Please select one or more Pay Periods to delete.");
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to delete all selected pay periods?")) {
				return;
			}
			List<PayPeriod> listSelectedPayPeriods = new List<PayPeriod>();
			for(int i = 0;i < gridMain.SelectedIndices.Length;i++) {
				listSelectedPayPeriods.Add((PayPeriod)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);
			}
			List<PayPeriod> listPayPeriodsToDelete;
			if(!IsSafeToDelete(listSelectedPayPeriods,out listPayPeriodsToDelete)) {
				return;
			}
			if(listPayPeriodsToDelete == null || listPayPeriodsToDelete.Count == 0) {
				return;
			}
			//Actual deletion logic below.
			foreach(PayPeriod payPeriod in listSelectedPayPeriods) {
				PayPeriods.Delete(payPeriod);
			}
			FillGrid();
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

		private void FormPayPeriods_FormClosing(object sender,FormClosingEventArgs e) {
			//validation on Form_Closing to account for if the user doesn't use the close button to close the form.
			string errors=TimeCardRules.ValidateOvertimeRules();
			if(!string.IsNullOrEmpty(errors)) {
				errors="Fix the following errors:\r\n"+errors;
				MessageBox.Show(errors);
				e.Cancel=true;
			}
			if(textADPCompanyCode.Text!="" && !Regex.IsMatch(textADPCompanyCode.Text,"^[a-zA-Z0-9]{2,3}$")) {
				MsgBox.Show(this,"ADP Company Code must be two or three alpha-numeric characters.\r\nFix or clear before continuing.");
				e.Cancel=true;
			}
			if(Prefs.UpdateString(PrefName.ADPCompanyCode,textADPCompanyCode.Text)) {
				changed=true;
			}
			if(changed) {
				DataValid.SetInvalid(InvalidType.Employees,InvalidType.Prefs,InvalidType.TimeCardRules);
			}
		}
	}
}






















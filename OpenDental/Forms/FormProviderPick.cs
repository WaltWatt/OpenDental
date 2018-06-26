using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary>Pick a provider from the list.</summary>
	public class FormProviderPick:ODForm {
		private OpenDental.UI.Button butClose;
		private IContainer components;
		private OpenDental.UI.ODGrid gridMain;
		//private bool changed;
		//private User user;
		//private DataTable table;
		private OpenDental.UI.Button butOK;
		private Label labelProvNum;
		private TextBox textProvNum;
		///<summary>This can be set ahead of time to preselect a provider.  After closing with OK, this will have the selected provider number.</summary>
		public long SelectedProvNum;
		private GroupBox groupDentalSchools;
		private TextBox textLName;
		private Label label2;
		private CheckBox checkShowAll;
		private TextBox textFName;
		private Label label1;
		private Timer timer1;
		private Label labelClass;
		private ComboBox comboClass;
		private List<SchoolClass> _schoolClasses;
		public bool IsStudentPicker=false;
		private UI.Button butSelectNone;
		///<summary>Setting to true will show a none button and will allow 0 to be returned in the SelectedProvNum variable.  It will be -1 if the user cancels out of the window.</summary>
		public bool IsNoneAvailable=false;
		///<summary>Will be set to a specific list of providers passed in.  Will be null if no defined list of providers is desired.</summary>
		private List<Provider> _listProviders;
		///<summary>Will enable the checkbox that shows all non-hidden providers regardless of schedule, clinic, or what _listProviders was set to initially</summary>
		public bool IsShowAllAvailable = false;
		
		///<summary></summary>
		public FormProviderPick(List<Provider> listProviders=null) {
			InitializeComponent();
			Lan.F(this);
			_listProviders=listProviders;
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProviderPick));
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.labelProvNum = new System.Windows.Forms.Label();
			this.textProvNum = new System.Windows.Forms.TextBox();
			this.groupDentalSchools = new System.Windows.Forms.GroupBox();
			this.labelClass = new System.Windows.Forms.Label();
			this.comboClass = new System.Windows.Forms.ComboBox();
			this.textLName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textFName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.butSelectNone = new OpenDental.UI.Button();
			this.checkShowAll = new System.Windows.Forms.CheckBox();
			this.groupDentalSchools.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(477, 627);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Cancel";
			this.butClose.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(16, 30);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(325, 624);
			this.gridMain.TabIndex = 13;
			this.gridMain.Title = "Providers";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableProviders";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(477, 595);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelProvNum
			// 
			this.labelProvNum.Location = new System.Drawing.Point(6, 19);
			this.labelProvNum.Name = "labelProvNum";
			this.labelProvNum.Size = new System.Drawing.Size(68, 18);
			this.labelProvNum.TabIndex = 27;
			this.labelProvNum.Text = "ProvNum";
			this.labelProvNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProvNum
			// 
			this.textProvNum.Location = new System.Drawing.Point(76, 19);
			this.textProvNum.MaxLength = 15;
			this.textProvNum.Name = "textProvNum";
			this.textProvNum.Size = new System.Drawing.Size(118, 20);
			this.textProvNum.TabIndex = 1;
			this.textProvNum.TextChanged += new System.EventHandler(this.textProvNum_TextChanged);
			// 
			// groupDentalSchools
			// 
			this.groupDentalSchools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupDentalSchools.Controls.Add(this.labelClass);
			this.groupDentalSchools.Controls.Add(this.comboClass);
			this.groupDentalSchools.Controls.Add(this.textLName);
			this.groupDentalSchools.Controls.Add(this.label2);
			this.groupDentalSchools.Controls.Add(this.textFName);
			this.groupDentalSchools.Controls.Add(this.label1);
			this.groupDentalSchools.Controls.Add(this.textProvNum);
			this.groupDentalSchools.Controls.Add(this.labelProvNum);
			this.groupDentalSchools.Location = new System.Drawing.Point(352, 28);
			this.groupDentalSchools.Name = "groupDentalSchools";
			this.groupDentalSchools.Size = new System.Drawing.Size(200, 110);
			this.groupDentalSchools.TabIndex = 1;
			this.groupDentalSchools.TabStop = false;
			this.groupDentalSchools.Text = "Dental School Filters";
			// 
			// labelClass
			// 
			this.labelClass.Location = new System.Drawing.Point(6, 82);
			this.labelClass.Name = "labelClass";
			this.labelClass.Size = new System.Drawing.Size(68, 18);
			this.labelClass.TabIndex = 33;
			this.labelClass.Text = "Class";
			this.labelClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClass
			// 
			this.comboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClass.FormattingEnabled = true;
			this.comboClass.Location = new System.Drawing.Point(76, 82);
			this.comboClass.Name = "comboClass";
			this.comboClass.Size = new System.Drawing.Size(118, 21);
			this.comboClass.TabIndex = 4;
			this.comboClass.SelectionChangeCommitted += new System.EventHandler(this.comboClass_SelectionChangeCommitted);
			// 
			// textLName
			// 
			this.textLName.Location = new System.Drawing.Point(76, 40);
			this.textLName.MaxLength = 15;
			this.textLName.Name = "textLName";
			this.textLName.Size = new System.Drawing.Size(118, 20);
			this.textLName.TabIndex = 2;
			this.textLName.TextChanged += new System.EventHandler(this.textLName_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 18);
			this.label2.TabIndex = 31;
			this.label2.Text = "LName";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFName
			// 
			this.textFName.Location = new System.Drawing.Point(76, 61);
			this.textFName.MaxLength = 15;
			this.textFName.Name = "textFName";
			this.textFName.Size = new System.Drawing.Size(118, 20);
			this.textFName.TabIndex = 3;
			this.textFName.TextChanged += new System.EventHandler(this.textFName_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 18);
			this.label1.TabIndex = 29;
			this.label1.Text = "FName";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// timer1
			// 
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// butSelectNone
			// 
			this.butSelectNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelectNone.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.butSelectNone.Autosize = true;
			this.butSelectNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSelectNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSelectNone.CornerRadius = 4F;
			this.butSelectNone.Location = new System.Drawing.Point(477, 545);
			this.butSelectNone.Name = "butSelectNone";
			this.butSelectNone.Size = new System.Drawing.Size(75, 26);
			this.butSelectNone.TabIndex = 14;
			this.butSelectNone.Text = "None";
			this.butSelectNone.UseVisualStyleBackColor = true;
			this.butSelectNone.Click += new System.EventHandler(this.butSelectNone_Click);
			// 
			// checkShowAll
			// 
			this.checkShowAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAll.Location = new System.Drawing.Point(252, 7);
			this.checkShowAll.Name = "checkShowAll";
			this.checkShowAll.Size = new System.Drawing.Size(89, 21);
			this.checkShowAll.TabIndex = 16;
			this.checkShowAll.Text = "Show All";
			this.checkShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAll.UseVisualStyleBackColor = true;
			this.checkShowAll.CheckedChanged += new System.EventHandler(this.checkShowAll_CheckedChanged);
			// 
			// FormProviderPick
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(574, 670);
			this.Controls.Add(this.checkShowAll);
			this.Controls.Add(this.butSelectNone);
			this.Controls.Add(this.groupDentalSchools);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(481, 234);
			this.Name = "FormProviderPick";
			this.ShowInTaskbar = false;
			this.Text = "Providers";
			this.Load += new System.EventHandler(this.FormProviderSelect_Load);
			this.groupDentalSchools.ResumeLayout(false);
			this.groupDentalSchools.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProviderSelect_Load(object sender, System.EventArgs e) {
			checkShowAll.Visible=IsShowAllAvailable;
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				groupDentalSchools.Visible=false;
			}
			else if(IsStudentPicker) {
				this.Text="Student Picker";
				gridMain.Title="Students";
				_schoolClasses=SchoolClasses.GetDeepCopy();
				for(int i=0;i<_schoolClasses.Count;i++) {
					comboClass.Items.Add(_schoolClasses[i].GradYear+" "+_schoolClasses[i].Descript);
				}
				if(comboClass.Items.Count>0) {
					comboClass.SelectedIndex=0;
				}
			}
			else {
				comboClass.Visible=false;
				labelClass.Visible=false;
			}
			FillGrid();
			if(_listProviders!=null) {
				for(int i=0;i<_listProviders.Count;i++) {
					if(_listProviders[i].ProvNum==SelectedProvNum) {
						gridMain.SetSelected(i,true);
						break;
					}
				}
			}
			else if(SelectedProvNum!=0) {
				gridMain.SetSelected(Providers.GetIndex(SelectedProvNum),true);
			}
			butSelectNone.Visible=IsNoneAvailable;
			if(IsNoneAvailable) {
				//Default value for the selected provider when none is an option is always -1
				SelectedProvNum=-1;
			}
		}

		private void FillGrid(){
			long provNum;
			if(!long.TryParse(textProvNum.Text,out provNum)) {
				provNum=0;
			}
			long classNum=0;
			if(IsStudentPicker) {
				classNum=_schoolClasses[comboClass.SelectedIndex].SchoolClassNum;
			}
			List<Provider> listProvs;
			if(_listProviders!=null && !checkShowAll.Checked) {//User wants to use a specific list of providers.
				listProvs=_listProviders;
			}
			else {
				listProvs=Providers.GetFilteredProviderList(provNum,textLName.Text,textFName.Text,classNum);
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				col=new ODGridColumn(Lan.g("TableProviders","ProvNum"),60);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableProviders","Abbrev"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviders","LName"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviders","FName"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<listProvs.Count;i++) {
				if(IsStudentPicker && listProvs[i].SchoolClassNum==0) {
					continue;
				}
				row=new ODGridRow();
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					row.Cells.Add(listProvs[i].ProvNum.ToString());
				}
				row.Cells.Add(listProvs[i].Abbr);
				row.Cells.Add(listProvs[i].LName);
				row.Cells.Add(listProvs[i].FName);
				row.Tag=listProvs[i].ProvNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SelectedProvNum=PIn.Long(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString());
			DialogResult=DialogResult.OK;
		}

		private void timer1_Tick(object sender,EventArgs e) {
			timer1.Stop();
			FillGrid();
		}

		private void textProvNum_TextChanged(object sender,EventArgs e) {
			timer1.Stop();
			timer1.Start();
		}

		private void textLName_TextChanged(object sender,EventArgs e) {
			timer1.Stop();
			timer1.Start();
		}

		private void textFName_TextChanged(object sender,EventArgs e) {
			timer1.Stop();
			timer1.Start();
		}

		private void comboClass_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkShowAll_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a provider first.");
				return;
			}
			SelectedProvNum=PIn.Long(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString());
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butSelectNone_Click(object sender,EventArgs e) {
			SelectedProvNum=0;
			DialogResult=DialogResult.OK;
		}
		



	

	}
}

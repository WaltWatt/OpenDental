using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public class FormReferralSelect:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.CheckBox checkHidden;
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsSelectionMode;
		///<summary>Used when coming from FormEHR if a Transition of Care is needed for a reconcile.</summary>
		public bool IsDoctorSelectionMode;
		private OpenDental.UI.Button butAdd;
		private List<Referral> listRef;
		private UI.ODGrid gridMain;
		private TextBox textSearch;
		private Label label1;
		private Label labelResultCount;
		private GroupBox groupBox1;
		private CheckBox checkShowOther;
		private CheckBox checkShowDoctor;
		private CheckBox checkShowPat;
		///<summary>This will contain the referral that was selected.</summary>
		public Referral SelectedReferral;
		///<summary>True by default.  Set to false if the results should exclude patient referral sources.
		///The show patient check box is set based on the value of this bool.</summary>
		public bool IsShowPat=true;
		///<summary>True by default.  Set to false if the results should exclude doctor referral sources.
		///The show doctor check box is set based on the value of this bool.</summary>
		public bool IsShowDoc=true;
		private CheckBox checkPreferred;
		///<summary>True by default.  Set to false if the results should exclude non-patient non-doctor referral sources.
		///The show other check box is set based on the value of this bool.</summary>
		public bool IsShowOther=true;

		///<summary></summary>
		public FormReferralSelect() {
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReferralSelect));
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.labelResultCount = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkPreferred = new System.Windows.Forms.CheckBox();
			this.checkShowOther = new System.Windows.Forms.CheckBox();
			this.checkShowDoctor = new System.Windows.Forms.CheckBox();
			this.checkShowPat = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkHidden
			// 
			this.checkHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(815, 17);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(104, 16);
			this.checkHidden.TabIndex = 11;
			this.checkHidden.Text = "Show Hidden";
			this.checkHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.Click += new System.EventHandler(this.checkHidden_Click);
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
			this.gridMain.Location = new System.Drawing.Point(8, 42);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(940, 610);
			this.gridMain.TabIndex = 15;
			this.gridMain.Title = "Select Referral";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableSelectReferral";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(106, 14);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(201, 20);
			this.textSearch.TabIndex = 0;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 14);
			this.label1.TabIndex = 17;
			this.label1.Text = "Search";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelResultCount
			// 
			this.labelResultCount.Location = new System.Drawing.Point(308, 17);
			this.labelResultCount.Name = "labelResultCount";
			this.labelResultCount.Size = new System.Drawing.Size(108, 14);
			this.labelResultCount.TabIndex = 18;
			this.labelResultCount.Text = "# results found";
			this.labelResultCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.butAdd.Location = new System.Drawing.Point(8, 661);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80, 24);
			this.butAdd.TabIndex = 12;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
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
			this.butCancel.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.butCancel.Location = new System.Drawing.Point(873, 661);
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
			this.butOK.Location = new System.Drawing.Point(785, 661);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkPreferred);
			this.groupBox1.Controls.Add(this.checkShowOther);
			this.groupBox1.Controls.Add(this.checkShowDoctor);
			this.groupBox1.Controls.Add(this.checkShowPat);
			this.groupBox1.Location = new System.Drawing.Point(399, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(370, 33);
			this.groupBox1.TabIndex = 19;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Show";
			// 
			// checkPreferred
			// 
			this.checkPreferred.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreferred.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPreferred.Location = new System.Drawing.Point(261, 13);
			this.checkPreferred.Name = "checkPreferred";
			this.checkPreferred.Size = new System.Drawing.Size(94, 16);
			this.checkPreferred.TabIndex = 23;
			this.checkPreferred.Text = "Preferred Only";
			this.checkPreferred.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreferred.Click += new System.EventHandler(this.checkPreferred_Click);
			// 
			// checkShowOther
			// 
			this.checkShowOther.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowOther.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowOther.Location = new System.Drawing.Point(177, 13);
			this.checkShowOther.Name = "checkShowOther";
			this.checkShowOther.Size = new System.Drawing.Size(75, 16);
			this.checkShowOther.TabIndex = 20;
			this.checkShowOther.Text = "Other";
			this.checkShowOther.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowOther.Click += new System.EventHandler(this.checkShowOther_Click);
			// 
			// checkShowDoctor
			// 
			this.checkShowDoctor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowDoctor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowDoctor.Location = new System.Drawing.Point(89, 13);
			this.checkShowDoctor.Name = "checkShowDoctor";
			this.checkShowDoctor.Size = new System.Drawing.Size(84, 16);
			this.checkShowDoctor.TabIndex = 21;
			this.checkShowDoctor.Text = "Doctor";
			this.checkShowDoctor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowDoctor.Click += new System.EventHandler(this.checkShowDoctor_Click);
			// 
			// checkShowPat
			// 
			this.checkShowPat.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowPat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowPat.Location = new System.Drawing.Point(3, 13);
			this.checkShowPat.Name = "checkShowPat";
			this.checkShowPat.Size = new System.Drawing.Size(84, 16);
			this.checkShowPat.TabIndex = 22;
			this.checkShowPat.Text = "Patient";
			this.checkShowPat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowPat.Click += new System.EventHandler(this.checkShowPat_Click);
			// 
			// FormReferralSelect
			// 
			this.ClientSize = new System.Drawing.Size(962, 696);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.labelResultCount);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormReferralSelect";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Referrals";
			this.Load += new System.EventHandler(this.FormReferralSelect_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormReferralSelect_Load(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ReferralAdd,true)) {
				butAdd.Enabled=false;
			}
			checkShowPat.Checked=IsShowPat;
			checkShowDoctor.Checked=IsShowDoc;
			checkShowOther.Checked=IsShowOther;
			checkPreferred.Checked=PrefC.GetBool(PrefName.ShowPreferedReferrals);
			FillTable();
			//labelResultCount.Text="";
		}

		private void FillTable() {
			Referrals.RefreshCache();
			listRef=Referrals.GetDeepCopy();
			if(!checkHidden.Checked) {
				listRef.RemoveAll(x => x.IsHidden);
			}
			if(!checkShowPat.Checked) {
				listRef.RemoveAll(x => x.PatNum>0);
			}
			if(!checkShowDoctor.Checked) {
				listRef.RemoveAll(x => x.IsDoctor);
			}
			if(!checkShowOther.Checked) {
				listRef.RemoveAll(x => x.PatNum==0 && !x.IsDoctor);
			}
			if(checkPreferred.Checked) {
				listRef.RemoveAll(x => !x.IsPreferred);
			}
			if(!string.IsNullOrWhiteSpace(textSearch.Text)) {
				string[] searchTokens=textSearch.Text.ToLower().Split(new[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
				listRef.RemoveAll(x => searchTokens.Any(y => !x.FName.ToLower().Contains(y) && !x.LName.ToLower().Contains(y)));
			}
			int scrollValue=gridMain.ScrollValue;
			long selectedRefNum=-1;
			if(gridMain.GetSelectedIndex()>-1) {
				selectedRefNum=((Referral)gridMain.Rows[gridMain.GetSelectedIndex()].Tag).ReferralNum;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableSelectRefferal","LastName"),150));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableSelectRefferal","FirstName"),80));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableSelectRefferal","MI"),30));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableSelectRefferal","Title"),70));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableSelectRefferal","Specialty"),60));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableSelectRefferal","Patient"),45));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableSelectRefferal","Note"),250));
			gridMain.Rows.Clear();
			ODGridRow row;
			int indexSelectedRef=-1;
			foreach(Referral refCur in listRef) {
				row=new ODGridRow();
				row.Cells.Add(refCur.LName);
				row.Cells.Add(refCur.FName);
				row.Cells.Add(refCur.MName.Left(1).ToUpper());//Left(1) will return empty string if MName is null or empty string, so ToUpper is null safe
				row.Cells.Add(refCur.Title);
				row.Cells.Add(refCur.IsDoctor?Lan.g("enumDentalSpecialty",Defs.GetName(DefCat.ProviderSpecialties,refCur.Specialty)):"");
				row.Cells.Add(refCur.PatNum>0?"X":"");
				row.Cells.Add(refCur.Note);
				if(refCur.IsHidden) {
					row.ColorText=Color.Gray;
				}
				row.Tag=refCur;
				gridMain.Rows.Add(row);
				if(refCur.ReferralNum==selectedRefNum) {
					indexSelectedRef=gridMain.Rows.Count-1;
				}
			}
			gridMain.EndUpdate();
			if(indexSelectedRef>-1) {
				gridMain.SetSelected(indexSelectedRef,true);
			}
			gridMain.ScrollValue=scrollValue;
			labelResultCount.Text=gridMain.Rows.Count.ToString()+Lan.g(this," results found");
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//This does not automatically select a referral when in selection mode; it just lets user edit.
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a referral first");
				return;
			}
			FormReferralEdit FormRE = new FormReferralEdit(listRef[e.Row]);
			FormRE.ShowDialog();
			if(FormRE.DialogResult!=DialogResult.OK) {
				return;
			}
			//int selectedIndex=gridMain.GetSelectedIndex();
			FillTable();
			//gridMain.SetSelected(selectedIndex,true);
		}

		private void butAdd_Click(object sender,System.EventArgs e) {
			Referral refCur=new Referral();
			bool referralIsNew=true;
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Is the referral source an existing patient?"))	{
				FormPatientSelect FormPS=new FormPatientSelect();
				FormPS.SelectionModeOnly=true;
				FormPS.ShowDialog();
				if(FormPS.DialogResult!=DialogResult.OK) {
					return;
				}
				refCur.PatNum=FormPS.SelectedPatNum;
				Referral referral=Referrals.GetFirstOrDefault(x => x.PatNum==FormPS.SelectedPatNum);
				if(referral!=null) {
					refCur=referral;
					referralIsNew=false;
				}
			}
			FormReferralEdit FormRE2=new FormReferralEdit(refCur);//the ReferralNum must be added here
			FormRE2.IsNew=referralIsNew;
			FormRE2.ShowDialog();
			if(FormRE2.DialogResult==DialogResult.Cancel) {
				return;
			}
			if(IsSelectionMode) {
				if(IsDoctorSelectionMode && !FormRE2.RefCur.IsDoctor) {
					MsgBox.Show(this,"Please select a doctor referral.");
					gridMain.SetSelected(false);//Remove selection to prevent caching issue on OK click.  This line is an attempted fix.
					FillTable();
					return;
				}
				SelectedReferral=FormRE2.RefCur;
				DialogResult=DialogResult.OK;
				return;
			}
			else {
				FillTable();
				for(int i=0;i<listRef.Count;i++) {
					if(listRef[i].ReferralNum==FormRE2.RefCur.ReferralNum) {
						gridMain.SetSelected(i,true);
					}
				}
			}
		}

		private void checkHidden_Click(object sender,System.EventArgs e) {
			FillTable();
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillTable();
		}

		private void checkShowPat_Click(object sender,EventArgs e) {
			FillTable();
		}

		private void checkShowDoctor_Click(object sender,EventArgs e) {
			FillTable();
		}

		private void checkShowOther_Click(object sender,EventArgs e) {
			FillTable();
		}

		private void checkPreferred_Click(object sender,EventArgs e) {
			FillTable();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(IsSelectionMode) {
				if(gridMain.GetSelectedIndex()==-1) {
					MsgBox.Show(this,"Please select a referral first");
					return;
				}
				if(IsDoctorSelectionMode && listRef[gridMain.GetSelectedIndex()].IsDoctor==false) {
					MsgBox.Show(this,"Please select a doctor referral.");
					return;
				}
				SelectedReferral=(Referral)listRef[gridMain.GetSelectedIndex()];
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}

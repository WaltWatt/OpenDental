using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenDental{
///<summary></summary>
	public class FormQueryFavorites:ODForm {
		#region Form Controls
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private IContainer components;
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butEdit;
		private UI.ODGrid gridMain;
		private SplitContainer splitContainer1;
		private UI.Button butShowHide;
		private ODtextBox textQuery;
		private CheckBox checkWrapText;
		#endregion
		private List<UserQuery> _listQueries;
		private Label label5;
		private TextBox textSearch;
		public UserQuery UserQueryCur;

		///<summary></summary>
		public FormQueryFavorites() {
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQueryFavorites));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butEdit = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.butShowHide = new OpenDental.UI.Button();
			this.checkWrapText = new System.Windows.Forms.CheckBox();
			this.textQuery = new OpenDental.ODtextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textSearch = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(898, 622);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
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
			this.butCancel.Location = new System.Drawing.Point(979, 622);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
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
			this.butAdd.Location = new System.Drawing.Point(367, 568);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(82, 24);
			this.butAdd.TabIndex = 34;
			this.butAdd.Text = "&New";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
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
			this.butDelete.Location = new System.Drawing.Point(5, 568);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(82, 24);
			this.butDelete.TabIndex = 35;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Image = global::OpenDental.Properties.Resources.editPencil;
			this.butEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEdit.Location = new System.Drawing.Point(279, 568);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(82, 24);
			this.butEdit.TabIndex = 36;
			this.butEdit.Text = "Edit";
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
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
			this.gridMain.Location = new System.Drawing.Point(6, 55);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(443, 509);
			this.gridMain.TabIndex = 38;
			this.gridMain.Title = "";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 0;
			this.gridMain.TranslationName = "TableQueryFavorites";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(5, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.textSearch);
			this.splitContainer1.Panel1.Controls.Add(this.butShowHide);
			this.splitContainer1.Panel1.Controls.Add(this.butDelete);
			this.splitContainer1.Panel1.Controls.Add(this.gridMain);
			this.splitContainer1.Panel1.Controls.Add(this.butAdd);
			this.splitContainer1.Panel1.Controls.Add(this.butEdit);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.checkWrapText);
			this.splitContainer1.Panel2.Controls.Add(this.textQuery);
			this.splitContainer1.Size = new System.Drawing.Size(1079, 613);
			this.splitContainer1.SplitterDistance = 454;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 39;
			// 
			// butShowHide
			// 
			this.butShowHide.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowHide.Autosize = true;
			this.butShowHide.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowHide.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowHide.CornerRadius = 4F;
			this.butShowHide.Location = new System.Drawing.Point(367, 3);
			this.butShowHide.Name = "butShowHide";
			this.butShowHide.Size = new System.Drawing.Size(82, 25);
			this.butShowHide.TabIndex = 40;
			this.butShowHide.Text = "Show Text >";
			this.butShowHide.UseVisualStyleBackColor = true;
			this.butShowHide.Click += new System.EventHandler(this.butShowHide_Click);
			// 
			// checkWrapText
			// 
			this.checkWrapText.Location = new System.Drawing.Point(0, 8);
			this.checkWrapText.Name = "checkWrapText";
			this.checkWrapText.Size = new System.Drawing.Size(227, 24);
			this.checkWrapText.TabIndex = 1;
			this.checkWrapText.Text = "Wrap Text";
			this.checkWrapText.UseVisualStyleBackColor = true;
			this.checkWrapText.CheckedChanged += new System.EventHandler(this.checkWrapText_CheckedChanged);
			// 
			// textQuery
			// 
			this.textQuery.AcceptsTab = true;
			this.textQuery.BackColor = System.Drawing.SystemColors.Control;
			this.textQuery.DetectLinksEnabled = false;
			this.textQuery.DetectUrls = false;
			this.textQuery.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.textQuery.Location = new System.Drawing.Point(0, 31);
			this.textQuery.Name = "textQuery";
			this.textQuery.QuickPasteType = OpenDentBusiness.QuickPasteType.ReadOnly;
			this.textQuery.ReadOnly = true;
			this.textQuery.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textQuery.Size = new System.Drawing.Size(622, 580);
			this.textQuery.TabIndex = 0;
			this.textQuery.Text = "";
			this.textQuery.WordWrap = false;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(6, 15);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(173, 16);
			this.label5.TabIndex = 163;
			this.label5.Text = "Search:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(6, 32);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(443, 20);
			this.textSearch.TabIndex = 162;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// FormQueryFavorites
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(1084, 656);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormQueryFavorites";
			this.ShowInTaskbar = false;
			this.Text = "Query Favorites";
			this.Load += new System.EventHandler(this.FormQueryFormulate_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormQueryFormulate_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQueryAdmin,true)) { //disable controls for users without permission.
				butEdit.Enabled=false;
				butDelete.Enabled=false;
				butAdd.Enabled=false;
			}
			//hide the query text by default.
			Width=500;
			splitContainer1.Panel2Collapsed=true;
			FillGrid();
			textSearch.Select();
		}

		private void FillGrid(bool refreshList=true,bool isScrollToSelection=true) {
			if(refreshList) {
				_listQueries=UserQueries.GetDeepCopy();
			}
			string[] strSearchTerms = Regex.Split(textSearch.Text,@"\W");//matches any non-word character
			//get all queries that contain ALL of the search terms entered, either in the query text or the query description.
			List<UserQuery> listDisplayQueries = _listQueries
				.Where(x => strSearchTerms.All(y => 
					x.QueryText.ToLowerInvariant().Contains(y.ToLowerInvariant()) || x.Description.ToLowerInvariant().Contains(y.ToLowerInvariant())
				)).ToList();
			//attempt to preserve the currently selected query.
			long selectedQueryNum=0;
			if(gridMain.GetSelectedIndex() != -1) {
				selectedQueryNum=gridMain.SelectedTag<UserQuery>().QueryNum;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g(gridMain.TranslationName,"Query"),350));
			if(Security.IsAuthorized(Permissions.UserQueryAdmin,true)) {
				gridMain.Columns.Add(new ODGridColumn(Lan.g(gridMain.TranslationName,"Released"),55,HorizontalAlignment.Center));
			}
			gridMain.Rows.Clear();
			foreach(UserQuery queryCur in listDisplayQueries) {
				if(!Security.IsAuthorized(Permissions.UserQueryAdmin,true) && !queryCur.IsReleased) {
					continue; //non-released queries only appear for people with UserQueryAdmin permission.
				}
				ODGridRow row = new ODGridRow();
				row.Cells.Add(queryCur.Description);
				if(Security.IsAuthorized(Permissions.UserQueryAdmin,true)) {
					row.Cells.Add(queryCur.IsReleased ? "X" : "");
				}
				row.Tag = queryCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			int selectedIdx=gridMain.Rows.Select(x => (UserQuery)x.Tag).ToList().FindIndex(y => y.QueryNum==selectedQueryNum);
			if(selectedIdx>-1) {
				gridMain.SetSelected(selectedIdx,true);
			}
			if(gridMain.GetSelectedIndex()==-1) {
				gridMain.SetSelected(0,true); //can handle values outside of the row count (so if there are no rows, this will not fail)
			}
			if(isScrollToSelection) {
				gridMain.ScrollToIndex(gridMain.GetSelectedIndex()); //can handle values outside of the row count
			}
			RefreshQueryCur();
		}

		///<summary>Refreshes UserQueryCur and fills the textbox.</summary>
		private void RefreshQueryCur() {
			UserQueryCur = null;
			textQuery.Text="";
			if(gridMain.GetSelectedIndex() != -1) {
				UserQueryCur = (UserQuery)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
				textQuery.Text=UserQueryCur.QueryText;
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==1) {//Released Column
				UserQuery query=gridMain.SelectedTag<UserQuery>();
				query.IsReleased=(!query.IsReleased);
				UserQueries.Update(query);
				DataValid.SetInvalid(InvalidType.UserQueries);
				FillGrid(true,false);//Results in RefreshQueryCur()
				return;
			}
			RefreshQueryCur();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(UserQueryCur == null) {
				MsgBox.Show(this,"Please select an item first."); //should never happen.
				return;
			}
			ReportSimpleGrid report=new ReportSimpleGrid();
			if(UserQueryCur.IsPromptSetup && UserQueries.ParseSetStatements(UserQueryCur.QueryText).Count > 0) {
				//if the user is not a query admin, they will not have the ability to edit 
				//the query before it is run, so show them the SET statement edit window.
				FormQueryParser FormQP = new FormQueryParser(UserQueryCur);
				FormQP.ShowDialog();
				if(FormQP.DialogResult==DialogResult.OK) {
					report.Query=UserQueryCur.QueryText;
					DialogResult=DialogResult.OK;
				}
			}
			else {
				//user has permission to edit the query, so just run the query.
				DialogResult=DialogResult.OK;
			}
		}

		private void butEdit_Click(object sender,EventArgs e) {
			//button is disabled for users without Query Admin permission.
			if(UserQueryCur==null) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			FormQueryEdit FormQE=new FormQueryEdit();
			FormQE.UserQueryCur=UserQueryCur;
			FormQE.IsNew=false;
			FormQE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			//button is disabled for users without Query Admin permission.
			FormQueryEdit FormQE=new FormQueryEdit();
			FormQE.IsNew=true;
			FormQE.UserQueryCur=new UserQuery();
			FormQE.ShowDialog();
			if(FormQE.DialogResult==DialogResult.OK){
				FillGrid();
			}
		}

		private void butShowHide_Click(object sender,EventArgs e) {
			splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
			if(splitContainer1.Panel2Collapsed) {
				Width = 500;
				butShowHide.Text=Lan.g(this,"Show Text") +" >";
			}
			else {
				Width = 1100;
				butShowHide.Text=Lan.g(this,"Hide Text") + " <";
			}
		}

		private void checkWrapText_CheckedChanged(object sender,EventArgs e) {
			textQuery.WordWrap = checkWrapText.Checked;
		}		

		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillGrid(false);
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			//button is disabled for users without Query Admin permission.
			if(UserQueryCur==null){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete Item?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			UserQueries.Delete(UserQueryCur);
			DataValid.SetInvalid(InvalidType.UserQueries);
			gridMain.SetSelected(false);
			UserQueryCur=null;
			FillGrid();
			textQuery.Text="";
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(UserQueryCur == null){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			ReportSimpleGrid report=new ReportSimpleGrid();
			if(UserQueryCur.IsPromptSetup && UserQueries.ParseSetStatements(UserQueryCur.QueryText).Count > 0) {
				//if the user is not a query admin, they will not have the ability to edit 
				//the query before it is run, so show them the SET statement edit window.
				FormQueryParser FormQP = new FormQueryParser(UserQueryCur);
				FormQP.ShowDialog();
				if(FormQP.DialogResult==DialogResult.OK) {
					report.Query=UserQueryCur.QueryText;
					DialogResult=DialogResult.OK;
				}
			}
			else {
				//user has permission to edit the query, so just run the query.
				DialogResult=DialogResult.OK;
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}

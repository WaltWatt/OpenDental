using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary></summary>
	public class FormHL7Defs:ODForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components=null;
		private UI.Button butClose;
		private UI.Button butCopy;
		private ODGrid grid2;
		private ODGrid grid1;
		List<HL7Def> ListInternal;
		private UI.Button butDuplicate;
		private UI.Button butHistory;
		List<HL7Def> ListCustom;
		///<summary>This gets set externally beforehand.  This is passed to FormHL7Msgs for loading the HL7 messages for the currently selected patient.</summary>
		public long CurPatNum;

		///<summary></summary>
		public FormHL7Defs()
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
				if(components!=null)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHL7Defs));
			this.grid1 = new OpenDental.UI.ODGrid();
			this.grid2 = new OpenDental.UI.ODGrid();
			this.butDuplicate = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butHistory = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.grid1.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.grid1.HasAddButton = false;
			this.grid1.HasDropDowns = false;
			this.grid1.HasMultilineHeaders = false;
			this.grid1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.grid1.HeaderHeight = 15;
			this.grid1.HScrollVisible = false;
			this.grid1.Location = new System.Drawing.Point(12, 38);
			this.grid1.Name = "grid1";
			this.grid1.ScrollValue = 0;
			this.grid1.Size = new System.Drawing.Size(470, 559);
			this.grid1.TabIndex = 14;
			this.grid1.Title = "Internal";
			this.grid1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.grid1.TitleHeight = 18;
			this.grid1.TranslationName = "TableInternal";
			this.grid1.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid1_CellDoubleClick);
			// 
			// grid2
			// 
			this.grid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid2.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.grid2.HasAddButton = false;
			this.grid2.HasDropDowns = false;
			this.grid2.HasMultilineHeaders = false;
			this.grid2.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.grid2.HeaderHeight = 15;
			this.grid2.HScrollVisible = false;
			this.grid2.Location = new System.Drawing.Point(491, 38);
			this.grid2.Name = "grid2";
			this.grid2.ScrollValue = 0;
			this.grid2.Size = new System.Drawing.Size(470, 559);
			this.grid2.TabIndex = 12;
			this.grid2.Title = "Custom";
			this.grid2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.grid2.TitleHeight = 18;
			this.grid2.TranslationName = "TableCustom";
			this.grid2.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid2_CellDoubleClick);
			// 
			// butDuplicate
			// 
			this.butDuplicate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDuplicate.Autosize = true;
			this.butDuplicate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDuplicate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDuplicate.CornerRadius = 4F;
			this.butDuplicate.Image = global::OpenDental.Properties.Resources.Add;
			this.butDuplicate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDuplicate.Location = new System.Drawing.Point(737, 607);
			this.butDuplicate.Name = "butDuplicate";
			this.butDuplicate.Size = new System.Drawing.Size(89, 24);
			this.butDuplicate.TabIndex = 20;
			this.butDuplicate.Text = "Duplicate";
			this.butDuplicate.Click += new System.EventHandler(this.butDuplicate_Click);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Image = global::OpenDental.Properties.Resources.Right;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(333, 607);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 15;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 607);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butHistory
			// 
			this.butHistory.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butHistory.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butHistory.Autosize = true;
			this.butHistory.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHistory.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHistory.CornerRadius = 4F;
			this.butHistory.Location = new System.Drawing.Point(38, 8);
			this.butHistory.Name = "butHistory";
			this.butHistory.Size = new System.Drawing.Size(75, 24);
			this.butHistory.TabIndex = 21;
			this.butHistory.Text = "History";
			this.butHistory.Click += new System.EventHandler(this.butHistory_Click);
			// 
			// FormHL7Defs
			// 
			this.ClientSize = new System.Drawing.Size(974, 641);
			this.Controls.Add(this.butHistory);
			this.Controls.Add(this.butDuplicate);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.grid1);
			this.Controls.Add(this.grid2);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormHL7Defs";
			this.ShowInTaskbar = false;
			this.Text = "HL7 Defs";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHL7Defs_FormClosing);
			this.Load += new System.EventHandler(this.FormHL7Defs_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormHL7Defs_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup,true)){
				butCopy.Enabled=false;
				grid2.Enabled=false;
				grid1.Enabled=false;
			}
			FillGrid1();
			FillGrid2();			
		}

		private void FillGrid1() {
			//Our strategy in this window and all sub windows is to get all data directly from the database (or internal).
			ListInternal=HL7Defs.GetDeepInternalList();
			grid1.BeginUpdate();
			grid1.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Description"),100);
			grid1.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Mode"),40);
			grid1.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"In Folder / Socket"),130);
			grid1.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Out Folder / Socket"),130);
			grid1.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Enabled"),35);
			grid1.Columns.Add(col);
			grid1.Rows.Clear();
			for(int i=0;i<ListInternal.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(ListInternal[i].Description);
				row.Cells.Add(Lan.g("enumModeTxHL7",ListInternal[i].ModeTx.ToString()));
				if(ListInternal[i].ModeTx==ModeTxHL7.File) {
					row.Cells.Add(ListInternal[i].IncomingFolder);
					row.Cells.Add(ListInternal[i].OutgoingFolder);
				}
				else if(ListInternal[i].ModeTx==ModeTxHL7.TcpIp) {
					row.Cells.Add(ListInternal[i].HL7Server+":"+ListInternal[i].IncomingPort);
					row.Cells.Add(ListInternal[i].OutgoingIpPort);
				}
				else {//Sftp
					row.Cells.Add(ListInternal[i].SftpInSocket);
					row.Cells.Add("N/A");
				}
				row.Cells.Add(ListInternal[i].IsEnabled?"X":"");
				grid1.Rows.Add(row);
			}
			grid1.EndUpdate();
		}

		private void FillGrid2() {
			//Our strategy in this window and all sub windows is to get all data directly from the database.
			//If it's too slow in this window due to the 20-30 database calls per row in grid2, then we might later optimize to pull from the cache.
			ListCustom=HL7Defs.GetDeepCustomList();
			grid2.BeginUpdate();
			grid2.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Description"),100);
			grid2.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Mode"),40);
			grid2.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"In Folder / Socket"),130);
			grid2.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Out Folder / Socket"),130);
			grid2.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Enabled"),35);
			grid2.Columns.Add(col);
			grid2.Rows.Clear();
			for(int i=0;i<ListCustom.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(ListCustom[i].Description);
				row.Cells.Add(Lan.g("enumModeTxHL7",ListCustom[i].ModeTx.ToString()));
				if(ListCustom[i].ModeTx==ModeTxHL7.File) {
					row.Cells.Add(ListCustom[i].IncomingFolder);
					row.Cells.Add(ListCustom[i].OutgoingFolder);
				}
				else if(ListCustom[i].ModeTx==ModeTxHL7.TcpIp) {
					row.Cells.Add(ListCustom[i].HL7Server+":"+ListCustom[i].IncomingPort);
					row.Cells.Add(ListCustom[i].OutgoingIpPort);
				}
				else {//Sftp
					row.Cells.Add(ListCustom[i].SftpInSocket);
					row.Cells.Add("N/A");
				}
				row.Cells.Add(ListCustom[i].IsEnabled?"X":"");
				grid2.Rows.Add(row);
			}
			grid2.EndUpdate();
		}

		private void butDuplicate_Click(object sender,System.EventArgs e) {
			if(grid2.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a Custom HL7Def from the list on the right first.");
				return;
			}
			HL7Def hl7def=ListCustom[grid2.GetSelectedIndex()].Clone();
			hl7def.IsEnabled=false;
			long hl7DefNum=HL7Defs.Insert(hl7def);
			for(int m=0;m<hl7def.hl7DefMessages.Count;m++) {
				hl7def.hl7DefMessages[m].HL7DefNum=hl7DefNum;
				long hl7DefMessageNum=HL7DefMessages.Insert(hl7def.hl7DefMessages[m]);
				for(int s=0;s<hl7def.hl7DefMessages[m].hl7DefSegments.Count;s++) {
					hl7def.hl7DefMessages[m].hl7DefSegments[s].HL7DefMessageNum=hl7DefMessageNum;
					long hl7DefSegmentNum=HL7DefSegments.Insert(hl7def.hl7DefMessages[m].hl7DefSegments[s]);
					for(int f=0;f<hl7def.hl7DefMessages[m].hl7DefSegments[s].hl7DefFields.Count;f++) {
						hl7def.hl7DefMessages[m].hl7DefSegments[s].hl7DefFields[f].HL7DefSegmentNum=hl7DefSegmentNum;
						HL7DefFields.Insert(hl7def.hl7DefMessages[m].hl7DefSegments[s].hl7DefFields[f]);
					}
				}
			}
			DataValid.SetInvalid(InvalidType.HL7Defs);
			FillGrid2();
			grid2.SetSelected(false);
		}

		private void butCopy_Click(object sender,EventArgs e) {
			if(grid1.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an internal HL7Def from the list on the left first.");
				return;
			}
			HL7Def hl7def=ListInternal[grid1.GetSelectedIndex()].Clone();
			hl7def.IsInternal=false;
			hl7def.IsEnabled=false;
			long hl7DefNum=HL7Defs.Insert(hl7def);
			for(int m=0;m<hl7def.hl7DefMessages.Count;m++) {
				hl7def.hl7DefMessages[m].HL7DefNum=hl7DefNum;
				long hl7DefMessageNum=HL7DefMessages.Insert(hl7def.hl7DefMessages[m]);
				for(int s=0;s<hl7def.hl7DefMessages[m].hl7DefSegments.Count;s++) {
					hl7def.hl7DefMessages[m].hl7DefSegments[s].HL7DefMessageNum=hl7DefMessageNum;
					long hl7DefSegmentNum=HL7DefSegments.Insert(hl7def.hl7DefMessages[m].hl7DefSegments[s]);
					for(int f=0;f<hl7def.hl7DefMessages[m].hl7DefSegments[s].hl7DefFields.Count;f++) {
						hl7def.hl7DefMessages[m].hl7DefSegments[s].hl7DefFields[f].HL7DefSegmentNum=hl7DefSegmentNum;
						HL7DefFields.Insert(hl7def.hl7DefMessages[m].hl7DefSegments[s].hl7DefFields[f]);
					}
				}
			}
			DataValid.SetInvalid(InvalidType.HL7Defs);
			FillGrid2();
			grid1.SetSelected(false);
		}

		private void butHistory_Click(object sender,EventArgs e) {
			FormHL7Msgs FormS=new FormHL7Msgs();
			FormS.CurPatNum=CurPatNum;
			FormS.ShowDialog();
			FillGrid1();
			FillGrid2();
		}

		private void grid1_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormHL7DefEdit FormS=new FormHL7DefEdit();
			FormS.HL7DefCur=ListInternal[e.Row];
			FormS.ShowDialog();
			FillGrid1();
			FillGrid2();	
		}

		private void grid2_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormHL7DefEdit FormS=new FormHL7DefEdit();
			FormS.HL7DefCur=ListCustom[e.Row];
			FormS.ShowDialog();
			FillGrid1();
			FillGrid2();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormHL7Defs_FormClosing(object sender,FormClosingEventArgs e) {
			DataValid.SetInvalid(InvalidType.HL7Defs);
			DataValid.SetInvalid(InvalidType.Prefs);
			DataValid.SetInvalid(InvalidType.ToolBut);
		}

	}
}




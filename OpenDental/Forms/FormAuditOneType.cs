using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>This form shows all of the security log entries for one fKey item. So far this only applies to a single appointment or a single procedure code.</summary>
	public class FormAuditOneType : ODForm {
		private OpenDental.UI.ODGrid grid;
		private long PatNum;
		private Label labelDisclaimer;
		private List <Permissions> PermTypes;
		private long FKey;
		private CheckBox checkIncludeArchived;

		///<summary>LogList can be filled before loading the window with a custom log list or it will get automatically filled upon load if left emtpy.  Used for showing mixtures of generic audit entries and FK entries.  Viewing specific ortho chart visit audits need to always have patient field changes.</summary>
		public SecurityLog[] LogList;

		///<summary>Supply the patient, types, and title.</summary>
		public FormAuditOneType(long patNum,List<Permissions> permTypes,string title,long fKey) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			Text=title;
			PatNum=patNum;
			PermTypes=new List<Permissions>(permTypes);
			FKey=fKey;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAuditOneType));
			this.grid = new OpenDental.UI.ODGrid();
			this.labelDisclaimer = new System.Windows.Forms.Label();
			this.checkIncludeArchived = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.grid.HasAddButton = false;
			this.grid.HasDropDowns = false;
			this.grid.HasMultilineHeaders = false;
			this.grid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.grid.HeaderHeight = 15;
			this.grid.HScrollVisible = false;
			this.grid.Location = new System.Drawing.Point(8, 21);
			this.grid.Name = "grid";
			this.grid.ScrollValue = 0;
			this.grid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.grid.Size = new System.Drawing.Size(889, 602);
			this.grid.TabIndex = 2;
			this.grid.Title = "Audit Trail";
			this.grid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.grid.TitleHeight = 18;
			this.grid.TranslationName = "TableAudit";
			// 
			// labelDisclaimer
			// 
			this.labelDisclaimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDisclaimer.ForeColor = System.Drawing.Color.Firebrick;
			this.labelDisclaimer.Location = new System.Drawing.Point(8, 3);
			this.labelDisclaimer.Name = "labelDisclaimer";
			this.labelDisclaimer.Size = new System.Drawing.Size(780, 15);
			this.labelDisclaimer.TabIndex = 3;
			this.labelDisclaimer.Text = "Changes made to this appointment before the update to 12.3 will not be reflected " +
    "below, but can be found in the regular audit trail.";
			this.labelDisclaimer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkIncludeArchived
			// 
			this.checkIncludeArchived.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeArchived.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIncludeArchived.Location = new System.Drawing.Point(755, 2);
			this.checkIncludeArchived.Name = "checkIncludeArchived";
			this.checkIncludeArchived.Size = new System.Drawing.Size(142, 18);
			this.checkIncludeArchived.TabIndex = 4;
			this.checkIncludeArchived.Text = "Include Archived";
			this.checkIncludeArchived.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeArchived.UseVisualStyleBackColor = true;
			this.checkIncludeArchived.CheckedChanged += new System.EventHandler(this.checkIncludeArchived_CheckedChanged);
			// 
			// FormAuditOneType
			// 
			this.ClientSize = new System.Drawing.Size(905, 634);
			this.Controls.Add(this.checkIncludeArchived);
			this.Controls.Add(this.labelDisclaimer);
			this.Controls.Add(this.grid);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAuditOneType";
			this.ShowInTaskbar = false;
			this.Text = "Audit Trail";
			this.Load += new System.EventHandler(this.FormAuditOneType_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAuditOneType_Load(object sender, System.EventArgs e) {
			//Default is "Changes made to this appointment before the update to 12.3 will not be reflected below, but can be found in the regular audit trail."
			if(PermTypes.Contains(Permissions.ProcFeeEdit)) {
				labelDisclaimer.Text=Lan.g(this,"Changes made to this procedure fee before the update to 13.2 were not tracked in the audit trail.");
			} 
			else if(PermTypes.Contains(Permissions.InsPlanChangeCarrierName)) {
				labelDisclaimer.Text=Lan.g(this,"Changes made to the carrier for this ins plan before the update to 13.2 were not tracked in the audit trail.");
			}
			else if(PermTypes.Contains(Permissions.RxEdit)) {
				labelDisclaimer.Text=Lan.g(this,"Changes made to the carrier for this Rx before the update to 14.2 were not tracked in the audit trail.");
			}
			else if(PermTypes.Contains(Permissions.OrthoChartEditFull)) {
				labelDisclaimer.Text=Lan.g(this,"Changes made to the ortho chart for this date before the update to 14.3 were not tracked in the audit trail.");
			}
			else if(PermTypes.Contains(Permissions.ImageEdit) || PermTypes.Contains(Permissions.ImageDelete)) {
				labelDisclaimer.Text=Lan.g(this,"Changes made to this document before the update to 15.1 will not be reflected below.");
			}
			else if(PermTypes.Contains(Permissions.EhrMeasureEventEdit)) {
				labelDisclaimer.Text=Lan.g(this,"Changes made to this measure event before the update to 15.2 will not be reflected below.");
			}
			FillGrid();
		}

		private void FillGrid() {
			try {
				LogList=SecurityLogs.Refresh(PatNum,PermTypes,FKey,checkIncludeArchived.Checked);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"There was a problem refreshing the Audit Trail with the current filters."),ex);
				LogList=new SecurityLog[0];
			}
			grid.BeginUpdate();
			grid.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableAudit","Date Time"),120);
			grid.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAudit","User"),70);
			grid.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAudit","Permission"),170);
			grid.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAudit","Log Text"),510);
			grid.Columns.Add(col);
			grid.Rows.Clear();
			ODGridRow row;
			Userod user;
			foreach(SecurityLog logCur in LogList) {
				row=new ODGridRow();
				row.Cells.Add(logCur.LogDateTime.ToShortDateString()+" "+logCur.LogDateTime.ToShortTimeString());
				user=Userods.GetUser(logCur.UserNum);
				if(user==null) {//Will be null for audit trails made by outside entities that do not require users to be logged in.  E.g. Web Sched.
					row.Cells.Add("unknown");
				}
				else {
					row.Cells.Add(user.UserName);
				}
				row.Cells.Add(logCur.PermType.ToString());
				row.Cells.Add(logCur.LogText);
				grid.Rows.Add(row);
			}
			grid.EndUpdate();
			grid.ScrollToEnd();
		}

		private void checkIncludeArchived_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			this.Close();
		}
	}
}






















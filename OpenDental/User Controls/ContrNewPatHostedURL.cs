using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental.User_Controls {
	public partial class ContrNewPatHostedURL:UserControl {

		private bool _isExpanded;
		private WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService _signup;
		private const int LAUNCHWF_COL=4;//The launch webforms column of the options grid is auto-filled, keep track of its index

		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				_isExpanded=value;
				butExpander.Text=_isExpanded ? "-" : "+";
				Height=_isExpanded ? 175 : 25;
			}
		}

		public WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService Signup
		{
			get { return _signup; }
			set { _signup=value; }
		}

		public ContrNewPatHostedURL(WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService signup) {
			InitializeComponent();
			IsExpanded=false;
			AddContextMenu(textWebFormToLaunch);
			AddContextMenu(textSchedulingURL);
			Signup=signup;
			FillControl();
		}

		public string GetPrefValue(PrefName prefName) {
			switch(prefName) {
				case PrefName.WebSchedNewPatAllowChildren:
					return FromGridCell(0);
				case PrefName.WebSchedNewPatVerifyInfo:
					return FromGridCell(1);
				case PrefName.WebSchedNewPatDoAuthEmail:
					return FromGridCell(2);
				case PrefName.WebSchedNewPatDoAuthText:
					return FromGridCell(3);
				case PrefName.WebSchedNewPatWebFormsURL:
					return textWebFormToLaunch.Text;
				default: return "";
			}
		}

		public long GetClinicNum() {
			return Signup.ClinicNum;
		}

		private void FillControl() {
			labelClinicName.Text=Signup.ClinicNum!=0 ? Clinics.GetDesc(Signup.ClinicNum) : Lan.g(this,"Headquarters");
			labelEnabled.Text=Signup.IsEnabled ? Lan.g(this,"Enabled") : Lan.g(this,"Disabled");
			FillGrid();
		}

		private void FillGrid() {
			gridOptions.BeginUpdate();
			//Columns
			gridOptions.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Allow Children"),95,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Show Pre-Screen Questions"),180,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Verify Email"),85,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Verify Text"),85,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Launch WebForm on Complete"),200,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			//Rows
			gridOptions.Rows.Clear();
			ODGridRow row=new ODGridRow();
			row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(PrefName.WebSchedNewPatAllowChildren,Signup.ClinicNum)));
			row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(PrefName.WebSchedNewPatVerifyInfo,Signup.ClinicNum)));
			row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(PrefName.WebSchedNewPatDoAuthEmail,Signup.ClinicNum)));
			row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(PrefName.WebSchedNewPatDoAuthText,Signup.ClinicNum)));
			string url="";
			if(Signup.ClinicNum==0) { //HQ always uses pref.
				url=PrefC.GetString(PrefName.WebSchedNewPatWebFormsURL);
			}
			else { //Clinic should not default back to HQ version of URL. This is unlike typical ClinicPref behavior.
				ClinicPref pref=ClinicPrefs.GetPref(PrefName.WebSchedNewPatWebFormsURL,Signup.ClinicNum);			
				if(pref!=null) {
					url=pref.ValueString;
				}
			}
			row.Cells.Add(ToGridStr(!string.IsNullOrWhiteSpace(url)));
			gridOptions.Rows.Add(row);
			gridOptions.EndUpdate();
			SetFormToLaunch(url);			
		}

		private string ToGridStr(bool value) {
			return value ? "X" : "";
		}

		private string FromGridCell(int cellIdx) {
			return gridOptions.Rows[0].Cells[cellIdx].Text=="X" ? "1" : "0";
		}

		private static void AddContextMenu(TextBox text) {
			if(text.ContextMenuStrip==null) {
				ContextMenuStrip menu=new ContextMenuStrip();
				ToolStripMenuItem browse = new ToolStripMenuItem("Browse");
        browse.Click += (sender, e) => {
					if(!string.IsNullOrWhiteSpace(text.Text)) {
						System.Diagnostics.Process.Start(text.Text);
					}
				};
        menu.Items.Add(browse);
        ToolStripMenuItem copy = new ToolStripMenuItem("Copy");
        copy.Click += (sender, e) => text.Copy();
        menu.Items.Add(copy);
        text.ContextMenuStrip = menu;
			}
		}

		private void SetFormToLaunch(string formURL) {
			textWebFormToLaunch.Text=formURL;
			string extraParams="";
			if(!string.IsNullOrWhiteSpace(formURL)) {
				gridOptions.Rows[0].Cells[LAUNCHWF_COL].Text="X";
				extraParams+="&WF=Y&ReturnURL="+formURL.Replace("&","%26");//encode &s so they aren't misinterpreted as separate parameters
			}
			else {
				gridOptions.Rows[0].Cells[LAUNCHWF_COL].Text="";
			}
			gridOptions.Refresh();
			textSchedulingURL.Text=Signup.HostedUrl+extraParams;
		}

		private void butExpander_Click(object sender,EventArgs e) {
			IsExpanded=!IsExpanded;
		}

		private void butEdit_Click(object sender,EventArgs e) {
			FormWebFormSetup formWFS=new FormWebFormSetup(Signup.ClinicNum,true);
			formWFS.ShowDialog();
			if(formWFS.DialogResult==DialogResult.OK) {
				SetFormToLaunch(formWFS.SheetURLs);
			}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			Clipboard.SetText(textSchedulingURL.Text);
		}

		private void gridOptions_CellClick(object sender,ODGridClickEventArgs e) {
			//Cell coordinates are [e.Row][e.Col]
			if(e.Col!=LAUNCHWF_COL) {
				string cellTextCur=gridOptions.Rows[e.Row].Cells[e.Col].Text;
				string cellTextNew=(cellTextCur=="X" ? "" : "X");
				gridOptions.Rows[e.Row].Cells[e.Col].Text=cellTextNew;
				gridOptions.Refresh();
			}
		}
	}
}

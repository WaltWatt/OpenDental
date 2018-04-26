using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormSiteLinkEdit:ODForm {
		private SiteLink _siteLink;

		public FormSiteLinkEdit(SiteLink siteLink) {
			InitializeComponent();
			Lan.F(this);
			_siteLink=siteLink;
		}

		private void FormSiteLinkEdit_Load(object sender,EventArgs e) {
			Site site=Sites.GetFirstOrDefault(x => x.SiteNum==_siteLink.SiteNum);
			if(_siteLink.SiteNum < 1 || site==null) {
				MsgBox.Show(this,"Invalid SiteNum set for the passed in siteLink.");
				DialogResult=DialogResult.Abort;
				Close();
				return;
			}
			textSite.Text=site.Description;
			//Octets
			if(!string.IsNullOrEmpty(_siteLink.OctetStart)) {
				string[] arrayOctets=_siteLink.OctetStart.Split('.');
				if(arrayOctets.Length > 0) {
					textOctet1.Text=arrayOctets[0];
				}
				if(arrayOctets.Length > 1) {
					textOctet2.Text=arrayOctets[1];
				}
				if(arrayOctets.Length > 2) {
					textOctet3.Text=arrayOctets[2];
				}
			}
			//Triage
			comboTriageCoordinator.Items.Clear();
			foreach(Employee employee in Employees.GetDeepCopy(true)) {
				int index=comboTriageCoordinator.Items.Add(new ODBoxItem<Employee>(Employees.GetNameFL(employee),employee));
				if(_siteLink.EmployeeNum==employee.EmployeeNum) {
					comboTriageCoordinator.SelectedIndex=index;
				}
			}
			//Colors
			panelSiteColor.BackColor=_siteLink.SiteColor;
			panelForeColor.BackColor=_siteLink.ForeColor;
			panelInnerColor.BackColor=_siteLink.InnerColor;
			panelOuterColor.BackColor=_siteLink.OuterColor;
			labelOpsCountPreview.SetColors(panelForeColor.BackColor,panelOuterColor.BackColor,panelInnerColor.BackColor);
		}

		private void textOctet1_TextChanged(object sender,EventArgs e) {
			if(textOctet1.Text.EndsWith(".")) {
				textOctet1.Text=textOctet1.Text.Trim('.');
				textOctet2.Focus();
				textOctet2.SelectAll();
			}
			textOctet1.Text=new string(textOctet1.Text.Where(x => char.IsDigit(x)).ToArray());
			if(textOctet1.TextLength > 3) {
				textOctet1.Text=textOctet1.Text.Substring(0,3);
			}
			textOctet1.SelectionStart=textOctet1.TextLength;
			if(textOctet1.TextLength==3) {
				textOctet2.Focus();
				textOctet2.SelectAll();
			}
		}

		private void textOctet2_TextChanged(object sender,EventArgs e) {
			if(textOctet2.Text.EndsWith(".")) {
				textOctet2.Text=textOctet2.Text.Trim('.');
				textOctet3.Focus();
				textOctet3.SelectAll();
			}
			textOctet2.Text=new string(textOctet2.Text.Where(x => char.IsDigit(x)).ToArray());
			if(textOctet2.TextLength > 3) {
				textOctet2.Text=textOctet2.Text.Substring(0,3);
			}
			textOctet2.SelectionStart=textOctet2.TextLength;
			if(textOctet2.TextLength==3) {
				textOctet3.Focus();
				textOctet3.SelectAll();
			}
		}

		private void textOctet3_TextChanged(object sender,EventArgs e) {
			textOctet3.Text=new string(textOctet3.Text.Where(x => char.IsDigit(x)).ToArray());
			if(textOctet3.TextLength > 3) {
				textOctet3.Text=textOctet3.Text.Substring(0,3);
			}
			textOctet3.SelectionStart=textOctet3.TextLength;
		}

		private Color GetColor(Color colorCur) {
			Color retVal=colorCur;
			ColorDialog colorDlg=new ColorDialog();
			colorDlg.AllowFullOpen=true;
			colorDlg.AnyColor=true;
			colorDlg.SolidColorOnly=false;
			colorDlg.Color=colorCur;
			if(colorDlg.ShowDialog()==DialogResult.OK) {
				retVal=colorDlg.Color;
			}
			return retVal;
		}

		private void butChangeSiteColor_Click(object sender,EventArgs e) {
			panelSiteColor.BackColor=GetColor(panelSiteColor.BackColor);
		}

		private void butChangeForeColor_Click(object sender,EventArgs e) {
			panelForeColor.BackColor=GetColor(panelForeColor.BackColor);
			labelOpsCountPreview.SetColors(panelForeColor.BackColor,panelOuterColor.BackColor,panelInnerColor.BackColor);
		}

		private void butChangeInnerColor_Click(object sender,EventArgs e) {
			panelInnerColor.BackColor=GetColor(panelInnerColor.BackColor);
			labelOpsCountPreview.SetColors(panelForeColor.BackColor,panelOuterColor.BackColor,panelInnerColor.BackColor);
		}

		private void butChangeOuterColor_Click(object sender,EventArgs e) {
			panelOuterColor.BackColor=GetColor(panelOuterColor.BackColor);
			labelOpsCountPreview.SetColors(panelForeColor.BackColor,panelOuterColor.BackColor,panelInnerColor.BackColor);
		}

		private void butOK_Click(object sender,EventArgs e) {
			_siteLink.OctetStart=PIn.Int(textOctet1.Text,false)
				+"."+PIn.Int(textOctet2.Text,false)
				+"."+PIn.Int(textOctet3.Text,false)
				+".";//End with a period so that the matching algorithm in other parts of the program are accurate.
			if(comboTriageCoordinator.SelectedIndex > -1) {
				_siteLink.EmployeeNum=((ODBoxItem<Employee>)comboTriageCoordinator.SelectedItem).Tag.EmployeeNum;
			}
			_siteLink.SiteColor=panelSiteColor.BackColor;
			_siteLink.ForeColor=panelForeColor.BackColor;
			_siteLink.InnerColor=panelInnerColor.BackColor;
			_siteLink.OuterColor=panelOuterColor.BackColor;
			SiteLinks.Upsert(_siteLink);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
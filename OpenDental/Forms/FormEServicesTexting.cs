﻿using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;

namespace OpenDental {
	public partial class FormEServicesSetup {		
		private Clinic _smsClinicSelected {
			get {
				if(gridSmsSummary.GetSelectedIndex()<0) {
					return new Clinic();
				}
				return ((Clinic)gridSmsSummary.Rows[gridSmsSummary.GetSelectedIndex()].Tag)??new Clinic();
			}			
		}
		
		private bool IsTabValidTexting() {
			return true;
		}

		private void FillTabTexting() {
			#region Update UI
			butDefaultClinic.Visible=PrefC.HasClinicsEnabled;
			butDefaultClinicClear.Visible=PrefC.HasClinicsEnabled;
			FillGridSmsUsage();
			#endregion			
		}

		private void SaveTabTexting() {

		}

		private void AuthorizeTabTexting(bool allowEdit) {
			butDefaultClinic.Enabled=allowEdit;
			butDefaultClinicClear.Enabled=allowEdit;
		}

		private void butDefaultClinic_Click(object sender,EventArgs e) {
			if(_smsClinicSelected.ClinicNum==0) {
				MsgBox.Show(this,"Select clinic to make default.");
				return;
			}
			Prefs.UpdateLong(PrefName.TextingDefaultClinicNum,(_smsClinicSelected.ClinicNum));
			Signalods.SetInvalid(InvalidType.Prefs);
			FillGridSmsUsage();
		}

		private void butDefaultClinicClear_Click(object sender,EventArgs e) {
			Prefs.UpdateLong(PrefName.TextingDefaultClinicNum,0);
			Signalods.SetInvalid(InvalidType.Prefs);
			FillGridSmsUsage();
		}

		private void FillGridSmsUsage() {			
			gridSmsSummary.BeginUpdate();
			gridSmsSummary.Columns.Clear();
			if(PrefC.HasClinicsEnabled) {
				gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Default"),80) { TextAlign=HorizontalAlignment.Center });
			}
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Location"),170,HorizontalAlignment.Left));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Subscribed"),80,HorizontalAlignment.Center));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Primary\r\nPhone Number"),105,HorizontalAlignment.Center));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Country\r\nCode"),60,HorizontalAlignment.Center));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Limit"),80,HorizontalAlignment.Right));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Sent\r\nFor Month"),70,HorizontalAlignment.Right));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Sent\r\nCharges"),70,HorizontalAlignment.Right));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Received\r\nFor Month"),70,HorizontalAlignment.Right));
			gridSmsSummary.Columns.Add(new ODGridColumn(Lan.g(this,"Received\r\nCharges"),70,HorizontalAlignment.Right));
			gridSmsSummary.Rows.Clear();
			Clinics.RefreshCache();
			List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
			if(!PrefC.HasClinicsEnabled) { //No clinics so just get the practice as a clinic.
				listClinics.Clear();
				listClinics.Add(Clinics.GetPracticeAsClinicZero());
			}
			var items=SmsPhones.GetSmsUsageLocal(listClinics.Select(x => x.ClinicNum).ToList(),dateTimePickerSms.Value)
				.Rows.Cast<DataRow>().Select(x => new {
					ClinicNum=PIn.Long(x["ClinicNum"].ToString()),
					PhoneNumber=x["PhoneNumber"].ToString(),
					CountryCode=x["CountryCode"].ToString(),
					SentMonth=PIn.Int(x["SentMonth"].ToString()),
					SentCharge=PIn.Double(x["SentCharge"].ToString()),
					RcvMonth=PIn.Int(x["ReceivedMonth"].ToString()),
					RcvCharge=PIn.Double(x["ReceivedCharge"].ToString())
				});			
			foreach(Clinic clinic in listClinics) {
				ODGridRow row=new ODGridRow();
				if(PrefC.HasClinicsEnabled) { //Default texting clinic?
					row.Cells.Add(clinic.ClinicNum==PrefC.GetLong(PrefName.TextingDefaultClinicNum) ? "X" : "");
				}
				row.Cells.Add(clinic.Abbr); //Location.				
				var dataRow=items.FirstOrDefault(x => x.ClinicNum==clinic.ClinicNum);				
				if(dataRow==null) {
					row.Cells.Add("No");//subscribed
					row.Cells.Add("");//phone number
					row.Cells.Add("");//country code
					row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//montly limit
					row.Cells.Add("0");//Sent Month
					row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Sent Charge
					row.Cells.Add("0");//Rcvd Month
					row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Rcvd Charge
				}
				else {
					row.Cells.Add(clinic.SmsContractDate.Year>1800 ? Lan.g(this,"Yes") : Lan.g(this,"No"));
					row.Cells.Add(dataRow.PhoneNumber);
					row.Cells.Add(dataRow.CountryCode);
					row.Cells.Add(clinic.SmsMonthlyLimit.ToString("c",new CultureInfo("en-US")));//Charge this month (Must always be in USD)
					row.Cells.Add(dataRow.SentMonth.ToString());
					row.Cells.Add(dataRow.SentCharge.ToString("c",new CultureInfo("en-US")));
					row.Cells.Add(dataRow.RcvMonth.ToString());
					row.Cells.Add(dataRow.RcvCharge.ToString("c",new CultureInfo("en-US")));
				}
				row.Tag=clinic;
				gridSmsSummary.Rows.Add(row);
			}
			if(listClinics.Count>1) {//Total row if there is more than one clinic (Will not display for practice because practice will have no clinics.
				ODGridRow row=new ODGridRow();
				row.Cells.Add("");
				row.Cells.Add("");
				row.Cells.Add("");
				row.Cells.Add("");
				row.Cells.Add(Lans.g(this,"Total"));
				row.Cells.Add(listClinics.Where(x => items.Any(y => y.ClinicNum==x.ClinicNum)).Sum(x => x.SmsMonthlyLimit).ToString("c",new CultureInfo("en-US")));
				row.Cells.Add(items.Sum(x => x.SentMonth).ToString());
				row.Cells.Add(items.Sum(x => x.SentCharge).ToString("c",new CultureInfo("en-US")));
				row.Cells.Add(items.Sum(x => x.RcvMonth).ToString());
				row.Cells.Add(items.Sum(x => x.RcvCharge).ToString("c",new CultureInfo("en-US")));
				row.ColorBackG=Color.LightYellow;
				gridSmsSummary.Rows.Add(row);
			}
			gridSmsSummary.EndUpdate();
		}

		private void butBackMonth_Click(object sender,EventArgs e) {
			dateTimePickerSms.Value=dateTimePickerSms.Value.AddMonths(-1);
		}

		private void butFwdMonth_Click(object sender,EventArgs e) {
			dateTimePickerSms.Value=dateTimePickerSms.Value.AddMonths(1);//triggers refresh
		}

		private void butThisMonth_Click(object sender,EventArgs e) {
			dateTimePickerSms.Value=DateTime.Now.Date;//triggers refresh
		}

		private void dateTimePickerSms_ValueChanged(object sender,EventArgs e) {
			FillGridSmsUsage();
		}
	}
}

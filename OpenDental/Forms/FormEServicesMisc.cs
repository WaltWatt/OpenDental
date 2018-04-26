using CodeBase;
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
		private const string _shortDateFormat="d";
		private const string _longDateFormat="D";
		private const string _dateFormatMMMMdyyyy="MMMM d, yyyy";
		private const string _dateFormatm="m";

		private bool IsTabValidMisc() {
			if(radioDateCustom.Checked) {
				bool isValidFormat=true;
				if(textDateCustom.Text.Trim()=="") {
					isValidFormat=false;
				}
				try {
					DateTime.Today.ToString(textDateCustom.Text);
				}
				catch(Exception ex) {
					ex.DoNothing();
					isValidFormat=false;
				}
				if(!isValidFormat) {
					MsgBox.Show(this,"Please enter a valid format in the Custom date format text box.");
					return false;
				}
			}
			return true;
		}

		private void FillTabMisc() {
			//.NET has a bug in the DateTimePicker control where the text will not get updated and will instead default to showing DateTime.Now.
			//In order to get the control into a mode where it will display the correct value that we set, we need to set the property Checked to true.
			//Today's date will show even when the property is defaulted to true (via the designer), so we need to do it programmatically right here.
			//E.g. set your computer region to Assamese (India) and the DateTimePickers on the Automation Setting tab will both be set to todays date
			// if the tab is NOT set to be the first tab to display (don't ask me why it works then).
			//This is bad for our customers because setting both of the date pickers to the same date and time will cause automation to stop.
			dateRunStart.Checked=true;
			dateRunEnd.Checked=true;
			//Now that the DateTimePicker controls are ready to display the DateTime we set, go ahead and set them.
			//If loading the picker controls with the DateTime fields from the database failed, the date picker controls default to 7 AM and 10 PM.
			ODException.SwallowAnyException(() => {
				dateRunStart.Value=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeStart);
				dateRunEnd.Value=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeEnd);
			});
			labelDateCustom.Text="";
			radioDateShortDate.Text=DateTime.Today.ToString(_shortDateFormat);//Formats as '3/15/2018'
			radioDateLongDate.Text=DateTime.Today.ToString(_longDateFormat);//Formats as 'Thursday, March 15, 2018'
			radioDateMMMMdyyyy.Text=DateTime.Today.ToString(_dateFormatMMMMdyyyy);//Formats as 'March 15, 2018'
			radioDatem.Text=DateTime.Today.ToString(_dateFormatm);//Formats as 'March 15'
			switch(PrefC.GetString(PrefName.PatientCommunicationDateFormat)) {
				case _shortDateFormat:
					radioDateShortDate.Checked=true;
					break;
				case _longDateFormat:
					radioDateLongDate.Checked=true;
					break;
				case _dateFormatMMMMdyyyy:
					radioDateMMMMdyyyy.Checked=true;
					break;
				case _dateFormatm:
					radioDatem.Checked=true;
					break;
				default:
					radioDateCustom.Checked=true;
					textDateCustom.Text=PrefC.GetString(PrefName.PatientCommunicationDateFormat);
					break;
			}
		}

		private void SaveTabMisc() {
			Prefs.UpdateDateT(PrefName.AutomaticCommunicationTimeStart,dateRunStart.Value);
			Prefs.UpdateDateT(PrefName.AutomaticCommunicationTimeEnd,dateRunEnd.Value);
			string dateFormat;
			if(radioDateShortDate.Checked) {
				dateFormat=_shortDateFormat;
			}
			else if(radioDateLongDate.Checked) {
				dateFormat=_longDateFormat;
			}
			else if(radioDateMMMMdyyyy.Checked) {
				dateFormat=_dateFormatMMMMdyyyy;
			}
			else if(radioDatem.Checked) {
				dateFormat=_dateFormatm;
			}
			else {
				dateFormat=textDateCustom.Text;
			}
			Prefs.UpdateString(PrefName.PatientCommunicationDateFormat,dateFormat);
		}

		private void AuthorizeTabMisc(bool allowEdit) {
			dateRunStart.Enabled=allowEdit;
			dateRunEnd.Enabled=allowEdit;
			groupDateFormat.Enabled=allowEdit;
		}

		private void textDateCustom_TextChanged(object sender,EventArgs e) {
			if(textDateCustom.Text.Trim()=="") {
				labelDateCustom.Text="";
				return;
			}
			try {
				labelDateCustom.Text=DateTime.Now.ToString(textDateCustom.Text);
			}
			catch(Exception ex) {
				ex.DoNothing();
				labelDateCustom.Text="";
			}
		}

		private void butShowOldMobileSych_Click(object sender,EventArgs e) {
			butShowOldMobileSych.Enabled=false;
			tabControl.TabPages.Add(tabMobileSynch);
			tabControl.SelectedTab=tabMobileSynch;
		}
	}
}

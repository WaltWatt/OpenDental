﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenDentBusiness {
	///<summary>Any row in this table will show up in the main menu of Open Dental to get the attention of the user.
	///The user will be able to click on the alert and take an action.  The actions available to the user are also determined in this row.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class AlertItem:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long AlertItemNum;
		///<summary>FK to clinic.ClinicNum. Can be 0 or -1. -1 indicates show the alert in all clinics.</summary>
		public long ClinicNum;
		///<summary>What is displayed in the menu item.</summary>
		public string Description;
		///<summary>Enum:AlertType Identifies what type of alert this row is.</summary>
		public AlertType Type;
		///<summary>Enum:SeverityType The severity will help determine what color this alert should be in the main menu.</summary>
		public SeverityType Severity;
		///<summary>Enum:ActionType Bitwise flag that represents what actions are available for this alert.</summary>
		public ActionType Actions;
		///<summary>The form to open when the user clicks "Open Form".</summary>
		public FormType FormToOpen;
		///<summary>A FK to a table associated with the AlertType.  0 indicates not in use.</summary>
		public long FKey;
		///<summary>Like description, but more specific. When set use ActionType.ShowItemValue to show this variable within a MsgBoxCopyPaste window.</summary>
		public string ItemValue;

		///<summary>Helper dictionary for sorting the ActionType enum in a particular way for display purposes.</summary>
		private static Dictionary<ActionType,int> _dictActionTypeOrder=new Dictionary<ActionType, int> 
		{
			{ ActionType.OpenForm, 1 },
			{ ActionType.ShowItemValue, 2 },
			{ ActionType.MarkAsRead, 3 },
			{ ActionType.Delete, 4 },
			{ ActionType.None, 5 },
		};

		public AlertItem() {
			
		}

		///<summary></summary>
		public AlertItem Copy() {
			return (AlertItem)this.MemberwiseClone();
		}

		public override bool Equals(object obj) {
			AlertItem alert=obj as AlertItem;
			if(alert==null) {
				return false;
			}
			return this.AlertItemNum==alert.AlertItemNum
				&& this.ClinicNum==alert.ClinicNum
				&& this.Description==alert.Description
				&& this.Type==alert.Type
				&& this.Severity==alert.Severity
				&& this.Actions==alert.Actions;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		///<summary>Comparer used to order the ActionType for display purposes.</summary>
		public static int CompareActionType(ActionType x, ActionType y) {
			return _dictActionTypeOrder[x].CompareTo(_dictActionTypeOrder[y]);
		}
	}

	///<summary>Enum representing different alert types.</summary>
	public enum AlertType {
		///<summary>0 - Generic. Informational, has no action associated with it</summary>
		Generic,
		///<summary>1 - Opens the Online Payments Window when clicked</summary>
		[Description("Online Payments Pending")]
		OnlinePaymentsPending,
		///<summary>2 - Only used by Open Dental HQ. The server monitoring incoming voicemails is not working.</summary>
		[Description("Voice Mail Monitor Issues")]
		[Alert(IsODHQ=true)]
		VoiceMailMonitor,
		///<summary>3 - Opens the Radiology Order List window when clicked.</summary>
		[Description("Radiology Orders")]
		RadiologyProcedures,
		///<summary>4 - A patient has clicked "Request Callback" on an e-Confirmation.</summary>
		[Description("Patient Requests Callback")]
		CallbackRequested,
		///<summary>5 - Alerts related to the Web Sched New Pat eService.</summary>
		[Description("Web Sched New Patient")]
		WebSchedNewPat,
		///<summary>6 - Alerts related to Web Sched New Patient Appointments.</summary>
		[Description("Web Sched New Patient Appointment Created")]
		WebSchedNewPatApptCreated,
		///<summary>7 - A number is not able to receive text messages.</summary>
		[Description("Number Barred From Texting")]
		NumberBarredFromTexting,
		///<summary>8 - The number of MySQL connections to the server has exceeded half the allowed number of connections.</summary>
		[Description("MySQL Maximum Connection Issues")]
		MaxConnectionsMonitor,
		///<summary>9 - Alerts related to new ASAP appointments via web sched.</summary>
		[Description("Web Sched ASAP Appointment Created")]
		WebSchedASAPApptCreated,
		///<summary>10 - Only used by Open Dental HQ. The Asterisk Server is not processing messages or is getting all blank payloads.</summary>
		[Description("Phone Tracking Server Issues")]
		[Alert(IsODHQ=true)]
		AsteriskServerMonitor,
		///<summary>11 - Multiple computers are running eConnector services. There should only ever be one.</summary>
		[Description("Multiple eConnectors")]
		MultipleEConnectors,
		///<summary>12 - The eConnector is in a critical state and not currently turned on. There should only ever be one.</summary>
		[Description("eConnection Down")]
		EConnectorDown,
		///<summary>13 - The eConnector has an error that is not critical but is worth looking into. There should only ever be one.</summary>
		[Description("eConnection Error")]
		EConnectorError,
		///<summary>14 - Alerts related to DoseSpot provider registration.</summary>
		[Description("DoseSpot Provider Registered")]
		DoseSpotProviderRegistered,
		///<summary>15 - Alerts related to DoseSpot clinic registration.</summary>
		[Description("DoseSpot Clinic Registered")]
		DoseSpotClinicRegistered,
		///<summary>16 - An appointment has been created via Web Sched Recall.</summary>
		[Description("Web Sched Recall Appointment Created")]
		WebSchedRecallApptCreated,
	}

	///<summary>Represents the urgency of the alert.  Also determines the color for the menu item in the main menu.</summary>
	public enum SeverityType {
		///<summary>0 - White</summary>
		Normal,
		///<summary>1 - Yellow</summary>
		Low,
		///<summary>2 - Orange</summary>
		Medium,
		///<summary>3 - Red</summary>
		High
	}

	///<summary>The possible actions that can be taken on this alert.  Multiple actions can be available for one alert.</summary>
	[Flags]
	public enum ActionType {
		///<summary></summary>
		None=0,
		///<summary></summary>
		MarkAsRead=1,
		///<summary></summary>
		OpenForm=2,
		///<summary></summary>
		Delete=4,
		///<summary></summary>
		ShowItemValue=8
	}

	///<summary>Add this.</summary>
	public enum FormType {
		///<summary>0 - No form.</summary>
		None,
		///<summary>1 - FormEServicesWebSchedRecall.</summary>
		[Description("eServices Web Sched Recall")]
		FormEServicesWebSchedRecall,
		///<summary>2 - FormPendingPayments.</summary>
		[Description("Pending Payments")]
		FormPendingPayments,
		///<summary>3 - FormRadOrderList.</summary>
		[Description("Radiology Orders")]
		FormRadOrderList,
		///<summary>4 - FormEServicesSetup.</summary>
		[Description("eServices Signup Portal")]
		FormEServicesSignupPortal,
		///<summary>5 - FormEServicesSetup. FKey will be the AptNum of the appointment to open.</summary>
		[Description("Appointment")]
		FormApptEdit,
		///<summary>6 - FormEServicesSetup Web Sched New Pat.</summary>
		[Description("eServices Web Sched New Pat")]
		FormEServicesWebSchedNewPat,
		///<summary>7 - FormWebSchedAppts.</summary>
		[Description("Web Sched Appointments")]
		FormWebSchedAppts,
		///<summary>8 - FormPatientEdit. FKey will be PatNum.</summary>
		[Description("Edit Patient Information")]
		FormPatientEdit,
		///<summary>9 - FormEServicesSetup eConnector Service.</summary>
		[Description("eServices eConnector Service")]
		FormEServicesEConnector,
		///<summary>10 - FormDoseSpotAssignUserId.</summary>
		[Description("DoseSpot Assign User ID")]
		FormDoseSpotAssignUserId,
    ///<summary>11 - FormDoseSpotAssignClinicId.</summary>
    [Description("DoseSpot Assign Clinic ID")]
    FormDoseSpotAssignClinicId,
  }

	///<summary>Alert-related attributes.</summary>
	public class AlertAttribute:Attribute {
		private bool _isODHQ=false;

		///<summary>The alert is only used at OD HQ. Defaults to false.</summary>
		public bool IsODHQ {
			get {
				return _isODHQ;
			}
			set {
				_isODHQ=value;
			}
		}
	}

}

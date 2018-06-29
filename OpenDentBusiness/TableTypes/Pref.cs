﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace OpenDentBusiness {
	///<summary>Stores small bits of data for a wide variety of purposes.  Any data that's too small to warrant its own table will usually end up here.</summary>
	[Serializable]
	[CrudTable(TableName="preference")]
	public class Pref:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long PrefNum;
		///<summary>The text 'key' in the key/value pairing.</summary>
		public string PrefName;
		///<summary>The stored value.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string ValueString;
		///<summary>Documentation on usage and values of each pref.  Mostly deprecated now in favor of using XML comments in the code.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Comments;

		///<summary>Returns a copy of the pref.</summary>
		public Pref Copy() {
			return (Pref)this.MemberwiseClone();
		}
	}

	///<summary>Because this enum is stored in the database as strings rather than as numbers, we can do the order alphabetically.  This enum must exactly match the prefs in the database.  Deprecated preferences will start with "Deprecated" in the summary section.</summary>
	public enum PrefName {
		AccountingCashIncomeAccount,
		AccountingDepositAccounts,
		AccountingIncomeAccount,
		AccountingLockDate,
		///<summary>Enum:AccountingSoftware 0=None, 1=Open Dental, 2=QuickBooks</summary>
		AccountingSoftware,
		///<summary>Boolean, false by default to preserve old functionality.  Allows users to make future dated patient payments when turned on.</summary>
		AccountAllowFutureDebits,
		///<summary>Defaulted to off, determines whether completed payment plans are visible in the account module.</summary>
		AccountShowCompletedPaymentPlans,
		AccountShowPaymentNums,
		///<summary>Show questionnaire button in account module toolbar.  Set in FormShowFeatures.</summary>
		AccountShowQuestionnaire,
		///<summary>Show TrojanCollect button in account module toolbar.  Set in FormShowFeatures.</summary>
		AccountShowTrojanExpressCollect,
		ADAComplianceDateTime,
		ADAdescriptionsReset,
		///<summary>Boolean, true by default.  When set to true and a new family member is added, the new patient's email will be autofilled with the 
		///guarantor's email.</summary>
		AddFamilyInheritsEmail,
		///<summary>Deprecated in version 17.4.40.  When set to true, the user will not be able to save a new adjustment without first attaching a procedure to it.</summary>
		AdjustmentsForceAttachToProc,
		///<summary>Enum:ADPCompanyCode Used to generate the export file from FormTimeCardManage. Set in FormTimeCardSetup.</summary>
		ADPCompanyCode,
		///<summary>Stored as DateTime, but cleared when aging finishes.  The DateTime will be used as a flag to signal other connections that aging
		///calculations have started and prevents another connection from running simultaneously.  In order to run aging, this will have to be cleared,
		///either by the connection that set the flag when aging finishes, or by the user overriding the lock and manually clearing this pref.</summary>
		AgingBeginDateTime,
		AgingCalculatedMonthlyInsteadOfDaily,
		///<summary>If true, aging will use the intermediate table famaging for calculating aging.</summary>
		AgingIsEnterprise,
		///<summary>If true, negative adjustments will be aged by AdjDate.  Otherwise (legacy behavior), negative adjustments are summed with all other
		///credits and applied to oldest charge first.</summary>
		AgingNegativeAdjsByAdjDate,
		///<summary>This pref is hidden, so no UI to enable this feature.  If this is true, there will be a checkbox in the aging report window to age
		///patient payments to payment plans.  Aging patient payments to payment plans will only work if the completed amounts on the payment plans are 0.
		///Otherwise the payments and the completed amounts will essentially double the amounts of the payment plans in the aging calculation.  This is
		///only for a specific customer, so no UI, defaults to false, only able to enable this via query.</summary>
		AgingReportShowAgePatPayplanPayments,
		///<summary>Stored as DateTime, but only the time is used.  This is the time of day during which aging will be calculated by the aging service.
		///Aging will run during a one hour block of time starting with the time set.  If AgingBeginDateTime is not blank, aging will not be calculated.
		///If AgingCalculatedMonthlyInsteadOfDaily is true, aging will not be calculated.  This will be blank if disabled.</summary>
		AgingServiceTimeDue,
		///<summary>FK to allergydef.AllergyDefNum</summary>
		AllergiesIndicateNone,
		///<summary>Boolean defaults to true.  If true, allows a user to email CC receipt otherwise not allowed.</summary>
		AllowEmailCCReceipt,
		AllowedFeeSchedsAutomate,
		///<summary>Boolean defauts to false.  If true, users can enter insurance payments that are for a future date.</summary>
		AllowFutureInsPayments,
		///<summary>Boolean, false by default to preserve old functionality.  Allows users to attach providers to prepayments while EnforceFully is on.
		///</summary>
		AllowPrepayProvider,
		///<summary>Bool. Allows adjustments from FormClaimEdit. 0 by default.</summary>
		AllowProcAdjFromClaim,
		AllowSettingProcsComplete,
		AppointmentBubblesDisabled,
		AppointmentBubblesNoteLength,
		AppointmentClinicTimeReset,
		///<summary>Enum:SearchBehaviorCriteria 0=ProviderTime, 1=ProviderTimeOperatory</summary>
		AppointmentSearchBehavior,
		AppointmentTimeArrivedTrigger,
		AppointmentTimeDismissedTrigger,
		///<summary>The number of minutes that the appointment schedule is broken up into.  E.g. "10" represents 10 minute increments.</summary>
		AppointmentTimeIncrement,
		///<summary>Set to true if appointment times are locked by default.</summary>
		AppointmentTimeIsLocked,
		///<summary>Used to set the color of the time indicator line in the appt module.  Stored as an int.</summary>
		AppointmentTimeLineColor,
		AppointmentTimeSeatedTrigger,
		///<summary>Controls whether or not creating new appointments prompt to select an appointment type.</summary>
		AppointmentTypeShowPrompt,
		///<summary>Controls whether or not a warning will be displayed when selecting an appointment type would detach procedures from an appointment..</summary>
		AppointmentTypeShowWarning,
		///<summary>Integer in minutes.  Defaults to 30.  The defualt length of an appointment that is created without attaching any procedures.</summary>
		AppointmentWithoutProcsDefaultLength,
		///<summary>Boolean defauts to true.  If true, users can set appointments without procedures complete.</summary>
		ApptAllowEmptyComplete,
		///<summary>Boolean defauts to false.  If true, users can set future appointments complete.</summary>
		ApptAllowFutureComplete,
		ApptBubbleDelay,
		ApptExclamationShowForUnsentIns,
		///<summary>Boolean defaults to 0.  If true, adds the adjustment total to the net production in appointment module.</summary>
		ApptModuleAdjustmentsInProd,
		///<summary>Boolean defaults to 0, when true appt module will default to week view</summary>
		ApptModuleDefaultToWeek,
		///<summary>Boolean defaults to 1 if there is relevant ortho chart info, when true appt menu will have an ortho chart item.</summary>
		ApptModuleShowOrthoChartItem,
		///<summary>Keeps the waiting room indicator times current.  Initially 1.</summary>
		ApptModuleRefreshesEveryMinute,
		///<summary>Integer</summary>
		ApptPrintColumnsPerPage,
		///<summary>Float</summary>
		ApptPrintFontSize,
		///<summary>Stored as DateTime.  Currently the date portion is not used but might be used in future versions.</summary>
		ApptPrintTimeStart,
		///<summary>Stored as DateTime.  Currently the date portion is not used but might be used in future versions.</summary>
		ApptPrintTimeStop,
		///<summary>Boolean. True by default. If true, clicking the confirm URL will bring up the Confirmation Portal where the patient can select a
		///confirmation option. If false, clicking the confirm URL in the message will confirm the appt (1-click confirmation).</summary>
		ApptEConfirm2ClickConfirmation,
		///<summary>FK to definition.DefNum.  If using automated confirmations, appointment set to this status when confirmation is sent.</summary>
		ApptEConfirmStatusSent,
		///<summary>FK to definition.DefNum.  If using automated confirmations, appointment set to this status when confirmation is confirmed.</summary>
		ApptEConfirmStatusAccepted,
		///<summary>FK to definition.DefNum.  If using automated confirmations, Anything that is not "Accepted" or "Sent".</summary>
		ApptEConfirmStatusDeclined,
		///<summary>FK to definition.DefNum.  If using automated confirmations, when failed by HQ for some reason.</summary>
		ApptEConfirmStatusSendFailed,
		///<summary>True if the office has actived eConfirmations in the UI.</summary>
		ApptConfirmAutoEnabled,
		///<summary>True if HQ has confirmed that this office is signed up for eConfirmations.</summary>
		ApptConfirmAutoSignedUp,
		///<summary>Bool; Only if using clinics, when true causes automation to skip appointments not assigned to a clinic.</summary>
		ApptConfirmEnableForClinicZero,
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from EConfirmation RSVP updates.
		///Prevents overwriting manual Confirmation status.</summary>
		ApptConfirmExcludeEConfirm,
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from EReminders.</summary>
		ApptConfirmExcludeERemind,
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from sending an EConfirmation.
		///Prevents overwriting manual Confirmation status.</summary>
		ApptConfirmExcludeESend,
		///<summary>Bool; 0 by default. When false, calculates net and gross production by provider bars in each appointment view.
		///When true, calulates net and gross production by appointments in the apppointment view. </summary>
		ApptModuleProductionUsesOps,
		///<summary>True if automated appointment reminders are enabled for the entire DB. See ApptReminderRules for setup details.
		///Permissions are still checked here at HQ so manually overriding this value will only make the program behave annoyingly, but won't break anything.</summary>
		ApptRemindAutoEnabled,
		///<summary>DEPRECATED.  See ApptReminderRule table instead.</summary>
		ApptReminderDayInterval,
		///<summary>DEPRECATED.  See ApptReminderRule table instead.</summary>
		ApptReminderDayMessage,
		///<summary>DEPRECATED.  See ApptReminderRule table instead.</summary>
		ApptReminderEmailMessage,
		///<summary>DEPRECATED.  See ApptReminderRule table instead.</summary>
		ApptReminderHourInterval,
		///<summary>DEPRECATED.  See ApptReminderRule table instead.</summary>
		ApptReminderHourMessage,
		///<summary>DEPRECATED.  See ApptReminderRule table instead.</summary>
		ApptReminderSendAll,
		///<summary>DEPRECATED.  See ApptReminderRule table instead.</summary>
		ApptReminderSendOrder,
		///<summary>Bool; False by default. When true, new appointments require at least one procedure to be attached.</summary>
		ApptsRequireProc,
		///<summary>DEPRECATED. Use InsChecksFrequency instead.</summary>
		ApptsCheckFrequency,
		///<summary>Bool; False by default.  When true, the secondary provider used when scheduling an appointment will use the Operatory's secondary provider no matter what.</summary>
		ApptSecondaryProviderConsiderOpOnly,
		///<summary>Date, MinDate by default.  The Date that was set within the "Archive entries on or before:" field within the Archive tab of the 
		///Backup window when the archive process was last ran successfully.</summary>
		ArchiveDate,
		///<summary>Encrypted password for the database user that will be used when directly connecting to the archive server.</summary>
		ArchivePassHash,
		///<summary>The name of the server where the archive database should be located.</summary>
		ArchiveServerName,
		///<summary>DEPRECATED.  Archiving with Middle Tier connection never fully implemented.  Here was the original intent behind the preference:
		///URI to a Middle Tier web service that is connected to the database where archives should be made.</summary>
		ArchiveServerURI,
		///<summary>The user name for the database user that will be used when directly connecting to the archive server.</summary>
		ArchiveUserName,
		///<summary>Default billing types selected when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		ArManagerBillingTypes,
		///<summary>Default state for the exclude if bad address (no zipcode) checkbox when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		ArManagerExcludeBadAddresses,
		///<summary>Default state for the exclude if unsent procs checkbox when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		ArManagerExcludeIfUnsentProcs,
		///<summary>Default state for the exclude if pending ins checkbox when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		ArManagerExcludeInsPending,
		///<summary>Default transaction types selected when loading the Sent Tab of the Accounts Receivable Manager - Sent tab.</summary>
		ArManagerLastTransTypes,
		///<summary>Default account age when loading the Sent Tab of the Accounts Receivable Manager.</summary>
		ArManagerSentAgeOfAccount,
		///<summary>Default number of days since the last payment when loading the Sent Tab of the Accounts Receivable Manager.</summary>
		ArManagerSentDaysSinceLastPay,
		///<summary>Default minimum balances when loading the Sent Tab of the Accounts Receivable Manager.</summary>
		ArManagerSentMinBal,
		///<summary>Default account age when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		ArManagerUnsentAgeOfAccount,
		///<summary>Default number of days since the last payment when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		ArManagerUnsentDaysSinceLastPay,
		///<summary>Default minimum balances when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		ArManagerUnsentMinBal,
		///<summary>The template that is used when manually texting patients on the ASAP list.</summary>
		ASAPTextTemplate,
		///<summary>Only used by OD HQ.  This is a strange preference because it isn't used in the typical cache fashion.
		///HQ directly queries this preference very often in order to see if the PhoneTrackingServer has gone down.</summary>
		AsteriskServerHeartbeat,
		///<summary>Used by OD HQ.  Not added to db convert script.  Used to store the IP address of the asterisk phone server for the phone comms and voice mails.</summary>
		AsteriskServerIp,
		///<summary>Deprecated, but must remain here to avoid breaking updates.</summary>
		AtoZfolderNotRequired,
		///<summary>Enum - Enumerations.DataStorageType.  Normally 1 (AtoZ).  This used to be called AtoZfolderNotRequired, but that name was confusing.</summary>
		AtoZfolderUsed,
		///<summary>The number of audit trail entries that are displayed in the grid.</summary>
		AuditTrailEntriesDisplayed,
		///<summary>Used to determine the runtime of the threads that do automatic communication in the listener.  Stored as a DateTime.</summary>
		AutomaticCommunicationTimeStart,
		///<summary>Used to determine the runtime of the threads that do automatic communication in the listener.  Stored as a DateTime.</summary>
		AutomaticCommunicationTimeEnd,
		///<summary>Boolean.  Defaults to same value as ShowFeatureEhr.  Used to determine whether automatic summary of care webmails are sent.</summary>
		AutomaticSummaryOfCareWebmail,
		AutoResetTPEntryStatus,
		/// <summary>Enum - AutoSplitPreference. Defaults to Adjustments (1). Used to choose order to apply unattached credits to adjustments in account module. </summary>
		AutoSplitLogic,
		BackupExcludeImageFolder,
		BackupFromPath,
		BackupReminderLastDateRun,
		BackupRestoreAtoZToPath,
		BackupRestoreFromPath,
		BackupRestoreToPath,
		BackupToPath,
		BadDebtAdjustmentTypes,
		BalancesDontSubtractIns,
		BankAddress,
		BankRouting,
		[PrefName(ValueType=PrefValueType.STRING)]
		BillingAgeOfAccount,
		BillingChargeAdjustmentType,
		BillingChargeAmount,
		BillingChargeLastRun,
		///<summary>Value is a string, either Billing or Finance.</summary>
		BillingChargeOrFinanceIsDefault,
		BillingDefaultsInvoiceNote,
		BillingDefaultsIntermingle,
		BillingDefaultsLastDays,
		///<summary>The statement modes that will also receive a text message. Stored as a comma-separated list of integers where each item is the integer
		///value of the StatementMode enum.</summary>
		BillingDefaultsModesToText,
		[PrefName(ValueType=PrefValueType.STRING)]
		BillingDefaultsNote,
		/// <summary>Boolean, false by default. Indicates if billing statements default to single patients(true) or guarantors(false).</summary>
		BillingDefaultsSinglePatient,
		///<summary>The template used for SMS text notifications for statements.</summary>
		BillingDefaultsSmsTemplate,
		///<summary>Value is an integer, identifying the max number of statements that can be sent per batch.  Default of 0, which indicates no limit.
		///This preference is used for both printed statements and electronic ones.  It was decided to not rename the pref.</summary>
		BillingElectBatchMax,
		///<summary>Deprecated.  Use ebill.ClientAcctNumber instead.</summary>
		BillingElectClientAcctNumber,
		///<summary>Boolean, true by default.  Indicates if electronic billing should generate a PDF document.</summary>
		BillingElectCreatePDF,
		BillingElectCreditCardChoices,
		///<summary>Deprecated.  Use ebill.ElectPassword instead.</summary>
		BillingElectPassword,
		///<summary>No UI, can only be manually enabled by a programmer.  Only used for debugging electronic statements, because it will bloat the OpenDentImages folder.  Originally created to help with the "missing brackets bug" for EHG billing.</summary>
		BillingElectSaveHistory,
		///<summary>Output path for ClaimX EStatments.</summary>
		BillingElectStmtOutputPathClaimX,
		///<summary>Output path for EDS EStatments.</summary>
		BillingElectStmtOutputPathEds,
		///<summary>Output path for POS EStatments.</summary>
		BillingElectStmtOutputPathPos,
		///<summary>URL that EStatments are uploaded to for Dental X Change. Previously hardcoded in version 16.2.18 and below.</summary>
		BillingElectStmtUploadURL,
		///<summary>Deprecated.  Use ebill.ElectUserName instead.</summary>
		BillingElectUserName,
		BillingElectVendorId,
		BillingElectVendorPMSCode,
		BillingEmailBodyText,
		BillingEmailSubject,
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingExcludeBadAddresses,
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingExcludeIfUnsentProcs,
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingExcludeInactive,
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingExcludeInsPending,
		[PrefName(ValueType=PrefValueType.STRING)]
		BillingExcludeLessThan,
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingExcludeNegative,
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingIgnoreInPerson,
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingIncludeChanged,
		///<summary>Used with repeat charges to apply repeat charges to patient accounts on billing cycle date.</summary>
		BillingUseBillingCycleDay,
		[PrefName(ValueType=PrefValueType.STRING)]
		BillingSelectBillingTypes,
		///<summary>Boolean.  Defaults to true.  Determines if the billing window shows progress when sending statements.</summary>
		BillingShowSendProgress,
		///<summary>Boolean. Allows option to show activity on statements since the last $0 balance on the account (or family).</summary>
		[PrefName(ValueType=PrefValueType.BOOL)]
		BillingShowTransSinceBalZero,
		///<summary>0=no,1=EHG,2=POS(xml file),3=ClaimX(xml file),4=EDS(xml file)</summary>
		BillingUseElectronic,
		BirthdayPostcardMsg,
		///<summary>FK to definition.DefNum.  The adjustment type that will be used on the adjustment that is automatically created when an appointment is broken.</summary>
		BrokenAppointmentAdjustmentType,
		///<summary>Enumeration of type "BrokenApptProcedure".  Missed by default when D9986 is present.  This preference determines how broken appointments are handeld.</summary>
		BrokenApptProcedure,
		///<summary>Deprecated.  Boolean.  0 by default.  When true, makes a commlog, otherwise makes an adjustment.</summary>
		BrokenApptCommLogNotAdjustment,
		///<summary>Boolean.  0 by default.  When true, makes a commlog when an appointment is broken.</summary>
		BrokenApptCommLog,
		///<summary>Boolean.  0 by default.  When true, makes an adjustment when an appointment is broken.</summary>
		BrokenApptAdjustment,
		///<summary>For Ontario Dental Association fee schedules.</summary>
		CanadaODAMemberNumber,
		///<summary>For Ontario Dental Association fee schedules.</summary>
		CanadaODAMemberPass,
		///<summary>Boolean.  0 by default.  If enabled, only CEMT can edit certain security settings.  Currently only used for global lock date.</summary>
		CentralManagerSecurityLock,
		///<summary>This is the hash of the password that is needed to open the Central Manager tool.</summary>
		CentralManagerPassHash,
		///<summary>Blank by default.  Contains a key for the CEMT.  Each CEMT database contains a unique sync code.  Syncing from the CEMT will skip any databases without the correct sync code.</summary>
		CentralManagerSyncCode,
		///<summary>Deprecated.</summary>
		ChartQuickAddHideAmalgam,
		///<summary>Deprecated. If set to true (1), then after adding a proc, a row will be added to datatable instead of rebuilding entire datatable by making queries to the database.
		///This preference was never fully implemented and should not be used.  We may revisit some day.</summary>
		ChartAddProcNoRefreshGrid,
		///<summary>Preference to warn users when they have a nonpatient selected.</summary>
		ChartNonPatientWarn,
		ClaimAttachExportPath,
		///<summary>Default value of "[PatNum]/".  Allows customization of ClaimIdentifier prefix format.</summary>
		ClaimIdPrefix,
		ClaimFormTreatDentSaysSigOnFile,
		///<summary>When true, the default ordering provider on medical eclaim procedures will be set to the procedure treating provider.</summary>
		ClaimMedProvTreatmentAsOrdering,
		ClaimMedTypeIsInstWhenInsPlanIsMedical,
		///<summary>Boolean, 0 by default.  When true, only Batch Insurance in Manage Module can be used to finalize payments.</summary>
		ClaimPaymentBatchOnly,
		///<summary>Int.  Valid values are >=0.  Use -1 to disable.  Used to be a date.  Now represents a rolling date, thus the name is a bit off.
		///We decided to keep the name the same instead of deprecating and creating a new pref, because user never sees the pref name and to avoid bloat.
		///Number of days (default 1) to subtract from the current date when deciding which NO PAYMENT claims and
		///$0 claimprocs to consider when using the This Claim Only button from the Edit Claim window or when creating a batch payment from Manage module.
		///Used for filtering Outstanding Claims from FormClaimPayBatch list, and when finalizing from Edit Claim window.</summary>
		ClaimPaymentNoShowZeroDate,
		///<summary>When true, procedurecode overrides will send the override's description to insurance instead of the original procedurecode's description.</summary>
		ClaimPrintProcChartedDesc,
		///<summary>Enum:ClaimProcCreditsGreaterThanProcFee.  Allow by default.  0=Allow, 1=Warn, 2=Block.  This preference either allows, warns or blocks the user from 
		///entering an insurance payment on the Enter Payment screen if (for a procedure) the sum of the Ins Pay + Writeoff + any attached adjustments + and attached 
		///patient payments > Procedure Fee </summary>
		ClaimProcAllowCreditsGreaterThanProcFee,
		///<summary>Boolean,  0 by default.  When true, allows claimprocs to be created for backdated completed procedures.</summary>
		ClaimProcsAllowedToBackdate,
		///<summary>For the Procedures Not Billed to Insurance report.  If true, when creating new claims from the report window, will group procedures
		///by clinic and site.  If false, will block user from creating claims if the selected procedures for a specific patient have different
		///clinis or different sites.  Default value is true to encourage automation.</summary>
		ClaimProcsNotBilledToInsAutoGroup,
		///<summary>Blank by default.  Computer name to receive reports from automatically.</summary>
		ClaimReportComputerName,
		///<summary>Boolean, 0 by default. When true, Open Dental Service will receive claim reports instead of the specified computer spawning a thread
		///in FormOpenDental.</summary>
		ClaimReportReceivedByService,
		///<summary>Report receive interval. In minutes. 30 by default. If the ClaimReportReceiveLastDateTime preference is set, then this value will
		///be 0.</summary>
		ClaimReportReceiveInterval,
		///<summary>Stores last time the reports were ran.</summary>
		ClaimReportReceiveLastDateTime,
		///<summary>Time to retrieve claim reports. Stored as a DateTime even though we only care about the time. If theClaimReportReceiveInterval 
		///preference is set, then this value will be an empty string.</summary>
		ClaimReportReceiveTime,
		///<summary>Boolean.  0 by default.  If enabled, the Send Claims window will automatically validate e-claims upon loading the window.
		///Validating all claims on load was old behavior that was significantly slowing down the loading of the send claims window.
		///Several offices complained that we took away the validation until they attempt sending the claim.</summary>
		ClaimsSendWindowValidatesOnLoad,
		///<summary>Boolean.  0 by default.  If enabled, snapshots of claimprocs are created when claims are created.</summary>
		ClaimSnapshotEnabled,
		///<summary>DateTime where the time is the only useful part. 
		///Stores the time of day that the eConnector should create a claimsnapshot.</summary>
		ClaimSnapshotRunTime,
		///<summary>Enumeration of type "ClaimSnapshotTrigger".  ClaimCreate by default.  This preference determines how ClaimSnapshots get created. Stored as the enumeration.ToString().</summary>
		ClaimSnapshotTriggerType,
		///<summary>When set to true, adding a claim tracking status to a claim requires an error.</summary>
		ClaimTrackingRequiresError,
		ClaimsValidateACN,
		ClearinghouseDefaultDent,
		///<summary>FK to clearinghouse.ClearingHouseNum.  Allows a different clearinghouse to be used for checking eligibility.
		///Defaults to the current dental (or medical) clearinghouse which preserves old behavior.</summary>
		ClearinghouseDefaultEligibility,
		ClearinghouseDefaultMed,
		///<summary>Boolean.  0 by default.  If enabled, new patients can be added with an usassigned clinic.</summary>
		ClinicAllowPatientsAtHeadquarters,
		///<summary>Boolean.  0 by default.  If enabled, lists clinics in alphabetical order.</summary>
		ClinicListIsAlphabetical,
		///<summary>String, "Workstation"(default), "User", "None". See FormMisc. Determines how recently viewed clinics should be tracked.</summary>
		ClinicTrackLast,
		///<summary>Boolean. 0 by default. When set to true, all new clones will be put into their own family (guarantor as themselves)
		///and then a super family will be created if one does not exist or the new clone will be associated to the master clone's super family.
		///When set to false, all new clones blindly inherit their master's guarantor and super family settings.</summary>
		CloneCreateSuperFamily,
		ColorTheme,
		///<summary>Boolean.  False by default.  When true, causes CommLogs to auto-save on a timer.</summary>
		CommLogAutoSave,
		ConfirmEmailMessage,
		ConfirmEmailSubject,
		ConfirmPostcardMessage,
		///<summary>FK to definition.DefNum.  Initially 0.</summary>
		ConfirmStatusEmailed,
		///<summary>FK to definition.DefNum.</summary>
		ConfirmStatusTextMessaged,
		///<summary>The message that goes out to patients when doing a batch confirmation.</summary>
		ConfirmTextMessage,
		///<summary>Selected connection group within the CEMT.</summary>
		ConnGroupCEMT,
		CoPay_FeeSchedule_BlankLikeZero,
		///<summary>Boolean.  Typically set to true when an update is in progress and will be set to false when finished.  Otherwise true means that the database is in a corrupt state.</summary>
		CorruptedDatabase,
		///<summary>This is the default encounter code used for automatically generating encounters when specific actions are performed in Open Dental.  The code is displayed/set in FormEhrSettings.  We will set it and give the user a list of 9 suggested codes to use such that the encounters generated will cause the pateint to be considered part of the initial patient population in the 9 clinical quality measures tracked by OD.  CQMDefaultEncounterCodeSystem will identify the code system this code is from and the code value will be a FK to that code system.</summary>
		CQMDefaultEncounterCodeValue,
		CQMDefaultEncounterCodeSystem,
		CropDelta,
		///<summary>Used by OD HQ.  Not added to db convert script.  Allowable timeout for Negotiator to establish a connection with Listener. Different than SocketTimeoutMS and TransmissionTimeoutMS.  Specifies the allowable timeout for Patient Portal Negotiator to establish a connection with Listener.  Negotiator will only wait this long to get an acknowledgement that the Listener is available for a transmission before timing out.  Initially 10000</summary>
		CustListenerConnectionRequestTimeoutMS,
		///<summary>Used by OD HQ.  Not added to db convert script.  Will be passed to OpenDentalEConnector when service initialized.  Specifies the time (in minutes) between each time that the listener service will upload it's current heartbeat to HQ.  Initially 360</summary>
		CustListenerHeartbeatFrequencyMinutes,
		///<summary>Used by OpenDentalEConnector.  String specifies which port the OpenDentalWebService should look for on the customer's server in order to create a socket connection.  Initially 25255</summary>
		CustListenerPort,
		///<summary>Used by OD HQ.  Not added to db convert script.  Will be passed to OpenDentalEConnector when service initialized.  Specifies the read/write socket timeout.  Initially 3000</summary>
		CustListenerSocketTimeoutMS,
		///<summary>Used by OD HQ.  Not added to db convert script.  Specifies the entire wait time alloted for a transmission initiated by the patient portal.  Negotiator will only wait this long for a valid response back from Listener before timing out.  Initially 30000</summary>		
		CustListenerTransmissionTimeoutMS,
		///<summary>Used by OD HQ. The name of the customers database that is considered the "source of truth".</summary>
		CustomersHQDatabase,
		///<summary>Used by OD HQ. The MySQL password hash for the HQ customers database.</summary>
		CustomersHQMySqlPassHash,
		///<summary>Used by OD HQ. The MySQL user for the HQ customers database.</summary>
		CustomersHQMySqlUser,
		///<summary>Used by OD HQ. The name of the customers server that is considered the "source of truth".</summary>
		CustomersHQServer,
		CustomizedForPracticeWeb,
		DatabaseConvertedForMySql41,
		///<summary>bool. Set to false by default. If true, the optimize database maintenance tool will be disabled.</summary>
		DatabaseMaintenanceDisableOptimize,
		///<summary>bool. Set to false by default. If true, database maintenance will skip table checks.</summary>
		DatabaseMaintenanceSkipCheckTable,
		DataBaseVersion,
		DateDepositsStarted,
		DateLastAging,
		DefaultCCProcs,
		DefaultClaimForm,
		DefaultProcedurePlaceService,
		///<summary>Long. 0 by default. Used to assign a user group to a new user that is added by a user who does not have the SecurityAdmin user 
		///permission.</summary>
		DefaultUserGroup,
		///<summary>Boolean. Set to true by default. When true, patient fields that do not have a matching patient field def will display at the bottom
		///of the patient fields with gray text.</summary>
		DisplayRenamedPatFields,
		///<summary>Boolean.  Set to 1 to indicate that this database holds customers instead of patients.  Used by OD HQ.  Used for showing extra phone numbers, showing some extra buttons for tools that only we use, behavior of checkboxes in repeating charge window, etc.  But phone panel visibility is based on DockPhonePanelShow.</summary>
		DistributorKey,
		DockPhonePanelShow,
		///<summary>The AtoZ folder path.</summary>
		DocPath,
		///<summary>There is no UI for user to change this.  Used by OD HQ. Determines if Task refreshes only update locally.  True is local only, false is every workstation.</summary>
		DoLimitTaskSignals,
		/// <summary>Boolean. Determine whether or not to allow users to bypass OpenDentalLogin using ActiveDirectory. Default is false.</summary>
		DomainLoginEnabled,
		///<summary>Specifies the path to use when ActiveDirectory/Domain Logins are enabled.</summary>
		DomainLoginPath,
		///<summary>The date this customer last checked with HQ to determine which provider have access to eRx.</summary>
		DoseSpotDateLastAccessCheck,
		///<summary>The ICD Diagnosis Code version primarily used by the practice.  Value of '9' for ICD-9, and '10' for ICD-10.</summary>
		DxIcdVersion,
		EasyBasicModules,
		/// <summary>Depricated.</summary>
		EasyHideAdvancedIns,
		EasyHideCapitation,
		EasyHideClinical,
		EasyHideDentalSchools,
		EasyHideHospitals,
		EasyHideInsurance,
		EasyHideMedicaid,
		EasyHidePrinters,
		EasyHidePublicHealth,
		EasyHideRepeatCharges,
		EasyNoClinics,
		EclaimsSeparateTreatProv,
		///<summary>Boolean, false by default.  Will be set to true when the update server successfully upgrades the CustListener service to the 
		///eConnector service.  This only needs to happen once.  This will automatically happen after upgrading past v15.4.
		///If automatically upgrading the CustListener service to the eConnector service fails, they can click Install in eService Setup.
		///NEVER programmatically set this preference back to false.</summary>
		EConnectorEnabled,
		///<summary>A JSON string of disparate pieces of information regarding the eConnector.</summary>
		EConnectorStatistics,
		EHREmailFromAddress,
		EHREmailPassword,
		EHREmailPOPserver,
		EHREmailPort,
		EhrRxAlertHighSeverity,
		///<summary>Boolean, false by default.  When this is set and using eRx it will utilize the currently selected clinic instead of the patient's default clinic.</summary>
		ElectronicRxClinicUseSelected,
		///<summary>This pref is hidden, so no practical way for user to turn this on.  Only used for ehr testing.</summary>
		EHREmailToAddress,
		///<summary>Date when user upgraded to 13.1.14 and started using NewCrop Guids on Rxs.</summary>
		ElectronicRxDateStartedUsing131,
		/// <summary>FK to EmailAddress.EmailAddressNum.  It is not required that a default be set.</summary>
		EmailDefaultAddressNum,
		///<summary>The name of the only computer allowed to get new email messages from an email inbox (including Direct messages).</summary>
		EmailInboxComputerName,
		///<summary>Time interval in minutes describing how often to automatically check the email inbox for new messages. Default is 5 minutes.</summary>
		EmailInboxCheckInterval,
		///<summary>FK to EmailAddress.EmailAddressNum.  Used for webmail notifications (Patient Portal).</summary>
		EmailNotifyAddressNum,
		/// <summary>Deprecated. Use emailaddress.EmailPassword instead.</summary>
		EmailPassword,
		/// <summary>Deprecated. Use emailaddress.ServerPort instead.</summary>
		EmailPort,
		/// <summary>Deprecated. Use emailaddress.SenderAddress instead.</summary>
		EmailSenderAddress,
		/// <summary>Deprecated. Use emailaddress.SMTPserver instead.</summary>
		EmailSMTPserver,
		/// <summary>Deprecated. Use emailaddress.EmailUsername instead.</summary>
		EmailUsername,
		/// <summary>Deprecated. Use emailaddress.UseSSL instead.</summary>
		EmailUseSSL,
		/// <summary>Boolean. 0 means false and means it is not an EHR Emergency, and emergency access to the family module is not granted.</summary>
		EhrEmergencyNow,
		///<summary>There is no UI for this.  It's only used by OD HQ.</summary>
		EhrProvKeyGeneratorPath,
		EnableAnesthMod,
		///<summary>Warns the user if the Medicaid ID is not the proper number of digits for that state.</summary>
		EnforceMedicaidIDLength,
		///<summary>Boolean, false by default.  When true then there will be 1 page per each claim paid for an ERA header and ERA claim paid on printouts.</summary>
		EraPrintOneClaimPerPage,
		ExportPath,
		///<summary>Allows guarantor access to all family health information in the patient portal.  Default is 1.</summary>
		FamPhiAccess,
		FinanceChargeAdjustmentType,
		FinanceChargeAPR,
		FinanceChargeAtLeast,
		FinanceChargeLastRun,
		FinanceChargeOnlyIfOver,
		///<summary>String. The e-mail template used for Web Sched verifications to new patients.</summary>
		WebSchedVerifyNewPatEmailBody,
		///<summary>String. The e-mail subject used for Web Sched verifications to new patients.</summary>
		WebSchedVerifyNewPatEmailSubj,
		///<summary>String. The text template used for Web Sched verifications to new patients.</summary>
		WebSchedVerifyNewPatText,
		///<summary>Enum. The communication type for Web Sched verifications to new patients. 0: None, 1: Text, 2: E-mail, 3: Text and E-mail</summary>
		WebSchedVerifyNewPatType,
		///<summary>String. The e-mail template used for Web Sched verifications to recalls.</summary>
		WebSchedVerifyRecallEmailBody,
		///<summary>String. The e-mail subject used for Web Sched verifications to recalls.</summary>
		WebSchedVerifyRecallEmailSubj,
		///<summary>String. The text template used for Web Sched verifications to recalls.</summary>
		WebSchedVerifyRecallText,
		///<summary>Enum. The communication type for Web Sched verifications to recalls. 0: None, 1: Text, 2: E-mail, 3: Text and E-mail</summary>
		WebSchedVerifyRecallType,
		///<summary>String. The e-mail template used for Web Sched verifications to ASAP patients.</summary>
		WebSchedVerifyASAPEmailBody,
		///<summary>String. The e-mail subject used for Web Sched verifications to ASAP patients.</summary>
		WebSchedVerifyASAPEmailSubj,
		///<summary>String. The text template used for Web Sched verifications to ASAP patients.</summary>
		WebSchedVerifyASAPText,
		///<summary>Enum. The communication type for Web Sched verifications to ASAP patients. 0: None, 1: Text, 2: E-mail, 3: Text and E-mail</summary>
		WebSchedVerifyASAPType,
		///<summary>Double defaults to 0. Prevents clicks from happening in a window. 
		///Currently used in ApptEdit to prevent procedures from being attached or detached for delay amount in tenths of a second.</summary>
		FormClickDelay,
		FuchsListSelectionColor,
		FuchsOptionsOn,
		/// <summary>Bool defaults to 0. When false, future dates transactions are not allowed. </summary>
		FutureTransDatesAllowed,
		GenericEClaimsForm,
		///<summary>A comma delimited list of computer names. If a computer name is in the list then it has verbose logging on.</summary>
		HasVerboseLogging,
		///<summary>Has no UI.  Used to validate help support.  See the OpenDentalHelp project for more information on HelpKey.</summary>
		HelpKey,
		HL7FolderOut,
		HL7FolderIn,
		///<summary>Deprecated.  Use SiteLink.EmployeeNum instead.  Used by HQ. Projected onto wall displayed on top of FormMapHQ</summary>
		HQTriageCoordinator,
		///<summary>procedurelog.DiagnosticCode will be set to this for new procedures and complete procedures if this field was blank when set complete.
		///This can be an ICD-9 or an ICD-10.  In future versions, could be another an ICD-11, ICD-12, etc.</summary>
		ICD9DefaultForNewProcs,
		///<summary>3-state prefernce used in the image module, state definitions are:
		///0 = Expand the document tree each time the Images module is visited.
		///1 = Document tree collapses when patient changes.
		///2 = Document tree folders persistent expand/collapse per user.</summary>
		ImagesModuleTreeIsCollapsed,
		ImageWindowingMax,
		ImageWindowingMin,
		///<summary>Boolean.  False by default.  When enabled a fix is enabled within ODTextBox (RichTextBox) for foreign users that use 
		///a different language input methodology that requires the composition of symbols in order to display their language correctly.
		///E.g. the Korean symbol '역' (dur) will not display correctly inside ODTextBoxes without this set to true.</summary>
		ImeCompositionCompatibility,
		Ins834ImportPath,
		Ins834IsPatientCreate,
		///<summary>Comma delimited list of procedure codes that represent bitewing codes.  Defaults to D codes for all users.</summary>
		InsBenBWCodes,
		///<summary>Comma delimited list of procedure codes that represent exam codes.  Defaults to D codes for all users.</summary>
		InsBenExamCodes,
		///<summary>Comma delimited list of procedure codes that represent pano codes.  Defaults to D codes for all users.</summary>
		InsBenPanoCodes,
		///<summary>0=Default practice provider, -1=Treating Provider. Otherwise, FK to provider.ProvNum.</summary>
		InsBillingProv,
		InsDefaultCobRule,
		InsDefaultPPOpercent,
		InsDefaultShowUCRonClaims,
		InsDefaultAssignBen,
		///<summary>0=unknown, user did not make a selection.  1=Yes, 2=No.</summary>
		InsPlanConverstion_7_5_17_AutoMergeYN,
		///<summary>Boolean.  False by default.  When enabled, procedure fees will always use the UCR fee.</summary>
		InsPpoAlwaysUseUcrFee,
		///<summary>0 by default.  If false, secondary PPO writeoffs will always be zero (normal).  At least one customer wants to see secondary writeoffs.</summary>
		InsPPOsecWriteoffs,
		///<summary>Boolean, false by default.  When true, treatment plan module and appointment scheduling checks for frequency conflicts.</summary>
		InsChecksFrequency,
		InsurancePlansShared,
		///<summary>7 by default.  Number of days before displaying insurances that need verified.</summary>
		InsVerifyAppointmentScheduledDays,
		///<summary>90 by default. Number of days before requiring insurance plans to be verified.</summary>
		InsVerifyBenefitEligibilityDays,
		///<summary>1 by default.  Number of days that a past appointment will show in the "Past Due" insurance verification grid.</summary>
		InsVerifyDaysFromPastDueAppt,
		///<summary>Boolean, false by default.  When true, defaults a filter to the current user instead of All when opening the InsVerifyList.</summary>
		InsVerifyDefaultToCurrentUser,
		///<summary>Boolean, false by default.  When true, excludes patient clones from the Insurance Verification List.</summary>
		InsVerifyExcludePatientClones,
		///<summary>Boolean, false by default.  When true, excludes patient plans associated to insurance plans that are marked "Do Not Verify" from the Insurance Verification List.</summary>
		InsVerifyExcludePatVerify,
		///<summary>Boolean, false by default.  When true, if an appointment is after the benefit renewal month for the insurance plan, make that plan be reverified and postdate the insverify.DateLastVerified.</summary>
		InsVerifyFutureDateBenefitYear,
		///<summary>30 by default.  Number of days before requiring patient plans to be verified.</summary>
		InsVerifyPatientEnrollmentDays,
		///<summary>Writeoff description displayed in the Account Module and on statements.  If blank, the default is "Writeoff".
		///We are using "Writeoff" since "PPO Discount" was only used for a brief time in 15.3 while it was Beta and no customer requested it</summary>
		InsWriteoffDescript,
		IntermingleFamilyDefault,
		///<summary>Preference to show writeoffs in the StatementInvoicePayment grid.</summary>
		InvoicePaymentsGridShowNetProd,
		///<summary>True if there is a row in the ehrprovkey table.  The OpenDentalService will check this preference and if it is false it will not query
		///the procedurelog table for scheduled non-CPOE radiology procs.  When the first row is inserted into the ehrprovkey table, or if there is an
		///existing row when the db is updated, this will be set to true.  Otherwise false.  Users can manually turn this pref on or off.</summary>
		IsAlertRadiologyProcsEnabled,
		///<summary>Enum.  Flags ItransNCpl.ItransUpdateFields: identifies what carrier fields to update when impotring carriers for ITRANS 2.0.</summary>
		ItransImportFields,
		JobManagerDefaultEmail,
		JobManagerDefaultBillingMsg,
		LabelPatientDefaultSheetDefNum,
		///<summary>Used to determine how many windows are displayed throughout the program, translation, charting, and other features. Version 15.4.1</summary>
		LanguageAndRegion,
		///<summary>Initially set to Declined to Specify.  Indicates which language from the LanguagesUsedByPatients preference is the language that indicates the patient declined to specify.  Text must exactly match a language in the list of available languages.  Can be blank if the user deletes the language from the list of available languages.</summary>
		LanguagesIndicateNone,
		///<summary>Comma-delimited list of two-letter language names and custom language names.  The custom language names are the full string name and are not necessarily supported by Microsoft.</summary>
		LanguagesUsedByPatients,
		LetterMergePath,
    ///<summary>Boolean. Only used to override server time in the following places: Time Cards.</summary>
    LocalTimeOverridesServerTime,
		MainWindowTitle,
		///<summary>0=Meaningful Use Stage 1, 1=Meaningful Use Stage 2, 2=Meaningful Use Modified Stage 2.  Global, affects all providers.  Changes the MU grid that is seen for individual patients and for summary reports.</summary>
		MeaningfulUseTwo,
		///<summary>Number of days after medication order start date until stop date.  Used when automatically inserting a medication order when creating
		///a new Rx.  Default value is 7 days.  If set to 0 days, the automatic stop date will not be entered.</summary>
		MedDefaultStopDays,
		///<summary>New procs will use the fee amount tied to the medical code instead of the ADA code.</summary>
		MedicalFeeUsedForNewProcs,
		///<summary>FK to medication.MedicationNum</summary>
		MedicationsIndicateNone,
		///<summary>If MedLabReconcileDone=="0", a one time reconciliation of the MedLab HL7 messages is needed. The reconcile will reprocess the original
		///HL7 messages for any MedLabs with PatNum=0 in order to create the embedded PDF files from the base64 text in the ZEF segments. The old method
		///of waiting to extract these files until the message is manually attached to a patient was very slow using the middle tier. The new method is to
		///create the PDF files and save them in the image folder in a subdirectory called "MedLabEmbeddedFiles" if a pat is not located from the details
		///in the PID segment of the message. Attaching the MedLabs to a patient is now just a matter of moving the files to the patient's image folder.
		///All files will now be extracted and stored, either in a pat's folder or in the "MedLabEmbeddedFiles" folder, by the HL7 service.</summary>
		MedLabReconcileDone,
		///<summary>Boolean, true by default. If true, the middle tier server will cache all non-hidden fees for all clinics. If false, it will cache no 
		///fees and go to the database every time.</summary>
		MiddleTierCacheFees,
		MobileSyncDateTimeLastRun,
		///<summary>Used one time after the conversion to 7.9 for initial synch of the provider table.</summary>
		MobileSynchNewTables79Done,
		///<summary>Used one time after the conversion to 11.2 for re-synch of the patient records because a)2 columns BalTotal and InsEst have been added to the patientm table. b) the table documentm has been added</summary>
		MobileSynchNewTables112Done,
		///<summary>Used one time after the conversion to 12.1 for the recallm table being added and for upload of the practice Title.</summary>
		MobileSynchNewTables121Done,
		MobileSyncIntervalMinutes,
		MobileSyncServerURL,
		MobileSyncWorkstationName,
		MobileExcludeApptsBeforeDate,
		MobileUserName,
		//MobileSyncLastFileNumber,
		//MobileSyncPath,
		///<summary>The major and minor version of the current MySQL connection.  Gets updated on startup when a new version is detected.</summary>
		MySqlVersion,
		///<summary>True by default.  Will use the claimsnapshot table for calculating production in the Net Production Detail report if the date range is today's date only.</summary>
		NetProdDetailUseSnapshotToday,
		///<summary>There is no UI for user to change this.  Format, if OD customer, is PatNum-(RandomString)(CheckSum).  Example: 1234-W6c43.  Format for resellers is up to them.</summary>
		NewCropAccountId,
		///<summary>The date this customer last checked with HQ to determine which provider have access to eRx.</summary>
		NewCropDateLastAccessCheck,
		///<summary>True for customers who were using NewCrop before version 15.4.  True if NewCropAccountId was not blank when upgraded.</summary>
		NewCropIsLegacy,
		///<summary>Controls which NewCrop database to use.  If false, then the customer uses the First Data Bank (FDB) database, otherwise the 
		///customer uses the LexiData database.  Connecting to LexiData saves NewCrop some money on the new accounts.  Additionally, the RxNorms which
		///come back from the prescription refresh in the Chart are more complete for the LexiData database than for the FDB database.</summary>
		NewCropIsLexiData,
		///<summary>There is no UI for user to change this. For distributors, this is part of the credentials.  OD credentials are not stored here, but are hard-coded.</summary>
		NewCropName,
		///<summary>There is no UI for user to change this.  For distributors, this is part of the credentials.
		///OD credentials are not stored here, but are hard-coded.</summary>
		NewCropPartnerName,
		///<summary>There is no UI for user to change this.  For distributors, this is part of the credentials.
		///OD credentials are not stored here, but are hard-coded.</summary>
		NewCropPassword,
		///<summary>URL of the time server to use for EHR time synchronization.  Only used for EHR.  Example nist-time-server.eoni.com</summary>
		NistTimeServerUrl,
		OpenDentalVendor,
		OracleInsertId,
		///<summary>User-defined automatic ortho claim procedure.  D8670.auto by default. Can be overridden at the insplan level.</summary>
		OrthoAutoProcCodeNum,
		///<summary>When turned on, ortho case information is shown in the ortho chart.</summary>
		OrthoCaseInfoInOrthoChart,
		///<summary>Determines whether claims with ortho procedures on them will automatically be marked as Ortho claims.</summary>
		OrthoClaimMarkAsOrtho,
		///<summary>When true, ortho claims' "OrthoDate" will be automatically set to the patient's first ortho procedure when created.</summary>
		OrthoClaimUseDatePlacement,
		///<summary>Byte, 24 by default.  The default number of months ortho treatments last.  Overridden by patientnote.OrthoMonthsTreat.</summary>
		OrthoDefaultMonthsTreat,
		///<summary>Defines whether ortho chart and ortho features should show.</summary>
		OrthoEnabled,
		///<summary>When turned on, prompts the user to move any completed ortho procedures' fees' to be moved to the D8080 procedurecode.</summary>
		OrthoInsPayConsolidated,
		///<summary>Comma delimited list of procedure code CodeNum's.  These procedures are used as flags in order to determine the Patients' DatePlacement.
		///DatePlacement is the ProcDate of the first completed procedure that is associated to any of the procedure codes in this list.</summary>
		OrthoPlacementProcsList,
		///<summary>Enum:RpOutstandingIns.DateFilterTab. Defaults to DaysOld. Determines which date filter tab to default load in Outstanding Insurance 
		///Report.</summary>
		OutstandingInsReportDateFilterTab,
		PasswordsMustBeStrong,
		///<summary>Boolean.  False by default.  When true strong passwords require a special character (Non letter or digit).</summary>
		PasswordsStrongIncludeSpecial,
		///<summary>Boolean.  False by default.  When true and PasswordsMustBeStrong is also true users without strong passwords will be prompted to change their password at next login.</summary>
		PasswordsWeakChangeToStrong,
		PatientAllSuperFamilySync,
		///<summary>The way that dates should be formatted when communicating with patients. Defaults to "d" which is equivalent to .ToShortDateString().
		///User editable.  Whatever value is in this preference is intended to be passed to DateTime.ToString().
		///Used in eReminders, eConfirms, manual confirmations, ASAP list texting, and other places.</summary>
		PatientCommunicationDateFormat,
		PatientFormsShowConsent,
		///<summary>Boolean.  Defaults to false. Used for the clinicpref table, in addition to the preference table. When true, this practice or 
		///clinic is enabled to send Patient Portal Invites.</summary>
		PatientPortalInviteEnabled,
		///<summary>Boolean.  Defaults to true. Only used for the clinicpref table, not the preference table. When true, this clinic will use the 
		///Patient Portal Invite rules for ClinicNum 0.</summary>
		PatientPortalInviteUseDefaults,
		///<summary>Free-form 'Body' text of the notification sent by this practice when a new secure EmailMessage is sent to patient.</summary>
		PatientPortalNotifyBody,
		///<summary>Free-form 'Subject' text of the notification sent by this practice when a new secure EmailMessage is sent to patient.</summary>
		PatientPortalNotifySubject,
		///<summary>Boolean.  Defaults to false.  True if the office is signed up for patient portal. Currently only set in AutoCommPatientPortalInvites.</summary>
		PatientPortalSignedUp,
		PatientPortalURL,
		PatientSelectUseFNameForPreferred,
		///<summary>Boolean. This is the default for new computers, otherwise it uses the computerpref PatSelectSearchMode.</summary>
		PatientSelectUsesSearchButton,
		///<summary>Boolean. False by default.  When true and assigning new primary insurance, the associated patients billing type will be inherited from insPlan.BillingType</summary>
		PatInitBillingTypeFromPriInsPlan,
		///<summary>Deprecated. PaySplitManager enum. 1 by default. 0=DoNotUse, 1=Prompt, 2=Force</summary>
		PaymentsPromptForAutoSplit,
		///<summary>0 by default.1=Prompt users to select payment type when creating new Payments.</summary>
		PaymentsPromptForPayType,
		///<summary>PayClinicSetting enum. 0 by default. 0=SelectedClinic, 1=PatientDefaultClinic, 2=SelectedExceptHQ</summary>
		PaymentClinicSetting,
		///<summary>When true, the payment window does not show paysplits by default.</summary>
		PaymentWindowDefaultHideSplits,
		///<summary>Int.  Represents PayPeriodInterval enum (Weekly, Bi-Weekly, Monthly). </summary>
		PayPeriodIntervalSetting,
		///<summary>Int.  If set, represents the number of days after the pay period the pay day is.</summary>
		PayPeriodPayAfterNumberOfDays,
		///<summary>Boolean.  True by default.  If true, pay days will fall before weekends.  If false, pay days will fall after weekends.</summary>
		PayPeriodPayDateBeforeWeekend,
		///<summary>Boolean.  True by default.  Pay Day cannot fall on weekend if true.</summary>
		PayPeriodPayDateExcludesWeekends,
		///<summary>Int. If set to 0, it's disabled, but any other number represents a day of the week. 1:Sunday, 2:Monday etc...</summary>
		PayPeriodPayDay,
		/// <summary>Long. Stores the defnum of the neg adjustment type chosen to use for pay plan adjustments default. </summary>
		PayPlanAdjType,
		/// <summary>bool. Set to false by default. If true, the "Due Now" column will be hidden from pay plans grid in acct module.</summary>
		PayPlanHideDueNow,
		PayPlansBillInAdvanceDays,
		///<summary>Boolean.  False by default.  If true, payment plan window will exclude past activity in the amortization grid by default.</summary>
		PayPlansExcludePastActivity,
		PayPlansUseSheets,
		///<summary>The Payment Plan version that the customer is using. Derives from PayPlanVersions enum.  Valid values are 1, 2, or 3.  1 by default.</summary>
		PayPlansVersion,
		PerioColorCAL,
		PerioColorFurcations,
		PerioColorFurcationsRed,
		PerioColorGM,
		PerioColorMGJ,
		PerioColorProbing,
		PerioColorProbingRed,
		PerioRedCAL,
		PerioRedFurc,
		PerioRedGing,
		PerioRedMGJ,
		PerioRedMob,
		PerioRedProb,
		///<summary>Enabled by default.  When a new perio exam is created, will always mark all missing teeth as skipped.</summary>
		PerioSkipMissingTeeth,
		///<summary>Enabled by default.  When a tooth with an implant procedure completed will not be skipped on perio exams</summary>
		PerioTreatImplantsAsNotMissing,
		///<summary>Can be any int.  Defaults to 0.</summary>
		PlannedApptDaysFuture,
		///<summary>Can be any int.  Defaults to 365.</summary>
		PlannedApptDaysPast,
		PlannedApptTreatedAsRegularAppt,
		PracticeAddress,
		PracticeAddress2,
		PracticeBankNumber,
		PracticeBillingAddress,
		PracticeBillingAddress2,
		PracticeBillingCity,
		PracticeBillingST,
		PracticeBillingZip,
		PracticeCity,
		PracticeDefaultBillType,
		PracticeDefaultProv,
		///<summary>In USA and Canada, enforced to be exactly 10 digits or blank.</summary>
		PracticeFax,
		///<summary>This preference is used to hide/change certain OD features, like hiding the tooth chart and changing 'dentist' to 'provider'.</summary>
		PracticeIsMedicalOnly,
		PracticePayToAddress,
		PracticePayToAddress2,
		PracticePayToCity,
		PracticePayToST,
		PracticePayToZip,
		///<summary>In USA and Canada, enforced to be exactly 10 digits or blank.</summary>
		PracticePhone,
		PracticeST,
		PracticeTitle,
		PracticeZip,
		///<summary>Boolean.  False by default.  If true, checks "Preferred only" in FormReferralSelect.</summary>
		ShowPreferedReferrals,
		///<summary>This is the default pregnancy code used for diagnosing pregnancy from FormVitalSignEdit2014 and is displayed/set in FormEhrSettings.  When the check box for BMI and BP not taken due to pregnancy Dx is selected, this code value will be inserted into the diseasedef table in the column identified by the PregnancyDefaultCodeSystem (i.e. diseasedef.SnomedCode, diseasedef.ICD9Code).  It will then be a FK in the diseasedef table to the associated code system table.</summary>
		PregnancyDefaultCodeValue,
		PregnancyDefaultCodeSystem,
		///<summary>FK to definition.DefNum for PaySplitUnearnedType defcat (29)</summary>
		PrepaymentUnearnedType,
		///<summary>In Patient Edit and Add Family windows, the Primary Provider defaults to 'Select Provider' instead of the practice provider.</summary>
		PriProvDefaultToSelectProv,
		///<summary>FK to diseasedef.DiseaseDefNum</summary>
		ProblemsIndicateNone,
		///<summary>Deprecated. Also spelled wrong.</summary>
		ProblemListIsAlpabetical,
		///<summary>Determines default sort order of Proc Codes list when accessed from Lists -> Procedure Codes.  Enum:ProcCodeListSort, 0 by default.</summary>
		ProcCodeListSortOrder,
		///<summary>In FormProcCodes, this is the default for the ShowHidden checkbox.</summary>
		ProcCodeListShowHidden,
		///<summary>Users must use suggested auto codes for a procedure.</summary>
		ProcEditRequireAutoCodes,
		///<summary>Determines if and how we want to update a procedures ProcFee when changing providers.
		///0 - No prompt, don't change fee (default)
		///1 - No prompt, always change fee
		///2 - Prompt - When patient portion changes
		///3 - Prompt - Always
		///</summary>
		ProcFeeUpdatePrompt,
		ProcLockingIsAllowed,
		///<summary>Bool.  Defaults to false.  Custom feature that a customer paid for to merge the current and last procedure note together.
		///The merging of the current and last procedure note will only happen when a concurrency issue has been identified.</summary>
		ProcNoteConcurrencyMerge,
		///<summary>True by default.  Allows for substituting AutoNote text for [[text]] segments in a procedure's default note.</summary>
		ProcPromptForAutoNote,
		///<summary>If this is on, the claimproc's provider will only inherit the provider on the procedure if the status is not Received or Supplemental.
		///If this is off AND the claimproc is attached to a claim, then the claimproc's provider and the procedure's provider are saved separately.
		///Most users will keep this on so their providers stay in sync.  
		///Pref created for customers who manually change claimproc Prov so they can have income attributed for specific prov for financial reasons.</summary>
		ProcProvChangesClaimProcWithClaim,
		///<summary>Frequency at which signals are processed. Also used by HQ to determine triage label refresh frequency.</summary>		
		ProcessSigsIntervalInSecs,
		ProcGroupNoteDoesAggregate,
		///<summary>DateTime.  Next date that the advertising programming properties will automatically check.</summary>		
		ProgramAdditionalFeatures,
		///<summary>Deprecated. Use updatehistory table instead.  Stored the DateTime of when the ProgramVersion preference last changed.</summary>		
		ProgramVersionLastUpdated,
		ProgramVersion,
		ProviderIncomeTransferShows,
		///<summary>Bool.  Defaults to true.  When true, allow the Provider Payroll report to select Today's date in the date range.</summary>		
		ProviderPayrollAllowToday,
		///<summary>Was never used.  Was supposed to indicate FK to sheet.Sheet_DEF_Num, so not even named correctly. Must be an exam sheet. Only makes sense if PublicHealthScreeningUsePat is true.</summary>
		PublicHealthScreeningSheet,
		///<summary>Was never used.  Always 0.  Boolean. Work for attaching to patients stopped 11/30/2012, there is currently no access to change the value of this preference.    When in this mode, screenings will be attached to actual PatNums rather than just freeform text names.</summary>
		PublicHealthScreeningUsePat,
		///<summary>Comma-delimited list of strings.  Each entry represents a QuickBooks "class" which is used to separate deposits (typically by clinics).
		///Empty by default.</summary>
		QuickBooksClassRefs,
		///<summary>Boolean, off by default.  Some users have clinics enabled but do not want QuickBooks to itemize their accounts.
		///Class Refs are a way for QuickBooks to itemize if set up correctly.</summary>
		QuickBooksClassRefsEnabled,
		QuickBooksCompanyFile,
		///<summary>Comma-delimited list of strings.  Each entry represents a QuickBooks deposit account.</summary>
		QuickBooksDepositAccounts,
		///<summary>Comma-delimited list of strings.  Each entry represents a QuickBooks income account.</summary>
		QuickBooksIncomeAccount,
		///<summary>Date when user upgraded to or past 15.4.1 and started using ADA procedures to count CPOE radiology orders for EHR.</summary>
		RadiologyDateStartedUsing154,
		///<summary>Boolean.  True if random primary keys have been turned on. There is no interface to change this preference because as of 17.2, all users of
		///replication must use primary key offset instead of random primary keys.
		///Causes all CRUD classes to look for an unused random PK before inserting instead of leaving it up to auto incrementing.</summary>
		RandomPrimaryKeys,
		RecallAdjustDown,
		RecallAdjustRight,
		///<summary>Defaults to 12 for new customers.  The number in this field is considered adult.  Only used when automatically adding procedures to a new recall appointment.</summary>
		RecallAgeAdult,
		RecallCardsShowReturnAdd,
		///<summary>-1 indicates min for all dates</summary>
		RecallDaysFuture,
		///<summary>-1 indicates min for all dates</summary>
		RecallDaysPast,
		RecallEmailFamMsg,
		RecallEmailFamMsg2,
		RecallEmailFamMsg3,
		RecallEmailMessage,
		RecallEmailMessage2,
		RecallEmailMessage3,
		RecallEmailSubject,
		RecallEmailSubject2,
		RecallEmailSubject3,
		RecallExcludeIfAnyFutureAppt,
		RecallGroupByFamily,
		///<summary> long. -1=infinite, 0=zero; if stored as -1, displays as "".</summary>
		RecallMaxNumberReminders,
		RecallPostcardFamMsg,
		RecallPostcardFamMsg2,
		RecallPostcardFamMsg3,
		RecallPostcardMessage,
		RecallPostcardMessage2,
		RecallPostcardMessage3,
		RecallPostcardsPerSheet,
		RecallShowIfDaysFirstReminder,
		RecallShowIfDaysSecondReminder,
		RecallStatusEmailed,
		RecallStatusEmailedTexted,
		RecallStatusMailed,
		RecallStatusTexted,
		///<summary>Used if younger than 12 on the recall date.</summary>
		RecallTypeSpecialChildProphy,
		RecallTypeSpecialPerio,
		RecallTypeSpecialProphy,
		///<summary>Comma-delimited list. FK to recalltype.RecallTypeNum.</summary>
		RecallTypesShowingInList,
		///<summary>If false, then it will only use email in the recall list if email is the preferred recall method.</summary>
		RecallUseEmailIfHasEmailAddress,
		///<summary>Bool, 0 by default.  When true, recurring charges will use the primary provider of the patient when creating paysplits.
		///When false, the provider that the family is most in debt to will be used.</summary>
		RecurringChargesUsePriProv,
		///<summary>Bool, 0 by default.  When true, uses the transaction date for the recurring charge payment date.
		///When false, the recurring charge date will be used as the recurring charge payment date.</summary>
		RecurringChargesUseTransDate,
		RegistrationKey,
		RegistrationKeyIsDisabled,
		RegistrationNumberClaim,
		RenaissanceLastBatchNumber,
		///<summary>Stored as DateTime, but cleared when repeating charges tool finishes.  The DateTime will be used as a flag to signal other connections
		///that repeating charges have started and prevents another connection from running simultaneously. In order to run repeating charges, 
		///this will have to be cleared, either by the connection that set the flag when repeating charges finishes or by an update query.</summary>
		RepeatingChargesBeginDateTime,
		///<summary>If replication has failed, this indicates the server_id.  No computer will be able to connect to this single server until this flag is cleared.</summary>
		ReplicationFailureAtServer_id,
		///<summary>The PK of the replication server that is flagged as the "report server".  If using replication, "create table" or "drop table" commands can only be executed within the user query window when connected to this server.</summary>
		ReplicationUserQueryServer,
		ReportFolderName,
    ///<summary>When using a distinct reporting server, stores the server name.</summary>
    ReportingServerCompName,
    ///<summary>When using a distinct reporting server, stores the database name.</summary>
    ReportingServerDbName,
    ///<summary>When using a distinct reporting server, stores the mysql username.</summary>
    ReportingServerMySqlUser,
    ///<summary>When using a distinct reporting server, stores the hashed mysql password.</summary>
    ReportingServerMySqlPassHash,
    ///<summary>When using a distinct reporting server over middle tier, stores the uri.</summary>
    ReportingServerURI,
    ///<summary>Boolean, on by default.</summary>
    ReportPandIhasClinicBreakdown,
    ///<summary>Boolean, off by default.</summary>
		ReportPandIhasClinicInfo,
		ReportPandIschedProdSubtractsWO,
		///<summary>Bool.  False by defualt, used to filter incomplete procedures by having no note in the Incomplete Procedures Report.</summary>
		ReportsIncompleteProcsNoNotes,
		///<summary>Bool.  False by defualt, used to filter incomplete procedures by having a note that is unsigned in the Incomplete Procedures Report.</summary>
		ReportsIncompleteProcsUnsigned,
		ReportsPPOwriteoffDefaultToProcDate,
		///<summary>Bool.  False by defualt, used to wrap columns when printing a custom report.</summary>
		ReportsWrapColumns,
		///<summary>Bool.  False by defualt, used to determine whether the reports progress bar will show a history or not.</summary>
		ReportsShowHistory,
		ReportsShowPatNum,
		RequiredFieldColor,
		///<summary>Tri-state enumeration. 1 by default. 0=Fully Enforced. 1=Auto-split but don't enforce rigorous accounting. 2=Don't auto-split and don't enforce.</summary>
		RigorousAccounting,
		///<summary>Tri-state enumeration. 1 by default. 0=Fully Enforced. 1=Auto-link but don't enforce. 2=Don't auto-link and don't enforce.</summary>
		RigorousAdjustments,
		///<summary>Defaults to false.  When true, will require procedure code to be attached to controlled prescriptions.</summary>
		RxHasProc,
		RxSendNewToQueue,
		///<summary>FK to definition.DefNum.  Represents default adjustment types for sales tax adjustments.</summary>
		SalesTaxAdjustmentType,
		SalesTaxPercentage,
		ScannerCompression,
		ScannerResolution,
		ScannerSuppressDialog,
		///<summary>Set to 1 by default. Selects all providers/employees when loading schedules.</summary>
		ScheduleProvEmpSelectAll,
		ScheduleProvUnassigned,
		///<summary>Boolean. Off by default so that users will have to opt into utilizing the screening with sheets feature.
		///Screening with sheets is extremely customized for Dental3 (D3)</summary>
		ScreeningsUseSheets,
		///<summary>UserGroupNum for Instructors.  Set only for dental schools in dental school setup.</summary>
		SecurityGroupForInstructors,
		///<summary>UserGroupNum for Students.  Set only for dental schools in dental school setup.</summary>
		SecurityGroupForStudents,
		SecurityLockDate,
		///<summary>Set to 0 to always grant permission. 1 means only today.</summary>
		SecurityLockDays,
		SecurityLockIncludesAdmin,
		///<summary>Set to 0 to disable auto logoff.</summary>
		SecurityLogOffAfterMinutes,
		SecurityLogOffWithWindows,
		///<summary>Bool.  True by default.  When enabled and user is on support and on the most recent stable or on any beta version a BugSubmissions will be reported to HQ.</summary>
		SendUnhandledExceptionsToHQ,
		///<summary>The DefNum for the default sheet def to use for Consent sheets</summary>
		SheetsDefaultConsent,
		///<summary>The DefNum for the default sheet def to use for DepositSlip sheets</summary>
		SheetsDefaultDepositSlip,
		///<summary>The DefNum for the default sheet def to use for ExamSheet sheets</summary>
		SheetsDefaultExamSheet,
		///<summary>The DefNum for the default sheet def to use for LapSlip sheets</summary>
		SheetsDefaultLabSlip,
		///<summary>The DefNum for the default sheet def to use for LabelAppointment sheets</summary>
		SheetsDefaultLabelAppointment,
		///<summary>The DefNum for the default sheet def to use for LabelCarrier sheets</summary>
		SheetsDefaultLabelCarrier,
		///<summary>The DefNum for the default sheet def to use for LabelPatient sheets</summary>
		SheetsDefaultLabelPatient,
		///<summary>The DefNum for the default sheet def to use for LabelReferral sheets</summary>
		SheetsDefaultLabelReferral,
		///<summary>The DefNum for the default sheet def to use for MedicalHistory sheets</summary>
		SheetsDefaultMedicalHistory,
		///<summary>The DefNum for the default sheet def to use for MedLabResults sheets</summary>
		SheetsDefaultMedLabResults,
		///<summary>The DefNum for the default sheet def to use for PatientForm sheets</summary>
		SheetsDefaultPatientForm,
		///<summary>The DefNum for the default sheet def to use for PatientLetter sheets</summary>
		SheetsDefaultPatientLetter,
		///<summary>The DefNum for the default sheet def to use for PaymentPlan sheets</summary>
		SheetsDefaultPaymentPlan,
		///<summary>The DefNum for the default sheet def to use for ReferralLetter sheets</summary>
		SheetsDefaultReferralLetter,
		///<summary>The DefNum for the default sheet def to use for ReferralSlip sheets</summary>
		SheetsDefaultReferralSlip,
		///<summary>The DefNum for the default sheet def to use for RoutingSlip sheets</summary>
		SheetsDefaultRoutingSlip,
		///<summary>The DefNum for the default sheet def to use for Rx sheets</summary>
		SheetsDefaultRx,
		///<summary>The DefNum for the default sheet def to use for RxMulti sheets</summary>
		SheetsDefaultRxMulti,
		///<summary>The DefNum for the default sheet def to use for Screening sheets</summary>
		SheetsDefaultScreening,
		///<summary>The DefNum for the default sheet def to use for Statement sheets</summary>
		SheetsDefaultStatement,
		///<summary>The DefNum for the default sheet def to use for TreatmentPlan sheets</summary>
		SheetsDefaultTreatmentPlan,
		ShowAccountFamilyCommEntries,
		///<summary>Set to 1 by default.  Prompts user to allocate unearned income after creating a claim.</summary>
		ShowAllocateUnearnedPaymentPrompt,
		///<summary>Set to 0 by default. Preference that controls if the auto deposit group box shows or not in FormClaimPayEdit.cs</summary>
		ShowAutoDeposit,
		ShowFeatureEhr,
		///<summary>Set to 1 by default.  Shows a button in Edit Patient Information that lets users launch Google Maps.</summary>
		ShowFeatureGoogleMaps,
		ShowFeatureMedicalInsurance,
		///<summary>Set to 1 to enable the Synch Clone button in the Family module which allows users to create and synch clones of patients.</summary>
		ShowFeaturePatientClone,
		ShowFeatureSuperfamilies,
		///<summary>0=None, 1=PatNum, 2=ChartNumber, 3=Birthdate</summary>
		ShowIDinTitleBar,
		///<summary>Boolean.  True by default. If true then the user might be prompted to create a planned appointment when leaving the Chart Module.</summary>
		ShowPlannedAppointmentPrompt,
		ShowProgressNotesInsteadofCommLog,
		///<summary>Deprecated.  Was used to hide the provider payroll report before users had the ability to remove it from the production and income listbox.</summary>
		ShowProviderPayrollReport,
		ShowUrgFinNoteInProgressNotes,
		///<summary>If enabled, allow Providers to digitally sign procedures and proc notes.</summary>
		SignatureAllowDigital,
		///<summary>Used to stop signals after a period of inactivity.  A value of 0 disables this feature.  Default value of 0 to maintain backward compatibility</summary>
		SignalInactiveMinutes,
		///<summary>Only used on startup.  The date in which stale signalods were removed.</summary>
		SignalLastClearedDate,
		///<summary>Blank if not signed. Date signed. For practice level contract, if using clinics see Clinic.SmsContractDate. Record of signing also kept at HQ.</summary>
		SmsContractDate,
		///<summary>(Deprecated) Blank if not signed. Name signed. For practice level contract, if using clinics see Clinic.SmsContractName. Record of signing also kept at HQ.</summary>
		SmsContractName,
		///<summary>Always stored in US dollars. This is the desired limit for SMS outbound messages per month.</summary>
		SmsMonthlyLimit,
		/// <summary>Name of this Software.  Defaults to 'Open Dental Software'.</summary>
		SoftwareName,
		SolidBlockouts,
		SpellCheckIsEnabled,
		StatementAccountsUseChartNumber,
		StatementsCalcDueDate,
		StatementShowCreditCard,
		///<summary>Show payment notes.</summary>
		StatementShowNotes,
		StatementShowAdjNotes,
		StatementShowProcBreakdown,
		StatementShowReturnAddress,
		///<summary>Deprecated.  Not used anywhere.</summary>
		StatementSummaryShowInsInfo,
		StatementsUseSheets,
		///<summary>Used by OD HQ. Indicates whether WebCamOD applications should be sending their images to the server or not.</summary>
		StopWebCamSnapshot,
		///<summary>Deprecated. We no longer allow storing of credit card numbers.</summary>
		StoreCCnumbers,
		StoreCCtokens,
		SubscriberAllowChangeAlways,
		SuperFamSortStrategy,
		SuperFamNewPatAddIns,
		TaskAncestorsAllSetInVersion55,
		TaskListAlwaysShowsAtBottom,
		///<summary>Deprecated.  Not used anywhere.  Previously used for the popup window to show how many new tasks for cur user after login.</summary>
		TasksCheckOnStartup,
		///<summary>If true use task.Status to determine if task is new. Otherwise use task.IsUnread.</summary>
		TasksNewTrackedByUser,
		TasksShowOpenTickets,
		///<summary>Boolean.  0 by default.  Sets appointment task lists to use special logic to sort by AptDateTime.</summary>
		TaskSortApptDateTime,
		///<summary>Boolean.  Defaults to false to hide repeating tasks feature if no repeating tasks are in use when updating to 16.3.</summary>
		TasksUseRepeating,
		///<summary>Keeps track of date of one-time cleanup of temp files.  Prevents continued annoying cleanups after the first month.</summary>
		TempFolderDateFirstCleaned,
		TerminalClosePassword,
		///<summary>If true, treat Yes-No-Unknown status of Unknown as if it were a No.</summary>
		TextMsgOkStatusTreatAsNo,
		TextingDefaultClinicNum,
		TimeCardADPExportIncludesName,
		///<summary>0=Sun,1=Mon...6=Sat</summary>
		TimeCardOvertimeFirstDayOfWeek,
		TimecardSecurityEnabled,
		///<summary>Boolean.  0 by default.  When enabled, FormTimeCard and FormTimeCardMange display H:mm:ss instead of HH:mm</summary>
		TimeCardShowSeconds,
		TimeCardsMakesAdjustmentsForOverBreaks,
		///<summary>bool</summary>
		TimeCardsUseDecimalInsteadOfColon,
		TimecardUsersDontEditOwnCard,
		///<summary>Boolean, false by default. When enabled, main title of FormOpenDental uses clinic abbr instead of description</summary>
		TitleBarClinicUseAbbr,
		TitleBarShowSite,
		///<summary>Deprecated.  Not used anywhere.</summary>
		ToothChartMoveMenuToRight,
		///<summary>The date and time when the OpenDentalService last sent update messages for account debits and credits.</summary>
		TransworldDateTimeLastUpdated,
		///<summary>Determines how often account activity is sent to Transworld.  Default is once per day at the time of day set in the
		///TransworldServiceTimeDue pref.  User can adjust this to be more or less frequent.</summary>
		TransworldServiceSendFrequency,
		///<summary>The time of day for the OpenDentalService to update Transoworld (TSI) with all payments and other debits and credits for families
		///where the guarantor has been sent to TSI for collection.</summary>
		TransworldServiceTimeDue,
		///<summary>FK to definition.DefNum.  Billing type the OpenDentalService will change guarantors to once an account is paid in full.</summary>
		TransworldPaidInFullBillingType,
		///<summary>Boolean,  true by default. When enabled, all procedures considered in the treatment finder report will count towards general benefits.</summary>
		TreatFinderProcsAllGeneral,
		TreatmentPlanNote,
		TreatPlanDiscountAdjustmentType,
		///<summary>Set to 0 to clear out previous discounts.</summary>
		TreatPlanDiscountPercent,
		TreatPlanItemized,
		TreatPlanPriorityForDeclined,
		///<summary>When a TP is signed a PDF will be generated and saved. If disabled, TPs will be redrawn with current data (pre 15.4 behavior).</summary>
		TreatPlanSaveSignedToPdf,
		TreatPlanShowCompleted,
		///<summary>No longer used</summary>
		TreatPlanShowGraphics,
		TreatPlanShowIns,
		///<summary>This preference merely defines what FormOpenDental.IsTreatPlanSortByTooth is on startup.
		///When true, procedures in the treatment plan module sort by priority, date, toothnum, surface, then PK. 
		///When false, does not sort by toothnum or surface. True by default to preserve old behavior.</summary>
		TreatPlanSortByTooth,
		///<summary>Deprecated.  All new TPs use sheets as of 17.1.  Old Printing for classic sheets is handled by ContrTreat.cs bool DoPrintUsingSheets.</summary>
		TreatPlanUseSheets,
		///<summary>Used by OD HQ. Not added to db convert script. Number of calls in triage.///</summary>
		TriageCalls,
		///<summary>Used by OD HQ. Not added to db convert script. Number of red calls in triage.///</summary>
		TriageRedCalls,
		///<summary>Used by OD HQ. Not added to db convert script. Minutes behind on red calls.///</summary>
		TriageRedTime,
		///<summary>Used by OD HQ. Not added to db convert script. Minutes behind on calls for red alert.///</summary>
		TriageTime,
		///<summary>Used by OD HQ. Not added to db convert script. Minutes behind on calls for yellow alert.///</summary>
		TriageTimeWarning,
		TrojanExpressCollectBillingType,
		TrojanExpressCollectPassword,
		TrojanExpressCollectPath,
		TrojanExpressCollectPreviousFileNumber,
		///<summary>Can be any int.  Defaults to 0.</summary>
		UnschedDaysFuture,
		///<summary>Can be any int.  Defaults to 365.</summary>
		UnschedDaysPast,
		UpdateCode,
		UpdateInProgressOnComputerName,
		///<summary>Described in the Update Setup window and in the manual.  Can contain multiple db names separated by commas.  Should not include current db name.</summary>
		UpdateMultipleDatabases,
		UpdateServerAddress,
		UpdateShowMsiButtons,
		///<summary>The next update date and time, set in FormUpdateSetup.  When this is set in the future, the main form's title bar will count down to the set time.</summary>
		UpdateDateTime,
		///<summary>Use GetStringNoCache() to get the value of this preference.</summary>
		UpdateStreamLinePassword,
		UpdateWebProxyAddress,
		UpdateWebProxyPassword,
		UpdateWebProxyUserName,
		UpdateWebsitePath,
		UpdateWindowShowsClassicView,
		UseBillingAddressOnClaims,
		///<summary>Enum:ToothNumberingNomenclature 0=Universal(American), 1=FDI, 2=Haderup, 3=Palmer</summary>
		UseInternationalToothNumbers,
		///<summary>Boolean.  0 by default.  When enabled, users must enter their user name manually at the log on window.</summary>
		UserNameManualEntry,
		///<summary>Boolean. 0 by default. When enabled, chart module procedures that are complete will use the provider's color as row's background color</summary>
		UseProviderColorsInChart,
		///<summary>Used by OD HQ. Not added to db convert script. The path where voice mails will be archived.</summary>
		VoiceMailArchivePath,
		///<summary>Used by OD HQ. Not added to db convert script. Number of voicemails that will make the alert color change.///</summary>
		VoicemailCalls,
		///<summary>Used by OD HQ. Not added to db convert script. The path where voice mails will be saved when they are entered in the database.
		///</summary>
		VoiceMailCreatePath,
		///<summary>Used by OD HQ. Not added to db convert script. Delete the voice mail if it is older than this many days.
		///Set to empty string to never delete voice mails.</summary>
		VoiceMailDeleteAfterDays,
		///<summary>Used by OD HQ. Not added to db convert script. The last time that the voice mail monitoring thread started its monitoring.</summary>
		VoiceMailMonitorHeartBeat,
		///<summary>Used by OD HQ. Not added to db convert script. The path where the phone tracking server looks for newly created voice mails
		///This preference stores a JSON-serialized list of KeyValuePairs holding a path for each computer name.</summary>
		VoiceMailOriginationPath,
		///<summary>Used by OD HQ.  Not added to db convert script.  Boolean. True if we are using SMB2 and false if we are using SMB1.</summary>
		VoiceMailSMB2Enabled,
		///<summary>Used by OD HQ.  Not added to db convert script.  The password in plain text that our required to access files via SMB2.</summary>
		VoiceMailSMB2Password,
		///<summary>Used by OD HQ.  Not added to db convert script.  The user name in plain text that our required to access files via SMB2.</summary>
		VoiceMailSMB2UserName,
		///<summary>Used by OD HQ. Not added to db convert script. How many minutes we can be behind on voicemails before alert color changes.///</summary>
		VoicemailTime,
		WaitingRoomAlertColor,
		///<summary>0 to disable.  When enabled, sets rows to alert color based on wait time.</summary>
		WaitingRoomAlertTime,
		///<summary>Boolean.  0 by default.  When enabled, the waiting room will filter itself by the selected appointment view.  0, normal filtering, will show all patients waiting for the entire practice (or entire clinic when using clinics).</summary>
		WaitingRoomFilterByView,
		///<summary>DEPRECATED.  Used by OD HQ.  Not added to db convert script.  No UI to change this value.
		///Determines how often in milliseconds that WebCamOD should capture and send a picture to the phone table.
		///If this value is manually changed, all Web Cams need to be restarted for the change to take effect.</summary>
		WebCamFrequencyMS,
		///<summary>Boolean.  1 by default.  Determines whether or not the checkbox in FormWebFormSetup is checked by default.</summary>
		WebFormsAutoFillNameAndBirthdate,
		///<summary>Only used for sheet synch.  See Mobile... for URL for mobile synch.</summary>
		WebHostSynchServerURL,
		///<summary>The template that will be used for Web Sched automation when a reminder for multiple recalls is sent to the same phone number. There
		///is no UI to change this preference.</summary>
		WebSchedAggregatedTextMessage,
		///<summary>The template that will be used for Web Sched automation when a reminder for multiple recalls is sent to the same email. There
		///is no UI to change this preference.</summary>
		WebSchedAggregatedEmailBody,
		///<summary>The template that will be used for Web Sched automation when a reminder for multiple recalls is sent to the same email. There
		///is no UI to change this preference.</summary>
		WebSchedAggregatedEmailSubject,
		///<summary>The subject line used for Web Sched ASAP emails.</summary>
		WebSchedAsapEmailSubj,
		///<summary>The template used for Web Sched ASAP email bodies.</summary>
		WebSchedAsapEmailTemplate,
		///<summary>Boolean. 0 by default. True when Web Sched ASAP service is enabled.
		///The eConnector keeps this preference current with OD HQ, calling our web service to verify status.</summary>
		WebSchedAsapEnabled,
		///<summary>The maximum number of texts allowed to be sent to a patient in a day. Blank means no limit.</summary>
		WebSchedAsapTextLimit,
		///<summary>The template used for Web Sched ASAP texts.</summary>
		WebSchedAsapTextTemplate,
		///<summary>Stored as an int value from the WebSchedAutomaticSend enum.</summary>
		WebSchedAutomaticSendSetting,
		///<summary>Stored as an int value from the WebSchedAutomaticSendText enum.</summary>
		WebSchedAutomaticSendTextSetting,
		WebSchedMessage,
		WebSchedMessageText,
		WebSchedMessage2,
		WebSchedMessageText2,
		WebSchedMessage3,
		WebSchedMessageText3,
		///<summary>The number of text messages sent automatically per batch. Currently one batch runs every 10 minutes.</summary>
		WebSchedTextsPerBatch,
		///<summary>Determines whether or not the birthdate of the Web Sched New Patient gets validated for being 18 years or older.</summary>
		WebSchedNewPatAllowChildren,
		///<summary>Boolean, false by default. If true, patients will be able to select their provider for Web Sched New Pat.</summary>
		WebSchedNewPatAllowProvSelection,
		///<summary>Deprecated as of 17.1, use signup portal.  Boolean.  0 by default.  True when the New Patient Appointment version of Web Sched is enabled.
		///Loosely keeps track of service status, calling our web service to verify active service is still required.</summary>
		WebSchedNewPatApptEnabled,
		///<summary>Boolean.  Defaults to true.  Determines whether or not the phone number field is forced to be formatted (currently only US format XXX-XXX-XXXX)</summary>
		WebSchedNewPatApptForcePhoneFormatting,
		///<summary>Comma delimited list.  Empty by default.  Stores the defnums to blockouts to ignore in Web Sched Recall web app.</summary>
		WebSchedNewPatApptIgnoreBlockoutTypes,
		///<summary>String.  Is not empty by default.  Stores the message that will show up on the Web Sched New Pat web application.</summary>
		WebSchedNewPatApptMessage,
		///<summary>Deprecated in v18.1.1 - Utilize appointment types instead.
		///Comma delimited list of procedures that should be put onto the new patient appointment.</summary>
		WebSchedNewPatApptProcs,
		///<summary>Deprecated in v18.1.1 - Utilize appointment types instead.
		///The time pattern that will be used to determine the length of the new patient appointment.
		///This time pattern is stored as /'s and X's that each represent an amount of time dictated by the current AppointmentTimeIncrement pref.
		///This functionality matches the recall system, not the appointment system (which always stores /'s and X's as 5 mins).</summary>
		WebSchedNewPatApptTimePattern,
		///<summary>Integer.  Represents the number of days into the future we will go before searching for available time slots.
		///Empty will start looking for available time slots today.</summary>
		WebSchedNewPatApptSearchAfterDays,
		///<summary>DefNum for the ApptConfirm status type that will automatically be assigned to Web Sched new patient appointments.</summary>
		WebSchedNewPatConfirmStatus,
		///<summary>Require new patient to respond to 2-step verification email before creating new appointment.</summary>
		WebSchedNewPatDoAuthEmail,
		///<summary>Require new patient to respond to 2-step verification text message before creating new appointment.</summary>
		WebSchedNewPatDoAuthText,
		///<summary>Boolean, default is true.  Used to verify important user information, such as if they are a new patient and how old they are.</summary>
		WebSchedNewPatVerifyInfo,
		/// <summary>String, the webforms url that should be launched after a new patient signs up using web sched.</summary>
		WebSchedNewPatWebFormsURL,
		///<summary>Enum: WebSchedProviderRules 0=FirstAvailable, 1=PrimaryProvider, 2=SecondaryProvider, 3=LastSeenHygienist</summary>
		WebSchedProviderRule,
		///<summary>Boolean, true by default. If true, patients will be able to select their provider for Web Sched Recall.</summary>
		WebSchedRecallAllowProvSelection,
		///<summary>DefNum for the ApptConfirm status type that will automatically be assigned to Web Sched Recall appointments.</summary>
		WebSchedRecallConfirmStatus,
		///<summary>Comma delimited list.  Empty by default.  Stores the defnums to blockouts to ignore in Web Sched Recall web app.</summary>
		WebSchedRecallIgnoreBlockoutTypes,
		///<summary>In seconds, how often the eConnector thread runs that sends Web Sched notifications. This preference can be updated from OD HQ.
		///</summary>
		WebSchedSendThreadFrequency,
		///<summary>In seconds, how often the eConnector thread runs that sends Web Sched ASAP messages.</summary>
		WebSchedSendASAPThreadFrequency,
		///<summary>Boolean. 0 by default. True when Web Sched service is enabled.
		///The eConnector keeps this preference current with OD HQ, calling our web service to verify active service is still required.</summary>
		WebSchedService,
		WebSchedSubject,
		WebSchedSubject2,
		WebSchedSubject3,
		WebServiceHQServerURL,
		WebServiceServerName,
		///<summary>If enabled, allows users to right click on ODTextboxes or ODGrids to populate the context menu with any detected wiki links.</summary>
		WikiDetectLinks,
		///<summary>If enabled, allows users to create new wiki pages when following links from textboxes and grids. (Disable to prevent proliferation of misspelled wiki pages.)</summary>
		WikiCreatePageFromLink,
		WordProcessorPath,
		XRayExposureLevel
	}

	///<summary>PrefName-related attributes.</summary>
	public class PrefNameAttribute:Attribute {
		private PrefValueType _valueType=PrefValueType.NONE;
		
		public PrefValueType ValueType{
			get{
				return _valueType;
			}
			set{
				_valueType=value;
			}
		}
	}

	public enum PrefValueType{
		NONE,
		BOOL,
		STRING,
		ENUM
	}

	///<summary>Used by pref "AppointmentSearchBehavior". </summary>
	public enum SearchBehaviorCriteria {
		ProviderTime,
		ProviderTimeOperatory
	}

	///<summary>Used by pref "AccountingSoftware".  0=OpenDental, 1=QuickBooks</summary>
	public enum AccountingSoftware {
		OpenDental,
		QuickBooks
	}

	public enum RigorousAccounting {
		///<summary>0 - Auto-splitting payments and enforcing paysplit validity is enforced.</summary>
		[Description("Enforce Fully")]
		EnforceFully,
		///<summary>1 - Auto-splitting payments is done, paysplit validity isn't enforced.</summary>
		[Description("Auto-Split Only")]
		AutoSplitOnly,
		///<summary>2 - Neither auto-splitting nor paysplit validity is enforced.</summary>
		[Description("Don't Enforce")]
		DontEnforce
	}

	public enum RigorousAdjustments {
		///<summary>0 - Automatically link adjustments and procedures, adjustment linking enforced.</summary>
		[Description("Enforce Fully")]
		EnforceFully,
		///<summary>1 - Adjustment links are made automatically, but it can be edited.</summary>
		[Description("Link Only")]
		LinkOnly,
		///<summary>2 - Adjustment links aren't made, nor are they enforced.</summary>
		[Description("Don't Enforce")]
		DontEnforce
	}

	///<summary>Used by pref "WebSchedProviderRule". Determines how Web Sched will decide on what provider time slots to show patients.</summary>
	public enum WebSchedProviderRules {
		///<summary>0 - Dynamically picks the first available provider based on the time slot picked by the patient.</summary>
		FirstAvailable,
		///<summary>1 - Only shows time slots that are available via the patient's primary provider.</summary>
		PrimaryProvider,
		///<summary>2 - Only shows time slots that are available via the patient's secondary provider.</summary>
		SecondaryProvider,
		///<summary>3 - Only shows time slots that are available via the patient's last seen hygienist.</summary>
		LastSeenHygienist
	}
	



}

using System;
using System.Collections;
using System.ComponentModel;

namespace OpenDentBusiness{

	///<summary>Every user group has certain permissions.  This defines a permission for a group.  The absense of permission would cause that row to be deleted from this table.</summary>
	[Serializable]
	[CrudTable(IsSynchable=true)]

	public class GroupPermission:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long GroupPermNum;
		///<summary>Only granted permission if newer than this date.  Can be Minimum (01-01-0001) to always grant permission.</summary>
		public DateTime NewerDate;
		///<summary>Can be 0 to always grant permission.  Otherwise, only granted permission if item is newer than the given number of days.  1 would mean only if entered today.</summary>
		public int NewerDays;
		///<summary>FK to usergroup.UserGroupNum.  The user group for which this permission is granted.  If not authorized, then this groupPermission will have been deleted.</summary>
		public long UserGroupNum;
		///<summary>Enum:Permissions</summary>
		public Permissions PermType;
		///<summary>Generic foreign key to any other table.  Typically used in combination with PermType to give permission to specific things.</summary>
		public long FKey;

		///<summary></summary>
		public GroupPermission Copy(){
			return (GroupPermission)this.MemberwiseClone();
		}

	}

	///<summary>A hard-coded list of permissions which may be granted to usergroups.</summary>
	public enum Permissions {
		///<summary>0</summary>
		[Description("")]
		None,
		///<summary>1</summary>
		[Description("Appointments Module")]
		AppointmentsModule,
		///<summary>2</summary>
		[Description("Family Module")]
		FamilyModule,
		///<summary>3</summary>
		[Description("Account Module")]
		AccountModule,
		///<summary>4</summary>
		[Description("TreatmentPlan Module")]
		TPModule,
		///<summary>5</summary>
		[Description("Chart Module")]
		ChartModule,
		///<summary>6</summary>
		[Description("Image Module")]
		ImagesModule,
		///<summary>7</summary>
		[Description("Manage Module")]
		ManageModule,
		///<summary>8. Currently covers a wide variety of setup functions. </summary>
		[Description("Setup - Covers a wide variety of setup functions")]
		Setup,
		///<summary>9</summary>
		[Description("Rx Create")]
		RxCreate,
		///<summary>10. Uses date restrictions.  Covers editing AND deleting of Completed, EO, and EC procs.  Deleting procs of other statuses are covered by ProcDelete.</summary>
		[Description("Edit Completed Procedure (full)")]
		ProcComplEdit,
		///<summary>11</summary>
		[Description("Choose Database")]
		ChooseDatabase,
		///<summary>12</summary>
		[Description("Schedules - Practice and Provider")]
		Schedules,
		///<summary>13</summary>
		[Description("Blockouts")]
		Blockouts,
		///<summary>14. Uses date restrictions.</summary>
		[Description("Claim Sent Edit")]
		ClaimSentEdit,
		///<summary>15</summary>
		[Description("Payment Create")]
		PaymentCreate,
		///<summary>16. Uses date restrictions.</summary>
		[Description("Payment Edit")]
		PaymentEdit,
		///<summary>17</summary>
		[Description("Adjustment Create")]
		AdjustmentCreate,
		///<summary>18. Uses date restrictions.</summary>
		[Description("Adjustment Edit")]
		AdjustmentEdit,
		///<summary>19</summary>
		[Description("User Query")]
		UserQuery,
		///<summary>20.  Not used anymore.</summary>
		StartupSingleUserOld,
		///<summary>21 Not used anymore.</summary>
		StartupMultiUserOld,
		///<summary>22</summary>
		[Description("Reports")]
		Reports,
		///<summary>23. Includes setting procedures complete.</summary>
		[Description("Create Completed Procedure (or set complete)")]
		ProcComplCreate,
		///<summary>24. At least one user must have this permission.</summary>
		[Description("Security Admin")]
		SecurityAdmin,
		///<summary>25. </summary>
		[Description("Appointment Create")]
		AppointmentCreate,
		///<summary>26</summary>
		[Description("Appointment Move")]
		AppointmentMove,
		///<summary>27</summary>
		[Description("Appointment Edit")]
		AppointmentEdit,
		///<summary>28</summary>
		[Description("Backup")]
		Backup,
		///<summary>29</summary>
		[Description("Edit All Timecards")]
		TimecardsEditAll,
		///<summary>30</summary>
		[Description("Deposit Slips")]
		DepositSlips,
		///<summary>31. Uses date restrictions.</summary>
		[Description("Accounting Edit Entry")]
		AccountingEdit,
		///<summary>32. Uses date restrictions.</summary>
		[Description("Accounting Create Entry")]
		AccountingCreate,
		///<summary>33</summary>
		[Description("Accounting")]
		Accounting,
		///<summary>34</summary>
		[Description("Intake Anesthetic Medications into Inventory")]
		AnesthesiaIntakeMeds,
		///<summary>35</summary>
		[Description("Edit Anesthetic Records; Edit/Adjust Inventory Counts")]
		AnesthesiaControlMeds,
		///<summary>36</summary>
		[Description("Insurance Payment Create")]
		InsPayCreate,
		///<summary>37. Uses date restrictions. Edit Batch Insurance Payment.</summary>
		[Description("Insurance Payment Edit")]
		InsPayEdit,
		///<summary>38. Uses date restrictions.</summary>
		[Description("Edit Treatment Plan")]
		TreatPlanEdit,
		///<summary>39. DEPRECATED</summary>
		[Description("Reports - Production and Income, Aging")]
		ReportProdInc,
		///<summary>40. Uses date restrictions.</summary>
		[Description("Timecard Delete Entry")]
		TimecardDeleteEntry,
		///<summary>41. Uses date restrictions. All other equipment functions are covered by .Setup.</summary>
		[Description("Equipment Delete")]
		EquipmentDelete,
		///<summary>42. Uses date restrictions. Also used in audit trail to log web form importing.</summary>
		[Description("Sheet Edit")]
		SheetEdit,
		///<summary>43. Uses date restrictions.</summary>
		[Description("Commlog Edit")]
		CommlogEdit,
		///<summary>44. Uses date restrictions.</summary>
		[Description("Image Delete")]
		ImageDelete,
		///<summary>45. Uses date restrictions.</summary>
		[Description("Perio Chart Edit")]
		PerioEdit,
		///<summary>46. Shows the fee textbox in the proc edit window.</summary>
		[Description("Show Procedure Fee")]
		ProcEditShowFee,
		///<summary>47</summary>
		[Description("Adjustment Edit Zero Amount")]
		AdjustmentEditZero,
		///<summary>48</summary>
		[Description("EHR Emergency Access")]
		EhrEmergencyAccess,
		///<summary>49. Uses date restrictions.  This only applies to non-completed procs.  Deletion of completed procs is covered by ProcComplEdit.</summary>
		[Description("TP Procedure Delete")]
		ProcDelete,
		///<summary>50 - Only used at OD HQ.  No user interface.</summary>
		[Description("Ehr Key Add")]
		EhrKeyAdd,
		///<summary>51- Allows user to edit all providers. This is not fine-grained enough for extremely large organizations such as dental schools, so other permissions are being added as well.</summary>
		[Description("Providers")]
		Providers,
		///<summary>52</summary>
		[Description("eCW Appointment Revise")]
		EcwAppointmentRevise,
		///<summary>53</summary>
		[Description("Procedure Note (full)")]
		ProcedureNoteFull,
		///<summary>54</summary>
		[Description("Referral Add")]
		ReferralAdd,
		///<summary>55</summary>
		[Description("Insurance Plan Change Subscriber")]
		InsPlanChangeSubsc,
		///<summary>56</summary>
		[Description("Referral, Attach to Patient")]
		RefAttachAdd,
		///<summary>57</summary>
		[Description("Referral, Delete from Patient")]
		RefAttachDelete,
		///<summary>58</summary>
		[Description("Carrier Create")]
		CarrierCreate,
		///<summary>59</summary>
		[Description("Reports - Graphical")]
		GraphicalReports,
		///<summary>60</summary>
		[Description("Auto/Quick Note Edit")]
		AutoNoteQuickNoteEdit,
		///<summary>61</summary>
		[Description("Equipment Setup")]
		EquipmentSetup,
		///<summary>62</summary>
		[Description("Billing")]
		Billing,
		///<summary>63</summary>
		[Description("Problem Edit")]
		ProblemEdit,
		///<summary>64- There is no user interface in the security window for this permission.  It is only used for tracking.  FK to CodeNum.</summary>
		[Description("Proc Fee Edit")]
		ProcFeeEdit,
		///<summary>65- There is no user interface in the security window for this permission.  It is only used for tracking.  Only tracks changes to carriername, not any other carrier info.  FK to PlanNum for tracking.</summary>
		[Description("TP InsPlan Change Carrier Name")]
		InsPlanChangeCarrierName,
		///<summary>66- (Was named TaskEdit prior to version 14.2.39) When editing an existing task: delete the task, edit original description, or double click on note rows.  Even if you don't have the permission, you can still edit your own task description (but not the notes) as long as it's in your inbox and as long as nobody but you has added any notes. </summary>
		[Description("Task Note Edit")]
		TaskNoteEdit,
		///<summary>67- Add or delete lists and list columns..</summary>
		[Description("Wiki List Setup")]
		WikiListSetup,
		///<summary>68- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks copying of patient information.  Required by EHR.</summary>
		[Description("Copy")]
		Copy,
		///<summary>69- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks printing of patient information.  Required by EHR.</summary>
		[Description("Printing")]
		Printing,
		///<summary>70- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks viewing of patient medical information.</summary>
		[Description("Medical Info Viewed")]
		MedicalInfoViewed,
		///<summary>71- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and editing of patient problems.</summary>
		[Description("Pat Problem List Edit")]
		PatProblemListEdit,
		///<summary>72- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and edting of patient medications.</summary>
		[Description("Pat Medication List Edit")]
		PatMedicationListEdit,
		///<summary>73- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and editing of patient allergies.</summary>
		[Description("Pat Allergy List Edit")]
		PatAllergyListEdit,
		///<summary>74- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and editing of patient family health history.</summary>
		[Description("Pat Family Health Edit")]
		PatFamilyHealthEdit,
		///<summary>75- There is no user interface in the security window for this permission.  It is only used for tracking.  Patient Portal access of patient information.  Required by EHR.</summary>
		[Description("Patient Portal")]
		PatientPortal,
		///<summary>76</summary>
		[Description("Rx Edit")]
		RxEdit,
		///<summary>77- Assign this permission to a staff person who will administer setting up and editing Dental School Students in the system.</summary>
		[Description("Student Edit")]
		AdminDentalStudents,
		///<summary>78- Assign this permission to an instructor who will be allowed to assign Grades to Dental School Students as well as manage classes assigned to them.</summary>
		[Description("Instructor Edit")]
		AdminDentalInstructors,
		///<summary>79- Uses date restrictions.  Has a unique audit trail so that users can track specific ortho chart edits.  FK to OrthoChartNum.</summary>
		[Description("Ortho Chart Edit (full)")]
		OrthoChartEditFull,
		///<summary>80- There is no user interface in the security window for this permission.  It is only used for tracking.  Mainly used for ortho clinics.</summary>
		[Description("Patient Field Edit")]
		PatientFieldEdit,
		///<summary>81- Assign this permission to a staff person who will edit evaluations in case of an emergency.  This is not meant to be a permanent permission given to a group.</summary>
		[Description("Admin Evaluation Edit")]
		AdminDentalEvaluations,
		///<summary>82- There is no user interface in the security window for this permission.  It is only used for tracking.</summary>
		[Description("Treat Plan Discount Edit")]
		TreatPlanDiscountEdit,
		///<summary>83- There is no user interface in the security window for this permission.  It is only used for tracking.</summary>
		[Description("User Log On Off")]
		UserLogOnOff,
		///<summary>84- Allows user to edit other users' tasks.</summary>
		[Description("Task Edit")]
		TaskEdit,
		///<summary>85- Allows user to send unsecured email</summary>
		[Description("Email Send")]
		EmailSend,
		///<summary>86- Allows user to send webmail</summary>
		[Description("Webmail Send")]
		WebMailSend,
		///<summary>87- Allows user to run, edit, and write non-released queries.</summary>
		[Description("User Query Admin")]
		UserQueryAdmin,
		///<summary>88- Security permission for assignment of benefits.</summary>
		[Description("Insurance Plan Change Assignement of Benefits")]
		InsPlanChangeAssign,
		///<summary>89- Audit trail for images and documents in the image module.  There is no user interface in the security window for this permission because it is only used for tracking.</summary>
		[Description("Image Edit")]
		ImageEdit,
		///<summary>90- Allows editing of all measure events.  Also used to track changes made to events.</summary>
		[Description("EHR Measure Event Edit")]
		EhrMeasureEventEdit,
		///<summary>91- Allows users to edit settings in the eServices Setup window.  Also causes the Listener Service monitor thread to start upon logging in.</summary>
		[Description("EServices Setup")]
		EServicesSetup,
		///<summary>92- Allows users to edit Fee Schedules throughout the program.  Logs editing of fee schedule properties.</summary>
		[Description("Fee Schedule Edit")]
		FeeSchedEdit,
		///<summary>93- Allows user to edit and delete provider specific fees overrides.</summary>
		[Description("Provider Fee Edit")]
		ProviderFeeEdit,
		///<summary>94- Allows user to merge patients.</summary>
		[Description("Patient Merge")]
		PatientMerge,
		///<summary>95- Only used in Claim History Status Edit</summary>
		[Description("Claim History Edit")]
		ClaimHistoryEdit,
		///<summary>96- Allows user to edit a completed appointment.</summary>
		[Description("Completed Appointment Edit")]
		AppointmentCompleteEdit,
		///<summary>97- Audit trail for deleting webmail messages.  There is no user interface in the security window for this permission.</summary>
		[Description("Webmail Delete")]
		WebMailDelete,
		///<summary>98- Audit trail for saving a patient with required fields missing.  There is no user interface in the security window for this 
		///permission.</summary>
		[Description("Required Fields Missing")]
		RequiredFields,
		///<summary>99- Allows user to merge referrals.</summary>
		[Description("Referral Merge")]
		ReferralMerge,
		///<summary>100- There is no user interface in the security window for this permission.  It is only used for tracking.
		///Currently only used for tracking automatically changing the IsCpoe flag on procedures.  Can be enhanced to do more in the future.
		///There is only one place where we could have automatically changed IsCpoe without a corresponding log of a different permission.
		///That place is in the OnClosing of the Procedure Edit window.  We update this flag even when the user Cancels out of it.</summary>
		[Description("Proc Edit")]
		ProcEdit,
		///<summary>101- Allows user to use the provider merge tool.</summary>
		[Description("Provider Merge")]
		ProviderMerge,
		///<summary>102- Allows user to use the medication merge tool.</summary>
		[Description("Medication Merge")]
		MedicationMerge,
		///<summary>103- Allow users to use the Quick Add tool in the Account module.</summary>
		[Description("Account Procs Quick Add")]
		AccountProcsQuickAdd,
		///<summary>104- Allow users to send claims.</summary>
		[Description("Claim Send")]
		ClaimSend,
		///<summary>105- Allow users to create new task lists.</summary>
		[Description("TaskList Create")]
		TaskListCreate,
		///<summary>106 - Audit when a new patient is added.</summary>
		[Description("Patient Create")]
		PatientCreate,
		///<summary>107- Allows changing the settings for graphical repots.</summary>
		[Description("Reports - Graphical Setup")]
		GraphicalReportSetup,
		///<summary>108 - Audit when a patient is edited.</summary>
		[Description("Patient Edit")]
		PatientEdit,
		///<summary>109 - Audit when an insurance plan is created.  Currently only used in X12 834 insurance plan import.</summary>
		[Description("Insurance Plan Create")]
		InsPlanCreate,
		///<summary>110 - Audit when an insurance plan is edited.  Currently only used in X12 834 insurance plan import.</summary>
		[Description("Insurance Plan Edit")]
		InsPlanEdit,
		///<summary>111 - Audit when an insurance subscriber is created.  The naming convention of this permission was decided upon by Nathan and Derek
		///based on the following existing permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign.
		///Currently only used in X12 834 insurance plan import.</summary>
		[Description("Insurance Plan Create Subscriber")]
		InsPlanCreateSub,
		///<summary>112 - Audit when an insurance subscriber is edited.  The naming convention of this permission was decided upon by Nathan and Derek
		///based on the following existing permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign.
		///Currently only used in X12 834 insurance plan import.</summary>
		[Description("Insurance Plan Edit Subscriber")]
		InsPlanEditSub,
		///<summary>113 - Audit when a patient is added to an insurance plan.  The naming convention of this permission was decided upon by Nathan and
		///Derek based on the following existing permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign.
		///Currently only used in X12 834 insurance plan import.</summary>
		[Description("Insurance Plan Add Patient")]
		InsPlanAddPat,
		///<summary>114 - Audit when a patient is dropped from an insurance plan.  The naming convention of this permission was decided upon by Nathan and
		///Derek based on the following existing permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign.
		///Currently only used in X12 834 insurance plan import.</summary>
		[Description("Insurance Plan Drop Patient")]
		InsPlanDropPat,
		///<summary>115 - Allows users to be assigned Insurance Verifications.</summary>
		[Description("Insurance Plan Verificaiton Assign")]
		InsPlanVerifyList,
		///<summary>116 - Allows users to bypass the global lock date to add paysplits.</summary>
		[Description("Pay Split Create after Global Lock Date")]
		SplitCreatePastLockDate,
		///<summary>117 - Uses date restrictions.  Covers editing some fields of completed procs.  Limited list includes treatment area, diagnosis,
		///add adjustment, Do Not Bill To Ins, Hide Graphics, Misc tab, Medical tab, E-claim note, and the Prosthesis Replacement group box.</summary>
		[Description("Edit Completed Procedure (limited)")]
		ProcComplEditLimited,
		///<summary>118 - Uses date restrictions based on the SecDateEntry field as the claim date.  Covers deleting a claim of any status
		///(Sent, Waiting to Send, Received, etc).</summary>
		[Description("Claim Delete")]
		ClaimDelete,
		///<summary>119 - Covers editing the Write Off and Write Off Override fields for claimprocs as well as deleting/creating claimprocs.
		///<para>Uses date/days restriction based on the attached proc.DateEntryC; unless it's a total payment, then uses claimproc.SecDateEntry.</para>
		///<para>Applies to all plan types (i.e. PPO, Category%, Capitation, etc).</para></summary>
		[Description("Insurance Write Off Edit")]
		InsWriteOffEdit,
		///<summary>120 - Allows users to change appointment confirmation status.</summary>
		[Description("Appointment Confirmation Status Edit")]
		ApptConfirmStatusEdit,
		///<summary>121 - Audit trail for when users change graphical settings for another workstation in FormGraphics.cs.</summary>
		GraphicsRemoteEdit,
		///<summary>122 - Audit Trail (Separated from SecurityAdmin permission)</summary>
		[Description("Audit Trail")]
		AuditTrail,
		///<summary>123 - Allows the user to change the presenter on a treatment plan.</summary>
		[Description("Edit Treatment Plan Presenter")]
		TreatPlanPresenterEdit,
		///<summary>124 - Allows users to use the Alphabetize Provider button from FormProviderSetup to permanently re-order providers.</summary>
		[Description("Providers - Alphabetize")]
		ProviderAlphabetize,
		///<summary>125 - Allows editing of claimprocs that are marked as received status.</summary>
		[Description("Claim Procedure Received Edit")]
		ClaimProcReceivedEdit,
		///<summary>126 - Used to diagnose an error in statement creation. Audit Trail Permission Only</summary>
		StatementPatNumMismatch,
		///<summary>127 - User has access to Mobile Web.</summary>
		[Description("Mobile Web")]
		MobileWeb,
		///<summary>128 - For logging purposes only.  Used when PatPlans are created and not otherwise logged.</summary>
		PatPlanCreate,
		///<summary>129 - Allows the user to change a patient's primary provider, with audit trail logging.</summary>
		[Description("Patient Primary Provider Edit")]
		PatPriProvEdit,
		///<summary>130</summary>
		[Description("Referral Edit")]
		ReferralEdit,
		///<summary>131 - Allows users to change a patient's billing type.</summary>
		[Description("Patient Billing Type Edit")]
		PatientBillingEdit,
		///<summary>132 - Allows viewing annual prod inc of all providers instead of just a single provider.</summary>
		[Description("Production and Income - View All Providers")]
		ReportProdIncAllProviders,
		///<summary>133 - Allows running daily reports. DEPRECATED.</summary>
		[Description("Reports - Daily")]
		ReportDaily,
		///<summary>134 - Allows viewing daily prod inc of all providers instead of just a single provider</summary>
		[Description("Daily payments - View All Providers")]
		ReportDailyAllProviders,
		///<summary>135 - Allows user to change the appointment schedule flag.</summary>
		[Description("Patient Restriction Edit")]
		PatientApptRestrict,
		///<summary>136 - Allows deleting sheets when they're associated to patients.</summary>
		[Description("Sheet Delete")]
		SheetDelete,
		///<summary>137 - Allows updating custom tracking on claims.</summary>
		[Description("Update Custom Tracking")]
		UpdateCustomTracking,
		///<summary>138 - Allows people to set graphics option for the workstation and other computers.</summary>
		[Description("Graphics Edit")]
		GraphicsEdit,
		///<summary>139 - Allows user to change the fields within the Ortho tab of the Ins Plan Edit window.</summary>
		[Description("Insurance Plan Ortho Edit")]
		InsPlanOrthoEdit,
		///<summary>140 - Allows user to change the provider on claimproc when claimproc is attached to a claim.</summary>
		[Description("Claim Procedure Provider Edit When Attached to Claim")]
		ClaimProcClaimAttachedProvEdit,
		///<summary>141 - Audit when insurance plans are merged.</summary>
		[Description("Insurance Plan Combine")]
		InsPlanMerge,
		///<summary>142 - Allows user to combine carriers.</summary>
		[Description("Insurance Carrier Combine")]
		InsCarrierCombine,
		///<summary>143 - Allows user to edit popups. A user without this permission will still be able to edit their own popups.</summary>
		[Description("Popup Edit (other users)")]
		PopupEdit,
		///<summary>144 - Allows user to select new insplan from list prior to dropping current insplan associated with a patplan.</summary>
		[Description("Change existing Ins Plan using Pick From List")]
		InsPlanPickListExisting,
		///<summary>145 - Allows user to edit their own signed ortho charts even if they don't have full permission.</summary>
		[Description("Ortho Chart Edit (same user, signed)")]
		OrthoChartEditUser,
		///<summary>146 - Allows user to edit procedure notes that they created themselves if they don't have full permission.</summary>
		[Description("Procedure Note (same user)")]
		ProcedureNoteUser,
		///<summary>147 - Allows user to edit group notes signed by other users. If a user does not have this permission, they can still edit group notes
		///that they themselves have signed.</summary>
		[Description("Group Note Edit (other users, signed)")]
		GroupNoteEditSigned,
		///<summary>148 - Allows user to lock and unlock wiki pages.  Also allows the user to edit locked wiki pages.</summary>
		[Description("Wiki Admin")]
		WikiAdmin,
		///<summary>149 - Allows user to create, edit, close, and delete payment plans.</summary>
		[Description("Pay Plan Edit")]
		PayPlanEdit,
		///<summary>150 - Used for logging when a claim is created, cancelled, or saved. </summary>
		ClaimEdit,
		///<summary>151- Allows user to run command queries. Command queries are any non-SELECT queries for any non-temporary table.</summary>
		[Description("Command Query")]
		CommandQuery,
		///<summary>152 - Gives user access to the replication setup window.</summary>
		[Description("Replication Setup")]
		ReplicationSetup,
		///<summary>153 - Allows user to edit and delete sent and received pre-auths. Uses date restriction.</summary>
		[Description("PreAuth Sent Edit")]
		PreAuthSentEdit,
		///<summary>154 - Edit fees (for logging only). Security log entry for this points to feeNum instead of CodeNum. </summary>
		LogFeeEdit,
		///<summary>155 - Log ClaimProcEdit</summary>
		LogSubscriberEdit,
		///<summary>156 - Logs changes to recalls, recalltypes, and recaltriggers.</summary>
		RecallEdit,
		///<summary>157 - Logs changes made to individual proc codes(excluding fee changes) including when run from proc code tools</summary>
		ProcCodeEdit,
		///<summary>158 - Allows users with this permission the ability to add new users. Security admins have this by default.</summary>
		[Description("Add New User")]
		AddNewUser,
		///<summary>159 - Allows users with this permission the ability to view claims.</summary>
		[Description("Claim View")]
		ClaimView,
		///<summary>160 - Allows users to run the Repeat Charge Tool.</summary>
		[Description("Repeating Charge Tool")]
		RepeatChargeTool,
		///<summary>161 - Logs when a discount plan is added or dropped from a patient.</summary>
		DiscountPlanAddDrop,
		///<summary>162 - Allows users with this permission the ability to sign treatment plans.</summary>
		[Description("Sign Treatment Plan")]
		TreatPlanSign,
		///<summary>163 - Allows users with this permission to edit an existing EO or EC procedure.</summary>
		[Description("Edit EO or EC Procedures")]
		ProcExistingEdit,
	}

	
}














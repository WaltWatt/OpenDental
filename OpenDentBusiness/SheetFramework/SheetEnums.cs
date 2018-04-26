﻿
namespace OpenDentBusiness {
	///<Summary>Different types of sheets that can be used.</Summary>
	public enum SheetTypeEnum{
		///<Summary>0-Requires SheetParameter for PatNum. Does not get saved to db.</Summary>
		LabelPatient,
		///<Summary>1-Requires SheetParameter for CarrierNum. Does not get saved to db.</Summary>
		LabelCarrier,
		///<Summary>2-Requires SheetParameter for ReferralNum. Does not get saved to db.</Summary>
		LabelReferral,
		///<Summary>3-Requires SheetParameters for PatNum,ReferralNum.</Summary>
		ReferralSlip,
		///<Summary>4-Requires SheetParameter for AptNum. Does not get saved to db.</Summary>
		LabelAppointment,
		///<Summary>5-Requires SheetParameter for RxNum.</Summary>
		Rx,
		///<summary>6-Requires SheetParameter for PatNum.</summary>
		Consent,
		///<summary>7-Requires SheetParameter for PatNum.</summary>
		PatientLetter,
		///<summary>8-Requires SheetParameters for PatNum,ReferralNum.</summary>
		ReferralLetter,
		///<Summary>9-Requires SheetParameter for PatNum.</Summary>
		PatientForm,
		///<Summary>10-Requires SheetParameter for AptNum.  Does not get saved to db.</Summary>
		RoutingSlip,
		///<Summary>11-Requires SheetParameter for PatNum.</Summary>
		MedicalHistory,
		///<Summary>12-Requires SheetParameter for PatNum, LabCaseNum.</Summary>
		LabSlip,
		///<Summary>13-Requires SheetParameter for PatNum.</Summary>
		ExamSheet,
		///<summary>14-Requires SheetParameter for DepositNum.</summary>
		DepositSlip,
		///<summary>15-Requires SheetParameter for PatNum.</summary>
		Statement,
		///<summary>16-Requires SheetParameters for PatNum,MedLab,MedLabResult.</summary>
		MedLabResults,
		///<summary>17-Requires SheetParameters for PatNum,TreatmentPlan.</summary>
		TreatmentPlan,
		///<summary>18-Requires SheetParameter for ScreenNum.  
		///Optional SheetParameter for PatNum if screening is associated to a patient.</summary>
		Screening,
		///<summary>19-Used for Payment Plans to Sheets.</summary>
		PaymentPlan,
		///<summary>20-Requires SheetParameters for ListRxSheet and ListRxNums.</summary>
		RxMulti,
		/*StatementHeader,
		TxPlanHeader,
		Postcard*/
		ERA,
		ERAGridHeader,
	}

	///<summary>For sheetFields</summary>
	public enum GrowthBehaviorEnum {
		///<Summary>Not allowed to grow.  Max size would be Height and Width.</Summary>
		None,
		///<Summary>Can grow down if needed, and will push nearby objects out of the way so that there is no overlap.</Summary>
		DownLocal,
		///<Summary>Can grow down, and will push down all objects on the sheet that are below it.  Mostly used when drawing grids.</Summary>
		DownGlobal
	}

	///<summary></summary>
	public enum SheetFieldType {
		///<Summary>0-Pulled from the database to be printed on the sheet.  Or also possibly just generated at runtime even though not pulled from the database.   User still allowed to change the output text as they are filling out the sheet so that it can different from what was initially generated.</Summary>
		OutputText,
		///<Summary>1-A blank box that the user is supposed to fill in.</Summary>
		InputField,
		///<Summary>2-This is text that is defined as part of the sheet and will never change from sheet to sheet.  </Summary>
		StaticText,
		///<summary>3-Stores a parameter other than the PatNum.  Not meant to be seen on the sheet.  Only used for SheetField, not SheetFieldDef.</summary>
		Parameter,
		///<Summary>4-Any image of any size, typically a background image for a form.</Summary>
		Image,
		///<summary>5-One sequence of dots that makes a line.  Continuous without any breaks.  Each time the pen is picked up, it creates a new field row in the database.</summary>
		Drawing,
		///<Summary>6-A simple line drawn from x,y to x+width,y+height.  So for these types, we must allow width and height to be negative or zero.</Summary>
		Line,
		///<Summary>7-A simple rectangle outline.</Summary>
		Rectangle,
		///<summary>8-A clickable area on the screen.  It's a form of input, so treated similarly to an InputField.  The X will go from corner to corner of the rectangle specified.  It can also behave like a radio button</summary>
		CheckBox,
		///<summary>9-A signature box, either Topaz pad or directly on the screen with stylus/mouse.  The signature is encrypted based an a hash of all other field values in the entire sheet, excluding other SigBoxes.  The order is critical.</summary>
		SigBox,
		///<Summary>10-An image specific to one patient.</Summary>
		PatImage,
		///<Summary>11-Special: Currently only used for Toothgrid</Summary>
		Special,
		///<summary>12-Grid: Placable grids similar to ODGrids. Used primarily in statements.</summary>
		Grid,
		///<summary>13-ComboBox: Placeable combo box for selecting filled options.</summary>
		ComboBox,
		///<summary>14-ScreenChart: A tooth chart that is desiged for screenings.</summary>
		ScreenChart
		//<summary></summary>
		//RadioButton
		
		//<Summary>Not yet supported.  This might be redundant, and we might use border element instead as the preferred way of drawing a box.</Summary>
		//Box
	}

	public enum SheetInternalType{
		LabelPatientMail,
		LabelPatientLFAddress,
		LabelPatientLFChartNumber,
		LabelPatientLFPatNum,
		LabelPatientRadiograph,
		LabelText,
		LabelCarrier,
		LabelReferral,
		ReferralSlip,
		LabelAppointment,
		Rx,
		Consent,
		PatientLetter,
		PatientLetterTxFinder,
		ReferralLetter,
		RoutingSlip,
		PatientRegistration,
		FinancialAgreement,
		HIPAA,
		MedicalHistSimple,
		MedicalHistNewPat,
		MedicalHistUpdate,
		LabSlip,
		ExamSheet,
		DepositSlip,
		Statement,
		///<summary>Users are NEVER allowed to use this sheet type. It is for internal use only. It should be hidden in all lists and unselectable.</summary>
		MedLabResults,
		TreatmentPlan,
		Screening,
		PaymentPlan,
		RxMulti,
		ERA,
		ERAGridHeader,
	}

	public enum OutInCheck{
		Out,
		In,
		Check
	}

}

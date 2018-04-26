using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OpenDentBusiness {
	///<summary>The info in the definition table is used by other tables extensively.  Almost every table in the database links to definition.  
	///Almost all links to this table will be to a DefNum.  Using the DefNum, you can find any of the other fields of interest, usually the ItemName.  
	///Make sure to look at the Defs class to see how the definitions are used.  Loaded into memory ahead of time for speed.  Some types of info such as 
	///operatories started out life in this table, but then got moved to their own table when more complexity was needed.</summary>
	[Serializable]
	[CrudTable(TableName="definition")]
	public class Def:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long DefNum;
		///<summary>Enum:DefCat</summary>
		public DefCat Category;
		///<summary>Order that each item shows on various lists. 0-indexed.</summary>
		public int ItemOrder;
		///<summary>Each category is a little different.  This field is usually the common name of the item.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.CleanText)]
		public string ItemName;
		///<summary>This field can be used to store extra info about the item.</summary>
		public string ItemValue;
		///<summary>Some categories include a color option.</summary>
		[XmlIgnore]
		public Color ItemColor;
		///<summary>If hidden, the item will not show on any list, but can still be referenced.</summary>
		public bool IsHidden;

		///<summary>Used only for serialization purposes</summary>
		[XmlElement("ItemColor",typeof(int))]
		public int ItemColorXml {
			get {
				return ItemColor.ToArgb();
			}
			set {
				ItemColor = Color.FromArgb(value);
			}
		}

		///<summary>Returns a copy of the def.</summary>
		public Def Copy() {
			return (Def)MemberwiseClone();
		}

		/*
		public Def(){

		}

		public Def(long defNum,DefCat category,int itemOrder,string itemName,string itemValue,Color itemColor,bool isHidden){
			DefNum=defNum;
			Category=category;
			ItemOrder=itemOrder;
			ItemName=itemName;
			ItemValue=itemValue;
			ItemColor=itemColor;
			IsHidden=isHidden;
		}*/

	}

	///<summary>Definition Category. Go to the definition setup window in the program to see how each of these categories is used.
	/// If you add a category, make sure to add it to the switch statement of FormDefinitions so the user can edit it.
	/// Add a "NotUsed" description attribute to defs that shouldn't show up in FormDefinitions.</summary>
	public enum DefCat {
		///<summary>0- Colors to display in Account module.</summary>
		[Description("Account Colors")]
		AccountColors,
		///<summary>1- Adjustment types.</summary>
		[Description("Adj Types")]
		AdjTypes,
		///<summary>2- Appointment confirmed types.</summary>
		[Description("Appt Confirmed")]
		ApptConfirmed,
		///<summary>3- Procedure quick add list for appointments.</summary>
		[Description("Appt Procs Quick Add")]
		ApptProcsQuickAdd,
		///<summary>4- Billing types.</summary>
		[Description("Billing Types")]
		BillingTypes,
		///<summary>5- Not used.</summary>
		[Description("NotUsed")]
		ClaimFormats,
		///<summary>6- Not used.</summary>
		[Description("NotUsed")]
		DunningMessages,
		///<summary>7- Not used.</summary>
		[Description("NotUsed")]
		FeeSchedNamesOld,
		///<summary>8- Not used.</summary>
		[Description("NotUsed")]
		MedicalNotes,
		///<summary>9- Not used.</summary>
		[Description("NotUsed")]
		OperatoriesOld,
		///<summary>10- Payment types.</summary>
		[Description("Payment Types")]
		PaymentTypes,
		///<summary>11- Procedure code categories.</summary>
		[Description("Proc Code Categories")]
		ProcCodeCats,
		///<summary>12- Progress note colors.</summary>
		[Description("Prog Notes Colors")]
		ProgNoteColors,
		///<summary>13- Statuses for recall, unscheduled, and next appointments.</summary>
		[Description("Recall/Unsched Status")]
		RecallUnschedStatus,
		///<summary>14- Not used.</summary>
		[Description("NotUsed")]
		ServiceNotes,
		///<summary>15- Discount types.</summary>
		[Description("NotUsed")]
		DiscountTypes,
		///<summary>16- Diagnosis types.</summary>
		[Description("Diagnosis Types")]
		Diagnosis,
		///<summary>17- Colors to display in the Appointments module.</summary>
		[Description("Appointment Colors")]
		AppointmentColors,
		///<summary>18- Image categories.</summary>
		[Description("Image Categories")]
		ImageCats,
		///<summary>19- Not used.</summary>
		[Description("NotUsed")]
		ApptPhoneNotes,
		///<summary>20- Treatment plan priority names.</summary>
		[Description("Treat' Plan Priorities")]
		TxPriorities,
		///<summary>21- Miscellaneous color options.</summary>
		[Description("Misc Colors")]
		MiscColors,
		///<summary>22- Colors for the graphical tooth chart.</summary>
		[Description("Chart Graphic Colors")]
		ChartGraphicColors,
		///<summary>23- Categories for the Contact list.</summary>
		[Description("Contact Categories")]
		ContactCategories,
		///<summary>24- Categories for Letter Merge.</summary>
		[Description("Letter Merge Cats")]
		LetterMergeCats,
		///<summary>25- Types of Schedule Blockouts.</summary>
		[Description("Blockout Types")]
		BlockoutTypes,
		///<summary>26- Categories of procedure buttons in Chart module</summary>
		[Description("Proc Button Categories")]
		ProcButtonCats,
		///<Summary>27- Types of commlog entries.</Summary>
		[Description("Commlog Types")]
		CommLogTypes,
		///<summary>28- Categories of Supplies</summary>
		[Description("Supply Categories")]
		SupplyCats,
		///<summary>29- Types of unearned income used in accrual accounting.</summary>
		[Description("PaySplit Unearned Types")]
		PaySplitUnearnedType,
		///<summary>30- Prognosis types.</summary>
		[Description("Prognosis")]
		Prognosis,
		///<summary>31- Custom Tracking, statuses such as 'review', 'hold', 'riskmanage', etc.</summary>
		[Description("Claim Custom Tracking")]
		ClaimCustomTracking,
		///<summary>32- PayType for claims such as 'Check', 'EFT', etc.</summary>
		[Description("Insurance Payment Types")]
		InsurancePaymentType,
		///<summary>33- Categories of priorities for tasks.</summary>
		[Description("Task Priorities")]
		TaskPriorities,
		///<summary>34- Categories for fee override colors.</summary>
		[Description("Fee Colors")]
		FeeColors,
		///<summary>35- Provider specialties.  General, Hygienist, Pediatric, Primary Care Physician, etc.</summary>
		[Description("Provider Specialties")]
		ProviderSpecialties,
		///<summary>36- Reason why a claim proc was rejected. This must be set on each individual claim proc.</summary>
		[Description("Claim Payment Tracking")]
		ClaimPaymentTracking,
		///<summary>37- Procedure quick charge list for patient accounts.</summary>
		[Description("Account Procs Quick Add")]
		AccountQuickCharge,
		///<summary>38- Insurance verification status such as 'Verified', 'Unverified', 'Pending Verification'.</summary>
		[Description("Insurance Verification Status")]
		InsuranceVerificationStatus,
		///<summary>39- Regions that clinics can be assigned to.</summary>
		[Description("Regions")]
		Regions,
		///<summary>40- ClaimPayment Payment Groups.</summary>
		[Description("Claim Payment Groups")]
		ClaimPaymentGroups,
		///<summary>41 - Auto Note Categories.  Used to categorize autonotes into custom categories.</summary>
		[Description("Auto Note Categories")]
		AutoNoteCats,
		///<summary>42 - Web Sched New Patient Appointment Types.  Displays in Web Sched.  Selected type shows in appointment note.</summary>
		[Description("Web Sched New Pat Appt Types")]
		WebSchedNewPatApptTypes,
		///<summary>43 - Custom Claim Status Error Code.</summary>
		[Description("Claim Error Code")]
		ClaimErrorCode,
		///<summary>44 - Specialties that clinics perform.  Useful for separating patient clones across clinics.</summary>
		[Description("Clinic Specialties")]
		ClinicSpecialty,
		///<summary>45 - HQ Only job priorities.</summary>
		[Description("Job Priorities HqOnly")]
		JobPriorities,
		///<summary>46 - Carrier Group Name.</summary>
		[Description("Carrier Group Names")]
		CarrierGroupNames,
		///<summary>47 - PayPlanCategory</summary>
		[Description("Payment Plan Categories")]
		PayPlanCategories,
		///<summary>48 - Associates an insurance payment to an account number.  Currently only used with "Auto Deposits".</summary>
		[Description("Auto Deposit Account")]
		AutoDeposit,
	}

	public class DefCatOptions {
		public DefCat DefCat;
		public bool CanEditName;
		public bool EnableValue;
		public bool EnableColor;
		///<summary>This is the text that will show up in the Guidelines section of the Definitions window.</summary>
		public string HelpText;
		public bool CanDelete;
		public bool CanHide;
		public string ValueText;
		public bool IsValueDefNum;
		public bool DoShowItemOrderInValue;

		public DefCatOptions(DefCat defCat,bool canDelete=false,bool canEditName=true,bool canHide=true,bool enableColor=false,bool enableValue=false,
			bool isValidDefNum=false) 
		{
			DefCat=defCat;
			CanDelete=canDelete;
			CanEditName=canEditName;
			CanHide=canHide;
			EnableColor=enableColor;
			EnableValue=enableValue;
			IsValueDefNum=isValidDefNum;
		}
	}





}

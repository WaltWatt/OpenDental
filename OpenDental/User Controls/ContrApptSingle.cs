/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.UI;
using System.Linq;
using CodeBase;

namespace OpenDental {

	///<summary></summary>
	public class ContrApptSingle:System.Windows.Forms.UserControl {
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary>Set on mouse down or from Appt module</summary>
		public static long ClickedAptNum;
		/// <summary>This is not the best place for this, but changing it now would cause bugs.  Set manually</summary>
		public static long SelectedAptNum;
		///<summary>True if this control is on the pinboard</summary>
		public bool ThisIsPinBoard;
		/////<summary>Stores the shading info for the provider bars on the left of the appointments module</summary>
		//public static int[][] ProvBar;
		///<summary>Stores the background bitmap for this control</summary>
		private Bitmap _shadow;
		private Font baseFont=new Font("Arial",8);
		private Font boldFont=new Font("Arial",8,FontStyle.Bold);
		///<summary>The db version of the pattern. Will be converted to PatternShowing befoe being draw to the screen.</summary>
		public string Pattern { get; private set; }
		///<summary>The actual slashes and Xs showing for the current view.</summary>
		public string PatternShowing { get; private set; }
		///<summary>This is a datarow that stores most of the info necessary to draw appt.  It comes from the table obtained in Appointments.GetPeriodApptsTable().</summary>
		public DataRow DataRoww;
		///<summary>This table contains all appointment fields for all appointments in the period. It's obtained in Appointments.GetApptFields().</summary>
		public DataTable TableApptFields;
		///<summary>This table contains all appointment fields for all appointments in the period. It's obtained in Appointments.GetApptFields().</summary>
		public DataTable TablePatFields;
		///<summary>Indicator that account has procedures with no ins claim</summary>
		public bool FamHasInsNotSent;
		///<Summary>Will show the highlight around the edges.  For now, this is only used for pinboard.  The ordinary selected appt is set with SelectedAptNum.</Summary>
		public bool IsSelected;
		///<summary>Shortcut for PIn.Long(DataRoww["AptNum"].ToString()).</summary>
		public long AptNum { get; private set; }
		///<summary>Shortcut for PIn.Long(DataRoww["PatNum"].ToString()).</summary>
		public long PatNum { get; private set; }
		///<summary>Shortcut for PIn.Long(DataRoww["ClinicNum"].ToString()).</summary>
		public long ClinicNum { get; private set; }
		///<summary>Shortcut for PIn.Long(DataRoww["Op"].ToString()).</summary>
		public long OpNum { get; private set; }
		///<summary>Shortcut for PIn.Long(DataRoww["ProvNum"].ToString()).</summary>
		public long ProvNum { get; private set; }
		///<summary>Shortcut for PIn.Long(DataRoww["ProvHyg"].ToString()).</summary>
		public long ProvHyg { get; private set; }
		///<summary>Shortcut for PIn.Long(DataRoww["Confirmed"].ToString()).</summary>
		public long Confirmed { get; private set; }
		///<summary>Shortcut for PIn.DateT(DataRoww["AptDateTime"].ToString()).</summary>
		public DateTime AptDateTime { get; private set; }
		///<summary>Shortcut for PIn.Bool(DataRoww["IsHygiene"].ToString()).</summary>
		public bool IsHygiene { get; private set; }
		///<summary>Shortcut for Enum.TryParse(PIn.String(DataRoww["AptStatus"].ToString()).</summary>
		public ApptStatus AptStatus { get; private set; }
		///<summary>Shortcut for PIn.Decimal(DataRoww["productionVal"].ToString()).</summary>
		public decimal GrossProduction { get; private set; }
		///<summary>Shortcut for PIn.Decimal(DataRoww["writeoffPPO"].ToString()).</summary>
		public decimal WriteoffPPO { get; private set; }
		///<summary>Shortcut for PIn.Decimal(DataRoww["adjustmentTotal"].ToString()).</summary>
		public decimal AdjustmentTotal { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["ImageFolder"].ToString()).</summary>
		public string ImageFolder { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["patientName"].ToString()).</summary>
		public string PatientName { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["aptDay"].ToString()).</summary>
		public string AptDay { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["aptDate"].ToString()).</summary>
		public string AptDate { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["aptTime"].ToString()).</summary>
		public string AptTime { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["aptLength"].ToString()).</summary>
		public string AptLength { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["Email"].ToString()).</summary>
		public string Email { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["language"].ToString()).</summary>
		public string Language { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["referralTo"].ToString()).</summary>
		public string ReferralTo { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["referralFrom"].ToString()).</summary>
		public string ReferralFrom { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["apptModNote"].ToString()).</summary>
		public string ApptModNote { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["famFinUrgNote"].ToString()).</summary>
		public string FamFinUrgNote { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["addrNote"].ToString()).</summary>
		public string AddrNote { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["insurance"].ToString()).</summary>
		public string Insurance { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["contactMethods"].ToString()).</summary>
		public string ContactMethods { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["wirelessPhone"].ToString()).</summary>
		public string WirelessPhone { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["wkPhone"].ToString()).</summary>
		public string WkPhone { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["hmPhone"].ToString()).</summary>
		public string HmPhone { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["age"].ToString()). Can be '?'.</summary>
		public string Age { get; private set; }
		///<summary>Shortcut for PIn.String(Defs.GetName(DefCat.BillingTypes,PIn.Long(rowRaw["patBillingType"].ToString()))).</summary>
		public string BillingType { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["chartNumber"].ToString()). From patient.ChartNumber.</summary>
		public string ChartNumber { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["Note"].ToString()). From appointment.Note.</summary>
		public string Note { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["procs"].ToString()). From appointment.ProcDescript.</summary>
		public string Procs { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["lab"].ToString()). Will vary depending on labcaseXXX date/times.</summary>
		public string Lab { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["MedUrgNote"].ToString()). Frompatient.MedUrgNote.</summary>
		public string MedUrgNote { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["preMedFlag"].ToString()). Will be 'Premedicate' or ''.</summary>
		public string PreMedFlag { get; private set; }
		///<summary>Shortcut for PIn.String(Defs.GetName(DefCat.ApptConfirmed,PIn.Long(rowRaw["apptConfirmed"].ToString()))).</summary>
		public string ConfirmedFromDef { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["production"].ToString()). Includes "c" string format.</summary>
		public string Production { get; private set; }
		///<summary>Shortcut for PIn.String(DataRoww["provider"].ToString()).</summary>
		public string Provider { get; private set; }
		///<summary>Shortcut for PIn.Long(DataRoww["Priority"].ToString()).</summary>
		public ApptPriority Priority { get; private set; }
		/// <summary>Used to restrict access to the _shadow object.</summary>
		private object _lockObject=new object();
		///<summary>Stores the background bitmap for this control.  Get returns deep a copy.</summary>
		public Bitmap Shadow {
			get {
				//When the appointment grid was being drawn, multiple threads would sometimes try and access
				//the shadow while it was in use.  This prevents that exception by locking the image when it requested.
				lock(_lockObject) {
					if(_shadow==null) {
						return null;
					}
					return new Bitmap(_shadow);//This constructor creates a deep copy.
				}
			}
			set {
				Bitmap shadowOld=null;
				Bitmap shadowNew=new Bitmap(value);//Creates a deep copy of the "value"
				lock(_lockObject) {
					shadowOld=this._shadow;
					_shadow=shadowNew;
				}
				//Bitmaps can take a lot of space, so we immediately mark the image for the garbage collector.
				if(shadowOld!=null) {
					shadowOld.Dispose();
					shadowOld=null;
				}
			}
		}
		public bool IsShadowValid {
			get {
				return _shadow!=null;
			}
		}
#if DEBUG
		public string LoggerString {
			get {
				var d=DataRoww.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToDictionary(x => x,x => DataRoww[x]);
				string ret="";
				foreach(var item in d) {
					ret+=item.Key+","+item.Value.ToString()+"\r\n";
				}
				return ret;
			}
		}
#endif
		public ContrApptSingle() {
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
			//Visible is used sparingly for this control so we will default it to false.
			Visible=false;
		}

		///<summary>Visible=false by default. This control is typically drawn using the double buffer bitmap generated via CreateShadow().
		///For this reason, Visible=false is common here.</summary>
		public ContrApptSingle(DataRow rowApt,DataTable tableApptFields,DataTable tablePatFields,Point location) : this() {
			ResetData(rowApt,tableApptFields,tablePatFields,location);			
		}

		///<summary>Set all fields of this control. This is an alternative to constructing an entirely new instance. 
		///All uses of this control currently construct as Visible=false so that is the default here.</summary>
		public void ResetData(DataRow rowApt,DataTable tableApptFields,DataTable tablePatFields,Point location,bool visible=false) {
			DataRoww=rowApt;
			TableApptFields=tableApptFields;
			TablePatFields=tablePatFields;
			Pattern=PIn.String(DataRoww["Pattern"].ToString());
			PatternShowing=ApptSingleDrawing.GetPatternShowing(Pattern);
			Location=location;
			Size=ApptSingleDrawing.SetSize(Pattern);
			//These controls are always drawn as their Shadow bitmap. 
			//They never actually render as a control. 
			//Always set Visible to false here so that parent panel/form doesn't account for them when drawings it's controls.
			//In the case where it is a draggable control or on the pinboard, then it will be set to Visible=true.
			Visible=visible;
			//These are used heavily so deserialize here once to save time when accessing.
			AptNum=PIn.Long(DataRoww["AptNum"].ToString());
			PatNum=PIn.Long(DataRoww["PatNum"].ToString());
			AptDateTime=PIn.DateT(DataRoww["AptDateTime"].ToString());
			OpNum=PIn.Long(DataRoww["Op"].ToString());
			ClinicNum=PIn.Long(DataRoww["ClinicNum"].ToString());
			ProvNum=PIn.Long(DataRoww["ProvNum"].ToString());
			ProvHyg=PIn.Long(DataRoww["ProvHyg"].ToString());
			Confirmed=PIn.Long(DataRoww["Confirmed"].ToString());
			IsHygiene=PIn.Bool(DataRoww["IsHygiene"].ToString());
			GrossProduction=PIn.Decimal(DataRoww["productionVal"].ToString());
			WriteoffPPO=PIn.Decimal(DataRoww["writeoffPPO"].ToString());
			AdjustmentTotal=PIn.Decimal(DataRoww["adjustmentTotal"].ToString());
			ImageFolder=PIn.String(DataRoww["ImageFolder"].ToString());
			PatientName=PIn.String(DataRoww["patientName"].ToString());
			AptDate=PIn.String(DataRoww["aptDate"].ToString());
			AptDay=PIn.String(DataRoww["aptDay"].ToString());
			AptLength=PIn.String(DataRoww["aptLength"].ToString());
			AptTime=PIn.String(DataRoww["aptTime"].ToString());
			Email=PIn.String(DataRoww["Email"].ToString());
			Language=PIn.String(DataRoww["language"].ToString());
			ReferralTo=PIn.String(DataRoww["referralTo"].ToString());
			ReferralFrom=PIn.String(DataRoww["referralFrom"].ToString());
			ApptModNote=PIn.String(DataRoww["apptModNote"].ToString());
			FamFinUrgNote=PIn.String(DataRoww["famFinUrgNote"].ToString());
			AddrNote=PIn.String(DataRoww["addrNote"].ToString());
			Insurance=PIn.String(DataRoww["insurance"].ToString());
			ContactMethods=PIn.String(DataRoww["contactMethods"].ToString());
			WirelessPhone=PIn.String(DataRoww["wirelessPhone"].ToString());
			WkPhone=PIn.String(DataRoww["wkPhone"].ToString());
			HmPhone=PIn.String(DataRoww["hmPhone"].ToString());
			Age=PIn.String(DataRoww["age"].ToString());
			BillingType=PIn.String(DataRoww["billingType"].ToString());
			ChartNumber=PIn.String(DataRoww["chartNumber"].ToString());
			Note=PIn.String(DataRoww["Note"].ToString());
			Procs=PIn.String(DataRoww["procs"].ToString());
			Lab=PIn.String(DataRoww["lab"].ToString());
			MedUrgNote=PIn.String(DataRoww["MedUrgNote"].ToString());
			PreMedFlag=PIn.String(DataRoww["preMedFlag"].ToString());
			ConfirmedFromDef=PIn.String(DataRoww["confirmed"].ToString());
			Production=PIn.String(DataRoww["production"].ToString());
			Provider=PIn.String(DataRoww["provider"].ToString());	
			ApptStatus aptStatus;
			if(Enum.TryParse(PIn.String(DataRoww["AptStatus"].ToString()),out aptStatus)) {
				AptStatus=aptStatus;
			}
			else {
				AptStatus=ApptStatus.None;
			}
			ApptPriority priority;
			if(Enum.TryParse(PIn.String(DataRoww["Priority"].ToString()),out priority)) {
				Priority=priority;
			}
			else {
				Priority=ApptPriority.Normal;
			}
		}

		///<summary></summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
				DisposeShadow();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent() {
			// 
			// ContrApptSingle
			// 
			this.Name = "ContrApptSingle";
			this.Size = new System.Drawing.Size(85,72);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContrApptSingle_MouseDown);

		}
		#endregion

		///<summary>Do nothing. This control will get drawn as double buffered bitmap from ContrApptSheet.DoubleBufferDraw().</summary>
		protected override void OnPaint(PaintEventArgs pea) {
		}

		///<summary>CreateShadow modifies this.BackgroundImage at the same time that it may be modifying Shadow. This causes OnPaintBackground to be called async, this is a feature of GDI drawing.
		///There is an opportunity for a race condition which would cause OnPaintBackground to throw an otherwise unhandled exception. This is harmless so catch and swallow the exception here.</summary>
		protected override void OnPaintBackground(PaintEventArgs e) {
			try {
				base.OnPaintBackground(e);
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
		}

		///<summary>It is planned to move some of this logic to OnPaint and use a true double buffer.</summary>
		public void CreateShadow() {
			if(this.Parent is ContrApptSheet) {
				bool isVisible=ApptDrawing.GetIndexOp(OpNum)>=0;
				if(!isVisible) {
					return;
				}
			}
			DisposeShadow();
			if(Width<4) {
				return;
			}
			if(Height<4) {
				return;
			}
			Bitmap shadowNew=new Bitmap(Width,Height);			
			using(Graphics g=Graphics.FromImage(shadowNew)) {
				ApptSingleDrawing.DrawEntireAppt(g,DataRoww,PatternShowing,Width,Height,IsSelected,ThisIsPinBoard,SelectedAptNum,
					ApptViewItemL.ApptRows,ApptViewItemL.ApptViewCur,TableApptFields,TablePatFields,8,false);				
			}
			//In most cases, drawing will be taken care of by ContrApptSheet.DoubleBufferDraw(). 
			//However, BackgroundImage must be set here to draw the bitmap when when dragging.
			//If this was inside the using block, a GDI+ exception would sometimes be thrown because the graphics object would still be accessing the 
			//image data when the image reference was changed.
			Shadow=shadowNew;
			BackgroundImage=shadowNew;
		}

		public void DisposeShadow() {
			if(_shadow!=null) {
				_shadow.Dispose();
				_shadow=null;
			}
		}

		private void ContrApptSingle_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			ClickedAptNum=AptNum;
		}
	}//end class
}//end namespace

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
using CodeBase;

namespace OpenDental.UI {
	
	public partial class ODDateRangePicker:UserControl {
		
		private bool _enableWeekButtons=true;
		private DateTime _defaultDateTimeFrom=new DateTime(DateTime.Today.Year,1,1);
		private DateTime _defaultDateTimeTo=DateTime.Today;
		///<summary>Event is fired when the speech is recognized.</summary>
		public event CalendarClosedHandler CalendarClosed=null;
		///<summary>Event is fired when either calendar has made a selection.</summary>
		public event CalendarSelectionHandler CalendarSelectionChanged=null;
		private bool _isParentChange;
		protected Point _locationOrigCalendarTo;
		protected Point _locationOrigCalendarFrom;
		///<summary>Hiding Control.Leave because the Leave event is fired whenever the user clicks on the calendar. This control will fire this Leave
		///event when the user truly leaves this control.</summary>
		public new event EventHandler Leave;

		#region Properties
		///<summary>Set true to enable butWeekPrevious and butWeekNext</summary>
		[Category("Behavior"), Description("Set true to enable butWeekPrevious and butWeekNext."), DefaultValue(true)]
		public bool EnableWeekButtons {
			get {
				return _enableWeekButtons;
			}
			set {
				_enableWeekButtons=value;
			}
		}

		///<summary>Set true to enable butWeekPrevious and butWeekNext</summary>
		[Category("Behavior"), Description("Set the initial from date for load.")]
		public DateTime DefaultDateTimeFrom {
			get {
				return _defaultDateTimeFrom;
			}
			set {
				_defaultDateTimeFrom=value;
			}
		}
		
		///<summary>Set true to enable butWeekPrevious and butWeekNext</summary>
		[Category("Behavior"), Description("Set the initial to date for load.")]
		public DateTime DefaultDateTimeTo {
			get {
				return _defaultDateTimeTo;
			}
			set {
				_defaultDateTimeTo=value;
			}
		}
		#endregion
		public ODDateRangePicker() {
			InitializeComponent();
			_locationOrigCalendarTo=calendarTo.Location;
			_locationOrigCalendarFrom=calendarFrom.Location;
			base.Leave += new System.EventHandler(this.ODDateRangePicker_Leave);
		}

		public DateTime GetDateTimeFrom() {
			return PIn.Date(textDateFrom.Text);
		}

		///<summary>Gets the end of the date that is entered.</summary>
		public DateTime GetDateTimeTo() {
			try {
				return DateTime.Parse(textDateTo.Text).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
			}
			catch(Exception ex) {
				ex.DoNothing();
				return DateTime.MinValue;
			}
		}

		public bool HasDateTimeError() {
				return (!textDateFrom.IsValid
					|| !textDateTo.IsValid);
		}

		public void SetDateTimeFrom(DateTime dateTime) {
			if(dateTime==DateTime.MinValue) {
				textDateFrom.Text="";
			}
			else {
				textDateFrom.Text=dateTime.ToShortDateString();
				if(dateTime>GetDateTimeTo()){
					textDateTo.Text=dateTime.ToShortDateString();//Setting 'From' in the future of 'To' auto updates 'To' to same date.
					calendarTo.SetDate(dateTime);
				}
			}
		}

		public void SetDateTimeTo(DateTime dateTime) {
			if(dateTime==DateTime.MinValue) {
				textDateTo.Text="";
			}
			else {
				textDateTo.Text=dateTime.ToShortDateString();
				if(dateTime<GetDateTimeFrom()){
					textDateFrom.Text=dateTime.ToShortDateString();//Setting 'To' in the past of 'From' auto updates 'From' to same date.
					calendarFrom.SetDate(dateTime);
				}
			}
		}
		
		///<summary>An empty string does not register as an error in ValidDate.</summary>
		public bool HasEmptyDateTimeFrom() {
			return (textDateFrom.Text=="");
		}

		///<summary>An empty string does not register as an error in ValidDate.</summary>
		public bool HasEmptyDateTimeTo() {
			return (textDateTo.Text=="");
		}

		private void ODDateRangePicker_Load(object sender,EventArgs e) {
			if(!_enableWeekButtons) {
				panelWeek.Visible=false;
			}
			textDateFrom.Text=_defaultDateTimeFrom.ToShortDateString();
			textDateTo.Text=_defaultDateTimeTo.ToShortDateString();
			HideCalendars();
		}
		
		protected virtual void butToggleCalendarFrom_Click(object sender,EventArgs e) {
			ToggleCalendars();//Both date textboxes should drop down both calendars.
		}

		protected virtual void butToggleCalendarTo_Click(object sender,EventArgs e) {
			ToggleCalendars();//Both date textboxes should drop down both calendars.
		}

		protected void ToggleCalendars() {
			ToggleCalendarTo();
			ToggleCalendarFrom();
		}

		protected void ToggleCalendarFrom() {
			if(calendarFrom.Visible) {
				HideCalendarFrom();
				CalendarClosed?.Invoke(this,new EventArgs());
				if(_isParentChange) {//Parent was not an ODForm, set back to original location and parent.
					calendarFrom.Location=_locationOrigCalendarFrom;
					calendarFrom.Parent=this;
				}
			}
			else {
				ShowCalendarFrom();
			}
		}

		protected void ToggleCalendarTo() {
			if(calendarTo.Visible) {
				HideCalendarTo();
				CalendarClosed?.Invoke(this,new EventArgs());
				if(_isParentChange) {//Parent was not an ODForm, set back to original location and parent.
					calendarTo.Location=_locationOrigCalendarTo;
					calendarTo.Parent=this;
				}
			}
			else {
				ShowCalendarTo();
			}
		}

		protected void HideCalendars() {
			HideCalendarFrom();	
			HideCalendarTo();
		}

		protected void HideCalendarFrom() {
			butDropFrom.ImageIndex=0;//Set to arrow down image.
			calendarFrom.Visible=false;
			panelCalendarGap.Visible=false;
			this.Height=this.MinimumSize.Height;	
		}
		
		protected void HideCalendarTo() {
			butDropTo.ImageIndex=0;//Set to arrow down image.
			calendarTo.Visible=false;
			panelCalendarGap.Visible=false;
			this.Height=this.MinimumSize.Height;	
		}

		private void ShowCalendarTo() {
			butDropTo.ImageIndex=1;//Set to arrow up image.
			//set the date on the calendars to match what's showing in the boxes
			if(!HasDateTimeError()) {
				if(textDateTo.Text=="") {
					//MonthCalendars have to have a selection, and Today's date seems to make the most sense,
					//even if implementors of this control interpret empty dates differently
					calendarTo.SetDate(DateTime.Today);
				}
				else {
					calendarTo.SetDate(PIn.Date(textDateTo.Text));
				}
			}
			if(!(this.Parent is ODForm)) {
				_isParentChange=true;
				Point locNew=calendarTo.Location;//Start from current context location.
				locNew=GetParentFormPoint(locNew,calendarTo);//Recursively work out to main form context.
				calendarTo.Location=locNew;//Set new location.
				calendarTo.Parent=Parent.FindForm();//Set new context.
				calendarTo.BringToFront();
			}
			//show the calendar
			calendarTo.Visible=true;
			this.Height=this.MaximumSize.Height;
		}

		private void ShowCalendarFrom() {
			butDropFrom.ImageIndex=1;//Set to arrow up image.
			//set the date on the calendars to match what's showing in the boxes
			if(!HasDateTimeError()) {
				if(textDateFrom.Text=="") {
					//MonthCalendars have to have a selection, and Today's date seems to make the most sense,
					//even if implementors of this control interpret empty dates differently
					calendarFrom.SetDate(DateTime.Today);
				}
				else {
					calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
				}
			}
			if(!(this.Parent is ODForm)) {
				_isParentChange=true;
				Point locNew=calendarFrom.Location;//Start from current context location.
				locNew=GetParentFormPoint(locNew,calendarFrom);//Recursively work out to main form context.
				calendarFrom.Location=locNew;//Set new location.
				calendarFrom.Parent=Parent.FindForm();//Set new context.
				calendarFrom.BringToFront();
			}
			//show the calendar
			calendarFrom.Visible=true;
			this.Height=this.MaximumSize.Height;
		}

		///<summary>Recursively calculates relative x-y coordinates up to this control's parent form.</summary>
		private Point GetParentFormPoint(Point locCur,Control contrCur) {
			if(contrCur.Parent==this.Parent.FindForm()) {
				return locCur;//Base case
			}
			locCur.Y+=contrCur.Parent.Location.Y;
			locCur.X+=contrCur.Parent.Location.X;
			return GetParentFormPoint(locCur,contrCur.Parent);
		}

		protected void calendarFrom_DateSelected(object sender,DateRangeEventArgs e) {
			SetDateTimeFrom(calendarFrom.SelectionStart);
			if(calendarFrom.Visible) {
				CalendarSelectionChanged?.Invoke(this,new EventArgs());
			}
		}

		protected void calendarTo_DateSelected(object sender,DateRangeEventArgs e) {
			SetDateTimeTo(calendarTo.SelectionStart);
			if(calendarTo.Visible) {
				CalendarSelectionChanged?.Invoke(this,new EventArgs());
			}
		}

		protected void butWeekPrevious_Click(object sender,EventArgs e) {
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateFrom!=DateTime.MinValue) {
				dateTo=dateFrom.AddDays(-1);
				textDateFrom.Text=dateTo.AddDays(-7).ToShortDateString();
				textDateTo.Text=dateTo.ToShortDateString();
			}
			else if(dateTo!=DateTime.MinValue) {//Invalid dateFrom but valid dateTo
				dateTo=dateTo.AddDays(-8);
				textDateFrom.Text=dateTo.AddDays(-7).ToShortDateString();
				textDateTo.Text=dateTo.ToShortDateString();
			}
			else {//Both dates invalid
				textDateFrom.Text=DateTime.Today.AddDays(-7).ToShortDateString();
				textDateTo.Text=DateTime.Today.ToShortDateString();
			}
			if(calendarFrom.Visible) { //textTo and textFrom are set above, so no check is necessary.
				calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
				calendarTo.SetDate(PIn.Date(textDateTo.Text));
			}
		}

		protected void butWeekNext_Click(object sender,EventArgs e) {
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateTo!=DateTime.MinValue) {
				dateFrom=dateTo.AddDays(1);
				textDateFrom.Text=dateFrom.ToShortDateString();
				textDateTo.Text=dateFrom.AddDays(7).ToShortDateString();
			}
			else if(dateFrom!=DateTime.MinValue) {//Invalid dateTo but valid dateFrom
				 dateFrom=dateFrom.AddDays(8);
				 textDateFrom.Text=dateFrom.ToShortDateString();
				 textDateTo.Text=dateFrom.AddDays(7).ToShortDateString();
			}
			else {//Both dates invalid
				textDateFrom.Text=DateTime.Today.ToShortDateString();
				textDateTo.Text=DateTime.Today.AddDays(7).ToShortDateString();
			}
			if(calendarFrom.Visible) { //textTo and textFrom are set above, so no check is necessary.
				calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
				calendarTo.SetDate(PIn.Date(textDateTo.Text));
			}
		}

		private void ODDateRangePicker_Leave(object sender,EventArgs e) {
			if(calendarFrom.Focused || calendarTo.Focused) {//Still using the calendar.
				this.Focus();//Spoof that we never left
				return;
			}
			if(calendarFrom.Visible||calendarTo.Visible) {
				HideCalendars();
				if(_isParentChange) {//Parent was not an ODForm, set back to original locaiton and parent.
					calendarTo.Location=_locationOrigCalendarTo;
					calendarFrom.Location=_locationOrigCalendarFrom;
					calendarTo.Parent=this;
					calendarFrom.Parent=this;
				}
				CalendarClosed?.Invoke(this,new EventArgs());
			}
			Leave?.Invoke(sender,e);
		}
	}

	public delegate void CalendarClosedHandler(object sender,EventArgs e);

	public delegate void CalendarSelectionHandler(object sender,EventArgs e);

}

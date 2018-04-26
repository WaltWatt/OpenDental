using System;

namespace OpenDental.UI {
	public partial class ODDateRangePickerVertical:ODDateRangePicker {
		public ODDateRangePickerVertical() {
			InitializeComponent();
			_locationOrigCalendarTo=calendarTo.Location;
			_locationOrigCalendarFrom=calendarFrom.Location;
		}

		protected override void butToggleCalendarFrom_Click(object sender,EventArgs e) {
			if(calendarTo.Visible) {//Only one calender allowed open at a time.
				ToggleCalendarTo();
			}
			ToggleCalendarFrom();
		}

		protected override void butToggleCalendarTo_Click(object sender,EventArgs e) {
			if(calendarFrom.Visible) {//Only one calender allowed open at a time.
				ToggleCalendarFrom();
			}
			ToggleCalendarTo();
		}
	}
}

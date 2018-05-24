using System;

namespace OpenDental {
	public partial class FormFriendlyException:ODForm {
		private string _friendlyMessage;
		private Exception _exception;
		private int _defaultDetailsHeight;

		public FormFriendlyException(string friendlyMessage,Exception ex) {
			InitializeComponent();
			Lan.F(this);
			_friendlyMessage=friendlyMessage;
			_exception=ex;
		}

		private void FormFriendlyException_Load(object sender,EventArgs e) {
			labelFriendlyMessage.Text=_friendlyMessage;
			textDetails.Text=_exception.GetType().Name+": "+_exception.Message+"\r\n"+_exception.StackTrace;
			_defaultDetailsHeight=textDetails.Height;
			//textDetails is visible by default so that it actually has height.
			ResizeDetails();//Invoke the ResizeDetails method so that the details are hidden when the window initially loads for the user.
		}

		private void labelDetails_Click(object sender,EventArgs e) {
			ResizeDetails();
		}

		///<summary>A helper method that toggles visibility of the details text box and adjusts the size of the form to accomodate the UI change.</summary>
		private void ResizeDetails() {
			if(textDetails.Visible) {
				textDetails.Visible=false;
				Height-=textDetails.Height;
			}
			else {
				textDetails.Visible=true;
				Height+=_defaultDetailsHeight;
				textDetails.Height=_defaultDetailsHeight;
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}

	public class FriendlyException {
		public static void Show(string friendlyMessage,Exception ex) {
			(new FormFriendlyException(friendlyMessage,ex)).ShowDialog();
		}
	}
}
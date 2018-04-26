using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormFriendlyException:ODForm {

		private string _friendlyMessage;
		private Exception _exception;

		public FormFriendlyException(string friendlyMessage,Exception ex) {
			InitializeComponent();
			Lan.F(this);
			_friendlyMessage=friendlyMessage;
			_exception=ex;
		}

		private void FormFriendlyException_Load(object sender,EventArgs e) {
			labelFriendlyMessage.Text=_friendlyMessage;
		}

		private void labelDetails_Click(object sender,EventArgs e) {
			textDetails.Text=_exception.GetType().Name+": "+_exception.Message+"\r\n"+_exception.StackTrace;
			ResizeDetails(true);
			textDetails.Visible=true;
		}

		private void ResizeDetails(bool doResizeForm) {
			using(Graphics g=textDetails.CreateGraphics()) {
				SizeF messageSize=g.MeasureString(textDetails.Text,textDetails.Font,textDetails.Width,new StringFormat(0));
				textDetails.Height=(int)messageSize.Height;
				if(doResizeForm) {
					Height=textDetails.Height+140;//Plus 140 to make room for the Close button
				}
			}
		}

		private void FormFriendlyException_Resize(object sender,EventArgs e) {
			ResizeDetails(false);
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
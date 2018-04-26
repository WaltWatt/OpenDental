using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>This is an MVC version of our BasicTemplate form.  This is a convenient "package" of classes to copy and paste when creating new forms.
	///This template takes care of setting many default settings that all Open Dental forms need to adhere to.  E.g. maximum size, icon, etc.</summary>
	public partial class BasicTemplateView : BaseBasicTemplateView {

		public BasicTemplateView() {
			InitializeComponent();
			Lan.F(this);
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}

	///<summary>Required so that Visual Studio can design this form.  The designer does not allow directly extending classes with generics.</summary>
	public class BaseBasicTemplateView : ODFormMVC<BasicTemplateModel,BasicTemplateView,BasicTemplateController> { }
}
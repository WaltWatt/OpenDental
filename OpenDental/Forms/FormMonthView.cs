using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormMonthView:ODForm {
		public FormMonthView() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMonthView_Load(object sender,EventArgs e) {

		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}

		
	}
}
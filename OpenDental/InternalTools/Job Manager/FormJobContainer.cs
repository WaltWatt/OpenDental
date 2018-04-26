﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormJobContainer:ODForm {
		public FormJobContainer(Control control,string title) {
			InitializeComponent();
			this.Controls.Add(control);
			this.Width=control.Width+10;
			this.Height=control.Height+10;
			control.Anchor=(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);//Allows resizing the control in this window.
			this.Text=title;
		}
	}
}

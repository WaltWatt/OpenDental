using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormMessagingButSetup : ODForm {
		private OpenDental.UI.Button butCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butDown;
		private Label label1;
		private ListBox listButtons;
		private ListBox listComputers;
		private Label label2;
		private SigButDef[] _arraySigButDefs;
		private List<Computer> _listComputers;

		///<summary></summary>
		public FormMessagingButSetup()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessagingButSetup));
			this.label1 = new System.Windows.Forms.Label();
			this.listButtons = new System.Windows.Forms.ListBox();
			this.listComputers = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(274,26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78,16);
			this.label1.TabIndex = 11;
			this.label1.Text = "Buttons";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listButtons
			// 
			this.listButtons.FormattingEnabled = true;
			this.listButtons.Location = new System.Drawing.Point(276,45);
			this.listButtons.Name = "listButtons";
			this.listButtons.Size = new System.Drawing.Size(200,264);
			this.listButtons.TabIndex = 12;
			this.listButtons.DoubleClick += new System.EventHandler(this.listButtons_DoubleClick);
			// 
			// listComputers
			// 
			this.listComputers.FormattingEnabled = true;
			this.listComputers.Location = new System.Drawing.Point(36,45);
			this.listComputers.Name = "listComputers";
			this.listComputers.Size = new System.Drawing.Size(198,264);
			this.listComputers.TabIndex = 23;
			this.listComputers.Click += new System.EventHandler(this.listComputers_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(34,26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(78,16);
			this.label2.TabIndex = 22;
			this.label2.Text = "Computer";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(401,332);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(75,26);
			this.butDown.TabIndex = 21;
			this.butDown.Text = "Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(276,332);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(75,26);
			this.butUp.TabIndex = 20;
			this.butUp.Text = "Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(545,332);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormMessagingButSetup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(654,383);
			this.Controls.Add(this.listComputers);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.listButtons);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMessagingButSetup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Messaging Button Setup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMessagingButSetup_FormClosing);
			this.Load += new System.EventHandler(this.FormMessagingButSetup_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormMessagingButSetup_Load(object sender,EventArgs e) {
			_listComputers=Computers.GetDeepCopy();
			listComputers.Items.Clear();
			listComputers.Items.Add(Lan.g(this,"All"));
			string s;
			for(int i=0;i<_listComputers.Count;i++) {
				s=_listComputers[i].CompName;
				if(SystemInformation.ComputerName==_listComputers[i].CompName){
					s+=" "+Lan.g(this,"(this computer)");
				}
				listComputers.Items.Add(s);
			}
			listComputers.SelectedIndex=0;
			_arraySigButDefs=SigButDefs.GetByComputer("");
			FillList();
		}

		private void FillList() {
			if(listComputers.SelectedIndex==-1) {//although I don't know how this could happen
				listComputers.SelectedIndex=0;
			}
			int selected=listButtons.SelectedIndex;
			listButtons.Items.Clear();
			SigButDef sigButDef;
			List<SigElementDef> listSigElementDefs;
			string buttonText;
			for(int i=0;i<40;i++) {
				sigButDef=SigButDefs.GetByIndex(i,_arraySigButDefs);
				if(sigButDef==null) {
					listButtons.Items.Add("-"+(i+1).ToString()+"-");
				}
				else {
					buttonText=sigButDef.ButtonText;
					listSigElementDefs=SigElementDefs.GetElementsForButDef(sigButDef);
					if(listSigElementDefs.Count > 0) {
						buttonText+=" ("+string.Join(",",listSigElementDefs.Select(x => x.SigText))+")";
					}
					if(sigButDef.ComputerName=="" && listComputers.SelectedIndex!=0) {
						buttonText+=" "+Lan.g(this,"(all)");
					}
					listButtons.Items.Add(buttonText);
				}
			}
		}

		private void listComputers_Click(object sender,EventArgs e) {
			//Cache needs to be saved to the database.
			if(SigButDefs.UpdateButtonIndexIfChanged(_arraySigButDefs)) {
				DataValid.SetInvalid(InvalidType.SigMessages);
			}
			if(listComputers.SelectedIndex==0) {
				_arraySigButDefs=SigButDefs.GetByComputer("");
			}
			else {
				//remember, defaults are mixed into this list unless overridden:
				_arraySigButDefs=SigButDefs.GetByComputer(_listComputers[listComputers.SelectedIndex-1].CompName);
			}
			FillList();
		}

		private void listButtons_DoubleClick(object sender,EventArgs e) {
			if(listButtons.SelectedIndex==-1) {//should never happen
				return;
			}
			//Save any changes to the cache because the item order could have changed.
			if(SigButDefs.UpdateButtonIndexIfChanged(_arraySigButDefs)) {
				DataValid.SetInvalid(InvalidType.SigMessages);
			}
			int selected=listButtons.SelectedIndex;
			SigButDef sigButDef=SigButDefs.GetByIndex(selected,_arraySigButDefs);
			//Keep track of the currently selected computer name so we know what computer buttons to refresh after making changes.
			string computerNameSelected=(listComputers.SelectedIndex > 0) ? _listComputers[listComputers.SelectedIndex-1].CompName : "";
			//Now create a new computer name variable that will represent the computer name for the SigButDef.
			string computerNameSigButDef="";
			if(sigButDef==null) {//Add
				sigButDef=new SigButDef();
				sigButDef.ButtonIndex=selected;
				if(listComputers.SelectedIndex!=0) {
					computerNameSigButDef=_listComputers[listComputers.SelectedIndex-1].CompName;
				}
				sigButDef.ComputerName=computerNameSigButDef;
				FormSigButDefEdit FormS=new FormSigButDefEdit(sigButDef.Copy());
				FormS.IsNew=true;
				FormS.ShowDialog();
			}
			else if(sigButDef.ComputerName=="" && listComputers.SelectedIndex!=0) {
				//create a copy of the default, and treat it as a new
				sigButDef.ComputerName=_listComputers[listComputers.SelectedIndex-1].CompName;
				FormSigButDefEdit FormS=new FormSigButDefEdit(sigButDef.Copy());
				FormS.IsNew=true;
				FormS.ShowDialog();
			}
			else {//edit
				if(listComputers.SelectedIndex>0) {//If "All" is selected, the computerName will already be blank, so it only needs reset if it isn't "All".
					computerNameSigButDef=_listComputers[listComputers.SelectedIndex-1].CompName;
				}
				FormSigButDefEdit FormS=new FormSigButDefEdit(sigButDef.Copy());
				FormS.ShowDialog();
			}
			//Refresh our local list to match the cache in case the user edited or added a new button item.
			_arraySigButDefs=SigButDefs.GetByComputer(computerNameSelected);
			FillList();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(listButtons.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			int selected=listButtons.SelectedIndex;
			if(selected==0) {
				return;
			}
			SigButDef button=SigButDefs.GetByIndex(selected,_arraySigButDefs);
			if(button==null) {
				return;
			}
			_arraySigButDefs=SigButDefs.MoveUp(button,_arraySigButDefs);
			FillList();
			listButtons.SelectedIndex=selected-1;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(listButtons.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			int selected=listButtons.SelectedIndex;
			if(selected==listButtons.Items.Count-1) {
				return;
			}
			SigButDef button=SigButDefs.GetByIndex(selected,_arraySigButDefs);
			if(button==null) {
				return;
			}
			_arraySigButDefs=SigButDefs.MoveDown(button,_arraySigButDefs);
			FillList();
			listButtons.SelectedIndex=selected+1;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormMessagingButSetup_FormClosing(object sender,FormClosingEventArgs e) {
			SigButDefs.UpdateButtonIndexIfChanged(_arraySigButDefs);
			DataValid.SetInvalid(InvalidType.SigMessages);
		}

		

		

		

		

	

		


	}
}






















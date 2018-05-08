using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental{
	/// <summary></summary>
	public class FormOperatoryEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.TextBox textOpName;
		private System.Windows.Forms.TextBox textAbbrev;
		private System.Windows.Forms.CheckBox checkIsHidden;
		private System.Windows.Forms.ComboBox comboHyg;
		private System.Windows.Forms.ComboBox comboProv;
		private System.Windows.Forms.CheckBox checkIsHygiene;
		private CheckBox checkSetProspective;
		private Label label3;
		private Operatory OpCur;
		private Label label4;
		private CheckBox checkIsWebSched;
		private UI.Button butPickClin;
		private UI.Button butPickProv;
		private UI.Button butPickHyg;
		public List<Operatory> ListOps;
		///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
		private List<Clinic> _listClinics;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is not in _listClinics.</summary>
		private long _selectedClinicNum;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvNum;
		private Label labelApptType;
		private GroupBox groupBoxApptType;
		private Label label10;
		private UI.Button butWSNPAPickApptTypes;
		///<summary>Instead of relying on _listProviders[comboProvHyg.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvHygNum;
		private TextBox textWSNPAApptTypes;
		///<summary>All of the Web Sched New Pat Appt appointment type defs that this operatory is associated to.</summary>
		private List<Def> _listWSNPAOperatoryDefs=new List<Def>();

		///<summary></summary>
		public FormOperatoryEdit(Operatory opCur)
		{
			//
			// Required for Windows Form Designer support
			//
			OpCur=opCur;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOperatoryEdit));
			this.labelApptType = new System.Windows.Forms.Label();
			this.butPickHyg = new OpenDental.UI.Button();
			this.butPickProv = new OpenDental.UI.Button();
			this.butPickClin = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.checkIsWebSched = new System.Windows.Forms.CheckBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.checkSetProspective = new System.Windows.Forms.CheckBox();
			this.checkIsHygiene = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboHyg = new System.Windows.Forms.ComboBox();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.textAbbrev = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textOpName = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBoxApptType = new System.Windows.Forms.GroupBox();
			this.textWSNPAApptTypes = new System.Windows.Forms.TextBox();
			this.butWSNPAPickApptTypes = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.groupBoxApptType.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelApptType
			// 
			this.labelApptType.Location = new System.Drawing.Point(16, 42);
			this.labelApptType.Name = "labelApptType";
			this.labelApptType.Size = new System.Drawing.Size(117, 17);
			this.labelApptType.TabIndex = 126;
			this.labelApptType.Text = "New Pat Appt Types";
			this.labelApptType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickHyg
			// 
			this.butPickHyg.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickHyg.Autosize = false;
			this.butPickHyg.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickHyg.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickHyg.CornerRadius = 2F;
			this.butPickHyg.Location = new System.Drawing.Point(412, 123);
			this.butPickHyg.Name = "butPickHyg";
			this.butPickHyg.Size = new System.Drawing.Size(23, 21);
			this.butPickHyg.TabIndex = 123;
			this.butPickHyg.Text = "...";
			this.butPickHyg.Click += new System.EventHandler(this.butPickHyg_Click);
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(412, 102);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(23, 21);
			this.butPickProv.TabIndex = 122;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// butPickClin
			// 
			this.butPickClin.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClin.Autosize = false;
			this.butPickClin.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClin.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClin.CornerRadius = 2F;
			this.butPickClin.Location = new System.Drawing.Point(412, 81);
			this.butPickClin.Name = "butPickClin";
			this.butPickClin.Size = new System.Drawing.Size(23, 21);
			this.butPickClin.TabIndex = 121;
			this.butPickClin.Text = "...";
			this.butPickClin.Click += new System.EventHandler(this.butPickClin_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(151, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(358, 16);
			this.label4.TabIndex = 120;
			this.label4.Text = "This operatory will be available for Web Sched recall appointments.";
			// 
			// checkIsWebSched
			// 
			this.checkIsWebSched.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsWebSched.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsWebSched.Location = new System.Drawing.Point(11, 19);
			this.checkIsWebSched.Name = "checkIsWebSched";
			this.checkIsWebSched.Size = new System.Drawing.Size(135, 16);
			this.checkIsWebSched.TabIndex = 119;
			this.checkIsWebSched.Text = "Is Recall";
			this.checkIsWebSched.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(160, 81);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(252, 21);
			this.comboClinic.TabIndex = 3;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(59, 85);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(98, 16);
			this.labelClinic.TabIndex = 118;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(178, 169);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(363, 16);
			this.label3.TabIndex = 117;
			this.label3.Text = "Change status of patients in this operatory to prospective.";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(178, 150);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(363, 16);
			this.label9.TabIndex = 117;
			this.label9.Text = "The hygienist will be considered the main provider for this op.";
			// 
			// checkSetProspective
			// 
			this.checkSetProspective.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSetProspective.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSetProspective.Location = new System.Drawing.Point(69, 168);
			this.checkSetProspective.Name = "checkSetProspective";
			this.checkSetProspective.Size = new System.Drawing.Size(104, 16);
			this.checkSetProspective.TabIndex = 7;
			this.checkSetProspective.Text = "Set Prospective";
			this.checkSetProspective.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsHygiene
			// 
			this.checkIsHygiene.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHygiene.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHygiene.Location = new System.Drawing.Point(69, 149);
			this.checkIsHygiene.Name = "checkIsHygiene";
			this.checkIsHygiene.Size = new System.Drawing.Size(104, 16);
			this.checkIsHygiene.TabIndex = 6;
			this.checkIsHygiene.Text = "Is Hygiene";
			this.checkIsHygiene.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(178, 64);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(316, 16);
			this.label8.TabIndex = 115;
			this.label8.Text = "(because you can\'t delete an operatory)";
			// 
			// comboHyg
			// 
			this.comboHyg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboHyg.Location = new System.Drawing.Point(160, 123);
			this.comboHyg.MaxDropDownItems = 30;
			this.comboHyg.Name = "comboHyg";
			this.comboHyg.Size = new System.Drawing.Size(252, 21);
			this.comboHyg.TabIndex = 5;
			this.comboHyg.SelectedIndexChanged += new System.EventHandler(this.comboHyg_SelectedIndexChanged);
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(160, 102);
			this.comboProv.MaxDropDownItems = 30;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(252, 21);
			this.comboProv.TabIndex = 4;
			this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(34, 127);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(123, 16);
			this.label6.TabIndex = 112;
			this.label6.Text = "Hygienist";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(23, 106);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(134, 16);
			this.label7.TabIndex = 111;
			this.label7.Text = "Provider";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHidden.Location = new System.Drawing.Point(69, 63);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(104, 16);
			this.checkIsHidden.TabIndex = 2;
			this.checkIsHidden.Text = "Is Hidden";
			this.checkIsHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAbbrev
			// 
			this.textAbbrev.Location = new System.Drawing.Point(160, 40);
			this.textAbbrev.MaxLength = 5;
			this.textAbbrev.Name = "textAbbrev";
			this.textAbbrev.Size = new System.Drawing.Size(78, 20);
			this.textAbbrev.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(151, 17);
			this.label2.TabIndex = 99;
			this.label2.Text = "Abbrev (max 5 char)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textOpName
			// 
			this.textOpName.Location = new System.Drawing.Point(160, 20);
			this.textOpName.MaxLength = 255;
			this.textOpName.Name = "textOpName";
			this.textOpName.Size = new System.Drawing.Size(252, 20);
			this.textOpName.TabIndex = 0;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(396, 309);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 8;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(487, 309);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Op Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxApptType
			// 
			this.groupBoxApptType.Controls.Add(this.textWSNPAApptTypes);
			this.groupBoxApptType.Controls.Add(this.butWSNPAPickApptTypes);
			this.groupBoxApptType.Controls.Add(this.label10);
			this.groupBoxApptType.Controls.Add(this.labelApptType);
			this.groupBoxApptType.Controls.Add(this.label4);
			this.groupBoxApptType.Controls.Add(this.checkIsWebSched);
			this.groupBoxApptType.Location = new System.Drawing.Point(26, 190);
			this.groupBoxApptType.Name = "groupBoxApptType";
			this.groupBoxApptType.Size = new System.Drawing.Size(515, 100);
			this.groupBoxApptType.TabIndex = 128;
			this.groupBoxApptType.TabStop = false;
			this.groupBoxApptType.Text = "Web Sched Settings";
			// 
			// textWSNPAApptTypes
			// 
			this.textWSNPAApptTypes.Location = new System.Drawing.Point(134, 41);
			this.textWSNPAApptTypes.MaxLength = 255;
			this.textWSNPAApptTypes.Name = "textWSNPAApptTypes";
			this.textWSNPAApptTypes.ReadOnly = true;
			this.textWSNPAApptTypes.Size = new System.Drawing.Size(252, 20);
			this.textWSNPAApptTypes.TabIndex = 129;
			// 
			// butWSNPAPickApptTypes
			// 
			this.butWSNPAPickApptTypes.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWSNPAPickApptTypes.Autosize = false;
			this.butWSNPAPickApptTypes.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWSNPAPickApptTypes.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWSNPAPickApptTypes.CornerRadius = 2F;
			this.butWSNPAPickApptTypes.Location = new System.Drawing.Point(386, 40);
			this.butWSNPAPickApptTypes.Name = "butWSNPAPickApptTypes";
			this.butWSNPAPickApptTypes.Size = new System.Drawing.Size(23, 22);
			this.butWSNPAPickApptTypes.TabIndex = 129;
			this.butWSNPAPickApptTypes.Text = "...";
			this.butWSNPAPickApptTypes.Click += new System.EventHandler(this.butWSNPAPickApptTypes_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(131, 65);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(378, 29);
			this.label10.TabIndex = 128;
			this.label10.Text = "Only the above appointment types will be allowed within this operatory.\r\nAppt Typ" +
    "e is required to be considered for Web Sched New Pat Appt.";
			// 
			// FormOperatoryEdit
			// 
			this.ClientSize = new System.Drawing.Size(574, 347);
			this.Controls.Add(this.groupBoxApptType);
			this.Controls.Add(this.butPickHyg);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.butPickClin);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.checkSetProspective);
			this.Controls.Add(this.checkIsHygiene);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.comboHyg);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.checkIsHidden);
			this.Controls.Add(this.textAbbrev);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textOpName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormOperatoryEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Operatory";
			this.Load += new System.EventHandler(this.FormOperatoryEdit_Load);
			this.groupBoxApptType.ResumeLayout(false);
			this.groupBoxApptType.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormOperatoryEdit_Load(object sender, System.EventArgs e) {
			textOpName.Text=OpCur.OpName;
			textAbbrev.Text=OpCur.Abbrev;
			checkIsHidden.Checked=OpCur.IsHidden;
			if(PrefC.GetBool(PrefName.EasyNoClinics)){
				labelClinic.Visible=false;
				comboClinic.Visible=false;
				butPickClin.Visible=false;
			}
			_listClinics=new List<Clinic>() { new Clinic() { Abbr=Lan.g(this,"None") } }; //Seed with "None"
			Clinics.GetForUserod(Security.CurUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
			_listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));//Add to UI control
			//Set Selected nums
			_selectedClinicNum=OpCur.ClinicNum;//can be 0
			_selectedProvNum=OpCur.ProvDentist;
			_selectedProvHygNum=OpCur.ProvHygienist;
			//Set comboindexes for compatibility with fillComboProvHyg
			comboProv.SelectedIndex=-1;
			comboHyg.SelectedIndex=-1;
			comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum)
				,() => { return Clinics.GetAbbr(_selectedClinicNum); });
			if(OpCur.ListWSNPAOperatoryDefNums!=null) {
				//This is an existing operatory with WSNPA appointment types associated.  Go get them in order to display to the user.
				_listWSNPAOperatoryDefs=Defs.GetDefs(DefCat.WebSchedNewPatApptTypes,OpCur.ListWSNPAOperatoryDefNums);
			}
			FillWSNPAApptTypes();
			FillComboProvHyg();
			checkIsHygiene.Checked=OpCur.IsHygiene;
			checkSetProspective.Checked=OpCur.SetProspective;
			checkIsWebSched.Checked=OpCur.IsWebSched;
		}

		private void FillWSNPAApptTypes() {
			textWSNPAApptTypes.Clear();
			if(_listWSNPAOperatoryDefs.Count < 1) {
				return;
			}
			textWSNPAApptTypes.Text=string.Join(", ",_listWSNPAOperatoryDefs.Select(x => x.ItemName));
		}

		private void butPickClin_Click(object sender,EventArgs e) {
			FormClinics FormC = new FormClinics() { ListClinics=_listClinics };
			FormC.IsSelectionMode=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedClinicNum=FormC.SelectedClinicNum;
			comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum),() => { return Clinics.GetAbbr(_selectedClinicNum); });
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvNum=FormPP.SelectedProvNum;
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		private void butPickHyg_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvHygNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvHygNum =FormPP.SelectedProvNum;
			comboHyg.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvHygNum),() => { return Providers.GetAbbr(_selectedProvHygNum); });
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			FillComboProvHyg();
		}

		private void comboProv_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboProv.SelectedIndex>-1) {
				_selectedProvNum=_listProviders[comboProv.SelectedIndex].ProvNum;
			}
		}

		private void comboHyg_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboHyg.SelectedIndex>-1) {
				_selectedProvHygNum=_listProviders[comboHyg.SelectedIndex].ProvNum;
			}
		}

		private void butWSNPAPickApptTypes_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDP=new FormDefinitionPicker(DefCat.WebSchedNewPatApptTypes,_listWSNPAOperatoryDefs);
			FormDP.IsMultiSelectionMode=true;
			if(FormDP.ShowDialog()==DialogResult.OK) {
				_listWSNPAOperatoryDefs=FormDP.ListSelectedDefs.Select(x => x.Copy()).ToList();
				FillWSNPAApptTypes();
			}
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillComboProvHyg() {
			if(comboProv.SelectedIndex>-1) {//valid prov selected, non none or nothing.
				_selectedProvNum = _listProviders[comboProv.SelectedIndex].ProvNum;
			}
			if(comboHyg.SelectedIndex>-1) {
				_selectedProvHygNum = _listProviders[comboHyg.SelectedIndex].ProvNum;
			}
			_listProviders=new List<Provider>() { new Provider() { Abbr="none" } };
			_listProviders.AddRange(Providers.GetProvsForClinic(_selectedClinicNum));
			_listProviders=_listProviders.OrderBy(x => x.ProvNum>0).ThenBy(x => x.ItemOrder).ToList();//order list properly, None first
			//fill comboProv
			comboProv.Items.Clear();
			_listProviders.ForEach(x => comboProv.Items.Add(x.Abbr));
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
			//Fill comboHyg
			comboHyg.Items.Clear();
			_listProviders.ForEach(x => comboHyg.Items.Add(x.Abbr));
			comboHyg.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvHygNum),() => { return Providers.GetAbbr(_selectedProvHygNum); });
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textOpName.Text==""){
				MsgBox.Show(this,"Operatory name cannot be blank.");
				return;
			}
			if(checkIsHidden.Checked==true && Operatories.HasFutureApts(OpCur.OperatoryNum,ApptStatus.UnschedList)) {
				MsgBox.Show(this,"Can not hide an operatory with future appointments.");
				checkIsHidden.Checked=false;
				return;
			}
			OpCur.OpName=textOpName.Text;
			OpCur.Abbrev=textAbbrev.Text;
			OpCur.IsHidden=checkIsHidden.Checked;
			OpCur.ClinicNum=_selectedClinicNum;
			OpCur.ProvDentist=_selectedProvNum;
			OpCur.ProvHygienist=_selectedProvHygNum;
			OpCur.IsHygiene=checkIsHygiene.Checked;
			OpCur.SetProspective=checkSetProspective.Checked;
			OpCur.IsWebSched=checkIsWebSched.Checked;
			OpCur.ListWSNPAOperatoryDefNums=_listWSNPAOperatoryDefs.Select(x => x.DefNum).ToList();
			if(IsNew) {
				ListOps.Insert(OpCur.ItemOrder,OpCur);//Insert into list at appropriate spot
				for(int i=0;i<ListOps.Count;i++) {
					ListOps[i].ItemOrder=i;//reset/correct item orders
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}






















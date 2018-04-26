using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Linq;
using System.Threading;
using MySql.Data.MySqlClient;

namespace OpenDental{
///<summary></summary>
	public class FormBilling : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butAll;
		private OpenDental.UI.Button butNone;
		private OpenDental.UI.Button butSend;
		private IContainer components;
		private OpenDental.UI.ODGrid gridBill;
		private Label labelTotal;
		private Label label1;
		private RadioButton radioUnsent;
		private RadioButton radioSent;
		private GroupBox groupBox1;
		private Label labelSelected;
		private Label labelEmailed;
		private Label labelPrinted;
		private OpenDental.UI.Button butEdit;
		private ValidDate textDateEnd;
		private ValidDate textDateStart;
		private Label label2;
		private Label label3;
		private OpenDental.UI.Button butRefresh;
		private ComboBox comboOrder;
		private Label label4;
		private OpenDental.UI.Button butPrintList;
		private ContextMenuStrip contextMenu;
		private ToolStripMenuItem menuItemGoTo;
		private bool headingPrinted;
		private int headingPrintH;
		private Label label5;
		private int pagesPrinted;
		///<summary>Used in the Activated event.</summary>
		private bool isPrinting=false;
		private DataTable table;
		private Label labelSentElect;
		private bool isInitial=true;
		private ComboBox comboClinic;
		private Label labelClinic;
		private bool ignoreRefreshOnce;
		public long ClinicNum;
		private ComboBox comboEmailFrom;
		private Label label6;
		///<summary>Do not pass a list of clinics in.  This list gets filled on load based on the user logged in.  ListClinics is used in other forms so it is public.</summary>
		public List<Clinic> ListClinics;
		private List<EmailAddress> _listEmailAddresses;
		private bool _isActivateFillDisabled;
		private List<long> _listStatementNumsSent;
		private List<long> _listStatementNumsToSkip;
		private UI.Button butDefaults;
		///<summary>This can be used to interact with FormProgressExtended.</summary>
		private ODProgressExtended _progExtended;
		private Label labelTexted;

		///<summary>The families that are selected when the user hits "Send". The key is the PatNum and the value is its Family.</summary>
		Dictionary<long,Family> _dictFams;

		///<summary></summary>
		public FormBilling(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBilling));
			this.butCancel = new OpenDental.UI.Button();
			this.butSend = new OpenDental.UI.Button();
			this.butNone = new OpenDental.UI.Button();
			this.butAll = new OpenDental.UI.Button();
			this.gridBill = new OpenDental.UI.ODGrid();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemGoTo = new System.Windows.Forms.ToolStripMenuItem();
			this.labelTotal = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.radioUnsent = new System.Windows.Forms.RadioButton();
			this.radioSent = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelSentElect = new System.Windows.Forms.Label();
			this.labelEmailed = new System.Windows.Forms.Label();
			this.labelPrinted = new System.Windows.Forms.Label();
			this.labelSelected = new System.Windows.Forms.Label();
			this.butEdit = new OpenDental.UI.Button();
			this.textDateEnd = new OpenDental.ValidDate();
			this.textDateStart = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.comboOrder = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butPrintList = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboEmailFrom = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.butDefaults = new OpenDental.UI.Button();
			this.labelTexted = new System.Windows.Forms.Label();
			this.contextMenu.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(802, 656);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 1;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSend.Autosize = true;
			this.butSend.BackColor = System.Drawing.SystemColors.Control;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Location = new System.Drawing.Point(802, 622);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(75, 24);
			this.butSend.TabIndex = 0;
			this.butSend.Text = "Send";
			this.butSend.UseVisualStyleBackColor = false;
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(103, 656);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 24);
			this.butNone.TabIndex = 23;
			this.butNone.Text = "&None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAll.CornerRadius = 4F;
			this.butAll.Location = new System.Drawing.Point(12, 656);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(75, 24);
			this.butAll.TabIndex = 22;
			this.butAll.Text = "&All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// gridBill
			// 
			this.gridBill.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridBill.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridBill.ContextMenuStrip = this.contextMenu;
			this.gridBill.HasAddButton = false;
			this.gridBill.HasDropDowns = false;
			this.gridBill.HasMultilineHeaders = false;
			this.gridBill.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridBill.HeaderHeight = 15;
			this.gridBill.HScrollVisible = false;
			this.gridBill.Location = new System.Drawing.Point(12, 58);
			this.gridBill.Name = "gridBill";
			this.gridBill.ScrollValue = 0;
			this.gridBill.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridBill.Size = new System.Drawing.Size(772, 590);
			this.gridBill.TabIndex = 28;
			this.gridBill.Title = "Bills";
			this.gridBill.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridBill.TitleHeight = 18;
			this.gridBill.TranslationName = "TableBilling";
			this.gridBill.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridBill_CellDoubleClick);
			this.gridBill.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridBill_CellClick);
			this.gridBill.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridBill_MouseDown);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGoTo});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(106, 26);
			// 
			// menuItemGoTo
			// 
			this.menuItemGoTo.Name = "menuItemGoTo";
			this.menuItemGoTo.Size = new System.Drawing.Size(105, 22);
			this.menuItemGoTo.Text = "Go To";
			this.menuItemGoTo.Click += new System.EventHandler(this.menuItemGoTo_Click);
			// 
			// labelTotal
			// 
			this.labelTotal.Location = new System.Drawing.Point(4, 19);
			this.labelTotal.Name = "labelTotal";
			this.labelTotal.Size = new System.Drawing.Size(89, 16);
			this.labelTotal.TabIndex = 29;
			this.labelTotal.Text = "Total=20";
			this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(787, 530);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 87);
			this.label1.TabIndex = 30;
			this.label1.Text = "This will immediately print or email all selected bills";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// radioUnsent
			// 
			this.radioUnsent.Checked = true;
			this.radioUnsent.Location = new System.Drawing.Point(31, 7);
			this.radioUnsent.Name = "radioUnsent";
			this.radioUnsent.Size = new System.Drawing.Size(75, 20);
			this.radioUnsent.TabIndex = 31;
			this.radioUnsent.TabStop = true;
			this.radioUnsent.Text = "Unsent";
			this.radioUnsent.UseVisualStyleBackColor = true;
			this.radioUnsent.Click += new System.EventHandler(this.radioUnsent_Click);
			// 
			// radioSent
			// 
			this.radioSent.Location = new System.Drawing.Point(31, 29);
			this.radioSent.Name = "radioSent";
			this.radioSent.Size = new System.Drawing.Size(75, 20);
			this.radioSent.TabIndex = 32;
			this.radioSent.Text = "Sent";
			this.radioSent.UseVisualStyleBackColor = true;
			this.radioSent.Click += new System.EventHandler(this.radioSent_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.labelTexted);
			this.groupBox1.Controls.Add(this.labelSentElect);
			this.groupBox1.Controls.Add(this.labelEmailed);
			this.groupBox1.Controls.Add(this.labelPrinted);
			this.groupBox1.Controls.Add(this.labelSelected);
			this.groupBox1.Controls.Add(this.labelTotal);
			this.groupBox1.Location = new System.Drawing.Point(788, 233);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(96, 136);
			this.groupBox1.TabIndex = 33;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Counts";
			// 
			// labelSentElect
			// 
			this.labelSentElect.Location = new System.Drawing.Point(4, 95);
			this.labelSentElect.Name = "labelSentElect";
			this.labelSentElect.Size = new System.Drawing.Size(89, 16);
			this.labelSentElect.TabIndex = 33;
			this.labelSentElect.Text = "SentElect=20";
			this.labelSentElect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelEmailed
			// 
			this.labelEmailed.Location = new System.Drawing.Point(4, 76);
			this.labelEmailed.Name = "labelEmailed";
			this.labelEmailed.Size = new System.Drawing.Size(89, 16);
			this.labelEmailed.TabIndex = 32;
			this.labelEmailed.Text = "Emailed=20";
			this.labelEmailed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelPrinted
			// 
			this.labelPrinted.Location = new System.Drawing.Point(4, 57);
			this.labelPrinted.Name = "labelPrinted";
			this.labelPrinted.Size = new System.Drawing.Size(89, 16);
			this.labelPrinted.TabIndex = 31;
			this.labelPrinted.Text = "Printed=20";
			this.labelPrinted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelSelected
			// 
			this.labelSelected.Location = new System.Drawing.Point(4, 38);
			this.labelSelected.Name = "labelSelected";
			this.labelSelected.Size = new System.Drawing.Size(89, 16);
			this.labelSelected.TabIndex = 30;
			this.labelSelected.Text = "Selected=20";
			this.labelSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Location = new System.Drawing.Point(796, 67);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(82, 24);
			this.butEdit.TabIndex = 34;
			this.butEdit.Text = "Edit Selected";
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			// 
			// textDateEnd
			// 
			this.textDateEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDateEnd.Location = new System.Drawing.Point(694, 32);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(77, 20);
			this.textDateEnd.TabIndex = 38;
			// 
			// textDateStart
			// 
			this.textDateStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDateStart.Location = new System.Drawing.Point(695, 8);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(77, 20);
			this.textDateStart.TabIndex = 37;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(628, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 14);
			this.label2.TabIndex = 36;
			this.label2.Text = "End Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(622, 11);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 14);
			this.label3.TabIndex = 35;
			this.label3.Text = "Start Date";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(796, 7);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(82, 24);
			this.butRefresh.TabIndex = 39;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// comboOrder
			// 
			this.comboOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboOrder.Location = new System.Drawing.Point(196, 8);
			this.comboOrder.MaxDropDownItems = 40;
			this.comboOrder.Name = "comboOrder";
			this.comboOrder.Size = new System.Drawing.Size(173, 21);
			this.comboOrder.TabIndex = 41;
			this.comboOrder.SelectionChangeCommitted += new System.EventHandler(this.comboOrder_SelectionChangeCommitted);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(112, 12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82, 14);
			this.label4.TabIndex = 40;
			this.label4.Text = "Order by";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPrintList
			// 
			this.butPrintList.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrintList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrintList.Autosize = true;
			this.butPrintList.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrintList.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrintList.CornerRadius = 4F;
			this.butPrintList.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrintList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintList.Location = new System.Drawing.Point(364, 656);
			this.butPrintList.Name = "butPrintList";
			this.butPrintList.Size = new System.Drawing.Size(88, 24);
			this.butPrintList.TabIndex = 42;
			this.butPrintList.Text = "Print List";
			this.butPrintList.Click += new System.EventHandler(this.butPrintList_Click);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(456, 651);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(165, 29);
			this.label5.TabIndex = 43;
			this.label5.Text = "Does not print individual bills.  Just prints the list of bills.";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboClinic
			// 
			this.comboClinic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(429, 8);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(187, 21);
			this.comboClinic.TabIndex = 44;
			this.comboClinic.Visible = false;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(375, 13);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(52, 14);
			this.labelClinic.TabIndex = 45;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// comboEmailFrom
			// 
			this.comboEmailFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEmailFrom.Location = new System.Drawing.Point(196, 32);
			this.comboEmailFrom.MaxDropDownItems = 40;
			this.comboEmailFrom.Name = "comboEmailFrom";
			this.comboEmailFrom.Size = new System.Drawing.Size(173, 21);
			this.comboEmailFrom.TabIndex = 47;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(112, 36);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(82, 14);
			this.label6.TabIndex = 46;
			this.label6.Text = "Email From";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDefaults
			// 
			this.butDefaults.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDefaults.Autosize = true;
			this.butDefaults.BackColor = System.Drawing.SystemColors.Control;
			this.butDefaults.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefaults.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefaults.CornerRadius = 4F;
			this.butDefaults.Location = new System.Drawing.Point(802, 503);
			this.butDefaults.Name = "butDefaults";
			this.butDefaults.Size = new System.Drawing.Size(74, 24);
			this.butDefaults.TabIndex = 48;
			this.butDefaults.Text = "Defaults";
			this.butDefaults.UseVisualStyleBackColor = false;
			this.butDefaults.Click += new System.EventHandler(this.butDefaults_Click);
			// 
			// labelTexted
			// 
			this.labelTexted.Location = new System.Drawing.Point(4, 114);
			this.labelTexted.Name = "labelTexted";
			this.labelTexted.Size = new System.Drawing.Size(89, 16);
			this.labelTexted.TabIndex = 34;
			this.labelTexted.Text = "Texted=20";
			this.labelTexted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormBilling
			// 
			this.AcceptButton = this.butSend;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(888, 688);
			this.Controls.Add(this.butDefaults);
			this.Controls.Add(this.comboEmailFrom);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.comboOrder);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butPrintList);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butEdit);
			this.Controls.Add(this.textDateStart);
			this.Controls.Add(this.textDateEnd);
			this.Controls.Add(this.radioUnsent);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.radioSent);
			this.Controls.Add(this.gridBill);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butAll);
			this.Controls.Add(this.butSend);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(883, 525);
			this.Name = "FormBilling";
			this.Text = "Bills";
			this.Activated += new System.EventHandler(this.FormBilling_Activated);
			this.Load += new System.EventHandler(this.FormBilling_Load);
			this.contextMenu.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormBilling_Load(object sender,System.EventArgs e) {
			//NOTE: this form can be very slow and reloads all data every time it gains focus.All data is requeried from the DB.
			//Suggestions on how to improve speed are:
			//1) use form-level signal processing to set a bool on this form to determine if it needs to refresh the grid when it regains focus.
			//2) add index to statement (IsSent, Patnum, DateSent)
			//3) split the get billing table query into two smaller querries, one that gets everything except the LastStatement column, and one to select
			//  the LastStatement, PatNum and then stitch them together in C#. 
			//  In testing this improved execution time from 1.3 seconds to return ~2500 rows down to 0.08 for the main query and 0.05 for the LastStatement date
			//  Stitching the data sets together was not tested but should be faster than MySQL.
			labelPrinted.Text=Lan.g(this,"Printed=")+"0";
			labelEmailed.Text=Lan.g(this,"E-mailed=")+"0";
			labelSentElect.Text=Lan.g(this,"SentElect=")+"0";
			labelTexted.Text=Lan.g(this,"Texted=")+"0";
			comboOrder.Items.Add(Lan.g(this,"BillingType"));
			comboOrder.Items.Add(Lan.g(this,"PatientName"));
			comboOrder.SelectedIndex=0;
			//ListClinics can be called even when Clinics is not turned on, therefore it needs to be set to something to avoid a null reference.
			ListClinics=new List<Clinic>();
			_listStatementNumsToSkip=new List<long>();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics.
				labelClinic.Visible=true;
				comboClinic.Visible=true;
				comboClinic.Items.Add(Lan.g(this,"All"));
				comboClinic.SelectedIndex=0;
				ListClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<ListClinics.Count;i++) {
					comboClinic.Items.Add(ListClinics[i].Abbr);
					if(ClinicNum==ListClinics[i].ClinicNum) {
						comboClinic.SelectedIndex=i+1;
					}
				}
			}
			FillComboEmail();
			_isActivateFillDisabled=false;
		}

		private void FormBilling_Activated(object sender,EventArgs e) {
			if(IsDisposed) {//Attempted bug fix for an exception which occurred in FillGrid() when the grid was already disposed.
				return;
			}
			_progExtended?.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper("",progressBarEventType:ProgBarEventType.BringToFront)));
			//this gets fired very frequently, including right in the middle of printing a batch.
			if(isPrinting){
				return;
			}
			if(ignoreRefreshOnce) {
				ignoreRefreshOnce=false;
				return;
			}
			if(_isActivateFillDisabled) {
				return;
			}
			FillGrid();
		}

		///<summary>We will always try to preserve the selected bills as well as the scroll postition.</summary>
		private void FillGrid() {
			if(textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateEnd.errorProvider1.GetError(textDateEnd)!="")
			{
				ignoreRefreshOnce=true;
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			int scrollPos=gridBill.ScrollValue;
			List<long> selectedKeys=gridBill.SelectedIndices.OfType<int>().Select(x => PIn.Long(((DataRow)gridBill.Rows[x].Tag)["StatementNum"].ToString())).ToList();
			DateTime dateFrom=PIn.Date(textDateStart.Text);
			DateTime dateTo=new DateTime(2200,1,1);
			if(textDateEnd.Text!=""){
				dateTo=PIn.Date(textDateEnd.Text);
			}
			List<long> clinicNums=new List<long>();//an empty list indicates to Statements.GetBilling to run for all clinics
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex>0) {
				clinicNums.Add(ListClinics[comboClinic.SelectedIndex-1].ClinicNum);
			}
			table=Statements.GetBilling(radioSent.Checked,comboOrder.SelectedIndex,dateFrom,dateTo,clinicNums);
			gridBill.BeginUpdate();
			gridBill.Columns.Clear();
			ODGridColumn col=null;
			if(PrefC.HasSuperStatementsEnabled) {
				col=new ODGridColumn(Lan.g("TableBilling","Name"),150);
			}
			else {
				col=new ODGridColumn(Lan.g("TableBilling","Name"),180);
			}
			gridBill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableBilling","BillType"),110);
			gridBill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableBilling","Mode"),80);
			gridBill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableBilling","LastStatement"),100);
			gridBill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableBilling","BalTot"),70,HorizontalAlignment.Right);
			gridBill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableBilling","-InsEst"),70,HorizontalAlignment.Right);
			gridBill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableBilling","=AmtDue"),70,HorizontalAlignment.Right);
			gridBill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableBilling","PayPlanDue"),70,HorizontalAlignment.Right);
			gridBill.Columns.Add(col);
			if(PrefC.HasSuperStatementsEnabled) {
				col=new ODGridColumn(Lan.g("TableBilling","SF"),30);
				gridBill.Columns.Add(col);
			}
			gridBill.Rows.Clear();
			ODGridRow row;
			foreach(DataRow rowCur in table.Rows) {
				row=new ODGridRow();
				row.Cells.Add(rowCur["name"].ToString());
				row.Cells.Add(rowCur["billingType"].ToString());
				row.Cells.Add(rowCur["mode"].ToString());
				row.Cells.Add(rowCur["lastStatement"].ToString());
				row.Cells.Add(rowCur["balTotal"].ToString());
				row.Cells.Add(rowCur["insEst"].ToString());
				if(PrefC.GetBool(PrefName.BalancesDontSubtractIns)) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(rowCur["amountDue"].ToString());
				}
				row.Cells.Add(rowCur["payPlanDue"].ToString());
				if(PrefC.HasSuperStatementsEnabled && rowCur["SuperFamily"].ToString()!="0") {
					row.Cells.Add("X");
				}
				row.Tag=rowCur;
				gridBill.Rows.Add(row);
			}
			gridBill.EndUpdate();
			if(isInitial){
				gridBill.SetSelected(true);
				isInitial=false;
			}
			else {
				for(int i=0;i<gridBill.Rows.Count;i++) {
					gridBill.SetSelected(i,selectedKeys.Contains(PIn.Long(((DataRow)gridBill.Rows[i].Tag)["StatementNum"].ToString())));
				}
			}
			gridBill.ScrollValue=scrollPos;
			labelTotal.Text=Lan.g(this,"Total=")+table.Rows.Count.ToString();
			labelSelected.Text=Lan.g(this,"Selected=")+gridBill.SelectedIndices.Length.ToString();
		}

		private void FillComboEmail() {
			_listEmailAddresses=EmailAddresses.GetDeepCopy();//Does not include user specific email addresses.
			List<Clinic> listClinicsAll=Clinics.GetDeepCopy();
			for(int i=0;i<listClinicsAll.Count;i++) {//Exclude any email addresses that are associated to a clinic.
				_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==listClinicsAll[i].EmailAddressNum);
			}
			//Exclude default practice email address.
			_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			//Exclude web mail notification email address.
			_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailNotifyAddressNum));
			comboEmailFrom.Items.Add(Lan.g(this,"Practice/Clinic"));//default
			comboEmailFrom.SelectedIndex=0;
			//Add all email addresses which are not associated to a user, a clinic, or either of the default email addresses.
			for(int i=0;i<_listEmailAddresses.Count;i++) {
				comboEmailFrom.Items.Add(_listEmailAddresses[i].EmailUsername);
			}
			//Add user specific email address if present.
			EmailAddress emailAddressMe=EmailAddresses.GetForUser(Security.CurUser.UserNum);//can be null
			if(emailAddressMe!=null) {
				_listEmailAddresses.Insert(0,emailAddressMe);
				comboEmailFrom.Items.Insert(1,Lan.g(this,"Me")+" <"+emailAddressMe.EmailUsername+">");//Just below Practice/Clinic
			}
		}

		private void butAll_Click(object sender, System.EventArgs e) {
			gridBill.SetSelected(true);
			labelSelected.Text=Lan.g(this,"Selected=")+gridBill.SelectedIndices.Length.ToString();
		}

		private void butNone_Click(object sender, System.EventArgs e) {	
			gridBill.SetSelected(false);
			labelSelected.Text=Lan.g(this,"Selected=")+gridBill.SelectedIndices.Length.ToString();
		}

		private void radioUnsent_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioSent_Click(object sender,EventArgs e) {
			textDateStart.Text=DateTime.Today.ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
			FillGrid();
		}

		private void comboOrder_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateEnd.errorProvider1.GetError(textDateEnd)!="")
			{
				ignoreRefreshOnce=true;
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			FillGrid();
		}

		private void gridBill_CellClick(object sender,ODGridClickEventArgs e) {
			labelSelected.Text=Lan.g(this,"Selected=")+gridBill.SelectedIndices.Length.ToString();
		}

		private void gridBill_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormStatementOptions FormSO=new FormStatementOptions();
			Statement stmt;
			stmt=Statements.GetStatement(PIn.Long(((DataRow)gridBill.Rows[e.Row].Tag)["StatementNum"].ToString()));
			//FormSO.StmtList=stmtList;
			FormSO.StmtCur=stmt;
			FormSO.ShowDialog();
		}

		private void gridBill_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right){
				gridBill.SetSelected(false);
			}
		}

		private void menuItemGoTo_Click(object sender,EventArgs e) {
			if(gridBill.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select one bill first.");
				return;
			}
			long patNum=PIn.Long(((DataRow)gridBill.Rows[gridBill.GetSelectedIndex()].Tag)["PatNum"].ToString());
			FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(patNum),false);
			GotoModule.GotoAccount(0);
			SendToBack();
		}

		private void butEdit_Click(object sender,EventArgs e) {
			if(gridBill.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select one or more bills first.");
				return;
			}
			FormStatementOptions FormSO=new FormStatementOptions();
			List<long> listStatementNums=new List<long>();
			foreach(int index in gridBill.SelectedIndices) {
				listStatementNums.Add(PIn.Long(((DataRow)gridBill.Rows[index].Tag)["StatementNum"].ToString()));
			}
			FormSO.StmtList=Statements.GetStatements(listStatementNums);
			FormSO.ShowDialog();
			//FillGrid happens automatically through Activated event.
		}

		private void butPrintList_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
			#else
				if(!PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Billing list printed")) {
					return;
				}
				try{
					pd.Print();
				}
				catch {
					MsgBox.Show(this,"Printer not available");
				}
			#endif			
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Billing List");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				//yPos+=(int)g.MeasureString(text,headingFont).Height;
				//text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				//g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=25;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridBill.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butSend_Click(object sender,System.EventArgs e) {
			_listStatementNumsSent=new List<long>();
			if(gridBill.SelectedIndices.Length==0){
				MessageBox.Show(Lan.g(this,"Please select items first."));
				return;
			}
			labelPrinted.Text=Lan.g(this,"Printed=")+"0";
			labelEmailed.Text=Lan.g(this,"E-mailed=")+"0";
			labelSentElect.Text=Lan.g(this,"SentElect=")+"0";
			labelTexted.Text=Lan.g(this,"Texted=")+"0";
			if(!MsgBox.Show(this,true,"Please be prepared to wait up to ten minutes while all the bills get processed.\r\nOnce complete, the pdf print preview will be launched in Adobe Reader.  You will print from that program.  Continue?")){
				return;
			}
			PdfDocument outputDocument = new PdfDocument();
			PrintDocument pd = null;
			DateTime dtNow = MiscData.GetNowDateTime();//used to keep track of the time when this first started.
			string errorMsg="";
			int skipped=0;
			int skippedElect=0;
			int skippedDeleted=0;
			Dictionary<string,int> dictSkippedElect=new Dictionary<string,int>();//The error message is the key and the value is the skipped count.
			int emailed=0;
			int printed=0;
			int sentElect=0;
			int texted=0;
			SendStatements(ref errorMsg,ref skipped,ref skippedElect,ref dictSkippedElect,ref emailed,ref printed,ref sentElect,ref outputDocument,ref pd,
				ref skippedDeleted,ref texted);
			_progExtended?.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),"100%",100,100,ProgBarStyle.Blocks,"1")));
			_progExtended?.Close();
			#region Printing Statements
			//now print-------------------------------------------------------------------------------------
			if(pd!=null) {
				string tempFileOutputDocument = PrefC.GetRandomTempFile(".pdf");
				outputDocument.Save(tempFileOutputDocument);
				try {
					Process.Start(tempFileOutputDocument);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Error: Please make sure Adobe Reader is installed.")+ex.Message);
				}
			}
			#endregion
			if(errorMsg!="") {
				MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(errorMsg);
				msgbox.ShowDialog();
			}
			string msg="";
			if(skipped>0){
				msg+=Lan.g(this,"Skipped due to missing or bad email address:")+" "+skipped.ToString()+"\r\n";
			}
			if(skippedElect>0) {
				msg+=Lan.g(this,"Skipped due to missing or bad mailing address:")+" "+skippedElect.ToString()+"\r\n";
			}
			if(skippedDeleted>0) {
				msg+=Lan.g(this,"Skipped due to being deleted by another user:")+" "+skippedDeleted.ToString()+"\r\n";
			}
			foreach(string errorElect in dictSkippedElect.Keys) {
				msg+=Lan.g(this,"Skipped due to")+" "+errorElect+": "+dictSkippedElect[errorElect].ToString()+"\r\n";
			}
			msg+=Lan.g(this,"Printed:")+" "+printed.ToString()+"\r\n"
				+Lan.g(this,"E-mailed:")+" "+emailed.ToString()+"\r\n"
				+Lan.g(this,"SentElect:")+" "+sentElect.ToString()+"\r\n"
				+Lan.g(this,"Texted:")+" "+texted.ToString();
			MessageBox.Show(msg);
			Cursor=Cursors.Default;
			isPrinting=false;
			FillGrid();//not automatic
		}

		public void SendStatements(ref string errorMsg,ref int skipped,ref int skippedElect,ref Dictionary<string,int> dictSkippedElect
			,ref int emailed,ref int printed,ref int sentElect,ref PdfDocument outputDocument,ref PrintDocument pd,ref int skippedDeleted,ref int texted) 
		{
			_progExtended=new ODProgressExtended("BillingEvent",new BillingEvent(),this,tag: new ProgressBarHelper(("Billing Progress")
				,progressBarEventType:ProgBarEventType.Header));
			_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper("",progressBarEventType:ProgBarEventType.BringToFront)));	
			Cursor=Cursors.WaitCursor;
			isPrinting=true;
			//Dictionary with key of clinicNum and a corresponding list of EbillStatements
			int numOfBatchesSent = 1;//Start at 1 so that it is better looking in the UI.
			int numOfBatchesTotal = 0;
			Dictionary<long,List<EbillStatement>> dictEbills = new Dictionary<long,List<EbillStatement>>();
			int maxStmtsPerBatch = PrefC.GetInt(PrefName.BillingElectBatchMax);
			if(maxStmtsPerBatch==0) {
				maxStmtsPerBatch=gridBill.SelectedIndices.Length;//Make the batch size equal to the list of statements so that we send them all at once.
			}
			numOfBatchesTotal=(int)Math.Ceiling((decimal)gridBill.SelectedIndices.Length/maxStmtsPerBatch);
			//FormProgressExtended will insert new bars on top. Statment is on bottom, batch middle, and overall on top. 
			_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n0 / 0","0%",0,100
				,ProgBarStyle.Blocks,"3",isTopHidden:true)));
			_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n0 / 0","0%",0,maxStmtsPerBatch
				,ProgBarStyle.Blocks,"2",isTopHidden:true)));
			_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),"1%",0,gridBill.SelectedIndices.Length
				,ProgBarStyle.Blocks,"1",isTopHidden:true)));
			_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Progress Log"),progressBarEventType:ProgBarEventType.ProgressLog)));
			_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Preparing First Batch")+"..."
				,progressBarEventType:ProgBarEventType.TextMsg)));
			List<Statement> listStatements=Statements.GetStatements(gridBill.SelectedTags<DataRow>()
				.Select(x => PIn.Long(x["StatementNum"].ToString())).ToList());
			_dictFams=Patients.GetFamilies(listStatements.Select(x => x.PatNum).ToList())
				.SelectMany(fam => fam.ListPats.Select(y => new { y.PatNum,fam }))
				.ToDictionary(x => x.PatNum,x => x.fam);
			AddInstallmentPlansToStatements(listStatements);
			//A dictionary of batches of statements.  The key is the batch num which is 1 based (helpful when displaying to the user).
			Dictionary<int,List<Statement>> dictStatementsForSend=new Dictionary<int,List<Statement>>();
			int batchCount=0;
			for(int i=0;i<listStatements.Count;i++) {
				if(i % maxStmtsPerBatch==0) {
					batchCount++;
					dictStatementsForSend.Add(batchCount,new List<Statement>());
				}
				dictStatementsForSend[batchCount].Add(listStatements[i]);
			}
			int curStatementsProcessed = 0;
			int curStmtIdx=0;//starting index to display on progress bar
			//TODO: Query the database to get an updated list of unsent bills and compare them to the local list to make sure that we do not resend statements that have already been sent by another user.
			while(numOfBatchesSent<=numOfBatchesTotal) {
				if(!BillingProgressPause()) {
					return;
				}
				curStatementsProcessed = 0;
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Preparing Batch")+" "+numOfBatchesSent
					,progressBarEventType:ProgBarEventType.TextMsg)));
				dictEbills.Clear();
				int pdfsToPrint = 0;
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Calculating # of PDFs to print")+"..."
					,progressBarEventType:ProgBarEventType.TextMsg)));
				foreach(Statement stmt in dictStatementsForSend[numOfBatchesSent]) {
					if(stmt==null) {//The statement was probably deleted by another user.
						continue;
					}
					string billingType = PrefC.GetString(PrefName.BillingUseElectronic);
					if(stmt.Mode_==StatementMode.Electronic && (billingType=="1" || billingType=="3") && !PrefC.GetBool(PrefName.BillingElectCreatePDF)) {
						//Do not create a pdf
					}
					else {
						pdfsToPrint++;
					}
				}
				//Now to print, send eBills, and text messages.  If any return false, the user canceled during execution.
				if(!PrintBatch(pdfsToPrint,maxStmtsPerBatch,numOfBatchesSent,ref errorMsg,ref skipped,ref skippedElect
						,ref dictSkippedElect,ref emailed,ref printed,ref dictEbills,ref outputDocument,ref pd,ref curStmtIdx
						,ref curStatementsProcessed,ref skippedDeleted,ref dictStatementsForSend)
					|| !SendEBills(maxStmtsPerBatch,numOfBatchesSent,ref errorMsg,ref sentElect,ref skippedElect,ref dictEbills
						,ref curStmtIdx,ref curStatementsProcessed,ref skippedDeleted,ref dictStatementsForSend)
					|| !SendTextMessages(numOfBatchesSent,ref errorMsg,ref dictStatementsForSend,ref texted)) 
				{
					return;
				}
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch Completed")+"..."
					,progressBarEventType:ProgBarEventType.TextMsg)));
				numOfBatchesSent++;
			}//End while loop
		}

		///<summary>Concat all the pdf's together to create one print job.
		///Returns false if the printing was canceled</summary>
		public bool PrintBatch(int pdfsToPrint,int maxStmtsPerBatch,int numOfBatchesSent,ref string errorMsg,ref int skipped,ref int skippedElect
			,ref Dictionary<string,int> dictSkippedElect,ref int emailed,ref int printed,ref Dictionary<long,List<EbillStatement>> dictEbills
			,ref PdfDocument outputDocument,ref PrintDocument pd,ref int curStmtIdx,ref int curStatementsProcessed,ref int skippedDeleted
			,ref Dictionary<int,List<Statement>> dictStatementsForSend) 
		{
			FormRpStatement FormST = new FormRpStatement();
			Random rnd;
			string fileName;
			string filePathAndName;
			string attachPath;
			EmailMessage message;
			EmailAttach attach;
			EmailAddress emailAddress;
			Patient pat;
			string patFolder;
			PdfDocument inputDocument;
			PdfPage page;
			string savedPdfPath;
			DataSet dataSet;
			int curStatementsInBatch = 0;
			int pdfsPrinted = 0;
			bool isComputeAging=true;//will be false if AgingIsEnterprise and aging was calculated for today already (or successfully runs for today)
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(PrefC.GetDate(PrefName.DateLastAging).Date!=MiscData.GetNowDateTime().Date && !RunAgingEnterprise()) {//run aging for all patients
					return false;//if aging fails, don't generate and print statements
				}
				isComputeAging=false;
			}
			foreach(Statement stmt in dictStatementsForSend[numOfBatchesSent]) {
				if(!BillingProgressPause()) {
					return false;
				}
				curStmtIdx++;
				if(stmt==null) {//The statement was probably deleted by another user.
					skippedDeleted++;
					curStatementsProcessed++;
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
						+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
					continue;
				}
				if(curStatementsInBatch==maxStmtsPerBatch) {
					break;
				}
				curStatementsInBatch++;
				if(_listStatementNumsToSkip.Contains(stmt.StatementNum)) {
					skipped++;
					curStatementsProcessed++;
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
						+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
					continue;
				}
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"5%",5,100,ProgBarStyle.Blocks,"3")));
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Generating Single PDFs")+"..."
					,progressBarEventType:ProgBarEventType.TextMsg)));
				Family fam=null;
				if(!_dictFams.TryGetValue(stmt.PatNum,out fam)) {
					fam=Patients.GetFamily(stmt.PatNum);
				}
				pat=fam.GetPatient(stmt.PatNum);
				patFolder=ImageStore.GetPatientFolder(pat,ImageStore.GetPreferredAtoZpath());
				dataSet=AccountModules.GetStatementDataSet(stmt,isComputeAging);
				if(comboEmailFrom.SelectedIndex==0) { //clinic/practice default
					emailAddress=EmailAddresses.GetByClinic(pat.ClinicNum);
				}
				else { //me or static email address, email address for 'me' is the first one in _listEmailAddresses
					emailAddress=_listEmailAddresses[comboEmailFrom.SelectedIndex-1];//-1 to account for predefined "Clinic/Practice" item in combobox
				}
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"10%",10,100,ProgBarStyle.Blocks,"3")));
				if(stmt.Mode_==StatementMode.Email) {
					if(emailAddress.SMTPserver=="") {
						_progExtended.Close();
						MsgBox.Show(this,"You need to enter an SMTP server name in e-mail setup before you can send e-mail.");
						Cursor=Cursors.Default;
						isPrinting=false;
						//FillGrid();//automatic
						return false;
					}
					if(pat.Email=="") {
						skipped++;
						curStatementsProcessed++;
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
							+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						continue;
					}
				}
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"15%",15,100,ProgBarStyle.Blocks,"3")));
				stmt.IsSent=true;
				stmt.DateSent=DateTimeOD.Today;
				#region Print PDFs
				string billingType = PrefC.GetString(PrefName.BillingUseElectronic);
				string tempPdfFile="";
				if(stmt.Mode_==StatementMode.Electronic && (billingType=="1" || billingType=="3") && !PrefC.GetBool(PrefName.BillingElectCreatePDF)) {
					//Do not create a pdf
				}
				else {
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"100%",100,100,ProgBarStyle.Blocks,"3")));
					try {
						if(PrefC.GetBool(PrefName.StatementsUseSheets)) {
							tempPdfFile=FormST.CreateStatementPdfSheets(stmt,pat,fam,dataSet);
						}
						else {
							tempPdfFile=FormST.CreateStatementPdfClassic(stmt,pat,fam,dataSet);
						}
					}
					catch(Exception ex) {
						errorMsg+=Lan.g(this,"Error creating PDF for PatNum")+": "+pat.PatNum+Environment.NewLine+ex.ToString()+Environment.NewLine;
						skipped++;
						curStatementsProcessed++;
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),
							Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,
							ProgBarStyle.Blocks,"1")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
							+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",
							curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						continue;
					}
					pdfsPrinted++;
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"100%",100,100,ProgBarStyle.Blocks,"3")));
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"PDF Created")+"..."
						,progressBarEventType:ProgBarEventType.TextMsg)));
					if(stmt.DocNum==0) {
						_progExtended.Close();
						MsgBox.Show(this,"Failed to save PDF.  In Setup, DataPaths, please make sure the top radio button is checked.");
						Cursor=Cursors.Default;
						isPrinting=false;
						return false;
					}
				}
				//imageStore = OpenDental.Imaging.ImageStore.GetImageStore(pat);
				//If stmt.DocNum==0, savedPdfPath will be "".  A blank savedPdfPath is fine for electronic statements.
				if(CloudStorage.IsCloudStorage) {
					if(tempPdfFile != "")
						savedPdfPath=tempPdfFile;//To save time by not having to download it.
					else {
						savedPdfPath=PrefC.GetRandomTempFile("pdf");
						FileAtoZ.Copy(ImageStore.GetFilePath(Documents.GetByNum(stmt.DocNum),patFolder),savedPdfPath,FileAtoZSourceDestination.AtoZToLocal,uploadMessage:"Downloading statement...");
					}
				}
				else {
					savedPdfPath=ImageStore.GetFilePath(Documents.GetByNum(stmt.DocNum),patFolder);
				}
				if(stmt.Mode_==StatementMode.InPerson || stmt.Mode_==StatementMode.Mail) {
					if(pd==null) {
						pd=new PrintDocument();
					}
					inputDocument=PdfReader.Open(savedPdfPath,PdfDocumentOpenMode.Import);
					for(int idx = 0;idx<inputDocument.PageCount;idx++) {
						page=inputDocument.Pages[idx];
						outputDocument.AddPage(page);
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,(((idx/inputDocument.PageCount)*85)+15)+"%",((idx/inputDocument.PageCount)*85)+15,100,ProgBarStyle.Blocks,"3")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"PDF Added to Print List")+"..."
							,progressBarEventType:ProgBarEventType.TextMsg)));
					}
					curStatementsProcessed++;
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
						+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
					printed++;
					labelPrinted.Text=Lan.g(this,"Printed=")+printed.ToString();
					Application.DoEvents();
					_listStatementNumsSent.Add(stmt.StatementNum);
					Statements.MarkSent(stmt.StatementNum,stmt.DateSent);
				}
				#endregion
				#region Preparing Email
				if(stmt.Mode_==StatementMode.Email) {
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Preparing Email")+"..."
						,progressBarEventType:ProgBarEventType.TextMsg)));
					attachPath=EmailAttaches.GetAttachPath();
					rnd=new Random();
					fileName=DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()+".pdf";
					filePathAndName=FileAtoZ.CombinePaths(attachPath,fileName);
					FileAtoZ.Copy(savedPdfPath,filePathAndName,FileAtoZSourceDestination.LocalToAtoZ,uploadMessage:"Uploading statement...");
					//Process.Start(filePathAndName);
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"40%",40,100,ProgBarStyle.Blocks,"3")));
					message=Statements.GetEmailMessageForStatement(stmt,pat);
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"70%",70,100,ProgBarStyle.Blocks,"3")));
					attach=new EmailAttach();
					attach.DisplayedFileName="Statement.pdf";
					attach.ActualFileName=fileName;
					message.Attachments.Add(attach);
					try {
						//If IsCloudStorage==true, then we will end up downloading the file again in EmailMessages.SendEmailUnsecure.
						EmailMessages.SendEmailUnsecure(message,emailAddress);
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"90%",90,100,ProgBarStyle.Blocks,"3")));
						message.SentOrReceived=EmailSentOrReceived.Sent;
						message.MsgDateTime=DateTime.Now;
						EmailMessages.Insert(message);
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"95%",95,100,ProgBarStyle.Blocks,"3")));
						emailed++;
						curStatementsProcessed++;
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
							+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Email Sent")+"...",
							progressBarEventType:ProgBarEventType.TextMsg)));
						labelEmailed.Text=Lan.g(this,"E-mailed=")+emailed.ToString();
						Application.DoEvents();
					}
					catch {
						//Cursor=Cursors.Default;
						//MessageBox.Show(ex.Message);
						//return;
						skipped++;
						curStatementsProcessed++;
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
							+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"100%",100,100,ProgBarStyle.Blocks,"3")));
						continue;
					}
					_listStatementNumsSent.Add(stmt.StatementNum);
					Statements.MarkSent(stmt.StatementNum,stmt.DateSent);
					try {
						File.Delete(tempPdfFile);
					}
					catch(Exception ex) {
						ex.DoNothing();//Will most likely get cleaned up when the user closes OD.
					}
				}
				#endregion
				#region Preparing E-Bills
				if(stmt.Mode_==StatementMode.Electronic) {
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"65%",65,100,ProgBarStyle.Blocks,"3")));
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Preparing E-Bills")+"...",
						progressBarEventType:ProgBarEventType.TextMsg)));
					Patient guar = fam.ListPats[0];
					if(guar.Address.Trim()=="" || guar.City.Trim()=="" || guar.State.Trim()=="" || guar.Zip.Trim()=="") {
						skippedElect++;
						curStatementsProcessed++;
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
							+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						continue;
					}
					EbillStatement ebillStatement = new EbillStatement();
					ebillStatement.family=fam;
					ebillStatement.statement=stmt;
					long clinicNum = 0;//If clinics are disabled, then all bills will go into the same "bucket"
					if(PrefC.HasClinicsEnabled) {
						clinicNum=fam.ListPats[0].ClinicNum;
					}
					List<string> listElectErrors = new List<string>();
					if(PrefC.GetString(PrefName.BillingUseElectronic)=="1") {//EHG
						listElectErrors=Bridges.EHG_statements.Validate(clinicNum);
					}
					if(listElectErrors.Count > 0) {
						foreach(string errorElect in listElectErrors) {
							if(!dictSkippedElect.ContainsKey(errorElect)) {
								dictSkippedElect.Add(errorElect,0);
							}
							dictSkippedElect[errorElect]++;
						}
						curStatementsProcessed++;
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
						_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
							+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						continue;//skip the current statement, since there are errors.
					}
					if(!dictEbills.ContainsKey(clinicNum)) {
						dictEbills.Add(clinicNum,new List<EbillStatement>());
					}
					dictEbills[clinicNum].Add(ebillStatement);
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"70%",70,100,ProgBarStyle.Blocks,"3")));
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"E-Bill Added To Send List")+"..."
						,progressBarEventType:ProgBarEventType.TextMsg)));
				}
				#endregion
			}
			return true;
		}

		///<summary>Attempt to send electronic bills if needed.
		///Returns false if the sending was canceled</summary>
		private bool SendEBills(int maxStmtsPerBatch,int numOfBatchesSent,ref string errorMsg,ref int sentElect,ref int skippedElect
			,ref Dictionary<long,List<EbillStatement>> dictEbills,ref int curStmtIdx,ref int curStatementsProcessed,ref int skippedDeleted
			,ref Dictionary<int,List<Statement>> dictStatementsForSend) 
		{
			//Attempt to send electronic bills if needed------------------------------------------------------------
			Family fam;
			Patient pat;
			DataSet dataSet;
			bool isComputeAging=true;//will be false if AgingIsEnterprise and aging was calculated for today already (or successfully runs for today)
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(PrefC.GetDate(PrefName.DateLastAging).Date!=MiscData.GetNowDateTime().Date && !RunAgingEnterprise()) {//run aging for all patients
					return false;//if aging fails, don't generate and print statements
				}
				isComputeAging=false;
			}
			_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"80%",80,100,ProgBarStyle.Blocks,"3")));
			foreach(KeyValuePair<long,List<EbillStatement>> entryForClinic in dictEbills) {//Go through the dictionary entries
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Sending E-Bills")+"..."
					,progressBarEventType:ProgBarEventType.TextMsg)));
				if(!BillingProgressPause()) {
					return false;
				}
				List<EbillStatement> listClinicStmts = entryForClinic.Value;
				int maxNumOfBatches = listClinicStmts.Count;//Worst case scenario is number of statements total.
				maxStmtsPerBatch = PrefC.GetInt(PrefName.BillingElectBatchMax);
				if(maxStmtsPerBatch==0) {
					maxStmtsPerBatch=listClinicStmts.Count;//Make the batch size equal to the list of statements so that we send them all at once.
				}
				XmlWriterSettings xmlSettings = new XmlWriterSettings();
				xmlSettings.OmitXmlDeclaration=true;
				xmlSettings.Encoding=Encoding.UTF8;
				xmlSettings.Indent=true;
				xmlSettings.IndentChars="   ";
				//Loop through all electronic bills and try to send them in batches.  Each batch size will be dictated via maxNumOfBatches.
				//At this point we know we will have at least one batch to send so we start batchNum to 1.
				for(int batchNum = 1;batchNum<=maxNumOfBatches;batchNum++) {
					if(listClinicStmts.Count==0) {//All statements have been sent for the current clinic.  Nothing more to do.
						break;
					}
					StringBuilder strBuildElect = new StringBuilder();
					XmlWriter writerElect = XmlWriter.Create(strBuildElect,xmlSettings);
					List<long> listElectStmtNums = new List<long>();
					if(PrefC.GetString(PrefName.BillingUseElectronic)=="1") {
						Bridges.EHG_statements.GeneratePracticeInfo(writerElect,entryForClinic.Key);
					}
					else if(PrefC.GetString(PrefName.BillingUseElectronic)=="2") {
						Bridges.POS_statements.GeneratePracticeInfo(writerElect,entryForClinic.Key);
					}
					else if(PrefC.GetString(PrefName.BillingUseElectronic)=="3") {
						Bridges.ClaimX_Statements.GeneratePracticeInfo(writerElect,entryForClinic.Key);
					}
					else if(PrefC.GetString(PrefName.BillingUseElectronic)=="4") {
						Bridges.EDS_Statements.GeneratePracticeInfo(writerElect,entryForClinic.Key);
					}
					int stmtCountCur = 0;
					//Generate the statements for each batch.
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"85%",85,100,ProgBarStyle.Blocks,"3")));
					for(int j = listClinicStmts.Count-1;j>=0;j--) {//Construct the string for sending this clinic's ebills
						if(!BillingProgressPause()) {
							return false;
						}
						Statement stmtCur = listClinicStmts[j].statement;
						if(stmtCur==null) {//The statement was probably deleted by another user.
							skippedDeleted++;
							curStatementsProcessed++;
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
								+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
							continue;
						}
						if(_listStatementNumsToSkip.Contains(stmtCur.StatementNum)) {
							skippedElect++;
							curStatementsProcessed++;
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
								+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
							continue;
						}
						fam=listClinicStmts[j].family;
						listClinicStmts.RemoveAt(j);//Remove the statement from our list so that we do not send it again in the next batch.
						pat=fam.GetPatient(stmtCur.PatNum);
						dataSet=AccountModules.GetStatementDataSet(stmtCur,isComputeAging);
						bool statementWritten = true;
						try {
							//Write the statement into a temporary string builder, so that if the statement fails to generate (due to exception),
							//then the partially generated statement will not be added to the strBuildElect.
							StringBuilder strBuildStatement = new StringBuilder();
							using(XmlWriter writerStatement = XmlWriter.Create(strBuildStatement,writerElect.Settings)) {
								if(PrefC.GetString(PrefName.BillingUseElectronic)=="0") {
									throw new Exception(Lan.g(this,"\'No billing electronic\' is currently selected in Billing Defaults."));
								}
								else if(PrefC.GetString(PrefName.BillingUseElectronic)=="1") {
									OpenDental.Bridges.EHG_statements.GenerateOneStatement(writerStatement,stmtCur,pat,fam,dataSet);
								}
								else if(PrefC.GetString(PrefName.BillingUseElectronic)=="2") {
									OpenDental.Bridges.POS_statements.GenerateOneStatement(writerStatement,stmtCur,pat,fam,dataSet);
								}
								else if(PrefC.GetString(PrefName.BillingUseElectronic)=="3") {
									OpenDental.Bridges.ClaimX_Statements.GenerateOneStatement(writerStatement,stmtCur,pat,fam,dataSet);
								}
								else if(PrefC.GetString(PrefName.BillingUseElectronic)=="4") {
									Bridges.EDS_Statements.GenerateOneStatement(writerStatement,stmtCur,pat,fam,dataSet);
								}
							}
							//Write this statement's XML to the XML document with all the statements.
							using(XmlReader readerStatement = XmlReader.Create(new StringReader(strBuildStatement.ToString()))) {
								writerElect.WriteNode(readerStatement,true);
							}
						}
						catch(Exception ex) {
							errorMsg+=Lan.g(this,"Error sending statement for PatNum")+": "+pat.PatNum+Environment.NewLine+ex.ToString()+Environment.NewLine;
							statementWritten=false;
						}
						if(statementWritten) {
							listElectStmtNums.Add(stmtCur.StatementNum);
							sentElect++;
							stmtCountCur++;
						}
						if(stmtCountCur==maxStmtsPerBatch) {
							break;
						}
					}
					if(stmtCountCur==0) {//All statements for this batch were either deleted or had an exception thrown while generating.
						continue;//Go on to next batch
					}
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"90%",90,100,ProgBarStyle.Blocks,"3")));
					if(PrefC.GetString(PrefName.BillingUseElectronic)=="1") {
						writerElect.Close();
						for(int attempts = 0;attempts<3;attempts++) {
							try {
								Bridges.EHG_statements.Send(strBuildElect.ToString(),entryForClinic.Key);
								//loop through all statements in the batch and mark them sent
								for(int i = 0;i<listElectStmtNums.Count;i++) {
									_listStatementNumsSent.Add(listElectStmtNums[i]);
									Statements.MarkSent(listElectStmtNums[i],DateTimeOD.Today);
									curStatementsProcessed++;
									_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"100%",100,100,ProgBarStyle.Blocks,"3")));
									_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
									_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
										+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
								}
								break;//At this point the batch was successfully sent so there is no need to loop through additional attempts.
							}
							catch(Exception ex) {
								if(attempts<2) {//Don't indicate the error unless it failed on the last attempt.
									continue;//The only thing skipped besides the error message is evaluating if the statement was written, which is wasn't.
								}
								sentElect-=listElectStmtNums.Count;
								errorMsg+=Clinics.GetAbbr(entryForClinic.Key)+": "+ex.Message;
								if(ex.Message.Contains("(404) Not Found")) {
									//The full error is "The remote server returned an error: (404) Not Found."  We convert the message into a more user friendly message.
									errorMsg+=Lan.g(this,"The connection to the server could not be established or was lost, or the upload timed out.  "
									+"Ensure your internet connection is working and that your firewall is not blocking this application.  "
									+"If the upload timed out after 10 minutes, try sending 25 statements or less in each batch to reduce upload time.");
								}
							}
							//This occurs so we can count unsent bills in the overall.
							curStatementsProcessed++;
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
								+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						}
					}
					if(PrefC.GetString(PrefName.BillingUseElectronic)=="2") {
						writerElect.Close();
						string filePath=PrefC.GetString(PrefName.BillingElectStmtOutputPathPos);
						if(Directory.Exists(filePath)) {
							filePath=ODFileUtils.CombinePaths(filePath,"Statements.xml");
						}
						else {
							//Only bring up save dialog if path is invalid
							//MessageBox.Show(this,Lan.g(this,"Billing Defaults Output Path value is invalid.\r\nPlease specify the path to save the file to."));
							SaveFileDialog dlg = new SaveFileDialog();
							dlg.FileName="Statements.xml";
							dlg.CheckPathExists=true;
							if(dlg.ShowDialog()!=DialogResult.OK) {
								sentElect-=listElectStmtNums.Count;
							}
							filePath=dlg.FileName;
						}
						File.WriteAllText(filePath,strBuildElect.ToString());
						for(int i = 0;i<listElectStmtNums.Count;i++) {
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"10%",10,100,ProgBarStyle.Blocks,"3")));
							_listStatementNumsSent.Add(listElectStmtNums[i]);
							Statements.MarkSent(listElectStmtNums[i],DateTimeOD.Today);
							curStatementsProcessed++;
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"100%",100,100,ProgBarStyle.Blocks,"3")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
								+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed /dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						}
					}
					if(PrefC.GetString(PrefName.BillingUseElectronic)=="3") {
						writerElect.Close();
						SaveFileDialog dlg = new SaveFileDialog();
						dlg.InitialDirectory=@"C:\StatementX\";//Clint from ExtraDent requested this default path.
						if(!Directory.Exists(dlg.InitialDirectory)) {
							try {
								Directory.CreateDirectory(dlg.InitialDirectory);
							}
							catch { }
						}
						dlg.FileName="Statements.xml";
						if(dlg.ShowDialog()!=DialogResult.OK) {
							sentElect-=listElectStmtNums.Count;
						}
						File.WriteAllText(dlg.FileName,strBuildElect.ToString());
						for(int i = 0;i<listElectStmtNums.Count;i++) {
							_listStatementNumsSent.Add(listElectStmtNums[i]);
							Statements.MarkSent(listElectStmtNums[i],DateTimeOD.Today);
							curStatementsProcessed++;
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"100%",100,100,ProgBarStyle.Blocks,"3")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
								+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed/dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						}
					}
					if(PrefC.GetString(PrefName.BillingUseElectronic)=="4") {
						writerElect.Close();
						string filePath=PrefC.GetString(PrefName.BillingElectStmtOutputPathEds);
						if(Directory.Exists(filePath)) {
							filePath=ODFileUtils.CombinePaths(filePath,"Statements.xml");
						}
						else {
							MessageBox.Show(this,Lan.g(this,"Billing Defaults Output Path value is invalid.\r\nPlease specify the path to save the file to."));
							SaveFileDialog dlg=new SaveFileDialog();
							dlg.FileName="Statements.xml";
							dlg.CheckPathExists=true;
							if(dlg.ShowDialog()!=DialogResult.OK) {
								sentElect-=listElectStmtNums.Count;
							}
							filePath=dlg.FileName;
						}
						File.WriteAllText(filePath,strBuildElect.ToString());
						for(int i=0;i<listElectStmtNums.Count;i++) {
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"10%",10,100,ProgBarStyle.Blocks,"3")));
							_listStatementNumsSent.Add(listElectStmtNums[i]);
							Statements.MarkSent(listElectStmtNums[i],DateTimeOD.Today);
							curStatementsProcessed++;
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Statement")+"\r\n"+curStmtIdx+" / "+gridBill.SelectedIndices.Length,"100%",100,100,ProgBarStyle.Blocks,"3")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Overall"),Math.Ceiling(((double)curStmtIdx/gridBill.SelectedIndices.Length)*100)+"%",curStmtIdx,gridBill.SelectedIndices.Length,ProgBarStyle.Blocks,"1")));
							_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Batch")+"\r\n"+numOfBatchesSent+" / "
								+dictStatementsForSend.Count,Math.Ceiling(((double)curStatementsProcessed /dictStatementsForSend[numOfBatchesSent].Count)*100)+"%",curStatementsProcessed,dictStatementsForSend[numOfBatchesSent].Count,ProgBarStyle.Blocks,"2")));
						}
					}
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"E-Bills Sent")+"..."
						,progressBarEventType:ProgBarEventType.TextMsg)));
					labelSentElect.Text=Lan.g(this,"SentElect=")+sentElect.ToString();
					Application.DoEvents();
				}
			}
			return true;
		}

		///<summary>Sends text messages to the current batch of statements.</summary>
		private bool SendTextMessages(int numOfBatchesSent,ref string errorMsg,ref Dictionary<int,List<Statement>> dictStatementsForSend,ref int texted) {
			List<SmsToMobile> listTextsToSend=new List<SmsToMobile>();
			List<long> listStmtNumsToUpdate=new List<long>();
			Dictionary<long,PatComm> dictPatComms=Patients.GetPatComms(_dictFams.Values.SelectMany(x => x.ListPats).DistinctBy(x => x.PatNum).ToList())
				.ToDictionary(x => x.PatNum,y => y);
			string guidBatch=null;
			foreach(Statement stmt in dictStatementsForSend[numOfBatchesSent]) {
				if(!BillingProgressPause()) {
					return false;
				}
				if(stmt.SmsSendStatus!=AutoCommStatus.SendNotAttempted) {
					continue;
				}
				PatComm patComm;
				if(!dictPatComms.TryGetValue(stmt.PatNum,out patComm)) {
					errorMsg+=Lan.g(this,"Unable to find patient communication method for PatNum")+": "+stmt.PatNum+Environment.NewLine;
					continue;
				}
				if(!patComm.IsSmsAnOption) {
					continue;
				}
				Family fam;
				if(!_dictFams.TryGetValue(stmt.PatNum,out fam)) {
					fam=Patients.GetFamily(stmt.PatNum);
				}
				Patient pat=fam.GetPatient(stmt.PatNum);
				SmsToMobile textToSend=new SmsToMobile {
					ClinicNum=pat.ClinicNum,
					GuidMessage=Guid.NewGuid().ToString(),
					IsTimeSensitive=false,
					MobilePhoneNumber=patComm.SmsPhone,
					PatNum=stmt.PatNum,
					MsgText=Statements.ReplaceVarsForSms(PrefC.GetString(PrefName.BillingDefaultsSmsTemplate),pat,stmt),
					MsgType=SmsMessageSource.Statements,
				};
				guidBatch=guidBatch??textToSend.GuidMessage;
				textToSend.GuidBatch=guidBatch;
				listTextsToSend.Add(textToSend);
				listStmtNumsToUpdate.Add(stmt.StatementNum);
			}
			if(!BillingProgressPause()) {
				return false;
			}
			if(listTextsToSend.Count==0) {
				return true;
			}
			try {
				_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Sending text messages")+"..."
					,progressBarEventType: ProgBarEventType.TextMsg)));
				SmsToMobiles.SendSmsMany(listTextsToSend,user: Security.CurUser);
				Statements.UpdateSmsSendStatus(listStmtNumsToUpdate,AutoCommStatus.SendSuccessful);
				texted+=listTextsToSend.Count;
				labelTexted.Text=Lan.g(this,"Texted=")+texted;
				Application.DoEvents();
			}
			catch(Exception ex) {
				errorMsg+=Lan.g(this,"Error sending text messages")+": "+Environment.NewLine+ex.Message+Environment.NewLine;
				Statements.UpdateSmsSendStatus(listStmtNumsToUpdate,AutoCommStatus.SendFailed);
			}
			return true;
		}

		///<summary>Sets the installment plans field on each of the statements passed in.</summary>
		private void AddInstallmentPlansToStatements(List<Statement> listStatements) {
			Dictionary<long,List<InstallmentPlan>> dictSuperFamInstallmentPlans=InstallmentPlans.GetForSuperFams(
				listStatements.Where(x => x.SuperFamily > 0)
					.Select(x => _dictFams[x.PatNum].Guarantor.SuperFamily).ToList());
			Dictionary<long,InstallmentPlan> dictFamInstallmentPlans=InstallmentPlans.GetForFams(
				listStatements.Where(x => x.SuperFamily==0)
					.Select(x => _dictFams[x.PatNum].Guarantor.PatNum).ToList());
			foreach(Statement stmt in listStatements) {
				if(stmt.SuperFamily > 0) {
					if(!dictSuperFamInstallmentPlans.TryGetValue(_dictFams[stmt.PatNum].Guarantor.SuperFamily,out stmt.ListInstallmentPlans)) {
						stmt.ListInstallmentPlans=new List<InstallmentPlan>();
					}
				}
				else if(dictFamInstallmentPlans.ContainsKey(_dictFams[stmt.PatNum].Guarantor.PatNum)) {
					stmt.ListInstallmentPlans=new List<InstallmentPlan> { dictFamInstallmentPlans[_dictFams[stmt.PatNum].Guarantor.PatNum] };
				}
				else {
					stmt.ListInstallmentPlans=new List<InstallmentPlan>();
				}
			}
		}

		private bool RunAgingEnterprise() {
			DateTime dtNow=MiscData.GetNowDateTime();
			DateTime dtToday=dtNow.Date;
			DateTime dateLastAging=PrefC.GetDate(PrefName.DateLastAging);
			if(dateLastAging.Date==dtToday) {
				return true;//already ran aging for this date, just move on
			}
			Prefs.RefreshCache();
			DateTime dateTAgingBeganPref=PrefC.GetDateT(PrefName.AgingBeginDateTime);
			if(dateTAgingBeganPref>DateTime.MinValue) {
				MessageBox.Show(this,Lan.g(this,"In order to print or send statments, aging must be re-calculated, but you cannot run aging until it has "
					+"finished the current calculations which began on")+" "+dateTAgingBeganPref.ToString()+".\r\n"+Lans.g(this,"If you believe the current "
					+"aging process has finished, a user with SecurityAdmin permission can manually clear the date and time by going to Setup | Miscellaneous "
					+"and pressing the 'Clear' button."));
				return false;
			}
			Prefs.UpdateString(PrefName.AgingBeginDateTime,POut.DateT(dtNow,false));//get lock on pref to block others
			Signalods.SetInvalid(InvalidType.Prefs);//signal a cache refresh so other computers will have the updated pref as quickly as possible
			Action actionCloseAgingProgress=null;
			Cursor=Cursors.WaitCursor;
			try {
				actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ComputeAging",this,
					Lan.g(this,"Calculating enterprise aging for all patients as of")+" "+dtToday.ToShortDateString()+"...");
				Ledgers.ComputeAging(0,dtToday);
				Prefs.UpdateString(PrefName.DateLastAging,POut.Date(dtToday,false));
			}
			catch(MySqlException ex) {
				actionCloseAgingProgress?.Invoke();//terminates progress bar
				Cursor=Cursors.Default;
				if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
					throw;
				}
				MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
				return false;
			}
			finally {
				actionCloseAgingProgress?.Invoke();//terminates progress bar
				Cursor=Cursors.Default;
				Prefs.UpdateString(PrefName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
				Signalods.SetInvalid(InvalidType.Prefs);
			}
			return true;
		}

		///<summary>Returns true unless the user clicks cancel in the progress window or the list has changed.  The method will wait infinitely if paused from the progress window.</summary>
		public bool BillingProgressPause() {
			//Check to see if the user wants to pause the sending of statements.  If so, wait until they decide to resume.
			List<long> listStatementNumsUnsent=new List<long>();
			_listStatementNumsToSkip=new List<long>();
			bool hasEventFired=false;
			DateTime dateFrom=PIn.Date(textDateStart.Text);
			DateTime dateTo=new DateTime(2200,1,1);
			List<long> clinicNums=new List<long>();//an empty list indicates to Statements.GetBilling to run for all clinics
			while(_progExtended.IsPaused) {
				if(!hasEventFired) {//Don't fire this event more than once.
					_progExtended.Fire(new ODEventArgs("BillingEvent",new ProgressBarHelper(Lan.g(this,"Warning")
						,progressBarEventType:ProgBarEventType.WarningOff)));
					hasEventFired=true;
				}
				Thread.Sleep(100);
				if(!_progExtended.IsPaused) {
					List<long> listStatementNumsSelected=gridBill.Rows.OfType<ODGridRow>()
						.Select(x =>PIn.Long(((DataRow)x.Tag)["StatementNum"].ToString())).ToList();
					listStatementNumsUnsent=listStatementNumsSelected.FindAll(x => !_listStatementNumsSent.Contains(x)).ToList();
					dateFrom=PIn.Date(textDateStart.Text);
					dateTo=new DateTime(2200,1,1);
					if(textDateEnd.Text!=""){
						dateTo=PIn.Date(textDateEnd.Text);
					}
					if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex>0) {
						clinicNums.Add(ListClinics[comboClinic.SelectedIndex-1].ClinicNum);
					}
					DataTable tableCur=Statements.GetBilling(radioSent.Checked,comboOrder.SelectedIndex,dateFrom,dateTo,clinicNums);
					List<long> listStatementNums=tableCur.Select().Select(x => PIn.Long(x["StatementNum"].ToString())).ToList();
					foreach(long stmtNum in listStatementNumsUnsent) {
						if(!listStatementNums.Contains(stmtNum)) {
							_listStatementNumsToSkip.Add(stmtNum);
						}
					}
				}
				if(_progExtended.IsCanceled) {
					return false;
				}
			}
			//Check to see if the user wants to stop sending statements.
			if(_progExtended.IsCanceled) {
				return false;
			}
			return true;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			if(gridBill.Rows.Count>0){
				_isActivateFillDisabled=true;
				DialogResult result=MessageBox.Show(Lan.g(this,"You may leave this window open while you work.  If you do close it, do you want to delete all unsent bills?"),
					"",MessageBoxButtons.YesNoCancel);
				if(result==DialogResult.Yes){
					List<long> listStmtNumsToDelete=gridBill.Rows.OfType<ODGridRow>().Select(x => (DataRow)x.Tag)
						.Where(x => x["IsSent"].ToString()=="0")
						.Select(x => PIn.Long(x["StatementNum"].ToString())).ToList();
					Statements.DeleteAll(listStmtNumsToDelete);
					//This is not an accurate permission type.
					SecurityLogs.MakeLogEntry(Permissions.Accounting,0,"Billing: Unsent statements were deleted.");
					MessageBox.Show(Lan.g(this,"Unsent statements deleted: ")+listStmtNumsToDelete.Count);
				}
				else if(result==DialogResult.No){
					DialogResult=DialogResult.Cancel;
					Close();
				}
				else{//cancel
					_isActivateFillDisabled=false;
					return;
				}
			}
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void butDefaults_Click(object sender,EventArgs e) {
			FormBillingDefaults formBD = new FormBillingDefaults();
			formBD.IsUserPassOnly=true;
			formBD.ShowDialog();
		}

		///// <summary></summary>
		//private void butSendEbill_Click(object sender,EventArgs e) {
		//	if (gridBill.SelectedIndices.Length == 0){
		//		MessageBox.Show(Lan.g(this, "Please select items first."));
		//		return;
		//	}
		//	Cursor.Current = Cursors.WaitCursor;
		//	// Populate Array And Open eBill Form
		//	ArrayList PatientList = new ArrayList();
		//	for (int i = 0; i < gridBill.SelectedIndices.Length; i++)
		//			PatientList.Add(PIn.Long(table.Rows[gridBill.SelectedIndices[i]]["PatNum"].ToString()));
		//	// Open eBill form
		//	FormPatienteBill FormPatienteBill = new FormPatienteBill(PatientList); 
		//	FormPatienteBill.ShowDialog();
		//	Cursor.Current = Cursors.Default;
		//}

		private delegate void WarningCallback();

	}

	public struct EbillStatement {

		public Statement statement;
		public Family family;

	}

}


















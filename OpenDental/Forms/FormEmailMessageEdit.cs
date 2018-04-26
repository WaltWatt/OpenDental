using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental{
	public class FormEmailMessageEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.IContainer components;
		private OpenDental.UI.Button butSend;
		private OpenDental.UI.Button butDeleteTemplate;
		private OpenDental.UI.Button butAddTemplate;
		private System.Windows.Forms.Label labelTemplate;
		private System.Windows.Forms.ListBox listTemplates;
		private OpenDental.UI.Button butInsertTemplate;
		private UI.Button butRefresh;
		public bool IsNew;
    private bool _hasTemplatesChanged;
    private System.Windows.Forms.Panel panelTemplates;
		private OpenDental.UI.Button butSave;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button buttonFuchsMailDSF;
		private OpenDental.UI.Button buttonFuchsMailDMF;
		private EmailMessage _emailMessage;
		private Label labelDecrypt;
		private UI.Button butDecrypt;
		private UI.Button butDirectMessage;
		private UI.Button butRawMessage;
		private Panel panelAutographs;
		private UI.Button butInsertAutograph;
		private UI.Button butDeleteAutograph;
		private UI.Button butAddAutograph;
		private Label label2;
		private ListBox listAutographs;
		private UI.Button butEditTemplate;
		private UI.Button butEditAutograph;
    private EmailPreviewControl emailPreview;
    private bool _isDeleteAllowed=true;
    public bool HasEmailChanged;
		///<summary>List of email messages to be considered for auto complete email address popup. When null will run query.</summary>
		private List<EmailMessage> _listAllEmailMessages=null;
		private List<EmailAutograph> _listEmailAutographs;
		private List<EmailTemplate> _listEmailTemplates;
		private ToolTip toolTipMessage;

		///<summary>isDeleteAllowed defines whether the email is able to be deleted when a patient is attached. 
		///Currently, emails that are "Deleted" from the inbox are actually just hidden if they have a patient attached.</summary>
		public FormEmailMessageEdit(EmailMessage emailMessage,EmailAddress emailAddressPreview=null,bool isDeleteAllowed=true,params List<EmailMessage>[] listAllEmailMessages){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
      _isDeleteAllowed=isDeleteAllowed;
      _emailMessage=emailMessage.Copy();
			if(emailAddressPreview==null) {
				emailAddressPreview=EmailAddresses.GetByClinic(Clinics.ClinicNum);
			}
			emailPreview.EmailAddressPreview=emailAddressPreview;
			List <List<EmailMessage>> listAllHistoric=listAllEmailMessages.ToList().FindAll(x => x!=null);
			if(listAllHistoric.Count>0) {
				_listAllEmailMessages=listAllEmailMessages.SelectMany(x => x).Where(x => x!=null).ToList();
			}
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailMessageEdit));
			this.labelTemplate = new System.Windows.Forms.Label();
			this.listTemplates = new System.Windows.Forms.ListBox();
			this.panelTemplates = new System.Windows.Forms.Panel();
			this.butEditTemplate = new OpenDental.UI.Button();
			this.butInsertTemplate = new OpenDental.UI.Button();
			this.butDeleteTemplate = new OpenDental.UI.Button();
			this.butAddTemplate = new OpenDental.UI.Button();
			this.labelDecrypt = new System.Windows.Forms.Label();
			this.panelAutographs = new System.Windows.Forms.Panel();
			this.butEditAutograph = new OpenDental.UI.Button();
			this.butInsertAutograph = new OpenDental.UI.Button();
			this.butDeleteAutograph = new OpenDental.UI.Button();
			this.butAddAutograph = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.listAutographs = new System.Windows.Forms.ListBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.butRawMessage = new OpenDental.UI.Button();
			this.butDirectMessage = new OpenDental.UI.Button();
			this.butDecrypt = new OpenDental.UI.Button();
			this.buttonFuchsMailDMF = new OpenDental.UI.Button();
			this.buttonFuchsMailDSF = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butSave = new OpenDental.UI.Button();
			this.butSend = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.emailPreview = new OpenDental.EmailPreviewControl();
			this.toolTipMessage = new System.Windows.Forms.ToolTip(this.components);
			this.panelTemplates.SuspendLayout();
			this.panelAutographs.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelTemplate
			// 
			this.labelTemplate.Location = new System.Drawing.Point(8, 7);
			this.labelTemplate.Name = "labelTemplate";
			this.labelTemplate.Size = new System.Drawing.Size(166, 14);
			this.labelTemplate.TabIndex = 18;
			this.labelTemplate.Text = "E-mail Template";
			this.labelTemplate.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listTemplates
			// 
			this.listTemplates.HorizontalScrollbar = true;
			this.listTemplates.Location = new System.Drawing.Point(10, 26);
			this.listTemplates.Name = "listTemplates";
			this.listTemplates.Size = new System.Drawing.Size(164, 173);
			this.listTemplates.TabIndex = 0;
			this.listTemplates.TabStop = false;
			this.listTemplates.DoubleClick += new System.EventHandler(this.listTemplates_DoubleClick);
			this.listTemplates.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listTemplates_MouseMove);
			// 
			// panelTemplates
			// 
			this.panelTemplates.Controls.Add(this.butEditTemplate);
			this.panelTemplates.Controls.Add(this.butInsertTemplate);
			this.panelTemplates.Controls.Add(this.butDeleteTemplate);
			this.panelTemplates.Controls.Add(this.butAddTemplate);
			this.panelTemplates.Controls.Add(this.labelTemplate);
			this.panelTemplates.Controls.Add(this.listTemplates);
			this.panelTemplates.Location = new System.Drawing.Point(8, 9);
			this.panelTemplates.Name = "panelTemplates";
			this.panelTemplates.Size = new System.Drawing.Size(180, 268);
			this.panelTemplates.TabIndex = 0;
			// 
			// butEditTemplate
			// 
			this.butEditTemplate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditTemplate.Autosize = true;
			this.butEditTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditTemplate.CornerRadius = 4F;
			this.butEditTemplate.Image = global::OpenDental.Properties.Resources.editPencil;
			this.butEditTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditTemplate.Location = new System.Drawing.Point(102, 236);
			this.butEditTemplate.Name = "butEditTemplate";
			this.butEditTemplate.Size = new System.Drawing.Size(75, 26);
			this.butEditTemplate.TabIndex = 19;
			this.butEditTemplate.Text = "Edit";
			this.butEditTemplate.Click += new System.EventHandler(this.butEditTemplate_Click);
			// 
			// butInsertTemplate
			// 
			this.butInsertTemplate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsertTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butInsertTemplate.Autosize = true;
			this.butInsertTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsertTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsertTemplate.CornerRadius = 4F;
			this.butInsertTemplate.Image = global::OpenDental.Properties.Resources.Right;
			this.butInsertTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butInsertTemplate.Location = new System.Drawing.Point(102, 202);
			this.butInsertTemplate.Name = "butInsertTemplate";
			this.butInsertTemplate.Size = new System.Drawing.Size(74, 26);
			this.butInsertTemplate.TabIndex = 2;
			this.butInsertTemplate.Text = "Insert";
			this.butInsertTemplate.Click += new System.EventHandler(this.butInsertTemplate_Click);
			// 
			// butDeleteTemplate
			// 
			this.butDeleteTemplate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeleteTemplate.Autosize = true;
			this.butDeleteTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeleteTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeleteTemplate.CornerRadius = 4F;
			this.butDeleteTemplate.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDeleteTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteTemplate.Location = new System.Drawing.Point(7, 236);
			this.butDeleteTemplate.Name = "butDeleteTemplate";
			this.butDeleteTemplate.Size = new System.Drawing.Size(75, 26);
			this.butDeleteTemplate.TabIndex = 3;
			this.butDeleteTemplate.Text = "Delete";
			this.butDeleteTemplate.Click += new System.EventHandler(this.butDeleteTemplate_Click);
			// 
			// butAddTemplate
			// 
			this.butAddTemplate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddTemplate.Autosize = true;
			this.butAddTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddTemplate.CornerRadius = 4F;
			this.butAddTemplate.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddTemplate.Location = new System.Drawing.Point(7, 202);
			this.butAddTemplate.Name = "butAddTemplate";
			this.butAddTemplate.Size = new System.Drawing.Size(75, 26);
			this.butAddTemplate.TabIndex = 1;
			this.butAddTemplate.Text = "&Add";
			this.butAddTemplate.Click += new System.EventHandler(this.butAddTemplate_Click);
			// 
			// labelDecrypt
			// 
			this.labelDecrypt.Location = new System.Drawing.Point(5, 548);
			this.labelDecrypt.Name = "labelDecrypt";
			this.labelDecrypt.Size = new System.Drawing.Size(267, 59);
			this.labelDecrypt.TabIndex = 31;
			this.labelDecrypt.Text = "Previous attempts to decrypt this message have failed.\r\nDecryption usually fails " +
    "when your private decryption key is not installed on the local computer.\r\nUse th" +
    "e Decrypt button to try again.";
			this.labelDecrypt.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelDecrypt.Visible = false;
			// 
			// panelAutographs
			// 
			this.panelAutographs.Controls.Add(this.butEditAutograph);
			this.panelAutographs.Controls.Add(this.butInsertAutograph);
			this.panelAutographs.Controls.Add(this.butDeleteAutograph);
			this.panelAutographs.Controls.Add(this.butAddAutograph);
			this.panelAutographs.Controls.Add(this.label2);
			this.panelAutographs.Controls.Add(this.listAutographs);
			this.panelAutographs.Location = new System.Drawing.Point(8, 279);
			this.panelAutographs.Name = "panelAutographs";
			this.panelAutographs.Size = new System.Drawing.Size(180, 268);
			this.panelAutographs.TabIndex = 19;
			// 
			// butEditAutograph
			// 
			this.butEditAutograph.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditAutograph.Autosize = true;
			this.butEditAutograph.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditAutograph.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditAutograph.CornerRadius = 4F;
			this.butEditAutograph.Image = global::OpenDental.Properties.Resources.editPencil;
			this.butEditAutograph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditAutograph.Location = new System.Drawing.Point(101, 236);
			this.butEditAutograph.Name = "butEditAutograph";
			this.butEditAutograph.Size = new System.Drawing.Size(75, 26);
			this.butEditAutograph.TabIndex = 20;
			this.butEditAutograph.Text = "Edit";
			this.butEditAutograph.Click += new System.EventHandler(this.butEditAutograph_Click);
			// 
			// butInsertAutograph
			// 
			this.butInsertAutograph.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsertAutograph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butInsertAutograph.Autosize = true;
			this.butInsertAutograph.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsertAutograph.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsertAutograph.CornerRadius = 4F;
			this.butInsertAutograph.Image = global::OpenDental.Properties.Resources.Right;
			this.butInsertAutograph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butInsertAutograph.Location = new System.Drawing.Point(102, 202);
			this.butInsertAutograph.Name = "butInsertAutograph";
			this.butInsertAutograph.Size = new System.Drawing.Size(74, 26);
			this.butInsertAutograph.TabIndex = 2;
			this.butInsertAutograph.Text = "Insert";
			this.butInsertAutograph.Click += new System.EventHandler(this.butInsertAutograph_Click);
			// 
			// butDeleteAutograph
			// 
			this.butDeleteAutograph.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeleteAutograph.Autosize = true;
			this.butDeleteAutograph.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeleteAutograph.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeleteAutograph.CornerRadius = 4F;
			this.butDeleteAutograph.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDeleteAutograph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteAutograph.Location = new System.Drawing.Point(7, 236);
			this.butDeleteAutograph.Name = "butDeleteAutograph";
			this.butDeleteAutograph.Size = new System.Drawing.Size(75, 26);
			this.butDeleteAutograph.TabIndex = 3;
			this.butDeleteAutograph.Text = "Delete";
			this.butDeleteAutograph.Click += new System.EventHandler(this.butDeleteAutograph_Click);
			// 
			// butAddAutograph
			// 
			this.butAddAutograph.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddAutograph.Autosize = true;
			this.butAddAutograph.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddAutograph.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddAutograph.CornerRadius = 4F;
			this.butAddAutograph.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddAutograph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAutograph.Location = new System.Drawing.Point(7, 202);
			this.butAddAutograph.Name = "butAddAutograph";
			this.butAddAutograph.Size = new System.Drawing.Size(75, 26);
			this.butAddAutograph.TabIndex = 1;
			this.butAddAutograph.Text = "&Add";
			this.butAddAutograph.Click += new System.EventHandler(this.butAddAutograph_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(166, 14);
			this.label2.TabIndex = 18;
			this.label2.Text = "E-mail Autograph";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listAutographs
			// 
			this.listAutographs.Location = new System.Drawing.Point(10, 26);
			this.listAutographs.Name = "listAutographs";
			this.listAutographs.Size = new System.Drawing.Size(164, 173);
			this.listAutographs.TabIndex = 0;
			this.listAutographs.TabStop = false;
			this.listAutographs.DoubleClick += new System.EventHandler(this.listAutographs_DoubleClick);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(356, 659);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 25);
			this.butRefresh.TabIndex = 37;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butRawMessage
			// 
			this.butRawMessage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRawMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRawMessage.Autosize = true;
			this.butRawMessage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRawMessage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRawMessage.CornerRadius = 4F;
			this.butRawMessage.Location = new System.Drawing.Point(138, 659);
			this.butRawMessage.Name = "butRawMessage";
			this.butRawMessage.Size = new System.Drawing.Size(89, 26);
			this.butRawMessage.TabIndex = 36;
			this.butRawMessage.Text = "Raw Message";
			this.butRawMessage.Visible = false;
			this.butRawMessage.Click += new System.EventHandler(this.butRawMessage_Click);
			// 
			// butDirectMessage
			// 
			this.butDirectMessage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDirectMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDirectMessage.Autosize = true;
			this.butDirectMessage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDirectMessage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDirectMessage.CornerRadius = 4F;
			this.butDirectMessage.Location = new System.Drawing.Point(692, 659);
			this.butDirectMessage.Name = "butDirectMessage";
			this.butDirectMessage.Size = new System.Drawing.Size(106, 25);
			this.butDirectMessage.TabIndex = 8;
			this.butDirectMessage.Text = "Direct Message";
			this.butDirectMessage.Visible = false;
			this.butDirectMessage.Click += new System.EventHandler(this.butDirectMessage_Click);
			// 
			// butDecrypt
			// 
			this.butDecrypt.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDecrypt.Autosize = true;
			this.butDecrypt.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDecrypt.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDecrypt.CornerRadius = 4F;
			this.butDecrypt.Location = new System.Drawing.Point(8, 610);
			this.butDecrypt.Name = "butDecrypt";
			this.butDecrypt.Size = new System.Drawing.Size(75, 25);
			this.butDecrypt.TabIndex = 7;
			this.butDecrypt.Text = "Decrypt";
			this.butDecrypt.Visible = false;
			this.butDecrypt.Click += new System.EventHandler(this.butDecrypt_Click);
			// 
			// buttonFuchsMailDMF
			// 
			this.buttonFuchsMailDMF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonFuchsMailDMF.Autosize = true;
			this.buttonFuchsMailDMF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonFuchsMailDMF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonFuchsMailDMF.CornerRadius = 4F;
			this.buttonFuchsMailDMF.Location = new System.Drawing.Point(197, 218);
			this.buttonFuchsMailDMF.Name = "buttonFuchsMailDMF";
			this.buttonFuchsMailDMF.Size = new System.Drawing.Size(78, 22);
			this.buttonFuchsMailDMF.TabIndex = 30;
			this.buttonFuchsMailDMF.Text = "To DMF";
			this.buttonFuchsMailDMF.Visible = false;
			this.buttonFuchsMailDMF.Click += new System.EventHandler(this.buttonFuchsMailDMF_Click);
			// 
			// buttonFuchsMailDSF
			// 
			this.buttonFuchsMailDSF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonFuchsMailDSF.Autosize = true;
			this.buttonFuchsMailDSF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonFuchsMailDSF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonFuchsMailDSF.CornerRadius = 4F;
			this.buttonFuchsMailDSF.Location = new System.Drawing.Point(197, 191);
			this.buttonFuchsMailDSF.Name = "buttonFuchsMailDSF";
			this.buttonFuchsMailDSF.Size = new System.Drawing.Size(78, 22);
			this.buttonFuchsMailDSF.TabIndex = 29;
			this.buttonFuchsMailDSF.Text = "To DSF";
			this.buttonFuchsMailDSF.Visible = false;
			this.buttonFuchsMailDSF.Click += new System.EventHandler(this.buttonFuchsMailDSF_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(8, 659);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 26);
			this.butDelete.TabIndex = 11;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Location = new System.Drawing.Point(278, 659);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(75, 25);
			this.butSave.TabIndex = 6;
			this.butSave.Text = "Save";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSend.Autosize = true;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Location = new System.Drawing.Point(804, 659);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(75, 25);
			this.butSend.TabIndex = 9;
			this.butSend.Text = "&Send";
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
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
			this.butCancel.Location = new System.Drawing.Point(885, 659);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 10;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// emailPreview
			// 
			this.emailPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailPreview.BccAddress = "";
			this.emailPreview.BodyText = "";
			this.emailPreview.CcAddress = "";
			this.emailPreview.IsPreview = false;
			this.emailPreview.Location = new System.Drawing.Point(189, 31);
			this.emailPreview.Name = "emailPreview";
			this.emailPreview.Size = new System.Drawing.Size(771, 622);
			this.emailPreview.Subject = "";
			this.emailPreview.TabIndex = 38;
			this.emailPreview.ToAddress = "";
			// 
			// toolTipMessage
			// 
			this.toolTipMessage.AutoPopDelay = 0;
			this.toolTipMessage.InitialDelay = 10;
			this.toolTipMessage.ReshowDelay = 100;
			// 
			// FormEmailMessageEdit
			// 
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.panelAutographs);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butRawMessage);
			this.Controls.Add(this.butDirectMessage);
			this.Controls.Add(this.butDecrypt);
			this.Controls.Add(this.labelDecrypt);
			this.Controls.Add(this.buttonFuchsMailDMF);
			this.Controls.Add(this.buttonFuchsMailDSF);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.panelTemplates);
			this.Controls.Add(this.butSend);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.emailPreview);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(875, 735);
			this.Name = "FormEmailMessageEdit";
			this.Text = "Edit Email Message";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEmailMessageEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormEmailMessageEdit_Load);
			this.panelTemplates.ResumeLayout(false);
			this.panelAutographs.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormEmailMessageEdit_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup,true)) {
				butAddTemplate.Enabled=false;
				butDeleteTemplate.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				butDirectMessage.Visible=true;
			}
			Cursor=Cursors.WaitCursor;
			RefreshAll();
			SetDefaultAutograph();
			EmailSaveEvent.Fired+=EmailSaveEvent_Fired;
			Cursor=Cursors.Default;
		}

		private void EmailSaveEvent_Fired(ODEventArgs e) {
			if(e.Name!="EmailSave") {
				return;
			}
			//save email
			SaveMsg();//I think this is all we need
		}

		private void RefreshAll() {
			_emailMessage.IsNew=IsNew;
			emailPreview.LoadEmailMessage(_emailMessage,_listAllEmailMessages);
			if(!emailPreview.IsComposing) {
				panelTemplates.Visible=false;
				panelAutographs.Visible=false;
				butDirectMessage.Enabled=false;//not allowed to send again.
				butSend.Visible=false;//not allowed to send again.
				butSave.Visible=false;//not allowed to save changes.
				//When opening an email from FormEmailInbox, the email status will change to read automatically,
				//and changing the text on the cancel button helps convey that to the user.
				butCancel.Text="Close";
			}
			FillTemplates();
			FillAutographs();
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
				buttonFuchsMailDMF.Visible=true;
				buttonFuchsMailDSF.Visible=true;
			}
			butRefresh.Visible=false;
			//For all email received types, we disable most of the controls and put the form into a mostly read-only state.
			if(_emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedDirect ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.ReadDirect ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.Received ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.Read ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead)
			{
				butRefresh.Visible=true;
				butRawMessage.Visible=true;
        butSend.Visible=true;
        butSend.Text=Lan.g(this,"Reply");
      }
			labelDecrypt.Visible=false;
			butDecrypt.Visible=false;
			if(_emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted) {
				labelDecrypt.Visible=true;
				butDecrypt.Visible=true;
				butRefresh.Visible=false;
			}
		}

		#region Templates

		private void FillTemplates() {
			listTemplates.Items.Clear();
			_listEmailTemplates=EmailTemplates.GetDeepCopy();
			for(int i=0;i<_listEmailTemplates.Count;i++) {
				listTemplates.Items.Add(_listEmailTemplates[i].Description);
			}
		}

		private void listTemplates_DoubleClick(object sender, System.EventArgs e) {
			if(listTemplates.SelectedIndex==-1){
				return;
			}
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormEmailTemplateEdit FormE=new FormEmailTemplateEdit();
			FormE.ETcur=_listEmailTemplates[listTemplates.SelectedIndex];
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK){
				return;
			}
			EmailTemplates.RefreshCache();
			_hasTemplatesChanged=true;
			FillTemplates();
		}

		private void butAddTemplate_Click(object sender, System.EventArgs e) {
			FormEmailTemplateEdit FormE=new FormEmailTemplateEdit();
			FormE.IsNew=true;
			FormE.ETcur=new EmailTemplate();
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK){
				return;
			}
			EmailTemplates.RefreshCache();
			_hasTemplatesChanged=true;
			FillTemplates();
		}

		private void butDeleteTemplate_Click(object sender, System.EventArgs e) {
			if(listTemplates.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete e-mail template?"),"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK){
				return;
			}
			EmailTemplates.Delete(_listEmailTemplates[listTemplates.SelectedIndex]);
			EmailTemplates.RefreshCache();
			_hasTemplatesChanged=true;
			FillTemplates();
		}

		private void butInsertTemplate_Click(object sender, System.EventArgs e) {
			if(listTemplates.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(emailPreview.BodyText!="" || emailPreview.Subject!="" || emailPreview.HasAttachments){
				if(MessageBox.Show(Lan.g(this,"Replace existing e-mail text with text from the template?  Existing attachments will not be deleted.")
					,"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
					return;
				}
			}
			List<EmailTemplate> listEmailTemplates=_listEmailTemplates;
			List<EmailAttach> listAttachments=EmailAttaches.GetForTemplate(listEmailTemplates[listTemplates.SelectedIndex].EmailTemplateNum);
			listAttachments.ForEach(x => x.EmailTemplateNum=0); //Unattach the emailattachments from the email template.
			emailPreview.LoadTemplate(listEmailTemplates[listTemplates.SelectedIndex].Subject,
				listEmailTemplates[listTemplates.SelectedIndex].BodyText,listAttachments);
		}

		private void butEditTemplate_Click(object sender,EventArgs e) {
			if(listTemplates.SelectedIndex==-1) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormEmailTemplateEdit FormE=new FormEmailTemplateEdit();
			FormE.ETcur=_listEmailTemplates[listTemplates.SelectedIndex];
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK) {
				return;
			}
			EmailTemplates.RefreshCache();
			_hasTemplatesChanged=true;
			FillTemplates();
		}

		private void listTemplates_MouseMove(object sender,MouseEventArgs e) {
			int itemIdx=listTemplates.IndexFromPoint(e.Location);
			if(itemIdx==-1) {
				toolTipMessage.Hide(listTemplates);
				return;
			}
			string description=listTemplates.Items[itemIdx].ToString();
			if(toolTipMessage.GetToolTip(listTemplates).Contains(description)) {
				return;
			}
			toolTipMessage.SetToolTip(listTemplates,description);
		}

		///<summary>Hard coded template.</summary>
		private void buttonFuchsMailDSF_Click(object sender,EventArgs e) {
			emailPreview.ToAddress="skimom@springfielddental.net";
			emailPreview.LoadTemplate("Statement to DSF",
				"For accounting, sent statement to skimom@springfielddental.net"+emailPreview.BodyText,new List<EmailAttach>());
		}

		///<summary>Hard coded template.</summary>
		private void buttonFuchsMailDMF_Click(object sender,EventArgs e) {
			emailPreview.ToAddress="smilecouple@yahoo.com";
			emailPreview.LoadTemplate("Statement to DMF",
				"For accounting, sent statement to smilecouple@yahoo.com"+emailPreview.BodyText,new List<EmailAttach>());
		}

		#endregion Templates

		#region Autographs
		/// <summary>Fills the autographs picklist.</summary>
		private void FillAutographs() {
			listAutographs.Items.Clear();
			_listEmailAutographs=EmailAutographs.GetDeepCopy();
			for(int i=0;i<_listEmailAutographs.Count;i++) {
				listAutographs.Items.Add(_listEmailAutographs[i].Description);
			}
		}

		///<summary>Sets the default autograph that shows in the message body. 
		///The default autograph is determined to be the first autograph with an email that matches the email address of the sender.</summary>
		private void SetDefaultAutograph() {
			if(!emailPreview.IsComposing || !IsNew) {
				return;
			}
			string emailUserName=EmailMessages.GetAddressSimple(emailPreview.GetOutgoingEmailAddress().EmailUsername);
			string emailSender=EmailMessages.GetAddressSimple(emailPreview.GetOutgoingEmailAddress().SenderAddress);
			string autographEmail;
			for(int i=0;i<_listEmailAutographs.Count;i++) {
				autographEmail=EmailMessages.GetAddressSimple(_listEmailAutographs[i].EmailAddress.Trim());
				//Use Contains() because an autograph can theoretically have multiple email addresses associated with it.
				if(autographEmail.Contains(emailUserName) || autographEmail.Contains(emailSender)) {
					InsertAutograph(_listEmailAutographs[i]);
					break;
				}
			}
		}
		
		///<summary>Currently just appends an autograph to the bottom of the email message.  When the functionality to reply to emails is implemented, 
		///this will need to be modified so that it inserts the autograph text at the bottom of the new message being composed, but above the message
		///history.</summary>
		private void InsertAutograph(EmailAutograph emailAutograph) {
			emailPreview.BodyText+="\r\n\r\n"+emailAutograph.AutographText;
		}
		
		private void listAutographs_DoubleClick(object sender,EventArgs e) { //edit an autograph
			if(listAutographs.SelectedIndex==-1) {
				return;
			}
			FormEmailAutographEdit FormEAE=new FormEmailAutographEdit(_listEmailAutographs[listAutographs.SelectedIndex]);
			FormEAE.ShowDialog();
			if(FormEAE.DialogResult==DialogResult.OK) {
				EmailAutographs.RefreshCache();
				FillAutographs();
			}
		}

		private void butAddAutograph_Click(object sender,EventArgs e) { //add a new autograph
			EmailAutograph emailAutograph=new EmailAutograph();
			FormEmailAutographEdit FormEAE=new FormEmailAutographEdit(emailAutograph);
			FormEAE.IsNew=true;
			FormEAE.ShowDialog();
			if(FormEAE.DialogResult==DialogResult.OK) {
				EmailAutographs.RefreshCache();
				FillAutographs();
			}
		}

		private void butInsertAutograph_Click(object sender,EventArgs e) {
			if(listAutographs.SelectedIndex==-1) {
				MessageBox.Show(Lan.g(this,"Please select an autograph before inserting."));
				return;
			}
			InsertAutograph(_listEmailAutographs[listAutographs.SelectedIndex]);
		}
		
		private void butDeleteAutograph_Click(object sender,EventArgs e) {
			if(listAutographs.SelectedIndex==-1) {
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete autograph?"),"",MessageBoxButtons.OKCancel) != DialogResult.OK) {
				return;
			}
			EmailAutographs.Delete(_listEmailAutographs[listAutographs.SelectedIndex].EmailAutographNum);
			EmailAutographs.RefreshCache();
			FillAutographs();
		}

		private void butEditAutograph_Click(object sender,EventArgs e) {
			if(listAutographs.SelectedIndex==-1) {
				return;
			}
			FormEmailAutographEdit FormEAE=new FormEmailAutographEdit(_listEmailAutographs[listAutographs.SelectedIndex]);
			FormEAE.ShowDialog();
			if(FormEAE.DialogResult==DialogResult.OK) {
				EmailAutographs.RefreshCache();
				FillAutographs();
			}
		}

		#endregion

		private void butDecrypt_Click(object sender,EventArgs e) {
			if(EmailMessages.GetReceiverUntrustedCount(_emailMessage.FromAddress) >= 0) {//Not trusted yet.
				string strTrustMessage=Lan.g(this,"The sender address must be added to your trusted addresses before you can decrypt the email")
					+". "+Lan.g(this,"Add")+" "+_emailMessage.FromAddress+" "+Lan.g(this,"to trusted addresses")+"?";
				if(MessageBox.Show(strTrustMessage,"",MessageBoxButtons.OKCancel)==DialogResult.OK) {
					Cursor=Cursors.WaitCursor;
					EmailMessages.TryAddTrustDirect(_emailMessage.FromAddress);
					Cursor=Cursors.Default;
					if(EmailMessages.GetReceiverUntrustedCount(_emailMessage.FromAddress) >= 0) {
						MsgBox.Show(this,"Failed to trust sender because a valid certificate could not be located.");
						return;
					}
				}
				else {
					MsgBox.Show(this,"Cannot decrypt message from untrusted sender.");
					return;
				}
			}
			Cursor=Cursors.WaitCursor;
			EmailAddress emailAddress=emailPreview.EmailAddressPreview;
			try {
				_emailMessage=EmailMessages.ProcessRawEmailMessageIn(_emailMessage.BodyText,_emailMessage.EmailMessageNum,emailAddress,true);//If decryption is successful, sets status to ReceivedDirect.
				//The Direct message was decrypted.
				EmailMessages.UpdateSentOrReceivedRead(_emailMessage);//Mark read, because we are already viewing the message within the current window.					
				RefreshAll();
        HasEmailChanged=true;
      }
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Decryption failed.")+"\r\n"+ex.Message);
				//Error=InvalidEncryption: means that someone used the wrong certificate when sending the email to this inbox, and we tried to decrypt with a different certificate.
				//Error=NoTrustedRecipients: means the sender is not added to the trust anchors in mmc.
			}
			Cursor=Cursors.Default;
		}

		private void butDelete_Click(object sender,EventArgs e) {
      if(IsNew){
        DialogResult=DialogResult.Cancel;
        Close();
        //It will be deleted in the FormClosing() Event.
      }
      else{
        if(_emailMessage.PatNum!=0 && !_isDeleteAllowed) {
          if(MsgBox.Show(this, MsgBoxButtons.YesNo,"Hide this email from the inbox and all modules?")) {
            _emailMessage.HideIn=(HideInFlags)Enum.GetValues(typeof(HideInFlags)).Cast<HideInFlags>()
							.Where(x => x!=HideInFlags.None).ToList().Sum(x=>(int)x); //all HideInFlags set
            EmailMessages.Update(_emailMessage);
            HasEmailChanged=true;
            DialogResult=DialogResult.OK;
            Close();
          }
        }
        else{
          if(MsgBox.Show(this,true,"Delete this email?")){
            EmailMessages.Delete(_emailMessage);
            HasEmailChanged=true;
            DialogResult=DialogResult.OK;
            Close();
          }
        }
      }
		}

		private void butRawMessage_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(_emailMessage.RawEmailIn);
			msgbox.ShowDialog();
		}		

		private void butSave_Click(object sender,EventArgs e) {
			//this will not be available if already sent.
			SaveMsg();
			DialogResult=DialogResult.OK;
      Close(); //this form can be opened modelessly.
    }

		private void SaveMsg(){
			//allowed to save message with invalid fields, so no validation here.  Only validate when sending.
			emailPreview.SaveMsg(_emailMessage);
			if(IsNew) {
				EmailMessages.Insert(_emailMessage);
				IsNew=false;//As soon as the message is saved to the database, it is no longer new because it has a primary key.  Prevents new email from being duplicated if saved multiple times.
			}
			else {
				EmailMessages.Update(_emailMessage);
			}
      HasEmailChanged=true;
    }

		private void butRefresh_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			EmailAddress emailAddress=emailPreview.EmailAddressPreview;
			try {
				_emailMessage=EmailMessages.ProcessRawEmailMessageIn(_emailMessage.RawEmailIn,_emailMessage.EmailMessageNum,emailAddress,false);
				EmailMessages.UpdateSentOrReceivedRead(_emailMessage);//Mark read, because we are already viewing the message within the current window.
				RefreshAll();
        HasEmailChanged=true;
      }
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Refreshing failed.")+"\r\n"+ex.Message);
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
    }

    private void butDirectMessage_Click(object sender,EventArgs e) {
			//this will not be available if already sent.
			if(emailPreview.FromAddress=="" || emailPreview.ToAddress=="") {
				MessageBox.Show("Addresses not allowed to be blank.");
				return;
			}
			EmailAddress emailAddressFrom=emailPreview.GetOutgoingEmailAddress();
			if(emailPreview.FromAddress!=emailAddressFrom.EmailUsername) {
				//Without this block, encryption would fail with an obscure error message, because the from address would not match the digital signature of the sender.
				MessageBox.Show(Lan.g(this,"From address must match email address username in email setup.")+"\r\n"+Lan.g(this,"From address must be exactly")+" "+emailAddressFrom.EmailUsername);
				return;
			}
			if(emailAddressFrom.SMTPserver=="") {
				MsgBox.Show(this,"The email address in email setup must have an SMTP server.");
				return;
			}
			if(emailPreview.ToAddress.Contains(",")) {
				MsgBox.Show(this,"Multiple recipient addresses not allowed for direct messaging.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(EmailMessages.GetReceiverUntrustedCount(emailPreview.ToAddress) >= 0) {//Not trusted yet.
				EmailMessages.TryAddTrustDirect(emailPreview.ToAddress);
			}
			SaveMsg();
			try {
				string strErrors=EmailMessages.SendEmailDirect(_emailMessage,emailAddressFrom);
				if(strErrors!="") {
					Cursor=Cursors.Default;
					MessageBox.Show(strErrors);
					return;
				}
				else {
					_emailMessage.SentOrReceived=EmailSentOrReceived.SentDirect;
					EmailMessages.Update(_emailMessage);
          MsgBox.Show(this,"Sent");
				}
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(ex.Message);
				msgBox.ShowDialog();
				return;
			}
			Cursor=Cursors.Default;
      DialogResult=DialogResult.OK;
      Close();//this form can be opened modelessly.
    }

		///<summary>Becomes the reply button if the email was received.</summary>
		private void butSend_Click(object sender, System.EventArgs e) {
      if(_emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted
         || _emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedDirect
         || _emailMessage.SentOrReceived==EmailSentOrReceived.ReadDirect
         || _emailMessage.SentOrReceived==EmailSentOrReceived.Received
         || _emailMessage.SentOrReceived==EmailSentOrReceived.Read
         || _emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived
         || _emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead)
      {
        if(!Security.IsAuthorized(Permissions.EmailSend)) {
          return;
        }
        FormEmailMessageEdit FormE=new FormEmailMessageEdit(EmailMessages.CreateReply(_emailMessage,emailPreview.EmailAddressPreview),emailPreview.EmailAddressPreview,true,_listAllEmailMessages);
        FormE.IsNew=true;
        FormE.ShowDialog();
        if(FormE.DialogResult==DialogResult.OK) {
          HasEmailChanged=true;
					SaveMsg();
          DialogResult=DialogResult.OK;
          Close();//this form can be opened modelessly.
        }
        return;
      }
      //this will not be available if already sent.
      if(emailPreview.FromAddress==""){ 
				MsgBox.Show(this,"Please enter a sender address.");
				return;
			}
			if(emailPreview.ToAddress=="" && emailPreview.CcAddress=="" && emailPreview.BccAddress=="") {
				MsgBox.Show(this,"Please enter at least one recipient.");
				return;
			}
			if(EhrCCD.HasCcdEmailAttachment(_emailMessage)) {
				MsgBox.Show(this,"The email has a summary of care attachment which may contain sensitive patient data.  Use the Direct Message button instead.");
				return;
			}
			EmailAddress emailAddress=emailPreview.GetOutgoingEmailAddress();
			if(emailAddress.SMTPserver==""){
				MsgBox.Show(this,"The email address in email setup must have an SMTP server.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			SaveMsg();
			try{
				if(emailPreview.IsSigned) {
					EmailMessages.SendEmailUnsecureWithSig(_emailMessage,emailAddress,emailPreview.Signature);
				}
				else {
					EmailMessages.SendEmailUnsecure(_emailMessage,emailAddress);
				}
				_emailMessage.SentOrReceived=EmailSentOrReceived.Sent;
				EmailMessages.Update(_emailMessage);
				MsgBox.Show(this,"Sent");
			}
			catch(Exception ex){
				Cursor=Cursors.Default;
				string message=Lan.g(this,"Failed to send email.")+"\r\n\r\n"+Lan.g(this,"Error message from the email client was")+":\r\n  "+ex.Message;
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(message);
				msgBox.ShowDialog();
				return;
			}
			Cursor=Cursors.Default;
      //MessageCur.MsgDateTime=DateTime.Now;
      DialogResult=DialogResult.OK;
      Close();//this form can be opened modelessly.
    }

		private void butCancel_Click(object sender, System.EventArgs e) {
			if(!emailPreview.IsComposing) {//Use clicked the 'Close' button.  This is a 'read' email, so only changeable property is HideInFlags.
				SaveMsg();
				DialogResult=DialogResult.OK;//Triggers a refresh in calling views.
			}
			else {
				DialogResult=DialogResult.Cancel;
			}
      Close();
		}

		private void FormEmailMessageEdit_FormClosing(object sender,FormClosingEventArgs e) {
      if(_hasTemplatesChanged){
        DataValid.SetInvalid(InvalidType.Email);
      }
      EmailSaveEvent.Fired-=EmailSaveEvent_Fired;
			if(HasEmailChanged){
        Signalods.SetInvalid(InvalidType.EmailMessages);
        return;
			}
			if(IsNew){
				EmailMessages.Delete(_emailMessage);
			}
		}

  }
}
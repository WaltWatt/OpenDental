using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormEmailTemplateEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label2;
		private OpenDental.ODtextBox textBodyText;
		///<summary></summary>
		public bool IsNew;
		private Label label1;
		private Label label3;
		private UI.Button butBodyFields;
		private ODtextBox textSubject;
		private ODtextBox textDescription;
		///<summary></summary>
		public EmailTemplate ETcur;
		private UI.Button butAttach;
		private UI.ODGrid gridAttachments;
		private UI.Button butSubjectFields;
		private List<EmailAttach> _listEmailAttachDisplayed;
		private ContextMenu contextMenuAttachments;
		private MenuItem menuItemOpen;
		private MenuItem menuItemRename;
		private MenuItem menuItemRemove;
		private List<EmailAttach> _listEmailAttachOld=new List<EmailAttach>();

		///<summary></summary>
		public FormEmailTemplateEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailTemplateEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textBodyText = new OpenDental.ODtextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butBodyFields = new OpenDental.UI.Button();
			this.textSubject = new OpenDental.ODtextBox();
			this.textDescription = new OpenDental.ODtextBox();
			this.butSubjectFields = new OpenDental.UI.Button();
			this.butAttach = new OpenDental.UI.Button();
			this.gridAttachments = new OpenDental.UI.ODGrid();
			this.contextMenuAttachments = new System.Windows.Forms.ContextMenu();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItemRename = new System.Windows.Forms.MenuItem();
			this.menuItemRemove = new System.Windows.Forms.MenuItem();
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
			this.butCancel.Location = new System.Drawing.Point(883, 656);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 6;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(802, 656);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 5;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Subject";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBodyText
			// 
			this.textBodyText.AcceptsTab = true;
			this.textBodyText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBodyText.BackColor = System.Drawing.SystemColors.Window;
			this.textBodyText.DetectLinksEnabled = false;
			this.textBodyText.DetectUrls = false;
			this.textBodyText.Location = new System.Drawing.Point(97, 86);
			this.textBodyText.Name = "textBodyText";
			this.textBodyText.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textBodyText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textBodyText.Size = new System.Drawing.Size(861, 564);
			this.textBodyText.TabIndex = 3;
			this.textBodyText.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 86);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Body";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butBodyFields
			// 
			this.butBodyFields.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBodyFields.Autosize = true;
			this.butBodyFields.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBodyFields.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBodyFields.CornerRadius = 4F;
			this.butBodyFields.Location = new System.Drawing.Point(182, 23);
			this.butBodyFields.Name = "butBodyFields";
			this.butBodyFields.Size = new System.Drawing.Size(82, 20);
			this.butBodyFields.TabIndex = 4;
			this.butBodyFields.Text = "Body Fields";
			this.butBodyFields.Click += new System.EventHandler(this.butBodyFields_Click);
			// 
			// textSubject
			// 
			this.textSubject.AcceptsTab = true;
			this.textSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSubject.BackColor = System.Drawing.SystemColors.Window;
			this.textSubject.DetectLinksEnabled = false;
			this.textSubject.DetectUrls = false;
			this.textSubject.Location = new System.Drawing.Point(97, 65);
			this.textSubject.Multiline = false;
			this.textSubject.Name = "textSubject";
			this.textSubject.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textSubject.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSubject.Size = new System.Drawing.Size(635, 20);
			this.textSubject.TabIndex = 2;
			this.textSubject.Text = "";
			// 
			// textDescription
			// 
			this.textDescription.AcceptsTab = true;
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.BackColor = System.Drawing.SystemColors.Window;
			this.textDescription.DetectLinksEnabled = false;
			this.textDescription.DetectUrls = false;
			this.textDescription.Location = new System.Drawing.Point(97, 44);
			this.textDescription.Multiline = false;
			this.textDescription.Name = "textDescription";
			this.textDescription.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescription.Size = new System.Drawing.Size(635, 20);
			this.textDescription.TabIndex = 1;
			this.textDescription.Text = "";
			// 
			// butSubjectFields
			// 
			this.butSubjectFields.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSubjectFields.Autosize = true;
			this.butSubjectFields.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSubjectFields.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSubjectFields.CornerRadius = 4F;
			this.butSubjectFields.Location = new System.Drawing.Point(97, 23);
			this.butSubjectFields.Name = "butSubjectFields";
			this.butSubjectFields.Size = new System.Drawing.Size(82, 20);
			this.butSubjectFields.TabIndex = 7;
			this.butSubjectFields.Text = "Subject Fields";
			this.butSubjectFields.Click += new System.EventHandler(this.butSubjectFields_Click);
			// 
			// butAttach
			// 
			this.butAttach.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAttach.Autosize = true;
			this.butAttach.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttach.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttach.CornerRadius = 4F;
			this.butAttach.Location = new System.Drawing.Point(738, 2);
			this.butAttach.Name = "butAttach";
			this.butAttach.Size = new System.Drawing.Size(82, 20);
			this.butAttach.TabIndex = 9;
			this.butAttach.Text = "Attach...";
			this.butAttach.Click += new System.EventHandler(this.butAttach_Click);
			// 
			// gridAttachments
			// 
			this.gridAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAttachments.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAttachments.HasAddButton = false;
			this.gridAttachments.HasDropDowns = false;
			this.gridAttachments.HasMultilineHeaders = false;
			this.gridAttachments.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAttachments.HeaderHeight = 15;
			this.gridAttachments.HScrollVisible = false;
			this.gridAttachments.Location = new System.Drawing.Point(738, 23);
			this.gridAttachments.Name = "gridAttachments";
			this.gridAttachments.ScrollValue = 0;
			this.gridAttachments.Size = new System.Drawing.Size(220, 62);
			this.gridAttachments.TabIndex = 8;
			this.gridAttachments.TabStop = false;
			this.gridAttachments.Title = "Attachments";
			this.gridAttachments.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAttachments.TitleHeight = 18;
			this.gridAttachments.TranslationName = "TableAttachments";
			this.gridAttachments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAttachments_CellDoubleClick);
			this.gridAttachments.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridAttachments_MouseDown);
			// 
			// contextMenuAttachments
			// 
			this.contextMenuAttachments.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpen,
            this.menuItemRename,
            this.menuItemRemove});
			this.contextMenuAttachments.Popup += new System.EventHandler(this.contextMenuAttachments_Popup);
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Text = "Open";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// menuItemRename
			// 
			this.menuItemRename.Index = 1;
			this.menuItemRename.Text = "Rename";
			this.menuItemRename.Click += new System.EventHandler(this.menuItemRename_Click);
			// 
			// menuItemRemove
			// 
			this.menuItemRemove.Index = 2;
			this.menuItemRemove.Text = "Remove";
			this.menuItemRemove.Click += new System.EventHandler(this.menuItemRemove_Click);
			// 
			// FormEmailTemplateEdit
			// 
			this.ClientSize = new System.Drawing.Size(974, 695);
			this.Controls.Add(this.butAttach);
			this.Controls.Add(this.gridAttachments);
			this.Controls.Add(this.butSubjectFields);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.textSubject);
			this.Controls.Add(this.butBodyFields);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBodyText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(933, 200);
			this.Name = "FormEmailTemplateEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Email Template";
			this.Load += new System.EventHandler(this.FormEmailTemplateEdit_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormEmailTemplateEdit_Load(object sender, System.EventArgs e) {
			gridAttachments.ContextMenu=contextMenuAttachments;
			textSubject.Text=ETcur.Subject;
			textBodyText.Text=ETcur.BodyText;
			textDescription.Text=ETcur.Description;
			if(ETcur.EmailTemplateNum==0) {//New email template
				_listEmailAttachDisplayed=new List<EmailAttach>();
			}
			else {
				_listEmailAttachDisplayed=EmailAttaches.GetForTemplate(ETcur.EmailTemplateNum); 
				foreach(EmailAttach attachment in _listEmailAttachDisplayed) {
					_listEmailAttachOld.Add(attachment.Copy());
				}
			}
			FillAttachments();
		}

		private void FillAttachments() {
			gridAttachments.BeginUpdate();
			gridAttachments.Rows.Clear();
			gridAttachments.Columns.Clear();
			gridAttachments.Columns.Add(new OpenDental.UI.ODGridColumn("",0));//No name column, since there is only one column.
			foreach(EmailAttach attachment in _listEmailAttachDisplayed) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(attachment.DisplayedFileName);
				gridAttachments.Rows.Add(row);
			}
			gridAttachments.EndUpdate();
			if(gridAttachments.Rows.Count>0) {
				gridAttachments.SetSelected(0,true);
			}
		}
		
		private void butAttach_Click(object sender,EventArgs e) {
			_listEmailAttachDisplayed.AddRange(EmailAttachL.PickAttachments(null));
			FillAttachments();
		}

		private void gridAttachments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			EmailAttach attach=_listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
			FileAtoZ.OpenFile(FileAtoZ.CombinePaths(EmailAttaches.GetAttachPath(),attach.ActualFileName),attach.DisplayedFileName);
		}

		private void butSubjectFields_Click(object sender,EventArgs e) {
			FormMessageReplacements FormMR=new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient | MessageReplaceType.User | MessageReplaceType.Misc);
			FormMR.IsSelectionMode=true;
			FormMR.ShowDialog();
			if(FormMR.DialogResult==DialogResult.OK) {
				textSubject.SelectedText=FormMR.Replacement;
			}
		}

		private void butBodyFields_Click(object sender,EventArgs e) {
			FormMessageReplacements FormMR=new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient | MessageReplaceType.User | MessageReplaceType.Misc);
			FormMR.IsSelectionMode=true;
			FormMR.ShowDialog();
			if(FormMR.DialogResult==DialogResult.OK) {
				textBodyText.SelectedText=FormMR.Replacement;
			}
		}

		private void gridAttachments_MouseDown(object sender,MouseEventArgs e) {
			//A right click also needs to select an items so that the context menu will work properly.
			if(e.Button==MouseButtons.Right) {
				int clickedIndex=gridAttachments.PointToRow(e.Y);
				if(clickedIndex!=-1) {
					gridAttachments.SetSelected(clickedIndex,true);
				}
			}
		}

		private void contextMenuAttachments_Popup(object sender,EventArgs e) {
			menuItemOpen.Enabled=false;
			menuItemRename.Enabled=false;
			menuItemRemove.Enabled=false;
			if(gridAttachments.SelectedIndices.Length > 0) {
				menuItemOpen.Enabled=true;
				menuItemRename.Enabled=true;
				menuItemRemove.Enabled=true;
			}
		}

		private void menuItemOpen_Click(object sender,EventArgs e) {
			EmailAttach attach=_listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
			FileAtoZ.OpenFile(FileAtoZ.CombinePaths(EmailAttaches.GetAttachPath(),attach.ActualFileName),attach.DisplayedFileName);
		}

		private void menuItemRename_Click(object sender,EventArgs e) {
			InputBox input=new InputBox(Lan.g(this,"Filename"));
			EmailAttach emailAttach=_listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
			input.textResult.Text=emailAttach.DisplayedFileName;
			input.ShowDialog();
			if(input.DialogResult!=DialogResult.OK) {
				return;
			}
			emailAttach.DisplayedFileName=input.textResult.Text;
			FillAttachments();
		}

		private void menuItemRemove_Click(object sender,EventArgs e) {
			EmailAttach emailAttach=_listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
			_listEmailAttachDisplayed.Remove(emailAttach);
			FillAttachments();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textSubject.Text=="" && textBodyText.Text==""){
				MsgBox.Show(this,"Both the subject and body of the template cannot be left blank.");
				return;
			}
			if(textDescription.Text==""){
				MsgBox.Show(this,"Please enter a description.");
				return;
			}
			ETcur.Subject=textSubject.Text;
			ETcur.BodyText=textBodyText.Text;
			ETcur.Description=textDescription.Text;
			if(IsNew){
				EmailTemplates.Insert(ETcur);
			}
			else{
				EmailTemplates.Update(ETcur);
			}
			foreach(EmailAttach attachment in _listEmailAttachDisplayed) {
				attachment.EmailTemplateNum=ETcur.EmailTemplateNum;
			}
			//Sync the email attachments and pass in an emailMessageNum of 0 because we will be providing _listEmailAttachOld.
			EmailAttaches.Sync(0,_listEmailAttachDisplayed,_listEmailAttachOld);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}






















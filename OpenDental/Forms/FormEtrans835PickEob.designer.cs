namespace OpenDental{
	partial class FormEtrans835PickEob {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835PickEob));
			this.butClose = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.gridEobs = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(245, 295);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(468, 18);
			this.label1.TabIndex = 4;
			this.label1.Text = "Choose an EOB from the list below.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridEobs
			// 
			this.gridEobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridEobs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridEobs.HasAddButton = false;
			this.gridEobs.HasDropDowns = false;
			this.gridEobs.HasMultilineHeaders = false;
			this.gridEobs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridEobs.HeaderHeight = 15;
			this.gridEobs.HScrollVisible = false;
			this.gridEobs.Location = new System.Drawing.Point(15, 30);
			this.gridEobs.Name = "gridEobs";
			this.gridEobs.ScrollValue = 0;
			this.gridEobs.Size = new System.Drawing.Size(305, 259);
			this.gridEobs.TabIndex = 5;
			this.gridEobs.Title = "EOB List";
			this.gridEobs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridEobs.TitleHeight = 18;
			this.gridEobs.TranslationName = "TableEOB";
			this.gridEobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridEobs_CellDoubleClick);
			// 
			// FormEtrans835PickEob
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(332, 331);
			this.Controls.Add(this.gridEobs);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormEtrans835PickEob";
			this.Text = "EOBs";
			this.Load += new System.EventHandler(this.FormEtrans835PickEob_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label label1;
		private UI.ODGrid gridEobs;
	}
}
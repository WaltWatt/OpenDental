namespace OpenDental{
	partial class FormEtrans835PickEra {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835PickEra));
			this.butClose = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.gridEras = new OpenDental.UI.ODGrid();
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
			this.label1.Text = "Choose an ERA from the list below.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridEras
			// 
			this.gridEras.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridEras.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridEras.HasAddButton = false;
			this.gridEras.HasDropDowns = false;
			this.gridEras.HasMultilineHeaders = false;
			this.gridEras.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridEras.HeaderHeight = 15;
			this.gridEras.HScrollVisible = false;
			this.gridEras.Location = new System.Drawing.Point(15, 30);
			this.gridEras.Name = "gridEras";
			this.gridEras.ScrollValue = 0;
			this.gridEras.Size = new System.Drawing.Size(305, 259);
			this.gridEras.TabIndex = 5;
			this.gridEras.Title = "ERA List";
			this.gridEras.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridEras.TitleHeight = 18;
			this.gridEras.TranslationName = "TableERA";
			this.gridEras.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridEras_CellDoubleClick);
			// 
			// FormEtrans835PickEra
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(332, 331);
			this.Controls.Add(this.gridEras);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormEtrans835PickEra";
			this.Text = "ERAs";
			this.Load += new System.EventHandler(this.FormEtrans835PickEra_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label label1;
		private UI.ODGrid gridEras;
	}
}
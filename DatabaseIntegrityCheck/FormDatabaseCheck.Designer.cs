namespace DatabaseIntegrityCheck {
	partial class FormDatabaseCheck {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseCheck));
			this.butRepair = new System.Windows.Forms.Button();
			this.textDatabase = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textComputerName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textUser = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.labelProgress = new System.Windows.Forms.Label();
			this.butCheck = new System.Windows.Forms.Button();
			this.textResults = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// butRepair
			// 
			this.butRepair.Location = new System.Drawing.Point(286, 126);
			this.butRepair.Name = "butRepair";
			this.butRepair.Size = new System.Drawing.Size(75, 24);
			this.butRepair.TabIndex = 0;
			this.butRepair.Text = "Repair";
			this.butRepair.UseVisualStyleBackColor = true;
			this.butRepair.Click += new System.EventHandler(this.butRepair_Click);
			// 
			// textDatabase
			// 
			this.textDatabase.Location = new System.Drawing.Point(161, 46);
			this.textDatabase.Name = "textDatabase";
			this.textDatabase.Size = new System.Drawing.Size(200, 20);
			this.textDatabase.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(42, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Database";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(42, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "Computer";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textComputerName
			// 
			this.textComputerName.Location = new System.Drawing.Point(161, 19);
			this.textComputerName.Name = "textComputerName";
			this.textComputerName.Size = new System.Drawing.Size(200, 20);
			this.textComputerName.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(42, 73);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(118, 20);
			this.label3.TabIndex = 6;
			this.label3.Text = "Username";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUser
			// 
			this.textUser.Location = new System.Drawing.Point(161, 73);
			this.textUser.Name = "textUser";
			this.textUser.Size = new System.Drawing.Size(200, 20);
			this.textUser.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(42, 100);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(118, 20);
			this.label4.TabIndex = 8;
			this.label4.Text = "Password";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(161, 100);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(200, 20);
			this.textPassword.TabIndex = 7;
			this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
			this.textPassword.Leave += new System.EventHandler(this.textPassword_Leave);
			// 
			// labelProgress
			// 
			this.labelProgress.Location = new System.Drawing.Point(161, 156);
			this.labelProgress.Name = "labelProgress";
			this.labelProgress.Size = new System.Drawing.Size(200, 24);
			this.labelProgress.TabIndex = 9;
			this.labelProgress.Text = "labelProgress";
			this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butCheck
			// 
			this.butCheck.Location = new System.Drawing.Point(205, 126);
			this.butCheck.Name = "butCheck";
			this.butCheck.Size = new System.Drawing.Size(75, 24);
			this.butCheck.TabIndex = 10;
			this.butCheck.Text = "Check";
			this.butCheck.UseVisualStyleBackColor = true;
			this.butCheck.Click += new System.EventHandler(this.butCheck_Click);
			// 
			// textResults
			// 
			this.textResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textResults.BackColor = System.Drawing.SystemColors.Window;
			this.textResults.Location = new System.Drawing.Point(29, 185);
			this.textResults.Multiline = true;
			this.textResults.Name = "textResults";
			this.textResults.ReadOnly = true;
			this.textResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textResults.Size = new System.Drawing.Size(425, 270);
			this.textResults.TabIndex = 11;
			// 
			// FormDatabaseCheck
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 467);
			this.Controls.Add(this.textResults);
			this.Controls.Add(this.butCheck);
			this.Controls.Add(this.labelProgress);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textPassword);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textUser);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textComputerName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textDatabase);
			this.Controls.Add(this.butRepair);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormDatabaseCheck";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Database Integrity Check";
			this.Load += new System.EventHandler(this.FormDatabaseCheck_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button butRepair;
		private System.Windows.Forms.TextBox textDatabase;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textComputerName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textUser;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.Label labelProgress;
		private System.Windows.Forms.Button butCheck;
		private System.Windows.Forms.TextBox textResults;
	}
}


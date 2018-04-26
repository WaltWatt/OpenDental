using System.Windows.Forms;
using OpenDental.UI;

namespace OpenDental
{
  partial class FormSplash
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    //private System.ComponentModel.IContainer components=null;

    /// </summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    //protected override void Dispose(bool disposing)
    //{
    //    if (disposing && (components != null))
    //    {
    //        components.Dispose();
    //    }
    //    base.Dispose(disposing);
    //}

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSplash));
			this.SuspendLayout();
			this.SuspendLayout();
			this.progressBar = new ProgressBar();
			this.labelProgress = new Label();
			// 
			// labelProgress
			// 
			this.labelProgress.AutoSize = false;
			this.labelProgress.Location = new System.Drawing.Point(0, 300);
			this.labelProgress.Name = "progressBar";
			this.labelProgress.Size = new System.Drawing.Size(500, 16);
			this.labelProgress.TabIndex = 0;
			this.labelProgress.Text = "Initializing Open Dental... (0%)";
			this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// progressBar
			// 
			this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBar.Location = new System.Drawing.Point(0, 316);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(500, 15);
			this.progressBar.TabIndex = 1;
			// 
			// FormSplash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(500, 331);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.labelProgress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSplash";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Welcome to Open Dental";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSplash_FormClosing);
			this.Load += new System.EventHandler(this.FormSplash_Load);
			this.ResumeLayout(false);

		}
		#endregion
		private ProgressBar progressBar;
		private Label labelProgress;
	}
}
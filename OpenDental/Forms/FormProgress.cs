using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormProgress : ODForm, IProgressHandler {
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;
		//<summary></summary>
		//public string FileName;
		///<summary>The size in MB.</summary>
		public double MaxVal;
		private System.Windows.Forms.Label labelProgress;
		///<summary>Starts as 0.  Progresses to MaxVal.</summary>
		public double CurrentVal;
		///<summary>eg: ?currentVal MB of ?maxVal MB copied.  The two parameters will be replaced by numbers using the format based on NumberFormat.  If there are no parameters, then it will just display the text as is.</summary>
		public string DisplayText;
		///<summary>F for fixed.2, N to include comma, etc.</summary>
		public string NumberFormat;
		///<summary>Since only int values are allowed for progress bar, this allows you to use a double for the current and max.  The true value of the progress bar will be obtained by multiplying the double by the number here.  For example, 100 if you want to show MB like this: 3.15 MB.  The current value might be 3.1496858596859609.  This will set the currentValue of the progress bar to 315.</summary>
		public int NumberMultiplication;
		private Label labelError;
		public string ErrorMessage;
		///<summary>Sets the number of milliseconds between ticks.  Default is 0.  If 0, then a value of 200 will be used.</summary>
		public int TickMS;

		public FormProgress(bool showCancelButton=true,double maxVal=100,int numberMultiplication=100,string numFormat="F") 
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			MaxVal=maxVal;
			NumberMultiplication=numberMultiplication;
			NumberFormat=numFormat;
			Lan.F(this);
			butCancel.Visible=showCancelButton;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgress));
			this.butCancel = new OpenDental.UI.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.labelProgress = new System.Windows.Forms.Label();
			this.labelError = new System.Windows.Forms.Label();
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
			this.butCancel.Location = new System.Drawing.Point(376, 215);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(73, 99);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(377, 23);
			this.progressBar1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(71, 69);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Progress";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 200;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// labelProgress
			// 
			this.labelProgress.Location = new System.Drawing.Point(71, 135);
			this.labelProgress.Name = "labelProgress";
			this.labelProgress.Size = new System.Drawing.Size(402, 55);
			this.labelProgress.TabIndex = 4;
			this.labelProgress.Text = "Preparing for Upload";
			// 
			// labelError
			// 
			this.labelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelError.ForeColor = System.Drawing.Color.DarkRed;
			this.labelError.Location = new System.Drawing.Point(32, 13);
			this.labelError.Name = "labelError";
			this.labelError.Size = new System.Drawing.Size(456, 41);
			this.labelError.TabIndex = 5;
			this.labelError.Text = "Error Message";
			this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormProgress
			// 
			this.ClientSize = new System.Drawing.Size(500, 294);
			this.Controls.Add(this.labelError);
			this.Controls.Add(this.labelProgress);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProgress";
			this.ShowInTaskbar = false;
			this.Text = "Progress";
			this.Load += new System.EventHandler(this.FormProgress_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProgress_Load(object sender, System.EventArgs e) {
			progressBar1.Maximum=(int)(MaxVal*NumberMultiplication);
			labelError.Visible=false;
			if(TickMS>0) {
				timer1.Interval=TickMS;
			}
			string progress=DisplayText.Replace("?currentVal",CurrentVal.ToString(NumberFormat));
			progress=progress.Replace("?maxVal",MaxVal.ToString(NumberFormat));
			labelProgress.Text=progress;
		}
		
		///<summary>Happens every TickMS milliseconds.  Default is 200ms.</summary>
		private void timer1_Tick(object sender, System.EventArgs e) {
			Cursor=Cursors.Default;
			if(!string.IsNullOrEmpty(ErrorMessage)) {
				labelError.Visible=true;
				labelError.Text=ErrorMessage;
				//and this form will also not close because the currentVal will never reach the maxVal.
			}
			//progress bar shows 0 maxVal size
			progressBar1.Maximum=(int)(MaxVal*NumberMultiplication);
			string progress=DisplayText.Replace("?currentVal",CurrentVal.ToString(NumberFormat));
			progress=progress.Replace("?maxVal",MaxVal.ToString(NumberFormat));
			labelProgress.Text=progress;
				//=((double)CurrentVal/1024).ToString("F")+" MB of "
				//+((double)MaxVal/1024).ToString("F")+" MB copied"; 
			if(CurrentVal<MaxVal){
				progressBar1.Value=(int)(CurrentVal*(double)NumberMultiplication);
			}
			else{
				//must be done.
				//progressBar1.Value=progressBar1.Maximum;
				DialogResult=DialogResult.OK;
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			Cursor=Cursors.Default;//probably not needed
			DialogResult=DialogResult.Cancel;
		}

		///<summary>OnProgress can be used if the progress is going to be updated from the business layer (not from Open Dental).</summary>
		public void OnProgress(double newCurVal,string newDisplayText,double newMaxVal,string errorMessage) {
			CurrentVal=newCurVal;
			DisplayText=newDisplayText;
			MaxVal=newMaxVal;
			ErrorMessage=errorMessage;
		}
		
		public void UpdateBytesRead(long numBytes) {
			CurrentVal=numBytes / (1024 * 1024.0);
		}

		public void DisplayError(string error) {
			ErrorMessage=error;
		}

		public void CloseProgress() {
			CurrentVal=MaxVal;
		}
	}

	///<summary></summary>
	public delegate void PassProgressDelegate(double newCurVal,string newDisplayText,double newMaxVal,string errorMessage);
}






















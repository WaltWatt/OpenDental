using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class ValidTime:System.Windows.Forms.TextBox {
		private ErrorProvider _errorProv=new ErrorProvider();
		private System.ComponentModel.Container components = null;

		///<summary>If the entry is not valid, returns the error. If the entry is valid, returns an empty string.</summary>
		public string Error {
			get {
				return _errorProv.GetError(this);
			}
		}

		public bool IsEntryValid {
			get {
				return (Error=="");
			}
		}

		///<summary></summary>
		public ValidTime(){
			InitializeComponent();
			_errorProv.BlinkStyle=ErrorBlinkStyle.NeverBlink;
			Size=new Size(120,20);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		private void InitializeComponent(){
			this.SuspendLayout();
			// 
			// ValidDate
			// 
			this.Validating += new System.ComponentModel.CancelEventHandler(this.ValidTime_Validating);
			this.ResumeLayout(false);

		}
		#endregion

		private void ValidTime_Validating(object sender, CancelEventArgs e) {
			string myMessage="";
			try{
				if(Text==""){
					_errorProv.SetError(this,"");
					return;
				}
				Text=DateTime.Parse(Text).ToString("HH:mm:ss tt");//Format as '10:05:30 PM'. Will throw exception if invalid.
				_errorProv.SetError(this,"");
			}
			catch(Exception ex){
				//Cancel the event and select the text to be corrected by the user
				if(ex.Message=="String was not recognized as a valid time."){
					myMessage="Invalid time";
				}
				else{
					myMessage=ex.Message;
				}
				_errorProv.SetError(this,Lan.g("ValidTime",myMessage));
			}
		}

		///<summary>Gets rid of the orange exlamation circle.</summary>
		public void ClearError() {
			_errorProv.SetError(this,"");
		}


	}
}











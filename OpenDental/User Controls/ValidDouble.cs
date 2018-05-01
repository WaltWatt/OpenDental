using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class ValidDouble:System.Windows.Forms.TextBox {

		private Double _maxVal=100000000;
		private Double _minVal=-100000000;

		///<summary></summary>
		[Category("Data"),
			Description("The maximum value that user can enter.")
		]
		public Double MaxVal {
			get { return _maxVal; }
			set { _maxVal=value; }
		}
		///<summary></summary>
		[Category("Data"),
			Description("The minimum value that user can enter.")
		]
		public Double MinVal {
			get { return _minVal; }
			set { _minVal=value; }
		}
		public ErrorProvider errorProvider1=new ErrorProvider();

		///<summary>True if the text entered is a valid double.</summary>
		public bool IsValid {
			get {
				return errorProvider1.GetError(this)=="";
			}
		}

		///<summary></summary>
		public ValidDouble(){
			InitializeComponent();
			errorProvider1.BlinkStyle=ErrorBlinkStyle.NeverBlink;
		}

		#region Component Designer generated code

		private void InitializeComponent(){
			this.SuspendLayout();
			// 
			// ValidDouble
			// 
			this.TextChanged += new System.EventHandler(this.ValidDouble_TextChanged);
			this.ResumeLayout(false);

		}
		#endregion

		/*private void ValidDouble_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			string myMessage="";
			try{
				if(Text==""){
					errorProvider1.SetError(this,"");
					return;//Text="0";
				}
				if(System.Convert.ToDouble(this.Text)>MaxVal)
					throw new Exception("Number must be less than "+(MaxVal+1).ToString());
				if(System.Convert.ToDouble(this.Text)<MinVal)
					throw new Exception("Number must be greater than or equal to "+(MinVal).ToString());
				errorProvider1.SetError(this,"");
			}
			catch(Exception ex){
				if(ex.Message=="Input string was not in a correct format."){
					myMessage="Must be a number. No letters or symbols allowed";
				}
				else{
					myMessage=ex.Message;
				}	
				errorProvider1.SetError(this,myMessage);
			}			
		}*/

		private void ValidDouble_TextChanged(object sender,EventArgs e) {
			string myMessage="";
			try {
				if(Text=="") {
					errorProvider1.SetError(this,"");
					return;//Text="0";
				}
				if(System.Convert.ToDouble(this.Text)>MaxVal)
					throw new Exception("Number must be less than or equal to "+MaxVal.ToString());
				if(System.Convert.ToDouble(this.Text)<MinVal)
					throw new Exception("Number must be greater than or equal to "+(MinVal).ToString());
				errorProvider1.SetError(this,"");
			}
			catch(Exception ex) {
				if(ex.Message=="Input string was not in a correct format.") {
					myMessage="Must be a number. No letters or symbols allowed";
				}
				else {
					myMessage=ex.Message;
				}
				errorProvider1.SetError(this,myMessage);
			}			
		}

		

		


	}
}

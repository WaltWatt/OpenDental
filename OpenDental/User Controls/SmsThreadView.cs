using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>For SMS Text Messaging.  Used in the Text Messaging window to display an SMS message thread much like a cell phone.
	///Since users are used to seeing text message threads on cell phones, this control will be intuitive to users.</summary>
	public partial class SmsThreadView:UserControl {

		private List<SmsThreadMessage> _listSmsThreadMessages=null;
		///<summary>Keeps track of what page we're on.</summary>
		private int _smsThreadPage=1;
		///<summary>Set this value externally before showing the control.</summary>
		public List<SmsThreadMessage> ListSmsThreadMessages {
			get {
				return _listSmsThreadMessages;
			}
			set {
				_listSmsThreadMessages=value;
				FillMessageThread();
			}
		}
		///<summary>The number of text messages to display per page.</summary>
		public int CountMessagesToDisplay=100;
		///<summary>The list of messages to display on this page.</summary>
		private List<SmsThreadMessage> _listSmsThreadToDisplay;

		public SmsThreadView() {
			InitializeComponent();
		}

		private void FillMessageThread() {
			panelScroll.Controls.Clear();
			Invalidate();
			if(_listSmsThreadMessages==null || _listSmsThreadMessages.Count<1) {
				return;
			}
			BuildListMessages();
			int bodyWidth=panelScroll.Width-SystemInformation.VerticalScrollBarWidth;
			int verticalPadding=5;
			int horizontalMargin=(int)(bodyWidth*0.02);
			int y=0;
			Control controlHighlighted=null;
			panelScroll.SuspendLayout();
			for(int i=0;i<_listSmsThreadToDisplay.Count;i++) {
				y+=verticalPadding;
				Label labelDateTime=new Label();
				labelDateTime.Name="labelSmsDateTime"+i;
				labelDateTime.Text=_listSmsThreadToDisplay[i].MsgDateTime.ToString();
				if(_listSmsThreadToDisplay[i].IsAlignedLeft) {
					labelDateTime.TextAlign=ContentAlignment.MiddleLeft;
				}
				else {//Aligned right
					labelDateTime.TextAlign=ContentAlignment.MiddleRight;
				}
				Size textSize=TextRenderer.MeasureText(labelDateTime.Text,panelScroll.Font,
					new Size(bodyWidth,Int32.MaxValue),TextFormatFlags.WordBreak);
				labelDateTime.Width=bodyWidth;
				labelDateTime.Height=textSize.Height+2;//Extra vertical padding to ensure that the text will fit when including the border.
				labelDateTime.Location=new Point(0,y);
				panelScroll.Controls.Add(labelDateTime);
				y+=labelDateTime.Height;
				TextBox textBoxMessage=new TextBox();
				textBoxMessage.BackColor=_listSmsThreadToDisplay[i].BackColor;
				if(_listSmsThreadToDisplay[i].IsHighlighted) {
					controlHighlighted=textBoxMessage;
				}
				if(_listSmsThreadToDisplay[i].IsImportant) {
					textBoxMessage.ForeColor=Color.Red;
				}
				textBoxMessage.Name="textSmsThreadMsg"+i;
				textBoxMessage.BorderStyle=BorderStyle.FixedSingle;
				textBoxMessage.Multiline=true;
				textBoxMessage.Text=_listSmsThreadToDisplay[i].Message;
				//Each message wraps horizontally.
				textSize=TextRenderer.MeasureText(textBoxMessage.Text,panelScroll.Font,
					new Size((int)(bodyWidth*0.7),Int32.MaxValue),TextFormatFlags.WordBreak);
				textBoxMessage.Width=textSize.Width+4;//Extra horizontal padding to ensure that the text will fit when including the border.
				textBoxMessage.Height=textSize.Height+4;//Extra vertical padding to ensure that the text will fit when including the border.
				textBoxMessage.ReadOnly=true;
				if(_listSmsThreadToDisplay[i].IsAlignedLeft) {
					textBoxMessage.Location=new Point(horizontalMargin,y);
				}
				else {//Right aligned
					textBoxMessage.Location=new Point(bodyWidth-horizontalMargin-textBoxMessage.Width,y);
				}
				panelScroll.Controls.Add(textBoxMessage);
				y+=textBoxMessage.Height;
			}
			Label labelBottomSpacer=new Label();
			labelBottomSpacer.Name="labelBottomSpacer";
			labelBottomSpacer.Width=bodyWidth;
			labelBottomSpacer.Height=verticalPadding;
			labelBottomSpacer.Location=new Point(0,y);
			panelScroll.Controls.Add(labelBottomSpacer);
			y+=labelBottomSpacer.Height;
			if(controlHighlighted==null) {
				controlHighlighted=labelBottomSpacer;
			}
			if(panelScroll.VerticalScroll.Value!=panelScroll.VerticalScroll.Maximum) {
				panelScroll.VerticalScroll.Value=panelScroll.VerticalScroll.Maximum; //scroll to the end first then scroll to control
			}
			panelScroll.ScrollControlIntoView(controlHighlighted);//Scroll to highlighted control, or if none highlighted, then scroll to the end.
			panelScroll.ResumeLayout();
		}

		private void BuildListMessages() {
			_listSmsThreadToDisplay=new List<SmsThreadMessage>();	//We'll hold what messages are to be shown in this list.
			//Sort and reverse it so the messages are in order when they're added.
			_listSmsThreadMessages=_listSmsThreadMessages.OrderByDescending(x => x.MsgDateTime).ToList();	
			int maxPage=(int)Math.Ceiling((double)_listSmsThreadMessages.Count/CountMessagesToDisplay);	//# messages per page, #/count-1=page index
			if(_smsThreadPage > maxPage) {	
				_smsThreadPage=maxPage;
			}
			labelCurrentPage.Text=(_smsThreadPage).ToString() +" "+Lan.g(this,"of")+" "+ (maxPage).ToString();
			//Here we fill the reference list that is displayed depending on which page we're on.
			int firstMessageIdx=CountMessagesToDisplay*(_smsThreadPage-1);
			int lastMessageIdx=Math.Min(CountMessagesToDisplay*_smsThreadPage,_listSmsThreadMessages.Count)-1;
			for(int i=firstMessageIdx;i<=lastMessageIdx;i++) {	
				_listSmsThreadToDisplay.Add(_listSmsThreadMessages[i]);
			}
			//Reverse order so older messages are at the top, new at the bottom.
			_listSmsThreadToDisplay=_listSmsThreadToDisplay.OrderBy(x => x.MsgDateTime).ToList();	
			if(_listSmsThreadMessages.Count<=CountMessagesToDisplay) {
				panelNavigation.Visible=false;
				panelScroll.Location=new Point(0,0);
			}
			else {
				panelNavigation.Visible=true;
				panelScroll.Location=new Point(0,panelNavigation.Location.Y+panelNavigation.Height);	//Buttress up against the panelNavigation
			}			
			if(_smsThreadPage==maxPage) {
				butBackPage.Enabled=false;
				butEnd.Enabled=false;
				butForwardPage.Enabled=true;
				butBeginning.Enabled=true;
			}
			else if(_smsThreadPage==1) {
				butBackPage.Enabled=true;
				butEnd.Enabled=true;
				butForwardPage.Enabled=false;
				butBeginning.Enabled=false;
			}
			else {
				butBackPage.Enabled=true;
				butEnd.Enabled=true;
				butForwardPage.Enabled=true;
				butBeginning.Enabled=true;
			}
		}

		private void butBeginning_Click(object sender,EventArgs e) {
			if(_smsThreadPage==1) {
				return;	//Skip redrawing what we already have.
			}
			_smsThreadPage=1;
			FillMessageThread();
		}

		private void butForwardPage_Click(object sender,EventArgs e) {
			if(_smsThreadPage==1) {
				return;	//Don't go before the first page.
			}
			_smsThreadPage--;
			FillMessageThread();
		}

		private void butBackPage_Click(object sender,EventArgs e) {
			_smsThreadPage++;	//If we're on the last page, this variable will be fixed in the grid fill area.
			FillMessageThread();
		}

		private void butEnd_Click(object sender,EventArgs e) {
			_smsThreadPage=Int32.MaxValue;	//This is reset back to the maximum page, so we arrive directly at the end.
			FillMessageThread();
		}
	}

	public class SmsThreadMessage {
		///<summary>The date and time the message was sent or received.</summary>
		public DateTime MsgDateTime;
		///<summary>The message itself.</summary>
		public string Message;
		///<summary>If true, the message will be left aligned.  Otherwise the message will be right aligned.  Left aligned messages will be messages from
		///the patient, and right aligned messages will be from the office.  The left/right alignment is used as a quick way to show the user who
		///wrote each part of the message thread.</summary>
		public bool IsAlignedLeft;
		///<summary>Causes the message text to show in red.</summary>
		public bool IsImportant;
		public bool IsHighlighted;

		public Color BackColor {
			get {
				Color retVal;
				if(IsAlignedLeft) {//From Customer
					retVal=Color.FromArgb(244,255,244);
					if(IsHighlighted) {
						retVal=Color.FromArgb(220,255,220);
					}
				}
				else {//Right aligned
					retVal=Color.White;
					if(IsHighlighted) {
						retVal=Color.FromArgb(220,220,220);
					}
				}
				return retVal;
			}
		}

		public SmsThreadMessage(DateTime msgDateTime,string message,bool isAlignedLeft,bool isImportant,bool isHighlighted) {
			MsgDateTime=msgDateTime;
			Message=message;
			IsAlignedLeft=isAlignedLeft;
			IsImportant=isImportant;
			IsHighlighted=isHighlighted;
		}

		public static int CompareMessages(SmsThreadMessage msg1,SmsThreadMessage msg2) {
			return msg1.MsgDateTime.CompareTo(msg2.MsgDateTime);
		}

	}

}

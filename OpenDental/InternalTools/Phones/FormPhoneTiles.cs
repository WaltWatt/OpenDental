using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPhoneTiles:ODForm {
		///<summary>When the GoToChanged event fires, this tells us which patnum.</summary>
		public long GotoPatNum;
		///<summary></summary>
		[Category("Property Changed"),Description("Event raised when user wants to go to a patient or related object.")]
		public event EventHandler GoToChanged=null;
		///<summary>This is the difference between server time and local computer time.  Used to ensure that times displayed are accurate to the second.  This value is usally just a few seconds, but possibly a few minutes.</summary>
		private TimeSpan timeDelta;
		//private int msgCount;
		private PhoneTile selectedTile;
		///<summary>This thread fills labelMsg</summary>
		private List<Phone> PhoneList;
		private List<ChatUser> ChatList;
		private List<PhoneEmpDefault> PhoneEmpDefaultList;
		///<summary>Max number of tiles that can be shown. Columns and tiles which are not needed will be hidden and the window will be sized accordingly.</summary>
		private int TileCount;
		///<summary>How many phone tiles should show up in each column before creating a new column.</summary>
		private int TilesPerColumn=15;
		private Phones.PhoneComparer.SortBy SortBy=Phones.PhoneComparer.SortBy.name;

		public void SetPhoneList(List<PhoneEmpDefault> peds,List<Phone> phones,List<ChatUser> chatList,bool isFillTiles=true) {
			//create a new list so our sorting doesn't affect this list elsewhere
			PhoneList=new List<Phone>(phones);
			PhoneList.Sort(new Phones.PhoneComparer(SortBy));
			TileCount=PhoneList.Count;
			ChatList=chatList;
			PhoneEmpDefaultList=peds;
			if(isFillTiles) {
				FillTiles(false);
			}
		}

		public void SetVoicemailCount(int voiceMailCount) {
			if(voiceMailCount==0) {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Regular);
				labelMsg.ForeColor=Color.Black;
			}
			else {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,10f,FontStyle.Bold);
				labelMsg.ForeColor=Color.Firebrick;
			}
			labelMsg.Text="Voice Mails: "+voiceMailCount.ToString();
		}

		public FormPhoneTiles() {
			InitializeComponent();
		}

		private void FormPhoneTiles_Load(object sender,EventArgs e) {
#if !DEBUG
				if(Environment.MachineName.ToLower()!="jordans"
					&& Environment.MachineName.ToLower()!="nathan") 
				{
					checkBoxAll.Visible=false;//so this will also be visible in debug
				}
#endif
			timeDelta=MiscData.GetNowDateTime()-DateTime.Now;
			PhoneTile tile;
			int x=0;
			int y=0;
			SetPhoneList(PhoneEmpDefaults.Refresh(),Phones.GetPhoneList(),ChatUsers.GetAll(),false);//Do not call FillTiles() yet. Need to create PhoneTile controls first.
			for(int i=1;i<TileCount+1;i++) {
				tile=new PhoneTile();
				tile.Name="phoneTile"+(i).ToString();
				tile.Location=new Point(tile.Width*x,butOverride.Bottom+15+(tile.Height*y));				
				tile.GoToChanged += new System.EventHandler(this.phoneTile_GoToChanged);
				tile.SelectedTileChanged += new System.EventHandler(this.phoneTile_SelectedTileChanged);
				tile.MenuNumbers=menuNumbers;
				tile.MenuStatus=menuStatus;
				//adding this in case we ever want to show the NeedsHelp button in the big phones. Currently, it is hidden.
				tile.NeedsHelpClicked+= new System.EventHandler(this.tiletoggleHelp_Click);
				this.Controls.Add(tile);
				y++;
				if(i%(TilesPerColumn)==0) {//If number is divisble by the number of tiles per column, move over to a new column.  TilesPerColumn subtracts one because i is zero based.
					y=0;
					x++;
				}
			}
			FillTiles(false);//initial fast load and anytime data changes.  After this, pumped in from main form.
			Control[] topLeftMatch=Controls.Find("phoneTile1",false);
			if(PhoneList.Count>=1 
				&& topLeftMatch!=null
				&& topLeftMatch.Length>=1) 
			{ 
				//Size the window to fit contents
				tile=((PhoneTile)topLeftMatch[0]);
				int columns=(int)Math.Ceiling((double)PhoneList.Count/TilesPerColumn);
				int autoWidth=columns*tile.Width;
				int autoHeight=tile.Top+(tile.Height*TilesPerColumn);
				if(autoWidth > 1650) {//Window was going off side of screen.  1650 chosen to accommodate side mounted windows taskbar.
					//Resize window to fit, add height for showing horizontal scrollbar.
					//Doesn't use #*column width here because the column width is changing soon, and want to show partial columns so user knows to scroll.
					autoWidth=1650;
					autoHeight=autoHeight+20;
				}
				this.ClientSize=new Size(autoWidth,autoHeight);
			}
			radioByExt.CheckedChanged+=radioSort_CheckedChanged;
			radioByName.CheckedChanged+=radioSort_CheckedChanged;
		}

		private void FormPhoneTiles_Shown(object sender,EventArgs e) {
			DateTime now=DateTime.Now;
			while(now.AddSeconds(1)>DateTime.Now) {
				Application.DoEvents();
			}
		}

		private void FillTiles(bool refreshList) {
			if(refreshList) { //Refresh the phone list. This will cause a database refresh for our list and call this function again with the new list.
				SetPhoneList(PhoneEmpDefaults.Refresh(),Phones.GetPhoneList(),ChatUsers.GetAll());
				return;
			}
			if(PhoneList==null) {
				return;
			}
			PhoneTile tile;
			for(int i=0;i<TileCount;i++) {
				//Application.DoEvents();
				Control[] controlMatches=Controls.Find("phoneTile"+(i+1).ToString(),false);
				if(controlMatches.Length==0) {//no match found for some reason.
					continue;
				}
				tile=((PhoneTile)controlMatches[0]);
				tile.TimeDelta=timeDelta;
				tile.ShowImageForced=checkBoxAll.Checked;
				if(PhoneList.Count>i) {
					tile.SetPhone(PhoneList[i],PhoneEmpDefaults.GetEmpDefaultFromList(PhoneList[i].EmployeeNum,PhoneEmpDefaultList),
						ChatList.Where(x => x.Extension == PhoneList[i].Extension).FirstOrDefault(),PhoneEmpDefaults.IsTriageOperatorForExtension(PhoneList[i].Extension,PhoneEmpDefaultList));
				}
				else {
					Controls.Remove(tile);
				}
			}
		}

		//Adding this in case we ever want to show the NeedsHelp button in the big phones. Currently, it is hidden.
		private void tiletoggleHelp_Click(object sender,EventArgs e) {
			PhoneTile phoneTile = (PhoneTile) sender;
			if(phoneTile.PhoneCur.ClockStatus == ClockStatusEnum.NeedsHelp
				|| phoneTile.PhoneCur.ClockStatus == ClockStatusEnum.HelpOnTheWay) {
				PhoneUI.Available(phoneTile);
			}
			else {
				PhoneUI.NeedsHelp(phoneTile);
			}
			FillTiles(true);
		}

		private void phoneTile_GoToChanged(object sender,EventArgs e) {
			PhoneTile tile=(PhoneTile)sender;
			if(tile.PhoneCur==null) {
				return;
			}
			if(tile.PhoneCur.PatNum==0) {
				return;
			}
			GotoPatNum=tile.PhoneCur.PatNum;
			OnGoToChanged();
		}

		protected void OnGoToChanged() {
			if(GoToChanged!=null) {
				GoToChanged(this,new EventArgs());
			}
		}

		private void phoneTile_SelectedTileChanged(object sender,EventArgs e) {
			selectedTile=(PhoneTile)sender;
		}

		private void butOverride_Click(object sender,EventArgs e) {
			FormPhoneEmpDefaults formPED=new FormPhoneEmpDefaults();
			formPED.ShowDialog();
		}

		private void butConfRooms_Click(object sender,EventArgs e) {
			FormPhoneConfs FormPC=new FormPhoneConfs();
			FormPC.ShowDialog();//ShowDialog because we do not this window to be floating open for long periods of time.
		}

		private void checkBoxAll_Click(object sender,EventArgs e) {
			Phones.ClearImages();
			FillTiles(false);
		}

		private void radioSort_CheckedChanged(object sender,EventArgs e) {
			if(sender==null
				|| !(sender is RadioButton)
				|| ((RadioButton)sender).Checked==false) 
			{
				return;
			}
			if(radioByExt.Checked) {
				SortBy=Phones.PhoneComparer.SortBy.ext;
			}
			else {
				SortBy=Phones.PhoneComparer.SortBy.name;
			}
			//Get the phone tiles anew. This will force a resort according the preference we just set.
			FillTiles(true);
		}
	
		private void menuItemManage_Click(object sender,EventArgs e) {
			PhoneUI.Manage(selectedTile);
		}

		private void menuItemAdd_Click(object sender,EventArgs e) {
			PhoneUI.Add(selectedTile);
		}

		//Timecards-------------------------------------------------------------------------------------

		private void menuItemAvailable_Click(object sender,EventArgs e) {
			PhoneUI.Available(selectedTile);
			FillTiles(true);
		}

		private void menuItemTraining_Click(object sender,EventArgs e) {
			PhoneUI.Training(selectedTile);
			FillTiles(true);
		}

		private void menuItemTeamAssist_Click(object sender,EventArgs e) {
			PhoneUI.TeamAssist(selectedTile);
			FillTiles(true);
		}

		private void menuItemNeedsHelp_Click(object sender,EventArgs e) {
			PhoneUI.NeedsHelp(selectedTile);
			FillTiles(true);
		}

		private void menuItemWrapUp_Click(object sender,EventArgs e) {
			PhoneUI.WrapUp(selectedTile);
			FillTiles(true);
		}

		private void menuItemOfflineAssist_Click(object sender,EventArgs e) {
			PhoneUI.OfflineAssist(selectedTile);
			FillTiles(true);
		}

		private void menuItemUnavailable_Click(object sender,EventArgs e) {
			PhoneUI.Unavailable(selectedTile);
			FillTiles(true);
		}

		private void menuItemBackup_Click(object sender,EventArgs e) {
			PhoneUI.Backup(selectedTile);
			FillTiles(true);
		}

		private void menuItemTCResponder_Click(object sender,EventArgs e) {
			PhoneUI.TCResponder(selectedTile);
			FillTiles(true);
		}

		private void menuItemEmployeeSettings_Click(object sender,EventArgs e) {
			PhoneUI.EmployeeSettings(selectedTile);
			FillTiles(true);
		}

		//RingGroups---------------------------------------------------

		private void menuItemQueueTech_Click(object sender,EventArgs e) {
			PhoneUI.QueueTech(selectedTile);
		}

		private void menuItemQueueNone_Click(object sender,EventArgs e) {
			PhoneUI.QueueNone(selectedTile);
		}

		private void menuItemQueueDefault_Click(object sender,EventArgs e) {
			PhoneUI.QueueDefault(selectedTile);
		}

		private void menuItemQueueBackup_Click(object sender,EventArgs e) {
			PhoneUI.QueueBackup(selectedTile);
		}

		//Timecard---------------------------------------------------

		private void menuItemLunch_Click(object sender,EventArgs e) {
			PhoneUI.Lunch(selectedTile);
			FillTiles(true);
		}

		private void menuItemHome_Click(object sender,EventArgs e) {
			PhoneUI.Home(selectedTile);
			FillTiles(true);
		}

		private void menuItemBreak_Click(object sender,EventArgs e) {
			PhoneUI.Break(selectedTile);
			FillTiles(true);
		}
	}
}

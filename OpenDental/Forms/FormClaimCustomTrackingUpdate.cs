using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormClaimCustomTrackingUpdate:ODForm {
		/// <summary>List of claims selected from outstanding claim report</summary>
		private List<Claim> _listClaims;
		///<summary>When creating a new claimTracking this list contains 1 for every claim in _listClaims. Otherwise null</summary>
		public List<ClaimTracking> ListNewClaimTracks;

		///<summary>Used when creating a brand new claimcustomtracking.</summary>
		public FormClaimCustomTrackingUpdate(List<Claim> listClaims) {
			InitializeComponent();
			Lan.F(this);
			_listClaims=listClaims;
			ListNewClaimTracks=new List<ClaimTracking>(new ClaimTracking[_listClaims.Count]);
		}

		///<summary>Used when creating a brand new claimcustomtracking.</summary>
		public FormClaimCustomTrackingUpdate(Claim claimCur,string noteText) {
			InitializeComponent();
			Lan.F(this);
			_listClaims=new List<Claim>() {claimCur};
			ListNewClaimTracks=new List<ClaimTracking>(new ClaimTracking[_listClaims.Count]);//Default to list of nulls.
			textNotes.Text=noteText;
		}

		///<summary>Used for editing a ClaimTracking object from FormClaimEdit.</summary>
		public FormClaimCustomTrackingUpdate(Claim claimCur,ClaimTracking claimTrack) {
			InitializeComponent();
			Lan.F(this);
			_listClaims=new List<Claim>() { claimCur };
			ListNewClaimTracks=new List<ClaimTracking>() { claimTrack };
		}

		private void FormClaimCustomTrackingUpdate_Load(object sender,EventArgs e) {
			comboCustomTracking.Items.Clear();
			comboCustomTracking.Items.Add(new ODBoxItem<Def>(Lan.g(this,"None"),new Def() {ItemValue="",DefNum=0}));
			comboCustomTracking.SelectedIndex=0;
			Def[] arrayCustomTrackingDefs=Defs.GetDefsForCategory(DefCat.ClaimCustomTracking,true).ToArray();
			ClaimTracking claimTrack=ListNewClaimTracks.FirstOrDefault();
			for(int i=0;i<arrayCustomTrackingDefs.Length;i++) { 
				comboCustomTracking.Items.Add(new ODBoxItem<Def>(arrayCustomTrackingDefs[i].ItemName,arrayCustomTrackingDefs[i]));
				if(claimTrack?.TrackingDefNum==arrayCustomTrackingDefs[i].DefNum) {
					comboCustomTracking.SelectedIndex=i+1;//adding 1 to the index because we have added a 'none' option above
				}
			}
			textNotes.Text=claimTrack?.Note??"";
			FillComboErrorCode();
		}

		private void FillComboErrorCode() {
			Def[] arrayErrorCodeDefs = Defs.GetDefsForCategory(DefCat.ClaimErrorCode,true).ToArray();
			comboErrorCode.Items.Clear();
			//Add "none" option.
			comboErrorCode.Items.Add(new ODBoxItem<Def>(Lan.g(this,"None"),new Def() {ItemValue="",DefNum=0}));
			comboErrorCode.SelectedIndex=0;
			if(arrayErrorCodeDefs.Length==0) {
				//if the list is empty, then disable the comboBox.
				comboErrorCode.Enabled=false;
				return;
			}
			//Fill comboErrorCode.
			ClaimTracking claimTrack=ListNewClaimTracks.FirstOrDefault();
			for(int i=0;i<arrayErrorCodeDefs.Length;i++) {
				//hooray for using new ODBoxItems!
				comboErrorCode.Items.Add(new ODBoxItem<Def>(arrayErrorCodeDefs[i].ItemName,arrayErrorCodeDefs[i]));
				if(claimTrack?.TrackingErrorDefNum==arrayErrorCodeDefs[i].DefNum) {
					comboErrorCode.SelectedIndex=i+1;//adding 1 to the index because we have added a 'none' option above
				}
			}
		}

		private void comboErrorCode_SelectionChangeCommitted(object sender,EventArgs e) {
			if((!comboErrorCode.Enabled) || ((ODBoxItem<Def>)comboErrorCode.SelectedItem).Tag==null) {
				textErrorDesc.Text="";
			}
			else {
				textErrorDesc.Text=((ODBoxItem<Def>)comboErrorCode.SelectedItem).Tag.ItemValue.ToString();
			}	
		}

		private void butUpdate_Click(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.ClaimTrackingRequiresError) 
				&& ((ODBoxItem<Def>)comboErrorCode.SelectedItem).Tag == null 
				&& comboErrorCode.Enabled )
			{
				MsgBox.Show(this,"You must specify an error code."); //Do they have to specify an error code even if they set the status to None?
				return;
			}
			Def customTrackingDef=((ODBoxItem<Def>)comboCustomTracking.SelectedItem).Tag;
			if(customTrackingDef.DefNum==0) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Setting the status to none will disable filtering in the Outstanding Claims Report."
						+"  Do you wish to set the status of this claim to none?")) {
					return;
				}
			}
			Def errorCodeDef=((ODBoxItem<Def>)comboErrorCode.SelectedItem).Tag;
			for(int i=0;i<_listClaims.Count;i++) { //when not called from FormRpOutstandingIns, this should only have one claim.
				_listClaims[i].CustomTracking=customTrackingDef.DefNum;
				Claims.Update(_listClaims[i]);
				ClaimTracking trackCur=ListNewClaimTracks[i];
				if(trackCur==null) {
					trackCur=new ClaimTracking();
					trackCur.ClaimNum=_listClaims[i].ClaimNum;
				}
				trackCur.Note=textNotes.Text;
				trackCur.TrackingDefNum=customTrackingDef.DefNum;
				trackCur.TrackingErrorDefNum=errorCodeDef.DefNum;
				trackCur.UserNum=Security.CurUser.UserNum;
				if(trackCur.ClaimTrackingNum==0) { //new claim tracking status.
					ClaimTrackings.Insert(trackCur);
				}
				else { //existing claim tracking status
					ClaimTrackings.Update(trackCur);
				}
				ListNewClaimTracks[i]=trackCur;//Update list.
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
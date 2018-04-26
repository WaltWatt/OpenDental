using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormJobLinks:ODForm {
		private long _jobNum;
		///<summary>A list of bugs, features, and tasks related to this job.</summary>
		private List<JobLink> _jobLinks;
		private bool _hasChanged=false;

		///<summary>Opens with links to the passed in JobNum.</summary>
		public FormJobLinks(long jobNum) {
			_jobNum=jobNum;
			JobHandler.JobFired+=ODEvent_Fired;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobLinks_Load(object sender,EventArgs e) {
			_jobLinks=JobLinks.GetJobLinks(_jobNum);
			FillGrid();
		}

		private void FillGrid() {
			long selectedLinkNum=0;
			if(gridMain.GetSelectedIndex()!=-1) {
				selectedLinkNum=(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Type",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",200);
			gridMain.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_jobLinks.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobLinks[i].LinkType.ToString());
				if(_jobLinks[i].LinkType==JobLinkType.Task) {
					Task task=Tasks.GetOne(_jobLinks[i].FKey);
					if(task==null) {//task was deleted
						continue;
					}
					if(task.Descript.Length>=80) {
						row.Cells.Add(task.Descript.Substring(0,80)+"...");
					}
					else {
						row.Cells.Add(task.Descript);
					}
				}
				else if(_jobLinks[i].LinkType==JobLinkType.Bug) {
					row.Cells.Add("Under Construction");
				}
				else if(_jobLinks[i].LinkType==JobLinkType.Request) {
					row.Cells.Add("Feature Request #"+_jobLinks[i].FKey);
				}
				//else if(_jobLinks[i].LinkType==JobLinkType.Quote) {
				//	JobQuote quote=JobQuotes.GetOne(_jobLinks[i].FKey);
				//	string quoteText="Amount: "+quote.Amount;
				//	if(quote.PatNum!=0) {
				//		Patient pat=Patients.GetPat(quote.PatNum);
				//		quoteText+="\r\nCustomer: "+pat.LName+", "+pat.FName;
				//	}
				//	if(quote.Note!="") {
				//		quoteText+="\r\nNote: "+quote.Note;
				//	}
				//	row.Cells.Add(quoteText);
				//}
				row.Tag=_jobLinks[i].JobLinkNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if((long)gridMain.Rows[i].Tag==selectedLinkNum) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			switch (_jobLinks[gridMain.SelectedIndices[0]].LinkType) {
				case JobLinkType.Task:
					Task task=Tasks.GetOne(_jobLinks[gridMain.SelectedIndices[0]].FKey);
					FormTaskEdit FormTE=new FormTaskEdit(task,task.Copy());
					FormTE.ShowDialog();
					FillGrid();
					break;
				case JobLinkType.Request:
					FormRequestEdit FormRE=new FormRequestEdit();
					FormRE.RequestId=_jobLinks[gridMain.SelectedIndices[0]].FKey;
					FormRE.IsAdminMode=true;
					FormRE.ShowDialog();
					FillGrid();
					break;
				case JobLinkType.Bug:
					break;
				//case JobLinkType.Quote://TODO
				//	JobQuote quote=JobQuotes.GetOne(_jobLinks[gridMain.SelectedIndices[0]].FKey);
				//	FormJobQuoteEdit FormJQE=new FormJobQuoteEdit(quote);
				//	FormJQE.JobLinkNum=_jobLinks[gridMain.SelectedIndices[0]].JobLinkNum;//Allows deletion of the link if the quote is deleted.
				//	FormJQE.ShowDialog();
				//	_jobLinks=JobLinks.GetJobLinks(_jobNum);
				//	FillGrid();
				//	break;
			}
		}

		private void butLinkBug_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butLinkTask_Click(object sender,EventArgs e) {
			FormTaskSearch FormTS=new FormTaskSearch();
			FormTS.IsSelectionMode=true;
			if(FormTS.ShowDialog()==DialogResult.OK) {
				JobLink jobLink=new JobLink();
				jobLink.JobNum=_jobNum;
				jobLink.FKey=FormTS.SelectedTaskNum;
				jobLink.LinkType=JobLinkType.Task;
				JobLinks.Insert(jobLink);
				_jobLinks=JobLinks.GetJobLinks(_jobNum);
				_hasChanged=true;
				FillGrid();
			}
		}

		private void butLinkFeatReq_Click(object sender,EventArgs e) {
			FormFeatureRequest FormFR=new FormFeatureRequest();
			FormFR.IsSelectionMode=true;
			FormFR.ShowDialog();
			if(FormFR.DialogResult==DialogResult.OK) {
				JobLink jobLink=new JobLink();
				jobLink.JobNum=_jobNum;
				jobLink.FKey=FormFR.SelectedFeatureNum;
				jobLink.LinkType=JobLinkType.Request;
				JobLinks.Insert(jobLink);
				_jobLinks=JobLinks.GetJobLinks(_jobNum);
				_hasChanged=true;
				FillGrid();
			}
		}

		private void butLinkQuote_Click(object sender,EventArgs e) {
			return;
			//if(!JobRoles.IsAuthorized(JobRoleType.Quote)) {
			//	return;
			//}
			//JobQuote jobQuote=new JobQuote();
			//jobQuote.IsNew=true;
			//FormJobQuoteEdit FormJQE=new FormJobQuoteEdit(jobQuote);
			//if(FormJQE.ShowDialog()==DialogResult.OK) {
			//	JobLink jobLink=new JobLink();
			//	jobLink.JobNum=_jobNum;
			//	jobLink.FKey=FormJQE.JobQuoteCur.JobQuoteNum;
			//	jobLink.LinkType=JobLinkType.Quote;
			//	JobLinks.Insert(jobLink);
			//	_jobLinks=JobLinks.GetJobLinks(_jobNum);
			//	_hasChanged=true;
			//	FillGrid();
			//}
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Select a link to remove first.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to remove this link?")) {
				return;
			}
			JobLinks.Delete((long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			_hasChanged=true;
			_jobLinks.RemoveAt(gridMain.GetSelectedIndex());
			FillGrid();
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			_jobLinks=JobLinks.GetJobLinks(_jobNum);
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void FormJobLinks_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanged) {
				DataValid.SetInvalid(InvalidType.Jobs);
			}
		}

	}
}
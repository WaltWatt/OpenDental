using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormAutoNoteCompose:ODForm {
		public string CompletedNote;
		public string MainTextNote;
		///<summary>On load, the UserOdPref that contains the comma delimited list of expanded category DefNums is retrieved from the database.  On close
		///the UserOdPref is updated with the current expanded DefNums.</summary>
		private UserOdPref _userOdCurPref;
		private List<AutoNoteListItem> _listAutoNotePrompts;
		private List<Def> _listAutoNoteCatDefs;

		public FormAutoNoteCompose() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormAutoNoteCompose_Load(object sender,EventArgs e) {
			_listAutoNoteCatDefs=Defs.GetDefsForCategory(DefCat.AutoNoteCats,true);
			_listAutoNotePrompts=new List<AutoNoteListItem>();
			_userOdCurPref=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.AutoNoteExpandedCats).FirstOrDefault();
			FillListTree();
		}

		private void FormAutoNoteCompose_Shown(object sender,EventArgs e) {
			if(!string.IsNullOrEmpty(MainTextNote)) {
				PromptForAutoNotes(MainTextNote);
			}
		}

		private void FillListTree() {
			List<long> listExpandedDefNums=new List<long>();
			if(_userOdCurPref!=null) {//if this is the fill on load, the node count will be 0, expanded node list from pref
				listExpandedDefNums=_userOdCurPref.ValueString.Split(',').Where(x => x!="" && x!="0").Select(x => PIn.Long(x)).ToList();
			}
			//clear current tree contents
			treeListMain.BeginUpdate();
			treeListMain.SelectedNode=null;
			treeListMain.Nodes.Clear();
			//add categories with all auto notes that are assigned to that category
			List<long> listCatDefNums=_listAutoNoteCatDefs.Select(x => x.DefNum).ToList();
			//call recursive function GetNodeAndChildren for any root cat (where def.ItemValue is blank) and any def with invalid parent def num (ItemValue)
			_listAutoNoteCatDefs.FindAll(x => string.IsNullOrWhiteSpace(x.ItemValue) || !listCatDefNums.Contains(PIn.Long(x.ItemValue)))
				.ForEach(x => treeListMain.Nodes.Add(CreateNodeAndChildren(x)));//child cats and categorized auto notes added in recursive function
			//add any uncategorized auto notes after the categorized ones and only for the root nodes
			AutoNotes.GetWhere(x => x.Category==0 || !listCatDefNums.Contains(x.Category))
				.ForEach(x => treeListMain.Nodes.Add(new TreeNode(x.AutoNoteName,1,1) { Tag=x }));
			if(listExpandedDefNums.Count>0) {
				treeListMain.Nodes.OfType<TreeNode>().SelectMany(x => GetNodeAndChildren(x))
					.Where(x => x.Tag is Def && listExpandedDefNums.Contains(((Def)x.Tag).DefNum)).ToList()
					.ForEach(x => x.Expand());
			}
			treeListMain.EndUpdate();
		}

		///<summary>Recursive function, returns a tree node with all descendants, including all auto note children for this def cat and all children for
		///any cat within this this cat.  Auto Notes that are at the 'root' level (considered uncategorized) have to be added separately after filling the
		///rest of the tree with this function and will be at the bottom of the root node list.</summary>
		private TreeNode CreateNodeAndChildren(Def defCur) {
			List<TreeNode> listChildNodes=_listAutoNoteCatDefs
				.Where(x => !string.IsNullOrWhiteSpace(x.ItemValue) && x.ItemValue==defCur.DefNum.ToString())
				.Select(CreateNodeAndChildren).ToList();
			listChildNodes.AddRange(AutoNotes.GetWhere(x => x.Category==defCur.DefNum)
				.Select(x => new TreeNode(x.AutoNoteName,1,1) { Tag=x }));
			return new TreeNode(defCur.ItemName,0,0,listChildNodes.OrderBy(x => x.Tag is AutoNote).ThenBy(x => x.Name).ToArray()) { Tag=defCur };
		}

		///<summary>Returns a flat list containing this TreeNode and all of its descendant TreeNodes.  Recursive function to walk the full depth of the
		///tree starting at this TreeNode.</summary>
		private List<TreeNode> GetNodeAndChildren(TreeNode treeNode) {
			return new[] { treeNode }.Concat(treeNode.Nodes.OfType<TreeNode>().SelectMany(x => GetNodeAndChildren(x))).ToList();
		}

		private void treeListMain_DoubleClick(object sender,EventArgs e) {
			InsertSelectedAutoNote();
		}

		private void butInsert_Click(object sender,EventArgs e) {
			InsertSelectedAutoNote();
		}

		private void InsertSelectedAutoNote() {
			if(treeListMain.SelectedNode==null || !(treeListMain.SelectedNode.Tag is AutoNote)) {
				return;
			}
			string note=((AutoNote)treeListMain.SelectedNode.Tag).MainText;
			PromptForAutoNotes(note);
			treeListMain.SelectedNode=null;//clear selected node
		}

		private void textMain_TextChanged(object sender,EventArgs e) {
			if(textMain.Text.Trim()!="") {
				butOK.Visible=true;
			}
			else {
				butOK.Visible=false;
			}
		}

		void PromptForAutoNotes(string noteToParse) {
			string note=noteToParse;
			int selectionStart=textMain.SelectionStart;
			if(selectionStart==0) {
				textMain.Text=note+textMain.Text;
			}
			else if(selectionStart==textMain.Text.Length-1) {
				textMain.Text=textMain.Text+note;
			}
			else if(selectionStart==-1) {//?is this even possible?
				textMain.Text=textMain.Text+note;
			}
			else {
				textMain.Text=textMain.Text.Substring(0,selectionStart)+note+textMain.Text.Substring(selectionStart);
			}
			List<AutoNoteControl> prompts=new List<AutoNoteControl>();
			List<Match> listMatches=Regex.Matches(note,@"\[Prompt:""[a-zA-Z_0-9 ]+""\]").OfType<Match>().ToList();
			listMatches.RemoveAll(x => AutoNoteControls.GetByDescript(x.Value.Substring(9,x.Value.Length-11))==null);
			string autoNoteDescript;
			AutoNoteControl control;
			string promptResponse;
			int matchloc;
			int startLoc=0;
			Stack<int> stackLoc = new Stack<int>();
			stackLoc.Push(startLoc);
			for(int i=0;i<listMatches.Count;i++) {
				//highlight the current match in red
				matchloc=textMain.Text.IndexOf(listMatches[i].Value,startLoc);
				startLoc=matchloc+1;
				textMain.Select(matchloc,listMatches[i].Value.Length);
				textMain.SelectionBackColor=Color.Yellow;
				textMain.SelectionLength=0;
				Application.DoEvents();//refresh the textbox
				autoNoteDescript=listMatches[i].Value.Substring(9,listMatches[i].Value.Length-11);
				control=AutoNoteControls.GetByDescript(autoNoteDescript);//should never be null since we removed nulls above
				promptResponse="";
				if(control.ControlType=="Text") {
					FormAutoNotePromptText FormT=new FormAutoNotePromptText(autoNoteDescript);
					FormT.PromptText=control.ControlLabel;
					FormT.ResultText=control.ControlOptions;
					if(i>0) {
						FormT.IsGoBack=true;//user can go back if at least one item in the list exist
					}
					if(_listAutoNotePrompts.Count>i) {
						FormT.CurPromptResponse=_listAutoNotePrompts[i].AutoNoteTextString;//pass the previous response to the form
					}
					FormT.ShowDialog();
					if(FormT.DialogResult==DialogResult.Retry) {//user clicked the go back button
						GoBack(i);
						stackLoc.Pop();
						startLoc=stackLoc.Peek();
						i-=2;
						continue;
					}
					if(FormT.DialogResult==DialogResult.OK) {
						promptResponse=FormT.ResultText;
						if(_listAutoNotePrompts.Count>i) {//reponse already exist for this control type. Update it
							_listAutoNotePrompts[i].AutoNoteTextString=promptResponse;
							_listAutoNotePrompts[i].AutoNoteTextStartPos=matchloc;
						}
						else {
							_listAutoNotePrompts.Add(new AutoNoteListItem(listMatches[i].Value,promptResponse,matchloc));//add new response to the list
						}
					}
					else {
						textMain.SelectAll();
						textMain.SelectionBackColor=Color.White;
						textMain.Select(textMain.Text.Length,0);
						return;
					}
				}
				else if(control.ControlType=="OneResponse") {
					FormAutoNotePromptOneResp FormOR=new FormAutoNotePromptOneResp(autoNoteDescript);
					FormOR.PromptText=control.ControlLabel;
					FormOR.PromptOptions=control.ControlOptions;
					if(i>0) {
						FormOR.IsGoBack=true;//user can go back if at least one item in the list exist
					}
					if(_listAutoNotePrompts.Count>i) {
						FormOR.CurPromptResponse=_listAutoNotePrompts[i].AutoNoteTextString;//pass the previous response if exist to the form.
					}
					FormOR.ShowDialog();
					if(FormOR.DialogResult==DialogResult.Retry) {//user clicked the go back button
						GoBack(i);
						stackLoc.Pop();
						startLoc=stackLoc.Peek();
						i-=2;
						continue;
					}
					if(FormOR.DialogResult==DialogResult.OK) {
						promptResponse=FormOR.ResultText;
						if(_listAutoNotePrompts.Count>i) {//reponse already exist for this control type. Update it
							_listAutoNotePrompts[i].AutoNoteTextString=promptResponse;
							_listAutoNotePrompts[i].AutoNoteTextStartPos=matchloc;
						}
						else {
							_listAutoNotePrompts.Add(new AutoNoteListItem(listMatches[i].Value,promptResponse,matchloc));//add new response to the list
						}
					}
					else {
						textMain.SelectAll();
						textMain.SelectionBackColor=Color.White;
						textMain.Select(textMain.Text.Length,0);
						return;
					}
				}
				else if(control.ControlType=="MultiResponse") {
					FormAutoNotePromptMultiResp FormMR=new FormAutoNotePromptMultiResp(autoNoteDescript);
					FormMR.PromptText=control.ControlLabel;
					FormMR.PromptOptions=control.ControlOptions;
					if(i>0) {
						FormMR.IsGoBack=true;//user can go back if at least one item in the list exist
					}
					if(_listAutoNotePrompts.Count>i) {
						FormMR.CurPromptResponse=_listAutoNotePrompts[i].AutoNoteTextString;//pass the previous response if exist to the form.
					}
					FormMR.ShowDialog();
					if(FormMR.DialogResult==DialogResult.Retry) {//user clicked the go back button
						GoBack(i);
						stackLoc.Pop();
						startLoc=stackLoc.Peek();
						i-=2;
						continue;
					}
					if(FormMR.DialogResult==DialogResult.OK) {
						promptResponse=FormMR.ResultText;
						if(_listAutoNotePrompts.Count>i) {//reponse already exist for this control type. Update it
							_listAutoNotePrompts[i].AutoNoteTextString=promptResponse;
							_listAutoNotePrompts[i].AutoNoteTextStartPos=matchloc;
						}
						else {
							_listAutoNotePrompts.Add(new AutoNoteListItem(listMatches[i].Value,promptResponse,matchloc));//add new response to the list
						}
					}
					else {
						textMain.SelectAll();
						textMain.SelectionBackColor=Color.White;
						textMain.Select(textMain.Text.Length,0);
						return;
					}
				}
				string resultstr=textMain.Text.Substring(0,matchloc)+promptResponse;
				if(textMain.Text.Length > matchloc+listMatches[i].Value.Length) {
					resultstr+=textMain.Text.Substring(matchloc+listMatches[i].Value.Length);
				}
				textMain.Text=resultstr;
				textMain.SelectAll();
				textMain.SelectionBackColor=Color.White;
				textMain.Select(textMain.Text.Length,0);
				if(string.IsNullOrEmpty(promptResponse)) {
					//if prompt was removed, add the previous start location onto the stack. 
					startLoc=stackLoc.Peek();
				}
				stackLoc.Push(startLoc);
				Application.DoEvents();//refresh the textbox
			}
			textMain.SelectAll();
			textMain.SelectionBackColor=Color.White;
			textMain.Select(textMain.Text.Length,0);
			_listAutoNotePrompts.Clear();
		}

		private void butOK_Click(object sender,EventArgs e) {
			CompletedNote=textMain.Text.Replace("\n","\r\n");
			DialogResult=DialogResult.OK;
		}

		///<summary>Removes previous response and inserts the AutoNote prompt into textMain.</summary>
		private void GoBack(int pos) {
			textMain.SelectAll();
			textMain.SelectionBackColor=Color.White;
			textMain.Text=textMain.Text.Remove(_listAutoNotePrompts[pos-1].AutoNoteTextStartPos,_listAutoNotePrompts[pos-1].AutoNoteTextString.Length).Insert(_listAutoNotePrompts[pos-1].AutoNoteTextStartPos,_listAutoNotePrompts[pos-1].AutoNotePromptText);
			if(_listAutoNotePrompts[pos-1].AutoNoteTextString==_listAutoNotePrompts[pos-1].AutoNotePromptText) {
				_listAutoNotePrompts[pos-1].AutoNoteTextString="";
			}
			Application.DoEvents();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormAutoNoteCompose_FormClosing(object sender,FormClosingEventArgs e) {
			//store the current node expanded state for this user
			List<long> listExpandedDefNums=treeListMain.Nodes.OfType<TreeNode>().SelectMany(x => GetNodeAndChildren(x))
				.Where(x => x.Tag is Def && x.IsExpanded).Select(x => ((Def)x.Tag).DefNum).Where(x => x>0).ToList();
			if(_userOdCurPref==null) {
				UserOdPrefs.Insert(new UserOdPref() {
					UserNum=Security.CurUser.UserNum,
					FkeyType=UserOdFkeyType.AutoNoteExpandedCats,
					ValueString=string.Join(",",listExpandedDefNums)
				});
			}
			else {
				_userOdCurPref.ValueString=string.Join(",",listExpandedDefNums);
				UserOdPrefs.Update(_userOdCurPref);
			}
		}

		private class AutoNoteListItem {
			public string AutoNotePromptText;
			public string AutoNoteTextString;
			public int AutoNoteTextStartPos;

			public AutoNoteListItem(string autoNotePrompt,string autoNoteTextString,int autoNoteStartPos) {
				AutoNotePromptText=autoNotePrompt;
				AutoNoteTextString=autoNoteTextString;
				AutoNoteTextStartPos=autoNoteStartPos;
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDental.UI;
using System.Text.RegularExpressions;
using System.Drawing;

namespace OpenDental {
	public partial class FormQueryParser:Form {
		private UserQuery _queryCur;
		private UserQuery _queryOld;

		public FormQueryParser(UserQuery queryCur) {
			InitializeComponent();
			Lan.F(this);
			_queryCur=queryCur;
			_queryOld=queryCur.Copy();
			//hide the query text by default.
			Height = 325;
			splitContainer1.Panel2Collapsed=true;
			this.Text +=" - " + _queryCur.Description;
		}

		private void FormQueryParser_Load(object sender,EventArgs e) {
			textQuery.Text=_queryCur.QueryText;
			FillGrid();
			if(!Security.IsAuthorized(Permissions.UserQueryAdmin,true)) {
				textQuery.ReadOnly=true;
			}
		}

		///<summary>This method takes care of parsing the query by pulling out SET statements and finding the variables with their assigned values.
		///Puts all of this information into the grid.</summary>
		private void FillGrid(bool isTypingText = false) {
			Point selectedCell = gridMain.SelectedCell;
			List<string> listSetStmts = UserQueries.ParseSetStatements(_queryCur.QueryText);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g(gridMain.TranslationName,"Variable"),200));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(gridMain.TranslationName,"Value"),200,true));
			gridMain.Rows.Clear();
			foreach(string strSetStmt in listSetStmts) { //for each SET statement
				List<QuerySetStmtObject> listQObjs = GetListQuerySetStmtObjs(strSetStmt); //find the variable name
				foreach(QuerySetStmtObject qObj in listQObjs) {
					ODGridRow row = new ODGridRow();
					row.Cells.Add(qObj.Variable);
					row.Cells.Add(qObj.Value);
					row.Tag=qObj;
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
			if(!isTypingText) {
				try {
					gridMain.SetSelected(selectedCell);
				}
				catch {
					//suppress if the row doesn't exist (such as filling the grid for the first time)
				}
			}
		}

		///<summary>Returns the list of variables in the query contained within the passed-in SET statement.
		///Pass in one SET statement. Used in conjunction with GetListVals.</summary>
		private List<QuerySetStmtObject> GetListQuerySetStmtObjs(string setStmt) {
			List<string> strSplits = UserQueries.SplitQuery(setStmt,false,",");
			for(int i = 0;i < strSplits.Count;i++) {
				Regex r = new Regex(@"\s*set\s+",RegexOptions.IgnoreCase);
				strSplits[i]=r.Replace(strSplits[i],"");
			}
			UserQueries.TrimList(strSplits);
			strSplits.RemoveAll(x => string.IsNullOrWhiteSpace(x) || !x.StartsWith("@") || x.StartsWith("@_"));
			List<QuerySetStmtObject> bufferList = new List<QuerySetStmtObject>();
			for(int i = 0;i < strSplits.Count;i++) {
				QuerySetStmtObject qObj = new QuerySetStmtObject();
				qObj.Stmt = setStmt;
				qObj.Variable = strSplits[i].Split(new char[] { '=' },2).First();
				qObj.Value = strSplits[i].Split(new char[] { '=' },2).Last();
				bufferList.Add(qObj);
			}
			return bufferList;
		}
		
		///<summary>When a row is double clicked, bring up an input box that allows the user to change the variable's value.</summary>
		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			QuerySetStmtObject qObjCur = (QuerySetStmtObject)gridMain.SelectedGridRows[0].Tag;
			InputBox inputBox=new InputBox(new List<InputBoxParam> {
				new InputBoxParam(InputBoxType.TextBox,Lan.g(this,"Set value for")+" "+ qObjCur.Variable,text: qObjCur.Value)
			});
			inputBox.ShowDialog();
			if(inputBox.DialogResult == DialogResult.OK) {
				string stmtOld = qObjCur.Stmt;
				//Regular expression for the expression @Variable = Value.
				Regex r = new Regex(Regex.Escape(qObjCur.Variable)+@"\s*=\s*"+Regex.Escape(qObjCur.Value));
				string stmtNew = r.Replace(stmtOld,qObjCur.Variable+"="+inputBox.textResult.Text);
				_queryCur.QueryText=_queryCur.QueryText.Replace(stmtOld,stmtNew);
				if(stmtOld == stmtNew) {
					return; //don't bother refilling the grid if the value didn't change.
				}
				textQuery.Text=_queryCur.QueryText;
				FillGrid();
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			SelectTextForSelectedRow();
		}

		private void gridMain_CellEnter(object sender,ODGridClickEventArgs e) {
			SelectTextForSelectedRow();
		}

		private void SelectTextForSelectedRow() {
			QuerySetStmtObject qObjCur = (QuerySetStmtObject)gridMain.SelectedGridRows[0].Tag;
			int startidx = textQuery.Text.IndexOf(qObjCur.Stmt);
			if(startidx == -1) {
				//the query object does not have comments in it, while the text area does.
				//this can cause finding the index of the set statement to fail if the user 
				//has comments contained within their set statements (which should never happen unless the query is malformed).
				//In this case, we'll just suppress any failures.
				return;
			}
			textQuery.Select(startidx,qObjCur.Stmt.Length);
		}

		private void butShowHide_Click(object sender,EventArgs e) {
			splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
			if(splitContainer1.Panel2Collapsed) {
				butShowHide.Text=Lan.g(this,"Show Text");
				Height = 325;
				this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			}
			else {
				butShowHide.Text=Lan.g(this,"Hide Text");
				Height = 720;
				this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowUpTriangle;
			}
		}

		private void timerRefreshGrid_Tick(object sender,EventArgs e) {
			//when the text in the textbox is changed, refresh the grid on a timer.
			_queryCur.QueryText=textQuery.Text;
			FillGrid(true);
			timerRefreshGrid.Stop();
		}
		

		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			QuerySetStmtObject qObjCur;
			try {
				qObjCur=(QuerySetStmtObject)gridMain.SelectedGridRows[0].Tag;
			}
			catch {//Has occurend when user types SHIFT+ENTER on the keyboard.
				return;
			}
			string stmtOld = qObjCur.Stmt;
			string varOld = qObjCur.Variable;
			string valOld = qObjCur.Value;
			string valNew = gridMain.SelectedGridRows[0].Cells[1].Text.ToString();
			if(UserQueries.SplitQuery(valNew,true,";").Count > 1) {
				Point _selectedCell = gridMain.SelectedCell;
				MsgBox.Show(this,"You may not include semicolons in the value text. Please remove all semicolons.");
				gridMain.SelectedGridRows[0].Cells[1].Text = valOld;
				gridMain.SetSelected(_selectedCell); //this just refreshes the cell that is being left. Is there an easy way to cancel the CellLeave action?
				return;
			}
			if(valOld == valNew) {
				return; //don't bother doing any of the logic below if nothing changed.
			}
			//Regular expression for the expression @Variable = Value.
			Regex r = new Regex(Regex.Escape(varOld)+@"\s*=\s*"+Regex.Escape(valOld));
			string stmtNew = r.Replace(stmtOld,varOld+"="+valNew);
			_queryCur.QueryText=_queryCur.QueryText.Replace(stmtOld,stmtNew);
			textQuery.Text=_queryCur.QueryText;
			qObjCur.Stmt=stmtNew;
			qObjCur.Value=valNew;
			gridMain.Rows.OfType<ODGridRow>().Where(x => ((QuerySetStmtObject)x.Tag).Stmt == stmtOld).ToList().ForEach(y => {
				((QuerySetStmtObject)y.Tag).Stmt = stmtNew;
			});
		}

		private void textQuery_KeyPress(object sender,KeyPressEventArgs e) {
			//when the text in the textbox is changed, refresh the grid on a timer.
			timerRefreshGrid.Enabled=true;
			timerRefreshGrid.Stop();
			timerRefreshGrid.Start();
		}
		
		private void butOk_Click(object sender,EventArgs e) {
			this.DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			this.DialogResult=DialogResult.Cancel;
		}

		private void FormQueryParser_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult != DialogResult.OK) {
				_queryCur.QueryText = _queryOld.QueryText; //change the text back to what it used to be if the users cancels.
				return;
			}
			_queryCur.QueryText=textQuery.Text;
		}

		///<summary>A tiny class that contains a single SET statement's variable, value, and the entire statement.</summary>
		private class QuerySetStmtObject {
			public string Variable;
			public string Value;
			public string Stmt;
		}
	}
}
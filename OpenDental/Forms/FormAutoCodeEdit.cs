using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormAutoCodeEdit : ODForm {
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkHidden;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.TextBox textDescript;
		private System.Windows.Forms.Label label1;
		private OpenDental.TableAutoItem tbAutoItem;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.CheckBox checkLessIntrusive;
		///<summary></summary>
    public bool IsNew;
		///<summary>Set this before opening the form.</summary>
		public AutoCode AutoCodeCur;
		List<AutoCodeItem> listForCode;
		private List<AutoCodeCond> _listAutoCodeConds;

		///<summary></summary>
		public FormAutoCodeEdit(){
			InitializeComponent();
      tbAutoItem.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(tbAutoItem_CellDoubleClicked);
			Lan.F(this);
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

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoCodeEdit));
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textDescript = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbAutoItem = new OpenDental.TableAutoItem();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.checkLessIntrusive = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkHidden
			// 
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(390, 14);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(124, 18);
			this.checkHidden.TabIndex = 1;
			this.checkHidden.Text = "Hidden";
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(682, 528);
			this.butCancel.Name = "butCancel";
			this.butCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 20;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(682, 494);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 19;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textDescript
			// 
			this.textDescript.Location = new System.Drawing.Point(148, 16);
			this.textDescript.Name = "textDescript";
			this.textDescript.Size = new System.Drawing.Size(210, 20);
			this.textDescript.TabIndex = 22;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 16);
			this.label1.TabIndex = 23;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tbAutoItem
			// 
			this.tbAutoItem.BackColor = System.Drawing.SystemColors.Window;
			this.tbAutoItem.Location = new System.Drawing.Point(36, 94);
			this.tbAutoItem.Name = "tbAutoItem";
			this.tbAutoItem.ScrollValue = 1;
			this.tbAutoItem.SelectedIndices = new int[0];
			this.tbAutoItem.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.tbAutoItem.Size = new System.Drawing.Size(719, 356);
			this.tbAutoItem.TabIndex = 24;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(402, 496);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(246, 56);
			this.label2.TabIndex = 26;
			this.label2.Text = "Clicking Cancel does not undo changes already made to items.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(36, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(436, 18);
			this.label3.TabIndex = 27;
			this.label3.Text = "You may have duplicate codes  in the following list.";
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(158, 478);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(88, 26);
			this.butDelete.TabIndex = 29;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(36, 478);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(84, 26);
			this.butAdd.TabIndex = 28;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// checkLessIntrusive
			// 
			this.checkLessIntrusive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkLessIntrusive.Location = new System.Drawing.Point(390, 34);
			this.checkLessIntrusive.Name = "checkLessIntrusive";
			this.checkLessIntrusive.Size = new System.Drawing.Size(354, 30);
			this.checkLessIntrusive.TabIndex = 30;
			this.checkLessIntrusive.Text = "Do not check codes in the procedure edit window, but only use this auto code for " +
    "procedure buttons.";
			this.checkLessIntrusive.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			// 
			// FormAutoCodeEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(794, 582);
			this.Controls.Add(this.checkLessIntrusive);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbAutoItem);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textDescript);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.checkHidden);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAutoCodeEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormAutoCodeEdit";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormAutoCodeEdit_Closing);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAutoCodeEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormAutoCodeEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAutoCodeEdit_Load(object sender, System.EventArgs e) {      
      if(IsNew){
        this.Text=Lan.g(this,"Add Auto Code");
      }
      else{
        this.Text=Lan.g(this,"Edit Auto Code");
        textDescript.Text=AutoCodeCur.Description;
        checkHidden.Checked=AutoCodeCur.IsHidden;
				checkLessIntrusive.Checked=AutoCodeCur.LessIntrusive;
      }
		  FillTable();
		}

    private void FillTable(){
      int count=0;
			AutoCodeItems.RefreshCache();
			AutoCodeConds.RefreshCache();
			_listAutoCodeConds=AutoCodeConds.GetDeepCopy();
			listForCode=AutoCodeItems.GetListForCode(AutoCodeCur.AutoCodeNum);
			tbAutoItem.ResetRows(listForCode.Count);
			tbAutoItem.SetGridColor(Color.Gray);
			tbAutoItem.SetBackGColor(Color.White);
			for(int i=0;i<listForCode.Count;i++) {
        tbAutoItem.Cell[0,i]=ProcedureCodes.GetProcCode(listForCode[i].CodeNum).ProcCode;
				tbAutoItem.Cell[1,i]=ProcedureCodes.GetProcCode(listForCode[i].CodeNum).Descript;
        count=0;
        for(int j=0;j<_listAutoCodeConds.Count;j++){
          if(_listAutoCodeConds[j].AutoCodeItemNum==listForCode[i].AutoCodeItemNum){
						if(count!=0){
							tbAutoItem.Cell[2,i]+=", ";
						}
						tbAutoItem.Cell[2,i]+=_listAutoCodeConds[j].Cond.ToString();
            count++;
          }
        }
			}
			tbAutoItem.LayoutTables();  
    }

    private void tbAutoItem_CellDoubleClicked(object sender, CellEventArgs e){
      AutoCodeItem AutoCodeItemCur=listForCode[tbAutoItem.SelectedRow];
      FormAutoItemEdit FormAIE=new FormAutoItemEdit();
			FormAIE.AutoCodeItemCur=AutoCodeItemCur;
      FormAIE.ShowDialog();
      FillTable(); 
    }

		private void butAdd_Click(object sender, System.EventArgs e) {
		  FormAutoItemEdit FormAIE=new FormAutoItemEdit();
      FormAIE.IsNew=true;
			FormAIE.AutoCodeItemCur=new AutoCodeItem();
			FormAIE.AutoCodeItemCur.AutoCodeNum=AutoCodeCur.AutoCodeNum;
      FormAIE.ShowDialog();
      FillTable();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(tbAutoItem.SelectedRow==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
        return;
			}
			AutoCodeItem AutoCodeItemCur=listForCode[tbAutoItem.SelectedRow];
      AutoCodeConds.DeleteForItemNum(AutoCodeItemCur.AutoCodeItemNum);
      AutoCodeItems.Delete(AutoCodeItemCur);
			FillTable();
		}  

		private void butOK_Click(object sender, System.EventArgs e) {
		  if(textDescript.Text==""){
        MessageBox.Show(Lan.g(this,"The Description cannot be blank"));
        return;
      }
			if(listForCode.Count==0){
				MsgBox.Show(this,"Must have at least one item in the list.");
				//This is not actually rigorous enough since items will already be deleted.
				return;
			}
      AutoCodeCur.Description=textDescript.Text;
      AutoCodeCur.IsHidden=checkHidden.Checked;
			AutoCodeCur.LessIntrusive=checkLessIntrusive.Checked;
			AutoCodes.Update(AutoCodeCur);
      DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {	
      DialogResult=DialogResult.Cancel;
		}

		private void FormAutoCodeEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK)
				return;
			if(IsNew){
				try {
					AutoCodes.Delete(AutoCodeCur);
				}
				catch(ApplicationException ex) {
					//should never happen
					MessageBox.Show(ex.Message);
					return;
				}
      }
		}

		private void FormAutoCodeEdit_FormClosing(object sender,FormClosingEventArgs e) {
			AutoCodeItems.RefreshCache();
			AutoCodeConds.RefreshCache();
			if(listForCode.Count==0) {//No entries. Let them close.
				return;
			}
			for(int i=0;i<listForCode.Count;i++) {//Attach the conditions to the items for better organization
				listForCode[i].ListConditions=new List<AutoCodeCond>();
        for(int j=0;j<_listAutoCodeConds.Count;j++){//Fill conditions for this AutoCodeItem
          if(_listAutoCodeConds[j].AutoCodeItemNum==listForCode[i].AutoCodeItemNum){
						listForCode[i].ListConditions.Add(_listAutoCodeConds[j]);
          }
        }
			}
			//Must have same number of conditions for each AutoCodeItem.----------------------------------------------------------------------------------
			for(int i=1;i<listForCode.Count;i++) {//start at 1 and compare to the 0 index.
				if(listForCode[i].ListConditions.Count!=listForCode[0].ListConditions.Count) {
					MsgBox.Show(this,"All AutoCode items must have the same number of conditions.");
					e.Cancel=true;
					return;
				}
			}
			if(listForCode[0].ListConditions.Count==0) {//Rest of the checks assume at least one condition.
				return;
			}
			//Check for duplicate AutoCodeItem condition lists.-------------------------------------------------------------------------------------------
			for(int i=1;i<listForCode.Count;i++) {//start at 1
				for(int j=0;j<i;j++) {//loop through the lower-indexed entries
					int matches=0;
					for(int k=0;k<listForCode[i].ListConditions.Count;k++) {//For each condition in i, check for matches with conditions in j
						if(listForCode[i].ListConditions[k].Cond==listForCode[j].ListConditions[k].Cond) {//if the same condition is in both rows.
							matches++;
						}
					}
					if(matches==listForCode[i].ListConditions.Count) {//If the number of matches equals the number of conditions on this row
						MsgBox.Show(this,"Cannot have two AutoCode Items with duplicate conditions.");
						e.Cancel=true;
						return;
					}
				}
			}		
			//Decide which categories are involved.------------------------------------------------------------------------------------------------------
			bool isAnt=false;//Not a category, could be isAntPost or isAntPreMol
			bool isAntPost=false;
			bool isAntPreMol=false;//Anterior/premolar/molar
			bool isNumSurf=false;
			bool isFirstEachAdd=false;
			bool isMaxMand=false;
			bool isPriPerm=false;
			bool isPontRet=false;
			for(int i=0;i<listForCode.Count;i++) {
				//If the item matches the category, set the boolean to true.
				for(int j=0;j<listForCode[i].ListConditions.Count;j++) {
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.Anterior) {
						isAnt=true;
						//We want to also set either isAntPost or isAntPreMol, but we don't have enough information yet to set that.
						continue;
					}
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.Posterior) {
						isAntPost=true;
						continue;
					}
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.Premolar
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Molar
						) {
						isAntPreMol=true;
						continue;
					}
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.One_Surf
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Two_Surf
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Three_Surf
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Four_Surf
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Five_Surf
						) {
						isNumSurf=true;
						continue;
					}
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.First
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.EachAdditional
						) {
						isFirstEachAdd=true;
						continue;
					}
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.Maxillary
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Mandibular
						) {
						isMaxMand=true;
						continue;
					}
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.Primary
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Permanent
						) {
						isPriPerm=true;
						continue;
					}
					if(listForCode[i].ListConditions[j].Cond==AutoCondition.Pontic
						|| listForCode[i].ListConditions[j].Cond==AutoCondition.Retainer
						) {
						isPontRet=true;
						continue;
					}
				}
			}
			//After the loop, you had better have exactly the same number of booleans true as number of conditions on each item.--------------------
			if(isAntPost && isAntPreMol) {
				MsgBox.Show(this,"Cannot have both Posterior and Premolar/Molar categories.");
				e.Cancel=true;
				return;
			}
			if(isAnt) {//This is the only purpose of the isAnt bool.  We won't use it anymore.
				if(!isAntPost && !isAntPreMol) {
					MsgBox.Show(this,"Anterior condition is present without any corresponding posterior or premolar/molar condition.");
					e.Cancel=true;
					return;
				}
			}
			//Count how many categories were hit.
			int numCategories=0;
			if(isAntPost) {
				numCategories++;
			}
			if(isAntPreMol) {
				numCategories++;
			}
			if(isNumSurf) {
				numCategories++;
			}
			if(isFirstEachAdd) {
				numCategories++;
			}
			if(isMaxMand) {
				numCategories++;
			}
			if(isPriPerm) {
				numCategories++;
			}
			if(isPontRet) {
				numCategories++;
			}
			if(numCategories!=listForCode[0].ListConditions.Count) {//Every row has to have the same number of conditions
				MessageBox.Show(Lan.g(this,"When using ")+listForCode[0].ListConditions.Count+Lan.g(this," condition(s), you must use conditions from ")											
					+listForCode[0].ListConditions.Count+Lan.g(this," logical categories. You are using conditions from ")+numCategories+Lan.g(this," logical categories."));
				e.Cancel=true;
				return;
			}
			//Make sure that the number of AutoCodeItems is right. For example, if isAntPost and isNumSurf are the only true one, there should be 10 items.----------------------------------------
			int reqNumAutoCodeItems=1;
			if(isAntPost) {
				reqNumAutoCodeItems=reqNumAutoCodeItems*2;
			}
			if(isAntPreMol) {
				if(isPriPerm) {
					reqNumAutoCodeItems=reqNumAutoCodeItems*5;//normally this would be 2*3 but primary molars don't exist, so we have 2*3-1=5
				}
				else {
					reqNumAutoCodeItems=reqNumAutoCodeItems*3;
				}
			}
			else {
				if(isPriPerm) {
					reqNumAutoCodeItems=reqNumAutoCodeItems*2;
				}
			}
			if(isNumSurf) {
				reqNumAutoCodeItems=reqNumAutoCodeItems*5;
			}
			if(isFirstEachAdd) {
				reqNumAutoCodeItems=reqNumAutoCodeItems*2;
			}
			if(isMaxMand) {
				reqNumAutoCodeItems=reqNumAutoCodeItems*2;
			}
			if(isPontRet) {
				reqNumAutoCodeItems=reqNumAutoCodeItems*2;
			}
			if(listForCode.Count!=reqNumAutoCodeItems) {
				MessageBox.Show(Lan.g(this,"For the condition categories you are using, you should have ")
					+reqNumAutoCodeItems+Lan.g(this," entries in your list. You have ")+listForCode.Count+".");
				e.Cancel=true;
				return;
			}
		}
	}
}

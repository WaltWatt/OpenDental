using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormRpReferrals : ODForm	{
		private System.Windows.Forms.TabPage tabData;
		private OpenDental.UI.Button butCheckAll;
		private OpenDental.UI.Button butClear;
		private System.Windows.Forms.TabPage tabFilters;
		private System.Windows.Forms.ListBox comboBox;
		private System.Windows.Forms.ListBox ListPrerequisites;
		private OpenDental.UI.Button butAddFilter;
		private System.Windows.Forms.ListBox ListConditions;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.ComboBox DropListFilter;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.TextBox textSQL;
		private System.Windows.Forms.TabControl tabReferrals;
		private System.ComponentModel.Container components = null;

		private FormQuery FormQuery2;
		private string SQLselect;
		private string SQLfrom;
		private string SQLwhere;
		private bool isText;
		private bool isDropDown;
		private string sItem;//just used in local loops
		private string[]  AfieldsSelected;
		private ArrayList ALrefSelect;
		private System.Windows.Forms.ListBox listSelect;
		private OpenDental.UI.Button butDeleteFilter;  //fields used in SELECT 
		private ArrayList ALrefFilter;

		///<summary></summary>
		public FormRpReferrals(){
			InitializeComponent();
			Text=Lan.g(this,"Referral Report");
      ALrefSelect=new ArrayList();
			ALrefFilter=new ArrayList();
      Fill();
			SQLselect="";
			SQLfrom="FROM referral ";
			SQLwhere="";
			ListConditions.SelectedIndex=0;
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpReferrals));
			this.tabReferrals = new System.Windows.Forms.TabControl();
			this.tabData = new System.Windows.Forms.TabPage();
			this.listSelect = new System.Windows.Forms.ListBox();
			this.butCheckAll = new OpenDental.UI.Button();
			this.butClear = new OpenDental.UI.Button();
			this.tabFilters = new System.Windows.Forms.TabPage();
			this.butDeleteFilter = new OpenDental.UI.Button();
			this.comboBox = new System.Windows.Forms.ListBox();
			this.ListPrerequisites = new System.Windows.Forms.ListBox();
			this.butAddFilter = new OpenDental.UI.Button();
			this.ListConditions = new System.Windows.Forms.ListBox();
			this.textBox = new System.Windows.Forms.TextBox();
			this.DropListFilter = new System.Windows.Forms.ComboBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textSQL = new System.Windows.Forms.TextBox();
			this.tabReferrals.SuspendLayout();
			this.tabData.SuspendLayout();
			this.tabFilters.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabReferrals
			// 
			this.tabReferrals.Controls.Add(this.tabData);
			this.tabReferrals.Controls.Add(this.tabFilters);
			this.tabReferrals.Location = new System.Drawing.Point(14,16);
			this.tabReferrals.Name = "tabReferrals";
			this.tabReferrals.SelectedIndex = 0;
			this.tabReferrals.Size = new System.Drawing.Size(814,492);
			this.tabReferrals.TabIndex = 39;
			// 
			// tabData
			// 
			this.tabData.Controls.Add(this.listSelect);
			this.tabData.Controls.Add(this.butCheckAll);
			this.tabData.Controls.Add(this.butClear);
			this.tabData.Location = new System.Drawing.Point(4,22);
			this.tabData.Name = "tabData";
			this.tabData.Size = new System.Drawing.Size(806,466);
			this.tabData.TabIndex = 1;
			this.tabData.Text = "SELECT";
			// 
			// listSelect
			// 
			this.listSelect.Location = new System.Drawing.Point(8,8);
			this.listSelect.Name = "listSelect";
			this.listSelect.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listSelect.Size = new System.Drawing.Size(184,407);
			this.listSelect.TabIndex = 3;
			this.listSelect.SelectedIndexChanged += new System.EventHandler(this.listSelect_SelectedIndexChanged);
			// 
			// butCheckAll
			// 
			this.butCheckAll.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCheckAll.Autosize = true;
			this.butCheckAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCheckAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCheckAll.CornerRadius = 4F;
			this.butCheckAll.Location = new System.Drawing.Point(10,430);
			this.butCheckAll.Name = "butCheckAll";
			this.butCheckAll.Size = new System.Drawing.Size(80,26);
			this.butCheckAll.TabIndex = 1;
			this.butCheckAll.Text = "&All";
			this.butCheckAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Location = new System.Drawing.Point(100,430);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(80,26);
			this.butClear.TabIndex = 2;
			this.butClear.Text = "&None";
			this.butClear.Click += new System.EventHandler(this.butNone_Click);
			// 
			// tabFilters
			// 
			this.tabFilters.Controls.Add(this.butDeleteFilter);
			this.tabFilters.Controls.Add(this.comboBox);
			this.tabFilters.Controls.Add(this.ListPrerequisites);
			this.tabFilters.Controls.Add(this.butAddFilter);
			this.tabFilters.Controls.Add(this.ListConditions);
			this.tabFilters.Controls.Add(this.textBox);
			this.tabFilters.Controls.Add(this.DropListFilter);
			this.tabFilters.Location = new System.Drawing.Point(4,22);
			this.tabFilters.Name = "tabFilters";
			this.tabFilters.Size = new System.Drawing.Size(806,466);
			this.tabFilters.TabIndex = 0;
			this.tabFilters.Text = "WHERE";
			this.tabFilters.Visible = false;
			// 
			// butDeleteFilter
			// 
			this.butDeleteFilter.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDeleteFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDeleteFilter.Autosize = true;
			this.butDeleteFilter.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeleteFilter.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeleteFilter.CornerRadius = 4F;
			this.butDeleteFilter.Image = ((System.Drawing.Image)(resources.GetObject("butDeleteFilter.Image")));
			this.butDeleteFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteFilter.Location = new System.Drawing.Point(10,426);
			this.butDeleteFilter.Name = "butDeleteFilter";
			this.butDeleteFilter.Size = new System.Drawing.Size(110,26);
			this.butDeleteFilter.TabIndex = 34;
			this.butDeleteFilter.Text = "&Delete Row";
			this.butDeleteFilter.Click += new System.EventHandler(this.butDeleteFilter_Click);
			// 
			// comboBox
			// 
			this.comboBox.Location = new System.Drawing.Point(358,12);
			this.comboBox.Name = "comboBox";
			this.comboBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.comboBox.Size = new System.Drawing.Size(266,199);
			this.comboBox.TabIndex = 12;
			this.comboBox.Visible = false;
			this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
			// 
			// ListPrerequisites
			// 
			this.ListPrerequisites.Location = new System.Drawing.Point(10,234);
			this.ListPrerequisites.Name = "ListPrerequisites";
			this.ListPrerequisites.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.ListPrerequisites.Size = new System.Drawing.Size(608,173);
			this.ListPrerequisites.TabIndex = 7;
			this.ListPrerequisites.SelectedIndexChanged += new System.EventHandler(this.ListPrerequisites_SelectedIndexChanged);
			// 
			// butAddFilter
			// 
			this.butAddFilter.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAddFilter.Autosize = true;
			this.butAddFilter.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddFilter.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddFilter.CornerRadius = 4F;
			this.butAddFilter.Enabled = false;
			this.butAddFilter.Location = new System.Drawing.Point(664,12);
			this.butAddFilter.Name = "butAddFilter";
			this.butAddFilter.Size = new System.Drawing.Size(75,23);
			this.butAddFilter.TabIndex = 6;
			this.butAddFilter.Text = "&Add";
			this.butAddFilter.Click += new System.EventHandler(this.butAddFilter_Click);
			// 
			// ListConditions
			// 
			this.ListConditions.Items.AddRange(new object[] {
            "LIKE",
            "=",
            ">",
            "<",
            ">=",
            "<=",
            "<>"});
			this.ListConditions.Location = new System.Drawing.Point(232,12);
			this.ListConditions.Name = "ListConditions";
			this.ListConditions.Size = new System.Drawing.Size(78,95);
			this.ListConditions.TabIndex = 5;
			// 
			// textBox
			// 
			this.textBox.Location = new System.Drawing.Point(358,12);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(262,20);
			this.textBox.TabIndex = 2;
			this.textBox.Visible = false;
			// 
			// DropListFilter
			// 
			this.DropListFilter.Location = new System.Drawing.Point(8,12);
			this.DropListFilter.MaxDropDownItems = 45;
			this.DropListFilter.Name = "DropListFilter";
			this.DropListFilter.Size = new System.Drawing.Size(172,21);
			this.DropListFilter.TabIndex = 1;
			this.DropListFilter.Text = "WHERE";
			this.DropListFilter.SelectedIndexChanged += new System.EventHandler(this.DropListFilter_SelectedIndexChanged);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(750,640);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 41;
			this.butCancel.Text = "&Cancel";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Enabled = false;
			this.butOK.Location = new System.Drawing.Point(750,602);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 40;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textSQL
			// 
			this.textSQL.Location = new System.Drawing.Point(16,542);
			this.textSQL.Multiline = true;
			this.textSQL.Name = "textSQL";
			this.textSQL.ReadOnly = true;
			this.textSQL.Size = new System.Drawing.Size(692,124);
			this.textSQL.TabIndex = 42;
			// 
			// FormRpReferrals
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(842,683);
			this.Controls.Add(this.tabReferrals);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.textSQL);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRpReferrals";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormRpReferrals";
			this.tabReferrals.ResumeLayout(false);
			this.tabData.ResumeLayout(false);
			this.tabFilters.ResumeLayout(false);
			this.tabFilters.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void Fill()  {
			FillALrefSelect();
			FillALrefFilter();
			FillSelectList();
			FillDropListFilter();
		}
 
		private void FillALrefSelect(){
      ALrefSelect.Add("Address"); 
      ALrefSelect.Add("Address2"); 
      ALrefSelect.Add("City");
      ALrefSelect.Add("Email");  
      ALrefSelect.Add("FName"); 
      ALrefSelect.Add("IsHidden"); 
      ALrefSelect.Add("LName");
      ALrefSelect.Add("MName"); 
      ALrefSelect.Add("Note");  
      ALrefSelect.Add("NotPerson");   
      ALrefSelect.Add("Phone2");
			ALrefSelect.Add("ReferralNum"); 
      ALrefSelect.Add("Specialty"); 
      ALrefSelect.Add("SSN"); 
      ALrefSelect.Add("ST"); 
      ALrefSelect.Add("Telephone");
      ALrefSelect.Add("Title"); 
      ALrefSelect.Add("UsingTIN");
      ALrefSelect.Add("Zip");
 		}

		private void FillALrefFilter(){//FillALrefFilter
      ALrefFilter.Add("Address"); 
      ALrefFilter.Add("Address2"); 
      ALrefFilter.Add("City");
      ALrefFilter.Add("Email");  
      ALrefFilter.Add("FName"); 
      ALrefFilter.Add("IsHidden"); 
      ALrefFilter.Add("LName");
      ALrefFilter.Add("MName");
      ALrefFilter.Add("Note");  
      ALrefFilter.Add("NotPerson");   
      ALrefFilter.Add("Phone2");
			ALrefFilter.Add("ReferralNum"); 
      ALrefFilter.Add("Specialty"); 
      ALrefFilter.Add("SSN"); 
      ALrefFilter.Add("ST"); 
      ALrefFilter.Add("Telephone");
      ALrefFilter.Add("Title"); 
      ALrefFilter.Add("UsingTIN");
      ALrefFilter.Add("Zip");  
		}
	  
		private void FillSelectList(){
      for(int i=0;i<ALrefSelect.Count;i++){
			  listSelect.Items.Add(ALrefSelect[i]);
			}
			SQLselect="";
		}

		private void FillDropListFilter(){
      for(int i=0;i<ALrefFilter.Count;i++){
			  DropListFilter.Items.Add(ALrefFilter[i]);
			}
		}
    
		private void fillSQLbox(){
      textSQL.Text=SQLselect+SQLfrom+SQLwhere;
		}

    private void createSQLfrom(){
      AfieldsSelected = new string[listSelect.SelectedItems.Count]; 
      if(listSelect.SelectedItems.Count > 0){
				SQLselect="SELECT ";
				listSelect.SelectedItems.CopyTo(AfieldsSelected,0);
				for(int i=0;i<AfieldsSelected.Length;i++){
					if(i!=AfieldsSelected.Length-1)
						SQLselect+=AfieldsSelected[i].ToString()+",";
					else
						SQLselect+=AfieldsSelected[i].ToString()+" ";
				}
		  	fillSQLbox();  
				butOK.Enabled=true;
			}
			else{
		    SQLselect="";
				fillSQLbox();
				butOK.Enabled=false;
			}
		}

		private void listSelect_SelectedIndexChanged(object sender, System.EventArgs e) {
      createSQLfrom();
		}

    private void createSQLwhere(){
			SQLwhere="WHERE ";
			for(int i=0;i<ListPrerequisites.Items.Count;i++){
				if(i==0)
					SQLwhere+=ListPrerequisites.Items[i].ToString();
				else if(ListPrerequisites.Items[i].ToString().Substring(0,2)=="OR")
					SQLwhere+=" "+ListPrerequisites.Items[i].ToString();
				else
					SQLwhere+=" AND "+ListPrerequisites.Items[i].ToString();
			}
			fillSQLbox();
			if(listSelect.SelectedItems.Count > 0)
				butOK.Enabled=true;
		}

		
		
		private void DropListFilter_SelectedIndexChanged(object sender, System.EventArgs e) {
			switch(DropListFilter.SelectedItem.ToString()){
   		  case "Address":
   		  case "Address2":
   		  case "City":
        case "EMail":
        case "FName":
  		  case "LName":
   		  case "MName":
        case "Note":
   		  case "Phone2":
        case "ReferralNum":
   		  case "ST":
   		  case "SSN":
   		  case "Telephone":
   		  case "Title":
   		  case "Zip":
					textBox.Clear();
			    ListConditions.Enabled=true;
          textBox.Visible=true;
					comboBox.Visible=false;
					textBox.Select();
					isText=true;
					isDropDown=false;
    			butAddFilter.Enabled=true;
					break;
   		  case "Specialty":
          setListBoxConditions();
          comboBox.Items.Clear();
					Def[] specDefs=Defs.GetDefsForCategory(DefCat.ProviderSpecialties,true).ToArray();
					for(int i=0;i<specDefs.Length;i++) {
						comboBox.Items.Add(Lan.g("enumDentalSpecialty",specDefs[i].ItemName));
					}
					break;
   		  case "UsingTIN":
        case "IsHidden":
        case "NotPerson": 
          setListBoxConditions();
					comboBox.Items.Clear();
					comboBox.Items.Add("False");
					comboBox.Items.Add("True");
					break;
			}
		}

		private void setListBoxConditions(){
			comboBox.Visible=true;
      textBox.Visible=false;
			isDropDown=true;
			isText=false;
			ListConditions.Enabled=true;
			comboBox.SelectedIndex=-1;
			butAddFilter.Enabled=false;
		}

		private void butAddFilter_Click(object sender, System.EventArgs e) {
			if(isText){
				if(ListConditions.SelectedIndex==0){
					ListPrerequisites.Items.Add
						(DropListFilter.SelectedItem.ToString()+" LIKE '%"+textBox.Text+"%'");
				}
				else{
					ListPrerequisites.Items.Add(DropListFilter.SelectedItem.ToString()+" "
						+ListConditions.SelectedItem.ToString()+" '"+textBox.Text+"'");
				}
  		}//end if(isText)
			else if(isDropDown){
				if(DropListFilter.SelectedItem.ToString()=="Specialty"){
					sItem="";
					Def[] specDefs=Defs.GetDefsForCategory(DefCat.ProviderSpecialties,true).ToArray();
					for(int i=0;i<comboBox.SelectedIndices.Count;i++){
						if(i==0){ 
              sItem="(";
            }
						else{ 
              sItem="OR ";
            }
						sItem+="Specialty "+ListConditions.SelectedItem.ToString()+" '"+
							specDefs[comboBox.SelectedIndices[i]].DefNum.ToString()+"'"; 
						if(i==comboBox.SelectedIndices.Count-1)
							sItem+=")";
						ListPrerequisites.Items.Add(sItem);
					}
        }
				else{
          for(int i=0;i<comboBox.SelectedItems.Count;i++){
						if(ListConditions.SelectedIndex==0){ 
							ListPrerequisites.Items.Add(DropListFilter.SelectedItem.ToString()+" LIKE '%"
								+comboBox.SelectedIndices[i].ToString()+"%'");  
						}
						else{
							ListPrerequisites.Items.Add(DropListFilter.SelectedItem.ToString()+" "
								+ListConditions.SelectedItem.ToString()+" '"
								+comboBox.SelectedIndices[i].ToString()+"'");   
						}
					}
 				}
				comboBox.SelectedIndex=-1;
				butAddFilter.Enabled=false;
			}//end else if(isDropDown)
			createSQLwhere();
			ListConditions.Enabled=true;
			textBox.Clear();
		}

		private void butDeleteFilter_Click(object sender, System.EventArgs e) {
      while(ListPrerequisites.SelectedIndices.Count > 0){ 
				ListPrerequisites.Items.RemoveAt(ListPrerequisites.SelectedIndices[0]);
			}
			if(ListPrerequisites.Items.Count > 0){
		    createSQLwhere();
      }
			else{
        SQLwhere="";
			}
      fillSQLbox();  
		}

		private void butAll_Click(object sender, System.EventArgs e){		
      for(int i=0;i<listSelect.Items.Count;i++){
				listSelect.SetSelected(i,true);
			}
  		createSQLfrom();	
		}

		private void butNone_Click(object sender, System.EventArgs e) {
      for(int i=0;i<listSelect.Items.Count;i++){
				listSelect.SetSelected(i,false);
			}
  		createSQLfrom();	
		}

		private void comboBox_SelectedIndexChanged(object sender, System.EventArgs e) {
			if(comboBox.SelectedItems.Count > 0)
				butAddFilter.Enabled=true;
			else
				butAddFilter.Enabled=false;
		}

		private void ListPrerequisites_SelectedIndexChanged(object sender, System.EventArgs e) {
			butDeleteFilter.Enabled=false;
			if(ListPrerequisites.Items.Count > 0 && ListPrerequisites.SelectedItems.Count > 0){
				butDeleteFilter.Enabled=true;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			ReportSimpleGrid report=new ReportSimpleGrid();
			report.Query=textSQL.Text;
			FormQuery2=new FormQuery(report);
			FormQuery2.IsReport=false;
			FormQuery2.SubmitQuery();	
      FormQuery2.textQuery.Text=report.Query;					
			FormQuery2.ShowDialog();
			//DialogResult=DialogResult.OK;				

		}



	}
}







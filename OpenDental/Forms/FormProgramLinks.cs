using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing;

namespace OpenDental{
	///<summary></summary>
	public class FormProgramLinks : ODForm {
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;// Required designer variable.
		private Programs Programs=new Programs();
		private Label label2;
		private Label label1;
		private bool changed;
		private UI.ODGrid gridProgram;
		private List<Program> _listPrograms;

		///<summary></summary>
		public FormProgramLinks(){
			InitializeComponent();// Required for Windows Form Designer support
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgramLinks));
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.gridProgram = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(372, 631);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 38;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(17, 631);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 26);
			this.butAdd.TabIndex = 41;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(15, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(293, 27);
			this.label2.TabIndex = 43;
			this.label2.Text = "Double click on one of the programs in the\r\nlist below to enable it or change its" +
    " settings";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(97, 630);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 29);
			this.label1.TabIndex = 44;
			this.label1.Text = "Do not Add unless you have a totally\r\ncustom bridge which we don\'t support.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridProgram
			// 
			this.gridProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProgram.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridProgram.HasAddButton = false;
			this.gridProgram.HasDropDowns = false;
			this.gridProgram.HasMultilineHeaders = false;
			this.gridProgram.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridProgram.HeaderHeight = 15;
			this.gridProgram.HScrollVisible = false;
			this.gridProgram.Location = new System.Drawing.Point(18, 42);
			this.gridProgram.Name = "gridProgram";
			this.gridProgram.ScrollValue = 0;
			this.gridProgram.Size = new System.Drawing.Size(429, 575);
			this.gridProgram.TabIndex = 45;
			this.gridProgram.Title = "Programs";
			this.gridProgram.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridProgram.TitleHeight = 18;
			this.gridProgram.TranslationName = "TablePrograms";
			this.gridProgram.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProgram_CellDoubleClick);
			// 
			// FormProgramLinks
			// 
			this.ClientSize = new System.Drawing.Size(464, 669);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.gridProgram);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(405, 195);
			this.Name = "FormProgramLinks";
			this.ShowInTaskbar = false;
			this.Text = "Program Links";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProgramLinks_Closing);
			this.Load += new System.EventHandler(this.FormProgramLinks_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProgramLinks_Load(object sender, System.EventArgs e) {
			FillList();
		}

		private void FillList(){
			Programs.RefreshCache();
			_listPrograms=Programs.GetListDeep();
			gridProgram.BeginUpdate();
			gridProgram.Columns.Clear();
			gridProgram.Columns.Add(new ODGridColumn("Enabled",55,HorizontalAlignment.Center));
			gridProgram.Columns.Add(new ODGridColumn("Program Name",-1));
			gridProgram.Rows.Clear();
			foreach(Program prog in _listPrograms){
				ODGridRow row=new ODGridRow() { Tag=prog };
				Color color = Color.FromArgb(230, 255, 238);
				row.ColorBackG=prog.Enabled ? color : row.ColorBackG;
				ODGridCell cell=new ODGridCell(prog.Enabled ? "X" : "");
				row.Cells.Add(cell);
				row.Cells.Add(prog.ProgDesc);
				gridProgram.Rows.Add(row);
			}
			gridProgram.EndUpdate();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormProgramLinkEdit FormPE=new FormProgramLinkEdit();
			FormPE.IsNew=true;
			FormPE.ProgramCur=new Program();
			FormPE.ShowDialog();
			changed=true;//because we don't really know what they did, so assume changed.
			FillList();
		}

		private void gridProgram_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DialogResult dResult=DialogResult.None;
			Program program=_listPrograms[gridProgram.GetSelectedIndex()].Copy();
			switch(program.ProgName) {
				case "UAppoint":
					FormUAppoint FormU=new FormUAppoint();
					FormU.ProgramCur=program;
					dResult=FormU.ShowDialog();
					break;
				case "eClinicalWorks":
					if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
						break;
					}
					FormEClinicalWorks FormECW=new FormEClinicalWorks();
					FormECW.ProgramCur=program;
					dResult=FormECW.ShowDialog();
					break;
				case "eRx":
					FormErxSetup FormES=new FormErxSetup();
					dResult=FormES.ShowDialog();
					break;
				case "Mountainside":
					FormMountainside FormM=new FormMountainside();
					FormM.ProgramCur=program;
					dResult=FormM.ShowDialog();
					break;
				case "PayConnect":
					FormPayConnectSetup fpcs=new FormPayConnectSetup();
					dResult=fpcs.ShowDialog();
					break;
				case "Podium":
					FormPodiumSetup FormPS=new FormPodiumSetup();
					dResult=FormPS.ShowDialog();
					break;
				case "Xcharge":
					FormXchargeSetup fxcs=new FormXchargeSetup();
					dResult=fxcs.ShowDialog();
					break;
				case "FHIR":
					FormFHIRSetup FormFS=new FormFHIRSetup();
					dResult=FormFS.ShowDialog();
					break;
				case "Transworld":
					FormTransworldSetup FormTs=new FormTransworldSetup();
					dResult=FormTs.ShowDialog();
					break;
				case "PaySimple":
					FormPaySimpleSetup formPS=new FormPaySimpleSetup();
					dResult=formPS.ShowDialog();
					break;
				default:
					FormProgramLinkEdit FormPE=new FormProgramLinkEdit();
					if(Programs.IsStatic(program)) {
						FormPE.AllowToolbarChanges=false;
					}
					FormPE.ProgramCur=program;
					dResult=FormPE.ShowDialog();
				break;
			}
			if(dResult==DialogResult.OK) {
				changed=true;
				FillList();
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormProgramLinks_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.Programs, InvalidType.ToolBut);
			}
		}
	}
}

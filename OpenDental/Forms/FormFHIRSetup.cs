using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormFHIRSetup:ODForm {
		///<summary>A local copy of the API keys obtained from ODHQ.</summary>
		private List<APIKey> _listApiKeysLocal;
		///<summary>The keys that are obtained from ODHQ. When closing the form, if any API key information has changed, a call will be made to ODHQ to
		///update the keys at HQ.</summary>
		private List<APIKey> _listApiKeysHQ;

		public FormFHIRSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormFHIRSetup_Load(object sender,EventArgs e) {
			Program prog=Programs.GetCur(ProgramName.FHIR);
			checkEnabled.Checked=prog.Enabled;
			textSubInterval.Text=ProgramProperties.GetPropVal(prog.ProgramNum,"SubscriptionProcessingFrequency");
			Cursor=Cursors.WaitCursor;
			_listApiKeysHQ=GetApiKeys();
			Cursor=Cursors.Default;
			if(_listApiKeysHQ==null) {
				DialogResult=DialogResult.Cancel;//We have already shown them an error message.
				return;
			}
			_listApiKeysLocal=_listApiKeysHQ.Select(x => x.Copy()).ToList();
			FillGrid();
			FillPermissions();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Developer"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"API Key"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Notes"),150);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listApiKeysLocal.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listApiKeysLocal[i].DeveloperName);
				row.Cells.Add(_listApiKeysLocal[i].Key);
				switch(_listApiKeysLocal[i].KeyStatus) {
					case APIKeyStatus.ReadEnabled:
						row.Cells.Add(Lan.g(this,"This key is authorized for read operations only."));
						break;
					case APIKeyStatus.WritePending:
						row.Cells.Add(Lan.g(this,"This key is pending approval for write operations. Read operations are authorized."));
						break;
					case APIKeyStatus.WriteDisabled:
						row.Cells.Add(Lan.g(this,"This key is disabled for write operations. Read operations are authorized."));
						break;
					case APIKeyStatus.Disabled:
						row.Cells.Add(Lan.g(this,"This key is disabled."));
						break;
					case APIKeyStatus.WriteEnabled:
					default:
						row.Cells.Add("");
						break;
				}
				row.Tag=_listApiKeysLocal[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		///<summary>Fills the permissions tree for when the form first loads.</summary>
		private void FillPermissions() {
			foreach(APIPermission perm in Enum.GetValues(typeof(APIPermission))) {
				switch(perm) {
					case APIPermission.AppointmentUpdate:
					case APIPermission.AppointmentDelete:
					case APIPermission.PatientUpdate:
					//^^These actions are not fully implemented yet.
					case APIPermission.CapabilityStatement:
					//^^An API permission is not needed to look at the conformance statement.
						continue;
					default:
						break;
				}
				TreeNode node=new TreeNode(Lan.g(this,perm.ToString()));
				node.Tag=perm;
				treePermissions.Nodes.Add(node);
			}
			treePermissions.Enabled=false;
		}

		private List<APIKey> GetApiKeys() {
			List<APIKey> listApiKeys=new List<APIKey>();
			//prepare the xml document to send--------------------------------------------------------------------------------------
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars=("    ");
			StringBuilder strbuild=new StringBuilder();
			//Send the message and get the result-------------------------------------------------------------------------------------
			string result="";
			try {
				string officeData=WebServiceMainHQProxy.CreateWebServiceHQPayload(strbuild.ToString(),eServiceCode.FHIR);
				result=WebServiceMainHQProxy.GetWebServiceMainHQInstance().GetFHIRAPIKeysForOffice(officeData);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return null;
			}
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			XPathNavigator nav=doc.CreateNavigator();
			//Process errors------------------------------------------------------------------------------------------------------------
			XPathNavigator node=nav.SelectSingleNode("//Error");
			if(node!=null) {
				MessageBox.Show(node.Value);
				return null;
			}
			//Process a valid return value------------------------------------------------------------------------------------------------{
			node=nav.SelectSingleNode("//ListAPIKeys");
			if(node!=null && node.MoveToFirstChild()) {
				do {
					APIKey apiKey=new APIKey();
					apiKey.Key=node.SelectSingleNode("APIKeyValue").Value;
					apiKey.FHIRAPIKeyNum=PIn.Long(node.SelectSingleNode("FHIRAPIKeyNum").Value);
					apiKey.DateDisabled=DateTime.Parse(node.SelectSingleNode("DateDisabled").Value);
					APIKeyStatus status;
					if(Enum.TryParse(node.SelectSingleNode("KeyStatus").Value,out status)) {
						apiKey.KeyStatus=status;
					}
					else {
						apiKey.KeyStatus=APIKeyStatus.ReadEnabled;
					}
					apiKey.DeveloperName=node.SelectSingleNode("DeveloperName").Value;
					apiKey.DeveloperEmail=node.SelectSingleNode("DeveloperEmail").Value;
					apiKey.DeveloperPhone=node.SelectSingleNode("DeveloperPhone").Value;
					apiKey.FHIRDeveloperNum=PIn.Long(node.SelectSingleNode("FHIRDeveloperNum").Value);
					XPathNavigator nodePerms=node.SelectSingleNode("ListAPIPermissions");
					if(nodePerms!=null && nodePerms.MoveToFirstChild()) {
						do {
							APIPermission perm;
							if(Enum.TryParse(nodePerms.Value,out perm)) {
								apiKey.ListPermissions.Add(perm);
							}
						} while(nodePerms.MoveToNext());
					}
					listApiKeys.Add(apiKey);
				} while(node.MoveToNext());
			}
			return listApiKeys;
		}

		private void butGenerateKey_Click(object sender,EventArgs e) {
			APIKey apiKey=GenerateNewKey();
			if(apiKey==null) {
				return;//We already showed them an error message.
			}
			_listApiKeysHQ.Add(apiKey);//Since this came from HQ, we want it in that list.
			FormFHIRAPIKeyEdit FormFAKE=new FormFHIRAPIKeyEdit();
			FormFAKE.APIKeyCur=apiKey.Copy();
			DialogResult res=FormFAKE.ShowDialog();
			if(res==DialogResult.OK) {
				_listApiKeysLocal.Add(FormFAKE.APIKeyCur);
				FillGrid();
			}
		}

		private APIKey GenerateNewKey() {
			APIKey apiKey=new APIKey();
			//prepare the xml document to send--------------------------------------------------------------------------------------
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars=("    ");
			StringBuilder strbuild=new StringBuilder();
			//Send the message and get the result-------------------------------------------------------------------------------------
			string result="";
			try {
				string officeData=WebServiceMainHQProxy.CreateWebServiceHQPayload(strbuild.ToString(),eServiceCode.FHIR);
				result=WebServiceMainHQProxy.GetWebServiceMainHQInstance().GenerateFHIRAPIKey(officeData);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return null;
			}
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			//Process errors------------------------------------------------------------------------------------------------------------
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				MessageBox.Show(node.InnerText);
				return null;
			}
			//Process a valid return value------------------------------------------------------------------------------------------------
			node=doc.SelectSingleNode("//APIKey");
			if(node!=null) {
				apiKey.Key=node.InnerText;
			}
			node=doc.SelectSingleNode("//FHIRAPIKeyNum");
			if(node!=null) {
				apiKey.FHIRAPIKeyNum=PIn.Long(node.InnerText);
			}
			node=doc.SelectSingleNode("//KeyStatus");
			if(node!=null) {
				APIKeyStatus status;
				if(Enum.TryParse(node.InnerText,out status)) {
					apiKey.KeyStatus=status;
				}
				else {
					apiKey.KeyStatus=APIKeyStatus.ReadEnabled;
				}
			}
			return apiKey;
		}

		private void butSetAll_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			APIKey apiKey=(APIKey)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			foreach(TreeNode node in treePermissions.Nodes) {
				node.ImageIndex=2;
				if(!apiKey.ListPermissions.Contains((APIPermission)node.Tag)) {//The selected key does have the permission for this node.					
					_listApiKeysLocal[gridMain.SelectedIndices[0]].ListPermissions.Add((APIPermission)node.Tag);
				}
			}
		}

		private void butUnsetAll_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			foreach(TreeNode node in treePermissions.Nodes) {
				node.ImageIndex=1;
				_listApiKeysLocal[gridMain.SelectedIndices[0]].ListPermissions.Remove((APIPermission)node.Tag);
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			SetPermissions();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormFHIRAPIKeyEdit FormFAKE=new FormFHIRAPIKeyEdit();
			FormFAKE.APIKeyCur=(APIKey)gridMain.Rows[e.Row].Tag;
			DialogResult res=FormFAKE.ShowDialog();
			if(res==DialogResult.OK) {
				_listApiKeysLocal[e.Row]=FormFAKE.APIKeyCur;
				FillGrid();
			}
		}

		///<summary>Displays the permissions that are assigned for the selected API key.</summary>
		private void SetPermissions() {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			treePermissions.Enabled=true;
			APIKey apiKey=(APIKey)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			foreach(TreeNode node in treePermissions.Nodes) {
				node.ImageIndex=1;
				if(apiKey.ListPermissions.Contains((APIPermission)node.Tag)) {//The selected key does have the permission for this node.					
					node.ImageIndex=2;
				}
			}
		}

		private void treePermissions_MouseDown(object sender,MouseEventArgs e) {
			TreeNode clickedPermNode=treePermissions.GetNodeAt(e.X,e.Y);
			if(clickedPermNode==null) {
				return;
			}
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			APIKey apiKey=(APIKey)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			if(clickedPermNode.ImageIndex==1) {//unchecked, so need to add a permission
				_listApiKeysLocal[gridMain.SelectedIndices[0]].ListPermissions.Add((APIPermission)clickedPermNode.Tag);
				clickedPermNode.ImageIndex=2;
			}
			else if(clickedPermNode.ImageIndex==2) {//checked, so need to delete the perm
				_listApiKeysLocal[gridMain.SelectedIndices[0]].ListPermissions.Remove((APIPermission)clickedPermNode.Tag);
				clickedPermNode.ImageIndex=1;
			}
		}

		private void treePermissions_AfterSelect(object sender,TreeViewEventArgs e) {
			treePermissions.SelectedNode=null;
			treePermissions.EndUpdate();
		}

		///<summary>Makes web call to WebServicesMainHQ with the update list of API keys. Returns true if the update was successful.</summary>
		private bool UpdateKeysForOffice(List<APIKey> _listApiKeysLocal) {
			APIKey apiKey=new APIKey();
			//prepare the xml document to send--------------------------------------------------------------------------------------
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars=("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer = XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))) {
				writer.WriteStartElement("Payload");
				writer.WriteStartElement("ListAPIKeysUpdated");
				WriteListAPIKeys(writer,_listApiKeysLocal);
				writer.WriteEndElement();
				writer.WriteEndElement(); //Payload	
			}
			//Send the message and get the result-------------------------------------------------------------------------------------
			string result="";
			try {
				string officeData=WebServiceMainHQProxy.CreateWebServiceHQPayload(strbuild.ToString(),eServiceCode.FHIR);
				result=WebServiceMainHQProxy.GetWebServiceMainHQInstance().UpdateFHIRAPIKeys(officeData);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			//Process errors------------------------------------------------------------------------------------------------------------
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				MessageBox.Show(node.InnerText);
				return false;
			}
			return true;
		}

		///<summary>Writes the list of API keys in the proper manner.</summary>
		private void WriteListAPIKeys(XmlWriter writer,List<APIKey> listApiKeys) {
			foreach(APIKey apiKey in listApiKeys) {
				writer.WriteStartElement("APIKey");
				writer.WriteStartElement("APIKeyValue");
				writer.WriteString(apiKey.Key);
				writer.WriteEndElement();
				writer.WriteStartElement("FHIRAPIKeyNum");
				writer.WriteString(POut.Long(apiKey.FHIRAPIKeyNum));
				writer.WriteEndElement();
				writer.WriteStartElement("DateDisabled");
				writer.WriteString(apiKey.DateDisabled.ToString());
				writer.WriteEndElement();
				writer.WriteStartElement("KeyStatus");
				writer.WriteString(apiKey.KeyStatus.ToString());
				writer.WriteEndElement();
				writer.WriteStartElement("DeveloperName");
				writer.WriteString(apiKey.DeveloperName);
				writer.WriteEndElement();
				writer.WriteStartElement("DeveloperEmail");
				writer.WriteString(apiKey.DeveloperEmail);
				writer.WriteEndElement();
				writer.WriteStartElement("DeveloperPhone");
				writer.WriteString(apiKey.DeveloperPhone);
				writer.WriteEndElement();
				writer.WriteStartElement("FHIRDeveloperNum");
				writer.WriteString(POut.Long(apiKey.FHIRDeveloperNum));
				writer.WriteEndElement();
				writer.WriteStartElement("ListAPIPermissions");
				foreach(APIPermission perm in apiKey.ListPermissions) {
					writer.WriteStartElement("APIPermission");
					writer.WriteString(perm.ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			if(textSubInterval.errorProvider1.GetError(textSubInterval)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			bool changed=false;
			foreach(APIKey apiKeyHQ in _listApiKeysHQ) {
				APIKey apiKeyLocal=_listApiKeysLocal.FirstOrDefault(x => x.Key==apiKeyHQ.Key);
				if(apiKeyLocal==null //A new key was generated but then Cancel was clicked on FormFHIRAPIKeyEdit.
					|| apiKeyLocal.DeveloperName!=apiKeyHQ.DeveloperName
					|| apiKeyLocal.DeveloperEmail!=apiKeyHQ.DeveloperEmail
					|| apiKeyLocal.DeveloperPhone!=apiKeyHQ.DeveloperPhone
					|| apiKeyLocal.ListPermissions.Any(x => !apiKeyHQ.ListPermissions.Contains(x))
					|| apiKeyHQ.ListPermissions.Any(x => !apiKeyLocal.ListPermissions.Contains(x)))
				{
					changed=true;
					break;
				}
			}
			if(changed && !UpdateKeysForOffice(_listApiKeysLocal)) {
				return;
			}
			Program prog=Programs.GetCur(ProgramName.FHIR);
			prog.Enabled=checkEnabled.Checked;
			Programs.Update(prog);
			ProgramProperty progProp=ProgramProperties.GetPropByDesc("SubscriptionProcessingFrequency",ProgramProperties.GetForProgram(prog.ProgramNum));
			ProgramProperties.UpdateProgramPropertyWithValue(progProp,textSubInterval.Text);
			DataValid.SetInvalid(InvalidType.Programs);
			Close();
		}

	}

	///<summary>A developer's API key. Provided by ODHQ.</summary>
	public class APIKey {
		///<summary>The string representation of the key.</summary>
		public string Key;
		///<summary>The status of the key.</summary>
		public APIKeyStatus KeyStatus;
		///<summary>The permissions that an APIKey possesses.</summary>
		public List<APIPermission> ListPermissions=new List<APIPermission>();
		///<summary>The name of the developer that owns this key.</summary>
		public string DeveloperName;
		///<summary>The email of the developer that owns this key.</summary>
		public string DeveloperEmail;
		///<summary>The phone number of the developer that owns this key.</summary>
		public string DeveloperPhone;
		///<summary>The phone number of the developer that owns this key.</summary>
		public DateTime DateDisabled;
		///<summary>FK to fhirdeveloper.FHIRDeveloperNum. This table only exists in serviceshq database.</summary>
		public long FHIRDeveloperNum;
		///<summary>FK to fhirapikey.FHIRAPIKeyNum. This table only exists in serviceshq database.</summary>
		public long FHIRAPIKeyNum;

		public APIKey Copy() {
			APIKey apiKey=(APIKey)MemberwiseClone();
			apiKey.ListPermissions=ListPermissions.ToList();
			return apiKey;
		}
	}
}
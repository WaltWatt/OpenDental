using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// </summary>
	public class FormAutoNotes:ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.TreeView treeNotes;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.IContainer components;
		public AutoNote AutoNoteCur;
		private Label labelSelection;
		private ImageList imageListTree;
		private CheckBox checkCollapse;
		public bool IsSelectionMode;
		///<summary>On load, the UserOdPref that contains the comma delimited list of expanded category DefNums is retrieved from the database.  On close
		///the UserOdPref is updated with the current expanded DefNums.</summary>
		private UserOdPref _userOdCurPref;
		private Dictionary<long,NodeChildren> _dictChildNodesForDefNum;

		///<summary>Allows distinction of child node types as both categories and as single autonotes.</summary>
		private class NodeChildren {
			public List<TreeNode> ListChildDefNodes=new List<TreeNode>();
			public List<TreeNode> ListAutoNoteNodes=new List<TreeNode>();
		}

		///<summary></summary>
		public FormAutoNotes(){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ){
			if( disposing )
			{
				if(components != null)	{
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoNotes));
			this.imageListTree = new System.Windows.Forms.ImageList(this.components);
			this.labelSelection = new System.Windows.Forms.Label();
			this.treeNotes = new System.Windows.Forms.TreeView();
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.checkCollapse = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// imageListTree
			// 
			this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
			this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTree.Images.SetKeyName(0, "imageFolder");
			this.imageListTree.Images.SetKeyName(1, "imageText");
			// 
			// labelSelection
			// 
			this.labelSelection.Location = new System.Drawing.Point(15, 4);
			this.labelSelection.Name = "labelSelection";
			this.labelSelection.Size = new System.Drawing.Size(268, 14);
			this.labelSelection.TabIndex = 8;
			this.labelSelection.Text = "Select an Auto Note by double clicking.";
			this.labelSelection.Visible = false;
			// 
			// treeNotes
			// 
			this.treeNotes.AllowDrop = true;
			this.treeNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeNotes.HideSelection = false;
			this.treeNotes.ImageIndex = 1;
			this.treeNotes.ImageList = this.imageListTree;
			this.treeNotes.Indent = 12;
			this.treeNotes.Location = new System.Drawing.Point(18, 21);
			this.treeNotes.Name = "treeNotes";
			this.treeNotes.SelectedImageIndex = 1;
			this.treeNotes.Size = new System.Drawing.Size(307, 641);
			this.treeNotes.TabIndex = 2;
			this.treeNotes.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeNotes_ItemDrag);
			this.treeNotes.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeNotes_MouseDoubleClick);
			this.treeNotes.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeNotes_DragDrop);
			this.treeNotes.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeNotes_DragEnter);
			this.treeNotes.DragOver += new System.Windows.Forms.DragEventHandler(this.treeNotes_DragOver);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(340, 637);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(79, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(340, 320);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 26);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// checkCollapse
			// 
			this.checkCollapse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCollapse.Location = new System.Drawing.Point(340, 21);
			this.checkCollapse.Name = "checkCollapse";
			this.checkCollapse.Size = new System.Drawing.Size(79, 20);
			this.checkCollapse.TabIndex = 227;
			this.checkCollapse.Text = "Collapse All";
			this.checkCollapse.UseVisualStyleBackColor = true;
			this.checkCollapse.CheckedChanged += new System.EventHandler(this.checkCollapse_CheckedChanged);
			// 
			// FormAutoNotes
			// 
			this.ClientSize = new System.Drawing.Size(431, 675);
			this.Controls.Add(this.checkCollapse);
			this.Controls.Add(this.labelSelection);
			this.Controls.Add(this.treeNotes);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(447, 414);
			this.Name = "FormAutoNotes";
			this.ShowInTaskbar = false;
			this.Text = "Auto Notes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAutoNotes_FormClosing);
			this.Load += new System.EventHandler(this.FormAutoNotes_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAutoNotes_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode) {
				butAdd.Visible=false;
				labelSelection.Visible=true;
			}
			_userOdCurPref=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.AutoNoteExpandedCats).FirstOrDefault();
			FillListTree();
		}

		private void FillListTree() {
			List<long> listExpandedDefNums=new List<long>();
			if(treeNotes.Nodes.Count==0 && _userOdCurPref!=null) {//if this is the fill on load, the node count will be 0, expanded node list from pref
				listExpandedDefNums=_userOdCurPref.ValueString.Split(',').Where(x => x!="" && x!="0").Select(x => PIn.Long(x)).ToList();
			}
			else {//either not fill on load or no user pref, store the expanded node state to restore after filling tree
				//only defs (category folders) can be expanded or have children nodes
				listExpandedDefNums=treeNotes.Nodes.OfType<TreeNode>().SelectMany(x => GetNodeAndChildren(x))
					.Where(x => x.IsExpanded && x.Tag is Def).Select(x => ((Def)x.Tag).DefNum).ToList();
			}
			TreeNode selectedNode=treeNotes.SelectedNode;
			TreeNode topNode=null;
			string topNodePath=treeNotes.TopNode?.FullPath;
			treeNotes.BeginUpdate();
			treeNotes.Nodes.Clear();//clear current tree contents
			_dictChildNodesForDefNum=Defs.GetDefsForCategory(DefCat.AutoNoteCats,true).GroupBy(x => x.ItemValue??"0")
				.ToDictionary(x => PIn.Long(x.Key),x => new NodeChildren() { ListChildDefNodes=x.Select(y => new TreeNode(y.ItemName,0,0) { Tag=y }).ToList() });
			Dictionary<long,List<TreeNode>> dictDefNumAutoNotes=AutoNotes.GetDeepCopy().GroupBy(x => x.Category)
				.ToDictionary(x => x.Key,x => x.Select(y => new TreeNode(y.AutoNoteName,1,1) { Tag=y }).ToList());
			foreach(KeyValuePair<long,List<TreeNode>> kvp in dictDefNumAutoNotes) {
				if(_dictChildNodesForDefNum.ContainsKey(kvp.Key)) {
					_dictChildNodesForDefNum[kvp.Key].ListAutoNoteNodes=kvp.Value;
				}
				else {
					_dictChildNodesForDefNum[kvp.Key]=new NodeChildren() { ListAutoNoteNodes=kvp.Value };
				}
			}
			List<TreeNode> listNodes=new List<TreeNode>();//all nodes to add to tree, categories and autonotes
			NodeChildren nodeChildren;
			if(_dictChildNodesForDefNum.TryGetValue(0,out nodeChildren)) {
				nodeChildren.ListChildDefNodes.ForEach(SetAllDescendantsForNode);
				listNodes.AddRange(nodeChildren.ListChildDefNodes);
				listNodes.AddRange(nodeChildren.ListAutoNoteNodes);
			}
			treeNotes.Nodes.AddRange(listNodes.OrderBy(x => x,new NodeSorter()).ToArray());//add node list to tree, after sorting
			List<TreeNode> listNodesCur=listNodes.SelectMany(x => GetNodeAndChildren(x)).ToList();//get flat list of all nodes, copy entire tree
			foreach(TreeNode nodeCur in listNodesCur) {
				if(!string.IsNullOrEmpty(topNodePath) && nodeCur.FullPath==topNodePath) {
					topNode=nodeCur;
				}
				if(nodeCur.Tag is Def && listExpandedDefNums.Contains(((Def)nodeCur.Tag).DefNum)) {
					nodeCur.Expand();
				}
				if(selectedNode==null) {
					continue;
				}
				if(Equals(nodeCur,selectedNode)) {
					treeNotes.SelectedNode=nodeCur;
				}
			}
			treeNotes.TopNode=topNode;
			if(topNode==null && treeNotes.Nodes.Count>0) {
				treeNotes.TopNode=treeNotes.SelectedNode??treeNotes.Nodes[0];
			}
			treeNotes.EndUpdate();
			treeNotes.Focus();
		}

		///<summary>Returns true if both nodes are tagged with Defs and both Defs have the same DefNum OR both nodes are tagged with AutoNotes and both
		///AutoNotes have the same AutoNoteNum.  If either node is null, returns false.</summary>
		private bool Equals(TreeNode nodeA,TreeNode nodeB) {
			if(nodeA==null || nodeB==null) {
				return false;
			}
			if((nodeA.Tag is AutoNote && nodeB.Tag is AutoNote && ((AutoNote)nodeA.Tag).AutoNoteNum==((AutoNote)nodeB.Tag).AutoNoteNum)
				|| (nodeA.Tag is Def && nodeB.Tag is Def && ((Def)nodeA.Tag).DefNum==((Def)nodeB.Tag).DefNum))
			{
				return true;
			}
			return false;
		}

		///<summary>Recursive function, returns a tree node with all descendants, including all auto note children for this def cat and all children for
		///any cat within this this cat.  Auto Notes that are at the 'root' level (considered uncategorized) have to be added separately after filling the
		///rest of the tree with this function and will be at the bottom of the root node list.</summary>
		private void SetAllDescendantsForNode(TreeNode defNodeCur) {
			if(defNodeCur==null || defNodeCur.Tag is AutoNote) {
				return;
			}
			List<TreeNode> listChildNodes=new List<TreeNode>();
			NodeChildren nodeChildrenCur;
			if(_dictChildNodesForDefNum.TryGetValue(((Def)defNodeCur.Tag).DefNum,out nodeChildrenCur)) {
				nodeChildrenCur.ListChildDefNodes.ForEach(x => SetAllDescendantsForNode(x));
				listChildNodes.AddRange(nodeChildrenCur.ListChildDefNodes);
				listChildNodes.AddRange(nodeChildrenCur.ListAutoNoteNodes);
			}
			defNodeCur.Nodes.AddRange(listChildNodes.OrderBy(x => x,new NodeSorter()).ToArray());
		}

		///<summary>Returns a flat list containing this TreeNode and all of its descendant TreeNodes.</summary>
		private List<TreeNode> GetNodeAndChildren(TreeNode treeNode,bool isCatsOnly=false) {
			List<TreeNode> listRetval=new List<TreeNode>();
			if(isCatsOnly && treeNode.Tag is AutoNote) {
				//guaranteed leaf node.
				return listRetval;
			}
			listRetval.Add(treeNode);
			for(int i=0;i<listRetval.Count;i++) {
				listRetval.AddRange(listRetval[i].Nodes.OfType<TreeNode>().Where(x => !isCatsOnly || x.Tag is Def));
			}
			return listRetval;
		}

		private bool IsValidDestination(TreeNode nodeCur,TreeNode nodeDest) {
			if(nodeCur==null //shouldn't happen, but if the selected node is null, nothing to do
				|| nodeCur.Parent==nodeDest //dest is already the parent of the selected node, even if both dest and the node's parent are null, nothing to do
				|| (nodeDest!=null && nodeDest.FullPath.StartsWith(nodeCur.FullPath))) //destination node is descendant of the selected node, invalid move
			{
				return false;
			}
			return true;
		}

		private void treeNotes_MouseDoubleClick(object sender,TreeNodeMouseClickEventArgs e) {
			if(e.Node==null || !(e.Node.Tag is AutoNote)) {
				return;
			}
			AutoNote noteCur=((AutoNote)e.Node.Tag).Copy();
			if(IsSelectionMode) {
				AutoNoteCur=noteCur;
				DialogResult=DialogResult.OK;
				return;
			}
			FormAutoNoteEdit FormA=new FormAutoNoteEdit();
			FormA.IsNew=false;
			FormA.AutoNoteCur=noteCur;
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				FillListTree();
			}
		}

		private TreeNode _grayNode=null;//only used in treeNotes_DragOver to reduce flickering.

		private void treeNotes_DragOver(object sender,DragEventArgs e) {
			Point pt=treeNotes.PointToClient(new Point(e.X,e.Y));
			TreeNode nodeSelected=treeNotes.GetNodeAt(pt);
			if(_grayNode!=null && _grayNode!=nodeSelected) {
				_grayNode.BackColor=Color.White;
				_grayNode=null;
			}
			if(nodeSelected!=null && nodeSelected.BackColor!=Color.LightGray) {
				nodeSelected.BackColor=Color.LightGray;
				_grayNode=nodeSelected;
			}
			if(pt.Y<25) {
				MiscUtils.SendMessage(treeNotes.Handle,277,0,0);//Scroll Up
			}
			else if(pt.Y>treeNotes.Height-25) {
				MiscUtils.SendMessage(treeNotes.Handle,277,1,0);//Scroll down.
			}
		}

		private void treeNotes_ItemDrag(object sender,ItemDragEventArgs e) {
			treeNotes.SelectedNode=(TreeNode)e.Item;
			DoDragDrop(e.Item,DragDropEffects.Move);
		}

		private void treeNotes_DragEnter(object sender,DragEventArgs e) {
			e.Effect=DragDropEffects.Move;
		}

		private void treeNotes_DragDrop(object sender,DragEventArgs e) {
			if(_grayNode!=null) {
				_grayNode.BackColor=Color.White;
			}
			if(!e.Data.GetDataPresent("System.Windows.Forms.TreeNode",false)) { 
				return; 
			}
			TreeNode sourceNode=(TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
			if(sourceNode==null || !(sourceNode.Tag is Def || sourceNode.Tag is AutoNote)) {
				return;
			}
			TreeNode topNodeCur=treeNotes.TopNode;
			if(treeNotes.TopNode==sourceNode && sourceNode.PrevVisibleNode!=null) {
				//if moving the topNode to another category, make the previous visible node the topNode once the move is successful
				topNodeCur=sourceNode.PrevVisibleNode;
			}
			Point pt=((TreeView)sender).PointToClient(new Point(e.X,e.Y));
			TreeNode destNode=((TreeView)sender).GetNodeAt(pt);
			if(destNode==null || !(destNode.Tag is Def || destNode.Tag is AutoNote)) {//moving to root node (category 0)
				if(sourceNode.Parent==null) {//already at the root node, nothing to do
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Move the selected "+(sourceNode.Tag is AutoNote?"Auto Note":"category")+" to the root level?")) {
					return;
				}
				if(sourceNode.Tag is Def) {
					((Def)sourceNode.Tag).ItemValue="";
				}
				else {//must be an AutoNote
					((AutoNote)sourceNode.Tag).Category=0;
				}
			}
			else {//moving to another folder (category)
				if(destNode.Tag is AutoNote) {
					destNode=destNode.Parent;//if destination is AutoNote, set destination to the parent, which is the category def node for the note
				}
				if(!IsValidDestination(sourceNode,destNode)) {
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,
					"Move the selected "+(sourceNode.Tag is AutoNote?"Auto Note":"category")+(destNode==null?" to the root level":"")+"?"))
				{
					return;
				}
				//destNode will be null if a root AutoNote was selected as the destination
				long destDefNum=(destNode==null?0:((Def)destNode.Tag).DefNum);
				if(sourceNode.Tag is Def) {
					((Def)sourceNode.Tag).ItemValue=(destDefNum==0?"":destDefNum.ToString());//make a DefNum of 0 be a blank string in the db, not a "0" string
				}
				else {//must be an AutoNote
					((AutoNote)sourceNode.Tag).Category=destDefNum;
				}
			}
			if(sourceNode.Tag is Def) {
				Defs.Update((Def)sourceNode.Tag);
				DataValid.SetInvalid(InvalidType.Defs);
			}
			else {//must be an AutoNote
				AutoNotes.Update((AutoNote)sourceNode.Tag);
				DataValid.SetInvalid(InvalidType.AutoNotes);
			}
			treeNotes.TopNode=topNodeCur;//if sourceNode was the TopNode and was moved, make the TopNode the previous visible node
			FillListTree();
		}

		private void checkCollapse_CheckedChanged(object sender,System.EventArgs e) {
			TreeNode topNode=treeNotes.TopNode;
			TreeNode selectedNode=treeNotes.SelectedNode;
			treeNotes.BeginUpdate();
			if(checkCollapse.Checked) {
				while(topNode.Parent!=null) {//store the topNode's root to set the topNode after collapsing all nodes
					topNode=topNode.Parent;
				}
				while(selectedNode!=null && selectedNode.Parent!=null) {//store the selectedNode's root to select after collapsing
					selectedNode=selectedNode.Parent;
				}
				treeNotes.CollapseAll();
			}
			else {
				treeNotes.ExpandAll();
			}
			treeNotes.EndUpdate();
			if(selectedNode==null) {
				treeNotes.TopNode=topNode;//set TopNode if there is no SelectedNode
			}
			else {//reselect the node and ensure that it is visible after expanding or collapsing
				treeNotes.SelectedNode=selectedNode;
				treeNotes.SelectedNode.EnsureVisible();
				treeNotes.Focus();
			}
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit)) {
				return;
			}
			long selectedDefNum=0;
			if(treeNotes.SelectedNode?.Tag is Def) {
				selectedDefNum=((Def)treeNotes.SelectedNode.Tag).DefNum;
			}
			else if(treeNotes.SelectedNode?.Tag is AutoNote) {
				selectedDefNum=((AutoNote)treeNotes.SelectedNode.Tag).Category;
			}
			FormAutoNoteEdit FormA=new FormAutoNoteEdit();
			FormA.IsNew=true;
			FormA.AutoNoteCur=new AutoNote() { Category=selectedDefNum };
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
				return;
			}
			treeNotes.SelectedNode?.Expand();//expanding an AutoNote has no effect, and if nothing selected nothing to expand
			FillListTree();
			if((FormA.AutoNoteCur?.AutoNoteNum??0)>0) {//select the newly added note in the tree
				treeNotes.SelectedNode=treeNotes.Nodes.OfType<TreeNode>().SelectMany(x => GetNodeAndChildren(x)).Where(x => x.Tag is AutoNote)
					.FirstOrDefault(x => ((AutoNote)x.Tag).AutoNoteNum==FormA.AutoNoteCur.AutoNoteNum);
				treeNotes.SelectedNode?.EnsureVisible();
				treeNotes.Focus();
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormAutoNotes_FormClosing(object sender,FormClosingEventArgs e) {
			//store the current node expanded state for this user
			List<long> listExpandedDefNums=treeNotes.Nodes.OfType<TreeNode>()
				.SelectMany(x => GetNodeAndChildren(x,true))
				.Where(x => x.IsExpanded)
				.Select(x => ((Def)x.Tag).DefNum)
				.Where(x => x>0).ToList();
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
	}

	///<summary>Sorting class used to sort a MethodInfo list by Name.</summary>
	public class NodeSorter:IComparer<TreeNode> {

		public int Compare(TreeNode x,TreeNode y) {
			if(x.Tag is Def && y.Tag is AutoNote) {
				return -1;
			}
			if(x.Tag is AutoNote && y.Tag is Def) {
				return 1;
			}
			if(x.Tag is Def && y.Tag is Def) {
				Def defX=(Def)x.Tag;
				Def defY=(Def)y.Tag;
				if(defX.ItemOrder!=defY.ItemOrder) {
					return defX.ItemOrder.CompareTo(defY.ItemOrder);
				}
			}
			//either both nodes are AutoNote nodes or both are Def nodes and both have the same ItemOrder (shouldn't happen), sort alphabetically
			return x.Text.CompareTo(y.Text);
		}
	}

}

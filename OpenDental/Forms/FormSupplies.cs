using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Drawing.Printing;
using System.Linq;

namespace OpenDental {
	public partial class FormSupplies:ODForm {
		///<summary>A cached copy of supplies obtained on window load.</summary>
		private List<Supply> _listSupplies;
		///<summary>Used to sync at the close of the window.</summary>
		private List<Supply> _listSuppliesOld;
		///<summary>A cached copy of all suppliers obtaine don window load.</summary>
		private List<Supplier> _listSuppliers;
		///<Summary>Sets the supplier that will first show when opening this window.  Used when IsSelectMode is true.</Summary>
		public long SelectedSupplierNum;
		///<summary>Used for selecting supply items to add to orders in FormSupplyOrders.</summary>
		public bool IsSelectMode;
		///<summary>Selected supply items.  Used outside this window for selecting supplies to order in FormSupplyOrders.</summary>
		public List<Supply> ListSelectedSupplies;
		///<summary>Used to re-select supplies after filters are applied.</summary>
		private List<long> _listSelectedSupplyNums;
		///<summary>Used to re-select supplies after filters are applied.</summary>
		private List<Supply> _listSelectedSupplies;

		//Variables used for printing are copied and pasted here
		PrintDocument pd2;
		private int pagesPrinted;
		private bool headingPrinted;
		private int headingPrintH;

		public FormSupplies() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSupplies_Load(object sender,EventArgs e) {
			this.Height=SystemInformation.WorkingArea.Height;//Resize height
			this.Location=new Point(Location.X,0);//Move window to compensate for changed height
			ListSelectedSupplies=new List<Supply>();
			_listSuppliers=Suppliers.GetAll();
			_listSupplies=Supplies.GetAll();
			_listSelectedSupplyNums=new List<long>();
			_listSelectedSupplies=new List<Supply>();
			_listSuppliesOld=new List<Supply>();
			foreach(Supply supply in _listSupplies) {//Make deep copy of the list so we can sync later.
				_listSuppliesOld.Add(supply.Copy());
			}
			FillComboSupplier();
			FillGrid();
			if(IsSelectMode) {
				comboSupplier.Enabled=false;
			}
		}

		private void FillComboSupplier() {
			comboSupplier.Items.Clear();
			comboSupplier.Items.Add(Lan.g(this,"All"));
			comboSupplier.SelectedIndex=0;//default to "All" otherwise selected index will be selected below.
			for(int i = 0;i<_listSuppliers.Count;i++) {
				comboSupplier.Items.Add(_listSuppliers[i].Name);
				if(_listSuppliers[i].SupplierNum==SelectedSupplierNum) {
					comboSupplier.SelectedIndex=i+1;//+1 to account for the ALL item.
				}
			}
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Category"),130);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Catalog #"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Supplier"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Description"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Price"),60,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"StockQty"),60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"OnHandQty"),80,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"IsHidden"),40,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listSupplies.Count;i++) {
				Supply supply=_listSupplies[i];
				if(!checkShowHidden.Checked && supply.IsHidden) {
					continue;//If we're filtering out hidden supplies and this one is hidden, skip it.
				}
				if(SelectedSupplierNum!=0 && supply.SupplierNum!=SelectedSupplierNum) {
					continue;//If we specified a suppliernum outside this form and this supply doesn't have that suppliernum, skip it.
				}
				else if(comboSupplier.SelectedIndex!=0 && _listSuppliers[comboSupplier.SelectedIndex-1].SupplierNum!=supply.SupplierNum) {
					continue;//If a supplier is selected in the combo box and this supply doesn't have it, skip it.
				}
				if(checkShowShoppingList.Checked && supply.LevelOnHand >= supply.LevelDesired) {
					continue;//If we are only showing those that require restocking and this one has the number desired, skip it.
				}
				if(!string.IsNullOrEmpty(textFind.Text)
					&& !supply.Descript.ToUpper().Contains(textFind.Text.ToUpper())
					&& !supply.CatalogNumber.ToString().ToUpper().Contains(textFind.Text.ToUpper())
					&& !Defs.GetName(DefCat.SupplyCats,supply.Category).ToUpper().Contains(textFind.Text.ToUpper())
					&& !supply.LevelDesired.ToString().Contains(textFind.Text)
					&& !supply.Price.ToString().ToUpper().Contains(textFind.Text.ToUpper())
					&& !supply.SupplierNum.ToString().Contains(textFind.Text))
				{//If nothing about the supply matches the text entered in the field, skip it.
					continue;
				}
				row=new ODGridRow();
				if(gridMain.Rows.Count==0 || (gridMain.Rows.Count>0 && supply.Category!=((Supply)gridMain.Rows[gridMain.Rows.Count-1].Tag).Category)) {
					row.Cells.Add(Defs.GetName(DefCat.SupplyCats,supply.Category));//Add the new category header in this row if it doesn't match the previous row's category.
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(supply.CatalogNumber);
				row.Cells.Add(Suppliers.GetName(_listSuppliers,supply.SupplierNum));
				row.Cells.Add(supply.Descript);
				if(supply.Price==0) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(supply.Price.ToString("n"));
				}
				if(supply.LevelDesired==0) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(supply.LevelDesired.ToString());
				}
				row.Cells.Add(supply.LevelOnHand.ToString());
				if(supply.IsHidden) {
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				row.Tag=supply;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(_listSelectedSupplies.Contains(((Supply)gridMain.Rows[i].Tag))) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(Defs.GetDefsForCategory(DefCat.SupplyCats,true).Count==0) {//No supply categories have been entered, not allowed to enter supply
				MsgBox.Show(this,"No supply categories have been created.  Go to the supply inventory window, select categories, and enter at least one supply category first.");
				return;
			}
			if(comboSupplier.SelectedIndex < 1) {//Includes no items or the ALL item being selected
				MsgBox.Show(this,"Please select a supplier first.");
				return;
			}
			Supply supp=new Supply();
			supp.IsNew=true;
			supp.SupplierNum=_listSuppliers[comboSupplier.SelectedIndex-1].SupplierNum;//Selected index -1 to account for ALL being at the top of the list.
			if(gridMain.GetSelectedIndex()>-1) {
				supp.Category=((Supply)gridMain.Rows[gridMain.GetSelectedIndex()].Tag).Category;
			}
			FormSupplyEdit FormS=new FormSupplyEdit();
			FormS.Supp=supp;
			FormS.ListSupplier=_listSuppliers;
			FormS.ShowDialog();//inserts supply in DB if needed.  Item order will be at selected index or end of category.
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormS.Supp==null) {//New supply deleted
				return;
			}
			FormS.Supp.ItemOrder=_listSupplies.FindAll(x=>x.Category==FormS.Supp.Category)
				.Select(x=>x.ItemOrder)
				.OrderByDescending(x=>x)
				.FirstOrDefault()+1;//Last item in the category.
			int idx=_listSupplies.FindLastIndex(x=>x.Category==FormS.Supp.Category);
			if(idx==-1) {
				_listSupplies.Add(FormS.Supp);//new category, add to bottom of list
			}
			else {
				_listSupplies.Insert(idx+1,FormS.Supp);//add to bottom of existing category
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			_listSelectedSupplies.Clear();
			foreach(int index in gridMain.SelectedIndices) {
				_listSelectedSupplies.Add((Supply)gridMain.Rows[index].Tag);
			}
			if(IsSelectMode) {
				ListSelectedSupplies.Clear();//just in case
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					ListSelectedSupplies.Add((Supply)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);
				}				
				SyncAndClose();
				return;
			}
			Supply selectedSupply=(Supply)gridMain.Rows[e.Row].Tag;
			long oldCategoryDefNum=selectedSupply.Category;
			FormSupplyEdit FormS=new FormSupplyEdit();
			FormS.Supp=selectedSupply;
			FormS.ListSupplier=_listSuppliers;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;	
			}
			if(FormS.Supp==null) {
				_listSupplies.Remove(selectedSupply);
			}
			else if(selectedSupply.Category!=oldCategoryDefNum) {//If category changed
				//Send supply to the bottom the new category
				_listSupplies.Remove(selectedSupply);//Remove so we can reinsert where it needs to be.
				selectedSupply.ItemOrder=_listSupplies.FindAll(x => x.Category==selectedSupply.Category)
					.Select(x => x.ItemOrder)
					.OrderByDescending(x => x)
					.FirstOrDefault()+1;//Last item in the category.
				int idx=_listSupplies.FindLastIndex(x => x.Category==selectedSupply.Category);
				if(idx==-1) {
					_listSupplies.Add(selectedSupply);//new category, add to bottom of list
				}
				else {
					_listSupplies.Insert(idx+1,selectedSupply);//add to bottom of existing category
				}
			}
			int scroll=gridMain.ScrollValue;
			FillGrid();
			gridMain.ScrollValue=scroll;
		}

		private void comboSupplier_SelectionChangeCommitted(object sender,EventArgs e) {
			_listSelectedSupplies.Clear();
			foreach(int index in gridMain.SelectedIndices) {
				_listSelectedSupplies.Add((Supply)gridMain.Rows[index].Tag);
			}
			FillGrid();
		}

		private void checkShowHidden_Click(object sender,EventArgs e) {
			_listSelectedSupplies.Clear();
			foreach(int index in gridMain.SelectedIndices) {
				_listSelectedSupplies.Add((Supply)gridMain.Rows[index].Tag);
			}
			FillGrid();
		}

		private void checkShowShoppingList_Click(object sender,EventArgs e) {
			_listSelectedSupplies.Clear();
			foreach(int index in gridMain.SelectedIndices) {
				_listSelectedSupplies.Add((Supply)gridMain.Rows[index].Tag);
			}
			FillGrid();
		}

		private void butUp_Click(object sender,EventArgs e) {
			//Nothing Selected
			if(gridMain.SelectedIndices.Length==0) {
				return;
			}
			_listSelectedSupplies.Clear();
			foreach(int index in gridMain.SelectedIndices) {
				_listSelectedSupplies.Add((Supply)gridMain.Rows[index].Tag);
			}
			//Loop through selected indices, moving each one as needed.
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				int indexSource=gridMain.SelectedIndices[i];
				int indexDest=indexSource-1;
				//Top of visible category-------------------------------------------------------------------------------
				if(indexSource==0) {
					continue;
				}
				Supply supplySource=(Supply)gridMain.Rows[indexSource].Tag;
				Supply supplyDest=(Supply)gridMain.Rows[indexDest].Tag;//Incorrect. 
				//Top of category---------------------------------------------------------------------------------------
				if(supplySource.Category!=supplyDest.Category) {
					continue;//already top of category
				}
				//Move the item up.  Change all item's item orders that it moved past (can be a filtered out result) and change its position in the global list.
				int sourceIdx=_listSupplies.IndexOf(supplySource);//sourceIdx = higher entry
				int destIdx=_listSupplies.IndexOf(supplyDest);//destIdx = lower entry
				_listSupplies[sourceIdx]=supplyDest;
				_listSupplies[destIdx]=supplySource;
				gridMain.Rows[indexSource].Tag=supplyDest;//Fix up the tags for future movement.
				gridMain.Rows[indexDest].Tag=supplySource;
				//Item orders are not set here, they are reconciled on OK click.
			}
			int scrollVal=gridMain.ScrollValue;
			FillGrid();
			gridMain.ScrollValue=scrollVal;
		}

		private void butDown_Click(object sender,EventArgs e) {
			//Nothing Selected
			if(gridMain.SelectedIndices.Length==0) {
				return;
			}
			_listSelectedSupplies.Clear();
			foreach(int index in gridMain.SelectedIndices) {
				_listSelectedSupplies.Add((Supply)gridMain.Rows[index].Tag);
			}
			//Loop through selected indices, moving each one as needed.
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {
				int indexSource=gridMain.SelectedIndices[i];//to reduce confusion
				int indexDest=indexSource+1;
				//Bottom of visible category-------------------------------------------------------------------------------
				if(indexSource==gridMain.Rows.Count-1) {
					continue;
				}
				Supply supplySource=(Supply)gridMain.Rows[indexSource].Tag;
				Supply supplyDest=(Supply)gridMain.Rows[indexDest].Tag;
				//Bottom of category---------------------------------------------------------------------------------------
				if(supplySource.Category!=supplyDest.Category) {
					continue;//already bottom of category
				}
				//Move the item down.  Change all item's item orders that it moved past (can be a filtered out result) and change its position in the global list.
				int sourceIdx=_listSupplies.IndexOf(supplySource);//Grid Tags are references to this in-memory list, so this will work.
				int destIdx=_listSupplies.IndexOf(supplyDest);
				_listSupplies[sourceIdx]=supplyDest;
				_listSupplies[destIdx]=supplySource;
				gridMain.Rows[indexSource].Tag=supplyDest;//Fix up the tags for future movement.
				gridMain.Rows[indexDest].Tag=supplySource;
				//Item orders are not set here, they are reconciled on OK click.
			}
			int scrollVal=gridMain.ScrollValue;
			FillGrid();
			gridMain.ScrollValue=scrollVal;
		}
		
		private void textFind_TextChanged(object sender,EventArgs e) {
			_listSelectedSupplies.Clear();
			foreach(int index in gridMain.SelectedIndices) {
				_listSelectedSupplies.Add((Supply)gridMain.Rows[index].Tag);
			}
			FillGrid();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(gridMain.Rows.Count<1) {
				MsgBox.Show(this,"Supply list is Empty.");
				return;
			}
			pagesPrinted=0;
			headingPrinted=false;
			pd2=new PrintDocument();
			pd2.DefaultPageSettings.Margins=new Margins(50,50,40,30);
			pd2.PrintPage+=new PrintPageEventHandler(pd2_PrintPage);
			if(pd2.DefaultPageSettings.PrintableArea.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
#if DEBUG
			FormRpPrintPreview pView=new FormRpPrintPreview();
			pView.printPreviewControl2.Document=pd2;
			pView.ShowDialog();
#else
				if(PrinterL.SetPrinter(pd2,PrintSituation.Default,0,"Supplies list printed")) {
					try{
						pd2.Print();
					}
					catch{
						MsgBox.Show(this,"Printer not available");
					}
				}
#endif
		}

		private void pd2_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			Font mainFont=new Font("Arial",9);
			int yPos=bounds.Top;
			#region printHeading
			//TODO: Decide what information goes in the heading.
			if(!headingPrinted) {
				text=Lan.g(this,"Supply List");
				g.DrawString(text,headingFont,Brushes.Black,425-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(checkShowShoppingList.Checked) {
					text=Lan.g(this,"Shopping List");
					g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				}
				if(checkShowHidden.Checked) {
					text=Lan.g(this,"Showing Hidden Items");
					g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				}
				else{
					text=Lan.g(this,"Not Showing Hidden Items");
					g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				}
				if(textFind.Text!="") {
					text=Lan.g(this,"Search Filter")+": "+textFind.Text;
					g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				}
				else {
					text=Lan.g(this,"No Search Filter");
					g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				}
				if(comboSupplier.SelectedIndex<1) {
					text=Lan.g(this,"All Suppliers");
					g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				}
				else {
					text=Lan.g(this,"Supplier")+": "+_listSuppliers[comboSupplier.SelectedIndex-1].Name;
					g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
					if(_listSuppliers[comboSupplier.SelectedIndex-1].Phone!="") {
						text=Lan.g(this,"Phone")+": "+_listSuppliers[comboSupplier.SelectedIndex-1].Phone;
						g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
						yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
					}
					if(_listSuppliers[comboSupplier.SelectedIndex-1].Name!="") {
						text=Lan.g(this,"Note")+": "+_listSuppliers[comboSupplier.SelectedIndex-1].Name;
						g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
						yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
					}
				}
				yPos+=15;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void SyncAndClose() {
			int itemOrder=0;
			for(int i=0;i<_listSupplies.Count;i++) {
				if(i>0 && _listSupplies[i-1].Category!=_listSupplies[i].Category) {
					itemOrder=0;
				}
				_listSupplies[i].ItemOrder=itemOrder;
				itemOrder++;
			}
			//Nuances of concurency using this sync are, 
			//Deletes always win,
			//last in edits win,
			//Added supplies are unaffected by concurency
			Supplies.Sync(_listSupplies,_listSuppliesOld);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(IsSelectMode) {
				if(gridMain.SelectedIndices.Length<1) {
					MsgBox.Show(this,"Please select a supply from the list first.");
					return;
				}
				ListSelectedSupplies.Clear();//just in case
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					ListSelectedSupplies.Add((Supply)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);
				}
			}
			SyncAndClose();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing.Printing;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormSupplyOrders:ODForm {
		private List<Supplier> _listSuppliers;
		private List<SupplyOrder> _listOrdersAll;
		private List<SupplyOrder> _listOrders;
		private DataTable _tableOrderItems;
		//Variables used for printing are copied and pasted here
		PrintDocument _pd2;
		private int _pagesPrinted;
		private bool _headingPrinted;
		private int _headingPrintH;

		public FormSupplyOrders() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSupplyOrders_Load(object sender,EventArgs e) {
			Height=SystemInformation.WorkingArea.Height;//max height
			Location=new Point(Location.X,0);//move to top of screen
			_listSuppliers = Suppliers.GetAll();
			_listOrdersAll = SupplyOrders.GetAll();
			_listOrders = new List<SupplyOrder>();
			FillComboSupplier();
			FillGridOrders();
			gridOrders.ScrollToEnd();
		}

		private void FillComboSupplier() {
			_listSuppliers=Suppliers.GetAll();
			comboSupplier.Items.Clear();
			comboSupplier.Items.Add(Lan.g(this,"All"));//add all to begining of list for composite listings.
			comboSupplier.SelectedIndex=0;
			for(int i=0;i<_listSuppliers.Count;i++) {
				comboSupplier.Items.Add(_listSuppliers[i].Name);
			}
		}

		private void comboSupplier_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridOrders();
			gridOrders.ScrollToEnd();
			FillGridOrderItem();
		}

		private void gridOrder_CellClick(object sender,ODGridClickEventArgs e) {
			FillGridOrderItem();
		}

		private void gridOrder_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormSupplyOrderEdit FormSOE = new FormSupplyOrderEdit();
			FormSOE.ListSupplier = _listSuppliers;
			FormSOE.Order = _listOrders[e.Row];
			FormSOE.ShowDialog();
			if(FormSOE.DialogResult!=DialogResult.OK) {
				return;
			}
			_listOrdersAll = SupplyOrders.GetAll();
			FillGridOrders();
			FillGridOrderItem();
		}

		private void butAddSupply_Click(object sender,EventArgs e) {
			if(gridOrders.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a supply order to add items to first.");
				return;
			}
			FormSupplies FormSup = new FormSupplies();
			FormSup.IsSelectMode = true;
			FormSup.SelectedSupplierNum = _listOrders[gridOrders.GetSelectedIndex()].SupplierNum;
			FormSup.ShowDialog();
			if(FormSup.DialogResult!=DialogResult.OK) {
				return;
			}
			
			for(int i=0;i<FormSup.ListSelectedSupplies.Count;i++) {
				//check for existing----			
				if(_tableOrderItems.Rows.OfType<DataRow>().Any(x => PIn.Long(x["SupplyNum"].ToString())==FormSup.ListSelectedSupplies[i].SupplyNum)) {
					//MsgBox.Show(this,"Selected item already exists in currently selected order. Please edit quantity instead.");
					continue;
				}
				SupplyOrderItem orderitem = new SupplyOrderItem();
				orderitem.SupplyNum = FormSup.ListSelectedSupplies[i].SupplyNum;
				orderitem.Qty=1;
				orderitem.Price = FormSup.ListSelectedSupplies[i].Price;
				orderitem.SupplyOrderNum = _listOrders[gridOrders.GetSelectedIndex()].SupplyOrderNum;
				//soi.SupplyOrderItemNum
				SupplyOrderItems.Insert(orderitem);
			}
			UpdatePriceAndRefresh();
		}

		private void FillGridOrders() {
			FilterListOrder();
			gridOrders.BeginUpdate();
			gridOrders.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date Placed"),80);
			gridOrders.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),70,HorizontalAlignment.Right);
			gridOrders.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Supplier"),120);
			gridOrders.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),200);
			gridOrders.Columns.Add(col);
			gridOrders.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listOrders.Count;i++) {
				row=new ODGridRow();
				if(_listOrders[i].DatePlaced.Year>2200) {
					row.Cells.Add(Lan.g(this,"pending"));
				}
				else {
					row.Cells.Add(_listOrders[i].DatePlaced.ToShortDateString());
				}
				row.Cells.Add(_listOrders[i].AmountTotal.ToString("c"));
				row.Cells.Add(Suppliers.GetName(_listSuppliers,_listOrders[i].SupplierNum));
				row.Cells.Add(_listOrders[i].Note);
				row.Tag=_listOrders[i];
				gridOrders.Rows.Add(row);
			}
			gridOrders.EndUpdate();
		}

		private void FilterListOrder() {
			_listOrders.Clear();
			long supplier=0;
			if(comboSupplier.SelectedIndex < 1) {//this includes selecting All or not having anything selected.
				supplier = 0;
			}
			else {
				supplier=_listSuppliers[comboSupplier.SelectedIndex-1].SupplierNum;//SelectedIndex-1 because All is added before all the other items in the list.
			}
			foreach(SupplyOrder order in _listOrdersAll) {
				if(supplier==0) {//Either the ALL supplier is selected or no supplier is selected.
					_listOrders.Add(order);
				}
				else if(order.SupplierNum == supplier) {
					_listOrders.Add(order);
					continue;
				}
			}
		}

		private void FillGridOrderItem() {
			long orderNum=0;
			if(gridOrders.GetSelectedIndex()!=-1) {//an order is selected
				orderNum=_listOrders[gridOrders.GetSelectedIndex()].SupplyOrderNum;
			}
			_tableOrderItems=SupplyOrderItems.GetItemsForOrder(orderNum);
			gridItems.BeginUpdate();
			gridItems.Columns.Clear();
			//ODGridColumn col=new ODGridColumn(Lan.g(this,"Supplier"),120);
			//gridItems.Columns.Add(col);
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Catalog #"),80);
			gridItems.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Description"),320);
			gridItems.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Qty"),60,HorizontalAlignment.Center);
			col.IsEditable=true;
			gridItems.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Price/Unit"),70,HorizontalAlignment.Right);
			col.IsEditable=true;
			gridItems.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Subtotal"),70,HorizontalAlignment.Right);
			gridItems.Columns.Add(col);
			gridItems.Rows.Clear();
			ODGridRow row;
			double price;
			int qty;
			double subtotal;
			double total=0;
			bool autocalcTotal=true;
			for(int i=0;i<_tableOrderItems.Rows.Count;i++) {
				row=new ODGridRow();
				//if(gridOrders.GetSelectedIndex()==-1){
				//	row.Cells.Add("");
				//}
				//else{
				//	row.Cells.Add(Suppliers.GetName(ListSuppliers,ListOrders[gridOrders.GetSelectedIndex()].SupplierNum));
				//}
				row.Cells.Add(_tableOrderItems.Rows[i]["CatalogNumber"].ToString());
				row.Cells.Add(_tableOrderItems.Rows[i]["Descript"].ToString());
				qty=PIn.Int(_tableOrderItems.Rows[i]["Qty"].ToString());
				row.Cells.Add(qty.ToString());
				price=PIn.Double(_tableOrderItems.Rows[i]["Price"].ToString());
				row.Cells.Add(price.ToString("n"));
				subtotal=((double)qty)*price;
				row.Cells.Add(subtotal.ToString("n"));
				gridItems.Rows.Add(row);
				if(subtotal==0) {
					autocalcTotal=false;
				}
				total+=subtotal;
			}
			gridItems.EndUpdate();
			if(gridOrders.GetSelectedIndex()!=-1 
				&& autocalcTotal
				&& total!=_listOrders[gridOrders.GetSelectedIndex()].AmountTotal) 
			{
				SupplyOrder order=_listOrders[gridOrders.GetSelectedIndex()].Copy();
				order.AmountTotal=total;
				SupplyOrders.Update(order);
				FillGridOrders();
				for(int i=0;i<_listOrders.Count;i++) {
					if(_listOrders[i].SupplyOrderNum==order.SupplyOrderNum) {
						gridOrders.SetSelected(i,true);
					}
				}
			}
		}

		private void gridOrderItem_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormSupplyOrderItemEdit FormSOIE = new FormSupplyOrderItemEdit();
			FormSOIE.ItemCur = SupplyOrderItems.CreateObject(PIn.Long(_tableOrderItems.Rows[e.Row]["SupplyOrderItemNum"].ToString()));
			FormSOIE.ListSupplier = Suppliers.GetAll();
			FormSOIE.ShowDialog();
			if(FormSOIE.DialogResult!=DialogResult.OK) {
				return;
			}
			SupplyOrderItems.Update(FormSOIE.ItemCur);
			UpdatePriceAndRefresh();
		}

		///<summary>Used to update subtotal when qty or price are edited.</summary>
		private void calculateSubtotalHelper() {
			try {
				gridItems.Rows[gridItems.SelectedCell.Y].ColorBackG=Color.White;
				if(gridItems.SelectedCell.X==2) {//Qty
					int qty=Int32.Parse(gridItems.Rows[gridItems.SelectedCell.Y].Cells[gridItems.SelectedCell.X].Text);
					gridItems.Rows[gridItems.SelectedCell.Y].Cells[4].Text=(qty*PIn.Double(gridItems.Rows[gridItems.SelectedCell.Y].Cells[3].Text)).ToString("n");
				}
				if(gridItems.SelectedCell.X==3) {//Price
					double price=Double.Parse(gridItems.Rows[gridItems.SelectedCell.Y].Cells[gridItems.SelectedCell.X].Text);
					gridItems.Rows[gridItems.SelectedCell.Y].Cells[4].Text=(price*PIn.Int(gridItems.Rows[gridItems.SelectedCell.Y].Cells[2].Text)).ToString("n");
				}
				Application.DoEvents();
				//save changes to order item on cell leave
			}
			catch(Exception ex) {
				ex.DoNothing();
				//problem calculating or parsing amount.
				gridItems.Rows[gridItems.SelectedCell.Y].ColorBackG=Color.LightPink;
				gridItems.Rows[gridItems.SelectedCell.Y].Cells[4].Text=0.ToString("n");
			}
		}

		private void butNewOrder_Click(object sender,EventArgs e) {
			if(comboSupplier.SelectedIndex < 1) {//Includes no items or the ALL supplier being selected.
				MsgBox.Show(this,"Please select a supplier first.");
				return;
			}
			for(int i=0;i<_listOrders.Count;i++) {
				if(_listOrders[i].DatePlaced.Year>2200) {
					MsgBox.Show(this,"Not allowed to add a new order when there is already one pending.  Please finish the other order instead.");
					return;
				}
			}
			SupplyOrder order=new SupplyOrder();
			if(comboSupplier.SelectedIndex==0) {//Supplier "All".
				order.SupplierNum=0;
			}
			else {//Specific supplier selected.
				order.SupplierNum=_listSuppliers[comboSupplier.SelectedIndex-1].SupplierNum;//SelectedIndex-1 because "All" is first option.
			}
			order.IsNew=true;
			order.DatePlaced=new DateTime(2500,1,1);
			order.Note="";
			SupplyOrders.Insert(order);
			_listOrdersAll=SupplyOrders.GetAll();//Refresh the list all.
			FillGridOrders();
			gridOrders.SetSelected(_listOrders.Count-1,true);
			gridOrders.ScrollToEnd();
			FillGridOrderItem();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(_tableOrderItems.Rows.Count<1) {
				MsgBox.Show(this,"Supply list is Empty.");
				return;
			}
			_pagesPrinted=0;
			_headingPrinted=false;
			_pd2=new PrintDocument();
			_pd2.DefaultPageSettings.Margins=new Margins(50,50,40,30);
			_pd2.PrintPage+=new PrintPageEventHandler(pd2_PrintPage);
			if(_pd2.DefaultPageSettings.PrintableArea.Height==0) {
				_pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
#if DEBUG
			FormRpPrintPreview pView=new FormRpPrintPreview();
			pView.printPreviewControl2.Document=_pd2;
			pView.ShowDialog();
#else
				if(PrinterL.SetPrinter(_pd2,PrintSituation.Default,0,"Supplies order from "+_listOrders[gridOrders.GetSelectedIndex()].DatePlaced.ToShortDateString()+" printed")) {
					try{
						_pd2.Print();
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
			if(!_headingPrinted) {
				text=Lan.g(this,"Supply List");
				g.DrawString(text,headingFont,Brushes.Black,425-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=Lan.g(this,"Order Number")+": "+_listOrders[gridOrders.SelectedIndices[0]].SupplyOrderNum;
				g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text=Lan.g(this,"Date")+": "+_listOrders[gridOrders.SelectedIndices[0]].DatePlaced.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				Supplier supCur=Suppliers.GetOne(_listOrders[gridOrders.SelectedIndices[0]].SupplierNum);
				text=supCur.Name;
				g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text=supCur.Phone;
				g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text=supCur.Note;
				g.DrawString(text,subHeadingFont,Brushes.Black,425-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				yPos+=15;
				_headingPrinted=true;
				_headingPrintH=yPos;
			}
			#endregion
			yPos=gridItems.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		///<summary>Save changes to orderItems based on input in grid.</summary>
		//private bool saveChangesHelper() {
		//	if(gridItems.Rows.Count==0) {
		//		return true;
		//	}
		//	//validate ------------------------------------------------------------------------
		//	for(int i=0;i<gridItems.Rows.Count;i++) {
		//		int qtyThisRow=0;
		//		double priceThisRow=0;
		//		if(gridItems.Rows[i].Cells[2].Text!=""){
		//			try{
		//					qtyThisRow=Int32.Parse(gridItems.Rows[i].Cells[2].Text);
		//			}
		//			catch{
		//				MsgBox.Show(this,"Please fix errors in Qty column first.");
		//				return false;
		//			}
		//		}
		//		if(gridItems.Rows[i].Cells[3].Text!=""){
		//			try{
		//					priceThisRow=double.Parse(gridItems.Rows[i].Cells[3].Text);
		//			}
		//			catch{
		//				MsgBox.Show(this,"Please fix errors in Price column first.");
		//				return false;
		//			}
		//		}
		//	}
		//	//Save changes---------------------------------------------------------------------------
		//	//List<SupplyOrderItem> listOrderItems=OpenDentBusiness.Crud.SupplyOrderItemCrud.TableToList(tableOrderItems);//turn table into list of supplyOrderItem objects
		//	for(int i=0;i<gridItems.Rows.Count;i++) {
		//		int qtyThisRow=PIn.Int(gridItems.Rows[i].Cells[2].Text);//already validated
		//		double priceThisRow=PIn.Double(gridItems.Rows[i].Cells[3].Text);//already validated
		//		if(qtyThisRow==PIn.Int(tableOrderItems.Rows[i]["Qty"].ToString())
		//			&& priceThisRow==PIn.Double(tableOrderItems.Rows[i]["Price"].ToString()))
		//		{
		//			continue;//no changes to order item.
		//		}
		//		SupplyOrderItem soi=new SupplyOrderItem();
		//		soi.SupplyNum=PIn.Long(tableOrderItems.Rows[i]["SupplyNum"].ToString());
		//		soi.SupplyOrderItemNum=PIn.Long(tableOrderItems.Rows[i]["SupplyOrderItemNum"].ToString());
		//		soi.SupplyOrderNum=ListOrders[gridOrders.GetSelectedIndex()].SupplyOrderNum;
		//		soi.Qty=qtyThisRow;
		//		soi.Price=priceThisRow;
		//		SupplyOrderItems.Update(soi);
		//	}//end gridItems
		//	SupplyOrders.UpdateOrderPrice(ListOrders[gridOrders.GetSelectedIndex()].SupplyOrderNum);
		//	int selectedIndex=gridOrders.GetSelectedIndex();
		//	ListOrdersAll = SupplyOrders.GetAll();//update new totals
		//	FillGridOrders();
		//	if(selectedIndex!=-1) {
		//		gridOrders.SetSelected(selectedIndex,true);
		//	}
		//	return true;
		//}

		private void gridItems_CellLeave(object sender,ODGridClickEventArgs e) {
			//no need to check which cell was edited, just reprocess both cells
			int qtyNew=0;//default value.
			try {
				qtyNew=PIn.Int(gridItems.Rows[e.Row].Cells[2].Text);//0 if not valid input
			}
			catch { }
			double priceNew=PIn.Double(gridItems.Rows[e.Row].Cells[3].Text);//0 if not valid input
			SupplyOrderItem suppOI=SupplyOrderItems.CreateObject(PIn.Long(_tableOrderItems.Rows[e.Row]["SupplyOrderItemNum"].ToString()));
			suppOI.Qty=qtyNew;
			suppOI.Price=priceNew;
			SupplyOrderItems.Update(suppOI);
			SupplyOrders.UpdateOrderPrice(suppOI.SupplyOrderNum);
			gridItems.Rows[e.Row].Cells[2].Text=qtyNew.ToString();//to standardize formatting.  They probably didn't type .00
			gridItems.Rows[e.Row].Cells[3].Text=priceNew.ToString("n");//to standardize formatting.  They probably didn't type .00
			gridItems.Rows[e.Row].Cells[4].Text=(qtyNew*priceNew).ToString("n");//to standardize formatting.  They probably didn't type .00
			gridItems.Invalidate();
			int si=gridOrders.GetSelectedIndex();
			_listOrdersAll=SupplyOrders.GetAll();
			FillGridOrders();
			gridOrders.SetSelected(si,true);
		}

		private void UpdatePriceAndRefresh() {
			SupplyOrder gridSelect=gridOrders.SelectedTag<SupplyOrder>();
			SupplyOrders.UpdateOrderPrice(_listOrders[gridOrders.GetSelectedIndex()].SupplyOrderNum);
			_listOrdersAll=SupplyOrders.GetAll();
			FillGridOrders();
			for(int i=0;i<gridOrders.Rows.Count;i++) {
				if(gridSelect!=null && ((SupplyOrder)gridOrders.Rows[i].Tag).SupplyOrderNum==gridSelect.SupplyOrderNum) {
					gridOrders.SetSelected(i,true);
				}
			}
			FillGridOrderItem();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//maybe rename to close, since most saving happens automatically.
			DialogResult=DialogResult.Cancel;
		}


	}
}
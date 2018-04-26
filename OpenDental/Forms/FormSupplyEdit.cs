using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSupplyEdit:ODForm {
		public Supply Supp;
		public List<Supplier> ListSupplier;
		private bool isHiddenInitialVal;
		private long categoryInitialVal;
		private Supply SuppOriginal;
		private List<Def> _listSupplyCatDefs;

		public FormSupplyEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSupplyEdit_Load(object sender,EventArgs e) {
			textSupplier.Text=Suppliers.GetName(ListSupplier,Supp.SupplierNum);
			SuppOriginal=Supp.Copy();
			_listSupplyCatDefs=Defs.GetDefsForCategory(DefCat.SupplyCats,true);
			for(int i=0;i<_listSupplyCatDefs.Count;i++){
				comboCategory.Items.Add(_listSupplyCatDefs[i].ItemName);
				if(Supp.Category==_listSupplyCatDefs[i].DefNum){
					comboCategory.SelectedIndex=i;
				}
			}
			if(comboCategory.SelectedIndex==-1){
				comboCategory.SelectedIndex=0;//There are no hidden cats, and presence of cats is checked before allowing user to add new.
			}
			categoryInitialVal=Supp.Category;
			textCatalogNumber.Text=Supp.CatalogNumber;
			textDescript.Text=Supp.Descript;
			if(Supp.LevelDesired!=0){
				textLevelDesired.Text=Supp.LevelDesired.ToString();
			}
			if(Supp.LevelOnHand!=0) {
				textLevelOnHand.Text=Supp.LevelOnHand.ToString();
			}
			if(Supp.Price!=0){
				textPrice.Text=Supp.Price.ToString("n");
			}
			checkIsHidden.Checked=Supp.IsHidden;
			isHiddenInitialVal=Supp.IsHidden;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(Supp.IsNew){
				DialogResult=DialogResult.Cancel;
			}
			if(!MsgBox.Show(this,true,"Delete?")){
				return;
			}
			try{
				Supplies.DeleteObject(Supp);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			Supp=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textLevelDesired.errorProvider1.GetError(textLevelDesired)!=""
				|| textPrice.errorProvider1.GetError(textPrice)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textDescript.Text==""){
				MsgBox.Show(this,"Please enter a description.");
				return;
			}
			Supp.Category=_listSupplyCatDefs[comboCategory.SelectedIndex].DefNum;
			Supp.CatalogNumber=textCatalogNumber.Text;
			Supp.Descript=textDescript.Text;
			Supp.LevelDesired=PIn.Float(textLevelDesired.Text);
			Supp.LevelOnHand=PIn.Float(textLevelOnHand.Text);
			Supp.Price=PIn.Double(textPrice.Text);
			Supp.IsHidden=checkIsHidden.Checked;
			if(Supp.Category!=categoryInitialVal) {
				Supp.ItemOrder=int.MaxValue;//changed categories, new or existing, move to bottom of new category.
			}
			//No longer saving changes from this form.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}
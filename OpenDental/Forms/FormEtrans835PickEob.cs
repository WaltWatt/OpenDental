using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEtrans835PickEob:ODForm {

		private string _messageText835;
		private List<string> _listEobTranIds;
		private Etrans _etrans;

		public FormEtrans835PickEob(List <string> listEobTranIds,string messageText835,Etrans etrans) {
			InitializeComponent();
			Lan.F(this);
			_listEobTranIds=listEobTranIds;
			_messageText835=messageText835;
			_etrans=etrans;
		}
		
		private void FormEtrans835PickEob_Load(object sender,EventArgs e) {
			FillGridEobs();
		}

		private void FillGridEobs() {
			gridEobs.BeginUpdate();
			gridEobs.Columns.Clear();
			gridEobs.Columns.Add(new UI.ODGridColumn("",0));
			gridEobs.Rows.Clear();
			for(int i=0;i<_listEobTranIds.Count;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				row.Cells.Add(_listEobTranIds[i]);
				gridEobs.Rows.Add(row);
			}
			gridEobs.EndUpdate();
		}

		private void gridEobs_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			FormEtrans835Edit Form835=new FormEtrans835Edit();
			Form835.EtransCur=_etrans;
			Form835.MessageText835=_messageText835;
			Form835.TranSetId835=_listEobTranIds[gridEobs.SelectedIndices[0]];
			Form835.Show();//Not attached to parent window because the user may have to close parent window to navigate other areas of the program.
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
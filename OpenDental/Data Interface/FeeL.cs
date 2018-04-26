using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FeeL {

		///<summary>Imports fees into the database from the provided file.</summary>
		///<param name="fileName">Must be a tab-delimited .xls or .csv file. Each row must have two columns. The first column must be the proc code
		///and the second column must be the fee amount.</param>
		public static void ImportFees(long feeSchedNum,long clinicNum,long provNum,string fileName,Form currentForm) {
			FeeCache _feeCache = new FeeCache();
			List<Fee> listNewFees=_feeCache.GetListFees(feeSchedNum,clinicNum,provNum);
			Action actionCloseFeeSchedImportProgress=ODProgressOld.ShowProgressStatus("FeeSchedImport",currentForm,
				Lan.g(currentForm,"Importing fees, please wait")+"...");
			string[] fields;
			double feeAmt;
			using(StreamReader sr=new StreamReader(fileName)){
				string line=sr.ReadLine();
				while(line!=null){
					currentForm.Cursor=Cursors.WaitCursor;
					fields=line.Split(new string[1] {"\t"},StringSplitOptions.None);
					if(fields.Length>1){// && fields[1]!=""){//we no longer skip blank fees
						if(fields[1]=="") {
							feeAmt=-1;//triggers deletion of existing fee, but no insert.
						}
						else {
							feeAmt=PIn.Double(fields[1]);
						}
						listNewFees=Fees.Import(fields[0],feeAmt,feeSchedNum,clinicNum,provNum,listNewFees);
					}
					line=sr.ReadLine();
				}
			}
			currentForm.Cursor=Cursors.Default;
			actionCloseFeeSchedImportProgress();
			MsgBox.Show(currentForm,"Fee schedule imported.");
			_feeCache.BeginTransaction();
			_feeCache.RemoveFees(feeSchedNum,clinicNum,provNum);
			foreach(Fee fee in listNewFees) {
				_feeCache.Add(fee);
			}
			currentForm.Cursor=Cursors.WaitCursor;
			_feeCache.SaveToDb();
			currentForm.Cursor=Cursors.Default;
		}

		/* doesn't seem to be used from anywhere
		///<summary>If the named fee schedule does not exist, then it will be created.  It always returns the defnum for the feesched used, regardless of whether it already existed.  procCode must have already been tested for valid code, and feeSchedName must not be blank.</summary>
		public static long ImportTrojan(string procCode,double amt,string feeSchedName) {
			FeeSched feeSched=FeeScheds.GetByExactName(feeSchedName);
			//if isManaged, then this should be done differently from here on out.
			if(feeSched==null){
				//add the new fee schedule
				feeSched=new FeeSched();
				feeSched.ItemOrder=FeeSchedC.ListLong.Count;
				feeSched.Description=feeSchedName;
				feeSched.FeeSchedType=FeeScheduleType.Normal;
				//feeSched.IsNew=true;
				FeeScheds.Insert(feeSched);
				//Cache.Refresh(InvalidType.FeeScheds);
				//Fees.Refresh();
				DataValid.SetInvalid(InvalidType.FeeScheds, InvalidType.Fees);
			}
			if(feeSched.IsHidden){
				feeSched.IsHidden=false;//unhide it
				FeeScheds.Update(feeSched);
				DataValid.SetInvalid(InvalidType.FeeScheds);
			}
			Fee fee=Fees.GetFee(ProcedureCodes.GetCodeNum(procCode),feeSched.FeeSchedNum);
			if(fee==null) {
				fee=new Fee();
				fee.Amount=amt;
				fee.FeeSched=feeSched.FeeSchedNum;
				fee.CodeNum=ProcedureCodes.GetCodeNum(procCode);
				Fees.Insert(fee);
			}
			else{
				fee.Amount=amt;
				Fees.Update(fee);
			}
			return feeSched.FeeSchedNum;
		}	*/

	}
}
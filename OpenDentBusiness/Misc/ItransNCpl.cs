using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenDentBusiness {
	public class ItransNCpl {
		///<summary></summary>
		[JsonProperty("Carriers")]
		private List<Carrier> ListCarriers { get; set; }
		///<summary>BINs is provided for retrieval of all outstanding response transactions.
		///Making a type 04 request to each of the supplied BINs will ensure that all outstanding mailboxes are cleared.</summary>
		[JsonProperty("Rot_Bins")]
		public List<string> ListRotBins { get; set; }
		///<summary></summary>
		[JsonProperty("Change_Log")]
		private List<ChangeLog> ListChangeLogs { get; set; }
		
		///<summary>Returns a blank string if there were no errors while attempting to update internal carriers using iTrans n-cpl.json file..</summary>
		public static string TryCarrierUpdate(bool isAutomatic=true,ItransImportFields fieldsToImport=ItransImportFields.None) {
			Clearinghouse clearinghouse=Clearinghouses.GetDefaultDental();
			if(clearinghouse.CommBridge!=EclaimsCommBridge.ITRANS
				|| string.IsNullOrEmpty(clearinghouse.ResponsePath)
				|| !File.Exists(ODFileUtils.CombinePaths(clearinghouse.ResponsePath,"ITRANS Claims Director.exe"))
				|| (isAutomatic && PrefC.GetString(PrefName.WebServiceServerName).ToLower()!=Dns.GetHostName().ToLower()))//Only server can run when isOnlyServer is true.
			{
				return Lans.g("Clearinghouse","ITRANS must be the default dental clearinghouse and your Report Path must be set first.");
			}
			Process process=new Process {
				StartInfo=new ProcessStartInfo {
					FileName=ODFileUtils.CombinePaths(clearinghouse.ResponsePath,"ITRANS Claims Director.exe"),
					Arguments=" --getncpl"
				}
			};
			process.Start();
			process.WaitForExit();
			string ncplFilePath=ODFileUtils.CombinePaths(clearinghouse.ResponsePath,"n-cpl.json");
			string json=File.ReadAllText(ncplFilePath);//Read n-cpl.json
			EtransMessageText msgTextPrev=EtransMessageTexts.GetMostRecentForType(EtransType.ItransNcpl);
			if(msgTextPrev!=null && msgTextPrev.MessageText==json) {
				return Lans.g("Clearinghouse","Carrier list has not changed since last checked.");//json has not changed since we last checked, no need to update.
			}
			//Save json as new etrans entry.
			Etrans etrans=Etranss.CreateEtrans(File.GetCreationTime(ncplFilePath),clearinghouse.HqClearinghouseNum,json,0);
			etrans.Etype=EtransType.ItransNcpl;
			Etranss.Insert(etrans);
			ItransNCpl iTransNCpl=null;
			try {
				iTransNCpl=JsonConvert.DeserializeObject<ItransNCpl>(json);//Deserialize n-cpl.json
			}
			catch(Exception ex) {
				ex.DoNothing();
				return Lans.g("Clearinghouse","Failed to import json.");
			}
			foreach(ItransNCpl.Carrier jsonCarrier in iTransNCpl.ListCarriers) {//Update providers.
				OpenDentBusiness.Carrier odCarrier=Carriers.GetByElectId(jsonCarrier.Bin);//Cached
				if(odCarrier==null) {//Carrier can not be matched to internal Carrier based on ElectID.
					if(!fieldsToImport.HasFlag(ItransImportFields.AddMissing)) {
						continue;
					}
					OpenDentBusiness.Carrier carrierNew=new OpenDentBusiness.Carrier();
					carrierNew.ElectID=jsonCarrier.Bin;
					carrierNew.IsCDA=true;
					carrierNew.CarrierName=jsonCarrier.Name.En;
					carrierNew.Phone=TelephoneNumbers.ReFormat(jsonCarrier.Telephone?.First().Value);
					if(jsonCarrier.Address.Count()>0) {
						Address add=jsonCarrier.Address.First();
						carrierNew.Address=add.Street1;
						carrierNew.Address2=add.Street2;
						carrierNew.City=add.City;
						carrierNew.State=add.Province;
						carrierNew.Zip=add.PostalCode;
					}
					carrierNew.CanadianSupportedTypes=GetSupportedTypes(jsonCarrier);
					carrierNew.CarrierName=jsonCarrier.Name.En;
					try {
						Carriers.Insert(carrierNew);
					}
					catch(Exception ex) {
						ex.DoNothing();
					}
					continue;
				}
				else if(!odCarrier.IsCDA) {
					continue;
				}
				OpenDentBusiness.Carrier odCarrierOld=odCarrier.Copy();
				odCarrier.CanadianSupportedTypes=GetSupportedTypes(jsonCarrier);
				odCarrier.CDAnetVersion=POut.Int(jsonCarrier.Versions.Max(x => PIn.Int(x)));
				List<ItransImportFields> listFields=Enum.GetValues(typeof(ItransImportFields)).Cast<ItransImportFields>().ToList();
				foreach(ItransImportFields field in listFields) {
					if(fieldsToImport==ItransImportFields.None) {
						break;//No point in looping.
					}
					if(field==ItransImportFields.None || !fieldsToImport.HasFlag(field)) {
						continue;
					}
					switch(field) {
						case ItransImportFields.Phone:
							if(jsonCarrier.Telephone.Count>0) {
								odCarrier.Phone=TelephoneNumbers.ReFormat(jsonCarrier.Telephone.First().Value);
							}
						break;
						case ItransImportFields.Address:
							if(jsonCarrier.Address.Count()>0) {
								Address add=jsonCarrier.Address.First();
								odCarrier.Address=add.Street1;
								odCarrier.Address2=add.Street2;
								odCarrier.City=add.City;
								odCarrier.State=add.Province;
								odCarrier.Zip=add.PostalCode;
							}
						break;
						case ItransImportFields.Name:
							odCarrier.CarrierName=jsonCarrier.Name.En;
						break;
					}
				}
				try {
					long userNum=0;
					if(!isAutomatic) {
						userNum=Security.CurUser.UserNum;
					}
					Carriers.Update(odCarrier,odCarrierOld,userNum);
				}
				catch(Exception ex) {
					ex.DoNothing();
				}
			}
			return "";//Blank string represents a completed update.
		}

		private static CanSupTransTypes GetSupportedTypes(Carrier jsonCarrier) {
			CanSupTransTypes supportedTypes=CanSupTransTypes.None;
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Eligibility_08,CanSupTransTypes.EligibilityTransaction_08);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Eligibility_18,CanSupTransTypes.EligibilityResponse_18);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Cob_07,CanSupTransTypes.CobClaimTransaction_07);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Claim_11,CanSupTransTypes.ClaimAckEmbedded_11e);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Claim_21,CanSupTransTypes.ClaimEobEmbedded_21e);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Reversal_02,CanSupTransTypes.ClaimReversal_02);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Reversal_12,CanSupTransTypes.ClaimReversalResponse_12);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Predetermination_03,CanSupTransTypes.PredeterminationSinglePage_03);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Predetermination_Multi,CanSupTransTypes.PredeterminationMultiPage_03);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Predetermination_13,CanSupTransTypes.PredeterminationAck_13|CanSupTransTypes.PredeterminationAckEmbedded_13e);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Predetermination_23,CanSupTransTypes.PredeterminationAck_13|CanSupTransTypes.PredeterminationAckEmbedded_13e);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Outstanding_04,CanSupTransTypes.RequestForOutstandingTrans_04);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Outstanding_14,CanSupTransTypes.OutstandingTransAck_14);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Summary_Reconciliation_05,CanSupTransTypes.RequestForSummaryReconciliation_05);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Summary_Reconciliation_15,CanSupTransTypes.SummaryReconciliation_15);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Payment_Reconciliation_06,CanSupTransTypes.RequestForPaymentReconciliation_06);
			UpdateSupportedTypes(supportedTypes,jsonCarrier.Payment_Reconciliation_16,CanSupTransTypes.PaymentReconciliation_16);
			return supportedTypes;
		}

		///<summary>Updates the given bitwise supportedType enum to include onSuccessTransType when jsonFieldValue passes validation.</summary>
		private static void UpdateSupportedTypes(CanSupTransTypes supportedType,EnumNXYZ jsonFieldValue,CanSupTransTypes onSuccessTransType) {
			supportedType=(jsonFieldValue==EnumNXYZ.Y?(supportedType|onSuccessTransType):supportedType);
		}

		private class Carrier {
			public EnglishFrenchStr Name { get; set; }
			public string Change_Date { get; set; }
			public List<CarrierTelephone> Telephone { get; set; }
			public string Bin { get; set; }
			public List<string> Versions { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Batch { get; set; }
			public long Age_Days { get; set; }
			public EnglishFrenchStr Policy_Number { get; set; }
			public EnglishFrenchStr Division_Number { get; set; }
			public EnglishFrenchStr Certificate_Number { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Claim_01 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Claim_11 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Claim_21 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Reversal_02 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Reversal_12 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_03 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_13 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_23 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Predetermination_Multi { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Outstanding_04 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Outstanding_14 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Summary_Reconciliation_05 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Summary_Reconciliation_15 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Payment_Reconciliation_06 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Payment_Reconciliation_16 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Cob_07 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Eligibility_08 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Eligibility_18 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Attachment_09 { get; set; }
			[JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
			public EnumNXYZ Attachment_19 { get; set; }
			public EnglishFrenchStr Cob_Instructions { get; set; }
			public EnglishFrenchStr Notes { get; set; }
			public ClaimsProcessor Claims_Processor { get; set; }
			public List<Address> Address { get; set; }
			public List<Network> Network { get; set; }
		}

		private class Address {
			public string Street1 { get; set; }
			public string Street2 { get; set; }
			public string City { get; set; }
			public string Province { get; set; }
			public string PostalCode { get; set; }
			public string Attention { get; set; }
			public EnglishFrenchStr Notes { get; set; }
		}

		private class EnglishFrenchStr {
			public string En { get; set; }
			public string Fr { get; set; }
		}

		private class ClaimsProcessor {
			public string ChangeDate { get; set; }
			public EnglishFrenchStr Name { get; set; }
			public EnglishFrenchStr ShortName { get; set; }
		}

		private class Network {
			public List<NetworkTelephone> Telephone { get; set; }
			public Load Load { get; set; }
			public string NetworkFolder { get; set; }
			public EnglishFrenchStr Name { get; set; }
			public string ChangeDate { get; set; }
		}

		private class Load {
			public string ChangeDate { get; set; }
			public long Percent { get; set; }
		}

		private class NetworkTelephone {
			public EnglishFrenchStr Name { get; set; }
			public string ChangeDate { get; set; }
			public string Phone { get; set; }
		}

		private class CarrierTelephone {
			public string ChangeDate { get; set; }
			public string Value { get; set; }
			public EnglishFrenchStr Name { get; set; }
		}

		private class ChangeLog {
			public string ChangeDate { get; set; }
			public EnglishFrenchStr Description { get; set; }
		}

		[JsonConverter(typeof(EnumConverter))]
		public enum EnumNXYZ {
			Missing,
			N,
			X,
			Y,
			Z 
		}

		public class EnumConverter:StringEnumConverter {
			public override object ReadJson(JsonReader reader,Type objectType,object existingValue,JsonSerializer serializer) {
				if(string.IsNullOrEmpty(reader.Value.ToString())) {
					return EnumNXYZ.Missing;
				}
				return base.ReadJson(reader,objectType,existingValue,serializer);
			}
		}

	}


	[Flags]
	public enum ItransImportFields {
		///<summary></summary>
		None=0,
		///<summary>When enabled, updates carrier.Phone field from ITRANS 2.0 json file.</summary>
		Phone=1,
		///<summary>When enabled, updates various carrier address fields from ITRANS 2.0 json file.</summary>
		Address=2,
		///<summary>When enabled, updates carrier.CarrierName field from ITRANS 2.0 json file.</summary>
		Name=4,
		///<summary>When enabled, inserts a new carrier from json file when it can not be matched to internal carrier via ElectID.</summary>
		AddMissing,
	}
}

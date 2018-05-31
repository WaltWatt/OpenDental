using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	[Serializable]
	public class SplitCollection:ICollection<PaySplit>,IXmlSerializable {
		private List<PaySplit> _listSplits=new List<PaySplit>();

		public int Count {
			get {
				return _listSplits.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public void Add(PaySplit paysplit) {
			if(_listSplits.Any(x => x.TagOD==paysplit.TagOD
				|| (x.SplitNum==paysplit.SplitNum && x.SplitNum!=0))) 
			{
				return;
			}
			if(string.IsNullOrEmpty((string)paysplit.TagOD)) {
				paysplit.TagOD=Guid.NewGuid().ToString();
			}
			_listSplits.Add(paysplit);
		}

		public void AddRange(List<PaySplit> listPaySplits) {
			foreach(PaySplit split in listPaySplits) {
				Add(split);
			}
		}

		public void Clear() {
			_listSplits.Clear();
		}

		public bool Contains(PaySplit paysplit) {
			return _listSplits.Any(x => x.TagOD==paysplit.TagOD
				|| (x.SplitNum==paysplit.SplitNum && x.SplitNum!=0));
		}

		public void CopyTo(PaySplit[] array,int arrayIndex) {
			for(int i=arrayIndex;i<_listSplits.Count;i++) {
				array[i]=_listSplits[i];
			}
		}

		public bool Remove(PaySplit paySplit) {
			return _listSplits.RemoveAll(x => x.TagOD==paySplit.TagOD || (x.SplitNum==paySplit.SplitNum && x.SplitNum!=0)) > 0;
		}

		public IEnumerator<PaySplit> GetEnumerator() {
			return _listSplits.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable<PaySplit>)_listSplits).GetEnumerator();
		}

		///<summary>Returns null.  Required when extending IXmlSerializable.</summary>
		public XmlSchema GetSchema() {
			return null;
		}

		public void ReadXml(XmlReader reader) {
			XmlSerializer serializer=new XmlSerializer(typeof(List<PaySplit>));
			bool wasEmpty=reader.IsEmptyElement;
			reader.Read();
			if(wasEmpty) {
				return;
			}
			_listSplits=(List<PaySplit>)serializer.Deserialize(reader);
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer) {
			XmlSerializer serializer=new XmlSerializer(typeof(List<PaySplit>));
			serializer.Serialize(writer,_listSplits);
		}
	}

	
}

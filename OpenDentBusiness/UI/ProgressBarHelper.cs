using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;

namespace OpenDentBusiness {
	public class ProgressBarHelper {
		private string _labelValue;
		private string _percentValue;
		private int _blockValue;
		private int _blockMax;
		private string _tagString;
		private ProgBarStyle _progressStyle;
		private ProgBarEventType _progressBarEventType;
		private int _marqueeSpeed;
		private string _labelTop;
		private bool _isValHidden;
		private bool _isTopHidden;
		private bool _isPercentHidden;
		

		///<summary>Used to set text in other controls to be displayed to the user like labels, text boxes, etc.</summary>
		public string LabelValue {
			get {
				return _labelValue;
			}
		}		
		
		///<summary>Used to set the label on the right of the progress bar</summary>
		public string PercentValue {
			get {
				return _percentValue;
			}
		}

		///<summary>Changes progress bar current block value</summary>
		public int BlockValue {
			get {
				return _blockValue;
			}
		}

		///<summary>Changes progress bar max value</summary>
		public int BlockMax {
			get {
				return _blockMax;
			}
		}

		///<summary>Used to uniquely identify this ODEvent for consumers. Can be null.</summary>
		public string TagString {
			get {
				return _tagString;
			}
		}

		///<summary>Changes progress bar style</summary>
		public ProgBarStyle ProgressStyle {
			get {
				return _progressStyle;
			}
			set
			{
				_progressStyle=value;
			}
		}

		///<summary>Indicates what event this progress bar helper represents.  Used heavily by FormProgressExtended.</summary>
		public ProgBarEventType ProgressBarEventType {
			get {
				return _progressBarEventType;
			}
			set {
				_progressBarEventType=value;
			}
		}

		///<summary>Changes progress bar marquee speed</summary>
		public int MarqueeSpeed {
			get {
				return _marqueeSpeed;
			}
		}

		public string LabelTop
		{
			get
			{
				return _labelTop;
			}
		}

		public bool IsValHidden
		{
			get
			{
				return _isValHidden;
			}
		}

		public bool IsTopHidden
		{
			get
			{
				return _isTopHidden;
			}
		}

		public bool IsPercentHidden
		{
			get
			{
				return _isPercentHidden;
			}
		}

		///<summary>Used as a shell to store information events need to update a progress window.</summary>
		public ProgressBarHelper(string labelValue,string percentValue="",int blockValue=0,int blockMax=0
			,ProgBarStyle progressStyle=ProgBarStyle.NoneSpecified,string tagString="",int marqueeSpeed=0,string labelTop="",bool isLeftValHidden=false,
			bool isTopHidden=false,bool isPercentHidden=false,ProgBarEventType progressBarEventType=ProgBarEventType.ProgressBar) 
		{
			_labelValue=labelValue;
			_percentValue=percentValue;
			_blockValue=blockValue;
			_blockMax=blockMax;
			_progressStyle=progressStyle;
			_progressBarEventType=progressBarEventType;
			_tagString=tagString;
			_marqueeSpeed=marqueeSpeed;
			_labelTop=labelTop;
			_isValHidden=isLeftValHidden;
			_isTopHidden=isTopHidden;
			_isPercentHidden=isPercentHidden;
		}
	}
	
}

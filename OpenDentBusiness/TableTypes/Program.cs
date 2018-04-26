using System;

namespace OpenDentBusiness{

	///<summary>Each row is a bridge to an outside program, frequently an imaging program.  Most of the bridges are hard coded, and simply need to be enabled.  But user can also add their own custom bridge.</summary>
	[Serializable]
	public class Program:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long ProgramNum;
		///<summary>Unique name for built-in program bridges. Not user-editable. enum ProgramName</summary>
		public string ProgName;
		///<summary>Description that shows.</summary>
		public string ProgDesc;
		///<summary>True if enabled.</summary>
		public bool Enabled;
		///<summary>The path of the executable to run or file to open.</summary>
		public string Path;
		///<summary>Some programs will accept command line arguments.</summary>
		public string CommandLine;
		///<summary>Notes about this program link. Peculiarities, etc.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Note;
		///<summary>If this is a Plugin, then this is the filename of the dll.  The dll must be located in the application directory.</summary>
		public string PluginDllName;
		///<summary>If no image, then will be an empty string.  In this case, the bitmap will be null when loaded from the database.
		///Must be a 22 x 22 image, and thus needs (width) x (height) x (depth) = 22 x 22 x 4 = 1936 bytes.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string ButtonImage;
		/// <summary>For custom program links only.  Stores the template of a file to be generated when launching the program link.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string FileTemplate;
		/// <summary>For custom program links only.  Stores the path of a file to be generated when launching the program link.</summary>
		public string FilePath;

		public Program Copy(){
			return (Program)this.MemberwiseClone();
		}

	}

	///<summary>This enum is stored in the database as strings rather than as numbers, so we can do the order alphabetically and we can change it whenever we want.</summary>
	public enum ProgramName {
		None,
		ActeonImagingSuite,
		Adstra,
		Apixia,
		Apteryx,
		AudaxCeph,
		BioPAK,
		///<summary>Newer version of MediaDent. Uses OLE COM interface.</summary>
		CADI,
		CallFire,
		Camsight,
		CaptureLink,
		Carestream,
		CentralDataStorage,
		Cerec,
		CleaRay,
		CliniView,
		ClioSoft,
		DBSWin,
		DemandForce,
		DentalEye,
		DentalIntel,
		DentalStudio,
		DentalTekSmartOfficePhone,
		DentForms,
		DentX,
		Dexis,
		Digora,
		Dimaxis,
		Divvy,
		Dolphin,
		DrCeph,
		Dropbox,
		Dxis,
		EasyNotesPro,
		eClinicalWorks,
		///<summary>electronic Rx.</summary>
		eRx,
		EvaSoft,
		EwooEZDent,
		FHIR,
		FloridaProbe,
		Guru,
		HandyDentist,
		HdxWill,
		HouseCalls,
		IAP,
		iCat,
		iDixel,
		ImageFX,
		iRYS,
		Lightyear,
		MediaDent,
		MiPACS,
		Mountainside,
		NewTomNNT,
		Office,
		///<summary>Please use Programs.UsingOrion where possible.</summary>
		Orion,
		OrthoCAD,
		OrthoInsight3d,
		OrthoPlex,
		Owandy,
		PandaPerio,
		PayConnect,
		PaySimple,
		Patterson,
		PerioPal,
		Podium,
		PracticeByNumbers,
		PracticeWebReports,
		Progeny,
		PT,
		PTupdate,
		RapidCall,
		RayMage,
		Romexis,
		Scanora,
		Schick,
		SFTP,
		Sirona,
		SMARTDent,
		Sopro,
		TigerView,
		Transworld,
		Triana,
		Trojan,
		Trophy,
		TrophyEnhanced,
		Tscan,
		UAppoint,
		Vipersoft,
		visOra,
		VistaDent,
		VixWin,
		VixWinBase36,
		VixWinBase41,
		VixWinNumbered,
		VixWinOld,
		Xcharge,
		XDR,
		XVWeb,
		ZImage
	}

	


}











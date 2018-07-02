
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental.UI.Voice {
	///<summary>Represents one command that a user can give.</summary>
	public class VoiceCommand {
		///<summary>The list of input commands that will trigger this action.</summary>
		public List<string> Commands;
		///<summary>The action to be performed when this command is given.</summary>
		public VoiceCommandAction ActionToPerform;
		///<summary>The areas of the program where this command can be used.</summary>
		public List<VoiceCommandArea> ListAreas;
		///<summary>The verbal feedback that will be given when this command is executed.</summary>
		public string Response {
			get {
				return _response??ActionToPerform.ToString();
			}
			set {
				_response=value;
			}
		}
		///<summary>Whether or not the Response should be spoken when this command is executed.</summary>
		public bool DoSayResponse=true;
		///<summary>The verbal feedback that will be given when this command is executed.</summary>
		private string _response;

		public VoiceCommand Copy() {
			VoiceCommand command=(VoiceCommand)MemberwiseClone();
			command.Commands=Commands.ToList();
			command.ListAreas=ListAreas.ToList();
			return command;
		}
	}

	///<summary>All the different actions that can be executed.</summary>
	public enum VoiceCommandAction {
		CreatePerioExam,
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Eleven,
		Twelve,
		Thirteen,
		Fourteen,
		Fifteen,
		Sixteen,
		Seventeen,
		Eighteen,
		Nineteen,
		Zero,
		ThreeTwoThree,//Twenty of the most common triplets to increase efficiency
		FourThreeFour,
		ThreeThreeThree,
		TwoTwoTwo,
		FourFourFour,
		TwoOneTwo,
		FourThreeThree,
		ThreeThreeFour,
		TwoTwoThree,
		ThreeTwoTwo,
		FiveFourFive,
		FiveThreeFive,
		ThreeThreeFive,
		FiveThreeThree,
		FourFourFive,
		FiveFourFour,
		FiveFiveFive,
		ThreeFourThree,
		FourThreeFive,
		FiveThreeFour,
		Triplets,
		CheckTriplets,
		UncheckTriplets,
		Bleeding,
		Calculus,
		Suppuration,
		Plaque,
		Backspace,
		SkipToothOne,
		SkipToothTwo,
		SkipToothThree,
		SkipToothFour,
		SkipToothFive,
		SkipToothSix,
		SkipToothSeven,
		SkipToothEight,
		SkipToothNine,
		SkipToothTen,
		SkipToothEleven,
		SkipToothTwelve,
		SkipToothThirteen,
		SkipToothFourteen,
		SkipToothFifteen,
		SkipToothSixteen,
		SkipToothSeventeen,
		SkipToothEighteen,
		SkipToothNineteen,
		SkipToothTwenty,
		SkipToothTwentyOne,
		SkipToothTwentyTwo,
		SkipToothTwentyThree,
		SkipToothTwentyFour,
		SkipToothTwentyFive,
		SkipToothTwentySix,
		SkipToothTwentySeven,
		SkipToothTwentyEight,
		SkipToothTwentyNine,
		SkipToothThirty,
		SkipToothThirtyOne,
		SkipToothThirtyTwo,
		Left,
		Right,
		Delete,
		CopyPrevious,
		StartListening,
		StopListening,
		GiveFeedback,
		StopGivingFeedback,
		DidntGetThat,
		GoToToothOneFacial,
		GoToToothTwoFacial,
		GoToToothFourFacial,
		GoToToothThreeFacial,
		GoToToothFiveFacial,
		GoToToothSixFacial,
		GoToToothSevenFacial,
		GoToToothEightFacial,
		GoToToothNineFacial,
		GoToToothTenFacial,
		GoToToothElevenFacial,
		GoToToothTwelveFacial,
		GoToToothThirteenFacial,
		GoToToothFourteenFacial,
		GoToToothFifteenFacial,
		GoToToothSixteenFacial,
		GoToToothSeventeenFacial,
		GoToToothEighteenFacial,
		GoToToothNineteenFacial,
		GoToToothTwentyFacial,
		GoToToothTwentyOneFacial,
		GoToToothTwentyTwoFacial,
		GoToToothTwentyThreeFacial,
		GoToToothTwentyFourFacial,
		GoToToothTwentyFiveFacial,
		GoToToothTwentySixFacial,
		GoToToothTwentySevenFacial,
		GoToToothTwentyEightFacial,
		GoToToothTwentyNineFacial,
		GoToToothThirtyFacial,
		GoToToothThirtyOneFacial,
		GoToToothThirtyTwoFacial,
		GoToToothOneLingual,
		GoToToothTwoLingual,
		GoToToothFourLingual,
		GoToToothThreeLingual,
		GoToToothFiveLingual,
		GoToToothSixLingual,
		GoToToothSevenLingual,
		GoToToothEightLingual,
		GoToToothNineLingual,
		GoToToothTenLingual,
		GoToToothElevenLingual,
		GoToToothTwelveLingual,
		GoToToothThirteenLingual,
		GoToToothFourteenLingual,
		GoToToothFifteenLingual,
		GoToToothSixteenLingual,
		GoToToothSeventeenLingual,
		GoToToothEighteenLingual,
		GoToToothNineteenLingual,
		GoToToothTwentyLingual,
		GoToToothTwentyOneLingual,
		GoToToothTwentyTwoLingual,
		GoToToothTwentyThreeLingual,
		GoToToothTwentyFourLingual,
		GoToToothTwentyFiveLingual,
		GoToToothTwentySixLingual,
		GoToToothTwentySevenLingual,
		GoToToothTwentyEightLingual,
		GoToToothTwentyNineLingual,
		GoToToothThirtyLingual,
		GoToToothThirtyOneLingual,
		GoToToothThirtyTwoLingual,
		Probing,
		MucoGingivalJunction,
		GingivalMargin,
		Furcation,
		Mobility,
		PlusOne,
		PlusTwo,
		PlusThree,
		PlusFour,
		PlusFive,
		PlusSix,
		PlusSeven,
		PlusNine,
		PlusEight,
		SkipCurrentTooth,
		Ok,
		Cancel,
		Yes,
		No,
	}
	
	public enum VoiceCommandArea {
		Global,
		PerioChart,
		FormOpenDental,
		VoiceMsgBox,
	}
}

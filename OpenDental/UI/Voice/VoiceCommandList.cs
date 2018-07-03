using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental.UI.Voice {
	///<summary>A list of all voice commands used in the program.</summary>
	public class VoiceCommandList {
		private static List<VoiceCommand> _commands=new List<VoiceCommand> {
			#region Global
				new VoiceCommand {
					Commands=new List<string> {
						"start listening",
					},
					ActionToPerform=VoiceCommandAction.StartListening,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.Global },
					Response="Listening"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"stop listening",
					},
					ActionToPerform=VoiceCommandAction.StopListening,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.Global },
					Response="No longer listening"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"give feedback",
						"start giving feedback",
						"turn feedback on"
					},
					ActionToPerform=VoiceCommandAction.GiveFeedback,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.Global },
					Response="Giving feedback"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"stop giving feedback",
						"turn feedback off"
					},
					ActionToPerform=VoiceCommandAction.StopGivingFeedback,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.Global },
					Response="No longer giving feedback"
				},
				new VoiceCommand {
					Commands=new List<string> { },
					ActionToPerform=VoiceCommandAction.DidntGetThat,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.Global, VoiceCommandArea.VoiceMsgBox },
					Response="I didn't get that"
				},
			#endregion Global
			#region PerioChart
				new VoiceCommand {
					Commands=new List<string> {
						"add perio exam",
						"new perio exam"
					},
					ActionToPerform=VoiceCommandAction.CreatePerioExam,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Adding perio exam"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"zero"
					},
					ActionToPerform=VoiceCommandAction.Zero,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"one"
					},
					ActionToPerform=VoiceCommandAction.One,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"two"
					},
					ActionToPerform=VoiceCommandAction.Two,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"three"
					},
					ActionToPerform=VoiceCommandAction.Three,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"four"
					},
					ActionToPerform=VoiceCommandAction.Four,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"five"
					},
					ActionToPerform=VoiceCommandAction.Five,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"six"
					},
					ActionToPerform=VoiceCommandAction.Six,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"seven"
					},
					ActionToPerform=VoiceCommandAction.Seven,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"eight"
					},
					ActionToPerform=VoiceCommandAction.Eight,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"nine"
					},
					ActionToPerform=VoiceCommandAction.Nine,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"ten"
					},
					ActionToPerform=VoiceCommandAction.Ten,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"eleven"
					},
					ActionToPerform=VoiceCommandAction.Eleven,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"twelve"
					},
					ActionToPerform=VoiceCommandAction.Twelve,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"thirteen"
					},
					ActionToPerform=VoiceCommandAction.Thirteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"fourteen"
					},
					ActionToPerform=VoiceCommandAction.Fourteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"fifteen"
					},
					ActionToPerform=VoiceCommandAction.Fifteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"sixteen"
					},
					ActionToPerform=VoiceCommandAction.Sixteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"seventeen"
					},
					ActionToPerform=VoiceCommandAction.Seventeen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"eightteen"
					},
					ActionToPerform=VoiceCommandAction.Eighteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"nineteen"
					},
					ActionToPerform=VoiceCommandAction.Nineteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"three two three"
					},
					ActionToPerform=VoiceCommandAction.ThreeTwoThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"four three four"
					},
					ActionToPerform=VoiceCommandAction.FourThreeFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"three three three"
					},
					ActionToPerform=VoiceCommandAction.ThreeThreeThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"two two two"
					},
					ActionToPerform=VoiceCommandAction.TwoTwoTwo,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"four four four"
					},
					ActionToPerform=VoiceCommandAction.FourFourFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"two one two"
					},
					ActionToPerform=VoiceCommandAction.TwoOneTwo,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"four three three"
					},
					ActionToPerform=VoiceCommandAction.FourThreeThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"three three four"
					},
					ActionToPerform=VoiceCommandAction.ThreeThreeFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"two two three"
					},
					ActionToPerform=VoiceCommandAction.TwoTwoThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"three two two"
					},
					ActionToPerform=VoiceCommandAction.ThreeTwoTwo,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"five four five"
					},
					ActionToPerform=VoiceCommandAction.FiveFourFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"five three five"
					},
					ActionToPerform=VoiceCommandAction.FiveThreeFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"three three five"
					},
					ActionToPerform=VoiceCommandAction.ThreeThreeFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"five three three"
					},
					ActionToPerform=VoiceCommandAction.FiveThreeThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"four four five"
					},
					ActionToPerform=VoiceCommandAction.FourFourFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"five four four"
					},
					ActionToPerform=VoiceCommandAction.FiveFourFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"five five five"
					},
					ActionToPerform=VoiceCommandAction.FiveFiveFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"three four three"
					},
					ActionToPerform=VoiceCommandAction.ThreeFourThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"four three five"
					},
					ActionToPerform=VoiceCommandAction.FourThreeFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"five three four"
					},
					ActionToPerform=VoiceCommandAction.FiveThreeFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"triplet",
						"triplets",
					},
					ActionToPerform=VoiceCommandAction.Triplets,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"check triplets",
					},
					ActionToPerform=VoiceCommandAction.CheckTriplets,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"uncheck triplets",
					},
					ActionToPerform=VoiceCommandAction.UncheckTriplets,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"bleeding",
						"mark bleeding"
					},
					ActionToPerform=VoiceCommandAction.Bleeding,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"calculus",
						"mark calculus"
					},
					ActionToPerform=VoiceCommandAction.Calculus,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plaque"
					},
					ActionToPerform=VoiceCommandAction.Plaque,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"suppuration"
					},
					ActionToPerform=VoiceCommandAction.Suppuration,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						//"back", //This got confused with 'five' too often.
						"backspace"
					},
					ActionToPerform=VoiceCommandAction.Backspace,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"left"
					},
					ActionToPerform=VoiceCommandAction.Left,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"right"
					},
					ActionToPerform=VoiceCommandAction.Right,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"delete"
					},
					ActionToPerform=VoiceCommandAction.Delete,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"copy previous",
						"copy previous exam"
					},
					ActionToPerform=VoiceCommandAction.CopyPrevious,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Copying previous exam"
				},

				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth one facial",
						"go to one facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothOneFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth one facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth two facial",
						"go to two facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwoFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth two facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth three facial",
						"go to three facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThreeFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth three facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth four facial",
						"go to four facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFourFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth four facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth five facial",
						"go to five facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFiveFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth five facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth six facial",
						"go to six facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSixFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth six facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth seven facial",
						"go to seven facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSevenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth seven facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth eight facial",
						"go to eight facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothEightFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eight facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth nine facial",
						"go to nine facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothNineFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth nine facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth ten facial",
						"go to ten facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth ten facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth eleven facial",
						"go to eleven facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothElevenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eleven facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twelve facial",
						"go to twelve facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwelveFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twelve facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirteen facial",
						"go to thirteen facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirteenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirteen facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth fourteen facial",
						"go to fourteen facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFourteenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth fourteen facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth fifteen facial",
						"go to fifteen facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFifteenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth fifteen facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth sixteen facial",
						"go to sixteen facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSixteenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth sixteen facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth seventeen facial",
						"go to seventeen facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSeventeenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth seventeen facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth eighteen facial",
						"go to eighteen facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothEighteenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eighteen facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth nineteen facial",
						"go to nineteen facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothNineteenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth nineteen facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty facial",
						"go to twenty facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty one facial",
						"go to twenty one facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyOneFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty one facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty two facial",
						"go to twenty two facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyTwoFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty two facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty three facial",
						"go to twenty three facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyThreeFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty three facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty four facial",
						"go to twenty four facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyFourFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty four facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty five facial",
						"go to twenty five facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyFiveFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty five facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty six facial",
						"go to twenty six facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentySixFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty six facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty seven facial",
						"go to twenty seven facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentySevenFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty seven facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty eight facial",
						"go to twenty eight facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyEightFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty eight facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty nine facial",
						"go to twenty nine facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyNineFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty nine facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirty facial",
						"go to thirty facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirtyFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirty one facial",
						"go to thirty one facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirtyOneFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty one facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirty two facial",
						"go to thirty two facial"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirtyTwoFacial,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty two facial"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth one lingual",
						"go to one lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothOneLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth one lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth two lingual",
						"go to two lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwoLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth two lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth three lingual",
						"go to three lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThreeLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth three lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth four lingual",
						"go to four lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFourLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth four lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth five lingual",
						"go to five lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFiveLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth five lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth six lingual",
						"go to six lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSixLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth six lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth seven lingual",
						"go to seven lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSevenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth seven lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth eight lingual",
						"go to eight lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothEightLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eight lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth nine lingual",
						"go to nine lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothNineLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth nine lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth ten lingual",
						"go to ten lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth ten lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth eleven lingual",
						"go to eleven lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothElevenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eleven lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twelve lingual",
						"go to twelve lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwelveLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twelve lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirteen lingual",
						"go to thirteen lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirteenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirteen lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth fourteen lingual",
						"go to fourteen lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFourteenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth fourteen lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth fifteen lingual",
						"go to fifteen lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothFifteenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth fifteen lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth sixteen lingual",
						"go to sixteen lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSixteenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth sixteen lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth seventeen lingual",
						"go to seventeen lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothSeventeenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth seventeen lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth eighteen lingual",
						"go to eighteen lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothEighteenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eighteen lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth nineteen lingual",
						"go to nineteen lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothNineteenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth nineteen lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty lingual",
						"go to twenty lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty one lingual",
						"go to twenty one lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyOneLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty one lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty two lingual",
						"go to twenty two lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyTwoLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty two lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty three lingual",
						"go to twenty three lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyThreeLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty three lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty four lingual",
						"go to twenty four lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyFourLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty four lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty five lingual",
						"go to twenty five lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyFiveLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty five lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty six lingual",
						"go to twenty six lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentySixLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty six lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty seven lingual",
						"go to twenty seven lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentySevenLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty seven lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty eight lingual",
						"go to twenty eight lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyEightLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty eight lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth twenty nine lingual",
						"go to twenty nine lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothTwentyNineLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty nine lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirty lingual",
						"go to thirty lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirtyLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirty one lingual",
						"go to thirty one lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirtyOneLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty one lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"go to tooth thirty two lingual",
						"go to thirty two lingual"
					},
					ActionToPerform=VoiceCommandAction.GoToToothThirtyTwoLingual,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty two lingual"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"probing"
					},
					ActionToPerform=VoiceCommandAction.Probing,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"Muco Gingival Junction",
						"MGJ"
					},
					ActionToPerform=VoiceCommandAction.MucoGingivalJunction,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="MGJ"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"Gingival Margin"
					},
					ActionToPerform=VoiceCommandAction.GingivalMargin,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"Furcation"
					},
					ActionToPerform=VoiceCommandAction.Furcation,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"mobility"
					},
					ActionToPerform=VoiceCommandAction.Mobility,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus one"
					},
					ActionToPerform=VoiceCommandAction.PlusOne,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus two"
					},
					ActionToPerform=VoiceCommandAction.PlusTwo,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus three"
					},
					ActionToPerform=VoiceCommandAction.PlusThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus four"
					},
					ActionToPerform=VoiceCommandAction.PlusFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus five"
					},
					ActionToPerform=VoiceCommandAction.PlusFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus six"
					},
					ActionToPerform=VoiceCommandAction.PlusSix,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus seven"
					},
					ActionToPerform=VoiceCommandAction.PlusSeven,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus eight"
					},
					ActionToPerform=VoiceCommandAction.PlusEight,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"plus nine"
					},
					ActionToPerform=VoiceCommandAction.PlusNine,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth one"
					},
					ActionToPerform=VoiceCommandAction.SkipToothOne,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth one skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth two"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwo,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth two skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth three"
					},
					ActionToPerform=VoiceCommandAction.SkipToothThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth three skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth four"
					},
					ActionToPerform=VoiceCommandAction.SkipToothFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth four skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth five"
					},
					ActionToPerform=VoiceCommandAction.SkipToothFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth five skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth six"
					},
					ActionToPerform=VoiceCommandAction.SkipToothSix,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth six skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth seven"
					},
					ActionToPerform=VoiceCommandAction.SkipToothSeven,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth seven skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth eight"
					},
					ActionToPerform=VoiceCommandAction.SkipToothEight,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eight skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth nine"
					},
					ActionToPerform=VoiceCommandAction.SkipToothNine,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth nine skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth ten"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth ten skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth eleven"
					},
					ActionToPerform=VoiceCommandAction.SkipToothEleven,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eleven skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twelve"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwelve,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twelve skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth thirteen"
					},
					ActionToPerform=VoiceCommandAction.SkipToothThirteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirteen skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth fourteen"
					},
					ActionToPerform=VoiceCommandAction.SkipToothFourteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth fourteen skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth fifteen"
					},
					ActionToPerform=VoiceCommandAction.SkipToothFifteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth fifteen skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth sixteen"
					},
					ActionToPerform=VoiceCommandAction.SkipToothSixteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth sixteen skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth seventeen"
					},
					ActionToPerform=VoiceCommandAction.SkipToothSeventeen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth seventeen skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth eighteen"
					},
					ActionToPerform=VoiceCommandAction.SkipToothEighteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth eighteen skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth nineteen"
					},
					ActionToPerform=VoiceCommandAction.SkipToothNineteen,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth nineteen skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwenty,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty one"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentyOne,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty one skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty two"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentyTwo,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty two skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty three"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentyThree,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty three skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty four"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentyFour,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty four skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty five"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentyFive,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty five skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty six"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentySix,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty six skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty seven"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentySeven,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty seven skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty eight"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentyEight,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty eight skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth twenty nine"
					},
					ActionToPerform=VoiceCommandAction.SkipToothTwentyNine,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth twenty nine skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth thirty"
					},
					ActionToPerform=VoiceCommandAction.SkipToothThirty,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth thirty one"
					},
					ActionToPerform=VoiceCommandAction.SkipToothThirtyOne,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty one skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip tooth thirty two"
					},
					ActionToPerform=VoiceCommandAction.SkipToothThirtyTwo,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth thirty two skipped"
				},
				new VoiceCommand {
					Commands=new List<string> {
						"skip this tooth",
						"skip current tooth"
					},
					ActionToPerform=VoiceCommandAction.SkipCurrentTooth,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.PerioChart },
					Response="Tooth skipped"
				},
			#endregion PerioChart
			#region VoiceMsgBox
			new VoiceCommand {
					Commands=new List<string> {
						"yes",
					},
					ActionToPerform=VoiceCommandAction.Yes,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.VoiceMsgBox }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"no",
					},
					ActionToPerform=VoiceCommandAction.No,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.VoiceMsgBox }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"okay",
					},
					ActionToPerform=VoiceCommandAction.Ok,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.VoiceMsgBox }
				},
				new VoiceCommand {
					Commands=new List<string> {
						"cancel",
					},
					ActionToPerform=VoiceCommandAction.Cancel,
					ListAreas=new List<VoiceCommandArea> { VoiceCommandArea.VoiceMsgBox }
				},
			#endregion VoiceMsgBox
		};

		///<summary>Gets all the commands used for the given areas of the program.</summary>
		public static List<VoiceCommand> GetCommands(List<VoiceCommandArea> listAreas) {
			return _commands.FindAll(x => x.ListAreas.Any(y => y.In(listAreas)));
		}

	}
}

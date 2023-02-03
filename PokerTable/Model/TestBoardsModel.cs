using DBLib;
using DecisionMaking;
using PokerTable.View;
using PokerTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class TestBoardsModel
    {
        #region Members
        private string cardsBoards;
        private double raisePercents;
        private double callPercents;
        private double foldPercents;
        private ImageSource img1;
        private ImageSource img2;
        private ImageSource img3;
        private DecisionMaker lastDecisionMaker;
        #endregion
        #region Properties
        public DecisionMaker LastDecisionMaker
        {
            get
            {
                return lastDecisionMaker;
            }
            set
            {
                lastDecisionMaker = value;               
            }
        }
        public ImageSource Img1
        {
            get { return img1; }
            set { img1 = value; }
        }
        public ImageSource Img2
        {
            get { return img2; }
            set { img2 = value; }
        }
        public ImageSource Img3
        {
            get { return img3; }
            set { img3 = value; }
        }     
        public double FoldPercents
        {
            get { return foldPercents; }
            set
            {
                foldPercents = value;
            }
        }
        public double CallPercents
        {
            get { return callPercents; }
            set
            {
                callPercents = value;
            }
        }
        public double RaisePercents
        {
            get { return raisePercents; }
            set
            {
                raisePercents = value;
            }
        }
        public string CardsBoards
        {
            get { return cardsBoards; }
            set
            {
                cardsBoards = value;
            }
        }
        #endregion
        #region Methods
        public static TestBoardsModel CreateTestBoardItemFlop(DecisionMaker decisionMaker, string cardBoards, double raisePercent, double callPercent, double foldPercent)
        {
            TestBoardsModel testBoard = new TestBoardsModel();
            testBoard.lastDecisionMaker = decisionMaker;
            testBoard.cardsBoards = cardBoards;
            testBoard.img1 = ImageChange.GetImageSource(cardBoards.Substring(0, 2));
            testBoard.img2 = ImageChange.GetImageSource(cardBoards.Substring(2, 2));
            testBoard.img3 = ImageChange.GetImageSource(cardBoards.Substring(4, 2));
            testBoard.raisePercents = raisePercent;
            testBoard.callPercents = callPercent;
            testBoard.foldPercents = foldPercent;
            return testBoard;
        }
        public static TestBoardsModel CreateTestBoardItemTurn(string cardBoards, double raisePercent, double callPercent, double foldPercent, ref DecisionMaker decisionMaker)
        {
            TestBoardsModel testBoard = new TestBoardsModel();
            testBoard.cardsBoards = cardBoards;
            testBoard.img1 = ImageChange.GetImageSource(decisionMaker.hs.Cards[5]);          
            testBoard.lastDecisionMaker = decisionMaker;
            testBoard.raisePercents = raisePercent;
            testBoard.callPercents = callPercent;
            testBoard.foldPercents = foldPercent;
            return testBoard;
        }
        public static TestBoardsModel CreateTestBoardItemRiver(string cardBoards, double raisePercent, double callPercent, double foldPercent, ref DecisionMaker decisionMaker)
        {
            TestBoardsModel testBoard = new TestBoardsModel();
            testBoard.cardsBoards = cardBoards;
            testBoard.img1 = ImageChange.GetImageSource(decisionMaker.hs.Cards[6]);          
            testBoard.lastDecisionMaker = decisionMaker;
            testBoard.raisePercents = raisePercent;
            testBoard.callPercents = callPercent;
            testBoard.foldPercents = foldPercent;
            return testBoard;
        }
        public static void TestingDifferentBoards(ref bool canRun, ref EnumPhase phase, ref bool lastMoveBot, ref TestBoards testBoards, ref DecisionMaker decisionMakerBeforeDecision, ref List<DecisionState> allStates, ref HandState handStateBeforeLastDecision, ref ObservableCollection<PlayerViewModel> playersToPlay, ref List<bool> didPlayersPlay, ref List<string> CardsDeck, MainWindowViewModel mainWindowViewModel, ref bool testBoardsOpen)
        {
            try
            {
                if (!canRun && phase != EnumPhase.Preflop && lastMoveBot)
                {
                    testBoards = new();
                    var lastDecisionMakerCopy = MainWindowViewModel.GetDeepCopy(decisionMakerBeforeDecision);
                    var decisionStateForTest = MainWindowViewModel.CopyDecisionState(allStates[allStates.Count - 2]);
                    decisionStateForTest.DecisionMaker = lastDecisionMakerCopy;
                    decisionStateForTest.Hs = handStateBeforeLastDecision;
                    testBoards.DataContext = new TestBoardsViewModel(decisionStateForTest, playersToPlay, phase, didPlayersPlay, CardsDeck, mainWindowViewModel);
                    testBoards.Show();
                    testBoardsOpen = true;
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Testing Different Boards: " + ex.ToString(), LogLevel.Error);
            }
        }
        #endregion
    }
}

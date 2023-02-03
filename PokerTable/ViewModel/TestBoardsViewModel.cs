using DBLib;
using DecisionMaking;
using DecisionMaking.DecisionMaking;
using MicroMvvm;
using Microsoft.Win32;
using PokerTable.Model;
using PokerUtil;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Cursor = System.Windows.Input.Cursor;
using Cursors = System.Windows.Input.Cursors;

namespace PokerTable.ViewModel
{
    public class TestBoardsViewModel : ViewModelBase
    {
        #region Members
        private string path;
        private DecisionMaker lastDecisionMaker;
        private DecisionState lastState;
        private ObservableCollection<PlayerViewModel> playersToPlay;
        private DBLib.EnumPhase phaseFromMain;
        private List<bool> didPlayersPlayed;
        private ObservableCollection<TestBoardsModel> testBoards;
        private string betOrRaiseTxt;
        private string checkOrCallTxt;
        private string foldTxt;
        private string enteredCards;
        private List<string> deck;
        private bool hasRivalBet;
        private Visibility visibilityTurn;
        private Visibility visibilityRiver;
        private string rootBoard;
        private TestBoardsModel selectedBoardRow;
        private MainWindowViewModel mainViewModel;
        private List<string> cardsDeckFree;
        private int allHandsCopy;
        private string itemBoardCopy;
        private string oldHandsCardsCopy;
        private List<string> deckFromMainCopy;
        private Cursor cursor;
        private bool canExecuteProcess;
        private int playerIndex;
        private DecisionMaker lastDecMakerCopy;
        private List<Tuple<Decision, DecisionMaker>> tupleDecisionMakerList;
        private Visibility foldColumnVisibility;
        private string betRaiseAverage;
        private string checkCallAverage;
        private string foldAverage;
        private ObservableCollection<EnumBoardModel> enumBoardsModel;
        private Dictionary<EnumBoards, Tuple<double, double, double, int>> boardsDict;
        private double raiseSum;
        private double callSum;
        private double foldSum;
        private string allAverage;
        #endregion
        #region Constructor
        public TestBoardsViewModel(DecisionState decisionState, ObservableCollection<PlayerViewModel> playersPlays, DBLib.EnumPhase phase, List<bool> didPlayersPlay, List<string> cardsDeck, MainWindowViewModel mainWindowViewModel)
        {
            raiseSum = 0;
            callSum = 0;
            foldSum = 0;
            boardsDict = new();
            enumBoardsModel = new();
            enteredCards = "";
            lastDecMakerCopy = new DecisionMaker();
            playerIndex = 0;
            tupleDecisionMakerList = new List<Tuple<Decision, DecisionMaker>>();
            canExecuteProcess = true;
            cursor = Cursors.Arrow;
            cardsDeckFree = new List<string>();
            mainViewModel = mainWindowViewModel;
            selectedBoardRow = new TestBoardsModel();
            visibilityTurn = Visibility.Collapsed;
            visibilityRiver = Visibility.Collapsed;
            deck = new List<string>();
            hasRivalBet = false;
            lastState = decisionState;
            lastDecisionMaker = decisionState.DecisionMaker;
            playersToPlay = playersPlays;
            phaseFromMain = phase;
            didPlayersPlayed = didPlayersPlay;
            testBoards = new ObservableCollection<TestBoardsModel>();
            int counter = 0;
            foreach (var played in didPlayersPlay)
            {
                if (played)
                {
                    counter++;
                }
            }
            if (counter == 1)
            {
                hasRivalBet = false;
            }
            else
            {
                for (int i = 0; i < lastState.Hs.Bets.Count; i++)
                {
                    if (lastState.Hs.Bets[i] > 0)
                    {
                        hasRivalBet = true;
                    }
                }
            }
            deck = cardsDeck;
            if (lastDecisionMaker.Hero.Strategy.Params.Board != null)
            {
                rootBoard = lastDecisionMaker.Hero.Strategy.Params.Board;
            }
        }
        #endregion
        #region Properties
        public string AllAverage
        {
            get
            {
                return allAverage;
            }
            set
            {
                allAverage = value;
                OnPropertyChanged(nameof(AllAverage));
            }
        }
        public ObservableCollection<EnumBoardModel> EnumBoardsModel
        {
            get
            {
                return enumBoardsModel;
            }
            set
            {
                enumBoardsModel = value;
                OnPropertyChanged(nameof(EnumBoardsModel));
            }
        }
        public Cursor CursorMain
        {
            get { return cursor; }
            set
            {
                cursor = value;
                OnPropertyChanged(nameof(CursorMain));
            }
        }
        public string FoldTxt
        {
            get
            {
                return foldTxt;
            }
            set
            {
                foldTxt = value;
                OnPropertyChanged(nameof(FoldTxt));
            }
        }
        public Visibility FoldColumnVisibility
        {
            get
            {
                return foldColumnVisibility;
            }
            set
            {
                foldColumnVisibility = value;
                OnPropertyChanged(nameof(FoldColumnVisibility));
            }
        }
        public TestBoardsModel SelectedBoardRow
        {
            get
            {
                return selectedBoardRow;
            }
            set
            {
                selectedBoardRow = value;
                if (selectedBoardRow != null)
                {
                    List<string> cardsFromBoard = new();
                    SettingCardsOnTable(cardsFromBoard);
                    var cardsFree = cardsDeckFree;
                    Tuple<Decision, DecisionMaker> selectedTuple = GettingSelectedTuple();
                    DecisionMakingForSelectedTuple(selectedTuple);
                    var cardsPlayerFromTable = CreatingListOfSelectedPlayerCards(selectedTuple);
                    bool haveToChangeCards = IsPlayerHandSameAsBoard(cardsPlayerFromTable, mainViewModel.CardsOnTable);
                    if (haveToChangeCards)
                    {
                        MainWindowViewModel.ChangeHandCards(selectedTuple.Item2, mainViewModel.CardsOnTable, selectedTuple.Item2.GameType, mainViewModel.PlayersToPlay, playerIndex, mainViewModel.DecisionMakers);
                        mainViewModel.DecisionMakers[playerIndex].Hero.Strategy.Params.Board = selectedBoardRow.CardsBoards.ToLower();
                    }
                    else
                    {
                        mainViewModel.DecisionMakers[playerIndex].Hero.Strategy.Params.Board = selectedBoardRow.CardsBoards.ToLower();
                    }
                    var selectedDecisionMaker = selectedBoardRow.LastDecisionMaker;
                    if (mainViewModel.gameType == EnumGameType.CashGame)
                    {
                        mainViewModel.CashGamePostflop(ref selectedDecisionMaker);
                        mainViewModel.StringsLog = selectedDecisionMaker.Hero.Strategy.Params.statusMsgs;
                    }
                    else
                    {
                        mainViewModel.OmahaGamePostFlop(ref selectedDecisionMaker);
                        mainViewModel.StringsLog = selectedDecisionMaker.Hero.Strategy.Params.statusMsgs;
                    }
                }
            }
        }
        public Visibility VisibilityTurn
        {
            get
            {
                return visibilityTurn;
            }
            set
            {
                visibilityTurn = value;
                OnPropertyChanged(nameof(VisibilityTurn));
            }
        }
        public Visibility VisibilityRiver
        {
            get
            {
                return visibilityRiver;
            }
            set
            {
                visibilityRiver = value;
                OnPropertyChanged(nameof(VisibilityRiver));
            }
        }
        public string BetOrRaiseTxt
        {
            get
            {
                return betOrRaiseTxt;
            }
            set
            {
                betOrRaiseTxt = value;
                OnPropertyChanged(nameof(BetOrRaiseTxt));
            }
        }
        public string CheckOrCallTxt
        {
            get
            {
                return checkOrCallTxt;
            }
            set
            {
                checkOrCallTxt = value;
                OnPropertyChanged(nameof(CheckOrCallTxt));
            }
        }
        public string EnteredCards
        {
            get
            {
                return enteredCards;
            }
            set
            {
                enteredCards = value;
                OnPropertyChanged(nameof(EnteredCards));
            }
        }
        public string BetRaiseAverage
        {
            get
            {
                return betRaiseAverage;
            }
            set
            {
                betRaiseAverage = value;
                OnPropertyChanged(nameof(BetRaiseAverage));
            }
        }
        public string CheckCallAverage
        {
            get
            {
                return checkCallAverage;
            }
            set
            {
                checkCallAverage = value;
                OnPropertyChanged(nameof(CheckCallAverage));
            }
        }
        public string FoldAverage
        {
            get
            {
                return foldAverage;
            }
            set
            {
                foldAverage = value;
                OnPropertyChanged(nameof(FoldAverage));
            }
        }
        public ObservableCollection<TestBoardsModel> TestBoards
        {
            get
            {
                return testBoards;
            }
            set
            {
                testBoards = value;
                OnPropertyChanged(nameof(TestBoards));
            }
        }
        #endregion
        #region Methods
        private void ChangingHSCardsFromLastState(ref DecisionState lastState, string cardBoard)
        {
            if (phaseFromMain == EnumPhase.Flop)
            {
                lastState.Hs.Cards[2] = cardBoard.Substring(0, 2).ToLower();
                lastState.Hs.Cards[3] = cardBoard.Substring(2, 2).ToLower();
                lastState.Hs.Cards[4] = cardBoard.Substring(4, 2).ToLower();
            }
            else if (phaseFromMain == EnumPhase.Turn)
            {
                lastState.Hs.Cards[5] = cardBoard.ToLower();
            }
            else if (phaseFromMain == EnumPhase.River)
            {
                lastState.Hs.Cards[6] = cardBoard.ToLower();
            }
        }
        private void RemovingCardsBoardFromDeck(string item, ref List<string> deckFromMain)
        {
            List<string> cardsBoard = new();
            if (phaseFromMain == EnumPhase.Flop)
            {
                cardsBoard.Add(item.Substring(0, 2));
                cardsBoard.Add(item.Substring(2, 2));
                cardsBoard.Add(item.Substring(4, 2));
            }
            else if (phaseFromMain == DBLib.EnumPhase.Turn)
            {
                cardsBoard.Add(item);
            }
            else if (phaseFromMain == DBLib.EnumPhase.River)
            {
                cardsBoard.Add(item);
            }
            // deleting cards from Card deck
            foreach (var cardBoard in cardsBoard)
            {
                if (deckFromMain.Contains(cardBoard))
                {
                    deckFromMain.Remove(cardBoard);
                }
            }
        }
        private void SettingCardsOnTable(List<string> cardsFromBoard)
        {
            if (phaseFromMain == EnumPhase.Flop)
            {
                string card1 = selectedBoardRow.CardsBoards.Substring(0, 2);
                string card2 = selectedBoardRow.CardsBoards.Substring(2, 2);
                string card3 = selectedBoardRow.CardsBoards.Substring(4, 2);
                cardsFromBoard.Add(card1);
                cardsFromBoard.Add(card2);
                cardsFromBoard.Add(card3);
                mainViewModel.CardsOnTable[0] = card1;
                mainViewModel.CardsOnTable[1] = card2;
                mainViewModel.CardsOnTable[2] = card3;
                mainViewModel.CardOnTable1 = ImageChange.GetImageSource(card1);
                mainViewModel.CardOnTable2 = ImageChange.GetImageSource(card2);
                mainViewModel.CardOnTable3 = ImageChange.GetImageSource(card3);
                selectedBoardRow.LastDecisionMaker.Hero.Strategy.Params.Board = selectedBoardRow.CardsBoards.ToLower();
            }
            if (phaseFromMain == EnumPhase.Turn)
            {
                string card4 = selectedBoardRow.CardsBoards.Substring(6, 2);
                mainViewModel.CardsOnTable[3] = card4;
                cardsFromBoard.Add(card4);
                mainViewModel.CardOnTable4 = ImageChange.GetImageSource(card4);
                string boardTurn = mainViewModel.CardsOnTable[0].ToLower() + mainViewModel.CardsOnTable[1].ToLower() + mainViewModel.CardsOnTable[2].ToLower() + card4.ToLower();
                selectedBoardRow.LastDecisionMaker.Hero.Strategy.Params.Board = boardTurn;
            }
            else if (phaseFromMain == EnumPhase.River)
            {
                string card5 = selectedBoardRow.CardsBoards.Substring(8, 2);
                mainViewModel.CardsOnTable[4] = card5;
                cardsFromBoard.Add(card5);
                mainViewModel.CardOnTable5 = ImageChange.GetImageSource(card5);
                string boardTurn = mainViewModel.CardsOnTable[0].ToLower() + mainViewModel.CardsOnTable[1].ToLower() + mainViewModel.CardsOnTable[2].ToLower() + mainViewModel.CardsOnTable[3].ToLower() + card5.ToLower();
                selectedBoardRow.LastDecisionMaker.Hero.Strategy.Params.Board = boardTurn;
            }
        }
        private Tuple<Decision, DecisionMaker> GettingSelectedTuple()
        {
            Tuple<Decision, DecisionMaker> selectedTuple = new Tuple<Decision, DecisionMaker>(null, null);
            foreach (var item in tupleDecisionMakerList)
            {
                if (item.Item2.Hero.Strategy.Params.Board == selectedBoardRow.LastDecisionMaker.Hero.Strategy.Params.Board)
                {
                    selectedTuple = item;
                    break;
                }
            }
            return selectedTuple;
        }
        private void DecisionMakingForSelectedTuple(Tuple<Decision, DecisionMaker> selectedTuple)
        {
            for (int i = 0; i < mainViewModel.PlayersToPlay.Count; i++)
            {
                if (mainViewModel.PlayersToPlay[i].Name == selectedBoardRow.LastDecisionMaker.HeroName)
                {
                    var decisionFromTuple = selectedTuple.Item1;
                    var playerViewModel = mainViewModel.PlayersToPlay[i];
                    mainViewModel.DecisionMaking(ref decisionFromTuple, ref playerViewModel, ref playerIndex);
                }
            }
        }
        private List<string> CreatingListOfSelectedPlayerCards(Tuple<Decision, DecisionMaker> selectedTuple)
        {
            List<string> cardsPlayer = new();
            for (int i = 0; i < mainViewModel.PlayersToPlay.Count; i++)
            {
                if (mainViewModel.PlayersToPlay[i].InGame)
                {
                    if (mainViewModel.PlayersToPlay[i].Name == selectedTuple.Item2.HeroName)
                    {
                        cardsPlayer.Add(mainViewModel.PlayersToPlay[i].Cards.Substring(0, 2));
                        cardsPlayer.Add(mainViewModel.PlayersToPlay[i].Cards.Substring(2, 2));
                        if (selectedBoardRow.LastDecisionMaker.hs.OmahaCards != null)
                        {
                            cardsPlayer.Add(mainViewModel.PlayersToPlay[i].Cards.Substring(4, 2));
                            cardsPlayer.Add(mainViewModel.PlayersToPlay[i].Cards.Substring(6, 2));
                        }
                    }
                }
            }
            return cardsPlayer;
        }
        public static bool IsPlayerHandSameAsBoard(List<string> cardsPlayerFromTable, ObservableCollection<string> cardsOnTable)
        {
            bool isSame = false;
            try
            {
                var comparingLists = cardsOnTable.Except(cardsPlayerFromTable).ToList();
                if (comparingLists.Count != cardsOnTable.Count)
                {
                    isSame = true;
                }
                else
                {
                    isSame = false;
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Comparing Player Hands With Board, Method Name: IsPlayerHandSameAsBoard" + ex.ToString(), LogLevel.Error);
            }
            return isSame;
        }
        private void ChangingHandCards(ref List<string> decisionMakersHandCards, string item, ref List<string> deckFromMain, ref DecisionMaker lastDecisionMakerCopy, ref DecisionState decisionState)
        {
            List<string> newCardsOnTable = new();
            if (phaseFromMain == EnumPhase.Flop)
            {
                newCardsOnTable.Add(item.Substring(0, 2).ToLower());
                newCardsOnTable.Add(item.Substring(2, 2).ToLower());
                newCardsOnTable.Add(item.Substring(4, 2).ToLower());
            }
            else if (phaseFromMain == EnumPhase.Turn || phaseFromMain == EnumPhase.River)
            {
                newCardsOnTable.Add(item.Substring(0, 2).ToLower());
            }
            for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.MyRange.Count; i++)
            {
                List<string> rangeFromMyRange = new();
                rangeFromMyRange.Add(lastDecisionMakerCopy.Hero.Strategy.Params.MyRange[i].Substring(0, 2));
                rangeFromMyRange.Add(lastDecisionMakerCopy.Hero.Strategy.Params.MyRange[i].Substring(2, 2));
                if (decisionState.Hs.OmahaCards != null)
                {
                    rangeFromMyRange.Add(lastDecisionMakerCopy.Hero.Strategy.Params.MyRange[i].Substring(4, 2));
                    rangeFromMyRange.Add(lastDecisionMakerCopy.Hero.Strategy.Params.MyRange[i].Substring(6, 2));
                }
                var comparingLists = newCardsOnTable.Except(rangeFromMyRange).ToList();
                if (comparingLists.Count != newCardsOnTable.Count)
                {
                    continue;
                }
                else
                {
                    lastDecisionMakerCopy.Hero.Strategy.Params.Hand = lastDecisionMakerCopy.Hero.Strategy.Params.MyRange[i];
                    if (lastDecisionMakerCopy.hs.OmahaCards == null)
                    {
                        lastDecisionMakerCopy.hs.Cards[0] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(0, 2);
                        lastDecisionMakerCopy.hs.Cards[1] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(2, 2);
                        decisionState.Hs.Cards[0] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(0, 2);
                        decisionState.Hs.Cards[1] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(2, 2);
                    }
                    else
                    {
                        lastDecisionMakerCopy.hs.Cards[0] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(0, 2);
                        lastDecisionMakerCopy.hs.Cards[1] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(2, 2);
                        lastDecisionMakerCopy.hs.OmahaCards[0] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(0, 2);
                        lastDecisionMakerCopy.hs.OmahaCards[1] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(2, 2);
                        lastDecisionMakerCopy.hs.OmahaCards[2] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(4, 2);
                        lastDecisionMakerCopy.hs.OmahaCards[3] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(6, 2);
                        decisionState.Hs.Cards[0] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(0, 2);
                        decisionState.Hs.Cards[1] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(2, 2);
                        decisionState.Hs.OmahaCards[0] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(0, 2);
                        decisionState.Hs.OmahaCards[1] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(2, 2);
                        decisionState.Hs.OmahaCards[2] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(4, 2);
                        decisionState.Hs.OmahaCards[3] = lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(6, 2);
                    }
                    break;
                }
            }
        }
        private bool AreHSCardsSame(HandState handState)
        {
            List<string> hsCards = new();
            List<string> cardsList = new();
            bool areSame = false;
            if (handState.OmahaCards == null)
            {
                foreach (var item in handState.Cards)
                {
                    if (item != "")
                    {
                        hsCards.Add(item.ToLower());
                    }
                }
                if (phaseFromMain == EnumPhase.Flop)
                {
                    foreach (var item in hsCards)
                    {
                        if (!cardsList.Contains(item))
                        {
                            cardsList.Add(item);
                        }
                    }
                }
                areSame = false;
                if (cardsList.Count != hsCards.Count)
                {
                    areSame = true;
                }
            }
            else
            {
                foreach (var item in handState.OmahaCards)
                {
                    hsCards.Add(item.ToLower());
                }
                List<string> cardsFromBoard = new();
                for (int i = 2; i < handState.Cards.Count; i++)
                {
                    if (handState.Cards[i] != "")
                    {
                        cardsFromBoard.Add(handState.Cards[i]);
                    }
                }
                if (phaseFromMain == EnumPhase.Flop)
                {
                    cardsList = cardsFromBoard.Except(hsCards).ToList();
                    if (cardsList.Count != cardsFromBoard.Count)
                    {
                        areSame = true;
                    }
                    else
                    {
                        areSame = false;
                    }
                }
            }
            return areSame;
        }
        public static string ChangeCardToRandom(List<string> deckFromMain)
        {
            Random r = new Random();
            int cardInt = r.Next(0, deckFromMain.Count);
            string cardRandom = deckFromMain[cardInt];
            return cardRandom;
        }
        private ObservableCollection<string> SetNewCardsOnTable(ObservableCollection<string> cardsOnTable, DecisionState decisionState)
        {
            if (phaseFromMain == EnumPhase.Flop)
            {
                cardsOnTable.Add(decisionState.Hs.Cards[2]);
                cardsOnTable.Add(decisionState.Hs.Cards[3]);
                cardsOnTable.Add(decisionState.Hs.Cards[4]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[3]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[4]);
            }
            else if (phaseFromMain == EnumPhase.Turn)
            {
                cardsOnTable.Add(mainViewModel.CardsOnTable[0]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[1]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[2]);
                cardsOnTable.Add(decisionState.Hs.Cards[5]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[4]);
            }
            else if (phaseFromMain == EnumPhase.River)
            {
                cardsOnTable.Add(mainViewModel.CardsOnTable[0]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[1]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[2]);
                cardsOnTable.Add(mainViewModel.CardsOnTable[3]);
                cardsOnTable.Add(decisionState.Hs.Cards[6]);
            }
            return cardsOnTable;
        }
        /// <summary>
        ///  Getting all data from state for eneterd item for testing
        /// </summary>
        /// <param name="item">entered board item </param>
        private void GettingSettingDataFromState(string item)
        {
            var deckFromMain = deck;
            RemovingCardsBoardFromDeck(item, ref deckFromMain);
            var lastDecisionMakerCopy = MainWindowViewModel.GetDeepCopy(lastDecisionMaker);
            DecisionState lastStateCopy = MainWindowViewModel.CopyDecisionState(lastState);
            ChangingHSCardsFromLastState(ref lastStateCopy, item);
            //ComparingCardsAndChangeIfSame(lastStateCopy, deckFromMain);
            ObservableCollection<string> newCardsOnTable = new();
            newCardsOnTable = SetNewCardsOnTable(newCardsOnTable, lastStateCopy);
            int indexAct = 0;
            for (int i = 0; i < playersToPlay.Count; i++)
            {
                if (lastDecisionMakerCopy.HeroName == playersToPlay[i].Name)
                {
                    indexAct = i;
                    playerIndex = i;
                }
            }
            var hsCopy = StrategyUtil.DeepCopy<HandState>(lastStateCopy.Hs);
            hsCopy.Cards = lastStateCopy.Hs.Cards;
            var hsCopyRotated = DecisionModel.GetRotatedHs(lastStateCopy.Hs.InGame.Count, indexAct, hsCopy, playersToPlay, phaseFromMain, didPlayersPlayed);
            var oldHandCards = lastDecisionMakerCopy.Hero.Strategy.Params.Hand;
            if (AreHSCardsSame(hsCopyRotated))
            {
                List<string> decisionMakersHandCards = new();
                decisionMakersHandCards.Add(lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(0, 2));
                decisionMakersHandCards.Add(lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(2, 2));
                if (lastStateCopy.Hs.OmahaCards != null)
                {
                    decisionMakersHandCards.Add(lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(4, 2));
                    decisionMakersHandCards.Add(lastDecisionMakerCopy.Hero.Strategy.Params.Hand.Substring(6, 2));
                }
                ChangingHandCards(ref decisionMakersHandCards, item, ref deckFromMain, ref lastDecisionMakerCopy, ref lastStateCopy);
                hsCopyRotated.Cards[0] = lastDecisionMakerCopy.hs.Cards[0].ToLower();
                hsCopyRotated.Cards[1] = lastDecisionMakerCopy.hs.Cards[1].ToLower();
                if (lastStateCopy.Hs.OmahaCards != null)
                {
                    hsCopyRotated.OmahaCards[0] = lastStateCopy.Hs.OmahaCards[0].ToLower();
                    hsCopyRotated.OmahaCards[1] = lastStateCopy.Hs.OmahaCards[1].ToLower();
                    hsCopyRotated.OmahaCards[2] = lastStateCopy.Hs.OmahaCards[2].ToLower();
                    hsCopyRotated.OmahaCards[3] = lastStateCopy.Hs.OmahaCards[3].ToLower();
                }
            }
            else
            {
                for (int i = 0; i < hsCopyRotated.Cards.Count; i++)
                {
                    hsCopyRotated.Cards[i] = hsCopyRotated.Cards[i].ToLower();
                }
            }
            hsCopyRotated.OriginalNames = hsCopyRotated.Names;
            Decision decision = lastDecisionMakerCopy.MakeDecision(hsCopyRotated, true, null);
            if (decision.Action == EnumDecisionType.Exception)
            {
                MessageBox.Show("EXCEPTION!");
            }
            Tuple<Decision, DecisionMaker> decisionMakerDecision = new Tuple<Decision, DecisionMaker>(decision, lastDecisionMakerCopy);
            tupleDecisionMakerList.Add(decisionMakerDecision);
            int allhands = SettingAllHandsCount(lastDecisionMakerCopy);
            allHandsCopy = allhands;
            itemBoardCopy = item;
            oldHandsCardsCopy = oldHandCards;
            deckFromMainCopy = StrategyUtil.DeepCopy<List<string>>(deckFromMain);
            lastDecMakerCopy = lastDecisionMakerCopy;
        }
        private async Task ShowingBoardsInGridAsync(List<string> boards)
        {
            foreach (var item in boards)
            {
                try
                {
                    if (item != "")
                    {
                        await Task.Run(() => GettingSettingDataFromState(item));
                        ProcessBoard(allHandsCopy, lastDecMakerCopy, itemBoardCopy, oldHandsCardsCopy, deckFromMainCopy);
                    }
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in cards in loading board :" + ex.ToString());
                }
            }
            await Task.WhenAll();
            MessageBox.Show("Done!");
            if (FoldColumnVisibility == Visibility.Visible)
            {
                AllAverage = "All Average: " + BetOrRaiseTxt + ": " + Math.Round(raiseSum / TestBoards.Count, 1).ToString() + ", " + CheckOrCallTxt + ": " + Math.Round(callSum / TestBoards.Count, 1).ToString() + ", " + FoldTxt + ": " + Math.Round(foldSum / TestBoards.Count, 1).ToString();
            }
            else
            {
                AllAverage = "All Average: " + BetOrRaiseTxt + ": " + Math.Round(raiseSum / TestBoards.Count, 1).ToString() + ", " + CheckOrCallTxt + ": " + Math.Round(callSum / TestBoards.Count, 1).ToString();
            }
        }
        private static Tuple<double, double, double> CountTuplePercents(Tuple<double, double, double, int> tuple)
        {
            double raise = tuple.Item1;
            double call = tuple.Item2;
            double fold = tuple.Item3;
            double itemsCount = tuple.Item4;
            if (itemsCount > 1)
            {
                Tuple<double, double, double> newTuple = new(Math.Round(raise / itemsCount, 2), Math.Round(call / itemsCount, 2), Math.Round(fold / itemsCount));
               return newTuple;
            }
            else
            {
                Tuple<double, double, double> newTuple = new(raise, call, fold);
                return newTuple;
            }
        }
        /// <summary>
        ///  Setting values in grid, showing items in grid 
        /// </summary>
        /// <param name="allHandsCopy">count od hands</param>
        /// <param name="copyOfLastDecisionMakerCopy">last decisionMaker copy</param>
        /// <param name="itemBoardCopy"> item from list boards</param>
        /// <param name="oldHandCardsCopy">old hand cards</param>
        /// <param name="deckFromMainCopy"> copy of deck from MainWindowViewModel</param>
        private void ProcessBoard(int allHandsCopy, DecisionMaker copyOfLastDecisionMakerCopy, string itemBoardCopy, string oldHandCardsCopy, List<string> deckFromMainCopy)
        {
            SettingItemsAndPercentsForGrid(allHandsCopy, copyOfLastDecisionMakerCopy, itemBoardCopy);
            SettingDeckToOldValues(ref copyOfLastDecisionMakerCopy, ref oldHandCardsCopy, ref deckFromMainCopy);
            cardsDeckFree = deckFromMainCopy;
        }
        /// <summary>
        /// Loading Boards from TextBox
        /// </summary>
        private void LoadingDifferentBoardsText()
        {
            raiseSum = 0;
            callSum = 0;
            foldSum = 0;
            enumBoardsModel.Clear();
            canExecuteProcess = false;
            tupleDecisionMakerList.Clear();
            TestBoards.Clear();
            OnPropertyChanged(nameof(TestBoards));
            CursorMain = Cursors.Wait;
            OnPropertyChanged(nameof(CursorMain));
            try
            {
                SettingColumnsNames();
                if (enteredCards.Length > 0)
                {
                    List<string> boards = new();
                    try
                    {
                        var partsFile = enteredCards.Split(' ', ',');
                        for (int i = 0; i < partsFile.Length; i++)
                        {
                            if (phaseFromMain == EnumPhase.Flop)
                            {
                                if (partsFile[i].Length == 6)
                                {
                                    boards.Add(partsFile[i]);
                                }
                            }
                            else if (phaseFromMain == EnumPhase.Turn)
                            {
                                if (partsFile[i].Length == 2)
                                {
                                    boards.Add(partsFile[i]);
                                }
                            }
                            else if (phaseFromMain == EnumPhase.River)
                            {
                                if (partsFile[i].Length == 2)
                                {
                                    boards.Add(partsFile[i]);
                                }
                            }
                        }
                        EnumBoardsModel.Clear();
                        ShowingBoardsInGridAsync(boards);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in Loading Typed Text Board: " + ex.ToString());
                    }
                    EnteredCards = "";
                }
                CursorMain = Cursors.Arrow;
                OnPropertyChanged(nameof(CursorMain));
                canExecuteProcess = true;
            }
            catch (Exception ex)
            {
                Singleton.Log("Entered Cards null,  Exception in: " + ex.ToString());
                throw;
            }
        }
        /// <summary>
        /// Loading different boards,
        /// if phase == Flop, than reads board from File AllCombos
        /// if phase == Turn, than reads all free cards from cardDeck
        /// if phase == River, than reads all free cards from cardDeck
        /// </summary>
        private void LoadingDifferentBoardsAll()
        {
            enumBoardsModel.Clear();
            raiseSum = 0;
            callSum = 0;
            foldSum = 0;
            canExecuteProcess = false;
            tupleDecisionMakerList.Clear();
            TestBoards.Clear();
            OnPropertyChanged(nameof(TestBoards));
            CursorMain = Cursors.Wait;
            OnPropertyChanged(nameof(CursorMain));
            List<string> boards = new();
            SettingColumnsNames();
            if (phaseFromMain == EnumPhase.Flop)
            {
                try
                {
                    var flopCombos = @"\Files\AllFlopCombos.txt";
                    string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var combos = dirName + flopCombos;
                    var boardsFlop = File.ReadAllText(combos);
                    var partsFile = boardsFlop.Split(' ', ',', '\r', '\n', ':');
                    for (int i = 0; i < partsFile.Length; i++)
                    {
                        if (!partsFile[i].Contains(".") && partsFile[i].Length == 6)
                        {
                            boards.Add(partsFile[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in Loading All Flop Combos from file: " + ex.ToString());
                }
            }
            else if (phaseFromMain == EnumPhase.Turn)
            {
                List<string> allCardsFlop = new();
                try
                {
                    allCardsFlop = Card.Deck();                   
                    foreach (var item in mainViewModel.CardsOnTable)
                    {
                        allCardsFlop.Remove(item);
                    }
                    boards = allCardsFlop;
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in Loading Text Board ProcessAll: " + ex.ToString());
                }
            }
            else if (phaseFromMain == EnumPhase.River)
            {
                List<string> allCardsRiver = new();
                try
                {
                    allCardsRiver = Card.Deck();
                    foreach (var item in mainViewModel.CardsOnTable)
                    {
                        allCardsRiver.Remove(item);
                    }
                    boards = allCardsRiver;
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in Loading Text Board ProcessAll: " + ex.ToString());
                }
            }
            EnumBoardsModel.Clear();
            ShowingBoardsInGridAsync(boards);
            CursorMain = Cursors.Arrow;
            OnPropertyChanged(nameof(CursorMain));
            canExecuteProcess = true;
        }
        /// <summary>
        ///  Loading Boards from file from Dialog
        /// </summary>
        private void LoadingDifferentBoards()
        {
            raiseSum = 0;
            callSum = 0;
            foldSum = 0;
            canExecuteProcess = false;
            tupleDecisionMakerList.Clear();
            enumBoardsModel.Clear();
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            TestBoards.Clear();
            OnPropertyChanged(nameof(TestBoards));
            CursorMain = Cursors.Wait;
            OnPropertyChanged(nameof(CursorMain));
            if (openFileDialog.FileName != "")
            {
                path = openFileDialog.FileName;
            }
            try
            {
                if (path != null || path != "")
                {
                    string fileText = "";
                    fileText = File.ReadAllText(path);
                    List<string> boards = new();
                    var partsFile = fileText.Split('\r', ':', '\n');
                    for (int i = 0; i < partsFile.Length; i++)
                    {
                        if (phaseFromMain == EnumPhase.Flop)
                        {
                            if (partsFile[i].Length == 6)
                            {
                                boards.Add(partsFile[i]);
                            }
                        }
                        else if (phaseFromMain == EnumPhase.Turn)
                        {
                            if (partsFile[i].Length == 2)
                            {
                                boards.Add(partsFile[i]);
                            }
                        }
                        else if (phaseFromMain == EnumPhase.River)
                        {
                            if (partsFile[i].Length == 2)
                            {
                                boards.Add(partsFile[i]);
                            }
                        }
                    }
                    SettingColumnsNames();
                    EnumBoardsModel.Clear();
                    ShowingBoardsInGridAsync(boards);
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in opening file for Testing boards: " + ex.ToString());
            }
            CursorMain = Cursors.Arrow;
            OnPropertyChanged(nameof(CursorMain));
            canExecuteProcess = true;
        }
        private void SettingDeckToOldValues(ref DecisionMaker lastDecisionMakerCopy, ref string oldHandCards, ref List<string> deckFromMain)
        {
            lastDecisionMakerCopy.Hero.Strategy.Params.Hand = oldHandCards;
            lastDecisionMakerCopy.Hero.Strategy.Params.Board = rootBoard;
            deckFromMain = deck;
        }
        /// <summary>
        /// Showing items (cards and %) and creating item % 
        /// </summary>
        /// <param name="allhands">count of all hands</param>
        /// <param name="lastDecisionMakerCopy">last decisionMaker copy</param>
        /// <param name="item">board item for testing</param>
        private void SettingItemsAndPercentsForGrid(int allhands, DecisionMaker lastDecisionMakerCopy, string item)
        {
            TestBoardsModel board = new();
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                if (allhands != 0)
                {
                    if (lastDecisionMakerCopy.hs.OmahaCards == null)
                    {
                        if (phaseFromMain == EnumPhase.Flop)
                        {
                            if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                            {
                                int multiRangesCount = 0;
                                for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                                {
                                    multiRangesCount += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                                }
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemFlop(lastDecisionMakerCopy, item, MainWindowViewModel.GetPercHands(multiRangesCount, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.Print_range2.Combos.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.Print_range3.Combos.Count, allhands));
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                CreateTupleEnumBoards(board);
                            }
                            VisibilityTurn = Visibility.Collapsed;
                            VisibilityRiver = Visibility.Collapsed;
                        }
                        else if (phaseFromMain == EnumPhase.Turn)
                        {
                            if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                            {
                                int multiRangesCount = 0;
                                for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                                {
                                    multiRangesCount += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                                }
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemTurn(item, MainWindowViewModel.GetPercHands(multiRangesCount, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.Print_range2.Combos.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.Print_range3.Combos.Count, allhands), ref lastDecisionMakerCopy);
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                board.CardsBoards = lastDecisionMakerCopy.Hero.Strategy.Params.Board.Substring(0, 6) + item;
                                CreateTupleEnumBoards(board);
                            }
                            VisibilityTurn = Visibility.Visible;
                        }
                        else if (phaseFromMain == EnumPhase.River)
                        {
                            if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                            {
                                int multiRangesCount = 0;
                                for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                                {
                                    multiRangesCount += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                                }
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemRiver(item, MainWindowViewModel.GetPercHands(multiRangesCount, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.Print_range2.Combos.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.Print_range3.Combos.Count, allhands), ref lastDecisionMakerCopy);
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                board.CardsBoards = lastDecisionMakerCopy.Hero.Strategy.Params.Board.Substring(0, 8) + item;
                                CreateTupleEnumBoards(board);
                            }
                            VisibilityRiver = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (phaseFromMain == EnumPhase.Flop)
                        {
                            if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                            {
                                int multiRangesCount = 0;
                                for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                                {
                                    multiRangesCount += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                                }
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemFlop(lastDecisionMakerCopy, item, MainWindowViewModel.GetPercHands(multiRangesCount, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange.Count, allhands));
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                CreateTupleEnumBoards(board);
                            }
                        }
                        else if (phaseFromMain == EnumPhase.Turn)
                        {
                            if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                            {
                                int multiRangesCount = 0;
                                for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                                {
                                    multiRangesCount += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                                }
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemTurn(item, MainWindowViewModel.GetPercHands(multiRangesCount, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange.Count, allhands), ref lastDecisionMakerCopy);
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                board.CardsBoards = lastDecisionMakerCopy.Hero.Strategy.Params.Board.Substring(0, 6) + item;
                                CreateTupleEnumBoards(board);
                            }
                            else
                            {
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemTurn(item, MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange.Count, allhands), ref lastDecisionMakerCopy);
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                board.CardsBoards = lastDecisionMakerCopy.Hero.Strategy.Params.Board.Substring(0, 6) + item;
                                CreateTupleEnumBoards(board);
                            }
                            VisibilityTurn = Visibility.Visible;
                        }
                        else if (phaseFromMain == EnumPhase.River)
                        {
                            if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                            {
                                int multiRangesCount = 0;
                                for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                                {
                                    multiRangesCount += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                                }
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemRiver(item, MainWindowViewModel.GetPercHands(multiRangesCount, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange.Count, allhands), ref lastDecisionMakerCopy);
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                board.CardsBoards = lastDecisionMakerCopy.Hero.Strategy.Params.Board.Substring(0, 8) + item;
                                CreateTupleEnumBoards(board);
                            }
                            else
                            {
                                var testBoardItem = TestBoardsModel.CreateTestBoardItemRiver(item, MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange.Count, allhands), MainWindowViewModel.GetPercHands(lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange.Count, allhands), ref lastDecisionMakerCopy);
                                TestBoards.Add(testBoardItem);
                                board = testBoardItem;
                                board.CardsBoards = lastDecisionMakerCopy.Hero.Strategy.Params.Board.Substring(0, 8) + item;
                                CreateTupleEnumBoards(board);
                            }
                            VisibilityRiver = Visibility.Visible;
                        }
                    }
                }
            });
        }
        private void CreateTupleEnumBoards(TestBoardsModel board)
        {
            EnumBoards itemNew = BoardEstimation.GetEnumBoard(board.CardsBoards);
            string raiseBetAction = BetOrRaiseTxt;
            string checkCallAction = CheckOrCallTxt;
            string foldAction = FoldTxt;
            if (!boardsDict.ContainsKey(itemNew))
            {
                Tuple<double, double, double, int> tupleBoards = new(board.RaisePercents, board.CallPercents, board.FoldPercents, 1);
                boardsDict.Add(itemNew, tupleBoards);
            }
            else
            {
                var tupleRaise = boardsDict[itemNew].Item1 + board.RaisePercents;
                var tupleCall = boardsDict[itemNew].Item2 + board.CallPercents;
                var tupleFold = boardsDict[itemNew].Item3 + board.FoldPercents;
                var count = boardsDict[itemNew].Item4 + 1;
                Tuple<double, double, double, int> tupleBoards = new(tupleRaise, tupleCall, tupleFold, count);
                boardsDict[itemNew] = tupleBoards;
            }
            enumBoardsModel.Clear();
            //var enumBoard = itemNew;
            foreach (var item in boardsDict)
            {
                var tpl = CountTuplePercents(boardsDict[item.Key]);
                var itemEnumBoardModel = EnumBoardModel.CreateEnumBoardModel(item.Key.ToString(), tpl.Item1, tpl.Item2, tpl.Item3);
                enumBoardsModel.Add(itemEnumBoardModel);
            }
            raiseSum += board.RaisePercents;
            callSum += board.CallPercents;
            foldSum += board.FoldPercents;
        }
        private void SettingColumnsNames()
        {
            if (hasRivalBet)
            {
                BetOrRaiseTxt = "Raise";
                CheckOrCallTxt = "Call";
                FoldTxt = "Fold";
                FoldColumnVisibility = Visibility.Visible;
            }
            else
            {
                BetOrRaiseTxt = "Bet";
                CheckOrCallTxt = "Check";
                FoldTxt = "";
                FoldColumnVisibility = Visibility.Collapsed;
            }
            OnPropertyChanged(nameof(FoldColumnVisibility));
        }
        private static int SettingAllHandsCount(DecisionMaker lastDecisionMakerCopy)
        {
            int allHands = 0;
            if (lastDecisionMakerCopy.hs.OmahaCards == null)
            {
                if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                {
                    for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                    {
                        allHands += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                    }
                    allHands += lastDecisionMakerCopy.Hero.Strategy.Params.Print_range2.Combos.Count + lastDecisionMakerCopy.Hero.Strategy.Params.Print_range3.Combos.Count;
                }
            }
            else
            {
                if (lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count != 0)
                {
                    for (int i = 0; i < lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges.Count; i++)
                    {
                        allHands += lastDecisionMakerCopy.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                    }
                    allHands += lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange.Count + lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange.Count;
                }
                else
                {
                    allHands = lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange.Count + lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange.Count + lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange.Count;
                }
            }
            return allHands;
        }
        private bool CanExecute()
        {
            return true;
        }
        private bool CanExecuteProcess()
        {
            return canExecuteProcess;
        }
        #endregion
        #region Commands
        public ICommand LoadingBoards { get { return new RelayCommand(LoadingDifferentBoards, CanExecute); } }
        public ICommand LoadingBoardsProcess { get { return new RelayCommand(LoadingDifferentBoardsText, CanExecuteProcess); } }
        public ICommand LoadingBoardsProcessAll { get { return new RelayCommand(LoadingDifferentBoardsAll, CanExecuteProcess); } }
        #endregion
    }
}

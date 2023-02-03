using DBLib;
using DecisionMaking;
using DecisionMaking.DecisionMaking;
using DecisionMaking.statistika;
using MicroMvvm;
using Newtonsoft.Json;
using PokerTable.Model;
using PokerTable.View;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Player = PokerTable.Model.Player;

namespace PokerTable.ViewModel
{
    #region Enums
    enum GameEnums
    {
        NLH,
        PLO4
    }
    enum GamePauseResume
    {
        Run,
        Pause,
        Resume
    }
    enum BotOrHuman
    {
        Bot,
        Human
    }
    enum GridShow
    {
        BetRaise, CheckCall, Fold
    }
    enum BotDecisionText
    {
        Call,
        Check,
        Bet,
        Raise
    }
    #endregion
    public class MainWindowViewModel : ViewModelBase
    {
        #region Members
        private Cursor cursor;
        private int numberOfPlayers;
        private double potSize;
        private bool isHandStart;
        private ObservableCollection<PlayerView> playerViews;
        private Visibility isVisible;
        private PlayerViewModel player1;
        private PlayerViewModel player2;
        private PlayerViewModel player3;
        private PlayerViewModel player4;
        private PlayerViewModel player5;
        private PlayerViewModel player6;
        private PlayerViewModel player7;
        private PlayerViewModel player8;
        private PlayerViewModel player9;
        private ObservableCollection<string> cardsOnTable;
        private List<string> cardsDeck;
        private List<string> dealedCardsToPlayer;
        private string txtShow;
        private string txtShowHide;
        private List<ImageSource> playerImageSourceCards;
        private ObservableCollection<PlayerViewModel> playersToPlay;
        private List<string> imgCardsDealedToPlayer;
        private bool isDealer;
        private bool canExecuteShow;
        private bool canExecuteRaise;
        private bool canExecuteBet;
        private bool canExecuteHide;
        private ObservableCollection<int> playerCount;
        private ObservableCollection<ImageSource> rangeImgSources1;
        private ObservableCollection<ImageSource> rangeImgSources2;
        private ObservableCollection<ImageSource> rangeImgSources3;
        private ObservableCollection<ImageSource> rangeImgSources4;
        private ObservableCollection<ImageSource> rangeImgSourcesmiddle1;
        private ObservableCollection<ImageSource> rangeImgSourcesmiddle2;
        private ObservableCollection<ImageSource> rangeImgSourcesmiddle3;
        private ObservableCollection<ImageSource> rangeImgSourcesmiddle4;
        private double bigBlind;
        private double smallBlind;
        private int handCount;
        private int indexAct;
        private HandState hs;
        private Visibility isVisibleCard1OnTable;
        private Visibility isVisibleCard2OnTable;
        private Visibility isVisibleCard3OnTable;
        private Visibility isVisibleCard4OnTable;
        private Visibility isVisibleCard5OnTable;
        private string playerTimeToAct;
        private double waitDecision;
        private double dealingCardsDelay;
        private double thresholdRebuy;
        private Visibility holdemVisibility;
        private Visibility holdemOnlyVisibility;
        private Visibility omahaVisibility;
        private List<string> chooseGame;
        private string gameFromSettings;
        private string gameChoosen;
        private EnumPhase phase;
        public EnumGameType gameType;
        private List<string> range;
        private readonly object imgsLock = new object();
        private List<string> numShowHands;
        private string numShowHandsChoosen;
        private List<string> allBetCoeff;
        private string betCoeff;
        private bool showRangesShow;
        private string botHuman;
        private Visibility humanDecision;
        private Visibility isVisibleFoldBtn;
        private Visibility isVisibleCallBtn;
        private Visibility isVisibleRaiseBtn;
        private Visibility isVisibleCheckBtn;
        private Visibility isVisibleBetBtn;
        private Visibility betRaiseOmahaVisibility;
        private Visibility checkCallOmahaVisibility;
        private Visibility foldOmahaVisibility;
        private string textPercentHandBet;
        private string textPercentHandCheck;
        private string textPercentHandFold;
        private int countHands;
        private List<DecisionMaker> decisionMakers;
        private string pauseResume;
        private int sleepTimeForMove;
        private double betSizePlayer;
        private double betPercent;
        private string btnBetSize1;
        private string btnBetSize2;
        private string btnBetSize3;
        private string btnBetSize4;
        private string btnBetSize5;
        private Visibility visibilityBet5;
        private DecisionMaking.Range rangeDecision;
        private DecisionMaking.Range rangeDecision2;
        private List<MultiRange> rangeDecisionRaiseBet;
        private Visibility visibilityHoldemOrOmaha;
        private double sumbets = 0;
        private bool isAllIn = false;
        private bool phaseOver = false;
        private bool canRun;
        private bool humanPlayed = false;
        private ObservableCollection<RangePercents> rangeTextBetRaise;
        private ObservableCollection<RangePercents> rangeTextCallCheck;
        private ObservableCollection<RangePercents> rangeTextFold;
        private ObservableCollection<Omaha> omahaRanges;
        private ObservableCollection<Omaha> omahaRangesMid;
        private ObservableCollection<Omaha> omahaRangesFold;
        private ObservableCollection<Holdem> holdemRanges;
        private ObservableCollection<Holdem> holdemRangesMid;
        private ObservableCollection<Holdem> holdemRangesFold;
        private List<string> stringsLog;
        private List<string> allCards;
        private List<string> winningHands;
        private List<ImageSource> imageSourceCardOnTable1;
        public ObservableCollection<PlayerViewModel> PlayersToPlays;
        private List<bool> didPlayersPlay;
        private ObservableCollection<RangePercents> rangePercTemp;
        private ObservableCollection<RangePercents> rangePercTemp2;
        private ObservableCollection<RangePercents> rangePercTemp3;
        private Random r = new Random();
        private Visibility allBetCoeffVisibility;
        private Visibility visibilityOmahaPostflop;
        private DecisionMaker lastDecisionMaker;
        private Decision lastDecision;
        private List<string> enumGridShow;
        private string gridShow;
        private static ObservableCollection<Omaha> selectedShowdowns;
        private ObservableCollection<Omaha> omahaShowDowns;
        private ObservableCollection<Omaha> omahaShowDownsCheckCall;
        private ObservableCollection<Omaha> omahaShowDownsFold;
        private ObservableCollection<Omaha> omahaDraws;
        private ObservableCollection<Omaha> omahaDrawsCheckCall;
        private ObservableCollection<Omaha> omahaDrawsFold;
        private ObservableCollection<PokerUtil.EnumOmahaDraw> drawBetRaise;
        private ObservableCollection<PokerUtil.EnumOmahaDraw> drawCheckCall;
        private ObservableCollection<PokerUtil.EnumOmahaDraw> drawFold;
        private int widthHoldemAndOmahaGrid;
        private bool isVisiblePlayer;
        private int widthMainRight;
        private bool isGamePaused;
        private bool canExecuteUndo;
        private DecisionState lastState;
        private List<DecisionState> allStates;
        private bool isInitialized;
        private bool userClickedRun;
        private bool canExecuteLoad;
        private object lockNext = new object();
        private HandStateManual hsManual;
        private int indexForHH;
        private bool potDealt;
        private bool isUndoClicked;
        private bool isGeneratedChecked;
        private ObservableCollection<EnumCasino> enumCasinosCollection;
        private EnumCasino selectedEnumCasino;
        private Dictionary<string, string> dictFileHandID;
        private string pathName;
        private string hhPath;
        private HHBuilder hhBuilder;
        private string handIdPath;
        private string handId;
        private bool isNewHand;
        private bool canExecuteRun;
        private bool canExecuteNext;
        private ImageSource pauseResumeImg;
        private string botCheckCall;
        private string botBetRaise;
        private Visibility botButtonVisible;
        private bool lastMoveBot;
        private bool canTestBoards;
        private HandState handStateBeforeLastDecision;
        private DecisionState stateBeforeDecision;
        private DecisionMaker decisionMakerBeforeDecision;
        private TestBoards testBoards;
        private bool testBoardsOpen;
        private Visibility visibilityShowRangesShow;
        private Winnings winningsView;
        private bool canExecuteWinnings;
        private bool isNewSession;
        public int sessionHands;
        public Dictionary<string, double> dictStartBalance;
        public Dictionary<string, double> dictSessionBalance;
        public Dictionary<string, int> dictSessionHands;
        public Dictionary<string, double> dictallTimeBalance;
        public Dictionary<string, double> dictallTimeBalanceCurrent;
        public Dictionary<string, int> dictAllTimeHands;
        public Dictionary<string, int> dictAllTimeHandsCurrent;
        private string allTimeWinningsPath;
        private string allTimeWinningsHandsPath;
        private List<PlayerViewModel> allPlayersProfiles;
        private string allTimeWinningsFromJson;
        private string allTimeWinningsHandsFromJson;
        private Dictionary<string, double> tempBets;
        private Visibility allButtonsVisibility;
        private Visibility runMode;
        private bool cardsShow;
        private bool canExecuteBot;
        private HandState tempAllInSituationOnTable;

        #endregion
        #region Constructor
        public MainWindowViewModel()
        {
            try
            {
                InitializeVariables();
                HoldemOmaha.CheckingAndSettingHandId(ref handIdPath, ref handCount);
                HoldemOmaha.AddingCasinosToCollection(ref enumCasinosCollection);
                HoldemOmaha.SettingSelectedEnumCasino(ref selectedEnumCasino);
                HoldemOmaha.SettingIsGeneratedChecked(ref isGeneratedChecked);
                HoldemOmaha.SettingUndoClickedRunPotInitializedAndCursor(ref isUndoClicked, ref potDealt, ref userClickedRun, ref isInitialized, ref cursor);
                SettingWidthMainRight();
                HoldemOmaha.SettingEnumGridShow(ref enumGridShow);
                CollapsingVisibilityInitial();
                HoldemOmaha.SettingCanExecuteBetRaiseIfBetSize0(ref betSizePlayer, ref canExecuteBet, ref canExecuteRaise);
                HoldemOmaha.CollapsingVisibilityHumanRaiseCheckFold(ref isVisibleFoldBtn, ref isVisibleCallBtn, ref isVisibleRaiseBtn, ref isVisibleCheckBtn, ref isVisibleBetBtn, ref humanDecision);
                CanExecuteRunNextTrue();
                SettingPauseResumeTxtToRun();
                SettingShowingRangesShow();
                SettingNumberShowHands();
                SettingGameFromSettings();
                HoldemOmaha.CollapsingOmahaVisibility(ref omahaVisibility);
                HoldemOmaha.SettingChooseGame(ref chooseGame);
                ConsoleWrittingRoutedEvents();
                SettingPlayerCountInitial();
                InitializingPlayersView();
                HoldemOmaha.CollapsingIsVisible(ref isVisible);
                SettingInitialVisibilityCardsOnTable();
                InitializingDealedCardsAndImagesCards();
                SettingCardDeck();
                SttingIsDealerToFalse();
                #region string cardPlayerX, cardXPlayerX,  cardXPlayerX
                SettingAllPlayersProfiles();
                #endregion
                SettingHandCountAndId();
                SettingTxtHideShow();
                SettingBBIfZero();
                SettingNumberOfPlayers();
                CanExecuteHideShowToFalse();
                SettingSmallBlind();
                SettingWaitDecisionDelayThresholdRebuy();
                SettingWidthHoldemAndOmahaGrid();
                SettingGameChoosen();
                CheckingNumShowHandsChoosen();
                SettingNumShowHandsChoosen();
                SettingWidthOfColumns();
                SettingHHPath();
                SettingDictFileHandID();
                Properties.Settings.Default.Save();
                SettingHandStateBeforeLastDecision();
                bool loadRules = gameType == EnumGameType.Omaha;
                RunNextToFalse();
                CheckIfIsInitOk();
                RunNextToTrue();
                SettingsStrategy.Default.running = false;
                AllButtonsVisibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in : " + nameof(MainWindowViewModel) + " " + ex.ToString());
                MessageBox.Show("Exception in : " + nameof(MainWindowViewModel) + " " + ex.ToString());
            }
        }
        #endregion
        #region Properties
        public bool CanExecuteBotProp
        {
            get
            {
                return canExecuteBot;
            }
            set
            {
                canExecuteBot = value;
                OnPropertyChanged(nameof(CanExecuteBotProp));
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public bool CanTestBoardsProp
        {
            get
            {
                return canTestBoards;
            }
            set
            {
                canTestBoards = value;
                OnPropertyChanged(nameof(CanTestBoardsProp));
            }
        }
        public Visibility RunMode
        {
            get
            {
                return runMode;
            }
            set
            {
                runMode = value;
                if (runMode == Visibility.Collapsed)
                {
                    BotButtonVisible = Visibility.Collapsed;
                }
                else
                {
                    BotButtonVisible = Visibility.Visible;
                }
                OnPropertyChanged(nameof(RunMode));
            }
        }
        public Visibility AllButtonsVisibility
        {
            get
            {
                return allButtonsVisibility;
            }
            set
            {
                allButtonsVisibility = value;
                OnPropertyChanged(nameof(AllButtonsVisibility));
            }
        }
        public Visibility VisibilityShowRangesShow
        {
            get
            {
                return visibilityShowRangesShow;
            }
            set
            {
                visibilityShowRangesShow = value;
                OnPropertyChanged(nameof(VisibilityShowRangesShow));
            }
        }
        public List<DecisionMaker> DecisionMakers
        {
            get
            {
                return decisionMakers;
            }
            set
            {
                decisionMakers = value;
                OnPropertyChanged(nameof(DecisionMakers));
            }
        }
        public Visibility BotButtonVisible
        {
            get
            {
                return botButtonVisible;
            }
            set
            {
                botButtonVisible = value;
                OnPropertyChanged(nameof(BotButtonVisible));
            }
        }
        public string BotCheckCall
        {
            get
            {
                return botCheckCall;
            }
            set
            {
                botCheckCall = value;
                OnPropertyChanged(nameof(BotCheckCall));
            }
        }
        public string BotBetRaise
        {
            get
            {
                return botBetRaise;
            }
            set
            {
                botBetRaise = value;
                OnPropertyChanged(nameof(BotBetRaise));
            }
        }
        public ImageSource PauseResumeImg
        {
            get
            {
                return pauseResumeImg;
            }
            set
            {
                pauseResumeImg = value;
                OnPropertyChanged(nameof(PauseResumeImg));
            }
        }
        public bool CanExecuteNext
        {
            get
            {
                return canExecuteNext;
            }
            set
            {
                canExecuteNext = value;
                if (PlayersToPlay != null)
                {
                    int indexNext = GetIndexForNextPlayerBotButtons(indexAct);
                    if (value && PlayersToPlay[indexNext].IsBot)
                    {
                        CanExecuteBotProp = true;
                    }
                    else if (PlayersToPlay[indexAct].IsBot)
                    {
                        CanExecuteBotProp = true;
                    }
                }
                OnPropertyChanged(nameof(CanExecuteNext));
            }
        }
        public EnumCasino SelectedEnumCasino
        {
            get
            {
                return selectedEnumCasino;
            }
            set
            {
                selectedEnumCasino = value;
                OnPropertyChanged(nameof(SelectedEnumCasino));
            }
        }
        public ObservableCollection<EnumCasino> EnumCasinosCollection
        {
            get
            {
                return enumCasinosCollection;
            }
            set
            {
                enumCasinosCollection = value;
                OnPropertyChanged(nameof(EnumCasinosCollection));
            }
        }
        public bool IsGeneratedChecked
        {
            get
            {
                return isGeneratedChecked;
            }
            set
            {
                isGeneratedChecked = value;
                OnPropertyChanged(nameof(IsGeneratedChecked));
            }
        }
        private List<DecisionState> AllStates
        {
            get
            {
                return allStates;
            }
            set
            {
                allStates = value;
                OnPropertyChanged(nameof(AllStates));
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
        public int WidthMainRight
        {
            get
            {
                return widthMainRight;
            }
            set
            {
                widthMainRight = value;
                OnPropertyChanged(nameof(WidthMainRight));
            }
        }
        public bool IsVisiblePlayer
        {
            get
            {
                return isVisiblePlayer;
            }
            set
            {
                isVisiblePlayer = value;
                OnPropertyChanged(nameof(IsVisiblePlayer));
            }
        }
        public ObservableCollection<PokerUtil.EnumOmahaDraw> DrawBetRaise
        {
            get { return drawBetRaise; }
            set
            {
                drawBetRaise = value;
                OnPropertyChanged(nameof(DrawBetRaise));
            }
        }
        public ObservableCollection<PokerUtil.EnumOmahaDraw> DrawCheckCall
        {
            get { return drawCheckCall; }
            set
            {
                drawCheckCall = value;
                OnPropertyChanged(nameof(DrawCheckCall));
            }
        }
        public ObservableCollection<PokerUtil.EnumOmahaDraw> DrawFold
        {
            get { return drawFold; }
            set
            {
                drawFold = value;
                OnPropertyChanged(nameof(DrawFold));
            }
        }
        #region Players Objects
        public PlayerViewModel Player1
        {
            get
            {
                return player1;
            }
            set
            {
                player1 = value;
                OnPropertyChanged(nameof(Player1));
            }
        }
        public PlayerViewModel Player2
        {
            get
            {
                return player2;
            }
            set
            {
                player2 = value;
                OnPropertyChanged(nameof(Player2));
            }
        }
        public PlayerViewModel Player3
        {
            get
            {
                return player3;
            }
            set
            {
                player3 = value;
                OnPropertyChanged(nameof(Player3));
            }
        }
        public PlayerViewModel Player4
        {
            get
            {
                return player4;
            }
            set
            {
                player4 = value;
                OnPropertyChanged(nameof(Player4));
            }
        }
        public PlayerViewModel Player5
        {
            get
            {
                return player5;
            }
            set
            {
                player5 = value;
                OnPropertyChanged(nameof(Player5));
            }
        }
        public PlayerViewModel Player6
        {
            get
            {
                return player6;
            }
            set
            {
                player6 = value;
                OnPropertyChanged(nameof(Player6));
            }
        }
        public PlayerViewModel Player7
        {
            get
            {
                return player7;
            }
            set
            {
                player7 = value;
                OnPropertyChanged(nameof(Player7));
            }
        }
        public PlayerViewModel Player8
        {
            get
            {
                return player8;
            }
            set
            {
                player8 = value;
                OnPropertyChanged(nameof(Player8));
            }
        }
        public PlayerViewModel Player9
        {
            get
            {
                return player9;
            }
            set
            {
                player9 = value;
                OnPropertyChanged(nameof(Player9));
            }
        }
        #endregion
        public int WidthHoldemAndOmahaGrid
        {
            get { return widthHoldemAndOmahaGrid; }
            set
            {
                widthHoldemAndOmahaGrid = value;
                OnPropertyChanged(nameof(WidthHoldemAndOmahaGrid));
            }
        }
        public List<string> EnumGridShow
        {
            get { return enumGridShow; }
            set
            {
                enumGridShow = value;
                OnPropertyChanged(nameof(EnumGridShow));
            }
        }
        public string GridShowSelected
        {
            get
            {
                return gridShow;
            }
            set
            {
                gridShow = value;
                if (gridShow != null)
                {
                    if (gridShow == GridShow.BetRaise.ToString())
                    {
                        BetRaiseOmahaVisibility = Visibility.Visible;
                        CheckCallOmahaVisibility = Visibility.Collapsed;
                        FoldOmahaVisibility = Visibility.Collapsed;
                        var omahaRangesBetRaise = lastDecisionMaker.Hero.Strategy.Params.Multi_ranges; // list multirange
                        ClearPostflopOmahaGrids();
                        string getBoard = GetBoard();
                        if (omahaRangesBetRaise.Count != 0)
                        {
                            int getStep = GetStep(RangeMultiDecisionToList(ref omahaRangesBetRaise));
                            if (isGamePaused)
                            {
                                Omaha.ShowPostflopRanges(RangeMultiDecisionToList(ref omahaRangesBetRaise), ref lastDecisionMaker, omahaRanges, omahaShowDowns, omahaDraws, ref getBoard, ref getStep, ref gameType);
                            }
                        }
                        else
                        {
                            int getStep = GetStep(lastDecisionMaker.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange);
                            if (isGamePaused)
                            {
                                Omaha.ShowPostflopRanges(lastDecisionMaker.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange, ref lastDecisionMaker, omahaRanges, omahaShowDowns, omahaDraws, ref getBoard, ref getStep, ref gameType);
                            }
                        }
                    }
                    else if (gridShow == GridShow.CheckCall.ToString())
                    {
                        BetRaiseOmahaVisibility = Visibility.Collapsed;
                        CheckCallOmahaVisibility = Visibility.Visible;
                        FoldOmahaVisibility = Visibility.Collapsed;
                        var omahaRangesCheckCall = lastDecisionMaker.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange; // list string
                        ClearPostflopOmahaGrids();
                        string getBoard = GetBoard();
                        int getStep = GetStep(omahaRangesCheckCall);
                        if (isGamePaused)
                        {
                            Omaha.ShowPostflopRanges(omahaRangesCheckCall, ref lastDecisionMaker, omahaRangesMid, omahaShowDownsCheckCall, omahaDrawsCheckCall, ref getBoard, ref getStep, ref gameType);
                        }
                    }
                    else
                    {
                        BetRaiseOmahaVisibility = Visibility.Collapsed;
                        CheckCallOmahaVisibility = Visibility.Collapsed;
                        FoldOmahaVisibility = Visibility.Visible;
                        var omahaRangesFoldLastDecision = lastDecisionMaker.Hero.Strategy.Params.OmahaRanges.FoldRange;
                        ClearPostflopOmahaGrids();
                        string getBoard = GetBoard();
                        int getStep = GetStep(omahaRangesFoldLastDecision);
                        if (isGamePaused)
                        {
                            Omaha.ShowPostflopRanges(omahaRangesFoldLastDecision, ref lastDecisionMaker, omahaRangesFold, omahaShowDownsFold, omahaDrawsFold, ref getBoard, ref getStep, ref gameType);
                        }
                    }
                }
                OnPropertyChanged(nameof(GridShowSelected));
            }
        }
        public Visibility VisibilityOmahaPostflop
        {
            get
            {
                return visibilityOmahaPostflop;
            }
            set
            {
                visibilityOmahaPostflop = value;
                OnPropertyChanged(nameof(VisibilityOmahaPostflop));
            }
        }
        public Visibility AllBetCoeffVisibility
        {
            get
            {
                return allBetCoeffVisibility;
            }
            set
            {
                allBetCoeffVisibility = value;
                OnPropertyChanged(nameof(AllBetCoeff));
            }
        }
        public ObservableCollection<PlayerViewModel> PlayersToPlay
        {
            get
            {
                return playersToPlay;
            }
            set
            {
                playersToPlay = value;
                OnPropertyChanged(nameof(PlayersToPlay));
            }
        }
        public ObservableCollection<int> PlayerCount
        {
            get
            {
                return playerCount;
            }
            set
            {
                playerCount = value;
                OnPropertyChanged(nameof(PlayerCount));
            }
        }
        public int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                OnPropertyChanged(nameof(PlayerViewModel.IsMyTurn));
                numberOfPlayers = value;
                potSize = 0;
                cardsDeck = Card.Deck();
                Player.ShowPlayers(ref dealedCardsToPlayer, ref cardsOnTable, ref cardsDeck, ref canExecuteHide, HideCardsClick, ref playersToPlay, ref numberOfPlayers, ref player1, ref player2, ref player3, ref player4, ref player5, ref player6, ref player7, ref player8, ref player9, ref gameChoosen, ref imgCardsDealedToPlayer);
                OnPropertyChanged(nameof(PlayersToPlay));
                ImageSourcesCardsOnTable = new();
                Table.DealCardsToTable(ref imageSourceCardOnTable1, ref cardsDeck, ref cardsOnTable, ref cardsOnTable, ref imageSourceCardOnTable1);
                foreach (var item in PlayersToPlay)
                {
                    if (item.IsMyTurn)
                    {
                        item.IsMyTurn = false;
                    }
                }
                GetDealer();
                OnPropertyChanged(nameof(IsDealer));
                OnPropertyChanged(nameof(DealerVisibility));
                if (numberOfPlayers != Properties.Settings.Default.NumberOfPlayers)
                {
                    InitDecisionMakers();
                }
                SetNewHand();
            }
        }
        public bool IsDealer
        {
            get
            {
                return isDealer;
            }
            set
            {
                isDealer = value;
                OnPropertyChanged(nameof(IsDealer));
            }
        }
        public Visibility DealerVisibility
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                OnPropertyChanged(nameof(DealerVisibility));
            }
        }
        public List<string> DealedCardsToPlayer
        {
            get
            {
                return dealedCardsToPlayer;
            }
            set
            {
                dealedCardsToPlayer = value;
            }
        }
        public List<string> CardsDeck
        {
            get
            {
                return cardsDeck;
            }
            set
            {
                cardsDeck = value;
                OnPropertyChanged(nameof(CardsDeck));
            }
        }
        public ObservableCollection<string> CardsOnTable
        {
            get
            {
                return cardsOnTable;
            }
            set
            {
                cardsOnTable = value;
                OnPropertyChanged(nameof(CardsOnTable));
            }
        }
        #region Cards Visibility
        public Visibility Card1Visibility
        {
            get
            {
                return isVisibleCard1OnTable;
            }
            set
            {
                isVisibleCard1OnTable = value;
                OnPropertyChanged(nameof(Card1Visibility));
            }
        }
        public Visibility Card2Visibility
        {
            get
            {
                return isVisibleCard2OnTable;
            }
            set
            {
                isVisibleCard2OnTable = value;
                OnPropertyChanged(nameof(Card2Visibility));
            }
        }
        public Visibility Card3Visibility
        {
            get
            {
                return isVisibleCard3OnTable;
            }
            set
            {
                isVisibleCard3OnTable = value;
                OnPropertyChanged(nameof(Card3Visibility));
            }
        }
        public Visibility Card4Visibility
        {
            get
            {
                return isVisibleCard4OnTable;
            }
            set
            {
                isVisibleCard4OnTable = value;
                OnPropertyChanged(nameof(Card4Visibility));
            }
        }
        public Visibility Card5Visibility
        {
            get
            {
                return isVisibleCard5OnTable;
            }
            set
            {
                isVisibleCard5OnTable = value;
                OnPropertyChanged(nameof(Card5Visibility));
            }
        }
        #endregion
        public double PotSize
        {
            get
            {
                return potSize;
            }
            set
            {
                potSize = value;
                OnPropertyChanged(nameof(PotSize));
            }
        }
        public double BigBlind
        {
            get
            {
                return bigBlind;
            }
            set
            {
                bigBlind = value;
                OnPropertyChanged(nameof(BigBlind));
            }
        }
        public double SmallBlind
        {
            get
            {
                return smallBlind;
            }
            set
            {
                smallBlind = value;
                OnPropertyChanged(nameof(SmallBlind));
            }
        }
        #region Range List Items
        public ObservableCollection<ImageSource> RangeImageSources1
        {
            get
            {
                return rangeImgSources1;
            }
            set
            {
                rangeImgSources1 = value;
                OnPropertyChanged(nameof(RangeImageSources1));
            }
        }
        public ObservableCollection<ImageSource> RangeImageSources2
        {
            get
            {
                return rangeImgSources2;
            }
            set
            {
                rangeImgSources2 = value;
                OnPropertyChanged(nameof(RangeImageSources2));
            }
        }
        public ObservableCollection<ImageSource> RangeImageSources3
        {
            get
            {
                return rangeImgSources3;
            }
            set
            {
                rangeImgSources3 = value;
                OnPropertyChanged(nameof(RangeImageSources3));
            }
        }
        public ObservableCollection<ImageSource> RangeImageSources4
        {
            get
            {
                return rangeImgSources4;
            }
            set
            {
                rangeImgSources4 = value;
                OnPropertyChanged(nameof(RangeImageSources4));
            }
        }
        public ObservableCollection<ImageSource> RangeImageSourcesMiddle1
        {
            get
            {
                return rangeImgSourcesmiddle1;
            }
            set
            {
                rangeImgSourcesmiddle1 = value;
                OnPropertyChanged(nameof(RangeImageSourcesMiddle1));
            }
        }
        public ObservableCollection<ImageSource> RangeImageSourcesMiddle2
        {
            get
            {
                return rangeImgSourcesmiddle2;
            }
            set
            {
                rangeImgSourcesmiddle2 = value;
                OnPropertyChanged(nameof(RangeImageSourcesMiddle2));
            }
        }
        public ObservableCollection<ImageSource> RangeImageSourcesMiddle3
        {
            get
            {
                return rangeImgSourcesmiddle3;
            }
            set
            {
                rangeImgSourcesmiddle3 = value;
                OnPropertyChanged(nameof(RangeImageSourcesMiddle3));
            }
        }
        public ObservableCollection<ImageSource> RangeImageSourcesMiddle4
        {
            get
            {
                return rangeImgSourcesmiddle4;
            }
            set
            {
                rangeImgSourcesmiddle4 = value;
                OnPropertyChanged(nameof(RangeImageSourcesMiddle4));
            }
        }
        #endregion
        #region Image Sources Cards On Table
        public List<ImageSource> ImageSourcesCardsOnTable
        {
            get
            {
                return imageSourceCardOnTable1;
            }
            set
            {
                imageSourceCardOnTable1 = value;
                OnPropertyChanged(nameof(ImageSourcesCardsOnTable));
            }
        }
        public ImageSource CardOnTable1
        {
            get
            {
                return ImageSourcesCardsOnTable[0];
            }
            set
            {
                ImageSourcesCardsOnTable[0] = value;
                OnPropertyChanged(nameof(CardOnTable1));
            }
        }
        public ImageSource CardOnTable2
        {
            get
            {
                return ImageSourcesCardsOnTable[1];
            }
            set
            {
                ImageSourcesCardsOnTable[1] = value;
                OnPropertyChanged(nameof(CardOnTable2));
            }
        }
        public ImageSource CardOnTable3
        {
            get
            {
                return ImageSourcesCardsOnTable[2];
            }
            set
            {
                ImageSourcesCardsOnTable[2] = value;
                OnPropertyChanged(nameof(CardOnTable3));
            }
        }
        public ImageSource CardOnTable4
        {
            get
            {
                return ImageSourcesCardsOnTable[3];
            }
            set
            {
                ImageSourcesCardsOnTable[3] = value;
                OnPropertyChanged(nameof(CardOnTable4));
            }
        }
        public ImageSource CardOnTable5
        {
            get
            {
                return ImageSourcesCardsOnTable[4];
            }
            set
            {
                ImageSourcesCardsOnTable[4] = value;
                OnPropertyChanged(nameof(CardOnTable5));
            }
        }
        #endregion
        public string TextShowHide
        {
            get
            {
                return txtShowHide;
            }
            set
            {
                txtShowHide = value;
                OnPropertyChanged(nameof(TextShowHide));
            }
        }
        public string TextShow
        {
            get
            {
                return txtShow;
            }
            set
            {
                txtShow = value;
                OnPropertyChanged(nameof(TextShow));
            }
        }
        public ObservableCollection<Omaha> OmahaRanges
        {
            get
            {
                return omahaRanges;
            }
            set
            {
                omahaRanges = value;
                OnPropertyChanged(nameof(OmahaRanges));
            }
        }
        public ObservableCollection<Omaha> OmahaShowdowns
        {
            get
            {
                return omahaShowDowns;
            }
            set
            {
                omahaShowDowns = value;
                if (omahaShowDowns.Count > 0)
                {
                    for (int i = 0; i < omahaShowDowns.Count; i++)
                    {
                        if (omahaShowDowns[i].IsChecked)
                        {
                            SelectedShowdowns.Add(omahaShowDowns[i]);
                        }
                        else
                        {
                            SelectedShowdowns.Remove(omahaShowDowns[i]);
                        }
                    }
                }
                OnPropertyChanged(nameof(OmahaShowdowns));
            }
        }
        public ObservableCollection<Omaha> OmahaShowdownsCheckCall
        {
            get
            {
                return omahaShowDownsCheckCall;
            }
            set
            {
                omahaShowDownsCheckCall = value;
                if (omahaShowDownsCheckCall.Count > 0)
                {
                    for (int i = 0; i < omahaShowDownsCheckCall.Count; i++)
                    {
                        if (omahaShowDownsCheckCall[i].IsChecked)
                        {
                            SelectedShowdowns.Add(omahaShowDownsCheckCall[i]);
                        }
                        else
                        {
                            SelectedShowdowns.Remove(omahaShowDownsCheckCall[i]);
                        }
                    }
                }
                OnPropertyChanged(nameof(OmahaShowdownsCheckCall));
            }
        }
        public ObservableCollection<Omaha> OmahaShowdownsFold
        {
            get
            {
                return omahaShowDownsFold;
            }
            set
            {
                omahaShowDownsFold = value;
                OnPropertyChanged(nameof(OmahaShowdownsFold));
            }
        }
        public ObservableCollection<Omaha> OmahaDraws
        {
            get
            {
                return omahaDraws;
            }
            set
            {
                omahaDraws = value;
                OnPropertyChanged(nameof(OmahaDraws));
            }
        }
        public ObservableCollection<Omaha> OmahaDrawsCheckCall
        {
            get
            {
                return omahaDrawsCheckCall;
            }
            set
            {
                omahaDrawsCheckCall = value;
                OnPropertyChanged(nameof(OmahaDrawsCheckCall));
            }
        }
        public ObservableCollection<Omaha> OmahaDrawsFold
        {
            get
            {
                return omahaDrawsFold;
            }
            set
            {
                omahaDrawsFold = value;
                OnPropertyChanged(nameof(OmahaDrawsFold));
            }
        }
        public ObservableCollection<Omaha> OmahaRangesMid
        {
            get
            {
                return omahaRangesMid;
            }
            set
            {
                omahaRangesMid = value;
                OnPropertyChanged(nameof(OmahaRangesMid));
            }
        }
        public ObservableCollection<Omaha> OmahaRangesFold
        {
            get
            {
                return omahaRangesFold;
            }
            set
            {
                omahaRangesFold = value;
                OnPropertyChanged(nameof(OmahaRangesFold));
            }
        }
        public ObservableCollection<Holdem> HoldemRanges
        {
            get
            {
                return holdemRanges;
            }
            set
            {
                holdemRanges = value;
                OnPropertyChanged(nameof(HoldemRanges));
            }
        }
        public ObservableCollection<Holdem> HoldemRangesMid
        {
            get
            {
                return holdemRangesMid;
            }
            set
            {
                holdemRangesMid = value;
                OnPropertyChanged(nameof(HoldemRangesMid));
            }
        }
        public ObservableCollection<Holdem> HoldemRangesFold
        {
            get
            {
                return holdemRangesFold;
            }
            set
            {
                holdemRangesFold = value;
                OnPropertyChanged(nameof(HoldemRangesFold));
            }
        }
        public ObservableCollection<RangePercents> RangeTextBetRaise
        {
            get
            {
                return rangeTextBetRaise;
            }
            set
            {
                rangeTextBetRaise = value;
                OnPropertyChanged(nameof(RangeTextBetRaise));
            }
        }
        public ObservableCollection<RangePercents> RangeTextCallCheck
        {
            get
            {
                return rangeTextCallCheck;
            }
            set
            {
                rangeTextCallCheck = value;
                OnPropertyChanged(nameof(RangeTextCallCheck));
            }
        }
        public ObservableCollection<RangePercents> RangeTextFold
        {
            get
            {
                return rangeTextFold;
            }
            set
            {
                rangeTextFold = value;
                OnPropertyChanged(nameof(RangeTextFold));
            }
        }
        public int CountHandsRaiseBet
        {
            get
            {
                return countHands;
            }
            set
            {
                countHands = value;
                OnPropertyChanged(nameof(CountHandsRaiseBet));
            }
        }
        public string TextPercentHandBet
        {
            get
            {
                return textPercentHandBet;
            }
            set
            {
                textPercentHandBet = value;
                OnPropertyChanged(nameof(TextPercentHandBet));
            }
        }
        public string TextPercentHandCheck
        {
            get
            {
                return textPercentHandCheck;
            }
            set
            {
                textPercentHandCheck = value;
                OnPropertyChanged(nameof(TextPercentHandCheck));
            }
        }
        public string TextPercentHandFold
        {
            get
            {
                return textPercentHandFold;
            }
            set
            {
                textPercentHandFold = value;
                OnPropertyChanged(nameof(TextPercentHandFold));
            }
        }
        public string BtnBetSize1
        {
            get { return btnBetSize1; }
            set
            {
                btnBetSize1 = value;
                OnPropertyChanged(nameof(BtnBetSize1));
            }
        }
        public string BtnBetSize2
        {
            get { return btnBetSize2; }
            set
            {
                btnBetSize2 = value;
                OnPropertyChanged(nameof(BtnBetSize2));
            }
        }
        public string BtnBetSize3
        {
            get { return btnBetSize3; }
            set
            {
                btnBetSize3 = value;
                OnPropertyChanged(nameof(BtnBetSize3));
            }
        }
        public string BtnBetSize4
        {
            get { return btnBetSize4; }
            set
            {
                btnBetSize4 = value;
                OnPropertyChanged(nameof(BtnBetSize4));
            }
        }
        public string BtnBetSize5
        {
            get { return btnBetSize5; }
            set
            {
                btnBetSize5 = value;
                OnPropertyChanged(nameof(BtnBetSize5));
            }
        }
        public double BetPercent
        {
            get { return betPercent; }
            set
            {
                betPercent = value;
                OnPropertyChanged(nameof(BetPercent));
            }
        }
        public double BetSizePlayer
        {
            get { return betSizePlayer; }
            set
            {
                betSizePlayer = value;
                if (betSizePlayer >= BigBlind)
                {
                    canExecuteBet = true;
                    canExecuteRaise = true;
                }
                else
                {
                    canExecuteBet = false;
                    canExecuteRaise = false;
                }
                OnPropertyChanged(nameof(BetSizePlayer));
            }
        }
        public Visibility BetRaiseOmahaVisibility
        {
            get { return betRaiseOmahaVisibility; }
            set
            {
                betRaiseOmahaVisibility = value;
                OnPropertyChanged(nameof(BetRaiseOmahaVisibility));
            }
        }
        public Visibility CheckCallOmahaVisibility
        {
            get { return checkCallOmahaVisibility; }
            set
            {
                checkCallOmahaVisibility = value;
                OnPropertyChanged(nameof(CheckCallOmahaVisibility));
            }
        }
        public Visibility FoldOmahaVisibility
        {
            get { return foldOmahaVisibility; }
            set
            {
                foldOmahaVisibility = value;
                OnPropertyChanged(nameof(FoldOmahaVisibility));
            }
        }
        public Visibility IsVisibleFoldBtn
        {
            get
            {
                return isVisibleFoldBtn;
            }
            set
            {
                isVisibleFoldBtn = value;
                OnPropertyChanged(nameof(IsVisibleFoldBtn));
            }
        }
        public Visibility IsVisibleCallBtn
        {
            get
            {
                return isVisibleCallBtn;
            }
            set
            {
                isVisibleCallBtn = value;
                OnPropertyChanged(nameof(IsVisibleCallBtn));
            }
        }
        public Visibility IsVisibleRaiseBtn
        {
            get
            {
                return isVisibleRaiseBtn;
            }
            set
            {
                isVisibleRaiseBtn = value;
                OnPropertyChanged(nameof(IsVisibleRaiseBtn));
            }
        }
        public Visibility IsVisibleCheckBtn
        {
            get
            {
                return isVisibleCheckBtn;
            }
            set
            {
                isVisibleCheckBtn = value;
                OnPropertyChanged(nameof(IsVisibleCheckBtn));
            }
        }
        public Visibility IsVisibleBetBtn
        {
            get
            {
                return isVisibleBetBtn;
            }
            set
            {
                isVisibleBetBtn = value;
                OnPropertyChanged(nameof(IsVisibleBetBtn));
            }
        }
        public Visibility HumanDecision
        {
            get
            {
                return humanDecision;
            }
            set
            {
                humanDecision = value;
                OnPropertyChanged(nameof(HumanDecision));
            }
        }
        public string BotHuman
        {
            get
            {
                return botHuman;
            }
            set
            {
                botHuman = value;
                OnPropertyChanged(nameof(BotHuman));
            }
        }
        public bool ShowRangesShow
        {
            get
            {
                return showRangesShow;
            }
            set
            {
                showRangesShow = value;
                if (showRangesShow)
                {
                    VisibilityShowRangesShow = Visibility.Visible;
                }
                else
                {
                    VisibilityShowRangesShow = Visibility.Collapsed;
                }
                OnPropertyChanged(nameof(ShowRangesShow));
            }
        }
        public List<string> AllBetCoeff
        {
            get
            {
                return allBetCoeff;
            }
            set
            {
                allBetCoeff = value;
                if (allBetCoeff.Count > 0)
                {
                    AllBetCoeffVisibility = Visibility.Visible;
                }
                else
                {
                    AllBetCoeffVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged(nameof(AllBetCoeffVisibility));
                OnPropertyChanged(nameof(AllBetCoeff));
            }
        }
        public string BetCoeff
        {
            get
            {
                return betCoeff;
            }
            set
            {
                betCoeff = value;
                rangePercTemp = new();
                if (gameType == EnumGameType.CashGame)
                {
                    if (HoldemRanges.Count != 0)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            HoldemRanges.Clear();
                        });
                    }
                    if (isGamePaused)
                    {
                        if (RangeDecisionRaiseBet.Count > 0)
                        {
                            if (isGamePaused)
                            {
                                Holdem.ShowPostflopRanges(RangeMultiDecisionToList(ref rangeDecisionRaiseBet), HoldemRanges, ref rangePercTemp, GetBoard(), phase, GetStep(RangeMultiDecisionToList(ref rangeDecisionRaiseBet)), gameType);
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (OmahaRanges.Count != 0 && OmahaShowdowns.Count != 0 && OmahaDraws.Count != 0)
                        {
                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                OmahaRanges.Clear();
                                OmahaShowdowns.Clear();
                                OmahaDraws.Clear();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Error in BetCoeff: " + ex.ToString());
                    }
                    if (lastDecisionMaker != null)
                    {
                        var omahaRangesBetRaise = lastDecisionMaker.Hero.Strategy.Params.Multi_ranges; // list multirange
                        if (omahaRangesBetRaise.Count != 0)
                        {
                            string getBoard = GetBoard();
                            int getStep = GetStep(RangeMultiDecisionToList(ref omahaRangesBetRaise));
                            if (isGamePaused)
                            {
                                Omaha.ShowPostflopRanges(RangeMultiDecisionToList(ref omahaRangesBetRaise), ref lastDecisionMaker, OmahaRanges, OmahaShowdowns, OmahaDraws, ref getBoard, ref getStep, ref gameType);
                            }
                        }
                    }
                }
                RangeTextBetRaise = rangePercTemp;
                if (BetCoeff == "")
                {
                    AllBetCoeffVisibility = Visibility.Collapsed;
                    OnPropertyChanged(nameof(AllBetCoeffVisibility));
                }
                OnPropertyChanged(nameof(BetCoeff));
            }
        }
        public List<string> NumShowHands
        {
            get
            {
                return numShowHands;
            }
            set
            {
                numShowHands = value;
                OnPropertyChanged(nameof(NumShowHands));
            }
        }
        public string NumShowHandsChoosen
        {
            get
            {
                return numShowHandsChoosen;
            }
            set
            {
                numShowHandsChoosen = value;
                OnPropertyChanged(nameof(NumShowHandsChoosen));
            }
        }
        public List<string> Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
                OnPropertyChanged(nameof(Range));
            }
        }
        public DecisionMaking.Range RangeDecisionCheckCall
        {
            get
            {
                return rangeDecision;
            }
            set
            {
                rangeDecision = value;
                OnPropertyChanged(nameof(RangeDecisionCheckCall));
            }
        }
        public DecisionMaking.Range RangeDecisionFold
        {
            get
            {
                return rangeDecision2;
            }
            set
            {
                rangeDecision2 = value;
                OnPropertyChanged(nameof(RangeDecisionFold));
            }
        }
        public List<MultiRange> RangeDecisionRaiseBet
        {
            get
            {
                return rangeDecisionRaiseBet;
            }
            set
            {
                rangeDecisionRaiseBet = value;
                OnPropertyChanged(nameof(RangeDecisionRaiseBet));
            }
        }
        public Visibility VisibilityOmaha
        {
            get
            {
                return omahaVisibility;
            }
            set
            {
                omahaVisibility = value;
                OnPropertyChanged(nameof(VisibilityOmaha));
            }
        }
        public Visibility VisibilityHoldem
        {
            get
            {
                return holdemVisibility;
            }
            set
            {
                holdemVisibility = value;
                OnPropertyChanged(nameof(VisibilityHoldem));
            }
        }
        public Visibility VisibilityHoldemOnly
        {
            get
            {
                return holdemOnlyVisibility;
            }
            set
            {
                holdemOnlyVisibility = value;
                OnPropertyChanged(nameof(VisibilityHoldemOnly));
            }
        }
        public List<string> ChooseGame
        {
            get
            {
                return chooseGame;
            }
            set
            {
                chooseGame = value;
                OnPropertyChanged(nameof(ChooseGame));
            }
        }
        public string GameChoosen
        {
            get
            {
                return gameChoosen;
            }
            set
            {
                gameChoosen = value;
                if (gameChoosen == GameEnums.NLH.ToString())
                {
                    if (userClickedRun)
                    {
                        InitDecisionMakers();
                    }
                    gameType = EnumGameType.CashGame;
                    VisibilityHoldem = Visibility.Visible;
                    VisibilityOmaha = Visibility.Collapsed;
                    VisibilityHoldemOnly = Visibility.Visible;
                    widthHoldemAndOmahaGrid = 90;
                    widthMainRight = 450;
                    if (isInitialized)
                    {
                        InitDecisionMakers();
                        MakingHHBuiler();
                    }
                }
                else
                {
                    if (userClickedRun)
                    {
                        InitDecisionMakers();
                    }
                    gameType = EnumGameType.Omaha;
                    VisibilityHoldem = Visibility.Collapsed;
                    VisibilityOmaha = Visibility.Visible;
                    VisibilityHoldemOnly = Visibility.Collapsed;
                    widthHoldemAndOmahaGrid = 120;
                    widthMainRight = 800;
                    if (isInitialized)
                    {
                        InitDecisionMakers();
                        MakingHHBuiler();
                    }
                }
                OnPropertyChanged(nameof(VisibilityHoldem));
                OnPropertyChanged(nameof(VisibilityHoldemOnly));
                OnPropertyChanged(nameof(WidthHoldemAndOmahaGrid));
                OnPropertyChanged(nameof(VisibilityOmaha));
                OnPropertyChanged(nameof(gameType));
                OnPropertyChanged(nameof(WidthMainRight));
                potSize = 0;
                cardsDeck = Card.Deck();
                Player.ShowPlayers(ref dealedCardsToPlayer, ref cardsOnTable, ref cardsDeck, ref canExecuteHide, HideCardsClick, ref playersToPlay, ref numberOfPlayers, ref player1, ref player2, ref player3, ref player4, ref player5, ref player6, ref player7, ref player8, ref player9, ref gameChoosen, ref imgCardsDealedToPlayer);
                foreach (var item in PlayersToPlay)
                {
                    if (item.IsMyTurn)
                    {
                        item.IsMyTurn = false;
                    }
                }
                GetDealer();
                OnPropertyChanged(nameof(IsDealer));
                OnPropertyChanged(nameof(DealerVisibility));
                SetNewHand();
                OnPropertyChanged(nameof(GameChoosen));
            }
        }
        public string PauseResumeTxt
        {
            get
            {
                return pauseResume;
            }
            set
            {
                pauseResume = value;
                OnPropertyChanged(nameof(PauseResumeTxt));
            }
        }
        public string PlayerTimeToAct
        {
            get
            {
                return playerTimeToAct;
            }
            set
            {
                playerTimeToAct = value;
                OnPropertyChanged(nameof(PlayerTimeToAct));
            }
        }
        public double WaitDecision
        {
            get
            {
                return waitDecision;
            }
            set
            {
                waitDecision = value;
                OnPropertyChanged(nameof(WaitDecision));
            }
        }
        public double DealingCardsDelay
        {
            get
            {
                return dealingCardsDelay;
            }
            set
            {
                dealingCardsDelay = value;
                OnPropertyChanged(nameof(DealingCardsDelay));
            }
        }
        public double ThresholdRebuy
        {
            get
            {
                return thresholdRebuy;
            }
            set
            {
                thresholdRebuy = value;
                OnPropertyChanged(nameof(ThresholdRebuy));
            }
        }
        public Visibility VisibilityBet5
        {
            get { return visibilityBet5; }
            set
            {
                visibilityBet5 = value;
                OnPropertyChanged(nameof(VisibilityBet5));
            }
        }
        public Visibility VisibilityHoldemOrOmaha
        {
            get
            {
                return visibilityHoldemOrOmaha;
            }
            set
            {
                visibilityHoldemOrOmaha = value;
                OnPropertyChanged(nameof(VisibilityHoldemOrOmaha));
            }
        }
        public List<string> StringsLog
        {
            get
            {
                return stringsLog;
            }
            set
            {
                stringsLog = value;
                OnPropertyChanged(nameof(StringsLog));
            }
        }
        public static ObservableCollection<Omaha> SelectedShowdowns
        {
            get
            {
                return selectedShowdowns;
            }
            set
            {
                selectedShowdowns = value;
            }
        }
        #endregion
        #region Methods
        #region Show and hide Methods
        public void ShowCardsMethod()
        {
            try
            {
                Player.ShowCardsMethod(PlayersToPlay, gameType);
                canExecuteHide = true;
                HideCardsClick.CanExecute(canExecuteHide);
                canExecuteShow = false;
                ShowCardsClick.CanExecute(canExecuteShow);
                cardsShow = true;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Showing Cards in game from Options Menu: " + ex.ToString(), LogLevel.Error);
            }
        }
        public void HideCardsMethod()
        {
            try
            {
                Player.HideCardsMethod(PlayersToPlay, gameType);
                canExecuteHide = false;
                HideCardsClick.CanExecute(canExecuteHide);
                canExecuteShow = true;
                ShowCardsClick.CanExecute(canExecuteShow);
                cardsShow = false;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Hiding Cards in game from Options Menu: " + ex.ToString(), LogLevel.Error);
            }
        }
        #endregion
        #region bools Execute
        bool CanExecuteShow()
        {
            return canExecuteShow;
        }
        bool CanExecuteHide()
        {
            return canExecuteHide;
        }
        bool CanExecute()
        {
            return true;
        }
        bool CanExecuteBot()
        {
            return canExecuteBot;
        }
        bool CanExecuteLoad()
        {
            return canExecuteLoad;
        }
        bool CanExecuteUndo()
        {
            return canExecuteUndo;
        }
        bool CanExecuteRaise()
        {
            return canExecuteRaise;
        }
        bool CanExecuteBet()
        {
            return canExecuteBet;
        }
        bool CanExecuteRun()
        {
            return canExecuteRun;
        }
        bool CanExecuteLoadFromHH()
        {
            return true;
        }
        public bool CanExecuteNextBtn()
        {
            return canExecuteNext;
        }
        #endregion
        #region Methods for setting values in constructor
        public static void CollapsingVisibility(ref Visibility[] visibilityParams)
        {
            for (int i = 0; i < visibilityParams.Length; i++)
            {
                visibilityParams[i] = Visibility.Collapsed;
            }
        }
        private void SettingWinningPathAndDictionary()
        {
            allTimeWinningsPath = @"C:\katarina\winnings.json";
            allTimeWinningsHandsPath = @"C:\katarina\winningsHands.json";
            if (!File.Exists(allTimeWinningsPath))
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    File.WriteAllText(allTimeWinningsPath, "");
                });
            }
            else
            {
                allTimeWinningsFromJson = File.ReadAllText(allTimeWinningsPath);
                dictallTimeBalanceCurrent = JsonConvert.DeserializeObject<Dictionary<string, double>>(allTimeWinningsFromJson);
            }
            if (!File.Exists(allTimeWinningsHandsPath))
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    File.WriteAllText(allTimeWinningsHandsPath, "");
                });
            }
            else
            {
                allTimeWinningsHandsFromJson = File.ReadAllText(allTimeWinningsHandsPath);
                dictAllTimeHandsCurrent = JsonConvert.DeserializeObject<Dictionary<string, int>>(allTimeWinningsHandsFromJson);
            }
        }
        private void InitializeVariables()
        {
            tempAllInSituationOnTable = new();
            PlayersToPlays = new();
            didPlayersPlay = new();
            rangePercTemp = new();
            rangePercTemp2 = new();
            rangePercTemp3 = new();
            HoldemRanges = new();
            BetCoeff = "";
            canExecuteBot = false;
            cardsShow = true;
            runMode = Visibility.Visible;
            allButtonsVisibility = Visibility.Collapsed;
            tempBets = new();
            dictAllTimeHands = new();
            dictAllTimeHandsCurrent = new();
            allPlayersProfiles = new();
            dictallTimeBalance = new();
            dictallTimeBalanceCurrent = new();
            SettingWinningPathAndDictionary();
            dictSessionHands = new();
            canExecuteWinnings = true;
            dictSessionBalance = new();
            dictStartBalance = new();
            sessionHands = 0;
            isNewSession = true;
            winningsView = new();
            visibilityShowRangesShow = Visibility.Collapsed;
            canExecuteLoad = true;
            testBoardsOpen = false;
            testBoards = new();
            decisionMakerBeforeDecision = new();
            stateBeforeDecision = new();
            canTestBoards = false;
            lastMoveBot = false;
            botButtonVisible = Visibility.Collapsed;
            pauseResumeImg = ImageChange.GetImageSourceBotHuman("play.png");
            hsManual = new();
            isNewHand = false;
            pathName = "";
            gridShow = "";
            selectedShowdowns = new();
            range = new();
            allStates = new();
            enumCasinosCollection = new();
            playerTimeToAct = "";
        }
        private void CollapsingVisibilityInitial()
        {
            visibilityOmahaPostflop = Visibility.Collapsed;
            allBetCoeffVisibility = Visibility.Collapsed;
            visibilityHoldemOrOmaha = Visibility.Collapsed;
            betRaiseOmahaVisibility = Visibility.Collapsed;
            checkCallOmahaVisibility = Visibility.Collapsed;
            foldOmahaVisibility = Visibility.Collapsed;
        }
        private void SettingNumberShowHands()
        {
            numShowHands = new List<string>{ "100", "1000", "All" };
        }
        private void SettingGameFromSettings()
        {
            gameFromSettings = Properties.Settings.Default.GameChoosen;
            if (gameFromSettings == "")
            {
                gameFromSettings = "NLH";
                Properties.Settings.Default.GameChoosen = gameFromSettings;
            }
        }
        private void CanExecuteRunNextTrue()
        {
            canExecuteRun = true;
            CanExecuteNext = true;
        }
        private void SettingShowingRangesShow()
        {
            ShowRangesShow = true;
            if (ShowRangesShow)
            {
                visibilityShowRangesShow = Visibility.Visible;
            }
        }
        private void SettingWidthMainRight()
        {
            widthMainRight = 450;
        }
        private void SettingPauseResumeTxtToRun()
        {
            PauseResumeTxt = GamePauseResume.Run.ToString();
        }       
        private void ConsoleWrittingRoutedEvents()
        {
            foreach (var item in EventManager.GetRoutedEvents())
            {
                Console.WriteLine(item.ToString());
            }
        }
        private void FillAllPlayersProfiles()
        {
            allPlayersProfiles.Add(player1);
            allPlayersProfiles.Add(player2);
            allPlayersProfiles.Add(player3);
            allPlayersProfiles.Add(player4);
            allPlayersProfiles.Add(player5);
            allPlayersProfiles.Add(player6);
            allPlayersProfiles.Add(player7);
            allPlayersProfiles.Add(player8);
            allPlayersProfiles.Add(player9);
        }
        private void SettingAllPlayersProfiles()
        {
            if (gameFromSettings == GameEnums.NLH.ToString())
            {
                Player.GettingNLHPlayers(ref imgCardsDealedToPlayer, ref cardsDeck, ref dealedCardsToPlayer, ref player1, ref player2, ref player3, ref player4, ref player5, ref player6, ref player7, ref player8, ref player9, ref isDealer);
                if (allPlayersProfiles.Count == 0)
                {
                    FillAllPlayersProfiles();
                }
            }
            else
            {
                Player.GettingPLO4Players(ref imgCardsDealedToPlayer, ref cardsDeck, ref dealedCardsToPlayer, ref player1, ref player2, ref player3, ref player4, ref player5, ref player6, ref player7, ref player8, ref player9, ref isDealer);
                if (allPlayersProfiles.Count == 0)
                {
                    FillAllPlayersProfiles();
                }
            }
        }
        private void SettingHandCountAndId()
        {
            handCount = int.Parse(File.ReadAllText(handIdPath)) + 1;
            handId = String.Format("{0:D10}", handCount);
        }
        private void SettingPlayerCountInitial()
        {
            playerCount = HoldemOmaha.Numbers();
        }
        private void InitializingPlayersView()
        {
            playerViews = new();
        }
        private void SttingIsDealerToFalse()
        {
            isDealer = false;
        }
        private void InitializingDealedCardsAndImagesCards()
        {
            dealedCardsToPlayer = new();
            playerImageSourceCards = new();
        }
        private void SettingCardDeck()
        {
            cardsDeck = Card.Deck();
        }
        private void SettingSmallBlind()
        {
            SmallBlind = BigBlind / 2;
        }
        private void SettingWidthHoldemAndOmahaGrid()
        {
            widthHoldemAndOmahaGrid = 90;
        }
        private void SettingGameChoosen()
        {
            GameChoosen = Properties.Settings.Default.GameChoosen;
        }
        private void SettingNumShowHandsChoosen()
        {
            NumShowHandsChoosen = Properties.Settings.Default.NumShowHandsChoosen;
        }
        private void SettingHHPath()
        {
            hhPath = StrategyUtil.GetHHPath(selectedEnumCasino, gameType);
        }
        private void SettingDictFileHandID()
        {
            dictFileHandID = new();
        }
        private void SettingHandStateBeforeLastDecision()
        {
            handStateBeforeLastDecision = new();
        }
        private void SettingNumberOfPlayers()
        {
            int numPlayers = Properties.Settings.Default.NumberOfPlayers;
            if (numPlayers < 2 || numPlayers > 9)
            {
                numPlayers = 6;
                Properties.Settings.Default.NumberOfPlayers = numPlayers;
            }
            NumberOfPlayers = numPlayers;
        }
        private void SettingInitialVisibilityCardsOnTable()
        {
            isVisibleCard1OnTable = Visibility.Collapsed;
            isVisibleCard2OnTable = Visibility.Collapsed;
            isVisibleCard3OnTable = Visibility.Collapsed;
            isVisibleCard4OnTable = Visibility.Collapsed;
            isVisibleCard5OnTable = Visibility.Collapsed;
        }
        private void SettingBBIfZero()
        {
            if (Properties.Settings.Default.BigBlind == 0)
            {
                Properties.Settings.Default.BigBlind = 10;
            }
            BigBlind = Properties.Settings.Default.BigBlind;
        }
        private void SettingTxtHideShow()
        {
            txtShowHide = "Hide Cards";
            txtShow = "Show Cards";
        }
        private void SettingWaitDecisionDelayThresholdRebuy()
        {
            waitDecision = Properties.Settings.Default.Delay;
            if (waitDecision == 0)
            {
                waitDecision = 2000;
            }
            dealingCardsDelay = Properties.Settings.Default.Delay;
            if (dealingCardsDelay == 0)
            {
                dealingCardsDelay = 2000;
                Properties.Settings.Default.Delay = (int)dealingCardsDelay;
            }
            ThresholdRebuy = Properties.Settings.Default.thresholdRebuy;
            if (thresholdRebuy == 0)
            {
                thresholdRebuy = 20;
                Properties.Settings.Default.thresholdRebuy = thresholdRebuy;
            }
        }
        private void CanExecuteHideShowToFalse()
        {
            canExecuteHide = false;
            canExecuteShow = false;
        }
        private void CheckingNumShowHandsChoosen()
        {
            if (numShowHandsChoosen == "")
            {
                numShowHandsChoosen = "100";
                Properties.Settings.Default.NumShowHandsChoosen = numShowHandsChoosen;
            }
        }
        private void CheckIfIsInitOk()
        {
            var isInitOk = Task.Run(async () => await StatsLoader.Initialize(@"c:/katarina/"));
            if (isInitOk.Result)
            {
                Singleton.Log("failed load resources", LogLevel.Info);
            }
            else
            {
                Singleton.Log("failed load resources aborting", LogLevel.Info);
                return;
            }
        }
        private void RunNextToFalse()
        {
            canExecuteRun = false;
            CanExecuteNext = false;
        }
        private void RunNextToTrue()
        {
            canExecuteRun = true;
            CanExecuteNext = true;
        }
        private void SettingWidthOfColumns()
        {
            Player1.WidthColumn = 60;
            Player2.WidthColumn = 60;
            Player3.WidthColumn = 30;
            Player4.WidthColumn = 30;
            Player5.WidthColumn = 30;
            Player6.WidthColumn = 30;
            Player7.WidthColumn = 30;
            Player8.WidthColumn = 30;
            Player9.WidthColumn = 30;
        }
        #endregion
        public void OpenSettingsView()
        {
            try
            {
                Settings settings = new();
                settings.DataContext = this;
                settings.Show();
                ShowPlayersWithoutDealing();
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in opening Settings View: " + ex.ToString(), LogLevel.Error);
            }
        }
        /// <summary>
        /// Saving properties in settings
        /// </summary>
        public void SavingData()
        {
            try
            {
                cursor = Cursors.Wait;
                OnPropertyChanged(nameof(CursorMain));
                Properties.Settings.Default.NumberOfPlayers = NumberOfPlayers;
                Properties.Settings.Default.BigBlind = BigBlind;
                Properties.Settings.Default.Player1Name = Player1.Name;
                Properties.Settings.Default.Player1Balance = Player1.Balance + Player1.BetSize;
                Properties.Settings.Default.Player1StrategyProfile = Player1.Strategy;
                Properties.Settings.Default.Player2Name = Player2.Name;
                Properties.Settings.Default.Player2Balance = Player2.Balance + Player2.BetSize;
                Properties.Settings.Default.Player2StrategyProfile = Player2.Strategy;
                Properties.Settings.Default.Player3Name = Player3.Name;
                Properties.Settings.Default.Player3Balance = Player3.Balance + Player3.BetSize;
                Properties.Settings.Default.Player3StrategyProfile = Player3.Strategy;
                Properties.Settings.Default.Player4Name = Player4.Name;
                Properties.Settings.Default.Player4Balance = Player4.Balance + Player4.BetSize;
                Properties.Settings.Default.Player4StrategyProfile = Player4.Strategy;
                Properties.Settings.Default.Player5Name = Player5.Name;
                Properties.Settings.Default.Player5Balance = Player5.Balance + Player5.BetSize;
                Properties.Settings.Default.Player5StrategyProfile = Player5.Strategy;
                Properties.Settings.Default.Player6Name = Player6.Name;
                Properties.Settings.Default.Player6Balance = Player6.Balance + Player6.BetSize;
                Properties.Settings.Default.Player6StrategyProfile = Player6.Strategy;
                Properties.Settings.Default.Player7Name = Player7.Name;
                Properties.Settings.Default.Player7Balance = Player7.Balance + Player7.BetSize;
                Properties.Settings.Default.Player7StrategyProfile = Player7.Strategy;
                Properties.Settings.Default.Player8Name = Player8.Name;
                Properties.Settings.Default.Player8Balance = Player8.Balance + Player8.BetSize;
                Properties.Settings.Default.Player8StrategyProfile = Player8.Strategy;
                Properties.Settings.Default.Player9Name = Player9.Name;
                Properties.Settings.Default.Player9Balance = Player9.Balance + Player9.BetSize;
                Properties.Settings.Default.Player9StrategyProfile = Player9.Strategy;
                Properties.Settings.Default.thresholdRebuy = ThresholdRebuy;
                Properties.Settings.Default.Delay = (int)DealingCardsDelay;
                Properties.Settings.Default.GameChoosen = GameChoosen;
                Properties.Settings.Default.NumShowHandsChoosen = NumShowHandsChoosen;
                Properties.Settings.Default.Player1IsBot = Player1.IsBot;
                Properties.Settings.Default.Player2IsBot = Player2.IsBot;
                Properties.Settings.Default.Player3IsBot = Player3.IsBot;
                Properties.Settings.Default.Player4IsBot = Player4.IsBot;
                Properties.Settings.Default.Player5IsBot = Player5.IsBot;
                Properties.Settings.Default.Player6IsBot = Player6.IsBot;
                Properties.Settings.Default.Player7IsBot = Player7.IsBot;
                Properties.Settings.Default.Player8IsBot = Player8.IsBot;
                Properties.Settings.Default.Player9IsBot = Player9.IsBot;
                Properties.Settings.Default.IsGeneratedChecked = isGeneratedChecked;
                Properties.Settings.Default.SelectedEnumCasino = selectedEnumCasino;
                Properties.Settings.Default.Save();
                if (!isInitialized)
                {
                    InitDecisionMakers();
                }
                cursor = Cursors.Arrow;
                OnPropertyChanged(nameof(CursorMain));
                MessageBox.Show("Saved successfully");
                Singleton.Log("Saved successfully", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in saving datas from settings: " + ex.ToString(), LogLevel.Error);
            }
        }
        public void ShowNextMove()
        {
            try
            {
                if (CanExecuteNext)
                {
                    CanExecuteNext = false;
                    canTestBoards = false;
                    UpdatingProperties();
                    try
                    {
                        Task taskNext = new Task(() => ShowingNextMove(false));
                        taskNext.Start();
                        taskNext.ContinueWith(task =>
                        {
                            if (PlayersToPlay[indexAct].IsBot)
                            {
                                CanExecuteNext = true;
                                if (phase != EnumPhase.Preflop)
                                {
                                    canTestBoards = true;
                                    CanTestBoardsProp = true;
                                    CommandManager.InvalidateRequerySuggested();
                                }
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in Showing Next Move Task: " + ex.ToString(), LogLevel.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Method for ShowNextMove Command: " + ex.ToString(), LogLevel.Error);
            }
        }
        public void ShowingNextMove(bool useSleep)
        {
            if (testBoardsOpen)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    testBoards.Close();
                    testBoardsOpen = false;
                });
            }
            isGamePaused = true;
            PauseResumeTxt = GamePauseResume.Resume.ToString();
            PauseResumeImg = ImageChange.GetImageSourceBotHuman("play.png");
            if (!userClickedRun)
            {
            }
            try
            {
                lock (lockNext)
                {
                    canExecuteRun = false;
                    if (!isInitialized)
                    {
                        InitDecisionMakers();
                        GettingDecision(false);
                    }
                    else if (isInitialized && isNewHand)
                    {
                        GettingDecision(false);
                    }
                    else
                    {
                        if (allStates.Count - 1 == 0)
                        {
                            if (!PlayersToPlay[indexAct].IsBot)
                            {
                                //GettingDecision(false);
                                CheckingEndHandOrPhase(false);
                                GettingDecision(false);
                            }
                            else
                            {
                                GettingDecision(false);
                            }
                        }
                        else if (allStates.Count - 1 >= 0)
                        {
                            canExecuteUndo = true;
                            CheckingEndHandOrPhase(false);
                            if (PlayersToPlay[indexAct].Action == null || PlayersToPlay[indexAct].Action == "" || !PlayersToPlay[indexAct].IsBot)
                            {
                                GettingDecision(false);
                            }
                            else
                            {
                                hs = Gethandstate();
                                GettingDecision(false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in ShowingNextMove, in locked block: " + ex.ToString());
            }
            if (PlayersToPlay[indexAct].IsBot)
            {
                AddState();
            }
            if (isGamePaused)
            {
                canExecuteLoad = true;
            }
            canExecuteRun = true;
        }
        /// <summary>
        /// This method checks if players bet and ButtonsContent depends on it
        /// </summary>
        //public static bool CheckIfRivalBet(ref double sumbets)
        //{
        //    return sumbets == 0 ? false : true;
        //}
        public double GetHighestBet()
        {
            hs = Gethandstate();
            return hs.Bets.Max();
        }
        private void GetButtonsContent()
        {
            double maxBet = GetHighestBet();
            HumanDecisionModel.GetButtonsContent(maxBet, phase, BigBlind, ref btnBetSize1, ref btnBetSize2, ref btnBetSize3, ref btnBetSize4, ref btnBetSize5, ref visibilityBet5);
            OnPropertyChanged(nameof(BtnBetSize1));
            OnPropertyChanged(nameof(BtnBetSize2));
            OnPropertyChanged(nameof(BtnBetSize3));
            OnPropertyChanged(nameof(BtnBetSize4));
            OnPropertyChanged(nameof(BtnBetSize5));
            OnPropertyChanged(nameof(VisibilityBet5));
        }
        public void GetDealer()
        {
            HoldemOmaha.GetDealer(ref r, playersToPlay, ref isDealer, ref isVisible);
            OnPropertyChanged(nameof(IsDealer));
            OnPropertyChanged(nameof(DealerVisibility));
            OnPropertyChanged(nameof(IsDealer));
            OnPropertyChanged(nameof(DealerVisibility));
        }
        /// <summary>
        /// This method makes decision and sets balance, bet, action...
        /// </summary>
        /// <param name="decision">made decision</param>
        /// <param name="playerVM">playerViewModel for player isMyTurn</param>
        /// <param name="playerIndex">player isMyTurn</param>
        public void DecisionMaking(ref Decision decision, ref PlayerViewModel playerVM, ref int playerIndex)
        {
            BotController.DecisionMaking(ref decision, ref playerVM, ref playerIndex, hs, testBoards, ref playersToPlay, ref didPlayersPlay);
        }
        /// <summary>
        /// This method Shows Ranges in Range List And shows Cards from ranges
        /// </summary>
        public void CashGamePreflop(ref Decision decision, ref DecisionMaker decisionMaker)
        {
            try
            {
                if (Range.Count > 0)
                {
                    Range.Clear();
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        RangeImageSources1.Clear();
                        RangeImageSources2.Clear();
                        RangeImageSources3.Clear();
                        RangeImageSources4.Clear();
                        RangeImageSourcesMiddle1.Clear();
                        RangeImageSourcesMiddle2.Clear();
                        RangeImageSourcesMiddle3.Clear();
                        RangeImageSourcesMiddle4.Clear();
                        TextPercentHandBet = "";
                        TextPercentHandCheck = "";
                    });
                }
                VisibilityHoldemOrOmaha = Visibility.Visible;
                if (decision.Action == EnumDecisionType.BET || decision.Action == EnumDecisionType.RAISE)
                {
                    Range = decisionMaker.Hero.Strategy.Params.PreflopRaiseRange;
                    if (isGamePaused)
                    {
                        int step = GetStep(range);
                        Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, Range, decision.Action, isGamePaused);
                    }
                    TextPercentHandBet = GetPercentHands(Range.Count, 1326);
                }
                else if (decision.Action == EnumDecisionType.CALL || decision.Action == EnumDecisionType.CHECK)
                {
                    Range = decisionMaker.Hero.Strategy.Params.PreflopCallRange;
                    if (isGamePaused)
                    {
                        int step = GetStep(range);
                        Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, Range, decision.Action, isGamePaused);
                    }
                    TextPercentHandCheck = GetPercentHands(Range.Count, 1326);
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Method CashGamePreflop: " + ex.ToString(), LogLevel.Error);
            }
        }
        /// <summary>
        /// This method Shows Ranges in Range List And shows Cards from ranges after Undo, it's called in RefreshTable method
        /// </summary>
        public void CashGamePreflopFromLaststate(Decision decision, List<string> rangeLaststate)
        {
            try
            {
                VisibilityHoldemOrOmaha = Visibility.Visible;
                if (decision.Action == EnumDecisionType.BET || decision.Action == EnumDecisionType.RAISE)
                {
                    int step = GetStep(range);
                    Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, rangeLaststate, decision.Action, isGamePaused);
                    TextPercentHandBet = GetPercentHands(rangeLaststate.Count, 1326);
                }
                else if (decision.Action == EnumDecisionType.CALL || decision.Action == EnumDecisionType.CHECK)
                {
                    int step = GetStep(range);
                    Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, rangeLaststate, decision.Action, isGamePaused);
                    TextPercentHandCheck = GetPercentHands(rangeLaststate.Count, 1326);
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Showing PREFLOP Ranges For CashGame From last state: " + ex.ToString(), LogLevel.Error);
            }
        }
        public void HandlingAllBetCoeff()
        {
            HoldemOmaha.HandlingAllBetCoeff(ref allBetCoeff, ref rangeDecisionRaiseBet, ref allBetCoeffVisibility, ref betCoeff);
            OnPropertyChanged(nameof(AllBetCoeffVisibility));
            OnPropertyChanged(nameof(AllBetCoeff));
            OnPropertyChanged(nameof(BetCoeff));
        }
        public void CashGamePostflop(ref DecisionMaker decisionMaker)
        {
            try
            {
                VisibilityHoldemOrOmaha = Visibility.Collapsed;
                RangeDecisionRaiseBet = decisionMaker.Hero.Strategy.Params.Multi_ranges.ToList();
                int indexOfBetCoeff = 0;
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    RangeTextBetRaise.Clear();
                    RangeTextCallCheck.Clear();
                    RangeTextFold.Clear();
                });
                if (RangeDecisionRaiseBet.Count != 0)
                {
                    HandlingAllBetCoeff();
                    for (int i = 0; i < AllBetCoeff.Count; i++)
                    {
                        if (BetCoeff == AllBetCoeff[i])
                        {
                            indexOfBetCoeff = i;
                        }
                    }
                }
                RangeDecisionCheckCall = decisionMaker.Hero.Strategy.Params.Print_range2;
                if (isGamePaused)
                {
                    Holdem.ShowPostflopRanges(DMRangeToList(RangeDecisionCheckCall), HoldemRangesMid, ref rangePercTemp2, GetBoard(), phase, GetStep(DMRangeToList(RangeDecisionCheckCall)), gameType);
                }
                RangeTextCallCheck = rangePercTemp2;
                RangeDecisionFold = decisionMaker.Hero.Strategy.Params.Print_range3;
                if (isGamePaused)
                {
                    Holdem.ShowPostflopRanges(DMRangeToList(RangeDecisionFold), HoldemRangesFold, ref rangePercTemp3, GetBoard(), phase, GetStep(DMRangeToList(RangeDecisionFold)), gameType);
                }
                RangeTextFold = rangePercTemp3;
                double allHands = 0;
                double allHandsBetRaise = 0;
                if (RangeDecisionRaiseBet.Count == 0)
                {
                    allHands = 0 + (double)RangeDecisionCheckCall.Combos.Count + (double)RangeDecisionFold.Combos.Count;
                }
                else
                {
                    foreach (var item in RangeDecisionRaiseBet)
                    {
                        allHandsBetRaise += item.Range.Count;
                    }
                    allHands = allHandsBetRaise + (double)RangeDecisionCheckCall.Combos.Count + (double)RangeDecisionFold.Combos.Count;
                }
                SettingPercentText(ref allHands, indexOfBetCoeff, ref allHandsBetRaise);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in showing POSTFLOP ranges for CashGame: " + ex.ToString(), LogLevel.Error);
            }
        }
        private void SettingPercentText(ref double allHands, int indexOfBetCoeff, ref double allHandsBetRaise)
        {
            HoldemOmaha.SettingPercentText(ref allHands, ref indexOfBetCoeff, ref allHandsBetRaise, ref textPercentHandBet, ref textPercentHandCheck, ref textPercentHandFold, ref rangeDecision, ref rangeDecision2, ref rangeDecisionRaiseBet);
            OnPropertyChanged(nameof(TextPercentHandBet));
            OnPropertyChanged(nameof(TextPercentHandCheck));
            OnPropertyChanged(nameof(TextPercentHandFold));
        }
        private void CheckIfRaiseBetIsNotZero(ref int indexOfBetCoeff)
        {
            if (RangeDecisionRaiseBet.Count != 0)
            {
                HandlingAllBetCoeff();
                for (int i = 0; i < AllBetCoeff.Count; i++)
                {
                    if (BetCoeff == AllBetCoeff[i])
                    {
                        indexOfBetCoeff = i;
                    }
                }
            }
        }
        public void CashGamePostflopFromLaststate(DecisionMaker decisionMaker, DecisionState rangeLaststate)
        {
            try
            {
                VisibilityHoldemOrOmaha = Visibility.Collapsed;
                RangeDecisionRaiseBet = rangeLaststate.MultiRanges;
                int indexOfBetCoeff = 0;
                CheckIfRaiseBetIsNotZero(ref indexOfBetCoeff);
                RangeDecisionCheckCall = rangeLaststate.RangeCheckCall;
                if (isGamePaused)
                {
                    Holdem.ShowPostflopRanges(DMRangeToList(RangeDecisionCheckCall), HoldemRangesMid, ref rangePercTemp2, GetBoard(), phase, GetStep(DMRangeToList(RangeDecisionCheckCall)), gameType);
                }
                RangeTextCallCheck = rangePercTemp2;
                RangeDecisionFold = rangeLaststate.RangeFold;
                if (isGamePaused)
                {
                    Holdem.ShowPostflopRanges(DMRangeToList(RangeDecisionFold), HoldemRangesFold, ref rangePercTemp3, GetBoard(), phase, GetStep(DMRangeToList(RangeDecisionFold)), gameType);
                }
                RangeTextFold = rangePercTemp3;
                if (gameType == EnumGameType.CashGame)
                {
                    foreach (var item in PlayersToPlay)
                    {
                        item.ImageSourceCard3 = null;
                        item.ImageSourceCard4 = null;
                    }
                }
                SettingTxtBetCheckFold(ref rangeLaststate);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in showing POSTFLOP ranges for Cash Game from LastState : " + ex.ToString(), LogLevel.Error);
            }
        }
        private void SettingTxtBetCheckFold(ref DecisionState rangeLaststate)
        {
            TextPercentHandBet = rangeLaststate.TextPercentHandBet;
            TextPercentHandCheck = rangeLaststate.TextPercentHandCheck;
            TextPercentHandFold = rangeLaststate.TextPercentHandFold;
        }
        private void OmahaGridsVisibilityPreflop()
        {
            VisibilityHoldemOrOmaha = Visibility.Visible;
            VisibilityOmaha = Visibility.Visible;
            VisibilityHoldem = Visibility.Visible;
            VisibilityHoldemOnly = Visibility.Collapsed;
        }
        public void OmahaGamePreflop(ref DecisionMaker decisionMaker, ref Decision decision)
        {
            try
            {
                OmahaGridsVisibilityPreflop();
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    RangeImageSources1.Clear();
                    RangeImageSources2.Clear();
                    RangeImageSources3.Clear();
                    RangeImageSources4.Clear();
                    RangeImageSourcesMiddle1.Clear();
                    RangeImageSourcesMiddle2.Clear();
                    RangeImageSourcesMiddle3.Clear();
                    RangeImageSourcesMiddle4.Clear();
                    TextPercentHandBet = "";
                    TextPercentHandCheck = "";
                });
                if (decision.Action == EnumDecisionType.BET || decision.Action == EnumDecisionType.RAISE)
                {
                    Range = decisionMaker.Hero.Strategy.Params.PreflopRaiseRange;
                    if (isGamePaused)
                    {
                        int step = GetStep(range);
                        Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, Range, decision.Action, isGamePaused);
                    }
                    TextPercentHandBet = GetPercentHands(Range.Count, 270725);
                }
                else if (decision.Action == EnumDecisionType.CALL || decision.Action == EnumDecisionType.CHECK)
                {
                    Range = decisionMaker.Hero.Strategy.Params.PreflopCallRange;
                    if (isGamePaused)
                    {
                        int step = GetStep(range);
                        Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, Range, decision.Action, isGamePaused);
                    }
                    TextPercentHandCheck = GetPercentHands(Range.Count, 270725);
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Showing PREFLOP ranges for Omaha: " + ex.ToString(), LogLevel.Error);
            }
        }
        public void OmahaGamePreflopFromLaststate(Decision decision, DecisionState lastDecisionState)
        {
            try
            {
                OmahaGridsVisibilityPreflop();
                if (decision.Action == EnumDecisionType.BET || decision.Action == EnumDecisionType.RAISE)
                {
                    int step = GetStep(range);
                    Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, lastDecisionState.RangeBetRaise, decision.Action, isGamePaused);
                    TextPercentHandBet = lastDecisionState.TextPercentHandBet;
                }
                else if (decision.Action == EnumDecisionType.CALL || decision.Action == EnumDecisionType.CHECK)
                {
                    int step = GetStep(range);
                    Holdem.ShowPreflopRange(step, gameType, RangeImageSources1, RangeImageSources2, RangeImageSources3, RangeImageSources4, RangeImageSourcesMiddle1, RangeImageSourcesMiddle2, RangeImageSourcesMiddle3, RangeImageSourcesMiddle4, lastDecisionState.RangeBetRaise, decision.Action, isGamePaused);
                    TextPercentHandCheck = lastDecisionState.TextPercentHandCheck;
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in showing PREFLOP ranges for Omaha from LastState : " + ex.ToString(), LogLevel.Error);
            }
        }
        public void OmahaGamePostFlop(ref DecisionMaker decisionMaker)
        {
            try
            {
                OmahaPostflopVisibilities();
                RangeDecisionRaiseBet = decisionMaker.Hero.Strategy.Params.Multi_ranges; // list multirange
                if (RangeDecisionRaiseBet.Count != 0)
                {
                    HandlingAllBetCoeff();
                    RangeMultiDecisionToList(ref rangeDecisionRaiseBet);
                }
                else
                {
                    HandlingAllBetCoeff();
                }
                var omahaRangesCheckCall = decisionMaker.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange; // list string
                var omahaRangesFoldLastDecision = decisionMaker.Hero.Strategy.Params.OmahaRanges.FoldRange; // list string
                if (!canRun)
                {
                    if (gridShow == "")
                    {
                        GridShowSelected = GridShow.BetRaise.ToString();
                    }
                    if (gridShow == GridShow.BetRaise.ToString())
                    {
                        AllBetCoeffVisibility = Visibility.Visible;
                        BetRaiseOmahaVisibility = Visibility.Visible;
                        CheckCallOmahaVisibility = Visibility.Collapsed;
                        FoldOmahaVisibility = Visibility.Collapsed;
                        ClearPostflopOmahaGrids();
                        string getBoard = GetBoard();
                        if (rangeDecisionRaiseBet.Count != 0)
                        {
                            int getStep = GetStep(RangeMultiDecisionToList(ref rangeDecisionRaiseBet));
                            if (isGamePaused)
                            {
                                Omaha.ShowPostflopRanges(RangeMultiDecisionToList(ref rangeDecisionRaiseBet), ref decisionMaker, omahaRanges, omahaShowDowns, omahaDraws, ref getBoard, ref getStep, ref gameType);
                            }
                        }
                        else
                        {
                            int getStep = GetStep(decisionMaker.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange);
                            if (isGamePaused)
                            {
                                Omaha.ShowPostflopRanges(decisionMaker.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange, ref decisionMaker, omahaRanges, omahaShowDowns, omahaDraws, ref getBoard, ref getStep, ref gameType);
                            }
                        }
                    }
                    else if (gridShow == GridShow.CheckCall.ToString())
                    {
                        AllBetCoeffVisibility = Visibility.Visible;
                        BetRaiseOmahaVisibility = Visibility.Collapsed;
                        CheckCallOmahaVisibility = Visibility.Visible;
                        FoldOmahaVisibility = Visibility.Collapsed;
                        ClearPostflopOmahaGrids();
                        string getBoard = GetBoard();
                        int getStep = GetStep(omahaRangesCheckCall);
                        if (isGamePaused)
                        {
                            Omaha.ShowPostflopRanges(omahaRangesCheckCall, ref decisionMaker, omahaRangesMid, omahaShowDownsCheckCall, omahaDrawsCheckCall, ref getBoard, ref getStep, ref gameType);
                        }
                    }
                    else
                    {
                        AllBetCoeffVisibility = Visibility.Visible;
                        BetRaiseOmahaVisibility = Visibility.Collapsed;
                        CheckCallOmahaVisibility = Visibility.Collapsed;
                        FoldOmahaVisibility = Visibility.Visible;
                        ClearPostflopOmahaGrids();
                        string getBoard = GetBoard();
                        int getStep = GetStep(omahaRangesFoldLastDecision);
                        if (isGamePaused)
                        {
                            Omaha.ShowPostflopRanges(omahaRangesFoldLastDecision, ref decisionMaker, omahaRangesFold, omahaShowDownsFold, omahaDrawsFold, ref getBoard, ref getStep, ref gameType);
                        }
                    }
                }
                else
                {
                    VisibilityOmahaPostflop = Visibility.Collapsed;
                    OnPropertyChanged(nameof(VisibilityOmahaPostflop));
                }
                double allHands = 0;
                SettingTxtPercents(ref decisionMaker, ref allHands, omahaRangesCheckCall, omahaRangesFoldLastDecision);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in showing POSTFLOP ranges for OmahA: " + ex.ToString(), LogLevel.Error);
            }
        }
        private void SettingTxtPercents(ref DecisionMaker decisionMaker, ref double allHands, List<string> omahaRangesCheckCall, List<string> omahaRangesFoldLastDecision)
        {
            if (rangeDecisionRaiseBet.Count != 0)
            {
                if (CountHandsRaiseBet != 0)
                {
                    allHands = (double)CountHandsRaiseBet + (double)omahaRangesCheckCall.Count + (double)omahaRangesFoldLastDecision.Count;
                }
                else
                {
                    for (int i = 0; i < decisionMaker.Hero.Strategy.Params.Multi_ranges.Count; i++)
                    {
                        allHands += decisionMaker.Hero.Strategy.Params.Multi_ranges[i].Range.Count;
                    }
                    allHands += (double)omahaRangesCheckCall.Count + (double)omahaRangesFoldLastDecision.Count;
                }
            }
            else
            {
                allHands = (double)decisionMaker.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange.Count + (double)omahaRangesCheckCall.Count + (double)omahaRangesFoldLastDecision.Count;
            }
            if (allHands == 0)
            {
                TextPercentHandBet = "Bet / Raise: 0%";
                TextPercentHandCheck = "Check/Call: 0%";
                TextPercentHandFold = "Fold: 0%";
            }
            else
            {
                TextPercentHandBet = "Bet/Raise: " + GetPercentHands((double)decisionMaker.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange.Count, allHands);
                TextPercentHandCheck = "Check/Call: " + GetPercentHands((double)omahaRangesCheckCall.Count, allHands);
                TextPercentHandFold = "Fold: " + GetPercentHands((double)omahaRangesFoldLastDecision.Count, allHands);
            }
        }
        private void OmahaPostflopVisibilities()
        {
            VisibilityHoldemOnly = Visibility.Collapsed;
            VisibilityHoldemOrOmaha = Visibility.Collapsed;
            visibilityOmahaPostflop = Visibility.Visible;
            OnPropertyChanged(nameof(VisibilityOmahaPostflop));
            VisibilityOmaha = Visibility.Visible;
            VisibilityHoldem = Visibility.Collapsed;
        }
        public void OmahaGamePostFlopFromLaststate(DecisionState lastDecisionState)
        {
            try
            {
                OmahaPostflopVisibilities();
                RangeDecisionRaiseBet = lastDecisionState.MultiRanges; // list multirange
                if (RangeDecisionRaiseBet != null)
                {
                    HandlingAllBetCoeff();
                    RangeMultiDecisionToList(ref rangeDecisionRaiseBet);
                }
                var omahaRangesCheckCall = lastDecisionState.OmahaRangesCheckCall; // list string
                var omahaRangesFoldLastDecision = lastDecisionState.OmahaRangesFold; // list string
                string getBoard = GetBoard();
                if (!canRun)
                {
                    if (gridShow == GridShow.BetRaise.ToString())
                    {
                        BetRaiseOmahaVisibility = Visibility.Visible;
                        CheckCallOmahaVisibility = Visibility.Collapsed;
                        FoldOmahaVisibility = Visibility.Collapsed;
                        ClearPostflopOmahaGrids();
                        if (rangeDecisionRaiseBet != null)
                        {
                            int getStep = GetStep(RangeMultiDecisionToList(ref rangeDecisionRaiseBet));
                            if (isGamePaused)
                            {
                                Omaha.ShowPostflopRangesFromLaststate(RangeMultiDecisionToList(ref rangeDecisionRaiseBet), lastDecisionState.DecisionMaker, omahaRanges, omahaShowDowns, omahaDraws, getBoard, getStep, gameType);
                            }
                        }
                        else
                        {
                            int getStep = GetStep(lastDecisionState.OmahaRangesBetRaiseRange);
                            if (isGamePaused)
                            {
                                Omaha.ShowPostflopRangesFromLaststate(lastDecisionState.OmahaRangesBetRaiseRange, lastDecisionState.DecisionMaker, omahaRanges, omahaShowDowns, omahaDraws, getBoard, getStep, gameType);
                            }
                        }
                    }
                    else if (gridShow == GridShow.CheckCall.ToString())
                    {
                        BetRaiseOmahaVisibility = Visibility.Collapsed;
                        CheckCallOmahaVisibility = Visibility.Visible;
                        FoldOmahaVisibility = Visibility.Collapsed;
                        ClearPostflopOmahaGrids();
                        int getStep = GetStep(omahaRangesCheckCall);
                        if (isGamePaused)
                        {
                            Omaha.ShowPostflopRangesFromLaststate(lastDecisionState.OmahaRangesCheckCall, lastDecisionState.DecisionMaker, omahaRangesMid, omahaShowDownsCheckCall, omahaDrawsCheckCall, getBoard, getStep, gameType);
                        }
                    }
                    else
                    {
                        BetRaiseOmahaVisibility = Visibility.Collapsed;
                        CheckCallOmahaVisibility = Visibility.Collapsed;
                        FoldOmahaVisibility = Visibility.Visible;
                        ClearPostflopOmahaGrids();
                        int getStep = GetStep(omahaRangesFoldLastDecision);
                        if (isGamePaused)
                        {
                            Omaha.ShowPostflopRangesFromLaststate(lastDecisionState.OmahaRangesFold, lastDecisionState.DecisionMaker, omahaRangesFold, omahaShowDownsFold, omahaDrawsFold, getBoard, getStep, gameType);
                        }
                    }
                }
                else
                {
                    VisibilityOmahaPostflop = Visibility.Collapsed;
                    OnPropertyChanged(nameof(VisibilityOmahaPostflop));
                }
                SettingTxtBetCheckFold(ref lastDecisionState);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in showing POSTFLOP ranges for Omaha from lastDecisionState: " + ex.ToString(), LogLevel.Error);
            }
        }
        private List<string> CreatingListOfSelectedPlayerCards(DecisionMaker actualDecisionMaker)
        {
            return HoldemOmaha.CreatingListOfSelectedPlayerCards(ref actualDecisionMaker, ref playersToPlay, ref gameType);
        }
        private void ChangingPublicCardsIfSame(ref HandState handState)
        {
            HoldemOmaha.ChangingPublicCardsIfSame(ref handState, ref phase, ref cardsOnTable, ref cardsDeck, CardOnTable4, CardOnTable5);
        }
        public static DecisionMaker GetDeepCopy(DecisionMaker dm)
        {
            return StrategyUtil.DeepCopy<DecisionMaker>(dm);
        }
        private Decision GetDecision(DecisionMaker decisionMaker, HandState handState, PlayerViewModel playerVM, int playerIndex)
        {
            try
            {
                StringsLog?.Clear();
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in StringsLog. SringsLog is null: " + ex.ToString());
            }
            decisionMaker.Hero.Strategy.Params.statusMsgs.Clear();
            CountHandsRaiseBet = 0;
            decisionMakerBeforeDecision = GetDeepCopy(decisionMaker);
            handStateBeforeLastDecision = StrategyUtil.DeepCopy<HandState>(handState);
            if (phase != EnumPhase.Preflop)
            {
                var cardsPlayerFromTable = CreatingListOfSelectedPlayerCards(decisionMaker);
                string cardsFromTbl = "";
                foreach (var item in CardsOnTable)
                {
                    cardsFromTbl += item;
                }
                if (TestBoardsViewModel.IsPlayerHandSameAsBoard(cardsPlayerFromTable, CardsOnTable))
                {
                    ChangeHandCards(decisionMaker, CardsOnTable, gameType, PlayersToPlay, indexAct, decisionMakers);
                    handState = Gethandstate();
                }
            }
            ChangingPublicCardsIfSame(ref handState);
            HandState hsRotated = GetRotatedHs(handState, playerIndex);
            Decision decision = decisionMaker.MakeDecision(hsRotated, true, null);
            lastDecisionMaker = GetDeepCopy(decisionMaker);
            if (lastDecisionMaker == null)
            {
                MessageBox.Show("Sending NULL decision Maker");
            }
            lastDecision = StrategyUtil.DeepCopy(decision);
            DecisionMaking(ref decision, ref playerVM, ref playerIndex);
            if (gameType == EnumGameType.CashGame)
            {
                if (phase == EnumPhase.Preflop)
                {
                    CashGamePreflop(ref decision, ref decisionMaker);
                }
                else
                {
                    CashGamePostflop(ref decisionMaker);
                }
            }
            else
            {
                if (phase == EnumPhase.Preflop)
                {
                    OmahaGamePreflop(ref decisionMaker, ref decision);
                }
                else
                {
                    OmahaGamePostFlop(ref decisionMaker);
                }
            }
            StringsLog = decisionMaker.Hero.Strategy.Params.statusMsgs;
            OnPropertyChanged(nameof(StringsLog));
            lastMoveBot = true;
            return decision;
        }
        /// <summary>
        ///  decision maker range to list
        /// </summary>
        /// <param name="rangeDecision">grange for convert to list</param>
        /// <returns></returns>
        private List<string> DMRangeToList(DecisionMaking.Range rangeDecision)
        {
            return HoldemOmaha.DMRangeToList(rangeDecision, ref gameType);
        }
        public static double PercentWithOneDecimal(double number)
        {
            number = number * 100;
            number = Math.Round(number, 1);
            return number;
        }
        private List<string> RangeMultiDecisionToList(ref List<MultiRange> multiRanges)
        {
            return HoldemOmaha.RangeMultiDecisionToList(ref multiRanges, ref countHands, ref allBetCoeff, ref betCoeff);
        }
        private int GetStep(List<string> ranges)
        {
            return HoldemOmaha.GetStep(ref ranges, ref numShowHandsChoosen);
        }
        private string GetBoard()
        {
            return HoldemOmaha.GetBoard(ref phase, ref cardsOnTable);
        }
        public static string GetPercentHands(double handscount, double totalHands)
        {
            string percentOfHand = Math.Round((handscount / totalHands) * 100, 1).ToString() + "%";
            return percentOfHand;
        }
        public static string GetPercentHands(int handscount, double totalHands)
        {
            string percentOfHand = Math.Round((handscount / totalHands) * 100, 1).ToString() + "%";
            return percentOfHand;
        }
        public static double GetPercHands(int handscount, double totalHands)
        {
            double percentOfHand = Math.Round((handscount / totalHands) * 100, 1);
            return percentOfHand;
        }
        private void PauseGame()
        {
            try
            {
                var tempImgPause = ImageChange.GetImageSourceBotHuman("pause.png");
                if (pauseResume == GamePauseResume.Pause.ToString())
                {
                    if (phase != EnumPhase.Preflop)
                    {
                        canTestBoards = true;
                        CanTestBoardsProp = true;
                        CommandManager.InvalidateRequerySuggested();
                    }
                    pauseResumeImg = ImageChange.GetImageSourceBotHuman("play.png");
                    canRun = false;
                    RunMode = Visibility.Visible;
                    PauseResumeTxt = GamePauseResume.Resume.ToString();
                    CanExecuteNext = true;
                    isGamePaused = true;
                    if (AllStates.Count > 1)
                    {
                        canExecuteUndo = true;
                    }
                    canExecuteLoad = true;
                }
                else if (pauseResume == GamePauseResume.Resume.ToString())
                {
                    canTestBoards = false;
                    pauseResumeImg = ImageChange.GetImageSourceBotHuman("pause.png");
                    PauseResumeTxt = GamePauseResume.Pause.ToString();
                    RunMode = Visibility.Collapsed;
                    canRun = true;
                    Task taskGame = new Task(() => ResumeGameTask(), TaskCreationOptions.LongRunning);
                    taskGame.Start();
                    CanExecuteNext = false;
                    isGamePaused = false;
                    canExecuteUndo = false;
                    canExecuteLoad = false;
                }
                OnPropertyChanged(nameof(PauseResumeTxt));
                OnPropertyChanged(nameof(PauseResumeImg));
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Pausing game: " + ex.ToString(), LogLevel.Error);
            }
        }
        private bool IsPhaseFinished()
        {
            hs = Gethandstate();
            bool phaseFinished = EndHandOrPhase.IsPhaseFinished(hs, phaseOver, didPlayersPlay, false, sleepTimeForMove);
            return phaseFinished;
        }
        private bool IsHandFinished(HandState hs, bool useSleep)
        {
            bool isFinished = false;
            try
            {
                hs = Gethandstate();
                int numInGame = hs.InGame.Count(o => o == true);
                if (numInGame == 1)
                {
                    for (int i = 0; i < PlayersToPlay.Count; i++)
                    {
                        PlayersToPlay[i].BetSize = 0;
                        PlayersToPlay[i].IsMyTurn = false;
                    }
                    if (useSleep)
                    {
                        Thread.Sleep(1000);
                    }
                    if (!potDealt)
                    {
                        foreach (var item in hs.Bets)
                        {
                            PotSize += item;
                        }
                        for (int i = 0; i < PlayersToPlay.Count; i++)
                        {
                            if (hs.InGame[i])
                            {
                                PotSizeToWinners(ComparingWinnersAndGetListWinners(), useSleep);
                            }
                        }
                    }
                    if (useSleep)
                    {
                        Thread.Sleep(sleepTimeForMove);
                    }
                    isFinished = true;
                    return isFinished;
                }
                else if (phase == EnumPhase.River && IsPhaseFinished())
                {
                    List<string> winners = ComparingWinnersAndGetListWinners();
                    for (int i = 0; i < PlayersToPlay.Count; i++)
                    {
                        PlayersToPlay[i].IsMyTurn = false;
                        PlayersToPlay[i].BetSize = 0;
                    }
                    if (!potDealt)
                    {
                        foreach (var item in hs.Bets)
                        {
                            PotSize += item;
                        }
                        PotSizeToWinners(winners, useSleep);
                    }
                    isFinished = true;
                    return isFinished;
                }
                else
                {
                    isFinished = false;
                    return isFinished;
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in checking if Hand is finished: " + ex.ToString(), LogLevel.Error);
            }
            return isFinished;
        }
        private void CheckIfBalanceSmallerThanBet()
        {
            HoldemOmaha.CheckIfBalanceSmallerThanBet(ref tempAllInSituationOnTable, ref playersToPlay, ref potSize);
        }      
        public List<string> ComparingWinnersAndGetListWinners()
        {
            allCards = AllCardsForCompare();
            winningHands = new();
            string cardsOnTable = "";
            try
            {
                foreach (var item in CardsOnTable)
                {
                    cardsOnTable += item;
                }
                string winner = allCards[0];
                winningHands.Add(winner);
                if (gameType == EnumGameType.CashGame)
                {
                    EndHandOrPhase.CashGameCompareHands(ref winner, ref cardsOnTable, ref allCards, ref winningHands);
                }
                else
                {
                    if (allCards.Count == 1)
                    {
                        return winningHands;
                    }
                    else
                    {
                        EndHandOrPhase.OmahaCompareHands2Winners(ref cardsOnTable, ref playersToPlay, ref allCards, ref winningHands);
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Comparing winners and getting list of winners: " + ex.ToString(), LogLevel.Error);
            }
            return winningHands;
        }       
        private void PotSizeToWinners(List<string> winnersList, bool useSleep)
        {
            try
            {
                CheckIfBalanceSmallerThanBet();
                EndHandOrPhase.PotSizeToWinners(winnersList, PlayersToPlay, PotSize, gameType);
                if (useSleep)
                {
                    Thread.Sleep(1000);
                }
                potDealt = true;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in dealing pot to winners: " + ex.ToString(), LogLevel.Error);
            }
        }
        // call this function only once in constructor
        private void InitDecisionMakers()
        {
            try
            {
                hhPath = StrategyUtil.GetHHPath(selectedEnumCasino, gameType);
                cursor = Cursors.Wait;
                OnPropertyChanged(nameof(CursorMain));
                EnumCasino casino = selectedEnumCasino;
                // this function loads downloads all strategy files from the server to the local PC that bot needs to play poker, it might take 2 hours if running first time
                // after 1st time it should be within 1-2 minutes.
                EnumHandLevel level = StrategyUtil.GetLevelForBBSize(BigBlind);
                double straddle = 0;
                double ante = 0;
                decisionMakers = new();
                for (int i = 0; i < playersToPlay.Count; i++)
                {
                    DecisionMaker decisionMaker = new DecisionMaker(playersToPlay[i].Name, playersToPlay[i].Name, selectedEnumCasino, level, gameType, straddle, ante, false);
                    decisionMakers.Add(decisionMaker);
                }
                cursor = Cursors.Arrow;
                OnPropertyChanged(nameof(CursorMain));
                isInitialized = true;
                MakingHHBuiler();
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in InitDecision Makers: " + ex.ToString());
                MessageBox.Show("Problem in InitDecision Makers, creating decision makers failed, or setting necessary data failed.");
            }
        }
        private void MakingHHBuiler()
        {
            try
            {
                if (isGeneratedChecked)
                {
                    hhBuilder = new HHBuilder(EnumHandLevel.NL2, selectedEnumCasino, EnumTableType.SlowTable, "table" + DateTime.Now, decisionMakers[1].Hero.Name, bigBlind, bigBlind / 2, numberOfPlayers, gameType);
                    for (int i = 0; i < PlayersToPlay.Count; i++)
                    {
                        if (PlayersToPlay[i].Name == decisionMakers[1].Hero.Name)
                        {
                            indexForHH = i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Making Hand History Builder: " + ex.ToString(), LogLevel.Error);
            }
        }
        public static List<string> ConcludeUsingEquity(string player1, string player2, string hand1, string hand2, string board)
        {
            try
            {
                long[] wins = new long[2];
                long[] loses = new long[2];
                long[] ties = new long[2];
                long totalHands = 0;
                EquityEstimation.HandOddsOmaha(new string[2] { hand1, hand2 }, board, "", wins, loses, ties, ref totalHands);
                if (wins[0] == 1)
                {
                    return new List<string>() { player1 };
                }
                else if (wins[1] == 1)
                {
                    return new List<string>() { player2 };
                }
                else
                {
                    return new List<string>() { player1, player2 };
                }
            }
            catch (Exception)
            {
                Singleton.Log("ex in ConcludeUsingEquity", LogLevel.Info);
                return new List<string>() { player1, player2 };
            }
        }
        private HandState GetRotatedHs(HandState hs, int playerIndex)
        {
            var hsCopy = StrategyUtil.DeepCopy<HandState>(hs);
            hsCopy = DecisionModel.GetRotatedHs(NumberOfPlayers, playerIndex, hsCopy, playersToPlay, phase, didPlayersPlay);
            return hsCopy;
        }
        private bool IsAllIn()
        {
            hs = Gethandstate();
            if (EndHandOrPhase.AllIn(hs, didPlayersPlay))
            {
                tempAllInSituationOnTable = StrategyUtil.DeepCopy<HandState>(hs);
                foreach (var item in PlayersToPlay)
                {
                    item.Action = "";
                    if (item.IsMyTurn)
                    {
                        item.IsMyTurn = false;
                    }
                }
                if (phase == EnumPhase.Preflop)
                {
                    ShowFlop();
                    Thread.Sleep(1000);
                    ShowTurn();
                    Thread.Sleep(1000);
                    ShowRiver();
                    Thread.Sleep(1000);
                }
                if (phase == EnumPhase.Flop)
                {
                    ShowTurn();
                    Thread.Sleep(1000);
                    ShowRiver();
                    Thread.Sleep(1000);
                }
                if (phase == EnumPhase.Turn)
                {
                    ShowRiver();
                    Thread.Sleep(1000);
                }
                PotSizeToWinners(ComparingWinnersAndGetListWinners(), true);
                SettingIsMyTurnDidPlayersPlayed();
                SetNewHand();
                isAllIn = true;
                foreach (var item in allPlayersProfiles)
                {
                    item.SessionHands++;
                }
                sessionHands++;
                return true;
            }
            return false;
        }
        private void SettingIsMyTurnDidPlayersPlayed()
        {
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                PlayersToPlay[i].IsMyTurn = false;
            }
            for (int i = 0; i < didPlayersPlay.Count; i++)
            {
                didPlayersPlay[i] = false;
            }
        }
        private bool CheckIfHandFinished()
        {
            if (IsHandFinished(hs, false))
            {
                foreach (var item in PlayersToPlay)
                {
                    item.Action = "";
                }
                for (int i = 0; i < didPlayersPlay.Count; i++)
                {
                    didPlayersPlay[i] = false;
                }
                if (potDealt)
                {
                    sessionHands++;
                    foreach (var item in allPlayersProfiles)
                    {
                        item.SessionHands++;
                    }
                }
                SetNewHand();
                isHandStart = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ChangePhasePreflopToFlop(bool useSleep)
        {
            if (isGamePaused)
            {
                canTestBoards = true;
                CanTestBoardsProp = true;
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                canTestBoards = false;
                CanTestBoardsProp = false;
            }
            phase = EnumPhase.Flop;
            if (useSleep)
            {
                Thread.Sleep(1000);
            }
            ShowFlop();
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].IsMyTurn)
                {
                    PlayersToPlay[i].IsMyTurn = false;
                }
            }
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].IsDealer)
                {
                    indexAct = i;
                }
            }
            for (int i = 0; i < didPlayersPlay.Count; i++)
            {
                didPlayersPlay[i] = false;
            }
        }
        private void ChangePhaseFlopToTurn(bool useSleep)
        {
            phase = EnumPhase.Turn;
            if (useSleep)
            {
                Thread.Sleep(1000);
            }
            ShowTurn();
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].IsMyTurn)
                {
                    PlayersToPlay[i].IsMyTurn = false;
                }
            }
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].IsDealer)
                {
                    indexAct = i;
                }
            }
            for (int i = 0; i < didPlayersPlay.Count; i++)
            {
                didPlayersPlay[i] = false;
            }
        }
        private void ChangeTurnToRiver(bool useSleep)
        {
            phase = EnumPhase.River;
            if (useSleep)
            {
                Thread.Sleep(1000);
            }
            ShowRiver();
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].IsMyTurn)
                {
                    PlayersToPlay[i].IsMyTurn = false;
                }
            }
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].IsDealer)
                {
                    indexAct = i;
                }
            }
            for (int i = 0; i < didPlayersPlay.Count; i++)
            {
                didPlayersPlay[i] = false;
            }
        }
        private void ProcessHandFinished(bool useSleep)
        {
            if (IsPhaseFinished())
            {
                foreach (var item in PlayersToPlay)
                {
                    item.Action = "";
                }
                sumbets = 0;
                if (phase == EnumPhase.Preflop)
                {
                    ChangePhasePreflopToFlop(useSleep);
                }
                else if (phase == EnumPhase.Flop)
                {
                    ChangePhaseFlopToTurn(useSleep);
                }
                else if (phase == EnumPhase.Turn)
                {
                    ChangeTurnToRiver(useSleep);
                }
            }
        }
        private void RunGame()
        {
            Task taskGame = new Task(() => RunGameTask(), TaskCreationOptions.LongRunning);
            taskGame.Start();
        }
        private void StartGame()
        {
            try
            {
                if (pauseResume == GamePauseResume.Resume.ToString() || pauseResume == GamePauseResume.Pause.ToString())
                {
                    PauseGame();
                    Singleton.Log("Game is paused", LogLevel.Info);
                }
                else
                {
                    userClickedRun = true;
                    if (pauseResume == GamePauseResume.Run.ToString())
                    {
                        pauseResume = GamePauseResume.Pause.ToString();
                        pauseResumeImg = ImageChange.GetImageSourceBotHuman("pause.png");
                        OnPropertyChanged(nameof(PauseResumeImg));
                        OnPropertyChanged(nameof(PauseResumeTxt));
                    }
                    isGamePaused = false;
                    if (!isInitialized)
                    {
                        InitDecisionMakers();
                    }
                    RunMode = Visibility.Collapsed;
                    RunGame();
                    Singleton.Log("Game has started", LogLevel.Info);
                    canTestBoards = false;
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in starting game: " + ex.ToString());
            }
        }
        private List<string> AllCardsForCompare()
        {
            hs = Gethandstate();
            allCards = new();
            for (int i = 0; i < hs.InGame.Count; i++)
            {
                if (hs.InGame[i])
                {
                    allCards.Add(PlayersToPlay[i].Cards);
                }
            }
            return allCards;
        }
        #region HumanDecision Methods
        private void GoAllIn()
        {
            HumanDecisionModel.GoAllIn(ref playersToPlay, ref indexAct, ref sumbets, ref humanPlayed, gameType, hs, PotSize);
            FinishDecisionHuman();
        }
        private void CheckIfInitCalled()
        {
            if (!isInitialized)
            {
                InitDecisionMakers();
            }
        }
        private double LimitBetSize(double balance)
        {
            double betSize = balance;
            return betSize;
        }
        private void Betting()
        {
            if (BetSizePlayer > PlayersToPlay[indexAct].Balance)
            {
                BetSizePlayer = LimitBetSize(PlayersToPlay[indexAct].Balance);
            }
            if (BetSizePlayer != 0)
            {
                PlayersToPlay[indexAct].BetSize = BetSizePlayer;
                PlayersToPlay[indexAct].Balance -= BetSizePlayer;
                PlayersToPlay[indexAct].Action = EnumDecisionType.BET.ToString();
                canExecuteBet = false;
                canExecuteRaise = false;
                FinishDecisionHuman();
            }
        }
        private void Folding()
        {
            PlayersToPlay[indexAct].InGame = false;
            PlayersToPlay[indexAct].Action = EnumDecisionType.FOLD.ToString();
            FinishDecisionHuman();
        }
        private void Calling()
        {
            double maxBet = GetHighestBet();
            double toCall = maxBet - PlayersToPlay[indexAct].BetSize;
            if (maxBet > PlayersToPlay[indexAct].Balance)
            {
                double betSize = PlayersToPlay[indexAct].BetSize;
                PlayersToPlay[indexAct].BetSize = PlayersToPlay[indexAct].Balance + PlayersToPlay[indexAct].BetSize;
                PlayersToPlay[indexAct].Balance -= PlayersToPlay[indexAct].Balance;
            }
            else
            {
                PlayersToPlay[indexAct].BetSize = maxBet;
                PlayersToPlay[indexAct].Balance -= toCall;
            }
            PlayersToPlay[indexAct].Action = EnumDecisionType.CALL.ToString();
            FinishDecisionHuman();
        }
        private void Raising()
        {
            if (BetSizePlayer > PlayersToPlay[indexAct].Balance)
            {
                BetSizePlayer = LimitBetSize(PlayersToPlay[indexAct].Balance);
            }
            PlayersToPlay[indexAct].BetSize = BetSizePlayer;
            PlayersToPlay[indexAct].Balance -= BetSizePlayer;
            PlayersToPlay[indexAct].Action = EnumDecisionType.RAISE.ToString();
            canExecuteBet = false;
            canExecuteRaise = false;
            FinishDecisionHuman();
        }
        private void Checking()
        {
            double maxBet = GetHighestBet();
            PlayersToPlay[indexAct].BetSize = maxBet;
            PlayersToPlay[indexAct].Balance -= maxBet;
            if (maxBet > PlayersToPlay[indexAct].Balance)
            {
                PlayersToPlay[indexAct].BetSize = PlayersToPlay[indexAct].Balance;
                PlayersToPlay[indexAct].Balance -= PlayersToPlay[indexAct].BetSize;
            }
            PlayersToPlay[indexAct].Action = EnumDecisionType.CHECK.ToString();
            FinishDecisionHuman();
        }
        #endregion
        #region ShowFlop, ShowRiver, ShowTurn
        public void ShowFlop()
        {
            foreach (PlayerViewModel item in PlayersToPlay)
            {
                PotSize += item.BetSize;
                item.BetSize = 0;
            }
            Card1Visibility = Visibility.Visible;
            Card2Visibility = Visibility.Visible;
            Card3Visibility = Visibility.Visible;
            Card4Visibility = Visibility.Collapsed;
            Card5Visibility = Visibility.Collapsed;
        }
        public void ShowTurn()
        {
            foreach (PlayerViewModel item in PlayersToPlay)
            {
                PotSize += item.BetSize;
                item.BetSize = 0;
            }
            Card1Visibility = Visibility.Visible;
            Card2Visibility = Visibility.Visible;
            Card3Visibility = Visibility.Visible;
            Card4Visibility = Visibility.Visible;
            Card5Visibility = Visibility.Collapsed;
        }
        public void ShowRiver()
        {
            foreach (PlayerViewModel item in PlayersToPlay)
            {
                PotSize += item.BetSize;
                item.BetSize = 0;
            }
            Card1Visibility = Visibility.Visible;
            Card2Visibility = Visibility.Visible;
            Card3Visibility = Visibility.Visible;
            Card4Visibility = Visibility.Visible;
            Card5Visibility = Visibility.Visible;
        }
        #endregion 
        public void ShowPlayersWithoutDealing()
        {
            try
            {
                Player.ShowPlayersWithoutDealing(ref canExecuteHide, HideCardsClick, ref playersToPlay, ref numberOfPlayers, ref player1, ref player2, ref player3, ref player4, ref player5, ref player6, ref player7, ref player8, ref player9);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in showing players without dealing: " + ex.ToString());
            }
        }
        public void SaveHand(HHBuilder hhbuilder, EnumGameType gametype)
        {
            try
            {
                if (potDealt)
                {
                    hhBuilder.ReplaceNames();
                    hhBuilder.FinalizeActions();
                    WriteHHToFile.Write(hhBuilder.DBHand, hhBuilder.tableType, hhBuilder.tableName, hhBuilder.path, hhBuilder.bbSize, hhBuilder.bbSize / 2, gameType);
                    Singleton.Log("Successfully written HH Builder.");
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Making HH Builder Exception in: " + ex.ToString());
            }
        }
        public void DefaultValuesNewHand()
        {
            isNewHand = true;
            potDealt = false;
            if (isGeneratedChecked)
            {
                if (!isUndoClicked)
                {
                    if (hhBuilder != null)
                    {
                        UploadStatistics.InsertHandsWhilePlayingLocally(hhPath, ref dictFileHandID, selectedEnumCasino, EnumHandLevel.NL2, 6, gameType);
                        SaveHand(hhBuilder, gameType);
                    }
                }
            }
            foreach (var item in PlayersToPlay)
            {
                item.Action = null;
            }
            AllBetCoeffVisibility = Visibility.Collapsed;
            AllBetCoeff = new();
            hsManual = new();
            allStates = new();
            canExecuteUndo = false;
            UpdatingProperties();
        }
        //public void SettingHsManualNewHand()
        //{
        //    int numOfPlayersCopy = NumberOfPlayers;
        //    hsManual.NumberOfPlayers = numOfPlayersCopy;
        //    foreach (var item in playersToPlay)
        //    {
        //        hsManual.Actions.Add(item.Action);
        //        hsManual.IsMyTurn.Add(item.IsMyTurn);
        //        hsManual.IsBot.Add(item.IsBot);
        //        var cardsOfPlayerCopy = item.Cards;
        //        hsManual.CardsOfPlayer.Add(cardsOfPlayerCopy);
        //        var dealerIdCopy = item.IsDealer;
        //        hsManual.IsDealer.Add(dealerIdCopy);
        //    }
        //    var cardsOnTableCopy = CardsOnTable.ToList();
        //    hsManual.PublicCards = cardsOnTableCopy.ToList();
        //    for (int i = 0; i < didPlayersPlay.Count; i++)
        //    {
        //        hsManual.DidPlayersPlayed.Add(didPlayersPlay[i]);
        //    }
        //}
        private void BackToDefaultValuesDecisionMaker()
        {
            if (decisionMakers != null)
            {
                if (decisionMakers.Count != PlayersToPlay.Count)
                {
                    foreach (var decisionMaker in decisionMakers)
                    {
                        if (decisionMaker.Hero.Strategy.Params.Hand != null)
                        {
                            decisionMaker.Hero.Strategy.Params.Hand = null;
                            for (int i = 0; i < decisionMaker.hs.Cards.Count; i++)
                            {
                                decisionMaker.hs.Cards[i] = "";
                            }
                        }
                    }
                }
            }
        }
        private void SettingsAndContentForNewHand(ref ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            if (!PlayersToPlay[indexAct].IsBot)
            {
                GetButtonsContent();
                SettingButtonsVisibility();
                HumanDecision = Visibility.Visible;
                ShowingButtonsForBot();
                CanExecuteNext = false;
            }
            else
            {
                HumanDecision = Visibility.Collapsed;
                ShowingButtonsForBot();
                CanExecuteNext = true;
            }
        }
        private void SettingDistStartBalance(ref Dictionary<string, double> dictStartBalance)
        {
            if (sessionHands == 0)
            {
                if (dictStartBalance.Count == 0)
                {
                    foreach (var item in allPlayersProfiles)
                    {
                        double balanceAndBet = item.Balance + item.BetSize;
                        dictStartBalance.Add(item.Name, balanceAndBet);
                    }
                }
                else
                {
                    foreach (var item in allPlayersProfiles)
                    {
                        double balanceAndBet = item.Balance + item.BetSize;
                        dictStartBalance[item.Name] = balanceAndBet;
                    }
                }
            }
        }
        private void SettingDictsSessionAndAllTime(ref ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            foreach (var item in PlayersToPlay)
            {
                double balanceAndBet = item.Balance + item.BetSize;
                dictSessionBalance[item.Name] = balanceAndBet;
                dictSessionHands[item.Name] = item.SessionHands;
                if (dictallTimeBalance.ContainsKey(item.Name))
                {
                    try
                    {
                        dictallTimeBalance[item.Name] = dictallTimeBalanceCurrent[item.Name] + balanceAndBet - dictStartBalance[item.Name];
                        dictAllTimeHands[item.Name] = dictAllTimeHandsCurrent[item.Name] + item.SessionHands;
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in dict balance all time or current balance: " + ex.ToString());
                    }
                }
                else
                {
                    if (!dictStartBalance.ContainsKey(item.Name))
                    {
                        dictStartBalance.Add(item.Name, item.Balance);
                    }
                    dictallTimeBalance.Add(item.Name, balanceAndBet - dictStartBalance[item.Name]);
                    dictAllTimeHands.Add(item.Name, item.SessionHands);
                }
            }
            allTimeWinningsFromJson = JsonConvert.SerializeObject(dictallTimeBalance);
            allTimeWinningsHandsFromJson = JsonConvert.SerializeObject(dictAllTimeHands);
            File.WriteAllText(allTimeWinningsPath, allTimeWinningsFromJson);
            File.WriteAllText(allTimeWinningsHandsPath, allTimeWinningsHandsFromJson);
        }
        public void SetNewHand()
        {
            HoldemOmaha.CheckIfTestingBoardsOpen(testBoardsOpen, testBoards);
            tempAllInSituationOnTable = new();
            tempBets = new();
            try
            {
                HoldemOmaha.CheckAndSetAllPreflopStats(ref selectedEnumCasino, ref gameType);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Singleton stats:" + ex.ToString(), LogLevel.Error);
            }
            DefaultValuesNewHand();
            BackToDefaultValuesDecisionMaker();
            cardsDeck = Card.Deck();
            SetNewHandFromTable();
            handId = String.Format("{0:D10}", handCount);
            canTestBoards = false;
            File.WriteAllText(handIdPath, handCount.ToString());           
            HoldemOmaha.SettingHsManualNewHand(ref numberOfPlayers, ref hsManual, ref playersToPlay, ref cardsOnTable, ref didPlayersPlay);
            lastDecisionMaker = null;
            var hsCopy = Gethandstate();
            allStates.Add(DecisionState.MakeDecisionStateFirst(hsCopy, null, indexAct, null, null, hsManual, phase, range, "", "", "", null, ""));
            SettingsAndContentForNewHand(ref playersToPlay);
            PropertyChangingSetNewHand();
            CheckingAndSettingDictsWinnings();
            if (!cardsShow)
            {
                HideCardsMethod();
            }
            SettingIsNewSession(ref isNewSession);
        }
        private void SetNewHandFromTable()
        {
            Table.SetNewHand(ref betRaiseOmahaVisibility, ref checkCallOmahaVisibility, ref foldOmahaVisibility, ref phaseOver, ref isHandStart, ref sumbets, ref thresholdRebuy, ref bigBlind, ref playersToPlay, ref isVisibleCard1OnTable, ref isVisibleCard2OnTable, ref isVisibleCard3OnTable, ref isVisibleCard4OnTable, ref isVisibleCard5OnTable, ref potSize, ref didPlayersPlay, ref phase, ref imageSourceCardOnTable1, ref dealedCardsToPlayer, ref cardsOnTable, ref cardsDeck, ref gameChoosen, ref imgCardsDealedToPlayer, ref imageSourceCardOnTable1, ref cardsOnTable, ref handCount, ref indexAct, isNewSession);
        }
        private void CheckingAndSettingDictsWinnings()
        {
            if (isNewSession)
            {
                SetDictsIfNewSession(ref allPlayersProfiles);
                SettingAllTimeWinnings(allTimeWinningsFromJson);
            }
            else
            {
                SettingDistStartBalance(ref dictStartBalance);
                SettingDictsSessionAndAllTime(ref playersToPlay);
            }
        }
        private void SettingAllTimeWinnings(string allTimeWinningsFromJson)
        {
            if (allTimeWinningsFromJson == null)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    allTimeWinningsFromJson = JsonConvert.SerializeObject(dictallTimeBalanceCurrent);
                    allTimeWinningsHandsFromJson = JsonConvert.SerializeObject(dictAllTimeHandsCurrent);
                });
                File.WriteAllText(allTimeWinningsPath, allTimeWinningsFromJson);
                File.WriteAllText(allTimeWinningsHandsPath, allTimeWinningsHandsFromJson);
            }
        }
        private void SetDictsIfNewSession(ref List<PlayerViewModel> allPlayersProfiles)
        {
            foreach (var item in allPlayersProfiles)
            {
                dictSessionBalance.Add(item.Name, 0);
                dictSessionHands.Add(item.Name, 0);
                if (allTimeWinningsFromJson == null)
                {
                    dictallTimeBalanceCurrent.Add(item.Name, 0);
                    dictallTimeBalance.Add(item.Name, 0);
                }
                else
                {
                    dictallTimeBalance.Add(item.Name, 0);
                }
                if (allTimeWinningsHandsFromJson == null)
                {
                    dictAllTimeHandsCurrent.Add(item.Name, 0);
                    dictAllTimeHands.Add(item.Name, 0);
                }
                else
                {
                    dictAllTimeHands.Add(item.Name, 0);
                }
            }
        }
        private void SettingIsNewSession(ref bool isNewSession)
        {
            if (isNewSession)
            {
                isNewSession = false;
            }
        }
        private void PropertyChangingSetNewHand()
        {
            OnPropertyChanged(nameof(HumanDecision));
            OnPropertyChanged(nameof(BetRaiseOmahaVisibility));
            OnPropertyChanged(nameof(CheckCallOmahaVisibility));
            OnPropertyChanged(nameof(FoldOmahaVisibility));
            OnPropertyChanged(nameof(PotSize));
            OnPropertyChanged(nameof(Card1Visibility));
            OnPropertyChanged(nameof(Card2Visibility));
            OnPropertyChanged(nameof(Card3Visibility));
            OnPropertyChanged(nameof(Card4Visibility));
            OnPropertyChanged(nameof(Card5Visibility));
            OnPropertyChanged(nameof(CardsOnTable));
            OnPropertyChanged(nameof(CardOnTable1));
            OnPropertyChanged(nameof(CardOnTable2));
            OnPropertyChanged(nameof(CardOnTable3));
            OnPropertyChanged(nameof(CardOnTable4));
            OnPropertyChanged(nameof(CardOnTable5));
        }
        private HandState Gethandstate()
        {
            try
            {
                hs = BotController.GetHandState(ref numberOfPlayers, ref indexAct, ref playersToPlay, ref phase, ref cardsOnTable, ref gameType, ref potSize, ref handId);
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in GetHandState: " + " " + ex.ToString());
            }
            return hs;
        }
        private int GetNextIndexPlayerToAct()
        {
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[indexAct].IsMyTurn == true)
                {
                    PlayersToPlay[indexAct].IsMyTurn = false;
                }
            }
            indexAct = HoldemOmaha.GetindexPlayerToAct(ref indexAct, ref playersToPlay);
            return indexAct;
        }
        private int GetIndexForNextPlayerBotButtons(int indexForAct)
        {
            return HoldemOmaha.GetIndexForNextPlayerBotButtons(ref indexForAct, ref playersToPlay);
        }
        private void UpdateHHBuilder()
        {
            hs = Gethandstate();
            var hsCopy = StrategyUtil.DeepCopy<HandState>(hs);
            if (PlayersToPlay[indexForHH].Name == hs.Names[0])
            {
                hhBuilder.UpdateHand(hsCopy);
            }
            else
            {
                var rotatedHs = GetRotatedHs(hsCopy, indexForHH);
                var rotatedHsCopy = StrategyUtil.DeepCopy<HandState>(rotatedHs);
                if (gameType == EnumGameType.CashGame)
                {
                    rotatedHsCopy.Cards[0] = PlayersToPlay[indexForHH].Cards.Substring(0, 2);
                    rotatedHsCopy.Cards[1] = PlayersToPlay[indexForHH].Cards.Substring(2, 2);
                }
                else
                {
                    rotatedHsCopy.OmahaCards[0] = PlayersToPlay[indexForHH].Cards.Substring(0, 2);
                    rotatedHsCopy.OmahaCards[1] = PlayersToPlay[indexForHH].Cards.Substring(2, 2);
                    rotatedHsCopy.OmahaCards[2] = PlayersToPlay[indexForHH].Cards.Substring(4, 2);
                    rotatedHsCopy.OmahaCards[3] = PlayersToPlay[indexForHH].Cards.Substring(6, 2);
                }
                if (IsHandFinished(hs, false))
                {
                    int rotatedHsInGame = 0;
                    for (int i = 0; i < rotatedHsCopy.InGame.Count; i++)
                    {
                        if (rotatedHsCopy.InGame[i])
                        {
                            rotatedHsInGame++;
                        }
                    }
                    if (rotatedHsInGame >= 2)
                    {
                        Omaha.AddShowdownsRotatedHSCopy(ref rotatedHsCopy, ref playersToPlay);
                    }
                }
                rotatedHsCopy.IsNewHand = isNewHand;
                hhBuilder.UpdateHand(rotatedHsCopy);
                isNewHand = false;
            }
        }  
        private void FinishDecisionHuman()
        {
            if (PauseResumeTxt == GamePauseResume.Run.ToString() && !HoldemOmaha.AllPlayersDidNotPlay(ref didPlayersPlay))
            {
                PauseResumeTxt = GamePauseResume.Resume.ToString();
            }
            sumbets += PlayersToPlay[indexAct].BetSize;
            humanPlayed = true;
            didPlayersPlay[indexAct] = true;
            CheckIfInitCalled();
            if (isGeneratedChecked)
            {
                UpdateHHBuilder();
            }
            HumanDecision = Visibility.Collapsed;
            BetPercent = 0;
            BetSizePlayer = 0;
            if (PauseResumeTxt == GamePauseResume.Pause.ToString() || PauseResumeTxt == GamePauseResume.Run.ToString())
            {
                CheckingEndHandOrPhase(true);
                RunGame();
            }
            else
            {
                CanExecuteNext = true;
                ShowingNextMove(false);
                //CheckingEndHandOrPhase(true);
                //CheckingEndHandOrPhaseForHuman(true);
            }
            canExecuteUndo = true;
            ShowingButtonsForBot();
            //CheckingIfCanRun();
            lastMoveBot = false;
        }
        void RunGameTask()
        {
            canRun = true;
            CanExecuteNext = false;
            // samo cim se klikne na Run mora da se preskoci dodela indeksa igracu jer je vec dodeljen.
            GettingDecision(true);
            if (PlayersToPlay[indexAct].IsBot)
            {
                AddState();
            } while (canRun)
            {
                RunGameCall();
            }
        }
        void ResumeGameTask()
        {
            canRun = true;
            CanExecuteNext = false;
            CheckingEndHandOrPhase(false);
            GettingDecision(true);
            if (PlayersToPlay[indexAct].IsBot)
            {
                AddState();
            } while (canRun)
            {
                RunGameCall();
            }
        }
        private void UpdatingProperties()
        {
            RangeImageSources1 = new();
            RangeImageSources2 = new();
            RangeImageSources3 = new();
            RangeImageSources4 = new();
            RangeImageSourcesMiddle1 = new();
            RangeImageSourcesMiddle2 = new();
            RangeImageSourcesMiddle3 = new();
            RangeImageSourcesMiddle4 = new();
            VisibilityOmahaPostflop = Visibility.Collapsed;
            OnPropertyChanged(nameof(VisibilityOmahaPostflop));
            OnPropertyChanged(nameof(RangeImageSources1));
            OnPropertyChanged(nameof(RangeImageSources2));
            OnPropertyChanged(nameof(RangeImageSourcesMiddle1));
            OnPropertyChanged(nameof(RangeImageSourcesMiddle2));
            RangeTextBetRaise = new();
            RangeTextCallCheck = new();
            RangeTextFold = new();
            if (App.Current != null)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    rangePercTemp.Clear();
                    rangePercTemp2.Clear();
                    rangePercTemp3.Clear();
                });
            }
            TextPercentHandBet = "";
            TextPercentHandCheck = "";
            TextPercentHandFold = "";
            OnPropertyChanged(nameof(TextPercentHandBet));
            OnPropertyChanged(nameof(TextPercentHandCheck));
            OnPropertyChanged(nameof(TextPercentHandFold));
            StringsLog = new();
            OnPropertyChanged(nameof(StringsLog));
            OmahaRanges = new();
            OmahaRangesMid = new();
            OmahaRangesFold = new();
            OmahaShowdowns = new();
            OmahaShowdownsCheckCall = new();
            OmahaShowdownsFold = new();
            OnPropertyChanged(nameof(OmahaShowdownsCheckCall));
            OnPropertyChanged(nameof(OmahaShowdowns));
            OnPropertyChanged(nameof(OmahaShowdownsFold));
            OmahaDraws = new();
            OmahaDrawsCheckCall = new();
            OmahaDrawsFold = new();
            OnPropertyChanged(nameof(OmahaDraws));
            OnPropertyChanged(nameof(OmahaDrawsCheckCall));
            OnPropertyChanged(nameof(OmahaDrawsFold));
            HoldemRanges = new();
            HoldemRangesMid = new();
            HoldemRangesFold = new();
            OnPropertyChanged(nameof(OmahaRanges));
            OnPropertyChanged(nameof(OmahaRangesMid));
            OnPropertyChanged(nameof(OmahaRangesFold));
            OnPropertyChanged(nameof(HoldemRanges));
            OnPropertyChanged(nameof(HoldemRangesMid));
            OnPropertyChanged(nameof(HoldemRangesFold));
        }
        private void SettingHSManualForState(ref HandStateManual hs, ref int numOfPlayersCopy)
        {
            hsManual.NumberOfPlayers = numOfPlayersCopy;
            foreach (var item in playersToPlay)
            {
                hsManual.Actions.Add(item.Action);
                hsManual.IsMyTurn.Add(item.IsMyTurn);
                hsManual.IsBot.Add(item.IsBot);
                var cardsOfPlayerCopy = item.Cards;
                hsManual.CardsOfPlayer.Add(cardsOfPlayerCopy);
                var dealerIdCopy = item.IsDealer;
                hsManual.IsDealer.Add(dealerIdCopy);
            }
            var cardsOnTableCopy = CardsOnTable.ToList();
            hsManual.PublicCards = cardsOnTableCopy.ToList();
            for (int i = 0; i < didPlayersPlay.Count; i++)
            {
                hsManual.DidPlayersPlayed.Add(didPlayersPlay[i]);
            }
        }
        private void AddState()
        {
            try
            {
                hsManual = new();
                List<DecisionMaker> decisionMakersCopy = StrategyUtil.DeepCopy<List<DecisionMaker>>(decisionMakers);
                int numOfPlayersCopy = NumberOfPlayers;
                SettingHSManualForState(ref hsManual, ref numOfPlayersCopy);                
                DecisionState state = null;
                var hsCopy = Gethandstate();
                var lastDecisionCopy = StrategyUtil.DeepCopy<Decision>(lastDecision);
                if (allStates.Count > 1 && lastDecisionMaker == null)
                {
                    MessageBox.Show("DecisionMaker is null");
                }
                var lastDecisionMakerCopy = GetDeepCopy(lastDecisionMaker);
                var hsManualCopy = StrategyUtil.DeepCopy(hsManual);
                var rangeDecisionRaiseBetCopy = StrategyUtil.DeepCopy(RangeDecisionRaiseBet);
                var rangeDecisionCheckCallCopy = StrategyUtil.DeepCopy(RangeDecisionCheckCall);
                var rangeDecisionFoldCopy = StrategyUtil.DeepCopy(RangeDecisionFold);
                if (phase == EnumPhase.Preflop)
                {
                    state = DecisionState.MakeDecisionState(hsCopy, decisionMakersCopy, indexAct, lastDecisionMakerCopy, lastDecisionCopy, hsManualCopy, phase, range, TextPercentHandBet, TextPercentHandCheck, TextPercentHandFold, StringsLog, PlayerTimeToAct);
                }
                else
                {
                    if (gameType == EnumGameType.CashGame)
                    {
                        state = DecisionState.MakeDecisionState(hsCopy, decisionMakersCopy, indexAct, lastDecisionMakerCopy, lastDecisionCopy, hsManualCopy, phase, rangeDecisionRaiseBetCopy, rangeDecisionCheckCallCopy, rangeDecisionFoldCopy, TextPercentHandBet, TextPercentHandCheck, TextPercentHandFold, StringsLog, PlayerTimeToAct);
                    }
                    else
                    {
                        if (RangeDecisionRaiseBet.Count != 0)
                        {
                            state = DecisionState.MakeDecisionStateOmahaPostflop(hsCopy, decisionMakersCopy, indexAct, lastDecisionMakerCopy, lastDecisionCopy, hsManualCopy, phase, rangeDecisionRaiseBetCopy, lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange, lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange, null, TextPercentHandBet, TextPercentHandCheck, TextPercentHandFold, StringsLog, PlayerTimeToAct);
                        }
                        else
                        {
                            if (lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges != null)
                            {
                                state = DecisionState.MakeDecisionStateOmahaPostflop(hsCopy, decisionMakersCopy, indexAct, lastDecisionMakerCopy, lastDecisionCopy, hsManualCopy, phase, null, lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange, lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.FoldRange, lastDecisionMakerCopy.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange, TextPercentHandBet, TextPercentHandCheck, TextPercentHandFold, StringsLog, PlayerTimeToAct);
                            }
                            else
                            {
                                state = DecisionState.MakeDecisionStateOmahaPostflop(hsCopy, decisionMakersCopy, indexAct, lastDecisionMakerCopy, lastDecisionCopy, hsManualCopy, phase, null, null, null, null, TextPercentHandBet, TextPercentHandCheck, TextPercentHandFold, StringsLog, PlayerTimeToAct);
                            }
                        }
                    }
                }
                if (PlayersToPlay[indexAct].IsBot)
                {
                    allStates.Add(state);
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Adding state: " + ex.ToString(), LogLevel.Error);
            }
        }
        private void SettingButtonsVisibility()
        {
            if (!BotController.CheckIfRivalBet(ref sumbets) && phase == EnumPhase.Preflop)
            {
                IsVisibleFoldBtn = Visibility.Visible;
                IsVisibleCallBtn = Visibility.Visible;
                IsVisibleRaiseBtn = Visibility.Visible;
                IsVisibleCheckBtn = Visibility.Collapsed;
                IsVisibleBetBtn = Visibility.Collapsed;
            }
            else if (!BotController.CheckIfRivalBet(ref sumbets))
            {
                IsVisibleCheckBtn = Visibility.Visible;
                IsVisibleBetBtn = Visibility.Visible;
                IsVisibleFoldBtn = Visibility.Collapsed;
                IsVisibleCallBtn = Visibility.Collapsed;
                IsVisibleRaiseBtn = Visibility.Collapsed;
            }
            else
            {
                IsVisibleFoldBtn = Visibility.Visible;
                IsVisibleCallBtn = Visibility.Visible;
                IsVisibleRaiseBtn = Visibility.Visible;
                IsVisibleCheckBtn = Visibility.Collapsed;
                IsVisibleBetBtn = Visibility.Collapsed;
            }
        }
        private void ClearingRangePercents()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                rangePercTemp.Clear();
                rangePercTemp2.Clear();
                rangePercTemp3.Clear();
                RangeDecisionRaiseBet = new();
                RangeDecisionCheckCall = new();
                RangeDecisionFold = new();
            });
        }
        private void SettingCanExecuteNext()
        {
            if (userClickedRun)
            {
                if (PauseResumeTxt == GamePauseResume.Pause.ToString())
                {
                    CanExecuteNext = false;
                }
                else
                {
                    CanExecuteNext = true;
                }
            }
            else
            {
                CanExecuteNext = true;
            }
        }
        private void GettingDecision(bool useSleep)
        {
            AllBetCoeff = new();
            ClearingRangePercents();
            int indexToAct = indexAct;
            hs = Gethandstate();
            Stopwatch sw = new();
            sw.Start();
            if (PlayersToPlay[indexToAct].IsBot)
            {
                var hsCopy = StrategyUtil.DeepCopy(hs);
                GetDecision(decisionMakers[indexToAct], hsCopy, PlayersToPlay[indexToAct], indexToAct);
                SettingCanExecuteNext();
            }
            else
            {
                HoldemOmaha.CollectBetsFromHs(ref hs, ref sumbets);
                CanExecuteNext = false;
                canRun = false;
                GetButtonsContent();
                SettingButtonsVisibility();
                HumanDecision = Visibility.Visible;
                return;
            }
            sumbets += PlayersToPlay[indexToAct].BetSize;
            humanPlayed = false;
            sw.Stop();
            HoldemOmaha.SettingTime(ref sw, ref indexToAct, ref useSleep, ref playerTimeToAct, ref playersToPlay, ref waitDecision, ref sleepTimeForMove, ref dealingCardsDelay);
            if (isGeneratedChecked)
            {
                UpdateHHBuilder();
            }
            ShowingButtonsForBot();
        }
        private void CheckingEndHandOrPhaseForHuman(bool useSleep)
        {
            IsAllIn();
            if (!isAllIn)
            {
                if (!CheckIfHandFinished())
                {
                    ProcessHandFinished(useSleep);
                }
                else
                {
                    ProcessHandFinished(useSleep);
                    if (phaseOver)
                    {
                        GetNextIndexPlayerToAct();
                    }
                }
            }
            else
            {
                CheckIfHandFinished();
                ProcessHandFinished(useSleep);
            }
            isAllIn = false;
        }
        private void CheckingEndHandOrPhase(bool useSleep)
        {
            IsAllIn();
            if (!isAllIn)
            {
                if (!CheckIfHandFinished())
                {
                    ProcessHandFinished(useSleep);
                    GetNextIndexPlayerToAct();
                }
                else
                {
                    ProcessHandFinished(useSleep);
                    if (phaseOver)
                    {
                        GetNextIndexPlayerToAct();
                    }
                }
            }
            else
            {
                CheckIfHandFinished();
                ProcessHandFinished(useSleep);
            }
            isAllIn = false;
        }
        private void RunGameCall()
        {
            CanExecuteNext = false;
            UpdatingProperties();
            CheckingEndHandOrPhase(true);
            GettingDecision(true);
            if (PlayersToPlay[indexAct].IsBot)
            {
                AddState();
            }
        } 
        public void Undoing()
        {
            Undo(true);
        }
        public void Undo(bool haveToDeleteState)
        {
            BetCoeff = "";
            AllBetCoeff.Clear();
            if (testBoardsOpen)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    testBoards.Close();
                    testBoardsOpen = false;
                });
            }
            isUndoClicked = true;
            sumbets = 0;
            if (haveToDeleteState)
            {
                if (playersToPlay[indexAct].IsBot)
                {
                    if (allStates.Count != 1)
                    {
                        allStates.RemoveAt(allStates.Count - 1);
                    }
                }
            }
            if (allStates.Count >= 1)
            {
                if (!PlayersToPlay[indexAct].IsBot)
                {
                    lastState = allStates[allStates.Count - 1];
                }
                else
                {
                    lastState = allStates[allStates.Count - 1];
                    if (allStates.Count - 1 == 0)
                    {
                        if (phase == EnumPhase.Preflop)
                        {
                            canExecuteUndo = false;
                        }
                        else
                        {
                            canExecuteUndo = true;
                        }
                    }
                    else
                    {
                        canExecuteUndo = true;
                    }
                }
            }
            if (lastState.Hs.Cards != null)
            {
                RefreshTable(lastState);
            }
            if (allStates.Count - 1 != 0)
            {
                RefreshDecisionMakers(lastState);
            }
        }
        private void RefreshDecisionMakers(DecisionState lastState)
        {
            if (lastState.AllDecisionMakers != null)
            {
                for (int i = 0; i < decisionMakers.Count; i++)
                {
                    decisionMakers[i] = GetDeepCopy(lastState.AllDecisionMakers[i]);
                }
            }
        }       
        private void ClearingHoldemShowsUndo()
        {
            RangeImageSources1.Clear();
            RangeImageSources2.Clear();
            RangeImageSources3.Clear();
            RangeImageSources4.Clear();
            RangeImageSourcesMiddle1.Clear();
            RangeImageSourcesMiddle2.Clear();
            RangeImageSourcesMiddle3.Clear();
            RangeImageSourcesMiddle4.Clear();
            HoldemRanges.Clear();
            HoldemRangesMid.Clear();
            HoldemRangesFold.Clear();
            RangeTextBetRaise.Clear();
            RangeTextCallCheck.Clear();
            RangeTextFold.Clear();
            TextPercentHandBet = "";
            TextPercentHandCheck = "";
            TextPercentHandFold = "";
        }
        private void ClearingOmahaShowsUndo()
        {
            OmahaShowdowns.Clear();
            OmahaShowdownsCheckCall.Clear();
            OmahaShowdownsFold.Clear();
            OmahaDraws.Clear();
            OmahaDrawsCheckCall.Clear();
            OmahaDrawsFold.Clear();
            OmahaRanges.Clear();
            OmahaRanges.Clear();
            OmahaRangesMid.Clear();
            TextPercentHandBet = "";
            TextPercentHandCheck = "";
            TextPercentHandFold = "";
        }
        private void ChangingVisibilityGridsPreflopOmaha()
        {
            VisibilityOmahaPostflop = Visibility.Collapsed;
            BetRaiseOmahaVisibility = Visibility.Collapsed;
            CheckCallOmahaVisibility = Visibility.Collapsed;
            FoldOmahaVisibility = Visibility.Collapsed;
        }
        private Phase SettingPhaseFromHSBuilder()
        {
            var phaseFromHS = HandState.GetPhase(lastState.Hs.Cards);
            if (phaseFromHS.ToString().ToLower() == "preflop")
            {
                phase = EnumPhase.Preflop;
                if (gameType == EnumGameType.Omaha)
                {
                    ChangingVisibilityGridsPreflopOmaha();
                }
            }
            else if (phaseFromHS.ToString().ToLower() == "flop")
            {
                phase = EnumPhase.Flop;
            }
            else if (phaseFromHS.ToString().ToLower() == "turn")
            {
                phase = EnumPhase.Turn;
            }
            else
            {
                phase = EnumPhase.River;
            }
            return phaseFromHS;
        }
        private void SettingImageSourcePublicCards()
        {
            CardOnTable1 = ImageChange.GetImageSource(CardsOnTable[0]);
            CardOnTable2 = ImageChange.GetImageSource(CardsOnTable[1]);
            CardOnTable3 = ImageChange.GetImageSource(CardsOnTable[2]);
            CardOnTable4 = ImageChange.GetImageSource(CardsOnTable[3]);
            CardOnTable5 = ImageChange.GetImageSource(CardsOnTable[4]);
        }
        private void CashGameShowing(ref DecisionState lastHandState)
        {
            ClearingHoldemShowsUndo();
            if (SettingPhaseFromHSBuilder().ToString().ToLower() == EnumPhase.Preflop.ToString().ToLower())
            {
                if (allStates[allStates.Count - 1].Decision != null && allStates[allStates.Count - 1].DecisionMaker != null)
                {
                    CashGamePreflopFromLaststate(allStates[allStates.Count - 1].Decision, lastHandState.RangeBetRaise);
                }
            }
            else
            {
                if (allStates[allStates.Count - 1].Decision != null && allStates[allStates.Count - 1].DecisionMaker != null)
                {
                    CashGamePostflopFromLaststate(allStates[allStates.Count - 1].DecisionMaker, lastHandState);
                }
            }
        }
        private void OmahaShowing(ref DecisionState lastHandState)
        {
            ClearingOmahaShowsUndo();
            if (SettingPhaseFromHSBuilder().ToString().ToLower() == EnumPhase.Preflop.ToString().ToLower())
            {
                if (allStates[allStates.Count - 1].Decision != null && allStates[allStates.Count - 1].DecisionMaker != null)
                {
                    OmahaGamePreflopFromLaststate(allStates[allStates.Count - 1].Decision, lastHandState);
                }
            }
            else
            {
                if (allStates[allStates.Count - 1].Decision != null && allStates[allStates.Count - 1].DecisionMaker != null)
                {
                    OmahaGamePostFlopFromLaststate(lastHandState);
                }
            }
        }
        /// <summary>
        /// This method serves to Refresh Table after UNDO and set all data from lastHandState
        /// </summary>
        /// <param name="lastHandState">Last Hand state saved</param>
        private void RefreshTable(DecisionState lastHandState)
        {
            try
            {
                StringsLog?.Clear();
                StringsLog = lastHandState.StringsLog;
                PlayerTimeToAct = lastHandState.PlayerTimeToAct;
                numberOfPlayers = lastHandState.HsManual.NumberOfPlayers;
                var cards = lastHandState.HsManual.PublicCards;
                ObservableCollection<string> cardsListToCollection = new();
                foreach (var item in cards)
                {
                    cardsListToCollection.Add(item);
                }
                CardsOnTable = cardsListToCollection;
                SettingImageSourcePublicCards();
                for (int i = 0; i < playersToPlay.Count; i++)
                {
                    PlayersToPlay[i].BetSize = lastHandState.Hs.Bets[i];
                    PlayersToPlay[i].Balance = lastHandState.Hs.Stacks[i];
                    PlayersToPlay[i].InGame = lastHandState.Hs.InGame[i];
                    PlayersToPlay[i].Action = lastHandState.HsManual.Actions[i];
                    PlayersToPlay[i].IsMyTurn = lastHandState.HsManual.IsMyTurn[i];
                    PlayersToPlay[i].IsBot = lastHandState.HsManual.IsBot[i];
                    PlayersToPlay[i].Cards = lastHandState.HsManual.CardsOfPlayer[i];
                    PlayersToPlay[i].ImageSourceCard1 = ImageChange.GetImageSource(lastHandState.HsManual.CardsOfPlayer[i].Substring(0, 2));
                    PlayersToPlay[i].ImageSourceCard2 = ImageChange.GetImageSource(lastHandState.HsManual.CardsOfPlayer[i].Substring(2, 2));
                    if (gameType == EnumGameType.Omaha)
                    {
                        PlayersToPlay[i].ImageSourceCard3 = ImageChange.GetImageSource(lastHandState.HsManual.CardsOfPlayer[i].Substring(4, 2));
                        PlayersToPlay[i].ImageSourceCard4 = ImageChange.GetImageSource(lastHandState.HsManual.CardsOfPlayer[i].Substring(6, 2));
                    }
                    if (PlayersToPlay[i].IsWinner)
                    {
                        PlayersToPlay[i].IsWinner = false;
                        potDealt = false;
                    }
                    PlayersToPlay[i].IsDealer = lastHandState.HsManual.IsDealer[i];
                    if (PlayersToPlay[i].IsDealer)
                    {
                        PlayersToPlay[i].VisibilityDealer = Visibility.Visible;
                    }
                    else
                    {
                        PlayersToPlay[i].VisibilityDealer = Visibility.Collapsed;
                    }
                    didPlayersPlay[i] = lastHandState.HsManual.DidPlayersPlayed[i];
                }
                hs = lastHandState.Hs;
                OnPropertyChanged(nameof(PlayersToPlay));
                for (int i = 0; i < lastHandState.Hs.Bets.Count; i++)
                {
                    sumbets += hs.Bets[i];
                }
                indexAct = lastHandState.IndexPlayer;
                for (int j = 0; j < lastHandState.HsManual.IsMyTurn.Count; j++)
                {
                    if (lastHandState.HsManual.IsMyTurn[j])
                    {
                        if (!PlayersToPlay[indexAct].IsBot)
                        {
                            CanExecuteNext = false;
                            HumanDecision = Visibility.Visible;
                        }
                        else
                        {
                            CanExecuteNext = true;
                            HumanDecision = Visibility.Collapsed;
                        }
                    }
                }
                PotSize = lastHandState.Hs.Pot;
                if (gameType == EnumGameType.CashGame)
                {
                    CashGameShowing(ref lastHandState);
                }
                else
                {
                    OmahaShowing(ref lastHandState);
                }
                SettingCardsVisibilityRefresh(lastHandState);
                if (phase == EnumPhase.Preflop)
                {
                    canTestBoards = false;
                }
                Console.WriteLine(" Phase after undo is:  " + phase.ToString() + " HS CARDS " + "1." + hs.Cards[0] + " " + "2." + hs.Cards[1] + " " + "3." + hs.Cards[2] + " " + "4." + hs.Cards[3] + " " + "5." + hs.Cards[4] + " " + "6." + hs.Cards[5] + " " + "7." + hs.Cards[6] + " ");
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Refreshing table: " + ex.ToString(), LogLevel.Error);
            }
        }
        public static bool AllPlayersDidntPlay(ref List<bool> didPlayersPlay)
        {
            bool result = false;
            foreach (var item in didPlayersPlay)
            {
                if (item)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        public void ShowingButtonsForBot()
        {
            BotController.ShowingButtonsForBot(ref phase, ref didPlayersPlay, ref playersToPlay, ref indexAct, ref botButtonVisible, ref runMode, ref sumbets, ref botCheckCall, ref botBetRaise);
            OnPropertyChanged(nameof(BotButtonVisible));
            OnPropertyChanged(nameof(BotBetRaise));
            OnPropertyChanged(nameof(BotCheckCall));
        }
        private void SettingCardsVisibilityRefresh(DecisionState lastHandState)
        {
            if (lastHandState.Phase == EnumPhase.Preflop)
            {
                Card1Visibility = Visibility.Collapsed;
                Card2Visibility = Visibility.Collapsed;
                Card3Visibility = Visibility.Collapsed;
                Card4Visibility = Visibility.Collapsed;
                Card5Visibility = Visibility.Collapsed;
            }
            else if (lastHandState.Phase == EnumPhase.Flop)
            {
                Card1Visibility = Visibility.Visible;
                Card2Visibility = Visibility.Visible;
                Card3Visibility = Visibility.Visible;
                Card4Visibility = Visibility.Collapsed;
                Card5Visibility = Visibility.Collapsed;
            }
            else if (lastHandState.Phase == EnumPhase.Turn)
            {
                Card1Visibility = Visibility.Visible;
                Card2Visibility = Visibility.Visible;
                Card3Visibility = Visibility.Visible;
                Card4Visibility = Visibility.Visible;
                Card5Visibility = Visibility.Collapsed;
            }
            else
            {
                Card1Visibility = Visibility.Visible;
                Card2Visibility = Visibility.Visible;
                Card3Visibility = Visibility.Visible;
                Card4Visibility = Visibility.Visible;
                Card5Visibility = Visibility.Visible;
            }
        }
        public void SavingJson()
        {
            JsonFile.SavingFileJson(allStates, pathName);
        }
        public static void SettingAllStatesFromJson(ref List<DecisionState> allStatesFromJson, ref EnumGameType gameType, ref int numberOfPlayers, ref List<DecisionState> allStates)
        {
            if (allStatesFromJson[allStatesFromJson.Count - 1].Hs.OmahaCards != null)
            {
                gameType = EnumGameType.Omaha;
                numberOfPlayers = allStatesFromJson[allStatesFromJson.Count - 1].HsManual.NumberOfPlayers;
            }
            else
            {
                gameType = EnumGameType.CashGame;
                numberOfPlayers = allStatesFromJson[allStatesFromJson.Count - 1].HsManual.NumberOfPlayers;
            }
            allStates = allStatesFromJson;

        }
        public void LoadingJson()
        {
            try
            {
                isGamePaused = true;
                var allStatesFromJson = JsonFile.LoadingFileJson(ref pathName);
                cursor = Cursors.Wait;
                SettingAllStatesFromJson(ref allStatesFromJson, ref gameType, ref numberOfPlayers, ref allStates);
                OnPropertyChanged(nameof(CursorMain));
                if (!isInitialized)
                {
                    InitDecisionMakers();
                }
                int indexlastState = allStates.Count - 1;
                lastState = allStates[indexlastState];
                ObservableCollection<string> publicCards = new();
                foreach (var item in lastState.HsManual.PublicCards)
                {
                    publicCards.Add(item);
                }
                CardsOnTable = publicCards;
                lastDecisionMaker = lastState.DecisionMaker;
                selectedEnumCasino = lastDecisionMaker.Casino;
                decisionMakers = lastState.AllDecisionMakers;
                Undo(false);
                DecisionMaker decMakerfromLastState = lastState.DecisionMaker;
                for (int i = 0; i < PlayersToPlay.Count; i++)
                {
                    if (PlayersToPlay[i].IsMyTurn)
                    {
                        if (phase == EnumPhase.Flop)
                        {
                            if (gameType == EnumGameType.CashGame)
                            {
                                //CashGamePostflop(ref decMakerfromLastState);
                                handStateBeforeLastDecision = lastState.Hs;
                                handStateBeforeLastDecision.Bets[i] = 0;
                                decisionMakerBeforeDecision = decisionMakers[i];
                                decisionMakerBeforeDecision.Phase = Phase.PREFLOP;
                                decisionMakerBeforeDecision.FlopSet = false;
                                decisionMakerBeforeDecision.Hero.Strategy.Params.Board = null;
                                //decisionMakerBeforeDecision.Hero.Strategy.Params.Phase = "";
                                if (decisionMakerBeforeDecision.hs != null)
                                {
                                    decisionMakerBeforeDecision.hs.Cards[2] = "";
                                    decisionMakerBeforeDecision.hs.Cards[3] = "";
                                    decisionMakerBeforeDecision.hs.Cards[4] = "";
                                }
                            }
                            else
                            {
                                //OmahaGamePostFlop(ref decMakerfromLastState);
                                handStateBeforeLastDecision = lastState.Hs;
                                handStateBeforeLastDecision.Bets[i] = 0;
                                decisionMakerBeforeDecision = decisionMakers[i];
                                decisionMakerBeforeDecision.Phase = Phase.PREFLOP;
                                decisionMakerBeforeDecision.Hero.Strategy.Params.Board = null;
                                decisionMakerBeforeDecision.hs.Cards[2] = "";
                                decisionMakerBeforeDecision.hs.Cards[3] = "";
                                decisionMakerBeforeDecision.hs.Cards[4] = "";
                            }
                        }
                        else if (phase == EnumPhase.Turn)
                        {
                            if (gameType == EnumGameType.CashGame)
                            {
                                CashGamePostflop(ref decMakerfromLastState);
                                handStateBeforeLastDecision = lastState.Hs;
                                handStateBeforeLastDecision.Bets[i] = 0;
                                decisionMakerBeforeDecision = decisionMakers[i];
                                decisionMakerBeforeDecision.Phase = Phase.FLOP;
                                decisionMakerBeforeDecision.Hero.Strategy.Params.Board = lastState.DecisionMaker.Hero.Strategy.Params.Board.Substring(0, 6);
                                decisionMakerBeforeDecision.hs.Cards[2] = decisionMakerBeforeDecision.Hero.Strategy.Params.Board.Substring(0, 2);
                                decisionMakerBeforeDecision.hs.Cards[3] = decisionMakerBeforeDecision.Hero.Strategy.Params.Board.Substring(2, 2);
                                decisionMakerBeforeDecision.hs.Cards[4] = decisionMakerBeforeDecision.Hero.Strategy.Params.Board.Substring(4, 2);
                                decisionMakerBeforeDecision.hs.Cards[5] = "";
                            }
                        }
                        else if (phase == EnumPhase.River)
                        {
                            if (gameType == EnumGameType.CashGame)
                            {
                                CashGamePostflop(ref decMakerfromLastState);
                                handStateBeforeLastDecision = lastState.Hs;
                                handStateBeforeLastDecision.Bets[i] = 0;
                                decisionMakerBeforeDecision = decisionMakers[i];
                                decisionMakerBeforeDecision.Phase = Phase.TURN;
                                decisionMakerBeforeDecision.Hero.Strategy.Params.Board = lastState.DecisionMaker.Hero.Strategy.Params.Board.Substring(0, 8);
                                decisionMakerBeforeDecision.hs.Cards[2] = decisionMakerBeforeDecision.Hero.Strategy.Params.Board.Substring(0, 2);
                                decisionMakerBeforeDecision.hs.Cards[3] = decisionMakerBeforeDecision.Hero.Strategy.Params.Board.Substring(2, 2);
                                decisionMakerBeforeDecision.hs.Cards[4] = decisionMakerBeforeDecision.Hero.Strategy.Params.Board.Substring(4, 2);
                                decisionMakerBeforeDecision.hs.Cards[5] = decisionMakerBeforeDecision.Hero.Strategy.Params.Board.Substring(6, 2);
                                decisionMakerBeforeDecision.hs.Cards[6] = "";
                            }
                        }
                    }
                }
                if (decisionMakerBeforeDecision.hs != null)
                {
                    decisionMakerBeforeDecision.hs.DealerID = lastState.Hs.DealerID;
                }
                if (gameType == EnumGameType.CashGame)
                {
                    VisibilityHoldem = Visibility.Visible;
                }
                hs = lastState.Hs;
                hs.OriginalNames = new(hs.Names);
                if (phase != EnumPhase.Preflop)
                {
                    canTestBoards = true;
                    CanTestBoardsProp = true;
                    CommandManager.InvalidateRequerySuggested();
                }
                isNewHand = false;
                lastMoveBot = true;
                cursor = Cursors.Arrow;
                OnPropertyChanged(nameof(CursorMain));
                ShowingButtonsForBot();
                OnPropertyChanged(nameof(PlayersToPlay));
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Loading JSON: " + ex.ToString(), LogLevel.Error);
            }
        }
        private void SettingGameTypeFromHH(string gameFromArray)
        {
            if (gameFromArray == "Texas")
            {
                if (GameChoosen != GameEnums.NLH.ToString())
                {
                    GameChoosen = GameEnums.NLH.ToString();
                }
            }
            else
            {
                if (GameChoosen != GameEnums.PLO4.ToString())
                {
                    GameChoosen = GameEnums.PLO4.ToString();
                }
            }
        }
        private void SettingSeatAndNamesFromHH(string[] parts, int i, ref int playersToPlayFromTxt, Dictionary<string, string> seatName, Dictionary<string, double> nameStackDict, ref List<string> namesPlayer)
        {
            for (int j = i; j < parts.Length; j++)
            {
                if (parts[j].Contains("Seat"))
                {
                    playersToPlayFromTxt++;
                    var seat = parts[j].Split(':');
                    var nameAndStack = seat[1].Split(' ');
                    var seatNumber = seat[0].Split('\n');
                    var nameSeat = nameAndStack[1];
                    seatName.Add(seatNumber[1], nameSeat);
                    var stackFromSeat = nameAndStack[3].Split('$');
                    nameStackDict.Add(nameAndStack[1], double.Parse(stackFromSeat[1]));
                    namesPlayer.Add(nameAndStack[1]);
                }
            }
        }
        private void SettingCardsForPlayersFromHH(string[] parts, int i)
        {
            string playerDealtCards = "";
            if (parts[i].Contains("Dealt to"))
            {
                var playerDealt = parts[i].Split(' ');
                playerDealtCards = playerDealt[2];
            }
            if (parts[i].Contains("shows"))
            {
                var playerShows = parts[i].Split(' ', ']', '[', ',', '\n');
                if (playerDealtCards != playerShows[1])
                {
                    for (int j = 0; j < PlayersToPlay.Count; j++)
                    {
                        if (PlayersToPlay[j].Name == playerDealtCards)
                        {
                            string cards = playerShows[6] + playerShows[8];
                            PlayersToPlay[j].Cards = cards;
                            PlayersToPlay[j].ImageSourceCard1 = ImageChange.GetImageSource(playerShows[6]);
                            PlayersToPlay[j].ImageSourceCard2 = ImageChange.GetImageSource(playerShows[8]);
                        }
                    }
                    CardsDeck.Remove(playerShows[6]);
                    CardsDeck.Remove(playerShows[8]);
                    OnPropertyChanged(nameof(PlayersToPlay));
                }
            }
        }
        private void SettingFlop(string[] parts, int i)
        {
            var cardsFlop = parts[i].Split(' ', ']', '[', ',');
            CardsOnTable[0] = cardsFlop[6];
            CardsOnTable[1] = cardsFlop[8];
            CardsOnTable[2] = cardsFlop[10];
            CardsDeck.Remove(cardsFlop[6]);
            CardsDeck.Remove(cardsFlop[8]);
            CardsDeck.Remove(cardsFlop[10]);
        }
        private void SettingTurn(string[] parts, int i)
        {
            var cardTurn = parts[i].Split(' ', ']', '[', ',');
            CardsOnTable[3] = cardTurn[6];
            CardsDeck.Remove(cardTurn[6]);
        }
        private void SettingRiver(string[] parts, int i)
        {
            var cardRiver = parts[i].Split(' ', ']', '[', ',');
            CardsOnTable[4] = cardRiver[6];
            CardsDeck.Remove(cardRiver[6]);
        }
        private void GettingDealerButton(string[] parts, int i, Dictionary<string, string> seatName, ref string seatBtn)
        {
            var seatButton = parts[i].Split('\n');
            seatBtn = seatButton[1].Substring(0, 6);
        }
        private void SettingDealer(Dictionary<string, string> seatName, ref string seatBtn)
        {
            for (int j = 0; j < PlayersToPlay.Count; j++)
            {
                if (PlayersToPlay[j].Name == seatName[seatBtn])
                {
                    PlayersToPlay[j].IsDealer = true;
                    PlayersToPlay[j].VisibilityDealer = Visibility.Visible;
                }
            }
        }
        private void DisablingDealer()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                for (int i = 0; i < PlayersToPlay.Count; i++)
                {
                    if (PlayersToPlay[i].IsDealer)
                    {
                        PlayersToPlay[i].IsDealer = false;
                        PlayersToPlay[i].VisibilityDealer = Visibility.Collapsed;
                    }
                }
            });
        }
        private void SettingSmallBlind(string[] sbParts, double bb)
        {
            for (int j = 0; j < PlayersToPlay.Count; j++)
            {
                if (PlayersToPlay[j].Name == sbParts[1])
                {
                    PlayersToPlay[j].BetSize = bb / 2;
                }
            }
        }
        private void GettingBigBlind(string[] parts, int i, ref string[] bBParts, ref double bB)
        {
            bBParts = parts[i].Split(' ', '\n');
            var bbLine = bBParts[5].Split('$');
            double bb = double.Parse(bbLine[1]);
            bB = bb;
        }
        private void SettingBigBlind(string[] bBParts, ref double bB)
        {
            for (int j = 0; j < PlayersToPlay.Count; j++)
            {
                if (PlayersToPlay[j].Name == bBParts[1])
                {
                    PlayersToPlay[j].BetSize = bB;
                }
            }
            BigBlind = bB;
        }
        private void DisablingTurnAndBetSize()
        {
            try
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    for (int i = 0; i < PlayersToPlay.Count; i++)
                    {
                        PlayersToPlay[i].BetSize = 0;
                        if (PlayersToPlay[i].IsMyTurn)
                        {
                            PlayersToPlay[i].IsMyTurn = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in \"Disabling Turn and bet size\" from hh: " + ex.ToString(), LogLevel.Error);
            }
        }
        private void DealtToFromHH(string[] parts, int i, string cardsForChange)
        {
            try
            {
                cardsForChange = parts[i];
                string[] lineDealtTo = cardsForChange.Split('[', ']');
                string cardsDealt = lineDealtTo[1].Trim();
                string[] cardDealtArr = cardsDealt.Split(' ');
                string cardForDealt = "";
                foreach (var item in cardDealtArr)
                {
                    cardForDealt += item.ToLower();
                }
                string playerDealt = lineDealtTo[0];
                string[] playerDealtName = playerDealt.Split(' ');
                string playerNameFromHH = playerDealtName[2];
                foreach (var item in PlayersToPlay)
                {
                    if (item.Name == playerNameFromHH)
                    {
                        item.Cards = cardForDealt;
                        CardsDeck.Remove(cardForDealt.Substring(0, 2));
                        CardsDeck.Remove(cardForDealt.Substring(2, 2));
                        item.ImageSourceCard1 = ImageChange.GetImageSource(cardForDealt.Substring(0, 2));
                        item.ImageSourceCard2 = ImageChange.GetImageSource(cardForDealt.Substring(2, 2));
                        if (gameType == EnumGameType.Omaha)
                        {
                            item.ImageSourceCard3 = ImageChange.GetImageSource(cardForDealt.Substring(4, 2));
                            item.ImageSourceCard4 = ImageChange.GetImageSource(cardForDealt.Substring(6, 2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in \"dealt to\" from hh: " + ex.ToString(), LogLevel.Error);
            }
        }
        /// <summary>
        /// Loading data from Hand history
        /// </summary>
        private void LoadingFromHH()
        {
            string txtHH = Clipboard.GetText();
            var parts = txtHH.Split('\n');
            parts = txtHH.Split('\r');
            int playersToPlayFromTxt = 0;
            string cardsForChange = "";
            List<string> namesPlayer = new();
            Dictionary<string, double> nameStackDict = new();
            Dictionary<string, string> seatName = new();
            double bB = 0;
            string[] bBParts = new string[] { };
            string[] sbParts = new string[] { };
            string seatBtn = "";
            bool isGameTypeSet = false;
            for (int i = 0; i < parts.Length; i++)
            {
                try
                {
                    if (parts[i].Contains(" Texas Hold'em ") || parts[i].Contains("Omaha"))
                    {
                        var gameParts = parts[i].Split(' ');
                        string gameFromArray = gameParts[4];
                        SettingGameTypeFromHH(gameFromArray);
                        isGameTypeSet = true;
                        Singleton.Log("Game type From HH loaded", LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in Setting Game type From HH: " + ex.ToString());
                }
                if (isGameTypeSet)
                {
                    try
                    {
                        if (parts[i].Contains("Seat ") && !parts[i].Contains("\nSeat 1 is the button"))
                        {
                            if (namesPlayer.Count == 0)
                            {
                                SettingSeatAndNamesFromHH(parts, i, ref playersToPlayFromTxt, seatName, nameStackDict, ref namesPlayer);
                                NumberOfPlayers = playersToPlayFromTxt;
                                DisablingTurnAndBetSize();
                                DisablingDealer();
                                SettingNamesAndBalancesFromHH(ref namesPlayer, nameStackDict);
                            }
                        }
                        Singleton.Log("Names and seats from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in setting names and seats from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("** Dealing Flop **"))
                        {
                            SettingFlop(parts, i);
                        }
                        Singleton.Log("Flop dealt from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in dealing flop from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("** Dealing Turn **"))
                        {
                            SettingTurn(parts, i);
                        }
                        Singleton.Log("Turn dealt from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in dealing turn from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("** Dealing River **"))
                        {
                            SettingRiver(parts, i);
                        }
                        Singleton.Log("River dealt from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in dealing river from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("Dealt to"))
                        {
                            Singleton.Log("Getting cards for player from HH loaded", LogLevel.Info);
                            SettingCardsForPlayersFromHH(parts, i);
                        }
                        Singleton.Log("Setting cards for player from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in setting cards for player from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("big blind"))
                        {
                            GettingBigBlind(parts, i, ref bBParts, ref bB);
                        }
                        Singleton.Log("Getting BB from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in getting BB from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("small"))
                        {
                            sbParts = parts[i].Split(' ', '\n');
                        }
                        Singleton.Log("Getting SB from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in getting SB from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("is the button"))
                        {
                            GettingDealerButton(parts, i, seatName, ref seatBtn);
                        }
                        Singleton.Log("Getting dealer from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in getting dealer from HH: " + ex.ToString());
                    }
                    try
                    {
                        if (parts[i].Contains("\nDealt to"))
                        {
                            DealtToFromHH(parts, i, cardsForChange);
                        }
                        Singleton.Log("Getting cards from HH loaded", LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Singleton.Log("Exception in getting cards from HH: " + ex.ToString());
                    }
                }
            }
            if (isGameTypeSet)
            {
                try
                {
                    SettingSmallBlind(sbParts, bB);
                    Singleton.Log("Setting SB from HH loaded", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in setting SB from HH: " + ex.ToString());
                }
                try
                {
                    SettingBigBlind(bBParts, ref bB);
                    Singleton.Log("Setting BB from HH loaded", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in setting BB from HH: " + ex.ToString());
                }
                try
                {
                    CardsDeck = Card.Deck();
                    SettingImageSourcePublicCards();
                    Singleton.Log("Setting public cards from HH loaded", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in setting public cards from HH: " + ex.ToString());
                }
                try
                {
                    SettingDealer(seatName, ref seatBtn);
                    Singleton.Log("Setting dealer from HH loaded", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in setting dealer from HH: " + ex.ToString());
                }
                try
                {
                    SettingBBAndIndexAct(bB);
                    Singleton.Log("Setting BB and index act from HH loaded", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    Singleton.Log("Exception in setting BB and index act from HH: " + ex.ToString());
                }
                InitDecisionMakers();
            }
        }
        private void SettingBBAndIndexAct(double bB)
        {
            //HoldemOmaha.SettingBBAndIndexAct(bB, playersToPlay, indexAct);
            if (PlayersToPlay.Count > 3)
            {
                for (int i = 0; i < PlayersToPlay.Count; i++)
                {
                    if (PlayersToPlay[i].BetSize == bB)
                    {
                        try
                        {
                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                int next = GetIndexForNextPlayerBotButtons(i);
                                PlayersToPlay[next].IsMyTurn = true;
                                indexAct = next;
                            });
                            Singleton.Log("Setting BB and index act from HH loaded", LogLevel.Info);
                        }
                        catch (Exception ex)
                        {
                            Singleton.Log("Exception in setting BB and index act from HH: " + ex.ToString());
                        }
                    }
                }
            }
            else if (PlayersToPlay.Count <= 3)
            {
                for (int i = 0; i < PlayersToPlay.Count; i++)
                {
                    if (PlayersToPlay[i].IsDealer)
                    {
                        try
                        {
                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                PlayersToPlay[i].IsMyTurn = true;
                                indexAct = i;
                            });
                            Singleton.Log("Setting BB and index act from HH loaded", LogLevel.Info);
                        }
                        catch (Exception ex)
                        {
                            Singleton.Log("Exception in setting BB and index act from HH: " + ex.ToString());
                        }
                    }
                }
            }
        }
        private void SettingNamesAndBalancesFromHH(ref List<string> namesPlayer, Dictionary<string, double> nameStackDict)
        {
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                PlayersToPlay[i].Name = namesPlayer[i];
                string dictName = namesPlayer[i];
                PlayersToPlay[i].Balance = nameStackDict[dictName];
            }
        }
        #region Buttons HumanDecision 
        private void BetBtn1()
        {
            double betSizeCopy = 0;
            HumanDecisionModel.BetBtn1(ref betPercent, ref hs, ref gameType, ref phase, ref bigBlind, ref betSizeCopy, ref playersToPlay, ref indexAct, ref potSize);
            BetSizePlayer = betSizeCopy;
            OnPropertyChanged(nameof(BetSizePlayer));
        }
        private void BetBtn2()
        {
            double betSizeCopy = 0;
            HumanDecisionModel.BetBtn2(ref betPercent, ref hs, ref gameType, ref phase, ref bigBlind, ref betSizeCopy, ref playersToPlay, ref indexAct, ref potSize);
            BetSizePlayer = betSizeCopy;
            OnPropertyChanged(nameof(BetSizePlayer));
        }
        private void BetBtn3()
        {
            double betSizeCopy = 0;
            HumanDecisionModel.BetBtn3(ref betPercent, ref hs, ref gameType, ref phase, ref bigBlind, ref betSizeCopy, ref playersToPlay, ref indexAct, ref potSize);
            BetSizePlayer = betSizeCopy;
            OnPropertyChanged(nameof(BetSizePlayer));
        }
        private void BetBtn4()
        {
            double betSizeCopy = 0;
            HumanDecisionModel.BetBtn4(ref betPercent, ref hs, ref gameType, ref phase, ref bigBlind, ref betSizeCopy, ref playersToPlay, ref indexAct, ref potSize);
            BetSizePlayer = betSizeCopy;
            OnPropertyChanged(nameof(BetSizePlayer));
        }
        private void BetBtn5()
        {
            double maxBet = hs.Bets.Max();
            double restBets = hs.Bets.Sum() - maxBet - PlayersToPlay[indexAct].BetSize;
            double betSizePlayer = GetPotSizeRaisePerc(PotSize + restBets, maxBet, 1);
            BetSizePlayer = betSizePlayer;
            if (gameType == EnumGameType.CashGame)
            {
                HumanDecisionModel.LimitBet(ref betSizePlayer, PlayersToPlay, indexAct);
                BetSizePlayer = betSizePlayer;
            }
            else
            {
                HumanDecisionModel.LimitPotSizeOmaha(betSizePlayer, betSizePlayer, PlayersToPlay, indexAct);
                BetSizePlayer = betSizePlayer;
            }
            OnPropertyChanged(nameof(BetSizePlayer));
        }
        public static double GetPotSizeRaisePerc(double pot, double betSize, double perc)
        {
            double potNext = 2 * betSize + pot;
            return betSize + perc * potNext;
        }
        #endregion
        public static DecisionState CopyDecisionState(DecisionState ds)
        {
            return StrategyUtil.DeepCopy<DecisionState>(ds);
        }
        private void TestingDifferentBoards()
        {
            TestBoardsModel.TestingDifferentBoards(ref canRun, ref phase, ref lastMoveBot, ref testBoards, ref decisionMakerBeforeDecision, ref allStates, ref handStateBeforeLastDecision, ref playersToPlay, ref didPlayersPlay, ref cardsDeck, this, ref testBoardsOpen);
        }
        private bool CanTestBoards()
        {
            return canTestBoards;
        }
        /// <summary>
        /// This method  check if Hand Cards after changing are same as PublicCards
        /// </summary>
        private bool IsHandCardEqualPublicCards(string hand)
        {
            return HoldemOmaha.IsHandCardEqualPublicCards(ref hand, ref cardsOnTable);
        }
        public static void ChangeHandCards(DecisionMaker decisionMaker, ObservableCollection<string> cardsOnTableActive, EnumGameType gameType, ObservableCollection<PlayerViewModel> playersToPlay, int indexAct, List<DecisionMaker> decisionMakers)
        {
            CardsManipulate.ChangeHandCards(ref decisionMaker, ref cardsOnTableActive, ref gameType, ref playersToPlay, ref indexAct, ref decisionMakers);
        }
        /// <summary>
        /// This method serves to Bot must play Check or Call
        /// </summary>
        private void CheckingCallingBot()
        {
            try
            {
                canExecuteBot = false;
                Singleton.DecisionSimulator = EnumDecisionType.CALL;
                Task taskNext = new Task(() => ShowingNextMove(false));
                taskNext.ContinueWith(task =>
                {
                    CanExecuteNext = false;
                    CanExecuteBotProp = false;
                    if (phase != EnumPhase.Preflop)
                    {
                        canTestBoards = true;
                        CanTestBoardsProp = true;
                        CommandManager.InvalidateRequerySuggested();
                    }
                    if (decisionMakers[indexAct].Hero.Strategy.Params.Hand == "")
                    {
                        MessageBox.Show("HAND IS EMPTY");
                    }
                    PlayersToPlay[indexAct].Cards = ParamsHandToCards(lastDecisionMaker.Hero.Strategy.Params.Hand, gameType);
                    if (IsHandCardEqualPublicCards(lastDecisionMaker.Hero.Strategy.Params.Hand))
                    {
                        ChangeHandCards(lastDecisionMaker, CardsOnTable, gameType, PlayersToPlay, indexAct, decisionMakers);
                    }
                    else
                    {
                        if (decisionMakers[indexAct].hs == null)
                        {
                            string indexNextStr = indexAct.ToString();
                        }
                        if (PlayersToPlay[indexAct].IsBot)
                        {
                            PlayersToPlay[indexAct].ImageSourceCard1 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(0, 2));
                            PlayersToPlay[indexAct].ImageSourceCard2 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(2, 2));
                            if (gameType == EnumGameType.Omaha)
                            {
                                PlayersToPlay[indexAct].ImageSourceCard3 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(4, 2));
                                PlayersToPlay[indexAct].ImageSourceCard4 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(6, 2));
                                decisionMakers[indexAct].hs.OmahaCards[0] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(0, 2);
                                decisionMakers[indexAct].hs.OmahaCards[1] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(2, 2);
                                decisionMakers[indexAct].hs.OmahaCards[2] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(4, 2);
                                decisionMakers[indexAct].hs.OmahaCards[3] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(6, 2);
                            }
                            decisionMakers[indexAct].Hero.Strategy.Params.Hand = PlayersToPlay[indexAct].Cards.ToLower();
                            decisionMakers[indexAct] = GetDeepCopy(lastDecisionMaker);
                        }
                    }
                    OnPropertyChanged(nameof(PlayersToPlay));
                    Singleton.DecisionSimulator = EnumDecisionType.NONE;
                    if (IsPhaseFinished())
                    {
                        ShowingButtonsForBot();
                        BotButtonVisible = Visibility.Collapsed;
                    }
                    CanExecuteNext = true;
                    CommandManager.InvalidateRequerySuggested();
                });
                taskNext.Start();
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Checking or Calling Bot: " + ex.ToString(), LogLevel.Error);
            }
        }
        /// <summary>
        /// This method serves to Bot must play Bet or Raise
        /// </summary>
        private void BettingRaisingBot()
        {
            try
            {
                canExecuteBot = false;
                int indexNext = GetIndexForNextPlayerBotButtons(indexAct);
                Singleton.DecisionSimulator = EnumDecisionType.RAISE;
                Task taskNext = new Task(() => ShowingNextMove(false));
                taskNext.ContinueWith(task =>
                {
                    CanExecuteNext = false;
                    if (phase != EnumPhase.Preflop)
                    {
                        canTestBoards = true;
                        CanTestBoardsProp = true;
                        CommandManager.InvalidateRequerySuggested();
                    }
                    PlayersToPlay[indexAct].Cards = ParamsHandToCards(lastDecisionMaker.Hero.Strategy.Params.Hand, gameType);
                    if (IsHandCardEqualPublicCards(lastDecisionMaker.Hero.Strategy.Params.Hand))
                    {
                        ChangeHandCards(lastDecisionMaker, CardsOnTable, gameType, PlayersToPlay, indexAct, decisionMakers);
                    }
                    else
                    {
                        PlayersToPlay[indexAct].ImageSourceCard1 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(0, 2));
                        PlayersToPlay[indexAct].ImageSourceCard2 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(2, 2));
                        if (gameType == EnumGameType.Omaha)
                        {
                            PlayersToPlay[indexAct].ImageSourceCard3 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(4, 2));
                            PlayersToPlay[indexAct].ImageSourceCard4 = ImageChange.GetImageSource(PlayersToPlay[indexAct].Cards.Substring(6, 2));
                            decisionMakers[indexAct].hs.OmahaCards[0] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(0, 2);
                            decisionMakers[indexAct].hs.OmahaCards[1] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(2, 2);
                            decisionMakers[indexAct].hs.OmahaCards[2] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(4, 2);
                            decisionMakers[indexAct].hs.OmahaCards[3] = decisionMakers[indexAct].Hero.Strategy.Params.Hand.Substring(6, 2);
                        }
                        decisionMakers[indexAct].Hero.Strategy.Params.Hand = PlayersToPlay[indexAct].Cards.ToLower();
                        decisionMakers[indexAct] = GetDeepCopy(lastDecisionMaker);
                    }
                    OnPropertyChanged(nameof(PlayersToPlay));
                    Singleton.DecisionSimulator = EnumDecisionType.NONE;
                    if (IsPhaseFinished())
                    {
                        ShowingButtonsForBot();
                    }
                    CanExecuteNext = true;
                    CommandManager.InvalidateRequerySuggested();
                });
                taskNext.Start();
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Betting or Raising Bot: " + ex.ToString(), LogLevel.Error);
            }
        }
        public static string ParamsHandToCards(string paramsHand, EnumGameType gameType)
        {
            return CardsManipulate.ParamsHandToCards(ref paramsHand, ref gameType);
        }
        private void ClearPostflopOmahaGrids()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                OmahaRanges.Clear();
                OmahaRangesMid.Clear();
                OmahaRangesFold.Clear();
                OmahaShowdowns.Clear();
                OmahaShowdownsCheckCall.Clear();
                OmahaShowdownsFold.Clear();
                OmahaDraws.Clear();
                OmahaDrawsCheckCall.Clear();
                OmahaDrawsFold.Clear();
            });
        }
        /// <summary>
        /// This method serves to Refresh Omaha Grid for Choosen Grid => Grid Bet\Raise or Grid Check\Call , or Grid Fold
        /// </summary>
        /// <param name="rangesList">Range for Show</param>
        private void RefreshingOmahaGrid(List<string> rangesList)
        {
            OmahaRanges.Clear();
            OmahaRangesMid.Clear();
            OmahaRangesFold.Clear();
            string board = GetBoard();
            int step = GetStep(rangesList);
            List<PokerUtil.OmahaHandCategoryType> showdownFilters = new();
            List<PokerUtil.EnumOmahaDraw> drawFilters = new();
            if (GridShowSelected == GridShow.BetRaise.ToString())
            {
                Omaha.RefreshingGridBetRaise(OmahaDraws, drawFilters, OmahaShowdowns, showdownFilters, lastDecisionMaker, rangesList, step, gameType, omahaRanges);
            }
            else if (GridShowSelected == GridShow.CheckCall.ToString())
            {
                Omaha.RefreshingGridCheckCall(OmahaDrawsCheckCall, drawFilters, OmahaShowdownsCheckCall, showdownFilters, lastDecisionMaker, rangesList, step, gameType, omahaRangesMid);
            }
            else
            {
                Omaha.RefreshingGridFold(OmahaDrawsFold, drawFilters, OmahaShowdownsFold, showdownFilters, lastDecisionMaker, rangesList, step, gameType, omahaRangesFold);
            }
        }
        /// <summary>
        /// This method serves to Refresh Omaha Grid for Choosen Grid. 
        /// </summary>
        public void Refreshing()
        {
            if (GridShowSelected == GridShow.BetRaise.ToString())
            {
                var betRaiseRanges = lastDecisionMaker.Hero.Strategy.Params.Multi_ranges;
                if (betRaiseRanges.Count != 0)
                {
                    RefreshingOmahaGrid(RangeMultiDecisionToList(ref betRaiseRanges));
                }
                else
                {
                    RefreshingOmahaGrid(lastDecisionMaker.Hero.Strategy.Params.OmahaRanges.BetOrRaiseRange);
                }
            }
            else if (GridShowSelected == GridShow.CheckCall.ToString())
            {
                RefreshingOmahaGrid(lastDecisionMaker.Hero.Strategy.Params.OmahaRanges.CheckOrCallRange);
            }
            else
            {
                RefreshingOmahaGrid(lastDecisionMaker.Hero.Strategy.Params.OmahaRanges.FoldRange);
            }
        }
        private void ViewOfWinnings()
        {
            try
            {
                winningsView = new();
                winningsView.DataContext = new WinningsViewModel(ref dictStartBalance, ref dictSessionBalance, ref dictSessionHands, PlayersToPlay, ref dictallTimeBalance, ref dictAllTimeHands, ref allPlayersProfiles, this);
                winningsView.Show();
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in View of Winnings: " + ex.ToString(), LogLevel.Error);
            }
        }
        private bool CanExecuteWinnings()
        {
            return canExecuteWinnings;
        }
        #endregion
        #region Commands
        public ICommand HideCardsClick { get { return new RelayCommand(HideCardsMethod, CanExecuteHide); } }
        public ICommand ShowCardsClick { get { return new RelayCommand(ShowCardsMethod, CanExecuteShow); } }
        public ICommand ShowStrategyClick { get { return new RelayCommand(OpenSettingsView, CanExecute); } }
        public ICommand SaveDataCommand { get { return new RelayCommand(SavingData, CanExecute); } }
        public ICommand MyICommandThatShouldHandleLoaded { get { return new RelayCommand(ShowPlayersWithoutDealing, CanExecute); } }
        public ICommand NextIsMyTurnCommand { get { return new RelayCommand(ShowNextMove); } }
        public ICommand RunCommand { get { return new RelayCommand(StartGame, CanExecuteRun); } }
        public ICommand PauseCommand { get { return new RelayCommand(PauseGame, CanExecute); } }
        public ICommand FoldBtn { get { return new RelayCommand(Folding, CanExecute); } }
        public ICommand CallBtn { get { return new RelayCommand(Calling, CanExecute); } }
        public ICommand RaiseBtn { get { return new RelayCommand(Raising, CanExecuteRaise); } }
        public ICommand CheckBtn { get { return new RelayCommand(Checking, CanExecute); } }
        public ICommand BetBtn { get { return new RelayCommand(Betting, CanExecuteBet); } }
        public ICommand Bet1 { get { return new RelayCommand(BetBtn1, CanExecute); } }
        public ICommand Bet2 { get { return new RelayCommand(BetBtn2, CanExecute); } }
        public ICommand Bet3 { get { return new RelayCommand(BetBtn3, CanExecute); } }
        public ICommand Bet4 { get { return new RelayCommand(BetBtn4, CanExecute); } }
        public ICommand Bet5 { get { return new RelayCommand(BetBtn5, CanExecute); } }
        public ICommand AllInBtn { get { return new RelayCommand(GoAllIn, CanExecute); } }
        public ICommand RefreshOmahaBet { get { return new RelayCommand(Refreshing, CanExecute); } }
        public ICommand UndoCommand { get { return new RelayCommand(Undoing, CanExecuteUndo); } }
        public ICommand NewHand { get { return new RelayCommand(SetNewHand, CanExecute); } }
        public ICommand SaveJson { get { return new RelayCommand(SavingJson, CanExecute); } }
        public ICommand LoadJson { get { return new RelayCommand(LoadingJson, CanExecuteLoad); } }
        public ICommand CheckCallBtnBot { get { return new RelayCommand(CheckingCallingBot, CanExecuteBot); } }
        public ICommand BetRaiseBtnBot { get { return new RelayCommand(BettingRaisingBot, CanExecuteBot); } }
        public ICommand LoadFromHH { get { return new RelayCommand(LoadingFromHH, CanExecuteLoadFromHH); } }
        public ICommand TestingBoards { get { return new RelayCommand(TestingDifferentBoards, CanTestBoards); } }
        public ICommand ViewWinnings { get { return new RelayCommand(ViewOfWinnings, CanExecuteWinnings); } }
        #endregion       
    }
}

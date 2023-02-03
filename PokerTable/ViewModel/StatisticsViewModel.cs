using DBLib;
using DecisionMaking;
using DecisionMaking.DecisionMaking;
using PokerTable.Model;
using PokerTable.View;
using QueryDBLib;
using System.Collections.ObjectModel;
using System.Windows;

namespace PokerTable.ViewModel
{
    public enum AllStatistic
    {
        Preflop,
        Postflop
    }
    public class StatisticsViewModel : ViewModelBase
    {
        #region Members
        private List<string> AllStats;
        private string selectedStatistic;
        private string namePlayer;
        private Statistic statistic;
        private ObservableCollection<Statistic> statistics;
        private ObservableCollection<Statistic> statisticsPostflop;
        private Visibility preflopStatsVisibility;
        private Visibility postflopStatsVisibility;
        private EnumCasino enumCasino = Properties.Settings.Default.SelectedEnumCasino;
        private string gameType;
        private Statistics statisticsWindow;
        private EnumGameType gameTypeEnum;
        private string connString;
        private double bigBlind;
        private Statistic selectedStatisticRow;
        private TestModel chartModelRight = new();
        private TestModel chartModelLeft = new();
        #endregion
        #region Constructors
        public StatisticsViewModel(string name, DBLib.EnumCasino enumCasino, string gameType)
        {
            bigBlind = Properties.Settings.Default.BigBlind;
            gameType = Properties.Settings.Default.GameChoosen;
            this.gameType = gameType;
            EnumGameType gametypeEnum = GameTypeToEnum(gameType);
            AllStats = new List<string>();
            connString = StrategyUtil.GetConnString((EnumCasino)enumCasino, (EnumHandLevel)StrategyUtil.GetLevelForBBSize(bigBlind), gametypeEnum);
            Properties.Settings.Default.connString = connString;
            Properties.Settings.Default.Save();
            connString = Properties.Settings.Default.connString;

            foreach (var item in Enum.GetValues(typeof(AllStatistic)))
            {
                ListAllStatistic.Add(item.ToString());
            }
            NamePlayer = name;
            statisticsWindow = new Statistics();
            statisticsWindow.DataContext = this;
            SelectedStatistic = AllStatistic.Preflop.ToString();
            statisticsWindow.ShowDialog();
        }
        public StatisticsViewModel()
        {
            AllStats = new List<string>();
            foreach (var item in Enum.GetValues(typeof(AllStatistic)))
            {
                ListAllStatistic.Add(item.ToString());
            }
        }
        #endregion
        #region Properties
        public TestModel TestModel
        {
            get { return chartModelRight; }
            set
            {
                chartModelRight = value;
                OnPropertyChanged(nameof(TestModel));
            }
        }
        public TestModel TestModelLeft
        {
            get { return chartModelLeft; }
            set
            {
                chartModelLeft = value;
                OnPropertyChanged(nameof(TestModelLeft));
            }
        }
        public Statistic Statistic
        {
            get
            {
                return statistic;
            }
            set
            {
                statistic = value;
                OnPropertyChanged(nameof(Statistic));
            }
        }
        public string NamePlayer
        {
            get
            {
                return namePlayer;
            }
            set
            {
                namePlayer = value;
                OnPropertyChanged(nameof(NamePlayer));
            }
        }
        public List<string> ListAllStatistic
        {
            get
            {
                return AllStats;
            }
            set
            {
                AllStats = value;
                OnPropertyChanged(nameof(ListAllStatistic));
            }
        }
        public string SelectedStatistic
        {
            get
            {
                return selectedStatistic;
            }
            set
            {
                selectedStatistic = value;
                if (SelectedStatisticRow != null)
                {
                    SelectedStatisticRow = null;
                }

                TestModel.DataList = new List<StatResult>();
                OnPropertyChanged(nameof(TestModel));
                ShowStats(NamePlayer);
                OnPropertyChanged(nameof(SelectedStatistic));
            }
        }
        public ObservableCollection<Statistic> Statistics
        {
            get
            {
                return statistics;
            }
            set
            {
                statistics = value;
                OnPropertyChanged(nameof(Statistics));
            }
        }
        public ObservableCollection<Statistic> StatisticsPostflop
        {
            get
            {
                return statisticsPostflop;
            }
            set
            {
                statisticsPostflop = value;
                OnPropertyChanged(nameof(StatisticsPostflop));
            }
        }
        public Visibility PreflopStatsVisibility
        {
            get
            {
                return preflopStatsVisibility;
            }
            set
            {
                preflopStatsVisibility = value;
                OnPropertyChanged(nameof(PreflopStatsVisibility));
            }
        }
        public Visibility PostflopStatsVisibility
        {
            get
            {
                return postflopStatsVisibility;
            }
            set
            {
                postflopStatsVisibility = value;
                OnPropertyChanged(nameof(PostflopStatsVisibility));

            }
        }
        public Statistic SelectedStatisticRow
        {
            get
            {
                return selectedStatisticRow;
            }
            set
            {
                selectedStatisticRow = value;
                if (selectedStatisticRow != null)
                {
                    List<string> vsPlayers = new();
                    if (!StrategyUtil.IsHouseBotCasino(enumCasino))
                    {
                        vsPlayers = new List<string>(Singleton.GetVsPlayers(enumCasino));
                    }
                    if (selectedStatistic == AllStatistic.Preflop.ToString())
                    {
                        List<string> vsPlayersEmpty = new();
                        LoadPreflopChart(selectedStatisticRow, NamePlayer, vsPlayersEmpty, Properties.Settings.Default.NumberOfPlayers, ref chartModelLeft);
                        LoadPreflopChart(selectedStatisticRow, NamePlayer, vsPlayers, Properties.Settings.Default.NumberOfPlayers, ref chartModelRight);
                    }
                    else
                    {
                        List<string> vsPlayersEmpty = new();
                        LoadPostflopChart(selectedStatisticRow, NamePlayer, vsPlayersEmpty, Properties.Settings.Default.NumberOfPlayers, ref chartModelLeft);
                        LoadPostflopChart(selectedStatisticRow, NamePlayer, vsPlayers, Properties.Settings.Default.NumberOfPlayers, ref chartModelRight);
                    }
                }
                OnPropertyChanged(nameof(SelectedStatisticRow));
            }
        }
        #endregion
        #region Methods
        private void GetKeysFromStatsDict(Dictionary<string, QueryDBLib.StatResult> stats, ref List<string> keys)
        {
            keys = stats.Keys.ToList<string>();
        }
        private void SetDictionaryToStatisticValues(List<string> keys, Dictionary<string, QueryDBLib.StatResult> stats)
        {
            if (keys.Count > 0)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    Statistic = new Statistic();
                    var statID = keys[i];
                    Statistic.StatID = statID;
                    Statistic.NumOfPlayers = Properties.Settings.Default.NumberOfPlayers;
                    char[] separator = { '#' };
                    string[] statistic = statID.Split(separator);
                    bool isNameEmpty = false;
                    string statPrevActions = "";
                    if (SelectedStatistic == AllStatistic.Preflop.ToString())
                    {
                        StatIDToParamsPreflop(statID);

                        string playerNameStat = statistic[1];
                        Statistic.StatPlayer = playerNameStat;
                        string statName = statistic[0];
                        Statistic.StatName = statName;
                    }
                    else if (SelectedStatistic == AllStatistic.Postflop.ToString())
                    {
                        if (statistic.Length > 1)
                        {
                            statPrevActions = statistic[1];
                            Statistic.PreviousActions = statPrevActions;
                            int a;
                            if (Int32.TryParse(statistic[0], out a))
                            {
                                a = int.Parse(statistic[0]);
                            }
                            string playerNameStat = statistic[2];
                            string statNameEnum = ((EnumActions)a).ToString();
                            Statistic.StatPlayer = playerNameStat;
                            string statName = statNameEnum;
                            Statistic.StatName = statName;
                        }
                        else if (statistic.Length == 1)
                        {
                            char[] separatorIfOne = { '_' };
                            string[] statisticIfOne = statID.Split(separatorIfOne);
                            string statName = statisticIfOne[0];
                            Statistic.StatName = statName;
                            string playerNameStat = statisticIfOne[1];
                            if (playerNameStat.Length == 0)
                            {
                                isNameEmpty = true;
                            }
                            Statistic.StatPlayer = playerNameStat;
                        }
                        StatIDToParamsPostflop(statID, NamePlayer, statPrevActions);
                    }
                    var statValue = stats[statID].Stat * 100;
                    var statSample = stats[statID].Sample_size;
                    Statistic.StatValue = statValue;
                    Statistic.StatSample = statSample;
                    if (SelectedStatistic == AllStatistic.Preflop.ToString())
                    {
                        if (NamePlayer == Statistic.StatPlayer)
                        {
                            OnPropertyChanged(nameof(Statistic));
                            Statistics.Add(Statistic);
                        }
                    }
                    else
                    {
                        if (!isNameEmpty)
                        {
                            if (NamePlayer == Statistic.StatPlayer)
                            {
                                StatisticsPostflop.Add(Statistic);
                            }
                        }
                    }
                }
                OnPropertyChanged(nameof(PreflopStatsVisibility));
                OnPropertyChanged(nameof(PostflopStatsVisibility));
                Statistics = new ObservableCollection<Statistic>(Statistics.OrderBy(Statistic => Statistic.StatName));
            }
        }
        private void StatIDToParamsPreflop(string key)
        {
            string stat_name = string.Empty;
            string player_name = string.Empty;
            string vs_player = string.Empty;
            int pos = -1;
            int vs_pos = -1;
            int num_opp = -1;
            PreflopStats.convert_id_to_params(key, ref stat_name, ref player_name, ref vs_player, ref pos, ref vs_pos, ref num_opp);
            Statistic.Position = Convert_pos(pos);
            Statistic.VsPosition = Convert_pos(vs_pos);
            Statistic.NumOpponents = Convert_num_opp(num_opp);
        }
        private string Convert_pos(int pos)
        {
            if (pos == 0)
            {
                return "UTG";
            }
            else if (pos == 1)
            {
                return "MP";
            }
            else if (pos == 2)
            {
                return "CO";
            }
            else if (pos == 3)
            {
                return "BTN";
            }
            else if (pos == 4)
            {
                return "SB";
            }
            else if (pos == 5)
            {
                return "BB";
            }
            else
            {
                return "AVG";
            }
        }
        private string Convert_num_opp(int num_opp)
        {
            if (num_opp == -1)
            {
                return "AVG";
            }
            else
            {
                return num_opp.ToString();
            }
        }
        private string Int_to_string(int value)
        {
            if (value == -1)
            {
                return "AVG";
            }
            else if (value == 0)
            {
                return "NO";
            }
            else
            {
                return "YES";
            }
        }
        private string Get_str_from_previous_actions(List<PreviousAction> actions)
        {
            string result = string.Empty;
            foreach (PreviousAction a in actions)
            {
                result += a.action.ToString() + " " + a.phase.ToString() + " ";
            }

            return result;
        }
        private List<PreviousAction> Get_previous_actions(string input)
        {
            List<PreviousAction> result = new List<PreviousAction>();
            input = input.Replace("__", "_");
            if (input == string.Empty || input == QueryDBLib.PostflopStats.nonePreviousAction)
            {
                return result;
            }

            char[] delimiter = new char[1];
            delimiter[0] = '_';
            string[] parts = input.Split(delimiter);
            if (parts.Length == 0)
            {
                return result;
            }
            else if (parts.Length == 2)
            {
                EnumActions action = this.GetActionForString(parts[0]);
                EnumPhase phase = this.GetPhaseForString(parts[1]);
                result.Add(new PreviousAction(action, phase));
                return result;
            }
            else
            {
                EnumActions actionFlop = this.GetActionForString(parts[0]);
                EnumPhase phaseFlop = this.GetPhaseForString(parts[1]);
                EnumActions actionTurn = this.GetActionForString(parts[2]);
                EnumPhase phaseTurn = this.GetPhaseForString(parts[3]);
                result.Add(new PreviousAction(actionFlop, phaseFlop));
                result.Add(new PreviousAction(actionTurn, phaseTurn));
                return result;
            }
        }
        private EnumPhase GetPhaseForString(string phase)
        {
            if (phase == EnumPhase.Flop.ToString())
            {
                return EnumPhase.Flop;
            }

            if (phase == EnumPhase.Turn.ToString())
            {
                return EnumPhase.Turn;
            }

            if (phase == EnumPhase.River.ToString())
            {
                return EnumPhase.River;
            }
            else
            {
                return EnumPhase.River;
            }
        }
        private EnumActions GetActionForString(string action)
        {
            if (action == EnumActions.Check.ToString())
            {
                return EnumActions.Check;
            }
            else if (action == EnumActions.Bet.ToString())
            {
                return EnumActions.Bet;
            }
            else if (action == EnumActions.Fold.ToString())
            {
                return EnumActions.Fold;
            }
            else if (action == EnumActions.Raise.ToString())
            {
                return EnumActions.Raise;
            }
            else if (action == EnumActions.ReRaise.ToString())
            {
                return EnumActions.ReRaise;
            }
            else if (action == EnumActions.Bet3Plus.ToString())
            {
                return EnumActions.Bet3Plus;
            }
            else if (action == "Call")
            {
                return EnumActions.Call_;
            }
            else if (action == EnumActions.CallRaise.ToString())
            {
                return EnumActions.CallRaise;
            }
            else if (action == EnumActions.CallReRaise.ToString())
            {
                return EnumActions.CallReRaise;
            }
            else if (action == EnumActions.CallBet3Plus.ToString())
            {
                return EnumActions.CallBet3Plus;
            }
            else
            {
                throw new Exception("unknown action in getActionForString");
            }
        }
        private void StatIDToParamsPostflop(string statID, string player, string previousAction)
        {
            EnumActions action = EnumActions.Bet;
            string previousActions = "";
            int phase = 0;
            string vs_player = "";
            int inPosition = 0;
            int preflop_initiative = 0;
            int pot_type = 0;
            int is_multiway = 0;
            int boardDrawy = 0;
            int boardHigh = 0;
            int bet_category = 0;
            PostflopStats.get_params_from_id(statID, ref action, ref previousActions, ref player, ref vs_player,
                                                     ref inPosition, ref preflop_initiative, ref pot_type, ref is_multiway, ref phase, ref boardDrawy, ref boardHigh, ref bet_category);
            string betCategory = ((DBLib.EnumBetSizeCategory)bet_category).ToString();
            string isMultiway = ((MWPotType)is_multiway).ToString();
            string potType = ((EnumPotType)pot_type).ToString();
            string preflopInitiative = Int_to_string(preflop_initiative);
            string inPositionString = Int_to_string(inPosition);
            string phaseStr = ((EnumPhase)phase).ToString();
            List<PreviousAction> previousActionsList = Get_previous_actions(previousAction);

            string strPrev = this.Get_str_from_previous_actions(previousActionsList);
            Statistic.Action = action.ToString();
            Statistic.Phase = phaseStr;
            Statistic.VsPlayer = vs_player;
            Statistic.PreflopInitiative = preflopInitiative;
            Statistic.PotType = potType;
            Statistic.IsMultiway = isMultiway;
            Statistic.InPosition = inPositionString;
            Statistic.BetCategory = betCategory;
            Statistic.PreviousActions = strPrev;

        }
        private EnumGameType GameTypeToEnum(string gameType)
        {
            if (gameType == "NLH")
            {
                gameTypeEnum = EnumGameType.CashGame;
            }
            else if (gameType == "PLO4")
            {
                gameTypeEnum = EnumGameType.Omaha;
            }
            return gameTypeEnum;
        }
        private void LoadPreflopChart(Statistic selectedRow, string player, List<string> vsPlayers, int numPlayers, ref TestModel chartModel)
        {
            string statID = selectedRow.StatID;
            var parts = statID.Split('#');
            if (parts.Length > 1)
            {
                int pos = Int32.Parse(parts[3]);
                int vsPos = Int32.Parse(parts[4]);
                int numOpp = Int32.Parse(parts[5]);

                int pos6max = PreflopCalculus.Get6MaxPos(pos, numPlayers);
                int vsPos6max = PreflopCalculus.Get6MaxPos(vsPos, numPlayers);
                PreflopStatNames statName = (PreflopStatNames)Enum.Parse(typeof(PreflopStatNames), parts[0]);
                List<int> samples = new List<int>() { 5, 10, 15, 20, 25, 30, 40, 50, 60, 70, 80, 90, 100, 150, 200, 500, 750, 1000, 1250, 1500 };
                List<Tuple<int, int>> result = new();
                switch (statName)
                {
                    case PreflopStatNames.VPIP:
                        break;
                    case PreflopStatNames.PFR:
                        break;
                    case PreflopStatNames.RFI:
                        break;
                    case PreflopStatNames.FoldToSteal:
                        foreach (int sample in samples)
                        {
                            StatResult stat = PreflopStats.GetFoldToStealLastN(sample, connString, player, vsPlayers, pos6max, vsPos6max, numOpp);

                            result.Add(new Tuple<int, int>(sample, Convert.ToInt32(stat.Stat * 100)));
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                        }
                        break;
                    case PreflopStatNames.Bet3:
                        foreach (int sample in samples)
                        {
                            StatResult stat = PreflopStats.get3betLastNPosVsPos(sample, connString, player, vsPlayers, pos6max, vsPos6max, numOpp, 2);
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                            result.Add(new Tuple<int, int>(sample, Convert.ToInt32(stat.Stat * 100)));
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                        }
                        break;
                    case PreflopStatNames.Bet4:
                        foreach (int sample in samples)
                        {
                            StatResult stat = PreflopStats.Get4betAfterRFI(sample, connString, player, vsPlayers, pos6max, vsPos6max, 2);
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                            result.Add(new Tuple<int, int>(sample, Convert.ToInt32(stat.Stat * 100)));
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                        }
                        break;
                    case PreflopStatNames.Bet5:
                        break;
                    case PreflopStatNames.Ft3b:
                        foreach (int sample in samples)
                        {
                            StatResult stat = PreflopStats.GetFt3bAfterRFILastN(sample, connString, player, vsPlayers, pos6max, numOpp, 2);
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                            result.Add(new Tuple<int, int>(sample, Convert.ToInt32(stat.Stat * 100)));
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                        }
                        break;
                    case PreflopStatNames.Ft4b:
                        foreach (int sample in samples)
                        {
                            StatResult stat = PreflopStats.GetFt4bAfter3betLastN(sample, connString, player, vsPlayers, vsPos6max, 2);
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                            result.Add(new Tuple<int, int>(sample, Convert.ToInt32(stat.Stat * 100)));
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                        }
                        break;
                    case PreflopStatNames.Ft5b:
                        break;
                    case PreflopStatNames.Limp3bet:
                        break;
                    case PreflopStatNames.Ft4bCold:
                        foreach (int sample in samples)
                        {
                            StatResult stat = PreflopStats.GetFt4bColdLastN(sample, connString, player, vsPlayers, pos6max, 2);
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                            result.Add(new Tuple<int, int>(sample, Convert.ToInt32(stat.Stat * 100)));
                            if (stat.Sample_size < sample)
                            {
                                break;
                            }
                        }
                        break;
                    case PreflopStatNames.Ft3bCold:
                        break;
                    case PreflopStatNames.Ft3bAfterCall:
                        break;
                    case PreflopStatNames.Bet4Cold:
                        break;
                    default:
                        break;
                }
                if (vsPlayers.Count == 0)
                {
                    LoadChart(result, ref chartModel);
                }
                else
                {
                    LoadChart(result, ref chartModel);
                }
            }
        }
        private void LoadPostflopChart(Statistic selectedRow, string player, List<string> vsPlayers, int numPlayers, ref TestModel chartModel)
        {
            string statID = selectedRow.StatID;
            List<int> samples = new List<int>() { 5, 10, 15, 20, 25, 30, 40, 50, 60, 70, 80, 90, 100, 150, 200, 500, 750, 1000, 1250, 1500 };
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            EnumActions action = EnumActions.Bet;
            string previousActions = "";
            int phase = 0;
            string vs_player = "";
            int inPosition = 0;
            int preflop_initiative = 0;
            int pot_type = 0;
            int is_multiway = 0;
            int boardDrawy = 0;
            int boardHigh = 0;
            int bet_category = 0;
            PostflopStats.get_params_from_id(statID, ref action, ref previousActions, ref player, ref vs_player,
                                                     ref inPosition, ref preflop_initiative, ref pot_type, ref is_multiway, ref phase,
                                                     ref boardDrawy, ref boardHigh, ref bet_category);
            MWPotType mwPot = (MWPotType)is_multiway;
            if (mwPot == MWPotType.HUEarly || mwPot == MWPotType.HULate)
            {
                mwPot = MWPotType.HUAny;
            }
            if (bet_category >= (int)DBLib.EnumBetSizeCategory.Pot150)
            {
                inPosition = -1;
                is_multiway = -1;
                pot_type = -1;
            }
            foreach (int sample in samples)
            {
                var stat = PostflopStats.GetPostflopStatLastNHands(connString, action, previousActions, player, vsPlayers, inPosition, preflop_initiative, pot_type,
                                         mwPot, phase, bet_category, sample, 2);
                result.Add(new Tuple<int, int>(sample, Convert.ToInt32(stat.Stat * 100)));
                if (stat.Sample_size < sample)
                {
                    break;
                }
            }
            if (vsPlayers.Count == 0)
            {
                LoadChart(result, ref chartModel);
            }
            else
            {
                LoadChart(result, ref chartModel);
            }
        }
        private void LoadChart(List<Tuple<int, int>> stats, ref TestModel chartLeftModel)
        {
            chartLeftModel = new TestModel();
            chartLeftModel.DataList = new List<StatResult>();

            List<StatResult> tempList = new List<StatResult>();
            foreach (var tuple in stats)
            {
                StatResult tuple1 = new StatResult();
                tuple1.Stat = tuple.Item1;
                tuple1.Sample_size = tuple.Item2;
                if (tuple1.Sample_size > 0)
                {
                    tempList.Add(new StatResult() { Sample_size = tuple1.Sample_size, Stat = tuple1.Stat });
                }
            }
            chartLeftModel.DataList = tempList;
            OnPropertyChanged(nameof(TestModelLeft));
            OnPropertyChanged(nameof(TestModel));
        }
        public void ShowStats(string name)
        {
            NamePlayer = name;
            string id_casino_level = StrategyUtil.GetIdCasinoLevelGameType(enumCasino, gameTypeEnum);
            if (SelectedStatistic == AllStatistic.Preflop.ToString())
            {
                var preflopAllStats = Singleton.All_preflop_stats;
                if (preflopAllStats.Count > 0)
                {
                    var stats = preflopAllStats[id_casino_level];
                    Statistics = new ObservableCollection<Statistic>();
                    OnPropertyChanged(nameof(Statistics));
                    PreflopStatsVisibility = Visibility.Visible;
                    PostflopStatsVisibility = Visibility.Collapsed;
                    List<string> keys = new List<string>();
                    GetKeysFromStatsDict(stats, ref keys);
                    SetDictionaryToStatisticValues(keys, stats);
                }
            }
            else if (SelectedStatistic == AllStatistic.Postflop.ToString())
            {
                var postflopAllStats = Singleton.All_postflop_stats;
                if (postflopAllStats.Count > 0)
                {
                    var statsPostflop = postflopAllStats[id_casino_level];
                    StatisticsPostflop = new ObservableCollection<Statistic>();
                    PreflopStatsVisibility = Visibility.Collapsed;
                    PostflopStatsVisibility = Visibility.Visible;
                    OnPropertyChanged(nameof(StatisticsPostflop));
                    List<string> keys = new List<string>();
                    GetKeysFromStatsDict(statsPostflop, ref keys);
                    SetDictionaryToStatisticValues(keys, statsPostflop);
                }
            }
            OnPropertyChanged(nameof(Statistics));
        }
        #endregion
    }
}

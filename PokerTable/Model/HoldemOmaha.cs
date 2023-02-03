using DBLib;
using DBUserModeling;
using DecisionMaking;
using DecisionMaking.DecisionMaking;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using PokerTable.View;
using PokerTable.ViewModel;
using QueryDBLib;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class HoldemOmaha
    {
        #region Methods
        public static int GetIndexForNextPlayerBotButtons(ref int indexForAct, ref ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            int index = indexForAct;
            bool indexFound = false;
            while (!indexFound)
            {
                index++;
                if (index == PlayersToPlay.Count)
                {
                    index = 0;
                }
                for (int i = 0; i < PlayersToPlay.Count; i++)
                {
                    if (PlayersToPlay[index].InGame == true)
                    {
                        indexFound = true;
                        break;
                    }
                }
            }
            return index;
        }
        public static void SettingBBAndIndexAct(double bB, ObservableCollection<PlayerViewModel> PlayersToPlay, int indexAct)
        {
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
                                int next = GetIndexForNextPlayerBotButtons(ref i, ref PlayersToPlay);
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
        public static void HandlingAllBetCoeff(ref List<string> allBetCoeff, ref List<MultiRange> rangeDecisionRaiseBet, ref Visibility AllBetCoeffVisibility, ref string BetCoeff)
        {
            allBetCoeff = new();
            allBetCoeff.Clear();
            if (rangeDecisionRaiseBet.Count > 1)
            {
                allBetCoeff.Add("ALL");
                AllBetCoeffVisibility = Visibility.Visible;
                BetCoeff = allBetCoeff[0];
            }
            foreach (var rangeRaiseBet in rangeDecisionRaiseBet)
            {
                allBetCoeff.Add(rangeRaiseBet.BetCoeff.ToString());
            }
            if (allBetCoeff.Count == 0)
            {
            }
            else if (allBetCoeff.Count >= 1)
            {
                AllBetCoeffVisibility = Visibility.Visible;
                BetCoeff = allBetCoeff[0];
            }
        }
        public static void SettingPercentText(ref double allHands,ref int indexOfBetCoeff, ref double allHandsBetRaise, ref string textPercentHandBet, ref string textPercentHandCheck, ref string textPercentHandFold, ref DecisionMaking.Range rangeDecision, ref DecisionMaking.Range rangeDecision2, ref List<MultiRange> rangeDecisionRaiseBet)
        {
            if (allHands == 0)
            {
                textPercentHandBet = "Bet / Raise: 0%";
                textPercentHandCheck = "Check/Call: 0%";
                textPercentHandFold = "Fold: 0%";
            }
            else
            {
                if (rangeDecisionRaiseBet.Count == 0)
                {
                    textPercentHandBet = "Bet/Raise: " + MainWindowViewModel.GetPercentHands(0, allHands);
                    textPercentHandCheck = "Check/Call: " + MainWindowViewModel.GetPercentHands((double)rangeDecision.Combos.Count, allHands);
                    textPercentHandFold = "Fold: " + MainWindowViewModel.GetPercentHands((double)rangeDecision2.Combos.Count, allHands);
                }
                else
                {
                    textPercentHandBet = "Bet/Raise: " + MainWindowViewModel.GetPercentHands(allHandsBetRaise, allHands);
                    textPercentHandCheck = "Check/Call: " + MainWindowViewModel.GetPercentHands((double)rangeDecision.Combos.Count, allHands);
                    textPercentHandFold = "Fold: " + MainWindowViewModel.GetPercentHands((double)rangeDecision2.Combos.Count, allHands);
                }
            }
        }
        public static int GetRandomNumberForDealer(ref Random r, ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            int randomNum = r.Next(0, PlayersToPlay.Count);
            return randomNum;
        }
        public static void GetDealer(ref Random random, ObservableCollection<PlayerViewModel> PlayersToPlay, ref bool IsDealer, ref Visibility DealerVisibility)
        {
            int r = GetRandomNumberForDealer(ref random, PlayersToPlay);
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (i == r)
                {
                    IsDealer = true;
                    PlayersToPlay[i].IsDealer = IsDealer;
                    PlayersToPlay[i].VisibilityDealer = Visibility.Visible;
                }
                else
                {
                    IsDealer = false;
                    PlayersToPlay[i].IsDealer = IsDealer;
                    DealerVisibility = Visibility.Collapsed;
                    PlayersToPlay[i].VisibilityDealer = Visibility.Collapsed;
                }
            }
        }
        /// <summary>
        ///  If public cards are in conflict (after loading new board), then change card turn or river 
        /// </summary>
        /// <param name="handState">current hand state</param>
        public static void ChangingPublicCardsIfSame(ref HandState handState, ref EnumPhase phase, ref ObservableCollection<string> CardsOnTable, ref List<string> cardsDeck, ImageSource CardOnTable4, ImageSource CardOnTable5)
        {
            try
            {
                if (phase == EnumPhase.Turn)
                {
                    List<string> newCardsList = new();
                    int counter = 0;
                    foreach (var item in CardsOnTable)
                    {
                        if (!newCardsList.Contains(item))
                        {
                            newCardsList.Add(item);
                        }
                        else
                        {
                            counter++;
                        }
                    }
                    if (counter > 0)
                    {
                        CardsOnTable[3] = TestBoardsViewModel.ChangeCardToRandom(cardsDeck);
                        CardOnTable4 = ImageChange.GetImageSource(CardsOnTable[3]);
                        cardsDeck.Remove(CardsOnTable[3]);
                        handState.Cards[5] = CardsOnTable[3];
                    }
                }
                else if (phase == EnumPhase.River)
                {
                    List<string> newCardsList = new();
                    int counter = 0;
                    foreach (var item in CardsOnTable)
                    {
                        if (!newCardsList.Contains(item))
                        {
                            newCardsList.Add(item);
                        }
                        else
                        {
                            counter++;
                        }
                    }
                    if (counter > 0)
                    {
                        CardsOnTable[4] = TestBoardsViewModel.ChangeCardToRandom(cardsDeck);
                        CardOnTable5 = ImageChange.GetImageSource(CardsOnTable[4]);
                        cardsDeck.Remove(CardsOnTable[4]);
                        handState.Cards[6] = CardsOnTable[4];
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Changing Public Cards If they are same, after Testing Boards and loading new FlopCards: " + ex.ToString(), LogLevel.Error);
                MessageBox.Show("2 or more cards of public cards are same!");
            }
        }
        public static List<string> CreatingListOfSelectedPlayerCards(ref DecisionMaker actualDecisionMaker, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref EnumGameType gameType)
        {
            List<string> cardsPlayer = new();
            try
            {
                for (int i = 0; i < PlayersToPlay.Count; i++)
                {
                    if (PlayersToPlay[i].InGame)
                    {
                        if (PlayersToPlay[i].Name == actualDecisionMaker.HeroName)
                        {
                            cardsPlayer.Add(PlayersToPlay[i].Cards.Substring(0, 2));
                            cardsPlayer.Add(PlayersToPlay[i].Cards.Substring(2, 2));
                            if (gameType == EnumGameType.Omaha)
                            {
                                cardsPlayer.Add(PlayersToPlay[i].Cards.Substring(4, 2));
                                cardsPlayer.Add(PlayersToPlay[i].Cards.Substring(6, 2));
                            }
                        }
                    }
                }
                return cardsPlayer;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Creating List Of Selected Player Cards: " + ex.ToString(), LogLevel.Error);
            }
            return cardsPlayer;
        }
        public static List<string> DMRangeToList(DecisionMaking.Range rangeDecision, ref EnumGameType gameType)
        {
            List<string> listOfRanges = new();
            try
            {
                for (int i = 0; i < rangeDecision.Combos.Count; i++)
                {
                    if (gameType == EnumGameType.CashGame)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            listOfRanges.Add(rangeDecision.Combos[i].Hand);
                        });
                    }
                }
                return listOfRanges;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Decision Maker Range to List: " + ex.ToString(), LogLevel.Error);
            }
            return listOfRanges;
        }
        public static List<string> RangeMultiDecisionToList(ref List<MultiRange> multiRanges, ref int CountHandsRaiseBet, ref List<string> AllBetCoeff, ref string BetCoeff)
        {
            CountHandsRaiseBet = 0;
            int indexBetcoeff = AllBetCoeff.IndexOf(BetCoeff);
            try
            {
                if (indexBetcoeff == -1)
                {
                    return new();
                }
                foreach (var multiRange in multiRanges)
                {
                    CountHandsRaiseBet += multiRange.Range.Count;
                }
                if (multiRanges.Count > 1)
                {
                    if (indexBetcoeff != 0)
                    {
                        return multiRanges[indexBetcoeff - 1].Range;
                    }
                    else
                    {
                        List<string> allMultiRanges = new();
                        for (int i = 0; i < multiRanges.Count; i++)
                        {
                            foreach (var item in multiRanges[i].Range)
                            {
                                allMultiRanges.Add(item);
                            }
                        }
                        return allMultiRanges;
                    }
                }
                return multiRanges[indexBetcoeff].Range;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Multi Range Decision to List: " + ex.ToString(), LogLevel.Error);
            }
            return multiRanges[indexBetcoeff].Range;
        }
        public static int GetStep(ref List<string> ranges, ref string NumShowHandsChoosen)
        {
            int numHands = 0;
            int step = 1;
            try
            {
                if (NumShowHandsChoosen == "100")
                {
                    numHands = 100;
                }
                else if (NumShowHandsChoosen == "1000")
                {
                    numHands = 1000;
                }
                else
                {
                    numHands = ranges.Count;
                }
                step = ranges.Count / numHands;
                if (ranges.Count < numHands)
                {
                    numHands = ranges.Count;
                    step = 1;
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Gettimng step for showing ranges: " + ex.ToString());
            }
            return step;
        }
        public static string GetBoard(ref EnumPhase phase, ref ObservableCollection<string> CardsOnTable)
        {
            string board = "";
            if (phase == EnumPhase.Flop)
            {
                for (int i = 0; i < CardsOnTable.Count - 2; i++)
                {
                    board += CardsOnTable[i];
                }
            }
            else if (phase == EnumPhase.Turn)
            {
                for (int i = 0; i < CardsOnTable.Count - 1; i++)
                {
                    board += CardsOnTable[i];
                }
            }
            else
            {
                foreach (var item in CardsOnTable)
                {
                    board += item;
                }
            }
            return board.ToLower();
        }
        public static void CollectBetsFromHs(ref HandState hs, ref double sumbets)
        {
            for (int i = 0; i < hs.Bets.Count; i++)
            {
                sumbets += hs.Bets[i];
            }
        }
        public static void SettingTime(ref Stopwatch sw, ref int indexToAct, ref bool useSleep, ref string PlayerTimeToAct, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref double WaitDecision, ref int sleepTimeForMove, ref double DealingCardsDelay)
        {
            double elapsedMS = sw.Elapsed.TotalMilliseconds;
            double elapsedS = Math.Round(sw.Elapsed.TotalSeconds, 2);
            string timeToAct = elapsedS.ToString();
            PlayerTimeToAct = PlayersToPlay[indexToAct].Name + " " + timeToAct + "s";
            int sleepTime = (int)WaitDecision - (int)elapsedMS;
            sleepTimeForMove = sleepTime;
            if (sleepTime < 0)
            {
                sleepTime = 0;
            }
            int delayTime = (int)DealingCardsDelay - (int)elapsedMS;
            if (delayTime < 0)
            {
                delayTime = 0;
            }
            if (useSleep)
            {
                Thread.Sleep(sleepTime);
            }
        }
        public static bool IsHandCardEqualPublicCards(ref string hand, ref ObservableCollection<string> CardsOnTable)
        {
            bool isSame = false;
            List<string> cards = new();
            cards.Add(hand.Substring(0, 2));
            cards.Add(hand.Substring(2, 2));
            if (hand.Length == 8)
            {
                cards.Add(hand.Substring(4, 2));
                cards.Add(hand.Substring(6, 2));
            }
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] == CardsOnTable[0].ToLower() || cards[i] == CardsOnTable[1].ToLower() || cards[i] == CardsOnTable[2].ToLower() || cards[i] == CardsOnTable[3].ToLower() || cards[i] == CardsOnTable[4].ToLower())
                {
                    isSame = true;
                    break;
                }
            }
            return isSame;
        }
        public static void CheckIfBalanceSmallerThanBet(ref HandState tempAllInSituationOnTable, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref double PotSize)
        {
            try
            {
                if (tempAllInSituationOnTable.Names != null)
                {
                    HandState hsCopy = tempAllInSituationOnTable;
                    List<double> betsFromHS = new();
                    List<double> betsFromHsWithoutBiggest = new();
                    List<double> betsWithoutBiggest = new();
                    for (int i = 0; i < hsCopy.Names.Count; i++)
                    {
                        if (hsCopy.InGame[i])
                        {
                            betsFromHS.Add(hsCopy.Bets[i]);
                        }
                    }
                    double biggestBet = betsFromHS.Max();
                    betsWithoutBiggest = betsFromHS;
                    betsWithoutBiggest.Remove(biggestBet);
                    double nextBiggestBet = betsWithoutBiggest.Max();
                    for (int i = 0; i < hsCopy.Bets.Count; i++)
                    {
                        if (hsCopy.Bets[i] == biggestBet)
                        {
                            double difference = biggestBet - nextBiggestBet;
                            hsCopy.Bets[i] = nextBiggestBet;
                            PlayersToPlay[i].BetSize = hsCopy.Bets[i];
                            PlayersToPlay[i].Balance += difference;
                            PotSize -= difference;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Checking if balance smaller than bet: " + ex.ToString(), LogLevel.Error); ;
            }
        }
        public static void CheckAndSetAllPreflopStats(ref EnumCasino selectedEnumCasino, ref EnumGameType gameType)
        {
            if (Singleton.All_preflop_stats?.Count > 0)
            {
                string id_casino_level = StrategyUtil.GetIdCasinoLevelGameType(selectedEnumCasino, gameType);
                if (!Singleton.All_preflop_stats.ContainsKey(id_casino_level))
                {
                    Singleton.All_preflop_stats.Add(id_casino_level, new Dictionary<string, StatResult>());
                    Singleton.All_postflop_stats.Add(id_casino_level, new Dictionary<string, StatResult>());
                }
                if (Singleton.All_preflop_stats[id_casino_level].Count > 0)
                {
                    Singleton.All_preflop_stats[id_casino_level].Clear();
                }
                else if (Singleton.All_postflop_stats[id_casino_level].Count > 0)
                {
                    Singleton.All_postflop_stats[id_casino_level].Clear();
                }
            }
        }
        public static ObservableCollection<int> Numbers()
        {
            ObservableCollection<int> nums = new();
            for (int i = 2; i <= 9; i++)
            {
                nums.Add(i);
            }
            return nums;
        }
        public static int GetindexPlayerToAct(ref int indexAct, ref ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            bool indexFound = false;
            while (!indexFound)
            {
                indexAct++;
                if (indexAct == PlayersToPlay.Count)
                {
                    indexAct = 0;
                    if (PlayersToPlay[indexAct].InGame == true)
                    {
                        PlayersToPlay[indexAct].IsMyTurn = true;
                    }
                }
                for (int i = 0; i < PlayersToPlay.Count; i++)
                {
                    if (PlayersToPlay[indexAct].InGame == true)
                    {
                        indexFound = true;
                        PlayersToPlay[indexAct].IsMyTurn = true;
                        break;
                    }
                }
            }
            return indexAct;
        }
        public static void CheckIfTestingBoardsOpen(bool testBoardsOpen, TestBoards testBoards)
        {
            if (testBoardsOpen)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    testBoards.Close();
                    testBoardsOpen = false;
                });
            }
        }
        public static void SettingHsManualNewHand(ref int NumberOfPlayers, ref HandStateManual hsManual, ref ObservableCollection<PlayerViewModel> playersToPlay, ref ObservableCollection<string> CardsOnTable, ref List<bool> didPlayersPlay)
        {
            int numOfPlayersCopy = NumberOfPlayers;
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
        public static bool AllPlayersDidNotPlay(ref List<bool> didPlayersPlay)
        {
            bool allDidNotPlayed = false;
            foreach (var item in didPlayersPlay)
            {
                if (item)
                {
                    allDidNotPlayed = true;
                }
            }
            return allDidNotPlayed;
        }
        public static void CollapsingVisibilityHumanRaiseCheckFold(ref Visibility isVisibleFoldBtn, ref Visibility isVisibleCallBtn, ref Visibility isVisibleRaiseBtn, ref Visibility isVisibleCheckBtn, ref Visibility isVisibleBetBtn, ref Visibility humanDecision)
        {
            isVisibleFoldBtn = Visibility.Collapsed;
            isVisibleCallBtn = Visibility.Collapsed;
            isVisibleRaiseBtn = Visibility.Collapsed;
            isVisibleCheckBtn = Visibility.Collapsed;
            isVisibleBetBtn = Visibility.Collapsed;
            humanDecision = Visibility.Collapsed;
        }
        public static void CollapsingOmahaVisibility(ref Visibility omahaVisibility)
        {
            omahaVisibility = Visibility.Collapsed;
        }
        public static void SettingEnumGridShow(ref List<string> enumGridShow)
        {
            enumGridShow = new();
            foreach (var item in Enum.GetValues(typeof(GridShow)))
            {
                enumGridShow.Add(item.ToString());
            }
        }
        public static void CheckingAndSettingHandId(ref string handIdPath, ref int handCount)
        {
            handIdPath = @"C:\katarina\handId.txt";
            if (!File.Exists(handIdPath))
            {
                handCount = 1;
                File.WriteAllText(handIdPath, handCount.ToString());
            }
        }
        public static void SettingChooseGame(ref List<string> chooseGame)
        {
            chooseGame = new();
            foreach (var item in Enum.GetValues(typeof(GameEnums)))
            {
                chooseGame.Add(item.ToString());
            }
        }
        public static void SettingCanExecuteBetRaiseIfBetSize0(ref double BetSizePlayer, ref bool canExecuteBet, ref bool canExecuteRaise)
        {
            if (BetSizePlayer <= 0)
            {
                canExecuteBet = false;
                canExecuteRaise = false;
            }
        }
        public static void AddingCasinosToCollection(ref ObservableCollection<EnumCasino> enumCasinosCollection)
        {
            foreach (EnumCasino item in Enum.GetValues(typeof(EnumCasino)))
            {
                enumCasinosCollection.Add(item);
            }
        }
        public static void SettingSelectedEnumCasino(ref EnumCasino selectedEnumCasino)
        {
            selectedEnumCasino = Properties.Settings.Default.SelectedEnumCasino;
            if (selectedEnumCasino == EnumCasino.PartyPoker)
            {
                selectedEnumCasino = EnumCasino.PPPoker;
                Properties.Settings.Default.SelectedEnumCasino = selectedEnumCasino;
            }
        }
        public static void SettingIsGeneratedChecked(ref bool isGeneratedChecked)
        {
            isGeneratedChecked = Properties.Settings.Default.IsGeneratedChecked;
            if (isGeneratedChecked != true || isGeneratedChecked != false)
            {
                isGeneratedChecked = true;
                Properties.Settings.Default.IsGeneratedChecked = isGeneratedChecked;
            }
        }
        public static void SettingUndoClickedRunPotInitializedAndCursor(ref bool isUndoClicked, ref bool potDealt, ref bool userClickedRun, ref bool isInitialized, ref Cursor cursor)
        {
            isUndoClicked = false;
            potDealt = false;
            userClickedRun = false;
            isInitialized = false;
            cursor = Cursors.Arrow;
        }
        public static void CollapsingIsVisible(ref Visibility isVisible)
        {
            isVisible = Visibility.Collapsed;
        }
        #endregion
    }
}

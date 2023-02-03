using DBLib;
using DecisionMaking;
using PokerTable.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class Omaha : ViewModelBase
    {
        #region Members
        private ImageSource img1;
        private ImageSource img2;
        private ImageSource img3;
        private ImageSource img4;
        private PokerUtil.OmahaHandCategoryType showdowns;     
        private PokerUtil.EnumOmahaDraw draws;
        private PokerUtil.EnumBlockerEffectOmaha blockers;
        private PokerUtil.OmahaHandCategoryType filteredShowdowns;
        private PokerUtil.EnumOmahaDraw filteredDraws;
        private string showdownPercent;
        private bool isChecked;
        private bool isCheckedDraw;
        private static ObservableCollection<Omaha> showdownList;
        private string drawsPercent;
        private string equity;
        #endregion
        #region Constructor
        public Omaha()
        {
            showdownList = new();
        }
        #endregion
        #region Properties
        public string Equity
        {
            get { return equity; }
            set
            {
                equity = value;
            }
        }
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
               isChecked = value;      
            }
        }
        public bool IsCheckedDraw
        {
            get { return isCheckedDraw; }
            set
            {
                isCheckedDraw = value;
            }
        }
        public string ShowdownPercent
        {
            get { return showdownPercent; }
            set
            {
                showdownPercent = value;
            }
        }
        public string DrawsPercent
        {
            get { return drawsPercent; }
            set
            {
                drawsPercent = value;
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
        public ImageSource Img4
        {
            get { return img4; }
            set { img4 = value; }
        }     
        public PokerUtil.OmahaHandCategoryType Showdowns
        {
            get { return showdowns; }
            set
            {
                showdowns = value;
            }
        }
        public PokerUtil.OmahaHandCategoryType FilteredShowdowns
        {
            get { return filteredShowdowns; }
            set
            {
                filteredShowdowns = value;
            }
        }
        public static ObservableCollection<Omaha> ShowdownList
        {
            get
            {
                return showdownList;
            }
            set
            {
                showdownList = value;
            }
        }
        public PokerUtil.EnumOmahaDraw Draws
        {
            get { return draws; }
            set
            {
                draws = value;
            }
        }
        public PokerUtil.EnumOmahaDraw FilteredDraws
        {
            get { return filteredDraws; }
            set
            {
                filteredDraws = value;
            }
        }
        public PokerUtil.EnumBlockerEffectOmaha Blockers
        {
            get { return blockers; }
            set
            {
                blockers = value;
            }
        }
        #endregion
        #region Methods
        public static Omaha GetOmahaItem(ImageSource img1, ImageSource img2,ImageSource img3, ImageSource img4, PokerUtil.OmahaHandCategoryType showdowns, PokerUtil.EnumOmahaDraw draws, PokerUtil.EnumBlockerEffectOmaha blockers, string equity)
        {
            Omaha item = new();
            item.img1 = img1;
            item.img2 = img2;
            item.img3 = img3;
            item.img4 = img4;
            item.showdowns = showdowns;
            item.draws = draws;
            item.blockers = blockers;
            item.equity = equity;
            return item;
        }
        public static Omaha GetOmahaFilteredShowdowns(PokerUtil.OmahaHandCategoryType showdown,  string percent, bool isChecked)
        {
            Omaha item = new();           
            item.filteredShowdowns = showdown;
            item.showdownPercent = percent;
            item.isChecked = isChecked;
            return item;
        }
        public static double MultiplyBy100(double equity)
        {
            return equity * 100;
        }
        public static Omaha GetOmahaFilteredDraws(PokerUtil.EnumOmahaDraw draws, string percent, bool isChecked)
        {
            Omaha item = new();
            item.filteredDraws = draws;
            item.drawsPercent = percent;
            item.isChecked = isChecked;
           return item;
        }
        /// <summary>
        /// Dealing cards to players - OMAHA 
        /// </summary>
        public static string DealCardsToPlayerPLO4(ref List<string> imgCardsDealedToPlayer, ref List<string> CardsDeck, ref List<string> DealedCardsToPlayer)
        {
            imgCardsDealedToPlayer = new();
            var cardsForPlayer = Card.RandomCards(4, ref CardsDeck);
            var cards = "";
            foreach (string card in cardsForPlayer)
            {
                DealedCardsToPlayer.Add(card);
                imgCardsDealedToPlayer.Add(card);
                cards += card;
            }
            return cards;
        }
        public static void ShowPostflopRanges(List<string> range,ref  DecisionMaker decisionMaker,  ObservableCollection<Omaha> omahaRanges, ObservableCollection<Omaha> showdowns, ObservableCollection<Omaha> draws, ref string board,ref  int step,ref EnumGameType gameType)
        {
            Dictionary<PokerUtil.OmahaHandCategoryType, int> countDictShowdown = new();
            Dictionary<PokerUtil.EnumOmahaDraw, int> countDictDraw = new();
            Dictionary<PokerUtil.OmahaHandCategoryType, int> newCountDictShowdown = new();
            Dictionary<PokerUtil.EnumOmahaDraw, int> newCountDictDraw = new();
            var equityList = decisionMaker.Hero.Strategy.Params.Equities;
            double equity = 0;
            string equityTxt = "";
            if (decisionMaker.Hero.Strategy.Params.ShowdownsDraws.Count != 0)
            {
                foreach (PokerUtil.OmahaHandCategoryType value in Enum.GetValues(typeof(PokerUtil.OmahaHandCategoryType)))
                {
                    countDictShowdown.Add(value, 0);
                }
                foreach (PokerUtil.EnumOmahaDraw enumOmahaDraw in Enum.GetValues(typeof(PokerUtil.EnumOmahaDraw)))
                {
                    countDictDraw.Add(enumOmahaDraw, 0);
                }
                for (int i = 0; i < range.Count; i += step)
                {
                    var item = range[i];
                    var itemDictElement = decisionMaker.Hero.Strategy.Params.ShowdownsDraws[item];
                    if (equityList.ContainsKey(item))
                    {
                        equity = Math.Round(MultiplyBy100(decisionMaker.Hero.Strategy.Params.Equities[item]), 1);
                        equityTxt = equity.ToString() + "%";
                    }
                    else
                    {
                        equity = 0;
                        equityTxt = "";
                    }
                    var newCard1 = CardsManipulate.FirstLetterToBig(item.Substring(0, 2));
                    var newCard2 = CardsManipulate.FirstLetterToBig(item.Substring(2, 2));
                    var newCard3 = CardsManipulate.FirstLetterToBig(item.Substring(4, 2));
                    var newCard4 = CardsManipulate.FirstLetterToBig(item.Substring(6, 2));
                    if (gameType == EnumGameType.Omaha)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            var omh = Omaha.GetOmahaItem(ImageChange.GetImageSource(newCard1), ImageChange.GetImageSource(newCard2), ImageChange.GetImageSource(newCard3), ImageChange.GetImageSource(newCard4), itemDictElement.Showdown, itemDictElement.Draw, itemDictElement.Blocker, equityTxt);
                            omahaRanges.Add(omh);
                            countDictShowdown[itemDictElement.Showdown]++;
                            countDictDraw[itemDictElement.Draw]++;
                        });
                    }
                }
                FillingShowDownsAndDraws(ref countDictShowdown,ref newCountDictShowdown, ref countDictDraw, ref newCountDictDraw, showdowns, draws);
            }
        }
        public static void ShowPostflopRangesFromLaststate(List<string> range, DecisionMaker decisionMaker, ObservableCollection<Omaha> omahaRanges, ObservableCollection<Omaha> showdowns, ObservableCollection<Omaha> draws, string board, int step, EnumGameType gameType)
        {
            Dictionary<PokerUtil.OmahaHandCategoryType, int> countDictShowdown = new();
            Dictionary<PokerUtil.EnumOmahaDraw, int> countDictDraw = new();
            Dictionary<PokerUtil.OmahaHandCategoryType, int> newCountDictShowdown = new();
            Dictionary<PokerUtil.EnumOmahaDraw, int> newCountDictDraw = new();
            var equityList = decisionMaker.Hero.Strategy.Params.Equities;
            double equity = 0;
            string equityTxt = "";
            if (decisionMaker.Hero.Strategy.Params.ShowdownsDraws.Count != 0)
            {
                foreach (PokerUtil.OmahaHandCategoryType value in Enum.GetValues(typeof(PokerUtil.OmahaHandCategoryType)))
                {
                    countDictShowdown.Add(value, 0);
                }
                foreach (PokerUtil.EnumOmahaDraw enumOmahaDraw in Enum.GetValues(typeof(PokerUtil.EnumOmahaDraw)))
                {
                    countDictDraw.Add(enumOmahaDraw, 0);
                }
                for (int i = 0; i < range.Count; i += step)
                {
                    var item = range[i];
                    var itemDictElement = decisionMaker.Hero.Strategy.Params.ShowdownsDraws[item];
                    if (equityList.ContainsKey(item))
                    {
                        equity = Math.Round(MultiplyBy100(decisionMaker.Hero.Strategy.Params.Equities[item]), 1);
                        equityTxt = equity.ToString() + "%";
                    }
                    else
                    {
                        equity = 0;
                        equityTxt = "";
                    }
                    var newCard1 = CardsManipulate.FirstLetterToBig(item.Substring(0, 2));
                    var newCard2 = CardsManipulate.FirstLetterToBig(item.Substring(2, 2));
                    var newCard3 = CardsManipulate.FirstLetterToBig(item.Substring(4, 2));
                    var newCard4 = CardsManipulate.FirstLetterToBig(item.Substring(6, 2));
                    if (gameType == EnumGameType.Omaha)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            var omh = Omaha.GetOmahaItem(ImageChange.GetImageSource(newCard1), ImageChange.GetImageSource(newCard2), ImageChange.GetImageSource(newCard3), ImageChange.GetImageSource(newCard4), itemDictElement.Showdown, itemDictElement.Draw, itemDictElement.Blocker, equityTxt);
                            omahaRanges.Add(omh);
                            countDictShowdown[itemDictElement.Showdown]++;
                            countDictDraw[itemDictElement.Draw]++;
                        });
                    }
                }
                FillingShowDownsAndDraws(ref countDictShowdown, ref newCountDictShowdown, ref countDictDraw, ref newCountDictDraw, showdowns, draws);
            }
        }
        public static void FillingShowDownsAndDraws(ref Dictionary<PokerUtil.OmahaHandCategoryType,int> countDictShowdown, ref Dictionary<PokerUtil.OmahaHandCategoryType, int> newCountDictShowdown,ref Dictionary<PokerUtil.EnumOmahaDraw, int> countDictDraw, ref Dictionary<PokerUtil.EnumOmahaDraw, int> newCountDictDraw, ObservableCollection<Omaha> showdowns, ObservableCollection<Omaha> draws)
        {
            foreach (var item in countDictShowdown)
            {
                var showdownItemKey = item.Key;
                var showdownItemValue = item.Value;
                if (showdownItemValue != 0)
                {
                    newCountDictShowdown.Add(showdownItemKey, showdownItemValue);
                }
            }
            foreach (var item in countDictDraw)
            {
                var drawItemKey = item.Key;
                var drawItemValue = item.Value;
                if (drawItemValue != 0)
                {
                    newCountDictDraw.Add(drawItemKey, drawItemValue);
                }
            }
            int totalHandsShowdown = 0;
            int totalHandsDraws = 0;
            foreach (var item in newCountDictShowdown)
            {
                totalHandsShowdown += item.Value;
            }
            foreach (var item in newCountDictDraw)
            {
                totalHandsDraws += item.Value;
            }
            foreach (var item in newCountDictShowdown)
            {
                var omhHand = Omaha.GetOmahaFilteredShowdowns(item.Key, MainWindowViewModel.GetPercentHands((double)item.Value, totalHandsShowdown), false);
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    showdowns.Add(omhHand);
                });
            }
            foreach (var item in newCountDictDraw)
            {
                var omhHand = Omaha.GetOmahaFilteredDraws(item.Key, MainWindowViewModel.GetPercentHands((double)item.Value, totalHandsShowdown), false);
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    draws.Add(omhHand);
                });
            }
        }
        public static void GetPlayersPLO4(ref List<string> imgCardsDealedToPlayer, ref List<string> CardsDeck,  ref List<string> DealedCardsToPlayer, ref ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                string cardsPlayer = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer,ref CardsDeck,ref DealedCardsToPlayer);
                string card1 = cardsPlayer.Substring(0, 2);
                string card2 = cardsPlayer.Substring(2, 2);
                string card3 = cardsPlayer.Substring(4, 2);
                string card4 = cardsPlayer.Substring(6, 2);
                PlayersToPlay[i].Cards = cardsPlayer;
                PlayersToPlay[i].ImageSourceCard1 = ImageChange.GetImageSource(card1);
                PlayersToPlay[i].ImageSourceCard2 = ImageChange.GetImageSource(card2);
                PlayersToPlay[i].ImageSourceCard3 = ImageChange.GetImageSource(card3);
                PlayersToPlay[i].ImageSourceCard4 = ImageChange.GetImageSource(card4);
            }
        }
        public static void RefreshingGridBetRaise(ObservableCollection<Omaha> OmahaDraws, List<PokerUtil.EnumOmahaDraw> drawFilters, ObservableCollection<Omaha> OmahaShowdowns, List<PokerUtil.OmahaHandCategoryType> showdownFilters, DecisionMaker lastDecisionMaker, List<string> rangesList, int step, DBLib.EnumGameType gameType, ObservableCollection<Omaha> omahaRanges)
        {
            foreach (var itemSelected in OmahaDraws)
            {
                if (itemSelected.IsCheckedDraw)
                {
                    if (!drawFilters.Contains(itemSelected.FilteredDraws))
                    {
                        drawFilters.Add(itemSelected.FilteredDraws);
                    }
                }
            }
            foreach (var itemSelected in OmahaShowdowns)
            {
                if (itemSelected.IsChecked)
                {
                    if (!showdownFilters.Contains(itemSelected.FilteredShowdowns))
                    {
                        showdownFilters.Add(itemSelected.FilteredShowdowns);
                    }
                }
            }
            if (lastDecisionMaker.Hero.Strategy.Params.ShowdownsDraws.Count != 0)
            {
                var equityList = lastDecisionMaker.Hero.Strategy.Params.Equities;
                double equity = 0;
                string equityTxt = "";
                for (int i = 0; i < rangesList.Count; i += step)
                {
                    var item = rangesList[i];
                    var itemDictElement = lastDecisionMaker.Hero.Strategy.Params.ShowdownsDraws[item];
                    if (equityList.ContainsKey(item))
                    {
                        equity = Math.Round(Omaha.MultiplyBy100(lastDecisionMaker.Hero.Strategy.Params.Equities[item]), 1);
                        equityTxt = equity.ToString() + "%";
                    }
                    else
                    {
                        equity = 0;
                        equityTxt = "";
                    }
                    var newCard1 = CardsManipulate.FirstLetterToBig(item.Substring(0, 2));
                    var newCard2 = CardsManipulate.FirstLetterToBig(item.Substring(2, 2));
                    var newCard3 = CardsManipulate.FirstLetterToBig(item.Substring(4, 2));
                    var newCard4 = CardsManipulate.FirstLetterToBig(item.Substring(6, 2));
                    if (gameType == EnumGameType.Omaha)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            var omh = Omaha.GetOmahaItem(ImageChange.GetImageSource(newCard1), ImageChange.GetImageSource(newCard2), ImageChange.GetImageSource(newCard3), ImageChange.GetImageSource(newCard4), itemDictElement.Showdown, itemDictElement.Draw, itemDictElement.Blocker, equityTxt);
                            if ((showdownFilters.Contains(itemDictElement.Showdown) || showdownFilters.Count == 0) && (drawFilters.Contains(itemDictElement.Draw) || drawFilters.Count == 0))
                            {
                                omahaRanges.Add(omh);
                            }
                        });
                    }
                }
            }
        }
        public static void RefreshingGridCheckCall(ObservableCollection<Omaha> OmahaDrawsCheckCall, List<PokerUtil.EnumOmahaDraw> drawFilters, ObservableCollection<Omaha> OmahaShowdownsCheckCall, List<PokerUtil.OmahaHandCategoryType> showdownFilters, DecisionMaker lastDecisionMaker, List<string> rangesList, int step, DBLib.EnumGameType gameType, ObservableCollection<Omaha> omahaRangesMid)
        {
            foreach (var itemSelected in OmahaDrawsCheckCall)
            {
                if (itemSelected.IsCheckedDraw)
                {
                    if (!drawFilters.Contains(itemSelected.FilteredDraws))
                    {
                        drawFilters.Add(itemSelected.FilteredDraws);
                    }
                }
            }
            foreach (var itemSelected in OmahaShowdownsCheckCall)
            {
                if (itemSelected.IsChecked)
                {
                    if (!showdownFilters.Contains(itemSelected.FilteredShowdowns))
                    {
                        showdownFilters.Add(itemSelected.FilteredShowdowns);
                    }
                }
            }
            if (lastDecisionMaker.Hero.Strategy.Params.ShowdownsDraws.Count != 0)
            {
                var equityList = lastDecisionMaker.Hero.Strategy.Params.Equities;
                double equity = 0;
                string equityTxt = "";
                for (int i = 0; i < rangesList.Count; i += step)
                {
                    var item = rangesList[i];
                    var itemDictElement = lastDecisionMaker.Hero.Strategy.Params.ShowdownsDraws[item];
                    if (equityList.ContainsKey(item))
                    {
                        equity = Math.Round(Omaha.MultiplyBy100(lastDecisionMaker.Hero.Strategy.Params.Equities[item]), 1);
                        equityTxt = equity.ToString() + "%";
                    }
                    else
                    {
                        equity = 0;
                        equityTxt = "";
                    }
                    var newCard1 = CardsManipulate.FirstLetterToBig(item.Substring(0, 2));
                    var newCard2 = CardsManipulate.FirstLetterToBig(item.Substring(2, 2));
                    var newCard3 = CardsManipulate.FirstLetterToBig(item.Substring(4, 2));
                    var newCard4 = CardsManipulate.FirstLetterToBig(item.Substring(6, 2));
                    if (gameType == EnumGameType.Omaha)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            var omh = Omaha.GetOmahaItem(ImageChange.GetImageSource(newCard1), ImageChange.GetImageSource(newCard2), ImageChange.GetImageSource(newCard3), ImageChange.GetImageSource(newCard4), itemDictElement.Showdown, itemDictElement.Draw, itemDictElement.Blocker, equityTxt);
                            if ((showdownFilters.Contains(itemDictElement.Showdown) || showdownFilters.Count == 0) && (drawFilters.Contains(itemDictElement.Draw) || drawFilters.Count == 0))
                            {
                                omahaRangesMid.Add(omh);
                            }
                        });
                    }
                }
            }
        }
        public static void RefreshingGridFold(ObservableCollection<Omaha> OmahaDrawsFold, List<PokerUtil.EnumOmahaDraw> drawFilters, ObservableCollection<Omaha> OmahaShowdownsFold, List<PokerUtil.OmahaHandCategoryType> showdownFilters, DecisionMaker lastDecisionMaker, List<string> rangesList, int step, DBLib.EnumGameType gameType, ObservableCollection<Omaha> omahaRangesFold)
        {
            foreach (var itemSelected in OmahaDrawsFold)
            {
                if (itemSelected.IsCheckedDraw)
                {
                    if (!drawFilters.Contains(itemSelected.FilteredDraws))
                    {
                        drawFilters.Add(itemSelected.FilteredDraws);
                    }
                }
            }
            foreach (var itemSelected in OmahaShowdownsFold)
            {
                if (itemSelected.IsChecked)
                {
                    if (!showdownFilters.Contains(itemSelected.FilteredShowdowns))
                    {
                        showdownFilters.Add(itemSelected.FilteredShowdowns);
                    }
                }
            }
            if (lastDecisionMaker.Hero.Strategy.Params.ShowdownsDraws.Count != 0)
            {
                var equityList = lastDecisionMaker.Hero.Strategy.Params.Equities;
                double equity = 0;
                string equityTxt = "";
                for (int i = 0; i < rangesList.Count; i += step)
                {
                    var item = rangesList[i];
                    var itemDictElement = lastDecisionMaker.Hero.Strategy.Params.ShowdownsDraws[item];
                    if (equityList.ContainsKey(item))
                    {
                        equity = Math.Round(Omaha.MultiplyBy100(lastDecisionMaker.Hero.Strategy.Params.Equities[item]), 1);
                        equityTxt = equity.ToString() + "%";
                    }
                    else
                    {
                        equity = 0;
                        equityTxt = "";
                    }
                    var newCard1 = CardsManipulate.FirstLetterToBig(item.Substring(0, 2));
                    var newCard2 = CardsManipulate.FirstLetterToBig(item.Substring(2, 2));
                    var newCard3 = CardsManipulate.FirstLetterToBig(item.Substring(4, 2));
                    var newCard4 = CardsManipulate.FirstLetterToBig(item.Substring(6, 2));
                    if (gameType == EnumGameType.Omaha)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            var omh = Omaha.GetOmahaItem(ImageChange.GetImageSource(newCard1), ImageChange.GetImageSource(newCard2), ImageChange.GetImageSource(newCard3), ImageChange.GetImageSource(newCard4), itemDictElement.Showdown, itemDictElement.Draw, itemDictElement.Blocker, equityTxt);
                            if ((showdownFilters.Contains(itemDictElement.Showdown) || showdownFilters.Count == 0) && (drawFilters.Contains(itemDictElement.Draw) || drawFilters.Count == 0))
                            {
                                omahaRangesFold.Add(omh);
                            }
                        });
                    }
                }
            }
        }
        public static void AddShowdownsRotatedHSCopy(ref HandState rotatedHs, ref ObservableCollection<PlayerViewModel> playersToPlay)
        {
            try
            {
                for (int i = 0; i < playersToPlay.Count; i++)
                {
                    if (playersToPlay[i].InGame)
                    {
                        var index = rotatedHs.Names.IndexOf(playersToPlay[i].Name);
                        rotatedHs.Showdowns[index] = playersToPlay[i].Cards;
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Adding Showdowns RotatedHSCopy: " + ex.ToString(), LogLevel.Error);
            }
        }
        #endregion
    }
}

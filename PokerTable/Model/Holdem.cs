using DBLib;
using DBUserModeling;
using DecisionMaking;
using PokerTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization;
using System.Windows;
using System.Windows.Media;

namespace PokerTable.Model
{
    public enum HandStrength
    {
        Strong, Tpgk, Top, Sec, Draws, Weak, Small, Bdfd, Overcards, Air
    }
    public class Holdem
    {
        #region Members
        private ImageSource img1;
        private ImageSource img2;
        private Brush brush;
        #endregion
        #region Properties
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
        public Brush BrushTxt
        {
            get
            {
                return brush;
            }
            set
            {
                brush = value;
            }
        }
        #endregion
        #region Methods
        public static Holdem GetHoldemHand(ImageSource img1, ImageSource img2, Brush brush)
        {
            Holdem holdem = new();
            holdem.img1 = img1;
            holdem.img2 = img2;
            holdem.brush = brush;
            return holdem;
        }
        /// <summary>
        /// This method Shows PREFLOP RANGES / For Holdem and Omaha
        /// </summary>
        /// <param name="step">Set step from iteration for range list. If range list is 1000, step is 10, than shows every 10th element from range list</param>
        /// <param name="gameType">Type of game</param>
        /// <param name="rangeImageSources1">ImageSources for card 1 preflop bet/raise from range list - for holdem and for omaha</param>
        /// <param name="rangeImageSources2">ImageSources for card 2 preflop bet/raise from range list - for holdem and for omaha</param>
        /// <param name="rangeImageSources3">ImageSources for card 3 preflop bet/raise from range list - only for omaha</param>
        /// <param name="rangeImageSources4">ImageSources for card 4 preflop bet/raise from range list - only for omaha</param>
        /// <param name="rangeImageSourcesMiddle1">ImageSources for card 1 preflop check/Call from range list - for holdem and for omaha</param>
        /// <param name="rangeImageSourcesMiddle2">ImageSources for card 2 preflop check/Call from range list - for holdem and for omaha</param>
        /// <param name="rangeImageSourcesMiddle3">ImageSources for card 3 preflop check/Call from range list - only for omaha</param>
        /// <param name="rangeImageSourcesMiddle4">ImageSources for card 4 preflop check/Call from range list - only for omaha</param>
        /// <param name="range">Range for show</param>
        /// <param name="action">Action played</param>
        /// <param name="isGamePaused">If game is paused, than ranges is visible, if game isn't paused than ranges is collapsed</param>
        public static void ShowPreflopRange(int step, EnumGameType gameType, ObservableCollection<ImageSource> rangeImageSources1, ObservableCollection<ImageSource> rangeImageSources2, ObservableCollection<ImageSource> rangeImageSources3, ObservableCollection<ImageSource> rangeImageSources4, ObservableCollection<ImageSource> rangeImageSourcesMiddle1, ObservableCollection<ImageSource> rangeImageSourcesMiddle2, ObservableCollection<ImageSource> rangeImageSourcesMiddle3, ObservableCollection<ImageSource> rangeImageSourcesMiddle4, List<string> range, EnumDecisionType action, bool isGamePaused)
        {
            try
            {
                if (isGamePaused)
                {
                    for (int i = 0; i < range.Count; i += step)
                    {
                        var item = range[i];
                        var card1 = item.Substring(0, 2);
                        var smallLetter = card1.Substring(0, 1);
                        var bigLetter = smallLetter.ToUpper();
                        var newCard1 = bigLetter + card1.Substring(1, 1);
                        var card2 = item.Substring(2, 2);
                        var smallLetter2 = card2.Substring(0, 1);
                        var bigLetter2 = smallLetter2.ToUpper();
                        var newCard2 = bigLetter2 + card2.Substring(1, 1);
                        if (gameType == EnumGameType.CashGame)
                        {
                            if (action == EnumDecisionType.BET || action == EnumDecisionType.RAISE)
                            {
                                App.Current.Dispatcher.Invoke((System.Action)delegate
                                {
                                    rangeImageSources1.Add(ImageChange.GetImageSource(newCard1));
                                    rangeImageSources2.Add(ImageChange.GetImageSource(newCard2));
                                });
                            }
                            else
                            {
                                App.Current.Dispatcher.Invoke((System.Action)delegate
                                {
                                    rangeImageSourcesMiddle1.Add(ImageChange.GetImageSource(newCard1));
                                    rangeImageSourcesMiddle2.Add(ImageChange.GetImageSource(newCard2));
                                });
                            }
                        }
                        else
                        {
                            var card3 = item.Substring(4, 2);
                            var smallLetter3 = card3.Substring(0, 1);
                            var bigLetter3 = smallLetter3.ToUpper();
                            var newCard3 = bigLetter3 + card3.Substring(1, 1);
                            var card4 = item.Substring(6, 2);
                            var smallLetter4 = card4.Substring(0, 1);
                            var bigLetter4 = smallLetter4.ToUpper();
                            var newCard4 = bigLetter4 + card4.Substring(1, 1);
                            if (action == EnumDecisionType.BET || action == EnumDecisionType.RAISE)
                            {
                                App.Current.Dispatcher.Invoke((System.Action)delegate
                                {
                                    rangeImageSources1.Add(ImageChange.GetImageSource(newCard1));
                                    rangeImageSources2.Add(ImageChange.GetImageSource(newCard2));
                                    rangeImageSources3.Add(ImageChange.GetImageSource(newCard3));
                                    rangeImageSources4.Add(ImageChange.GetImageSource(newCard4));
                                });
                            }
                            else
                            {
                                App.Current.Dispatcher.Invoke((System.Action)delegate
                                {
                                    rangeImageSourcesMiddle1.Add(ImageChange.GetImageSource(newCard1));
                                    rangeImageSourcesMiddle2.Add(ImageChange.GetImageSource(newCard2));
                                    rangeImageSourcesMiddle3.Add(ImageChange.GetImageSource(newCard3));
                                    rangeImageSourcesMiddle4.Add(ImageChange.GetImageSource(newCard4));
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Showing Preflop Range: " + ex.ToString(), LogLevel.Error);
            }
        }
        public static Brush GetColor(HandStrength handStrength)
        {
            Brush brush = null;
            if (handStrength == HandStrength.Strong)
            {
                brush = Brushes.Red;
            }
            else if (handStrength == HandStrength.Tpgk)
            {
                brush = Brushes.Orange;
            }
            else if (handStrength == HandStrength.Top)
            {
                brush = Brushes.Pink;
            }
            else if (handStrength == HandStrength.Sec)
            {
                brush = Brushes.Brown;
            }
            else if (handStrength == HandStrength.Draws)
            {
                brush = Brushes.Blue;
            }
            else if (handStrength == HandStrength.Weak)
            {
                brush = Brushes.Coral;
            }
            else if (handStrength == HandStrength.Small)
            {
                brush = Brushes.Purple;
            }
            else if (handStrength == HandStrength.Bdfd)
            {
                brush = Brushes.Green;
            }
            else if (handStrength == HandStrength.Overcards)
            {
                brush = Brushes.GreenYellow;
            }
            else if (handStrength == HandStrength.Air)
            {
                brush = Brushes.Indigo;
            }
            else
            {
                brush = Brushes.Black;
            }
            return brush;
        }
        /// <summary>
        /// Dealing cards to players - Holdem 
        /// </summary>
        public static string DealCardsToPlayerNLH(List<string> imgCardsDealedToPlayer, List<string> CardsDeck, List<string> DealedCardsToPlayer)
        {
            imgCardsDealedToPlayer = new();
            var cardsForPlayer = Card.RandomCards(2, ref CardsDeck);
            var cards = "";
            foreach (string card in cardsForPlayer)
            {
                DealedCardsToPlayer.Add(card);
                imgCardsDealedToPlayer.Add(card);
                cards += card;
            }
            return cards;
        }
        /// <summary>
        /// This method servers to Populate Grid Ranges with description of Hand Strength and sets color of hand strength
        /// </summary>
        /// <param name="whole_range">All Ranges</param>
        /// <param name="board">Current board</param>
        /// <param name="phase">Current phase</param>
        /// <param name="rangeTextList">Text for ranges</param>
        /// <param name="dictHandStrenght">Dictionary with HandStrength</param>
        public static void PopulateGridRange(List<string> whole_range, string board, EnumPhase phase, ObservableCollection<RangePercents> rangeTextList, Dictionary<string, HandStrength> dictHandStrenght)
        {
            int total = whole_range.Count;
            List<string> rangeList = whole_range.ToList();
            whole_range.Clear();
            List<string> strong_range = RangeQuery.GetTwoPairPlus(rangeList, board);
            RangeQuery.RemoveDuplicatesFromSecondRange(strong_range, rangeList);
            double percStrong = strong_range.Count / (double)total;
            foreach (var item in strong_range)
            {
                dictHandStrenght.Add(item, HandStrength.Strong);
                whole_range.Add(item);
            }
            List<string> tpgk_range = RangeQuery.GetTpgkPlus(rangeList, board);
            RangeQuery.RemoveDuplicatesFromSecondRange(tpgk_range, rangeList);
            double percTpgk = tpgk_range.Count / (double)total;
            foreach (var item in tpgk_range)
            {
                dictHandStrenght.Add(item, HandStrength.Tpgk);
                whole_range.Add(item);
            }
            List<string> top_pair_range = RangeQuery.GetTopPairPlus(rangeList, board);
            RangeQuery.RemoveDuplicatesFromSecondRange(top_pair_range, rangeList);
            double percTopPairRange = top_pair_range.Count / (double)total;
            foreach (var item in top_pair_range)
            {
                dictHandStrenght.Add(item, HandStrength.Top);
                whole_range.Add(item);
            }
            List<string> sec_pair_range = RangeQuery.GetMidPairPlus(rangeList, board);
            RangeQuery.RemoveDuplicatesFromSecondRange(sec_pair_range, rangeList);
            double percSecPairRange = sec_pair_range.Count / (double)total;
            foreach (var item in sec_pair_range)
            {
                dictHandStrenght.Add(item, HandStrength.Sec);
                whole_range.Add(item);
            }
            List<string> draws = RangeQuery.GetDrawsFromRange(rangeList, board, 4);
            RangeQuery.RemoveDuplicatesFromSecondRange(draws, rangeList);
            double percDraws = draws.Count / (double)total;
            foreach (var item in draws)
            {
                dictHandStrenght.Add(item, HandStrength.Draws);
                whole_range.Add(item);
            }
            List<string> weak_pairs_range = RangeQuery.GetWeakpairPlus(rangeList, board);
            RangeQuery.RemoveDuplicatesFromSecondRange(weak_pairs_range, rangeList);
            double percWeakPairsRange = weak_pairs_range.Count / (double)total;
            foreach (var item in weak_pairs_range)
            {
                dictHandStrenght.Add(item, HandStrength.Weak);
                whole_range.Add(item);
            }
            List<string> small_pp_range = RangeQuery.GetSmallPP(rangeList, board);
            RangeQuery.RemoveDuplicatesFromSecondRange(small_pp_range, rangeList);
            double percSmallPPRange = small_pp_range.Count / (double)total;
            foreach (var item in small_pp_range)
            {
                dictHandStrenght.Add(item, HandStrength.Small);
                whole_range.Add(item);
            }
            int countBefore = rangeList.Count;
            List<string> bdfd_range = RangeQuery.GetAllBackdoorsFromRange(rangeList, board, phase != EnumPhase.Flop);
            RangeQuery.RemoveDuplicatesFromSecondRange(bdfd_range, rangeList);
            double percBdfdRange = bdfd_range.Count / (double)total;
            foreach (var item in bdfd_range)
            {
                dictHandStrenght.Add(item, HandStrength.Bdfd);
                whole_range.Add(item);
            }
            List<string> overcards_range = RangeQuery.GetOvercards(rangeList, board);
            RangeQuery.RemoveDuplicatesFromSecondRange(overcards_range, rangeList);
            double percOvercardsRange = overcards_range.Count / (double)total;
            foreach (var item in overcards_range)
            {
                dictHandStrenght.Add(item, HandStrength.Overcards);
                whole_range.Add(item);
            }
            List<string> airRange = new(rangeList);
            RangeQuery.RemoveDuplicatesFromSecondRange(airRange, rangeList);
            double percAirRange = airRange.Count / (double)total;
            foreach (var item in airRange)
            {
                dictHandStrenght.Add(item, HandStrength.Air);
                whole_range.Add(item);
            }
            double sum = MainWindowViewModel.PercentWithOneDecimal(percStrong) + MainWindowViewModel.PercentWithOneDecimal(percTpgk) + MainWindowViewModel.PercentWithOneDecimal(percTopPairRange) + MainWindowViewModel.PercentWithOneDecimal(percSecPairRange) + MainWindowViewModel.PercentWithOneDecimal(percDraws) + MainWindowViewModel.PercentWithOneDecimal(percWeakPairsRange) + MainWindowViewModel.PercentWithOneDecimal(percSmallPPRange) + MainWindowViewModel.PercentWithOneDecimal(percBdfdRange) + MainWindowViewModel.PercentWithOneDecimal(percOvercardsRange) + MainWindowViewModel.PercentWithOneDecimal(percAirRange);
            if (total != 0)
            {
                RangePercents.RangePercentsCollection(rangeTextList, total, percStrong, percTpgk, percTopPairRange, percSecPairRange, percDraws, percWeakPairsRange, percSmallPPRange, percBdfdRange, percOvercardsRange, percAirRange);
            }
        }
        /// <summary>
        /// This method shows ranges cards in grid
        /// and add color to hand
        /// </summary>
        /// <param name="rangesList">List of ranges from decisionMaker.</param>
        /// <param name="holdems">Observable collection of Holdem class / which list will be fill.</param>
        /// <param name="rangePercents">which range list will be fill</param>
        public static void ShowPostflopRanges(List<string> rangesList, ObservableCollection<Holdem> holdems, ref ObservableCollection<RangePercents> rangePercents, string board, EnumPhase phase, int step, EnumGameType gameType)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                holdems.Clear();
            });
            Dictionary<string, HandStrength> dictHandStrenght = new();            
            if (rangesList.Count != 0)
            {
                Holdem.PopulateGridRange(rangesList, board, phase, rangePercents, dictHandStrenght);
            }
            for (int i = 0; i < rangesList.Count; i += step)
            {
                var item = rangesList[i];
                var itemDictElement = dictHandStrenght[item];

                var card1 = item.Substring(0, 2);
                var smallLetter = card1.Substring(0, 1);
                var bigLetter = smallLetter.ToUpper();
                var newCard1 = bigLetter + card1.Substring(1, 1);

                var card2 = item.Substring(2, 2);
                var smallLetter2 = card2.Substring(0, 1);
                var bigLetter2 = smallLetter2.ToUpper();
                var newCard2 = bigLetter2 + card2.Substring(1, 1);
                if (gameType == EnumGameType.CashGame)
                {
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        var hand = Holdem.GetHoldemHand(ImageChange.GetImageSource(newCard1), ImageChange.GetImageSource(newCard2), Holdem.GetColor(itemDictElement));
                        holdems.Add(hand);
                    });
                }
            }
        }
        public static void GetPlayersNLH(List<string> imgCardsDealedToPlayer, List<string> CardsDeck, List<string> DealedCardsToPlayer, ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                string cardsPlayer = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
                string card1 = cardsPlayer.Substring(0, 2);
                string card2 = cardsPlayer.Substring(2, 2);
                PlayersToPlay[i].Cards = cardsPlayer;
                PlayersToPlay[i].ImageSourceCard1 = ImageChange.GetImageSource(card1);
                PlayersToPlay[i].ImageSourceCard2 = ImageChange.GetImageSource(card2);
            }
        }
       
        #endregion
    }
}

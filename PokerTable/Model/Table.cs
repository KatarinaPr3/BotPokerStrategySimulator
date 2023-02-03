using DBLib;
using PokerTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class Table : ViewModelBase
    {
        #region Methods
        public static void DealCardsToTable(ref List<ImageSource> imageSourceCardOnTable1,ref List<string> CardsDeck,ref ObservableCollection<string> cardsOnTable,ref ObservableCollection<string> CardsOnTable,ref List<ImageSource> ImageSourcesCardsOnTable)
        {
            imageSourceCardOnTable1 = new();
            List<string> cardsOnTableList = Card.RandomCards(5, ref CardsDeck);
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (string card in cardsOnTableList)
            {
                cardsOnTable.Add(card);
            }
            foreach (var item in CardsOnTable)
            {
                ImageSourcesCardsOnTable.Add(ImageChange.GetImageSource(item));
            }
        }
        public static void SetNewHand(ref Visibility BetRaiseOmahaVisibility, ref  Visibility CheckCallOmahaVisibility, ref Visibility FoldOmahaVisibility, ref  bool phaseOver, ref bool isHandStart, ref double sumbets,ref double ThresholdRebuy,ref double BigBlind, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref Visibility Card1Visibility,ref Visibility Card2Visibility,ref Visibility Card3Visibility,ref Visibility Card4Visibility,ref Visibility Card5Visibility,ref double PotSize,ref List<bool> didPlayersPlay, ref EnumPhase phase, ref List<ImageSource> ImageSourcesCardsOnTable,ref List<string> DealedCardsToPlayer,ref ObservableCollection<string> cardsOnTable,ref  List<string> CardsDeck,ref string GameChoosen,ref List<string> imgCardsDealedToPlayer,ref List<ImageSource> imageSourceCardOnTable1,ref ObservableCollection<string> CardsOnTable, ref int handCount, ref int indexAct, bool isNewSession)
        {
            
            BetRaiseOmahaVisibility = Visibility.Collapsed;
            CheckCallOmahaVisibility = Visibility.Collapsed;
            FoldOmahaVisibility = Visibility.Collapsed;
            phaseOver = false;
            isHandStart = false;
            sumbets = 0;
            Player.CheckPlayersBalance(ref ThresholdRebuy, ref BigBlind, ref PlayersToPlay);
            Card1Visibility = Visibility.Collapsed;
            Card2Visibility = Visibility.Collapsed;
            Card3Visibility = Visibility.Collapsed;
            Card4Visibility = Visibility.Collapsed;
            Card5Visibility = Visibility.Collapsed;           
            PotSize = 0;
            didPlayersPlay = new();
            phase = EnumPhase.Preflop;
            
            foreach (var item in PlayersToPlay)
            {
                item.IsWinner = false;

                if (item.VisibilityForCards == Visibility.Collapsed)
                {
                    item.InGame = true;
                    item.VisibilityForCards = Visibility.Visible;
                    item.IsMyTurn = false;
                }
                if (item.IsMyTurn)
                {
                    item.IsMyTurn = false;
                }
            }
            foreach (var item in PlayersToPlay)
            {
                didPlayersPlay.Add(false);
            }
            ImageSourcesCardsOnTable = new();
            DealedCardsToPlayer = new();
            cardsOnTable = new();
            CardsDeck = Card.Deck();
            if (GameChoosen == GameEnums.NLH.ToString())
            {
                Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, PlayersToPlay);
            }
            else
            {
                Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer,ref  CardsDeck,ref DealedCardsToPlayer, ref PlayersToPlay);
            }
            Table.DealCardsToTable(ref imageSourceCardOnTable1,ref CardsDeck, ref cardsOnTable, ref CardsOnTable,ref ImageSourcesCardsOnTable);
            handCount++;
            int numberForDealer;
            int smallBlind;
            int bigBlind;
            int numForPlayerIsMyTurn;
            int indexForAct;
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                PlayersToPlay[i].BetSize = 0;
            }
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].IsDealer == true)
                {
                    PlayersToPlay[i].IsDealer = false;
                    PlayersToPlay[i].VisibilityDealer = Visibility.Collapsed;
                    numberForDealer = i + 1;
                    if (numberForDealer == PlayersToPlay.Count)
                    {
                        numberForDealer = 0;

                    }
                    smallBlind = numberForDealer + 1;
                    if (smallBlind == PlayersToPlay.Count)
                    {
                        smallBlind = 0;
                    }
                    bigBlind = smallBlind + 1;
                    if (bigBlind == PlayersToPlay.Count)
                    {
                        bigBlind = 0;
                    }
                    numForPlayerIsMyTurn = bigBlind + 1;
                    if (numForPlayerIsMyTurn == PlayersToPlay.Count)
                    {
                        numForPlayerIsMyTurn = 0;
                    }
                    if (PlayersToPlay.Count == 2)
                    {
                        smallBlind = numberForDealer;

                        bigBlind = numForPlayerIsMyTurn;
                        numForPlayerIsMyTurn = numberForDealer;
                    }
                    PlayersToPlay[numForPlayerIsMyTurn].IsMyTurn = true;
                    indexForAct = numForPlayerIsMyTurn;
                    indexAct = indexForAct;
                    PlayersToPlay[numberForDealer].IsDealer = true;
                    PlayersToPlay[numberForDealer].VisibilityDealer = Visibility.Visible;
                    //PlayersToPlay[smallBlind].BetSize = 0.5;
                    //PlayersToPlay[bigBlind].BetSize = 1;
                    var smallBlinfRef = BigBlind / 2;
                    if (!isNewSession)
                    {
                        SetBet(ref smallBlind, ref smallBlinfRef, ref PlayersToPlay);
                        SetBet(ref bigBlind, ref BigBlind, ref PlayersToPlay);
                    }                  
                    break;
                }
            }
            foreach (var item in PlayersToPlay)
            {
                if (item.BetSize == 0)
                {
                    if (item.BetVisibility == Visibility.Visible)
                    {
                        item.BetVisibility = Visibility.Collapsed;
                    }
                }
            }
        }
        /// <summary>
        /// This method setting small or big blind
        /// </summary>      
        public static void SetBet(ref int playerIndex,ref double betSize,ref ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            PlayersToPlay[playerIndex].BetSize = betSize;
            PlayersToPlay[playerIndex].Balance = PlayersToPlay[playerIndex].Balance - betSize;
        }
        #endregion
    }
}

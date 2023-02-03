using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DBLib;
using PokerTable.ViewModel;

namespace PokerTable.Model
{
    public enum EnumProfile
    {
        Regular1,
        Regular2,
        Regular3,
        Regular4,
        AgroFish,
        VeryAgroFish,
        PassiveFish,
        VeryPassiveFish
    }
    [Serializable]
    public class Player
    {
        #region Members
        private string cards, name;
        private double balance, betSize;
        private bool isDealer, isVisible;
        public ImageSource imgSource1, imgSource2, imgSource3, imgSource4;
        private string card1, card2, card3, card4;
        private Visibility dealerVisibility;
        private int rowVisibility, columnVisibility;
        private string strategy;
        private bool inGame;
        private bool isMyTurn;
        private int rowVisibilityBtn;
        private int columnVisibilityBtn;
        private int rowVisibilityDealer;
        private int columnVisibilityDealer;
        private string action;
        private bool isWinner;
        private bool isBot;
        private ImageSource botHumanImg;
        private int rowVisibilityStats;
        private int columnVisibilityStats;
        private int sessionHands;
        #endregion
        #region Properties
        public int SessionHands
        {
            get
            {
                return sessionHands;
            }
            set
            {
                sessionHands = value;
            }
        }
        public ImageSource BotHumanImg
        {
            get
            {
                return botHumanImg;
            }
            set 
            { 
                botHumanImg = value;
            }
        }
        public bool IsBot
        {
            get
            {
                return isBot;
            }
            set
            {
                isBot = value;
            }
        }
        public bool IsWinner
        {
            get
            {
                return isWinner;
            }
            set
            {
                isWinner = value;
            }
        }
        public string Action
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }
        public bool IsMyTurn
        {
            get
            {
                return isMyTurn;
            }
            set
            {
                isMyTurn = value;
            }
        }
        public bool InGame
        {
            get
            {
                return inGame;
            }
            set
            {
                inGame = value;
            }
        }
        public string StrategyProfile
        {
            get
            {
                return strategy;
            }
            set
            {
                strategy = value;            
            }
        }            
        public string Cards 
        {
            get
            {
                return cards;
            }
            set
            {
                cards = value;          
            }
        }
        public string Card1
        {
            get
            {
                return card1;
            }
            set
            {
                card1 = value;   
            }
        }
        public string Card2
        {
            get
            {
                return card2;
            }
            set
            {
                card2 = value;
            }
        }
        public string Card3
        {
            get
            {
                return card3;
            }
            set
            {
                card3 = value;
            }
        }
        public string Card4
        {
            get
            {
                return card4;
            }
            set
            {
                card4 = value;
            }
        }
        public double Balance 
        {
            get
            {
                return Math.Round(balance, 2);
            }
            set
            {
                balance = Math.Round(value,2);
            }
        }
        public double BetSize 
        {
            get
            {
                return betSize;
            }
            set
            {
                betSize = value;
            }
        }
        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public bool IsDealer {
            get
            {
                return isDealer;
            }
            set
            {
                isDealer = value;
            }
        }
        public bool IsVisible {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
            }
        }
        public Visibility DealerVisibility
        {
            get
            {
                return dealerVisibility;
            }
            set
            {
                dealerVisibility = value;
            }
        }
        public ImageSource ImageSource1 
        {
            get
            {
                return imgSource1;
            }
            set
            {
                imgSource1 = value;
            }
        }
        public ImageSource ImageSource2
        {
            get
            {
                return imgSource2;
            }
            set
            {
                imgSource2 = value;
            }
        }
        public ImageSource ImageSource3
        {
            get
            {
                return imgSource3;
            }
            set
            {
                imgSource3 = value;
            }
        }
        public ImageSource ImageSource4
        {
            get
            {
                return imgSource4;
            }
            set
            {
                imgSource4 = value;
            }
        }
        public int RowVisibility
        {
            get
            {
                return rowVisibility;
            }
            set
            {
                rowVisibility = value;
            }
        }
        public int ColumnVisibility
        {
            get
            {
                return columnVisibility;
            }
            set
            {
                columnVisibility = value;
            }
        }
        public int RowButton
        {
            get
            {
                return rowVisibilityBtn;
            }
            set
            {
                rowVisibilityBtn = value;
            }
        }
        public int ColumnButton
        {
            get
            {
                return columnVisibilityBtn;
            }
            set
            {
                columnVisibilityBtn = value;
            }
        }
        public int RowDealer
        {
            get
            {
                return rowVisibilityDealer;
            }
            set
            {
                rowVisibilityDealer = value;
            }
        }
        public int ColumnDealer
        {
            get
            {
                return columnVisibilityDealer;
            }
            set
            {
                columnVisibilityDealer = value;
            }
        }
        public int RowVisibilityStats
        {
            get
            {
                return rowVisibilityStats;
            }
            set
            {
                rowVisibilityStats = value;
            }
        }
        public int ColumnVisibilityStats
        {
            get
            {
                return columnVisibilityStats;
            }
            set
            {
                columnVisibilityStats = value;
            }
        }
        #endregion
        #region Methods
        #region Setting Default Values Players
        private static void SettingDefaultsPlayer1()
        {
            if (Properties.Settings.Default.Player1Name == "")
            {
                Properties.Settings.Default.Player1Name = "Player1";
            }
            if (Properties.Settings.Default.Player1Balance == 0)
            {
                Properties.Settings.Default.Player1Balance = 200;
            }
            if (Properties.Settings.Default.Player1StrategyProfile == "")
            {
                Properties.Settings.Default.Player1StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer2()
        {
            if (Properties.Settings.Default.Player2Name == "")
            {
                Properties.Settings.Default.Player2Name = "Player2";
            }
            if (Properties.Settings.Default.Player2Balance == 0)
            {
                Properties.Settings.Default.Player2Balance = 200;
            }
            if (Properties.Settings.Default.Player2StrategyProfile == "")
            {
                Properties.Settings.Default.Player2StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer3()
        {
            if (Properties.Settings.Default.Player3Name == "")
            {
                Properties.Settings.Default.Player3Name = "Player3";
            }
            if (Properties.Settings.Default.Player3Balance == 0)
            {
                Properties.Settings.Default.Player3Balance = 200;
            }
            if (Properties.Settings.Default.Player3StrategyProfile == "")
            {
                Properties.Settings.Default.Player3StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer4()
        {
            if (Properties.Settings.Default.Player4Name == "")
            {
                Properties.Settings.Default.Player4Name = "Player4";
            }
            if (Properties.Settings.Default.Player4Balance == 0)
            {
                Properties.Settings.Default.Player4Balance = 200;
            }
            if (Properties.Settings.Default.Player4StrategyProfile == "")
            {
                Properties.Settings.Default.Player4StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer5()
        {
            if (Properties.Settings.Default.Player5Name == "")
            {
                Properties.Settings.Default.Player5Name = "Player5";
            }
            if (Properties.Settings.Default.Player5Balance == 0)
            {
                Properties.Settings.Default.Player5Balance = 200;
            }
            if (Properties.Settings.Default.Player5StrategyProfile == "")
            {
                Properties.Settings.Default.Player5StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer6()
        {
            if (Properties.Settings.Default.Player6Name == "")
            {
                Properties.Settings.Default.Player6Name = "Player6";
            }
            if (Properties.Settings.Default.Player6Balance == 0)
            {
                Properties.Settings.Default.Player6Balance = 200;
            }
            if (Properties.Settings.Default.Player6StrategyProfile == "")
            {
                Properties.Settings.Default.Player6StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer7()
        {
            if (Properties.Settings.Default.Player7Name == "")
            {
                Properties.Settings.Default.Player7Name = "Player7";
            }
            if (Properties.Settings.Default.Player7Balance == 0)
            {
                Properties.Settings.Default.Player7Balance = 200;
            }
            if (Properties.Settings.Default.Player7StrategyProfile == "")
            {
                Properties.Settings.Default.Player7StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer8()
        {
            if (Properties.Settings.Default.Player8Name == "")
            {
                Properties.Settings.Default.Player8Name = "Player8";
            }
            if (Properties.Settings.Default.Player8Balance == 0)
            {
                Properties.Settings.Default.Player8Balance = 200;
            }
            if (Properties.Settings.Default.Player8StrategyProfile == "")
            {
                Properties.Settings.Default.Player8StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        private static void SettingDefaultsPlayer9()
        {
            if (Properties.Settings.Default.Player9Name == "")
            {
                Properties.Settings.Default.Player9Name = "Player9";
            }
            if (Properties.Settings.Default.Player9Balance == 0)
            {
                Properties.Settings.Default.Player9Balance = 200;
            }
            if (Properties.Settings.Default.Player9StrategyProfile == "")
            {
                Properties.Settings.Default.Player9StrategyProfile = "Regular1";
            }
            Properties.Settings.Default.Save();
        }
        #endregion
        /// <summary>
        /// Thim method set all values Omaha Players, it's called in MainWindowViewModel Constructor
        /// </summary>    
        public static void GettingPLO4Players(ref List<string> imgCardsDealedToPlayer,ref List<string> CardsDeck,ref List<string> DealedCardsToPlayer, ref PlayerViewModel player1, ref PlayerViewModel player2, ref PlayerViewModel player3, ref PlayerViewModel player4, ref PlayerViewModel player5, ref PlayerViewModel player6, ref PlayerViewModel player7, ref PlayerViewModel player8, ref PlayerViewModel player9,ref bool IsDealer)
        {
            string cardsPlayer1 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer,ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player1 = cardsPlayer1.Substring(0, 2);
            string card2Player1 = cardsPlayer1.Substring(2, 2);
            string card3Player1 = cardsPlayer1.Substring(4, 2);
            string card4Player1 = cardsPlayer1.Substring(6, 2);
            string cardsPlayer2 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player2 = cardsPlayer2.Substring(0, 2);
            string card2Player2 = cardsPlayer2.Substring(2, 2);
            string card3Player2 = cardsPlayer2.Substring(4, 2);
            string card4Player2 = cardsPlayer2.Substring(6, 2);
            string cardsPlayer3 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player3 = cardsPlayer3.Substring(0, 2);
            string card2Player3 = cardsPlayer3.Substring(2, 2);
            string card3Player3 = cardsPlayer3.Substring(4, 2);
            string card4Player3 = cardsPlayer3.Substring(6, 2);
            string cardsPlayer4 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player4 = cardsPlayer4.Substring(0, 2);
            string card2Player4 = cardsPlayer4.Substring(2, 2);
            string card3Player4 = cardsPlayer4.Substring(4, 2);
            string card4Player4 = cardsPlayer4.Substring(6, 2);
            string cardsPlayer5 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player5 = cardsPlayer5.Substring(0, 2);
            string card2Player5 = cardsPlayer5.Substring(2, 2);
            string card3Player5 = cardsPlayer5.Substring(4, 2);
            string card4Player5 = cardsPlayer5.Substring(6, 2);
            string cardsPlayer6 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player6 = cardsPlayer6.Substring(0, 2);
            string card2Player6 = cardsPlayer6.Substring(2, 2);
            string card3Player6 = cardsPlayer6.Substring(4, 2);
            string card4Player6 = cardsPlayer6.Substring(6, 2);
            string cardsPlayer7 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player7 = cardsPlayer7.Substring(0, 2);
            string card2Player7 = cardsPlayer7.Substring(2, 2);
            string card3Player7 = cardsPlayer7.Substring(4, 2);
            string card4Player7 = cardsPlayer7.Substring(6, 2);
            string cardsPlayer8 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player8 = cardsPlayer8.Substring(0, 2);
            string card2Player8 = cardsPlayer8.Substring(2, 2);
            string card3Player8 = cardsPlayer8.Substring(4, 2);
            string card4Player8 = cardsPlayer8.Substring(6, 2);
            string cardsPlayer9 = Omaha.DealCardsToPlayerPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer);
            string card1Player9 = cardsPlayer9.Substring(0, 2);
            string card2Player9 = cardsPlayer9.Substring(2, 2);
            string card3Player9 = cardsPlayer9.Substring(4, 2);
            string card4Player9 = cardsPlayer9.Substring(6, 2);            
            SettingDefaultsPlayer1();
            SettingDefaultsPlayer2();
            SettingDefaultsPlayer3();
            SettingDefaultsPlayer4();
            SettingDefaultsPlayer5();
            SettingDefaultsPlayer6();
            SettingDefaultsPlayer7();
            SettingDefaultsPlayer8();
            SettingDefaultsPlayer9();
            player1 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player1Name, Properties.Settings.Default.Player1Balance, 0, cardsPlayer1, IsDealer, ImageChange.GetImageSource(card1Player1), ImageChange.GetImageSource(card2Player1), ImageChange.GetImageSource(card3Player1), ImageChange.GetImageSource(card4Player1), Visibility.Collapsed, 1, 2, Properties.Settings.Default.Player1StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player1IsBot, 0, 1, 1, 0, HorizontalAlignment.Right, VerticalAlignment.Center);
            player2 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player2Name, Properties.Settings.Default.Player2Balance, 0, cardsPlayer2, IsDealer, ImageChange.GetImageSource(card1Player2), ImageChange.GetImageSource(card2Player2), ImageChange.GetImageSource(card3Player2), ImageChange.GetImageSource(card4Player2), Visibility.Collapsed, 1, 0, Properties.Settings.Default.Player2StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player2IsBot, 0, 1, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
            player3 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player3Name, Properties.Settings.Default.Player3Balance, 0, cardsPlayer3, IsDealer, ImageChange.GetImageSource(card1Player3), ImageChange.GetImageSource(card2Player3), ImageChange.GetImageSource(card3Player3), ImageChange.GetImageSource(card4Player3), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player3StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player3IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player4 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player4Name, Properties.Settings.Default.Player4Balance, 0, cardsPlayer4, IsDealer, ImageChange.GetImageSource(card1Player4), ImageChange.GetImageSource(card2Player4), ImageChange.GetImageSource(card3Player4), ImageChange.GetImageSource(card4Player4), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player4StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player4IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player5 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player5Name, Properties.Settings.Default.Player5Balance, 0, cardsPlayer5, IsDealer, ImageChange.GetImageSource(card1Player5), ImageChange.GetImageSource(card2Player5), ImageChange.GetImageSource(card3Player5), ImageChange.GetImageSource(card4Player5), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player5StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player5IsBot, 1, 0, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
            player6 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player6Name, Properties.Settings.Default.Player6Balance, 0, cardsPlayer6, IsDealer, ImageChange.GetImageSource(card1Player6), ImageChange.GetImageSource(card2Player6), ImageChange.GetImageSource(card3Player6), ImageChange.GetImageSource(card4Player6), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player6StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player6IsBot, 1, 0, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
            player7 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player7Name, Properties.Settings.Default.Player7Balance, 0, cardsPlayer7, IsDealer, ImageChange.GetImageSource(card1Player7), ImageChange.GetImageSource(card2Player7), ImageChange.GetImageSource(card3Player7), ImageChange.GetImageSource(card4Player7), Visibility.Collapsed, 0, 1, Properties.Settings.Default.Player7StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player7IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player8 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player8Name, Properties.Settings.Default.Player8Balance, 0, cardsPlayer8, IsDealer, ImageChange.GetImageSource(card1Player8), ImageChange.GetImageSource(card2Player8), ImageChange.GetImageSource(card3Player8), ImageChange.GetImageSource(card4Player8), Visibility.Collapsed, 0, 1, Properties.Settings.Default.Player8StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player8IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player9 = PlayerViewModel.GetPlayerPLO4(false, Properties.Settings.Default.Player9Name, Properties.Settings.Default.Player9Balance, 0, cardsPlayer9, IsDealer, ImageChange.GetImageSource(card1Player9), ImageChange.GetImageSource(card2Player9), ImageChange.GetImageSource(card3Player9), ImageChange.GetImageSource(card4Player9), Visibility.Collapsed, 0, 1, Properties.Settings.Default.Player9StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player9IsBot, 1, 0, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
        }
        /// <summary>
        /// Thim method set all values HOLDEM Players, it's called in MainWindowViewModel Constructor
        /// </summary> 
        public static void GettingNLHPlayers(ref List<string> imgCardsDealedToPlayer,ref List<string> CardsDeck,ref List<string> DealedCardsToPlayer,ref PlayerViewModel player1,ref PlayerViewModel player2,ref PlayerViewModel player3,ref PlayerViewModel player4, ref PlayerViewModel player5, ref PlayerViewModel player6,ref  PlayerViewModel player7,ref PlayerViewModel player8,ref PlayerViewModel player9, ref bool IsDealer)
        {
            string cardsPlayer1 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player1 = cardsPlayer1.Substring(0, 2);
            string card2Player1 = cardsPlayer1.Substring(2, 2);
            string cardsPlayer2 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player2 = cardsPlayer2.Substring(0, 2);
            string card2Player2 = cardsPlayer2.Substring(2, 2);
            string cardsPlayer3 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player3 = cardsPlayer3.Substring(0, 2);
            string card2Player3 = cardsPlayer3.Substring(2, 2);
            string cardsPlayer4 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player4 = cardsPlayer4.Substring(0, 2);
            string card2Player4 = cardsPlayer4.Substring(2, 2);
            string cardsPlayer5 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player5 = cardsPlayer5.Substring(0, 2);
            string card2Player5 = cardsPlayer5.Substring(2, 2);
            string cardsPlayer6 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player6 = cardsPlayer6.Substring(0, 2);
            string card2Player6 = cardsPlayer6.Substring(2, 2);
            string cardsPlayer7 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player7 = cardsPlayer7.Substring(0, 2);
            string card2Player7 = cardsPlayer7.Substring(2, 2);
            string cardsPlayer8 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player8 = cardsPlayer8.Substring(0, 2);
            string card2Player8 = cardsPlayer8.Substring(2, 2);
            string cardsPlayer9 = Holdem.DealCardsToPlayerNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer);
            string card1Player9 = cardsPlayer9.Substring(0, 2);
            string card2Player9 = cardsPlayer9.Substring(2, 2);
            SettingDefaultsPlayer1();
            SettingDefaultsPlayer2();
            SettingDefaultsPlayer3();
            SettingDefaultsPlayer4();
            SettingDefaultsPlayer5();
            SettingDefaultsPlayer6();
            SettingDefaultsPlayer7();
            SettingDefaultsPlayer8();
            SettingDefaultsPlayer9();
            player1 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player1Name, Properties.Settings.Default.Player1Balance, 0, cardsPlayer1, IsDealer, ImageChange.GetImageSource(card1Player1), ImageChange.GetImageSource(card2Player1), Visibility.Collapsed, 1, 2, Properties.Settings.Default.Player1StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player1IsBot, 0, 1, 1, 0, HorizontalAlignment.Right, VerticalAlignment.Center);
            player2 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player2Name, Properties.Settings.Default.Player2Balance, 0, cardsPlayer2, IsDealer, ImageChange.GetImageSource(card1Player2), ImageChange.GetImageSource(card2Player2), Visibility.Collapsed, 1, 0, Properties.Settings.Default.Player2StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player2IsBot, 0, 1, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
            player3 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player3Name, Properties.Settings.Default.Player3Balance, 0, cardsPlayer3, IsDealer, ImageChange.GetImageSource(card1Player3), ImageChange.GetImageSource(card2Player3), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player3StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player3IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player4 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player4Name, Properties.Settings.Default.Player4Balance, 0, cardsPlayer4, IsDealer, ImageChange.GetImageSource(card1Player4), ImageChange.GetImageSource(card2Player4), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player4StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player4IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player5 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player5Name, Properties.Settings.Default.Player5Balance, 0, cardsPlayer5, IsDealer, ImageChange.GetImageSource(card1Player5), ImageChange.GetImageSource(card2Player5), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player5StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player5IsBot, 1, 0, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
            player6 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player6Name, Properties.Settings.Default.Player6Balance, 0, cardsPlayer6, IsDealer, ImageChange.GetImageSource(card1Player6), ImageChange.GetImageSource(card2Player6), Visibility.Collapsed, 2, 1, Properties.Settings.Default.Player6StrategyProfile, true, false, 0, 1, Properties.Settings.Default.Player6IsBot, 1, 0, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
            player7 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player7Name, Properties.Settings.Default.Player7Balance, 0, cardsPlayer7, IsDealer, ImageChange.GetImageSource(card1Player7), ImageChange.GetImageSource(card2Player7), Visibility.Collapsed, 0, 1, Properties.Settings.Default.Player7StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player7IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player8 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player8Name, Properties.Settings.Default.Player8Balance, 0, cardsPlayer8, IsDealer, ImageChange.GetImageSource(card1Player8), ImageChange.GetImageSource(card2Player8), Visibility.Collapsed, 0, 1, Properties.Settings.Default.Player8StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player8IsBot, 1, 2, 1, 0, HorizontalAlignment.Left, VerticalAlignment.Center);
            player9 = PlayerViewModel.GetPlayerNLH(false, Properties.Settings.Default.Player9Name, Properties.Settings.Default.Player9Balance, 0, cardsPlayer9, IsDealer, ImageChange.GetImageSource(card1Player9), ImageChange.GetImageSource(card2Player9), Visibility.Collapsed, 0, 1, Properties.Settings.Default.Player9StrategyProfile, true, false, 2, 1, Properties.Settings.Default.Player9IsBot, 1, 0, 1, 2, HorizontalAlignment.Left, VerticalAlignment.Center);
        }
        /// <summary>
        /// Thim method checks if Player's Balance is smaller than Big Bling * ThresholdRebuy. If Player's Balance Is smaller, than sets Balance to ThresholdRebuy * BigBlind
        /// </summary> 
        public static void CheckPlayersBalance(ref double ThresholdRebuy, ref double BigBlind,ref  ObservableCollection<PlayerViewModel> PlayersToPlay)
        {
            double valueSettingsBB = ThresholdRebuy * BigBlind;
            for (int i = 0; i < PlayersToPlay.Count; i++)
            {
                if (PlayersToPlay[i].Balance < valueSettingsBB)
                {
                    PlayersToPlay[i].Balance = valueSettingsBB;
                }
            }
        }
        public static void ShowPlayersWithoutDealing(ref bool canExecuteHide, ICommand HideCardsClick, ref ObservableCollection<PlayerViewModel> playersToPlay, ref int NumberOfPlayers, ref PlayerViewModel Player1, ref PlayerViewModel Player2, ref PlayerViewModel Player3, ref PlayerViewModel Player4, ref PlayerViewModel Player5, ref PlayerViewModel Player6, ref PlayerViewModel Player7, ref PlayerViewModel Player8, ref PlayerViewModel Player9)
        {
            canExecuteHide = true;
            HideCardsClick.CanExecute(canExecuteHide);
            playersToPlay = new();
            switch (NumberOfPlayers)
            {
                case 2:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = false;
                    Player4.IsVisible = false;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = false;
                    Player8.IsVisible = false;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player2);
                    break;
                case 3:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = false;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = false;
                    Player8.IsVisible = false;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player2);
                    break;
                case 4:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = false;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = false;
                    Player8.IsVisible = true;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player8);
                    break;
                case 5:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = false;
                    Player4.IsVisible = true;
                    Player5.IsVisible = false;
                    Player6.IsVisible = true;
                    Player7.IsVisible = true;
                    Player8.IsVisible = false;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player6);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player7);
                    break;
                case 6:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = true;
                    Player8.IsVisible = false;
                    Player9.IsVisible = true;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player7);
                    break;
                case 7:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = true;
                    Player6.IsVisible = false;
                    Player7.IsVisible = true;
                    Player8.IsVisible = false;
                    Player9.IsVisible = true;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player5);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player7);
                    break;
                case 8:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = true;
                    Player6.IsVisible = false;
                    Player7.IsVisible = true;
                    Player8.IsVisible = true;
                    Player9.IsVisible = true;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player5);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player8);
                    playersToPlay.Add(Player7);
                    break;
                case 9:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = true;
                    Player6.IsVisible = true;
                    Player7.IsVisible = true;
                    Player8.IsVisible = true;
                    Player9.IsVisible = true;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player5);
                    playersToPlay.Add(Player6);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player8);
                    playersToPlay.Add(Player7);
                    break;
                default:
                    break;
            }
        }
        public static void ShowPlayers(ref List<string> DealedCardsToPlayer, ref ObservableCollection<string> cardsOnTable, ref List<string> CardsDeck, ref bool canExecuteHide, ICommand HideCardsClick, ref ObservableCollection<PlayerViewModel> playersToPlay, ref int NumberOfPlayers, ref PlayerViewModel Player1, ref PlayerViewModel Player2, ref PlayerViewModel Player3, ref PlayerViewModel Player4, ref PlayerViewModel Player5, ref PlayerViewModel Player6, ref PlayerViewModel Player7, ref PlayerViewModel Player8, ref PlayerViewModel Player9, ref string GameChoosen, ref List<string> imgCardsDealedToPlayer)
        {
            DealedCardsToPlayer = new();
            cardsOnTable = new();
            CardsDeck = Card.Deck();
            canExecuteHide = true;
            HideCardsClick.CanExecute(canExecuteHide);
            playersToPlay = new();
            Player1.Balance += Player1.BetSize;
            Player2.Balance += Player2.BetSize;
            Player3.Balance += Player3.BetSize;
            Player4.Balance += Player4.BetSize;
            Player5.Balance += Player5.BetSize;
            Player6.Balance += Player6.BetSize;
            Player7.Balance += Player7.BetSize;
            Player8.Balance += Player8.BetSize;
            Player9.Balance += Player9.BetSize;
            switch (NumberOfPlayers)
            {
                case 2:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = false;
                    Player4.IsVisible = false;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = false;
                    Player8.IsVisible = false;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player2);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                case 3:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = false;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = false;
                    Player8.IsVisible = false;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player2);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                case 4:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = false;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = false;
                    Player8.IsVisible = true;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player8);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                case 5:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = false;
                    Player4.IsVisible = true;
                    Player5.IsVisible = false;
                    Player6.IsVisible = true;
                    Player7.IsVisible = true;
                    Player8.IsVisible = false;
                    Player9.IsVisible = false;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player6);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player7);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                case 6:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = false;
                    Player6.IsVisible = false;
                    Player7.IsVisible = true;
                    Player8.IsVisible = false;
                    Player9.IsVisible = true;
                    Player4.ColumnStats = 2;
                    Player4.ColumnDealer = 0;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player7);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                case 7:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = true;
                    Player6.IsVisible = false;
                    Player7.IsVisible = true;
                    Player8.IsVisible = false;
                    Player9.IsVisible = true;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player5);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player7);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                case 8:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = true;
                    Player6.IsVisible = false;
                    Player7.IsVisible = true;
                    Player8.IsVisible = true;
                    Player9.IsVisible = true;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player5);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player8);
                    playersToPlay.Add(Player7);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                case 9:
                    Player1.IsVisible = true;
                    Player2.IsVisible = true;
                    Player3.IsVisible = true;
                    Player4.IsVisible = true;
                    Player5.IsVisible = true;
                    Player6.IsVisible = true;
                    Player7.IsVisible = true;
                    Player8.IsVisible = true;
                    Player9.IsVisible = true;
                    playersToPlay.Add(Player1);
                    playersToPlay.Add(Player3);
                    playersToPlay.Add(Player4);
                    playersToPlay.Add(Player5);
                    playersToPlay.Add(Player6);
                    playersToPlay.Add(Player2);
                    playersToPlay.Add(Player9);
                    playersToPlay.Add(Player8);
                    playersToPlay.Add(Player7);
                    if (GameChoosen == GameEnums.NLH.ToString())
                    {
                        Holdem.GetPlayersNLH(imgCardsDealedToPlayer, CardsDeck, DealedCardsToPlayer, playersToPlay);
                    }
                    else
                    {
                        Omaha.GetPlayersPLO4(ref imgCardsDealedToPlayer, ref CardsDeck, ref DealedCardsToPlayer, ref playersToPlay);
                    }
                    break;
                default:
                    break;
            }
        }
        public static void ShowCardsMethod(ObservableCollection<PlayerViewModel> PlayersToPlay, DBLib.EnumGameType gameType) 
        {
            foreach (PlayerViewModel item in PlayersToPlay)
            {
                string card1 = item.Cards.Substring(0, 2);
                string card2 = item.Cards.Substring(2, 2);
                item.ImageSourceCard1 = ImageChange.GetImageSource(card1);
                item.ImageSourceCard2 = ImageChange.GetImageSource(card2);
                if (gameType == EnumGameType.Omaha)
                {
                    string card3 = item.Cards.Substring(4, 2);
                    string card4 = item.Cards.Substring(6, 2);
                    item.ImageSourceCard3 = ImageChange.GetImageSource(card3);
                    item.ImageSourceCard4 = ImageChange.GetImageSource(card4);
                }
            }
        }
        public static void HideCardsMethod(ObservableCollection<PlayerViewModel> PlayersToPlay, DBLib.EnumGameType gameType)
        {
            var imgPath = @"\Images\Slike\cardBack.png";
            var converter = new ImageSourceConverter();
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            imgPath = dirName + imgPath;
            ImageSource img1, img2, img3, img4;
            ImageSource imgSourceGet = (ImageSource)converter.ConvertFromString(imgPath);
            foreach (PlayerViewModel item in PlayersToPlay)
            {
                if (item.IsBot)
                {
                    if (gameType == EnumGameType.CashGame)
                    {
                        img1 = item.ImageSourceCard1;
                        img2 = item.ImageSourceCard1;
                        item.ImageSourceCard1 = imgSourceGet;
                        item.ImageSourceCard2 = imgSourceGet;
                    }
                    else
                    {
                        img1 = item.ImageSourceCard1;
                        img2 = item.ImageSourceCard1;
                        img3 = item.ImageSourceCard1;
                        img4 = item.ImageSourceCard1;
                        item.ImageSourceCard1 = imgSourceGet;
                        item.ImageSourceCard2 = imgSourceGet;
                        item.ImageSourceCard3 = imgSourceGet;
                        item.ImageSourceCard4 = imgSourceGet;
                    }
                }
            }   
        }
        #endregion
    }
}

using DBLib;
using DecisionMaking;
using PokerTable.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace PokerTable.Model
{
    public class HumanDecisionModel
    {
        #region Methods
        public static void GoAllIn(ref ObservableCollection<PlayerViewModel> PlayersToPlay,ref int indexAct, ref double sumbets, ref bool humanPlayed, EnumGameType gameType, HandState hs, double PotSize)
        {
            if (gameType == EnumGameType.CashGame)
            {
                PlayersToPlay[indexAct].BetSize = PlayersToPlay[indexAct].BetSize + PlayersToPlay[indexAct].Balance;
                PlayersToPlay[indexAct].Balance = 0;
                sumbets += PlayersToPlay[indexAct].BetSize;
            }
            else
            {
                PlayersToPlay[indexAct].BetSize = PotSizeRaise(hs, PotSize);
                if (PlayersToPlay[indexAct].Balance < PlayersToPlay[indexAct].BetSize)
                {
                    PlayersToPlay[indexAct].BetSize = PlayersToPlay[indexAct].Balance;
                    PlayersToPlay[indexAct].Balance -= PlayersToPlay[indexAct].BetSize;

                }
                else
                {
                    PlayersToPlay[indexAct].Balance -= PlayersToPlay[indexAct].BetSize;
                }
                sumbets += PlayersToPlay[indexAct].BetSize;
            }
            PlayersToPlay[indexAct].Action = "ALL IN";
            humanPlayed = true;
        }
        public static double GetHighestBet(HandState hs)
        {
            return hs.Bets.Max();
        }
        public static void LimitBet(ref double BetSizePlayer, ObservableCollection<PlayerViewModel> PlayersToPlay, int indexAct)
        {
            if (BetSizePlayer > PlayersToPlay[indexAct].Balance)
            {
                BetSizePlayer = PlayersToPlay[indexAct].Balance;
                PlayersToPlay[indexAct].BetSize = BetSizePlayer;
            }
        }
        public static void GetButtonsContent(double maxBet, EnumPhase phase, double BigBlind, ref string btnBetSize1, ref string btnBetSize2, ref string btnBetSize3, ref string btnBetSize4, ref string btnBetSize5, ref Visibility visibilityForPotButton)
        {
            if (phase == EnumPhase.Preflop && maxBet == BigBlind)
            {
                btnBetSize1 = "2x";
                btnBetSize2 = "2.5x";
                btnBetSize3 = "3x";
                btnBetSize4 = "3.5x";
                btnBetSize5 = "Pot";
                visibilityForPotButton = Visibility.Visible;
            }
            else
            {
                btnBetSize1 = "1/3";
                btnBetSize2 = "1/2";
                btnBetSize3 = "3/4";
                btnBetSize4 = "Pot";
                visibilityForPotButton = Visibility.Collapsed;
            }            
        }
        public static double PotSizeRaise(HandState hs, double PotSize)
        {
            double maxBet = GetHighestBet(hs);
            double restBets = hs.Bets.Sum() - maxBet;
            double potSizeRaise = 2 * maxBet + PotSize + restBets;
            return potSizeRaise;
        }
        public static void LimitPotSizeOmaha(double potlimitOmaha, double BetSizePlayer, ObservableCollection<PlayerViewModel> PlayersToPlay, int indexAct)
        {

            if (PlayersToPlay[indexAct].Balance < potlimitOmaha)
            {
                BetSizePlayer = PlayersToPlay[indexAct].Balance;
                PlayersToPlay[indexAct].BetSize += potlimitOmaha;
            }
        }
        /// <summary>
        /// This method serves to set Bet Button for human with values 1 or 3.5 
        /// </summary>       
        public static void BetBtn4(ref double BetPercent, ref HandState hs, ref EnumGameType gameType, ref EnumPhase phase, ref double BigBlind, ref double BetSizePlayer, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref int indexAct, ref double PotSize)
        {
            double maxBet = GetHighestBet(hs);
            double restBets = hs.Bets.Sum() - maxBet - PlayersToPlay[indexAct].BetSize;
            if (phase == EnumPhase.Preflop && maxBet <= BigBlind)
            {
                BetPercent = 3.5;
                BetSizePlayer = Math.Round(BetPercent * maxBet, 2);
                if (gameType == EnumGameType.CashGame)
                {
                    if (true)
                    {

                    }
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);
                }
            }
            else
            {
                BetSizePlayer = Math.Round(GetPotSizeRaisePerc(PotSize + restBets, maxBet, 1), 2);
                if (gameType == EnumGameType.CashGame)
                {
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);

                }
            }
        }
        /// <summary>
        /// This method serves to set Bet Button for human with values 0.75 or 3 
        /// </summary>       
        public static void BetBtn3(ref double BetPercent, ref HandState hs, ref EnumGameType gameType, ref EnumPhase phase, ref double BigBlind, ref double BetSizePlayer, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref int indexAct, ref double PotSize)
        {
            double maxBet = GetHighestBet(hs);
            double restBets = hs.Bets.Sum() - maxBet - PlayersToPlay[indexAct].BetSize;

            if (phase == EnumPhase.Preflop && maxBet <= BigBlind)
            {
                BetPercent = 3;
                BetSizePlayer = Math.Round(BetPercent * maxBet, 2);
                if (gameType == EnumGameType.CashGame)
                {
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);
                }
            }
            else
            {
                BetSizePlayer = Math.Round(GetPotSizeRaisePerc(PotSize + restBets, maxBet, 0.75), 2);
                if (gameType == EnumGameType.CashGame)
                {
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);
                }
            }          
        }
        public static double GetPotSizeRaisePerc(double pot, double betSize, double perc)
        {
            double potNext = 2 * betSize + pot;
            return betSize + perc * potNext;
        }
        /// <summary>
        /// This method serves to set Bet Button for human with values 0.5 or 2.5 
        /// </summary>       
        public static void BetBtn2(ref double BetPercent, ref HandState hs, ref EnumGameType gameType, ref EnumPhase phase, ref double BigBlind, ref double BetSizePlayer, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref int indexAct, ref double PotSize)
        {
            BetPercent = 0;
            double maxBet = GetHighestBet(hs);
            double restBets = hs.Bets.Sum() - maxBet - PlayersToPlay[indexAct].BetSize;
            if (phase == EnumPhase.Preflop && maxBet <= BigBlind)
            {
                BetPercent = 2.5;
                BetSizePlayer = Math.Round(BetPercent * maxBet, 2);
                if (gameType == EnumGameType.CashGame)
                {
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);
                }
            }
            else
            {
                BetSizePlayer = Math.Round(GetPotSizeRaisePerc(PotSize + restBets, maxBet, 0.5), 2);
                if (gameType == EnumGameType.CashGame)
                {
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);
                }
            }
        }
        /// <summary>
        /// This method serves to set Bet Button for human with values 0.33 or 2 
        /// </summary>       
        public static void BetBtn1(ref double BetPercent, ref HandState hs, ref EnumGameType gameType, ref EnumPhase phase, ref double BigBlind, ref double BetSizePlayer, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref int indexAct, ref double PotSize)
        {
            double maxBet = GetHighestBet(hs);
            double restBets = hs.Bets.Sum() - maxBet - PlayersToPlay[indexAct].BetSize;
            if (phase == EnumPhase.Preflop && maxBet <= BigBlind)
            {
                BetPercent = 2;
                BetSizePlayer = Math.Round(BetPercent * maxBet, 2);
                if (gameType == EnumGameType.CashGame)
                {
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);
                }
            }
            else
            {
                BetSizePlayer = Math.Round(GetPotSizeRaisePerc(PotSize + restBets, maxBet, 0.33), 2);
                if (gameType == EnumGameType.CashGame)
                {
                    LimitBet(ref BetSizePlayer, PlayersToPlay, indexAct);
                }
                else
                {
                    LimitPotSizeOmaha(PotSizeRaise(hs, PotSize), BetSizePlayer, PlayersToPlay, indexAct);
                }
            }
        }
        #endregion
    }
}

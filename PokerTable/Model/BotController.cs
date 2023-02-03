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
using System.Windows;

namespace PokerTable.Model
{
    public class BotController
    {
        #region Members
        #endregion
        #region Construcor
        public BotController()
        {
        }
        #endregion
        #region Methods
        public static HandState GetHandState(ref int numberOfPlayers, ref int indexAct, ref ObservableCollection<PlayerViewModel> playersToPlay, ref EnumPhase phase, ref ObservableCollection<string> cardsOnTable, ref EnumGameType gameType, ref double potSize, ref string handId)
        {
            HandState hs = new(numberOfPlayers);
            try
            {
                int playerIndex = indexAct;

                hs.Bets = new();
                hs.Names = new();
                hs.Stacks = new();
                hs.InGame = new();
                foreach (var item in playersToPlay)
                {
                    hs.Bets.Add(item.BetSize);
                    hs.Names.Add(item.Name);
                    hs.Stacks.Add(item.Balance);
                    hs.InGame.Add(item.InGame);
                }
                hs.Cards[0] = playersToPlay[playerIndex].Cards.Substring(0, 2);
                hs.Cards[1] = playersToPlay[playerIndex].Cards.Substring(2, 2);
                if (phase == EnumPhase.Preflop)
                {
                    hs.Cards[2] = "";
                    hs.Cards[3] = "";
                    hs.Cards[4] = "";
                    hs.Cards[5] = "";
                    hs.Cards[6] = "";
                }
                else if (phase == EnumPhase.Flop)
                {
                    hs.Cards[2] = cardsOnTable[0];
                    hs.Cards[3] = cardsOnTable[1];
                    hs.Cards[4] = cardsOnTable[2];
                }
                else if (phase == EnumPhase.Turn)
                {
                    hs.Cards[2] = cardsOnTable[0];
                    hs.Cards[3] = cardsOnTable[1];
                    hs.Cards[4] = cardsOnTable[2];
                    hs.Cards[5] = cardsOnTable[3];
                }
                else
                {
                    hs.Cards[2] = cardsOnTable[0];
                    hs.Cards[3] = cardsOnTable[1];
                    hs.Cards[4] = cardsOnTable[2];
                    hs.Cards[5] = cardsOnTable[3];
                    hs.Cards[6] = cardsOnTable[4];
                }
                if (gameType == EnumGameType.Omaha)
                {

                    hs.OmahaCards = new List<string>() { playersToPlay[playerIndex].Cards.Substring(0, 2).ToLower(),
                                                         playersToPlay[playerIndex].Cards.Substring(2, 2).ToLower(),
                                                         playersToPlay[playerIndex].Cards.Substring(4, 2).ToLower(),
                                                         playersToPlay[playerIndex].Cards.Substring(6, 2).ToLower() };


                }
                for (int i = 0; i < playersToPlay.Count; i++)
                {
                    if (playersToPlay[i].IsDealer == true)
                    {
                        hs.DealerID = i;
                    }
                }
                hs.HandID = handId;
                hs.OriginalNames = new List<string>(hs.Names);
                hs.Pot = potSize;
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in GetHandState from BotController: " + " " + ex.ToString());
            }
            return hs;
        }
       
        public static bool CheckIfRivalBet(ref double sumbets)
        {
            return sumbets == 0 ? false : true;
        }
        public static void ShowingButtonsForBot(ref EnumPhase phase, ref List<bool> didPlayersPlay, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref int indexAct, ref Visibility botButtonVisible, ref Visibility RunMode, ref double sumbets, ref string botCheckCall, ref string botBetRaise)
        {
            if (phase == EnumPhase.Preflop && !MainWindowViewModel.AllPlayersDidntPlay(ref didPlayersPlay) && !PlayersToPlay[indexAct].IsBot)
            {
                botButtonVisible = Visibility.Collapsed;
            }
            else
            {
                int indexNext = HoldemOmaha.GetIndexForNextPlayerBotButtons(ref indexAct, ref PlayersToPlay);
                int counter = 0;
                if (RunMode == Visibility.Visible)
                {
                    for (int i = 0; i < didPlayersPlay.Count; i++)
                    {
                        if (didPlayersPlay[i])
                        {
                            counter++;
                        }
                    }
                    if (counter == 0)
                    {
                        if (PlayersToPlay[indexAct].IsBot)
                        {
                            botButtonVisible = Visibility.Visible;
                            if (!CheckIfRivalBet(ref sumbets) && phase == EnumPhase.Preflop)
                            {
                                botCheckCall = BotDecisionText.Call.ToString();
                                botBetRaise = BotDecisionText.Raise.ToString();
                            }
                            else if (!CheckIfRivalBet(ref sumbets))
                            {
                                botCheckCall = BotDecisionText.Check.ToString();
                                botBetRaise = BotDecisionText.Bet.ToString();
                            }
                            else
                            {
                                botCheckCall = BotDecisionText.Call.ToString();
                                botBetRaise = BotDecisionText.Raise.ToString();
                            }
                        }
                        else
                        {
                            botButtonVisible = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (PlayersToPlay[indexNext].IsBot)
                        {
                            botButtonVisible = Visibility.Visible;
                            if (!CheckIfRivalBet(ref sumbets) && phase == EnumPhase.Preflop)
                            {
                                botCheckCall = BotDecisionText.Call.ToString();
                                botBetRaise = BotDecisionText.Raise.ToString();
                            }
                            else if (!CheckIfRivalBet(ref sumbets))
                            {
                                botCheckCall = BotDecisionText.Check.ToString();
                                botBetRaise = BotDecisionText.Bet.ToString();
                            }
                            else
                            {
                                botCheckCall = BotDecisionText.Call.ToString();
                                botBetRaise = BotDecisionText.Raise.ToString();
                            }
                        }
                        else
                        {
                            botButtonVisible = Visibility.Collapsed;
                        }
                    }
                    if (!PlayersToPlay[indexNext].IsBot)
                    {
                        if (!didPlayersPlay[indexAct] && PlayersToPlay[indexAct].IsBot)
                        {
                            botButtonVisible = Visibility.Visible;
                        }
                        else
                        {
                            botButtonVisible = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        botButtonVisible = Visibility.Visible;
                    } 
                }
            }
        }

        /// <summary>
        /// This method makes decision and sets balance, bet, action...
        /// </summary>
        /// <param name="decision">made decision</param>
        /// <param name="playerVM">playerViewModel for player isMyTurn</param>
        /// <param name="playerIndex">player isMyTurn</param>
        public static void DecisionMaking(ref Decision decision, ref PlayerViewModel playerVM, ref int playerIndex, HandState hs, TestBoards testBoards, ref ObservableCollection<PlayerViewModel> PlayersToPlay, ref List<bool> didPlayersPlay)
        {
            if (decision.Action == EnumDecisionType.BET || decision.Action == EnumDecisionType.RAISE || decision.Action == EnumDecisionType.CALL)
            {
                PlayerViewModel playerVModel = new();
                double diffBet = decision.BetSize - playerVM.BetSize;
                playerVM.BetSize = decision.BetSize;
                hs.Bets[0] = playerVM.BetSize;
                hs.Stacks[0] = playerVM.Balance;
                playerVM.Balance -= diffBet;
                if (playerVM.BetSize <= 0)
                {
                    playerVM.BetSize = 0;
                }
                playerVModel = playerVM;

                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    if (testBoards.IsActive)
                    {
                        playerVModel.InGame = true;
                    }
                });
                playerVM = playerVModel;
            }
            else if (decision.Action == EnumDecisionType.FOLD)
            {
                PlayerViewModel playerVModel = new();
                playerVModel = playerVM;
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    if (testBoards.IsActive)
                    {
                        playerVModel.BetSize = 0;
                    }
                    else
                    {
                        playerVModel.InGame = false;
                        hs.InGame[0] = false;
                    }
                });
                playerVM = playerVModel;
            }
            else if (decision.Action == EnumDecisionType.CHECK)
            {
                PlayerViewModel playerVModel = new();
                foreach (PlayerViewModel player in PlayersToPlay)
                {
                    if (player.Name == playerVM.Name)
                    {
                        playerVModel = player;
                    }
                }
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    if (testBoards.IsActive)
                    {
                        playerVModel.BetSize = 0;
                        playerVModel.InGame = true;
                    }
                });
                playerVM = playerVModel;
            }
            else
            {
                MessageBox.Show("There was an error with decision: " + decision.Action.ToString());
            }
            didPlayersPlay[playerIndex] = true;
            playerVM.Action = decision.Action.ToString();
            Console.WriteLine(PlayersToPlay[playerIndex].Name + " Action: " + decision.Action + " BetSize: " + decision.BetSize);
        }
        #endregion
    }
}

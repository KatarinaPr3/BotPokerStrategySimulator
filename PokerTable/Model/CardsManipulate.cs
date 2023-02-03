using DBLib;
using DecisionMaking;
using PokerTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Model
{
    public class CardsManipulate
    {
        #region Methods
        public static string ParamsHandToCards(ref string paramsHand, ref EnumGameType gameType)
        {
            string newCard = "";
            string card = paramsHand;
            try
            {
                if (gameType == EnumGameType.CashGame)
                {
                    var smallLetter = card.Substring(0, 1);
                    var bigLetter = smallLetter.ToUpper();
                    newCard = bigLetter + card.Substring(1, 1);
                    var smallLetter2 = card.Substring(2, 1);
                    var bigLetter2 = smallLetter2.ToUpper();
                    newCard = newCard + bigLetter2 + card.Substring(3, 1);
                }
                else
                {
                    var smallLetter = card.Substring(0, 1);
                    var bigLetter = smallLetter.ToUpper();
                    newCard = bigLetter + card.Substring(1, 1);
                    var smallLetter2 = card.Substring(2, 1);
                    var bigLetter2 = smallLetter2.ToUpper();
                    newCard = newCard + bigLetter2 + card.Substring(3, 1);
                    var smallLetter3 = card.Substring(4, 1);
                    var bigLetter3 = smallLetter3.ToUpper();
                    newCard = newCard + bigLetter3 + card.Substring(5, 1);
                    var smallLetter4 = card.Substring(6, 1);
                    var bigLetter4 = smallLetter4.ToUpper();
                    newCard = newCard + bigLetter4 + card.Substring(7, 1);
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Params.Hand for player: " + ex.ToString());
            }
            return newCard;
        }
        public static void ChangeHandCards(ref DecisionMaker decisionMaker, ref ObservableCollection<string> cardsOnTableActive, ref EnumGameType gameType, ref ObservableCollection<PlayerViewModel> playersToPlay, ref int indexAct, ref List<DecisionMaker> decisionMakers)
        {
            List<string> cardsOnTable = new();
            try
            {
                foreach (var item in cardsOnTableActive)
                {
                    cardsOnTable.Add(item.ToLower());
                }
                for (int i = 0; i < decisionMaker.Hero.Strategy.Params.MyRange.Count; i++)
                {
                    List<string> rangeFromMyRange = new();
                    rangeFromMyRange.Add(decisionMaker.Hero.Strategy.Params.MyRange[i].Substring(0, 2));
                    rangeFromMyRange.Add(decisionMaker.Hero.Strategy.Params.MyRange[i].Substring(2, 2));
                    if (gameType == EnumGameType.Omaha)
                    {
                        rangeFromMyRange.Add(decisionMaker.Hero.Strategy.Params.MyRange[i].Substring(4, 2));
                        rangeFromMyRange.Add(decisionMaker.Hero.Strategy.Params.MyRange[i].Substring(6, 2));
                    }
                    var comparingLists = cardsOnTable.Except(rangeFromMyRange).ToList();
                    if (comparingLists.Count != cardsOnTable.Count)
                    {
                        continue;
                    }
                    else
                    {
                        var dm = decisionMaker.Hero.Strategy.Params.MyRange[i];
                        playersToPlay[indexAct].Cards = ParamsHandToCards(ref dm, ref gameType);
                        playersToPlay[indexAct].ImageSourceCard1 = ImageChange.GetImageSource(playersToPlay[indexAct].Cards.Substring(0, 2));
                        playersToPlay[indexAct].ImageSourceCard2 = ImageChange.GetImageSource(playersToPlay[indexAct].Cards.Substring(2, 2));
                        decisionMaker.Hero.Strategy.Params.Hand = playersToPlay[indexAct].Cards.ToLower();
                        if (gameType == EnumGameType.CashGame)
                        {
                            decisionMaker.hs.Cards[0] = playersToPlay[indexAct].Cards.Substring(0, 2);
                            decisionMaker.hs.Cards[1] = playersToPlay[indexAct].Cards.Substring(2, 2);
                        }
                        else if (gameType == EnumGameType.Omaha)
                        {
                            playersToPlay[indexAct].ImageSourceCard3 = ImageChange.GetImageSource(playersToPlay[indexAct].Cards.Substring(4, 2));
                            playersToPlay[indexAct].ImageSourceCard4 = ImageChange.GetImageSource(playersToPlay[indexAct].Cards.Substring(6, 2));
                            decisionMaker.hs.Cards[0] = playersToPlay[indexAct].Cards.Substring(0, 2);
                            decisionMaker.hs.Cards[1] = playersToPlay[indexAct].Cards.Substring(2, 2);
                            decisionMaker.hs.OmahaCards[0] = playersToPlay[indexAct].Cards.Substring(0, 2);
                            decisionMaker.hs.OmahaCards[1] = playersToPlay[indexAct].Cards.Substring(2, 2);
                            decisionMaker.hs.OmahaCards[2] = playersToPlay[indexAct].Cards.Substring(4, 2);
                            decisionMaker.hs.OmahaCards[3] = playersToPlay[indexAct].Cards.Substring(6, 2);
                        }
                        decisionMakers[indexAct] = MainWindowViewModel.GetDeepCopy(decisionMaker);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in Changing Hand Cards: " + ex.ToString(), LogLevel.Error);
            }
        }
        public static string FirstLetterToBig(string card)
        {
            var smallLetter = card.Substring(0, 1);
            var bigLetter = smallLetter.ToUpper();
            var newCard = bigLetter + card.Substring(1, 1);
            return newCard;
        }
        #endregion
    }
}

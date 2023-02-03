using DBLib;
using DecisionMaking;
using DecisionMaking.DecisionMaking;
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
    public class DecisionModel
    {
        #region Methods
        public static List<T> RotateList<T>( List<T> myList, int shift)
        {

            for (int i = 0; i < shift; i++)
            {
                myList.Insert(0, myList[myList.Count - 1]);
                myList.RemoveAt(myList.Count - 1);
            }
            return myList;
        }
        public static HandState GetRotatedHs(int numberOfPlayers, int indexAct, HandState hs, ObservableCollection<PlayerViewModel> playersToPlay, EnumPhase phase, List<bool> didPlayersPlay)
        {
            int a = numberOfPlayers - indexAct;
            hs.Bets = RotateList<double>(hs.Bets, a);
            hs.Names = RotateList<string>(hs.Names, a);
            hs.OriginalNames = RotateList<string>(hs.OriginalNames, a);
            hs.Stacks = RotateList<double>(hs.Stacks, a);
            hs.EmptySeats = RotateList<bool>(hs.EmptySeats, a);
            hs.InGame = RotateList<bool>(hs.InGame, a);
            hs.DealerID -= indexAct;
            if (hs.DealerID < 0)
            {
                hs.DealerID = playersToPlay.Count - Math.Abs(hs.DealerID);
            }
            if (phase == EnumPhase.Preflop)
            {
                hs.IsNewHand = !didPlayersPlay[indexAct];
            }
            else
            {
                hs.IsNewHand = false;
            }

            // this one makes copy of hand state so that you keep original
            HandState hsRotated =    StrategyUtil.DeepCopy<HandState>(hs);

            // TO-DO here rotate the elements in the list such that player playerIndex is first element in the lists bets, stacks, names, inGame

            // original hs object has player1 as first index, player2 as 2nd index. If the playerIndex = 4, then the 4th elements in bets, stacks, names, inGame has to 
            // come to 0th element in the list, the 5th elements need to be 1st elements in the list and so on. 

            return hsRotated;
        }
        #endregion
    }
}

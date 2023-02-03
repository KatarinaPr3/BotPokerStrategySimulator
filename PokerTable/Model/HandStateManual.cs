using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Model
{
    [Serializable]
    public class HandStateManual
    {
        #region Members
        private List<string> actions;
        private List<bool> isMyTurnList;
        private List<bool> didPlayersPlayed;
        private List<bool> isBot;
        private int numberOfPlayers;
        private List<string> publicCards;
        private List<string> cardsOfPlayer;
        private List<bool> isDealer;
        #endregion
        #region Constructor
        public HandStateManual()
        {
            Actions = new();
            IsMyTurn = new();
            DidPlayersPlayed = new();
            IsBot = new();
            PublicCards = new();
            CardsOfPlayer = new();
            IsDealer = new();
        }
        #endregion
        #region Properties
        public List<bool> IsDealer
        {
            get
            {
                return isDealer;
            }
            set
            {
                isDealer = value;
            }
        }
        public List<string> CardsOfPlayer
        {
            get
            {
                return cardsOfPlayer; 
            }
            set
            {
                cardsOfPlayer = value;
            }
        }
        public List<string> PublicCards
        {
            get
            {
                return publicCards;
            }
            set
            {
                publicCards = value;
            }
        }
        public int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                numberOfPlayers = value;
            }
        }
        public List<bool> IsBot
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
        public List<bool> DidPlayersPlayed
        {
            get
            {
                return didPlayersPlayed;
            }
            set
            {
                didPlayersPlayed = value;
            }
        }
        public List<string> Actions
        {
            get
            {
                return actions;
            }
            set
            {
                actions = value;
            }
        }
        public List<bool> IsMyTurn
        {
            get
            {
                return isMyTurnList;
            }
            set
            {
                isMyTurnList = value;
            }
        }
        #endregion
    }
}

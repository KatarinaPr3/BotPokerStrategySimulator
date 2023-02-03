using DBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Model
{
    public class Statistic
    {
        #region Members
        private string statName;
        private double statValue;
        private string statPlayer;
        private double statSample;
        private string previousActions;
        private string position;
        private string vsPosition;
        private string numOpponents;
        private string action;
        private string phase;
        private string vsPlayer;
        private string preflopInitiative;
        private string potType;
        private string isMultiway;
        private string inPosition;
        private string betCategory;
        private string statID;
        private int numOfPlayers;
        #endregion
        #region Properties
        public string PreviousActions
        {
            get
            {
                return previousActions;
            }
            set
            {
                previousActions = value;
            }
        }
        public string StatName
        {
            get
            {
                return statName;
            }
            set
            {
                statName = value;
            }
        }
        public double StatValue
        {
            get
            {
                return statValue;
            }
            set
            {
                statValue = value;
            }
        }
        public string StatPlayer
        {
            get
            {
                return statPlayer;
            }
            set
            {
                statPlayer = value;
            }
        }
        public double StatSample
        {
            get
            {
                return statSample;
            }
            set
            {
                statSample = value;
            }
        }   
        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public string VsPosition
        {
            get
            {
                return vsPosition;
            }
            set
            {
                vsPosition = value;
            }
        }
        public string NumOpponents
        {
            get
            {
                return numOpponents;
            }
            set
            {
                numOpponents = value;
            }
        }       
        public string BetCategory
        {
            get
            {
                return betCategory;
            }
            set
            {
                betCategory = value;
            }
        }
        public string InPosition
        {
            get
            {
                return inPosition;
            }
            set
            {
                inPosition = value;
            }
        }
        public string IsMultiway
        {
            get
            {
                return isMultiway;
            }
            set
            {
                isMultiway = value;
            }
        }
        public string PotType
        {
            get
            {
                return potType;
            }
            set
            {
                potType = value;
            }
        }
        public string PreflopInitiative
        {
            get
            {
                return preflopInitiative;
            }
            set
            {
                preflopInitiative = value;
            }
        }
        public string VsPlayer
        {
            get
            {
                return vsPlayer;
            }
            set
            {
                vsPlayer = value;
            }
        }
        public string Phase
        {
            get
            {
                return phase;
            }
            set
            {
                phase = value;
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
        public int NumOfPlayers
        {
            get
            {
                return numOfPlayers;
            }
            set
            {
                numOfPlayers = value;
            }
        }
        public string StatID
        {
            get
            {
                return statID;
            }
            set
            {
                statID = value;
            }
        }
        #endregion
    }
}

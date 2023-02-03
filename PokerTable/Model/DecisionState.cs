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

namespace PokerTable.Model
{
    [Serializable]
    public class DecisionState
    {
        #region Members
        private int indexAct;
        private HandState hs;
        private List<DecisionMaker> allDecisionMakers;
        private Decision decision;
        private DecisionMaker decisionMaker;
        private HandStateManual hsManual;
        private EnumPhase phase;
        private List<string> rangeBetRaise;      
        private List<MultiRange> multiRanges;
        private DecisionMaking.Range rangeCheckCall;
        private DecisionMaking.Range rangeFold;
        private List<string> omahaRangesCheckCall;
        private List<string> omahaRangesFold;
        private List<string> omahaRangesBetRaiseRange;
        private string textPercentHandBet;
        private string textPercentHandCheck;
        private string textPercentHandFold;
        private List<string> stringsLog;
        private string playerTimeToAct;
        #endregion
        #region Properties
        public string PlayerTimeToAct
        {
            get
            {
                return playerTimeToAct;
            }
            set
            {
                playerTimeToAct = value;
            }
        }
        public List<string> StringsLog
        {
            get
            {
                return stringsLog;
            }
            set
            {
                stringsLog = value;
            }
        }

        public string TextPercentHandBet
        {
            get
            {
                return textPercentHandBet;
            }
            set
            {
                textPercentHandBet = value;
            }
        }
        public string TextPercentHandCheck
        {
            get
            {
                return textPercentHandCheck;
            }
            set
            {
                textPercentHandCheck = value;
            }
        }
        public string TextPercentHandFold
        {
            get
            {
                return textPercentHandFold;
            }
            set
            {
                textPercentHandFold = value;
            }
        }
        public List<string> OmahaRangesBetRaiseRange
        {
            get
            {
                return omahaRangesBetRaiseRange;
            }
            set
            {
                omahaRangesBetRaiseRange = value;
            }
        }

        public List<string> OmahaRangesCheckCall
        {
            get
            {
                return omahaRangesCheckCall;
            }
            set
            {
                omahaRangesCheckCall = value;
            }
        }
        public List<string> OmahaRangesFold
        {
            get
            {
                return omahaRangesFold;
            }
            set
            {
                omahaRangesFold = value;
            }
        }
        public List<MultiRange> MultiRanges
        {
            get
            {
                return multiRanges;
            }
            set
            {
                multiRanges = value;
            }
        }

        public DecisionMaking.Range RangeCheckCall
        {
            get 
            {
               return rangeCheckCall; 
            }
            set
            {
                rangeCheckCall = value;
            }
        }        
        public DecisionMaking.Range RangeFold
        {
            get
            {
                return rangeFold;
            }
            set
            {
                rangeFold = value;
            }
        }
        public List<string> RangeBetRaise
        {
            get
            {
                return rangeBetRaise;
            }
            set
            {
                rangeBetRaise = value;
            }            
        }       
        public EnumPhase Phase
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
        public int IndexPlayer
        {
            get
            {
                return indexAct;
            }
            set
            {
                indexAct = value;
            }
        }
        public HandStateManual HsManual
        {
            get
            {
                return hsManual;
            }
            set
            {
                hsManual = value;
            }
        }
        public DecisionMaker DecisionMaker
        {
            get
            {
                return decisionMaker;
            }
            set
            {
                decisionMaker = value;
            }
        }
        public Decision Decision
        {
            get
            {
                return decision;
            }
            set
            {
                decision = value;
            }
        }
        public HandState Hs
        {
            get
            {
                return hs;
            }
            set
            {
                hs = value;
            }
        }
        public List<DecisionMaker> AllDecisionMakers
        {
            get 
            {
                return allDecisionMakers; 
            }
            set
            {
                allDecisionMakers = value;
            }
        }
        #endregion
        #region Constructor
        public DecisionState()
        {
        }
        #endregion
        #region Methods
        /// <summary>
        /// This method makes first state without moves
        /// it serves to return everything to the initial state
        /// </summary>
        /// <param name="hs">handstate from table</param>
        /// <param name="decisionMakers">All decision Makers for first state are null</param>
        /// <param name="indexPlayer">current player</param>
        /// <param name="dm">current decision Maker for first state is null</param>
        /// <param name="decision">decision for current player for first state is null</param>
        /// <param name="handStateManual">manually add the necessary data</param>
        /// <param name="phase">current phase</param>
        /// <param name="range">range list of players, for firstState is empty</param>
        /// <param name="textBet">text % for bet/Raise ranges, for firstState is empty </param>
        /// <param name="textCheck">text % for check/call ranges, for firstState is empty </param>
        /// <param name = "textFold" > text % for fold ranges, for firstState is empty</param>
        /// <param name="stringsLog">text stringsLog for player decision, for firstState is empty</param>
        /// <param name="playerTimeToAct">text for player acting time, for firstState is empty</param>
        public static DecisionState MakeDecisionStateFirst(HandState hs, List<DecisionMaker> decisionMakers, int indexPlayer, DecisionMaker dm, Decision decision, HandStateManual handStateManual, EnumPhase phase, List<string> range, string textBet, string textCheck, string textFold, List<string> stringsLog, string playerTimeToAct)
        {
            DecisionState decisionState = new();
            decisionState.hs =    StrategyUtil.DeepCopy<HandState>(hs);
            decisionState.indexAct = indexPlayer;
            decisionState.allDecisionMakers =decisionMakers;
            decisionState.decisionMaker = dm;
            decisionState.decision = decision;
            decisionState.hsManual = handStateManual;
            decisionState.phase = phase;
            if (phase == EnumPhase.Preflop)
            {
                decisionState.rangeBetRaise = range;
            }
            decisionState.textPercentHandBet = textBet;
            decisionState.textPercentHandCheck = textCheck;
            decisionState.textPercentHandFold = textFold;
            decisionState.stringsLog = stringsLog;
            decisionState.playerTimeToAct = playerTimeToAct;
            return decisionState;
        }
        /// <summary>
        /// This method makes Decision state for PREFLOP with move for player
        /// it serves to return everything to the state before decision
        /// </summary>
        /// <param name="hs">current handstate from table</param>
        /// <param name="decisionMakers">All decision Makers</param>
        /// <param name="indexPlayer">current player</param>
        /// <param name="dm">current decision Maker</param>
        /// <param name="decision">decision for current player</param>
        /// <param name="handStateManual">manually add the necessary data</param>
        /// <param name="phase">current phase</param>
        /// <param name="rangeBetRaise">range bet/Raise/check or call of player</param>
        /// <param name="textBet">text % for bet/Raise ranges</param>
        /// <param name="textCheck">text % for check/call ranges</param>
        /// <param name = "textFold" > text % for fold ranges</param>
        /// <param name="stringsLog">text stringsLog for player decision</param>
        /// <param name="playerTimeToAct">text for player acting time</param>       
        public static DecisionState MakeDecisionState(HandState hs,  List<DecisionMaker> decisionMakers, int indexPlayer, DecisionMaker dm, Decision decision, HandStateManual handStateManual, EnumPhase phase, List<string> rangeBetRaise, string textBet, string textCheck, string textFold, List<string> stringsLog, string playerTimeToAct)
        {
            DecisionState decisionState = new();
            decisionState.hs = StrategyUtil.DeepCopy<HandState>(hs);
            decisionState.indexAct = indexPlayer;
            decisionState.allDecisionMakers = new();
            foreach (var dmTemp in decisionMakers)
            {
                decisionState.allDecisionMakers.Add(MainWindowViewModel.GetDeepCopy(dmTemp));
            }
            decisionState.decisionMaker = MainWindowViewModel.GetDeepCopy(dm);
            decisionState.decision = StrategyUtil.DeepCopy <Decision>(decision);
            decisionState.hsManual = StrategyUtil.DeepCopy<HandStateManual>(handStateManual);
            decisionState.phase = phase;           
            if (phase == EnumPhase.Preflop)
            {
                decisionState.rangeBetRaise = rangeBetRaise;
            }
            decisionState.textPercentHandBet = textBet;
            decisionState.textPercentHandCheck = textCheck;
            decisionState.textPercentHandFold = textFold;
            decisionState.stringsLog = stringsLog;
            decisionState.playerTimeToAct = playerTimeToAct;
            return decisionState;
        }
        /// <summary>
        /// This method makes Decision state for POSTFLOP HOLDEM with move for player
        /// it serves to return everything to the state before decision
        /// </summary>
        /// <param name="hs">current handstate from table</param>
        /// <param name="decisionMakers">All decision Makers</param>
        /// <param name="indexPlayer">current player</param>
        /// <param name="dm">current decision Maker</param>
        /// <param name="decision">decision for current player</param>
        /// <param name="handStateManual">manually add the necessary data</param>
        /// <param name="phase">current phase</param>
        /// <param name="multiRanges">range bet/Raise for current decisionMaker</param>
        /// <param name="rangeCheckCall">range check/call for current decisionMaker</param>
        /// <param name="rangeFold">range fold for current decisionMaker</param>
        /// <param name="textBet">text % for bet/Raise ranges</param>
        /// <param name="textCheck">text % for check/call ranges</param>
        /// <param name= "textFold" > text % for fold ranges</param>
        /// <param name="stringsLog">text stringsLog for player decision</param>
        /// <param name="playerTimeToAct">text for player acting time</param>
        public static DecisionState MakeDecisionState(HandState hs, List<DecisionMaker> decisionMakers, int indexPlayer, DecisionMaker dm, Decision decision, HandStateManual handStateManual, EnumPhase phase, List<MultiRange> multiRanges, DecisionMaking.Range rangeCheckCall, DecisionMaking.Range rangeFold, string textBet, string textCheck, string textFold, List<string> stringsLog, string playerTimeToAct)
        {
            DecisionState decisionState = new();
            decisionState.hs = StrategyUtil.DeepCopy<HandState>(hs);
            decisionState.indexAct = indexPlayer;
            decisionState.allDecisionMakers = new();
            foreach(var dmTemp in decisionMakers)
            {
                decisionState.allDecisionMakers.Add(MainWindowViewModel.GetDeepCopy(dmTemp));
            }
            decisionState.decisionMaker = MainWindowViewModel.GetDeepCopy(dm);
            decisionState.decision = StrategyUtil.DeepCopy<Decision>(decision);
            decisionState.hsManual = StrategyUtil.DeepCopy<HandStateManual>(handStateManual);
            decisionState.phase = phase;
            decisionState.multiRanges = multiRanges;
            decisionState.rangeCheckCall = rangeCheckCall;
            decisionState.rangeFold = rangeFold;
            decisionState.textPercentHandBet = textBet;
            decisionState.textPercentHandCheck = textCheck;
            decisionState.textPercentHandFold = textFold;
            decisionState.stringsLog = stringsLog;
            decisionState.playerTimeToAct = playerTimeToAct;
            return decisionState;
        }
        /// <summary>
        /// This method makes Decision state for POSTFLOP OMAHA with move for player
        /// it serves to return everything to the state before decision
        /// </summary>
        /// <param name="hs">current handstate from table</param>
        /// <param name="decisionMakers">All decision Makers</param>
        /// <param name="indexPlayer">current player</param>
        /// <param name="dm">current decision Maker</param>
        /// <param name="decision">decision for current player</param>
        /// <param name="handStateManual">manually add the necessary data</param>
        /// <param name="phase">current phase</param>
        /// <param name="multiRanges">range bet/Raise for current decisionMaker</param>
        /// <param name="rangeCheckCall">range check/call for current decisionMaker</param>
        /// <param name="rangeFold">range fold for current decisionMaker</param>
        /// <param name="textBet">text % for bet/Raise ranges</param>
        /// <param name="textCheck">text % for check/call ranges</param>
        /// <param name = "textFold" > text % for fold ranges</param>
        /// <param name="stringsLog">text stringsLog for player decision</param>
        /// <param name="playerTimeToAct">text for player acting time</param>
        public static DecisionState MakeDecisionStateOmahaPostflop(HandState hs, List<DecisionMaker> decisionMakers, int indexPlayer, DecisionMaker dm, Decision decision, HandStateManual handStateManual, EnumPhase phase, List<MultiRange> multiRanges, List<string> rangeCheckCall, List<string> rangeFold, List<string> omahaRangesBetRaiseRange, string textBet, string textCheck, string textFold, List<string> stringsLog, string playerTimeToAct)
        {
            DecisionState decisionState = new();
            decisionState.hs =    StrategyUtil.DeepCopy<HandState>(hs);
            decisionState.indexAct = indexPlayer;
            decisionState.allDecisionMakers = new();
            foreach (var item in decisionMakers)
            {
                decisionState.allDecisionMakers.Add(MainWindowViewModel.GetDeepCopy(item));
            }
            //decisionState.allDecisionMakers =  StrategyUtil.DeepCopy<List<DecisionMaker>>(decisionMakers);
            decisionState.decisionMaker = MainWindowViewModel.GetDeepCopy(dm);
            decisionState.decision = StrategyUtil.DeepCopy<Decision>(decision);
            decisionState.hsManual = StrategyUtil.DeepCopy<HandStateManual>(handStateManual);
            decisionState.phase = phase;
            decisionState.multiRanges = multiRanges;
            decisionState.omahaRangesCheckCall = rangeCheckCall;
            decisionState.omahaRangesFold = rangeFold;
             decisionState.omahaRangesBetRaiseRange = omahaRangesBetRaiseRange;
            decisionState.textPercentHandBet = textBet;
            decisionState.textPercentHandCheck = textCheck;
            decisionState.textPercentHandFold = textFold;
            decisionState.stringsLog = stringsLog;
            decisionState.playerTimeToAct = playerTimeToAct;
            return decisionState;
        }
        #endregion
    }
}

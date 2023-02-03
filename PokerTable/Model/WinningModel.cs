using PokerTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class WinningModel
    {
        #region Members
        private string name;
        private double allTimeWinnings;
        private double sessionWinnings;
        private int allTimeHands;
        private int sessionHands;
        private Brush brushSession;
        private Brush brushAllTime;
        #endregion
        #region Properties
        public Brush BrushSession
        {
            get
            {
                return brushSession;
            }
            set
            {
                brushSession = value;
            }
        }
        public Brush BrushAllTime
        {
            get
            {
                return brushAllTime;
            }
            set
            {
                brushAllTime = value;
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public double AllTimeWinnings
        {
            get
            {
                return allTimeWinnings;
            }
            set
            {
                allTimeWinnings = value;
            }
        }
        public double SessionWinnings
        {
            get
            {
                return sessionWinnings;
            }
            set
            {
                sessionWinnings = value;
            }
        }
        public int AllTimeHands
        {
            get
            {
                return allTimeHands;
            }
            set
            {
                allTimeHands = value;
            }
        }
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
        #endregion
        #region Methods
        public static WinningModel CreateWinningModel(string name, double sessionWinnings, int sessionHands, double allTimeWinnings, int allTimeHands)
        {
            WinningModel winningModel = new();
            winningModel.name = name;
            winningModel.sessionWinnings = sessionWinnings;
            winningModel.allTimeWinnings = allTimeWinnings;
            winningModel.allTimeHands = allTimeHands;
            if (sessionWinnings > 0)
            {
                winningModel.brushSession = Brushes.Green;
            }
            else if (sessionWinnings < 0)
            {
                winningModel.brushSession = Brushes.Red;
            }
            if (allTimeWinnings > 0)
            {
                winningModel.brushAllTime = Brushes.Green;
            }
            else if (allTimeWinnings < 0)
            {
                winningModel.brushAllTime = Brushes.Red;
            }
            winningModel.sessionHands = sessionHands;
            return winningModel;
        }
        #endregion
    }
}

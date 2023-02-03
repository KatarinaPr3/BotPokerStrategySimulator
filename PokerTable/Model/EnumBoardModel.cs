using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Model
{
    public class EnumBoardModel
    {
        #region Members
        private string enumBoards;
        private double raisePercents;
        private double callPercents;
        private double foldPercents;
        #endregion
        #region Properties
        public double FoldPercents
        {
            get { return foldPercents; }
            set
            {
                foldPercents = value;
            }
        }
        public double CallPercents
        {
            get { return callPercents; }
            set
            {
                callPercents = value;
            }
        }
        public double RaisePercents
        {
            get { return raisePercents; }
            set
            {
                raisePercents = value;
            }
        }
        public string EnumBoards
        {
            get { return enumBoards; }
            set
            {
                enumBoards = value;
            }
        }
        #endregion
        #region Methods
        public static EnumBoardModel CreateEnumBoardModel(string board, double raise, double call, double fold)
        {
            EnumBoardModel enumBoardModel = new EnumBoardModel();
            enumBoardModel.enumBoards = board;
            enumBoardModel.raisePercents = raise;
            enumBoardModel.callPercents = call;
            enumBoardModel.foldPercents = fold;
            return enumBoardModel;

        }
        #endregion
    }
}

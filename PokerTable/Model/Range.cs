using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class Range
    {
        #region Members
        private string cards, card1, card2;
        public ImageSource imgSource1, imgSource2;
        #endregion
        #region Properties
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
        #endregion
    }
}

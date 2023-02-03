using PokerTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class RangePercents : ViewModelBase
    {
        #region Members
        private Brush brush;
        private string rangeName;
        private double percentage;
        #endregion
        #region Constructor
        public RangePercents()
        {
        }
        #endregion
        #region Properties
        public Brush BrushTxt
        {
            get
            {
                return brush;
            }
            set
            {
                brush = value;
                OnPropertyChanged(nameof(BrushTxt));
            }
        }
        public string RangeName
        {
            get
            {
                return rangeName;
            }
            set
            {
                rangeName = value;
                OnPropertyChanged(nameof(RangeName));
            }
        }
        public double Percentage
        {
            get
            {
                return percentage;   
            }
            set 
            { 
                percentage = value; 
                OnPropertyChanged(nameof(Percentage));
            }
        }
        #endregion
        #region Methods
        public static RangePercents GetRangeTxtComboAndPerc(string name, double perc, Brush color)
        {
            RangePercents rangePercents = new();
            rangePercents.rangeName = name;
            rangePercents.percentage = perc;
            rangePercents.brush = color;
            return rangePercents;
        }
        public static RangePercents GetRangeTxt(string name, double perc, Brush color)
        {
            RangePercents rangePercents = new();
            rangePercents.rangeName = name;
            rangePercents.percentage = PercentWithOneDecimal(perc);
            rangePercents.brush = color;
            return rangePercents;
        }
        public static RangePercents GetSeparator(string name,  Brush color)
        {
            RangePercents rangePercents = new();
            rangePercents.rangeName = name;
            rangePercents.brush = color;
            return rangePercents;
        }
        public static ObservableCollection<RangePercents> RangePercentsCollection(ObservableCollection<RangePercents> rangePercents, double cmb, double strongR, double tpgkR, double topR, double secR, double drawsR, double weakR, double smallR, double bdfdR, double overR, double airR)
        {
            RangePercents combos = GetRangeTxtComboAndPerc("Combos: ", cmb, Brushes.Black);
            RangePercents separator = GetSeparator("-----------", Brushes.Black);
            RangePercents strong = GetRangeTxt("Strong: ", strongR, Brushes.Red);
            RangePercents tpgk = GetRangeTxt("TPGK: ", tpgkR, Brushes.Orange);
            RangePercents top = GetRangeTxt("TPWK: ", topR, Brushes.Pink);
            RangePercents sec = GetRangeTxt("2nd pairs: ", secR, Brushes.Brown);
            RangePercents draws = GetRangeTxt("Weak: ", drawsR, Brushes.Blue);
            RangePercents weak = GetRangeTxt("Draws: ", weakR, Brushes.Coral);
            RangePercents small = GetRangeTxt("Small: ", smallR, Brushes.Purple);
            RangePercents bdfd = GetRangeTxt("BDFD: ", bdfdR, Brushes.Green);
            RangePercents overcards = GetRangeTxt("Overcards: ", overR, Brushes.GreenYellow);
            RangePercents air = GetRangeTxt("Air: ", airR, Brushes.Indigo);
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                rangePercents.Add(combos);
                rangePercents.Add(separator);
                if (strongR != 0)
                {
                    rangePercents.Add(strong);
                }
                if (tpgkR != 0)
                {
                    rangePercents.Add(tpgk);
                }
                if (topR != 0)
                {
                    rangePercents.Add(top);
                }
                if (secR != 0)
                {
                    rangePercents.Add(sec);

                }
                if (drawsR != 0)
                {
                    rangePercents.Add(draws);
                }
                if (weakR != 0)
                {
                    rangePercents.Add(weak);
                }
                if (smallR != 0)
                {
                    rangePercents.Add(small);
                }
                if (bdfdR != 0)
                {
                    rangePercents.Add(bdfd);
                }
                if (overR != 0)
                {
                    rangePercents.Add(overcards);
                }
                if (airR != 0)
                {
                    rangePercents.Add(air);
                }
            }); 
            return rangePercents;
        }
        private static double PercentWithOneDecimal(double number)
        {
            number = number * 100;
            number = Math.Round(number, 1);
            return number;
        }
        #endregion
    }
}

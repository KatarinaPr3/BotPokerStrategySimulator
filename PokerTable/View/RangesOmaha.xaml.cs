using PokerTable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokerTable.View
{
    /// <summary>
    /// Interaction logic for RangesOmaha.xaml
    /// </summary>
    public partial class RangesOmaha : UserControl
    {
        public Omaha SelectedShowdown
        {
            get
            {
                return (Omaha)GetValue(PropShowdownSelected);
            }
            set
            {
                SetValue(PropShowdownSelected, value);
            }
        }
        public static readonly DependencyProperty PropShowdownSelected =
       DependencyProperty.Register("SelectedShowdown", typeof(Omaha), typeof(RangesOmaha), new PropertyMetadata(null));

        public ObservableCollection<Omaha> DrawsOmaha
        {
            get
            {
                return (ObservableCollection<Omaha>)GetValue(PropDrawsOmaha);
            }
            set
            {
                SetValue(PropDrawsOmaha, value);
            }
        }
        public static readonly DependencyProperty PropDrawsOmaha =
        DependencyProperty.Register("DrawsOmaha", typeof(ObservableCollection<Omaha>), typeof(RangesOmaha), new PropertyMetadata(null));

        public ObservableCollection<Omaha> RangeListOmaha
        {
            get
            {
                return (ObservableCollection<Omaha>)GetValue(PropRangeList);
            }
            set
            {
                SetValue(PropRangeList, value);
            }
        }
        public static readonly DependencyProperty PropRangeList =
        DependencyProperty.Register("RangeListOmaha", typeof(ObservableCollection<Omaha>), typeof(RangesOmaha), new PropertyMetadata(null));

        public ObservableCollection<Omaha> ShowdownsDrawsOmaha
        {
            get
            {
                return (ObservableCollection<Omaha>)GetValue(PropShowdownsDrawsOmaha);
            }
            set
            {
                SetValue(PropShowdownsDrawsOmaha, value);
            }
        }
        public static readonly DependencyProperty PropShowdownsDrawsOmaha =
        DependencyProperty.Register("ShowdownsDrawsOmaha", typeof(ObservableCollection<Omaha>), typeof(RangesOmaha), new PropertyMetadata(null));
        // public ICommand Refresh
        // {
        //     get => (ICommand)GetValue(RefreshCommandProperty);
        //     set => SetValue(RefreshCommandProperty, value);
        // }
        // public static readonly DependencyProperty RefreshCommandProperty =
        //DependencyProperty.Register(nameof(Refresh), typeof(ICommand), typeof(RangesOmaha), new PropertyMetadata(null));
        public RangesOmaha()
        {
            InitializeComponent();
        }       
    }
}

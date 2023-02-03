using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokerTable.View
{
    /// <summary>
    /// Interaction logic for PotUC.xaml
    /// </summary>
    public partial class PotUC : UserControl
    {
        public double PotSize
        {
            get { return (double)GetValue(PotSizeProperty); }
            set { SetValue(PotSizeProperty, value); }
        }

        public static readonly DependencyProperty PotSizeProperty =
   DependencyProperty.Register("PotSize", typeof(double),
     typeof(PotUC), new PropertyMetadata(null));
        public double TargetNumber
        {
            get { return (double)GetValue(TargetNumberProperty); }
            set { SetValue(TargetNumberProperty, value); }
        }
        public static readonly DependencyProperty TargetNumberProperty =
            DependencyProperty.Register("TargetNumber", typeof(double), typeof(PotUC), new PropertyMetadata(0.00
                , new PropertyChangedCallback(Target_PropertyChanged2)));

        private static void Target_PropertyChanged2(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Storyboard sb = ((PotUC)obj).rootPot.Resources["sb"] as Storyboard;
            DoubleAnimation da = sb.Children[0] as DoubleAnimation;
            Storyboard.SetTarget(da, obj);
            da.From = (Double)e.OldValue;
            da.To = (Double)e.NewValue;
            sb.Begin();
        }
        public PotUC()
        {
            InitializeComponent();
        }
    }
}

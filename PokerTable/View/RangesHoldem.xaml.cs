using PokerTable.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PokerTable.View
{
    /// <summary>
    /// Interaction logic for RangesHoldem.xaml
    /// </summary>
    public partial class RangesHoldem : UserControl
    {
      
        public int WidthRangesHoldem
        {
            get
            {
                return (int)GetValue(PropWidth);
            }
            set
            {
                SetValue(PropWidth, value);
            }
        }
        public static readonly DependencyProperty PropWidth =
        DependencyProperty.Register("WidthRangesHoldem", typeof(int), typeof(RangesHoldem), new PropertyMetadata(null));

        public ObservableCollection<RangePercents> RangeList
        {
            get
            {
                return (ObservableCollection<RangePercents>)GetValue(PropRangeList);
            }
            set
            {
                SetValue(PropRangeList, value);
            }
        }
        public static readonly DependencyProperty PropRangeList =
        DependencyProperty.Register("RangeList", typeof(ObservableCollection<RangePercents>), typeof(RangesHoldem), new PropertyMetadata(null));

        public ObservableCollection<Holdem> RangeListHoldem
        {
            get
            {
                return (ObservableCollection<Holdem>)GetValue(PropRangeListHoldem);
            }
            set
            {
                SetValue(PropRangeListHoldem, value);
            }
        }
        public static readonly DependencyProperty PropRangeListHoldem =
        DependencyProperty.Register("RangeListHoldem", typeof(ObservableCollection<Holdem>), typeof(RangesHoldem), new PropertyMetadata(null));



        public ObservableCollection<Brush> ColorRanges
        {
            get
            {
                return (ObservableCollection<Brush>)GetValue(ColorRangesProp);
            }
            set
            {
                SetValue(ColorRangesProp, value);
            }
        }
        public static readonly DependencyProperty ColorRangesProp =
        DependencyProperty.Register("ColorRanges", typeof(ObservableCollection<Brush>), typeof(RangesHoldem), new PropertyMetadata(null));
        public ObservableCollection<ImageSource> ImageSources1
        {
            get
            {
                return (ObservableCollection<ImageSource>)GetValue(PropImgSource1);
            }
            set
            {
                SetValue(PropImgSource1, value);
            }
        }
        public static readonly DependencyProperty PropImgSource1 =
        DependencyProperty.Register("ImageSources1", typeof(ObservableCollection<ImageSource>), typeof(RangesHoldem), new PropertyMetadata(null));

        public ObservableCollection<ImageSource> ImageSources2
        {
            get
            {
                return (ObservableCollection<ImageSource>)GetValue(PropImgSource2);
            }
            set
            {
                SetValue(PropImgSource2, value);
            }
        }

        public static readonly DependencyProperty PropImgSource2 =
        DependencyProperty.Register("ImageSources2", typeof(ObservableCollection<ImageSource>), typeof(RangesHoldem), new PropertyMetadata(null));
        public ObservableCollection<ImageSource> ImageSources3
        {
            get
            {
                return (ObservableCollection<ImageSource>)GetValue(PropImgSource3);
            }
            set
            {
                SetValue(PropImgSource3, value);
            }
        }

        public static readonly DependencyProperty PropImgSource3 =
        DependencyProperty.Register("ImageSources3", typeof(ObservableCollection<ImageSource>), typeof(RangesHoldem), new PropertyMetadata(null));
        public ObservableCollection<ImageSource> ImageSources4
        {
            get
            {
                return (ObservableCollection<ImageSource>)GetValue(PropImgSource4);
            }
            set
            {
                SetValue(PropImgSource4, value);
            }
        }

        public static readonly DependencyProperty PropImgSource4 =
        DependencyProperty.Register("ImageSources4", typeof(ObservableCollection<ImageSource>), typeof(RangesHoldem), new PropertyMetadata(null));

        public Visibility VisibilityHoldemOrOmaha
        {
            get
            {
                return (Visibility)GetValue(VisibilityPropHoldemOmaha);
            }
            set
            {
                SetValue(VisibilityPropHoldemOmaha, value);
            }
        }
        public static readonly DependencyProperty VisibilityPropHoldemOmaha =
           DependencyProperty.Register("VisibilityHoldemOrOmaha", typeof(Visibility), typeof(RangesHoldem), new PropertyMetadata(null));
        public int WidthHoldemAndOmaha
        {
            get
            {
                return (int)GetValue(WidthHoldemOmahaProp);
            }
            set
            {
                SetValue(WidthHoldemOmahaProp, value);
            }
        }
        public static readonly DependencyProperty WidthHoldemOmahaProp =
           DependencyProperty.Register("WidthHoldemAndOmaha", typeof(int), typeof(RangesHoldem), new PropertyMetadata(null));
        public RangesHoldem()
        {
            InitializeComponent();
        }
    }
}

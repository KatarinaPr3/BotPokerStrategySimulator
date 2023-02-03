using MicroMvvm;
using PokerTable.ViewModel;
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
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
        }
        #region Properties
        public Orientation BetDealerOrientation
        {
            get 
            { 
                return (Orientation)GetValue(OrientationProp); 
            }
            set 
            { 
                SetValue(OrientationProp, value); 
            }
        }        
        public int WidthOfColumn
        {
            get 
            { 
                return (int)GetValue(WidthProp); 
            }
            set 
            { 
                SetValue(WidthProp, value); 
            }
        }
        public string NameOfPlayer
        {
            get 
            { 
                return (string)GetValue(NameProp); 
            }
            set 
            { 
                SetValue(NameProp, value); 
            }
        }
        public double Balance
        {
            get 
            { 
                return (double)GetValue(BalanceProperty); 
            }
            set 
            { 
                SetValue(BalanceProperty, value); 
            }
        }
        public double BetSize
        {
            get 
            { 
                return (double)GetValue(BetProperty); 
            }
            set 
            { 
                SetValue(BetProperty, value); 
            }
        }
        public bool IsDealer
        {
            get
            {
                return (bool)GetValue(DealerProperty);
            }
            set
            {
                SetValue(DealerProperty, value);
            }
        }
        public string Cards
        {
            get
            {
                return (string)GetValue(CardsProperty);
            }
            set
            {
                SetValue(CardsProperty, value);
            }
        }
        public bool IsVisibleBool
        {
            get
            {
                return (bool)GetValue(IsVisibleProp);
            }
            set
            {
                SetValue(IsVisibleProp, value);
            }
        }
        public ImageSource ImageBotHuman
        {
            get
            {
                return (ImageSource)GetValue(IconImageBotHuman);
            }
            set
            {
                SetValue(IconImageBotHuman, value);
            }
        }
        public ImageSource ImageSource1
        {
            get
            {
                return (ImageSource)GetValue(IconImage);
            }
            set
            {
                SetValue(IconImage, value);
            }
        }
        public ImageSource ImageSource2
        {
            get
            {
                return (ImageSource)GetValue(IconImage2);
            }
            set
            {
                SetValue(IconImage2, value);
            }
        }
        public ImageSource ImageSource3
        {
            get
            {
                return (ImageSource)GetValue(IconImage3);
            }
            set
            {
                SetValue(IconImage3, value);
            }
        }
        public ImageSource ImageSource4
        {
            get
            {
                return (ImageSource)GetValue(IconImage4);
            }
            set
            {
                SetValue(IconImage4, value);
            }
        }
        public Visibility VisibilityPlayerLogo
        {
            get
            {
                return (Visibility)GetValue(LogoVisibilityProp);
            }
            set
            {
                SetValue(LogoVisibilityProp, value);
            }
        }
        public Visibility DealerVisibility
        {
            get
            {
                return (Visibility)GetValue(DealerVisibilityProp);
            }
            set
            {
                SetValue(DealerVisibilityProp, value);
            }
        }
        public int RowVisibility
        {
            get
            {
                return (int)GetValue(RowProperty);
            }
            set
            {
                SetValue(RowProperty, value);
            }
        }
        public int ColumnVisibility
        {
            get
            {
                return (int)GetValue(ColumnProperty);
            }
            set
            {
                SetValue(ColumnProperty, value);
            }
        }
        public int RowVisibilityDealer
        {
            get
            {
                return (int)GetValue(RowPropertyDealer);
            }
            set
            {
                SetValue(RowPropertyDealer, value);
            }
        }
        public int ColumnVisibilityDealer
        {
            get
            {
                return (int)GetValue(ColumnPropertyDealer);
            }
            set
            {
                SetValue(ColumnPropertyDealer, value);
            }
        }
        public int RowVisibilityButton
        {
            get
            {
                return (int)GetValue(RowBtnProperty);
            }
            set
            {
                SetValue(RowBtnProperty, value);
            }
        }
        public int ColumnVisibilityButton
        {
            get
            {
                return (int)GetValue(ColumnBtnProperty);
            }
            set
            {
                SetValue(ColumnBtnProperty, value);
            }
        }
        public string StrategyProfile
        {
            get
            {
                return (string)GetValue(StrategyProperty);
            }
            set
            {
                SetValue(StrategyProperty, value);
            }
        }
        public string Action
        {
            get
            {
                return (string)GetValue(ActionProperty);
            }
            set
            {
                SetValue(ActionProperty, value);
            }
        }
        public bool IsMyTurn
        {
            get
            {
                return (bool)GetValue(IsMyTurnProperty);
            }
            set
            {
                SetValue(IsMyTurnProperty, value);
            }
        }
        public bool InGame
        {
            get
            {
                return (bool)GetValue(InGameProperty);
            }
            set
            {
                SetValue(InGameProperty, value);
            }
        }
        public Visibility BetSizeVisibility
        {
            get
            {
                return (Visibility)GetValue(BetVisibilityProp);
            }
            set
            {
                SetValue(BetVisibilityProp, value);
            }
        }
        public Visibility VisibilityCardsInView
        {
            get
            {
                return (Visibility)GetValue(CardsVisibilityProp);
            }
            set
            {
                SetValue(CardsVisibilityProp, value);
            }
        }
        public Visibility VisibilityOmaha
        {
            get
            {
                return (Visibility)GetValue(VisibilityOmahaProp);
            }
            set
            {
                SetValue(VisibilityOmahaProp, value);
            }
        }
        public bool IsWinner
        {
            get
            {
                return (bool)GetValue(IsWinnerProperty);
            }
            set
            {
                SetValue(IsWinnerProperty, value);
            }
        }       
        public HorizontalAlignment HorizontalAlignmentStats
        {
            get
            {
                return (HorizontalAlignment)GetValue(HorizontalAlignmentStatsProp);
            }
            set
            {
                SetValue(HorizontalAlignmentStatsProp, value);
            }
        }
        public VerticalAlignment VerticalAlignmentStats
        {
            get
            {
                return (VerticalAlignment)GetValue(VerticalAlignmentStatsProp);
            }
            set
            {
                SetValue(VerticalAlignmentStatsProp, value);
            }
        }
        public int RowVisibilityStats
        {
            get
            {
                return (int)GetValue(RowPropertyStats);
            }
            set
            {
                SetValue(RowPropertyStats, value);
            }
        }
        public int ColumnVisibilityStats
        {
            get
            {
                return (int)GetValue(ColumnPropertyStats);
            }
            set
            {
                SetValue(ColumnPropertyStats, value);
            }
        }
        public double TargetNumber
        {
            get
            {
                return (double)GetValue(TargetNumberProperty);
            }
            set
            {
                SetValue(TargetNumberProperty, value);
            }
        }
        #endregion
        #region Dependency
        public static readonly DependencyProperty OrientationProp =
        DependencyProperty.Register("BetDealerOrientation", typeof(int),
        typeof(PlayerView), new PropertyMetadata(null));
        
        public static readonly DependencyProperty WidthProp =
        DependencyProperty.Register("WidthOfColumn", typeof(int),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty BotOrHumanCommandProperty =
        DependencyProperty.Register(nameof(BotOrHuman), typeof(ICommand),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty HorizontalAlignmentStatsProp =
        DependencyProperty.Register("HorizontalAlignmentStats", typeof(HorizontalAlignment),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty VerticalAlignmentStatsProp =
        DependencyProperty.Register("VerticalAlignmentStats", typeof(VerticalAlignment),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty RowPropertyStats =
        DependencyProperty.Register("RowVisibilityStats", typeof(int),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnPropertyStats =
        DependencyProperty.Register("ColumnVisibilityStats", typeof(int),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty VisibilityOmahaProp =
        DependencyProperty.Register("VisibilityOmaha", typeof(Visibility),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IsWinnerProperty =
        DependencyProperty.Register("IsWinner", typeof(bool),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty ActionProperty =
        DependencyProperty.Register("Action", typeof(string),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty RowBtnProperty =
        DependencyProperty.Register("RowVisibilityButton", typeof(int),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnBtnProperty =
        DependencyProperty.Register("ColumnVisibilityButton", typeof(int),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty FoldCommandProperty =
        DependencyProperty.Register(nameof(FoldCommand), typeof(ICommand),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty StatisticCommandProperty =
        DependencyProperty.Register(nameof(ShowStatistic), typeof(ICommand),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IsMyTurnProperty =
        DependencyProperty.Register("IsMyTurn", typeof(bool),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty InGameProperty =
        DependencyProperty.Register("InGame", typeof(bool), typeof(PlayerView),
        new PropertyMetadata(null));

        public static readonly DependencyProperty BetVisibilityProp =
        DependencyProperty.Register("BetSizeVisibility", typeof(Visibility),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty CardsVisibilityProp =
        DependencyProperty.Register("VisibilityCardsInView", typeof(Visibility),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty StrategyProperty =
        DependencyProperty.Register("StrategyProfile", typeof(string),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty RowProperty =
        DependencyProperty.Register("RowVisibility", typeof(int), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnProperty =
        DependencyProperty.Register("ColumnVisibility", typeof(int), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty RowPropertyDealer =
        DependencyProperty.Register("RowVisibilityDealer", typeof(int), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnPropertyDealer =
        DependencyProperty.Register("ColumnVisibilityDealer", typeof(int), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty DoubleClickCommandPropery =
        DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty DealerVisibilityProp =
        DependencyProperty.Register("DealerVisibility", typeof(Visibility), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IconImage =
        DependencyProperty.Register("ImageSource1", typeof(ImageSource), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IconImage2 =
        DependencyProperty.Register("ImageSource2", typeof(ImageSource), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IconImageBotHuman =
        DependencyProperty.Register("ImageBotHuman", typeof(ImageSource), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IconImage3 =
        DependencyProperty.Register("ImageSource3", typeof(ImageSource), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IconImage4 =
        DependencyProperty.Register("ImageSource4", typeof(ImageSource), 
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty DealerProperty =
        DependencyProperty.Register("IsDealer", typeof(bool),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty IsVisibleProp =
        DependencyProperty.Register("IsVisibleBool", typeof(bool),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty LogoVisibilityProp =
        DependencyProperty.Register("VisibilityPlayerLogo", typeof(bool),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty CardsProperty =
        DependencyProperty.Register("Cards", typeof(string),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty NameProp =
        DependencyProperty.Register("NameOfPlayer", typeof(string),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty BetProperty =
        DependencyProperty.Register("BetSize", typeof(double),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty BalanceProperty =
        DependencyProperty.Register("Balance", typeof(double),
        typeof(PlayerView), new PropertyMetadata(null));

        public static readonly DependencyProperty TargetNumberProperty =
        DependencyProperty.Register("TargetNumber", typeof(double), typeof(PlayerView),
        new PropertyMetadata(0.00, new PropertyChangedCallback(Target_PropertyChanged)));
        #endregion
        #region Methods
        private static void Target_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Storyboard sb = ((PlayerView)obj).root.Resources["sb"] as Storyboard;
            DoubleAnimation da = sb.Children[0] as DoubleAnimation;
            Storyboard.SetTarget(da, obj);
            da.From = Math.Round((double)e.OldValue, 2);
            da.To = (Double)e.NewValue;
            sb.Begin();
        }
        #endregion
        #region Commands
        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandPropery);
            set => SetValue(DoubleClickCommandPropery, value);
        }

        public ICommand FoldCommand
        {
            get => (ICommand)GetValue(FoldCommandProperty);
            set => SetValue(FoldCommandProperty, value);
        }
        public ICommand BotOrHuman
        {
            get => (ICommand)GetValue(BotOrHumanCommandProperty);
            set => SetValue(BotOrHumanCommandProperty, value);
        }

        public ICommand ShowStatistic
        {
            get => (ICommand)GetValue(StatisticCommandProperty);
            set => SetValue(StatisticCommandProperty, value);
        }
        #endregion
    }
}

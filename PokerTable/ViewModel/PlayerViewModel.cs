using MicroMvvm;
using PokerTable.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PokerTable.ViewModel
{
    [Serializable]
    public class PlayerViewModel : ViewModelBase
    {
        #region Members
        private ObservableCollection<string> strategyProfiles;
        private Player player;
        private Visibility isVisible;
        private Visibility isBetVisible;
        private int widthColumn;
        #endregion
        #region Constructor
        public PlayerViewModel()
        {
            player = new Player();
            isVisible = Visibility.Visible;
            strategyProfiles = new ObservableCollection<string>();
            foreach (var item in Enum.GetValues(typeof(EnumProfile)))
            {
                strategyProfiles.Add(item.ToString());
            }
        }
        #endregion
        #region Properties
        public int WidthColumn
        {
            get { return widthColumn; }
            set
            {
                widthColumn = value;
                OnPropertyChanged(nameof(WidthColumn));
            }
        }
        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
                OnPropertyChanged(nameof(Player));
            }
        }
        public string Name
        {
            get
            {
                return player.Name;
            }
            set
            {
                player.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public double BetSize
        {
            get
            {
                return player.BetSize;
            }
            set
            {
                player.BetSize = value;
                if (player.BetSize > 0)
                {
                    BetVisibility = Visibility.Visible;
                }
                else
                {
                    BetVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged(nameof(BetSize));
            }
        }
        public double Balance
        {
            get
            {
                return player.Balance;
            }
            set
            {
                player.Balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }
        public bool IsDealer
        {
            get
            {
                return player.IsDealer;
            }
            set
            {
                player.IsDealer = value;
                OnPropertyChanged(nameof(IsDealer));
            }
        }
        public string Cards
        {
            get
            {
                return player.Cards;
            }
            set
            {
                player.Cards = value;
                OnPropertyChanged(nameof(Cards));
                OnPropertyChanged(nameof(Player.Cards));
                OnPropertyChanged(nameof(Player.ImageSource1));
                OnPropertyChanged(nameof(Player.ImageSource2));
            }
        }
        public ImageSource ImageSourceCard1
        {
            get
            {
                return player.ImageSource1;
            }
            set
            {
                player.ImageSource1 = value;
                OnPropertyChanged(nameof(ImageSourceCard1));
            }
        }
        public ImageSource ImageSourceCard2
        {
            get
            {
                return player.ImageSource2;
            }
            set
            {
                player.ImageSource2 = value;
                OnPropertyChanged(nameof(ImageSourceCard2));
            }
        }
        public ImageSource ImageSourceCard3
        {
            get
            {
                return player.ImageSource3;
            }
            set
            {
                player.ImageSource3 = value;
                OnPropertyChanged(nameof(ImageSourceCard3));
            }
        }
        public ImageSource ImageSourceCard4
        {
            get
            {
                return player.ImageSource4;
            }
            set
            {
                player.ImageSource4 = value;
                OnPropertyChanged(nameof(ImageSourceCard4));
            }
        }
        public Visibility VisibilityDealer
        {
            get
            {
                return player.DealerVisibility;
            }
            set
            {
                player.DealerVisibility = value;
                OnPropertyChanged(nameof(VisibilityDealer));
            }
        }
        public bool IsVisible
        {
            get
            {
                return player.IsVisible;
            }
            set
            {
                player.IsVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        public int RowBet
        {
            get
            {
                return player.RowVisibility;
            }
            set
            {
                player.RowVisibility = value;
                OnPropertyChanged(nameof(RowBet));
            }
        }
        public int ColumnBet
        {
            get
            {
                return player.ColumnVisibility;
            }
            set
            {
                player.ColumnVisibility = value;
                OnPropertyChanged(nameof(ColumnBet));
            }
        }
        public int RowButton
        {
            get
            {
                return player.RowButton;
            }
            set
            {
                player.RowButton = value;
                OnPropertyChanged(nameof(RowButton));
            }
        }
        public int ColumnButton
        {
            get
            {
                return player.ColumnButton;
            }
            set
            {
                player.ColumnButton = value;
                OnPropertyChanged(nameof(ColumnButton));
            }
        }
        public int RowDealer
        {
            get
            {
                return player.RowDealer;
            }
            set
            {
                player.RowDealer = value;
                OnPropertyChanged(nameof(RowDealer));
            }
        }
        public int ColumnDealer
        {
            get
            {
                return player.ColumnDealer;
            }
            set
            {
                player.ColumnDealer = value;
                OnPropertyChanged(nameof(ColumnDealer));
            }
        }
        public int RowStats
        {
            get
            {
                return player.RowVisibilityStats;
            }
            set
            {
                player.RowVisibilityStats = value;
                OnPropertyChanged(nameof(RowStats));
            }
        }
        public int ColumnStats
        {
            get
            {
                return player.ColumnVisibilityStats;
            }
            set
            {
                player.ColumnVisibilityStats = value;
                OnPropertyChanged(nameof(ColumnStats));
            }
        }
        public string Strategy
        {
            get
            {
                return player.StrategyProfile;
            }
            set
            {
                player.StrategyProfile = value;
                OnPropertyChanged(nameof(Strategy));
            }
        }
        public ObservableCollection<string> StrategyProfiles
        {
            get
            {
                return strategyProfiles;
            }
            set
            {
                strategyProfiles = value;
                OnPropertyChanged(nameof(StrategyProfiles));
            }
        }
        public bool InGame
        {
            get
            {
                return player.InGame;
            }
            set
            {
                player.InGame = value;
                if (player.InGame)
                {
                    VisibilityForCards = Visibility.Visible;
                }
                else
                {
                    VisibilityForCards = Visibility.Collapsed;
                }
                OnPropertyChanged(nameof(InGame));
            }
        }
        public bool IsMyTurn
        {
            get
            {
                return player.IsMyTurn;
            }
            set
            {
                player.IsMyTurn = value;
                OnPropertyChanged(nameof(IsMyTurn));
            }
        }
        public Visibility BetVisibility
        {
            get
            {
                return isBetVisible;
            }
            set
            {
                isBetVisible = value;
                OnPropertyChanged(nameof(BetVisibility));
            }
        }
        public Visibility VisibilityForCards
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                OnPropertyChanged(nameof(VisibilityForCards));
            }
        }
        public int IndexOfPlayer { get; set; }
        public string Action
        {
            get
            {
                return player.Action;
            }
            set
            {
                player.Action = value;
                OnPropertyChanged(nameof(Action));
            }
        }
        public bool IsWinner
        {
            get
            {
                return player.IsWinner;
            }
            set
            {
                player.IsWinner = value;
                OnPropertyChanged(nameof(IsWinner));
            }
        }
        public bool IsBot
        {
            get
            {
                return player.IsBot;
            }
            set
            {
                player.IsBot = value;
                OnPropertyChanged(nameof(IsBot));
                if (IsBot)
                {
                    BotHumanImg = ImageChange.GetImageSourceBotHuman("bot.png");
                }
                else
                {
                    BotHumanImg = ImageChange.GetImageSourceBotHuman("human.jpg");
                }
                OnPropertyChanged(nameof(BotHumanImg));
            }
        }
        public int SessionHands
        {
            get
            {
                return player.SessionHands;
            }
            set
            {
                player.SessionHands = value;
                OnPropertyChanged(nameof(SessionHands));
            }
        }
        public ImageSource BotHumanImg
        {
            get
            {
                return player.BotHumanImg;
            }
            set
            {
                player.BotHumanImg = value;
                OnPropertyChanged(nameof(BotHumanImg));
            }
        }
        private HorizontalAlignment horizontalAlignmentStats;
        private VerticalAlignment verticalAlignmentStats;
        public VerticalAlignment VerticalAlignmentStats
        {
            get
            {
                return verticalAlignmentStats;
            }
            set
            {
                verticalAlignmentStats = value;
                OnPropertyChanged(nameof(verticalAlignmentStats));
            }
        }
        public HorizontalAlignment HorizontalAlignmentStats
        {
            get
            {
                return horizontalAlignmentStats;
            }
            set
            {
                horizontalAlignmentStats = value;
                OnPropertyChanged(nameof(HorizontalAlignmentStats));
            }
        }
        #endregion
        #region Methods
        public static PlayerViewModel GetPlayerNLH(bool isVisible, string name, double balance, double betsize, string cards, bool isdealer, ImageSource imageSource1, ImageSource imageSource2, Visibility visibilityDealer, int rowBet, int columnBet, string strategy, bool inGame, bool isMyTurn, int rowBtn, int columnBtn, bool isBot, int rowDealer, int columnDealer, int rowStats, int columnStats, HorizontalAlignment horizontalAlignmentStats, VerticalAlignment verticalAlignmentStats)
        {
            PlayerViewModel playerVM = new PlayerViewModel();
            playerVM.IsVisible = isVisible;
            playerVM.Name = name;
            playerVM.Balance = balance;
            playerVM.BetSize = betsize;
            playerVM.Cards = cards;
            playerVM.IsDealer = isdealer;
            playerVM.ImageSourceCard1 = imageSource1;
            playerVM.ImageSourceCard2 = imageSource2;
            playerVM.VisibilityDealer = visibilityDealer;
            playerVM.RowBet = rowBet;
            playerVM.ColumnBet = columnBet;
            playerVM.Strategy = strategy;
            playerVM.InGame = inGame;
            playerVM.IsMyTurn = isMyTurn;
            playerVM.RowButton = rowBtn;
            playerVM.ColumnButton = columnBtn;
            playerVM.IsBot = isBot;
            playerVM.RowDealer = rowDealer;
            playerVM.ColumnDealer = columnDealer;
            playerVM.RowStats = rowStats;
            playerVM.ColumnStats = columnStats;
            playerVM.HorizontalAlignmentStats = horizontalAlignmentStats;
            playerVM.verticalAlignmentStats = verticalAlignmentStats;
            return playerVM;
        }
        public static PlayerViewModel GetPlayerPLO4(bool isVisible, string name, double balance, double betsize, string cards, bool isdealer, ImageSource imageSource1, ImageSource imageSource2, ImageSource imageSource3, ImageSource imageSource4, Visibility visibilityDealer, int rowBet, int columnBet, string strategy, bool inGame, bool isMyTurn, int rowBtn, int columnBtn, bool isBot, int rowDealer, int columnDealer, int rowStats, int columnStats, HorizontalAlignment horizontalAlignmentStats, VerticalAlignment verticalAlignmentStats)
        {
            PlayerViewModel playerVM = new();
            playerVM.IsVisible = isVisible;
            playerVM.Name = name;
            playerVM.Balance = balance;
            playerVM.BetSize = betsize;
            playerVM.Cards = cards;
            playerVM.IsDealer = isdealer;
            playerVM.ImageSourceCard1 = imageSource1;
            playerVM.ImageSourceCard2 = imageSource2;
            playerVM.ImageSourceCard3 = imageSource3;
            playerVM.ImageSourceCard4 = imageSource4;
            playerVM.VisibilityDealer = visibilityDealer;
            playerVM.RowBet = rowBet;
            playerVM.ColumnBet = columnBet;
            playerVM.Strategy = strategy;
            playerVM.InGame = inGame;
            playerVM.IsMyTurn = isMyTurn;
            playerVM.RowButton = rowBtn;
            playerVM.ColumnButton = columnBtn;
            playerVM.IsBot = isBot;
            playerVM.RowDealer = rowDealer;
            playerVM.ColumnDealer = columnDealer;
            playerVM.RowStats = rowStats;
            playerVM.ColumnStats = columnStats;
            playerVM.HorizontalAlignmentStats = horizontalAlignmentStats;
            playerVM.verticalAlignmentStats = verticalAlignmentStats;
            return playerVM;
        }
        public void ShowCardsForPlayer()
        {
            string card1 = Cards.Substring(0, 2);
            string card2 = Cards.Substring(2, 2);
            ImageSource imgSource1 = ImageChange.GetImageSource(card1);
            ImageSource imgSource2 = ImageChange.GetImageSource(card2);
            ImageSource imgSourceBack = ImageChange.GetCardBack();
            ImageSource currentImageSource1 = ImageSourceCard1;
            if (currentImageSource1.ToString() == imgSourceBack.ToString())
            {
                ImageSourceCard1 = imgSource1;
                ImageSourceCard2 = imgSource2;
            }
            else
            {
                ImageSourceCard1 = imgSourceBack;
                ImageSourceCard2 = imgSourceBack;
            }
            if (Cards.Length > 4)
            {
                string card3 = Cards.Substring(4, 2);
                string card4 = Cards.Substring(6, 2);
                ImageSource imgSource3 = ImageChange.GetImageSource(card3);
                ImageSource imgSource4 = ImageChange.GetImageSource(card4);
                if (currentImageSource1.ToString() == imgSourceBack.ToString())
                {
                    ImageSourceCard3 = imgSource3;
                    ImageSourceCard4 = imgSource4;
                }
                else
                {
                    ImageSourceCard3 = imgSourceBack;
                    ImageSourceCard4 = imgSourceBack;
                }
            }
        }
        public void ShowingIconPlayer()
        {
            if (IsBot)
            {
                BotHumanImg = ImageChange.GetImageSourceBotHuman("human.jpg");
                IsBot = false;
            }
            else
            {
                BotHumanImg = ImageChange.GetImageSourceBotHuman("bot.png");
                IsBot = true;
            }
        }
        public void FoldCards()
        {
            player.InGame = false;
            VisibilityForCards = Visibility.Collapsed;
        }
        bool CanExecuteShow()
        {
            return true;
        }
        bool CanExecute()
        {
            return true;
        }
        public void BotOrHumanClick()
        {
            if (player.IsBot)
            {
                player.IsBot = false;
                BotHumanImg = ImageChange.GetImageSourceBotHuman("human.jpg");
            }
            else
            {
                player.IsBot = true;
                BotHumanImg = ImageChange.GetImageSourceBotHuman("bot.png");
            }
        }
        public void ShowingStatistic()
        {
            StatisticsViewModel s = new StatisticsViewModel(this.Name, Properties.Settings.Default.SelectedEnumCasino, Properties.Settings.Default.GameChoosen);
        }
        #endregion
        #region Commands
        public ICommand ShowCardsPlayerClick { get { return new RelayCommand(ShowCardsForPlayer, CanExecuteShow); } }
        public ICommand FoldCommandForPlayer { get { return new RelayCommand(FoldCards, CanExecute); } }
        public ICommand ShowIconPlayer { get { return new RelayCommand(ShowingIconPlayer, CanExecute); } }
        public ICommand ShowStatistic { get { return new RelayCommand(ShowingStatistic, CanExecute); } }
        public ICommand BotOrHuman { get { return new RelayCommand(BotOrHumanClick, CanExecute); } }
        #endregion        
    }
}

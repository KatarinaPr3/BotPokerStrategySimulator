using DecisionMaking.DecisionMaking;
using MicroMvvm;
using Newtonsoft.Json;
using PokerTable.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace PokerTable.ViewModel
{
    public class WinningsViewModel : ViewModelBase
    {
        #region Members
        private ObservableCollection<WinningModel> winningModels;
        private Dictionary<string, double> startDataWinnings;
        private Dictionary<string, double> sessionWinnings;
        private Dictionary<string, int> sessionHands;
        private List<PlayerViewModel> allPlayersProfilesFromMain;
        private ObservableCollection<PlayerViewModel> playersToPlay;
        private Dictionary<string, double> sessionData;
        private Dictionary<string, double> startData;
        private Dictionary<string, double> allTimeWinnings;
        private Dictionary<string, int> allTimeHands;
        private MainWindowViewModel mainWindowViewModel;
        private string allWinningsPath;
        private string allWinningsHandsPath;
        private string allTimeWinningsFromJson;
        private string allTimeHandsFromJson;
        #endregion
        #region Constructor
        public WinningsViewModel(ref Dictionary<string, double> startData, ref Dictionary<string, double> sessionData, ref Dictionary<string, int> sessionHands, ObservableCollection<PlayerViewModel> playersToPlay, ref Dictionary<string, double> allTimeWinnings, ref Dictionary<string, int> allTimeHands, ref List<PlayerViewModel> allPlayersProfiles, MainWindowViewModel mainViewModel)
        {
            allWinningsPath = @"C:\katarina\winnings.json";
            allWinningsHandsPath = @"C:\katarina\winningsHands.json";
            allPlayersProfilesFromMain = allPlayersProfiles;
            winningModels = new ObservableCollection<WinningModel>();
            this.sessionData = sessionData;
            this.startData = startData;
            this.allTimeWinnings = allTimeWinnings;
            this.allTimeHands = allTimeHands;
            startDataWinnings = startData;
            sessionWinnings = sessionData;
            this.sessionHands = sessionHands;
            this.playersToPlay = playersToPlay;
            mainWindowViewModel = mainViewModel;
            ShowDataInGrid();
        }
        #endregion
        #region Properties
        public ObservableCollection<WinningModel> WinningModels
        {
            get
            {
                return winningModels;
            }
            set
            {
                winningModels = value;
                OnPropertyChanged(nameof(WinningModel));
            }
        }
        #endregion
        #region Methods
        private void ShowDataInGrid()
        {
            foreach (var player in playersToPlay)
            {
                double sessionWinnings = sessionData[player.Name] - startData[player.Name];
                double allTimeWinningsForModel = allTimeWinnings[player.Name];
                WinningModel winModel = WinningModel.CreateWinningModel(player.Name, sessionWinnings, sessionHands[player.Name], allTimeWinningsForModel, allTimeHands[player.Name]);
                WinningModels.Add(winModel);
            }
        }
        private void RestartingAllValues()
        {
            mainWindowViewModel.sessionHands = 0;
            WinningModels.Clear();
            mainWindowViewModel.dictStartBalance = StrategyUtil.DeepCopy<Dictionary<string, double>>(mainWindowViewModel.dictSessionBalance);
            startData = mainWindowViewModel.dictStartBalance;
            for (int i = 0; i < mainWindowViewModel.PlayersToPlay.Count; i++)
            {
                mainWindowViewModel.PlayersToPlay[i].SessionHands = 0;
            }
            mainWindowViewModel.Player1.SessionHands = 0;
            mainWindowViewModel.Player2.SessionHands = 0;
            mainWindowViewModel.Player3.SessionHands = 0;
            mainWindowViewModel.Player4.SessionHands = 0;
            mainWindowViewModel.Player5.SessionHands = 0;
            mainWindowViewModel.Player6.SessionHands = 0;
            mainWindowViewModel.Player7.SessionHands = 0;
            mainWindowViewModel.Player8.SessionHands = 0;
            mainWindowViewModel.Player9.SessionHands = 0;

            foreach (var item in allPlayersProfilesFromMain)
            {
                if (sessionWinnings.ContainsKey(item.Name))
                {
                    sessionHands[item.Name] = mainWindowViewModel.sessionHands;
                }
            }
            allTimeWinningsFromJson = File.ReadAllText(allWinningsPath);
            allTimeHandsFromJson = File.ReadAllText(allWinningsHandsPath);
            Dictionary<string, double> allTimeWinningsDict = new();
            Dictionary<string, int> allTimeHandsDict = new();
            allTimeWinningsDict = JsonConvert.DeserializeObject<Dictionary<string, double>>(allTimeWinningsFromJson);
            allTimeHandsDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(allTimeHandsFromJson);
            foreach (var player in allPlayersProfilesFromMain)
            {
                allTimeWinningsDict[player.Name] = 0;
                allTimeHandsDict[player.Name] = 0;
                mainWindowViewModel.dictallTimeBalanceCurrent[player.Name] = 0;
                mainWindowViewModel.dictAllTimeHandsCurrent[player.Name] = 0;
            }
            allTimeWinningsFromJson = JsonConvert.SerializeObject(allTimeWinningsDict);
            allTimeHandsFromJson = JsonConvert.SerializeObject(allTimeHandsDict);
            mainWindowViewModel.dictallTimeBalance = allTimeWinningsDict;
            mainWindowViewModel.dictAllTimeHands = allTimeHandsDict;
            allTimeHands = allTimeHandsDict;
            allTimeWinnings = allTimeWinningsDict;
            File.WriteAllText(allWinningsPath, allTimeWinningsFromJson);
            File.WriteAllText(allWinningsHandsPath, allTimeHandsFromJson);
            ShowDataInGrid();
        }
        private bool CanExecute()
        {
            return true;
        }
        #endregion
        #region Commands
        public ICommand ResetAllValues { get { return new RelayCommand(RestartingAllValues, CanExecute); } }
        #endregion

    }
}

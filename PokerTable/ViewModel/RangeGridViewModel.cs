namespace PokerTable.ViewModel
{
    public class RangeGridViewModel : ViewModelBase
    {
        #region Members
        private List<string> range;
        #endregion
        #region Constructor
        public RangeGridViewModel()
        {
        }
        #endregion
        #region Properties
        public List<string> Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
                OnPropertyChanged(nameof(Range));
            }
        }
        #endregion
    }
}

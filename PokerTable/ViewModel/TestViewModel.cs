using PokerTable.Model;
using Prism.Mvvm;

namespace PokerTable.ViewModel
{
    class TestViewModel : BindableBase
    {
        #region Members
        private TestModel testModel = new();
        #endregion
        #region Constructor
        public TestViewModel()
        {
            TestModel = new();
            TestModel.DataList = new List<QueryDBLib.StatResult>();
        }
        #endregion
        #region Properties
        public TestModel TestModel
        {
            get { return testModel; }
            set { SetProperty(ref testModel, value); }
        }
        #endregion
    }
}

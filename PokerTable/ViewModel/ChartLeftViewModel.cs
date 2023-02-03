using PokerTable.Model;
using Prism.Mvvm;

namespace PokerTable.ViewModel
{
    public class ChartLeftViewModel : BindableBase
    {
        private TestModel testModel = new();
        public ChartLeftViewModel()
        {
            TestModel = new();
            TestModel.DataList = new List<QueryDBLib.StatResult>();
        }
        public TestModel TestModel
        {
            get { return testModel; }
            set { SetProperty(ref testModel, value); }
        }
    }
}

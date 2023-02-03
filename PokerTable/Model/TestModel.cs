using PokerTable.ViewModel;
using Prism.Mvvm;
using QueryDBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Model
{
    public class TestModel : BindableBase
    {
        private List<StatResult> _DataList = new List<StatResult>();
        public List<StatResult> DataList { get { return _DataList; } set { SetProperty(ref _DataList, value); } }
    }
    public class Keyvalue : BindableBase
    {
        private int _Key;
        public int Key { get { return _Key; } set { SetProperty(ref _Key, value); } }

        private int _Value;
        public int Value { get { return _Value; } set { SetProperty(ref _Value, value); } }
    }
}

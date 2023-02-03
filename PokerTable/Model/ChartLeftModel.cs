using Prism.Mvvm;
using QueryDBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Model
{
    public class ChartLeftModel : BindableBase
    {
        private List<StatResult> _DataList = new();
        public List<StatResult> DataList { get { return _DataList; } set { SetProperty(ref _DataList, value); } }
    }
}

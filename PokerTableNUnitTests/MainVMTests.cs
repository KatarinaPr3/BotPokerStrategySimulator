using DecisionMaking;
using DecisionMaking.DecisionMaking;
using PokerTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTableNUnitTests
{
    [Apartment(ApartmentState.STA)]
    public class MainVMTests
    {
        private PokerTable.ViewModel.MainWindowViewModel mainWindowViewModel { get; set; } = null!;
        

        [SetUp]
        public void Setup()
        {
            mainWindowViewModel = new PokerTable.ViewModel.MainWindowViewModel();            
        }
       
        [Test]
        public void TestDecisionObjectsDeepCopy()
        {
            Decision decision1 = new Decision();
            decision1.Action = EnumDecisionType.CALL;
            decision1.BetSize = 10;
            decision1.Phase = Phase.PREFLOP;
            Decision decision2 = new Decision();
            decision2 =    StrategyUtil.DeepCopy(decision1);
            decision2.Phase = Phase.FLOP;
            decision1 =    StrategyUtil.DeepCopy(decision2);
            Assert.That(decision1.Phase, Is.EqualTo(decision2.Phase));
        }

        [Test]
        public void OmahaTest()
        {
            string cards = "kskdtdth";
            string cards2 = "asad3c3s";
            string publicCards = "kh4d5d9h8c";
            List<string> expected = new List<string>();
            expected.Add("Ana");
            var cardsCompare = PokerTable.ViewModel.MainWindowViewModel.ConcludeUsingEquity("Ana", "Ana2", cards, cards2, publicCards);
            CollectionAssert.AreEqual(cardsCompare, expected);
        }
    }
}

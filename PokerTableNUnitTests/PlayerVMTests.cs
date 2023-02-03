using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PokerTableNUnitTests
{
    public class PlayerVMTests
    {
        [Test]
        public void CanGetPlayerViewModel()
        {
            // arrange
            var visibility = System.Windows.Visibility.Visible;
            var imgPath = @"\Images\Karte\card_Kd.png";
            var converter = new ImageSourceConverter();
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            imgPath = dirName + imgPath;
            ImageSource imgSourceGet = (ImageSource)converter.ConvertFromString(imgPath);
            var getPlayer = PokerTable.ViewModel.PlayerViewModel.GetPlayerNLH(true, "Ana", 32.32, 2.2, "KdKs", true, imgSourceGet, imgSourceGet, visibility, 1, 1, "Strategy", true, false, 2, 2, false, 0 , 1, 0 , 1, System.Windows.HorizontalAlignment.Left, System.Windows.VerticalAlignment.Top);

            // act
            //assert

            // Assert.AreEqual(getPlayer.Strategy, "Strategy");
            Assert.AreEqual(getPlayer.Cards, "KdKs");
        }
    }
}

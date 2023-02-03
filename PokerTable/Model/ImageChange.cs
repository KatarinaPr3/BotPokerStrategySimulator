using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PokerTable.Model
{
    public class ImageChange
    {
        #region Methods
        #region ImageSource & CardBack
        public static ImageSource GetCardBack()
        {
            var imgPath = @"\Images\Slike\cardBack.png";
            var converter = new ImageSourceConverter();
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            imgPath = dirName + imgPath;
            ImageSource imgSourceGet = (ImageSource)converter.ConvertFromString(imgPath);
            return imgSourceGet;
        }
        /// <summary>
        /// This method gets ImageSource for cards
        /// </summary>
        /// <param name="imgSource">name of card</param>
        public static ImageSource GetImageSource(string imgSource)
        {
            var imgPath = @"\Images\Karte\card_" + imgSource + ".png";
            var converter = new ImageSourceConverter();
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            imgPath = dirName + imgPath;
            ImageSource imgSourceGet = (ImageSource)converter.ConvertFromString(imgPath);
            return imgSourceGet;
        }
        /// <summary>
        /// This method gets ImageSource for Human or bot logo
        /// </summary>
        /// <param name="imgSource">name of card human or bot</param>
        public static ImageSource GetImageSourceBotHuman(string imgSource)
        {
            var imgPath = @"\Images\Slike\" + imgSource;
            var converter = new ImageSourceConverter();
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            imgPath = dirName + imgPath;

            ImageSource imgSourceGet = (ImageSource)converter.ConvertFromString(imgPath);
            return imgSourceGet;
        }
        #endregion
        #endregion
    }
}

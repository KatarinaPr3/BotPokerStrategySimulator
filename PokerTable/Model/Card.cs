namespace PokerTable.Model
{
    public class Card
    {
        #region Members
        public static Random r = new Random();
        #endregion
        #region Methods
        public static List<string> Deck()
        {
            string[] suits = new string[] { "s", "d", "h", "c" };
            string[] ranks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };

            var cards = ranks.SelectMany(rank => suits.Select(suit => rank + suit)).ToList();
            return cards;
        }
        public static List<string> RandomCards(int numCards, ref List<string> CardsDeck)
        {
            var randomCards = new List<string>(numCards);
            for (int i = 0; i < numCards; i++)
            {
                int randomIndex = r.Next(CardsDeck.Count);
                randomCards.Add(CardsDeck[randomIndex]);
                CardsDeck.RemoveAt(randomIndex);
            }
            return randomCards;
        }
        #endregion
    }
}

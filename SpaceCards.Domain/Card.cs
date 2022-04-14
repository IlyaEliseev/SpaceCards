namespace SpaceCards.Domain
{
    public class Card
    {
        public Card(int id, string word, string wordTranslate)
        {
            Id = id;
            Word = word;
            WordTranslate = wordTranslate;
        }

        public int Id { get; }

        public string Word { get; }

        public string WordTranslate { get; }

        public static (Card? Result, string[] Errors) Create(string word, string wordTranslate)
        {
            if (string.IsNullOrWhiteSpace(wordTranslate) && string.IsNullOrWhiteSpace(wordTranslate))
            {
                return (null, new[] { $"'{nameof(word)}' and '{nameof(wordTranslate)}' cannot be null or whitespace." });
            }

            if (string.IsNullOrWhiteSpace(word))
            {
                return (null, new[] { $"'{nameof(word)}' cannot be null or whitespace." });
            }

            if (string.IsNullOrWhiteSpace(wordTranslate))
            {
                return (null, new[] { $"'{nameof(wordTranslate)}' cannot be null or whitespace." });
            }

            var card = new Card(0, word, wordTranslate);
            CardRepository.Cards.Add(card);

            return (card, Array.Empty<string>());
        }

        public static Card[] Get()
        {
            var card = CardRepository.Cards.ToArray();

            return card;
        }

        public static (bool Result, string[] Errors) Delete(string cardWord)
        {
            if (string.IsNullOrWhiteSpace(cardWord))
            {
                return (false, new[] { $"'{nameof(cardWord)}' cannot be null or whitespace." });
            }

            var deletedCard = CardRepository.Cards
                .FirstOrDefault(x => x.Word == cardWord);

            if (deletedCard == null)
            {
                return (false, new[] { $"Card with this {nameof(cardWord)} not found" });
            }

            CardRepository.Cards.Remove(deletedCard);

            return (true, Array.Empty<string>());
        }

        public static (bool Result, string[] Errors) Update(string cardWord, string cardUpdateWord, string cardUpdateWordTranslate)
        {
            if (string.IsNullOrWhiteSpace(cardUpdateWord))
            {
                return (false, new[] { $"'{nameof(cardUpdateWord)}' cannot be null or whitespace." });
            }

            if (string.IsNullOrWhiteSpace(cardUpdateWordTranslate))
            {
                return (false, new[] { $"'{nameof(cardUpdateWordTranslate)}' cannot be null or whitespace." });
            }

            var cards = CardRepository.Cards;
            var findCard = cards.FirstOrDefault(x => x.Word == cardWord);

            if (findCard == null)
            {
                return (false, new[] { $"Card with this {nameof(cardWord)} not found." });
            }

            var updatedCard = new Card(0, cardUpdateWord, cardUpdateWordTranslate);
            cards[cards.IndexOf(findCard)] = updatedCard;

            return (true, Array.Empty<string>());
        }
    }
}

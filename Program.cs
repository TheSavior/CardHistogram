using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardHistogram
{
    // Rules of the game
    // Flip over a card
    // if the card flipped and the one 4 before it are the same number
    // discard all 4 cards
    // if they are the same suit, but different number
    // discard the two cards in the middle
    // otherwise flip another card
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            int[] left = new int[52];
            int games = 1000000;
            // Play the game a bunch of times times
            for (int i = 0; i < games; i++)
            {
                var cardsLeft = PlayGame(random);
                left[cardsLeft]++;
            }

            using (StreamWriter file = new StreamWriter(@"histogram.txt"))
            {
                for (int i = 0; i < left.Length; i+=2)
                {
                    file.WriteLine((i + "\t" + left[i] / (float)games));
                }
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static int PlayGame(Random rand)
        {
            var deck = new Deck();
            var cards = deck.getShuffled(rand);
            
            int index1 = 0;
            int index2 = 3;

            while (index2 < cards.Count)
            {
                if (cards[index1].value == cards[index2].value)
                {
                    cards.RemoveRange(index1, 4);

                    // naive approach, restart our indexes
                    index1 = 0;
                    index2 = 4;
                }
                else if (cards[index1].suit == cards[index2].suit)
                {
                    cards.RemoveRange(index1 + 1, 2);

                    // naive approach, restart our indexes
                    index1 = 0;
                    index2 = 4;
                }
                index1++;
                index2++;
            }

            return cards.Count;
        }
    }

    class Deck
    {
        private List<Card> deck;

        public Deck() {
            // Build our deck

            List<Card> cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach(Value value in Enum.GetValues(typeof(Value)))
                {
                    cards.Add(new Card(suit, value));
                }
            }

            deck = cards;
        }

        public List<Card> getShuffled(Random rand)
        {
            var cards = new List<Card>();

            while (deck.Count > 0)
            {
                int index = rand.Next(deck.Count);
                cards.Add(deck[index]);
                deck.RemoveAt(index);
            }

            return cards;
        }
    }

    class Card
    {
        public Suit suit { get; private set; }
        public Value value { get; private set; }

        public Card(Suit suit, Value value) {
            this.suit = suit;
            this.value = value;
        }

        public override string ToString()
        {
            return value + " of " + suit;
        }
    }

    enum Suit
    {
        Heart, Spade, Diamond, Club
    }

    enum Value
    {
       Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
    }
}

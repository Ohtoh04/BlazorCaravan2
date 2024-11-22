using CaravanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaravanDomain.Models {
    public class Deck {
        private List<Card> _deck = new List<Card>();

        public int Count {
            get { return _deck.Count; }
        }
        public Deck() {
            foreach (CardRank cardRank in Enum.GetValues(typeof(CardRank))) {
                foreach (CardSuit cardSuit in Enum.GetValues(typeof(CardSuit))) {
                    _deck.Add(new Card(cardRank, cardSuit));
                }
            }
            //_deck.Remove(_deck.First(crd => crd.CardSuit == CardSuit.Hearts && crd.CardNumber == CardRank.Joker));
            //_deck.Remove(_deck.First(crd => crd.CardSuit == CardSuit.Spades && crd.CardNumber == CardRank.Joker));
            Shuffle();
        }

        public void Shuffle() {
            var rand = new Random();
            _deck = new List<Card>(_deck.OrderBy(a => rand.Next()));
        }

        public Card? Draw() {
            if (_deck.Count == 0) return null;
            var card = _deck[0];
            _deck.RemoveAt(0);
            return card;
        }
    }
}

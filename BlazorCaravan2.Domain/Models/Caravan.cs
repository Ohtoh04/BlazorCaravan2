using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaravanDomain.Entities;

namespace CaravanDomain.Models {
    public class Caravan {
        public List<Card> Cards { get; private set; } = new List<Card>();
        public int TotalValue { get; set; } = 0;
        private bool _isReversed = false;

        public void ReverseDirection() {
            _isReversed = !_isReversed;
        }

        public void RemoveAllCardsOfSuitExcept(CardSuit suit, Card exceptCard) {
            Cards.RemoveAll(c => c.CardSuit == suit && c != exceptCard);
        }

        public void RemoveAllCardsOfValueExcept(CardRank rank, Card exceptCard) {
            Cards.RemoveAll(c => c.CardNumber == rank && c != exceptCard);
        }
    }
}

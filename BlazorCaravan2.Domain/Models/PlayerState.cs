using CaravanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaravanDomain.Models {
    public class PlayerState {
        public List<Card> Hand = new List<Card>();
        public List<Caravan> Caravans = new List<Caravan>();
        public void PlayCard(Card card) {
            Hand.Remove(card);
            // Additional logic to place card on a caravan
        }

        public Card DrawCard() {
            // Draw logic here
            var drawnCard = new Card(CardRank.Ace, CardSuit.Clubs);  // Placeholder
            Hand.Add(drawnCard);
            return drawnCard;
        }
    }
}

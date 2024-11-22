using CaravanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaravanDomain.Models {
    public class PlayerState {
        public List<Card> Hand { get; set; } = new List<Card>();
        public List<Caravan> Caravans { get; set; } = new List<Caravan>() { new Caravan(), new Caravan(), new Caravan() };
        public Deck Deck { get; set; } = new Deck();
        public void PlayCard(Card card) {
            Hand.Remove(card);
            // Additional logic to place card on a caravan
        }

        public Card DrawCard() {
            var drawnCard = Deck.Draw();
            Hand.Add(drawnCard);
            return drawnCard;
        }
    }
}

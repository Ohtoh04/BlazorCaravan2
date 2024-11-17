using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaravanDomain.Entities {
    public class Card
    {
        public CardRank CardNumber { get; set; }
        public CardSuit CardSuit { get; set; }

        public Card(CardRank cardNumber, CardSuit cardSuit)
        {
            CardNumber = cardNumber;
            CardSuit = cardSuit;
        }
    }
}

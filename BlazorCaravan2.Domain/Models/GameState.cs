using CaravanDomain.Entities;
using CaravanDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaravanDomain.Models {
    public class GameState {
        public GameState(List<Card> yourHand, List<Caravan> yourCaravans, List<Caravan> opponentCaravans, int opponentsHandCount, int yourDeckCount, int opponentsDeckCount) {
            YourHand = yourHand;
            YourCaravans = yourCaravans;
            OpponentCaravans = opponentCaravans;
            OpponentsHandCount = opponentsHandCount;
            YourDeckCount = yourDeckCount;
            OpponentsDeckCount = opponentsDeckCount;
        }
        
        public int OpponentsHandCount { get; set; }
        public int YourDeckCount { get; set; }
        public int OpponentsDeckCount { get; set; }
        public List<Card> YourHand { get; set; } = new List<Card>();
        public List<Caravan> YourCaravans { get; set; } = new List<Caravan>() { new Caravan(), new Caravan(), new Caravan() };
        public List<Caravan> OpponentCaravans { get; set; } = new List<Caravan>() { new Caravan(), new Caravan(), new Caravan() };

    }
}

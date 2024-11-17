using CaravanDomain.Entities;
using CaravanDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaravanDomain.Models {
    public class GameState {
        public List<Card> YourHand { get; set; } = new List<Card>();
        public List<Caravan> YourCaravans { get; set; } = new List<Caravan>() { new Caravan(), new Caravan(), new Caravan() };
        public List<Caravan> OpponentCaravans { get; set; } = new List<Caravan>() { new Caravan(), new Caravan(), new Caravan() };

    }
}

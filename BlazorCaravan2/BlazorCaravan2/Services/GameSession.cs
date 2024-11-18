using CaravanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaravanDomain.Models {
    public class GameSession {
        private Deck _deck;
        private List<string> _players = new List<string>();
        private Dictionary<string, PlayerState> _playerStates = new Dictionary<string, PlayerState>();
        private int _currentTurn = 0;

        public void AddPlayer(string playerId) {
            if (_players.Count < 2) {
                _players.Add(playerId);
                _playerStates[playerId] = new PlayerState();
            }
        }

        public bool CanStart() => _players.Count == 2;

        public void StartGame() {
            _deck = new Deck();
            _deck.Shuffle();

            // Deal initial hand to each player
            foreach (var playerId in _players) {
                var playerState = _playerStates[playerId];
                for (int i = 0; i < 8; i++) {
                    playerState.Hand.Add(_deck.Draw());
                }
            }
        }

        public bool IsPlayerTurn(string playerId) {
            return _players[_currentTurn] == playerId;
        }

        public void PlayCard(string playerId, Card card, int caravanIndex) {
            if (IsPlayerTurn(playerId)) {
                var playerState = _playerStates[playerId];
                if (!playerState.Hand.Contains(card)) return;

                playerState.Hand.Remove(card);
                var caravan = playerState.Caravans[caravanIndex];
                caravan.Cards.Add(card);

                ApplyCardEffect(caravan, card);

                EndTurn();
            }
        }

        private void ApplyCardEffect(Caravan caravan, Card card) {
            switch (card.CardNumber) {
                case CardRank.Joker:
                    ApplyJokerEffect(caravan, card);
                    break;
                case CardRank.Jack:
                    ApplyJackEffect(caravan, card);
                    break;
                case CardRank.Queen:
                    ApplyQueenEffect(caravan);
                    break;
                case CardRank.King:
                    ApplyKingEffect(caravan, card);
                    break;
                // 2-10 and Ace add normally to the caravan
                default:
                    caravan.TotalValue += (int)card.CardNumber;
                    break;
            }
        }

        private void ApplyJokerEffect(Caravan caravan, Card joker) {
            var targetCard = caravan.Cards[^2];
            if (targetCard.CardNumber == CardRank.Ace) {
                caravan.RemoveAllCardsOfSuitExcept(targetCard.CardSuit, targetCard);
            }
            else if (targetCard.CardNumber >= CardRank.Two && targetCard.CardNumber <= CardRank.Ten) {
                caravan.RemoveAllCardsOfValueExcept(targetCard.CardNumber, targetCard);
            }
        }

        private void ApplyJackEffect(Caravan caravan, Card jack) {
            var targetCard = caravan.Cards[^2];
            caravan.Cards.Remove(targetCard);
        }

        private void ApplyQueenEffect(Caravan caravan) {
            caravan.ReverseDirection();
        }

        private void ApplyKingEffect(Caravan caravan, Card king) {
            var targetCard = caravan.Cards[^2];
            caravan.TotalValue += (int)targetCard.CardNumber;
        }

        public Card DrawCard(string playerId) {
            return _playerStates[playerId].DrawCard();
        }

        public void EndTurn() {
            _currentTurn = (_currentTurn + 1) % _players.Count;
        }

        public bool IsGameOver() {
            int player1SoldCount = 0;
            int player2SoldCount = 0;

            // Check each of the three caravans
            for (int i = 0; i < 3; i++) {
                var player1Caravan = _playerStates[_players[0]].Caravans[i];
                var player2Caravan = _playerStates[_players[1]].Caravans[i];

                // Check if each player's caravan is "sold" (i.e., within 21-26 range)
                bool player1Sold = IsCaravanSold(player1Caravan);
                bool player2Sold = IsCaravanSold(player2Caravan);

                // Determine the outcome for this caravan
                if (player1Sold && player2Sold) {
                    // Tie - no winner for this caravan
                    continue;
                }
                else if (player1Sold) {
                    player1SoldCount++;
                }
                else if (player2Sold) {
                    player2SoldCount++;
                }
            }

            // Game is over if all caravans are sold (no ties) and a player has 2 or more sales
            bool allCaravansSold = (player1SoldCount + player2SoldCount == 3);
            bool hasWinner = player1SoldCount >= 2 || player2SoldCount >= 2;

            return allCaravansSold && hasWinner;
        }

        // Helper method to determine if a caravan is sold
        private bool IsCaravanSold(Caravan caravan) {
            return caravan.TotalValue >= 21 && caravan.TotalValue <= 26;
        }

        public string? GetWinner() {
            int player1SoldCount = 0;
            int player2SoldCount = 0;

            // Count sold caravans again to determine winner
            for (int i = 0; i < 3; i++) {
                var player1Caravan = _playerStates[_players[0]].Caravans[i];
                var player2Caravan = _playerStates[_players[1]].Caravans[i];

                bool player1Sold = IsCaravanSold(player1Caravan);
                bool player2Sold = IsCaravanSold(player2Caravan);

                if (player1Sold && player2Sold) continue;
                if (player1Sold) player1SoldCount++;
                if (player2Sold) player2SoldCount++;
            }

            if (player1SoldCount >= 2) return _players[0];
            if (player2SoldCount >= 2) return _players[1];

            // Null if both caravans are sold
            return null; 
        }

        public Dictionary<string, PlayerState> GetGameState() {
            return new Dictionary<string, PlayerState>(_playerStates);
        }
    }

}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaravanDomain.Entities;
using CaravanDomain.Models;
using System.Collections.Concurrent;

namespace BlazorCaravan2.Hubs {
    public class CaravanHub : Hub {
        private static ConcurrentDictionary<string, GameSession> Sessions = new ConcurrentDictionary<string, GameSession>();

        // Called when a player joins a game session
        public async Task JoinGame(string sessionId, string playerId) {
            if (Sessions.TryGetValue(sessionId, out var session)) {
                // Add the player to the session
                session.AddPlayer(playerId);

                // Notify the player they have joined successfully
                await Clients.Caller.SendAsync("GameJoined", sessionId, playerId);

                // Notify other players in the session
                await Clients.Group(sessionId).SendAsync("PlayerJoined", Sessions[sessionId].Players);

                // Add the player to the SignalR group for the session
                await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            } else {
                // Notify the player that the session does not exist
                await Clients.Caller.SendAsync("Error", "Session not found");
            }
        }

        // Starts the game once both players have joined
        public async Task StartGame(string sessionId) {
            if (Sessions.TryGetValue(sessionId, out var session) && session.CanStart()) {
                session.StartGame();
                await Clients.Group(sessionId).SendAsync("GameStarted", sessionId, session.GetGameState());
            }
        }
        public async Task CreateGame(string sessionId) {
            GameSession gameSession = new GameSession();
            Sessions.GetOrAdd(sessionId, gameSession);
            await Clients.Group(sessionId).SendAsync("GameCreated", sessionId);
        }

        // Play a card
        public async Task PlayCard(string sessionId, string playerId, Card card, int caravanIndex) {
            if (Sessions.TryGetValue(sessionId, out var session) && session.IsPlayerTurn(playerId)) {
                session.PlayCard(playerId, card, caravanIndex);
                await Clients.Group(sessionId).SendAsync("CardPlayed", session.GetGameState());

                if (session.IsGameOver()) {
                    var winner = session.GetWinner();
                    await Clients.Group(sessionId).SendAsync("GameOver", winner);
                }
            }
        }


        // Draw a card
        public async Task DrawCard(string sessionId, string playerId) {
            if (Sessions.TryGetValue(sessionId, out var session) && session.IsPlayerTurn(playerId)) {
                var card = session.DrawCard(playerId);
                await Clients.Caller.SendAsync("CardDrawn", card);
                await Clients.Group(sessionId).SendAsync("GameUpdated", session.GetGameState());
            }
        }

        // Ends the current player's turn
        public async Task EndTurn(string sessionId, string playerId) {
            if (Sessions.TryGetValue(sessionId, out var session) && session.IsPlayerTurn(playerId)) {
                session.EndTurn();
                await Clients.Group(sessionId).SendAsync("TurnEnded", session.GetGameState());
            }
        }
    }

}


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
using BlazorCaravan2.Data;

namespace BlazorCaravan2.Hubs {
    public class CaravanHub : Hub {
        private readonly UserManager<ApplicationUser> _userManager;
        private static ConcurrentDictionary<string, GameSession> Sessions = new ConcurrentDictionary<string, GameSession>();

        public CaravanHub(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        // Called when a player joins a game session
        public async Task JoinGame(string sessionId, string playerId) {
            if (Sessions.TryGetValue(sessionId, out var session)) {
                // Add the player to the session
                session.AddPlayer(playerId);

                // Add the player to the SignalR group for the session
                await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

                // Notify the player they have joined successfully
                await Clients.Caller.SendAsync("GameJoined", Sessions[sessionId].Players);

                // Notify other players in the session
                await Clients.Group(sessionId).SendAsync("PlayerJoined", Sessions[sessionId].Players);


            } else {
                // Notify the player that the session does not exist
                await Clients.Caller.SendAsync("Error", "Session not found");
            }
        }

        // Starts the game once both players have joined
        public async Task StartGame(string sessionId, string playerId) {
            if (Sessions.TryGetValue(sessionId, out var session) && session.CanStart()) {
                session.StartGame();

                foreach (var player in session.Players) {
                    var gameState = session.GetGameState(player);
                    var user = await _userManager.FindByIdAsync(player);
                    await Clients.Group(sessionId).SendAsync("GameStarted", sessionId, gameState, player);
                }
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

                foreach (var player in session.Players) {
                    var gameState = session.GetGameState(player);
                    await Clients.Group(sessionId).SendAsync("CardPlayed", gameState, player);

                }

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

                foreach (var player in session.Players) {
                    var gameState = session.GetGameState(player);
                    await Clients.Group(sessionId).SendAsync("GameUpdated", gameState, player);

                }
            }
        }

        // Ends the current player's turn
        public async Task EndTurn(string sessionId, string playerId) {
            if (Sessions.TryGetValue(sessionId, out var session) && session.IsPlayerTurn(playerId)) {
                session.EndTurn();
                foreach (var player in session.Players) {
                    var gameState = session.GetGameState(player);
                    await Clients.Group(sessionId).SendAsync("TurnEnded", gameState, playerId);
                }
            }
        }
    }

}


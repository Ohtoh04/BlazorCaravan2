using BlazorCaravan2.Data;
using CaravanDomain.Models;

namespace BlazorCaravan2.Services {
    public class GameService : IGameService {
        //private ApplicationDbContext _context;
        public Task<ResponseData<GameSession>> CreateGame(GameSession gameSession) {
            throw new NotImplementedException();
        }

        public Task DeleteGame(int id) {
            throw new NotImplementedException();
        }

        public Task<ResponseData<GameSession>> GetGameById(int id) {
            throw new NotImplementedException();
        }

        public Task<ResponseData<List<GameSession>>> GetGamesList() {
            throw new NotImplementedException();
        }
    }
}

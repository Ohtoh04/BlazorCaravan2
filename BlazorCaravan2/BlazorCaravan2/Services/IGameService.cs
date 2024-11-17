using CaravanDomain.Models;

namespace BlazorCaravan2.Services {
    public interface IGameService {
        Task<ResponseData<GameSession>> CreateGame(GameSession gameSession);

        Task<ResponseData<List<GameSession>>> GetGamesList();

        Task<ResponseData<GameSession>> GetGameById(int id);

        Task DeleteGame(int id);
    }
}

using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorCaravan2.Client.Services {
    public interface IGameHubService {
        public Task StartConnectionAsync(string hubUrl);
        public Task StopConnectionAsync();
        public Task SendAsync(string methodName, params object[] args);
        public void On(string methodName, Action<object> handler);
    }
}

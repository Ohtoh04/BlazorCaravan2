
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorCaravan2.Client.Services {
    public class GameHubService : IGameHubService {
        private HubConnection _hubConnection;
        public HubConnectionState ConnectionState => _hubConnection?.State ?? HubConnectionState.Disconnected;


        public void On(string methodName, Action<object> handler) {
            throw new NotImplementedException();
        }

        public Task SendAsync(string methodName, params object[] args) {
            throw new NotImplementedException();
        }

        public Task StartConnectionAsync(string hubUrl) {
            throw new NotImplementedException();
        }

        public Task StopConnectionAsync() {
            throw new NotImplementedException();
        }
    }
}

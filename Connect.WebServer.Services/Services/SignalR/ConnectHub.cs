using Microsoft.AspNetCore.SignalR;

namespace Connect.WebServer.Services
{
    public class ConnectHub : Hub
    {
        public async Task AddToLocation(string locationId)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, locationId);
        }

        public async Task RemoveFromLocation(string locationId)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, locationId);
        }

        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}

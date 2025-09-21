using Microsoft.AspNetCore.SignalR;

namespace RoadGuard.Hubs
{
    public class RoadGuardHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", $"Welcome! Your connectionId: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task UpdateLocation(double latitude, double longitude)
        {
            await Clients.Others.SendAsync("UserLocationUpdated", Context.ConnectionId, latitude, longitude);
        }

        public async Task ReportCreated(Guid reportId, double latitude, double longitude, string type, string description)
        {
            await Clients.All.SendAsync("ReportCreated", new
            {
                ReportId = reportId,
                Latitude = latitude,
                Longitude = longitude,
                Type = type,
                Description = description
            });
        }
        
        public async Task ReportExpired(Guid reportId)
        {
            await Clients.All.SendAsync("ReportExpired", reportId);
        }
    }
}

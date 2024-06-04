using CheckDrive.Domain.Interfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CheckDrive.Services.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        public async Task SendPrivateRequest(string userId, string massege)
        {
            await Clients.User(userId).SendAsync("ReceiveRequest", massege);
        }

        public async Task SendPrivateResponse(string employeeId, bool isApproved)
        {
            await Clients.User(employeeId).SendAsync("ReceiveResponse", isApproved);
        }
    }
}

using CheckDrive.Domain.Interfaces.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace CheckDrive.Services.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IHubContext<ChatHub> _context;
        private static ConcurrentDictionary<string, string> userConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(ILogger<ChatHub> logger, IHubContext<ChatHub> context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task SendPrivateRequest(string userId, string message)
        {
            _logger.LogInformation($"SendPrivateMessage: {userId}, {message}");
            if (userConnections.TryGetValue(userId, out var connectionId))
            {
                await _context.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
            else
            {
                _logger.LogWarning($"User {userId} is not connected.");
            }
        }

        public async Task SendPrivateResponse(string employeeId, bool isApproved)
        {
            _logger.LogInformation($"SendPrivateResponse: {employeeId}, {isApproved}");
            await Clients.User(employeeId).SendAsync("ReceiveResponse", isApproved);
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            userConnections[userId] = Context.ConnectionId;
            _logger.LogInformation($"User connected: {userId}, ConnectionId: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"User disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}

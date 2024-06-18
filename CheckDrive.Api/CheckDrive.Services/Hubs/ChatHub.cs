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
        private static ConcurrentDictionary<string, List<(int, string)>> undeliveredMessages = new ConcurrentDictionary<string, List<(int, string)>>();

        public ChatHub(ILogger<ChatHub> logger, IHubContext<ChatHub> context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task SendPrivateRequest(int id, string userId, string message)
        {
            _logger.LogInformation($"SendPrivateMessage: {userId}, {message}");
            if (userConnections.TryGetValue(userId, out var connectionId))
            {
                await _context.Clients.Client(connectionId).SendAsync("ReceiveMessage", id, message);
            }
            else
            {
                _logger.LogWarning($"User {userId} is not connected. Storing message.");
                StoreUndeliveredMessage(id, userId, message);
            }
        }

        public async Task ReceivePrivateResponse(bool response)
        {
            _logger.LogInformation($"Response received: {response}");
            // Обработайте ответ здесь, если необходимо
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            userConnections[userId] = Context.ConnectionId;
            _logger.LogInformation($"User connected: {userId}, ConnectionId: {Context.ConnectionId}");

            await SendPendingMessages(userId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            userConnections.TryRemove(userId, out _);
            _logger.LogInformation($"User disconnected: {userId}");
            await base.OnDisconnectedAsync(exception);
        }

        private void StoreUndeliveredMessage(int id, string userId, string message)
        {
            undeliveredMessages.AddOrUpdate(userId, new List<(int, string)> { (id, message) }, (key, existingList) =>
            {
                existingList.Add((id, message));
                return existingList;
            });
        }

        private async Task SendPendingMessages(string userId)
        {
            if (undeliveredMessages.TryRemove(userId, out var messages))
            {
                foreach (var message in messages)
                {
                    var (id, context) = message;
                    if (userConnections.TryGetValue(userId, out var connectionId))
                    {
                        await _context.Clients.Client(connectionId).SendAsync("ReceiveMessage", id, context);
                    }
                }
            }
        }
    }
}

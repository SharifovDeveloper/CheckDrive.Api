using CheckDrive.ApiContracts;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace CheckDrive.Services.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IHubContext<ChatHub> _context;
        private readonly CheckDriveDbContext _dbContext;
        private static ConcurrentDictionary<string, string> userConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(ILogger<ChatHub> logger, IHubContext<ChatHub> context, CheckDriveDbContext checkDriveDbContext)
        {
            _logger = logger;
            _context = context;
            _dbContext = checkDriveDbContext ?? throw new ArgumentNullException(nameof(checkDriveDbContext));
        }

        public async Task SendPrivateRequest(SendingMessageStatus sendingMessageStatus, int reviewId, string userId, string message)
        {
            _logger.LogInformation($"SendPrivateMessage: {userId}, {message}");
            if (userConnections.TryGetValue(userId, out var connectionId))
            {
                await _context.Clients.Client(connectionId).SendAsync("ReceiveMessage", sendingMessageStatus, reviewId, message);
            }
            else
            {
                _logger.LogWarning($"User {userId} is not connected. Storing message.");
                StoreUndeliveredMessage(sendingMessageStatus, reviewId, userId, message);
            }
        }

        public async Task ReceivePrivateResponse(int statusReview, int reviewId, bool response)
        {
            _logger.LogInformation($"Response received:{statusReview} {reviewId} {response}");
            try
            {
                switch (statusReview)
                {
                    case 0:
                        await UpdateStatusForMechanicHandover(reviewId, response);
                        break;
                    case 1:
                        await UpdateStatusForOperatorReview(reviewId, response);
                        break;
                    case 2:
                        await UpdateStatusForMechanicAcceptances(reviewId, response);
                        break;
                    default:
                        _logger.LogWarning($"Unknown status review: {statusReview}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating status review for reviewId: {reviewId}");
            }
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

        private void StoreUndeliveredMessage(SendingMessageStatus sendingMessageStatus, int reviewId, string userId, string message)
        {
            undeliveredMessages.AddOrUpdate(userId, new List<(SendingMessageStatus sendingMessageStatus, int, string)> { (sendingMessageStatus, reviewId, message) }, (key, existingList) =>
            {
                existingList.Add((sendingMessageStatus, reviewId, message));
                return existingList;
            });
        }

        private async Task SendPendingMessages(string userId)
        {
            if (undeliveredMessages.TryRemove(userId, out var messages))
            {
                foreach (var message in messages)
                {
                    var (sendingMessageStatus, reviewId, context) = message;
                    if (userConnections.TryGetValue(userId, out var connectionId))
                    {
                        await _context.Clients.Client(connectionId).SendAsync("ReceiveMessage", sendingMessageStatus, reviewId, context);
                    }
                }
            }
        }

        private async Task UpdateStatusForOperatorReview(int reviewId, bool response)
        {
            var operatorReview = await _dbContext.OperatorReviews
                .FirstOrDefaultAsync(x => x.Id == reviewId);

            operatorReview.Status = (Status)(response ? StatusForDto.Completed : StatusForDto.RejectedByDriver);

            _dbContext.OperatorReviews.Update(operatorReview);
            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateStatusForMechanicHandover(int reviewId, bool response)
        {
            var operatorReview = await _dbContext.MechanicsHandovers
                .FirstOrDefaultAsync(x => x.Id == reviewId);

            operatorReview.Status = (Status)(response ? StatusForDto.Completed : StatusForDto.RejectedByDriver);

            _dbContext.MechanicsHandovers.Update(operatorReview);
            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateStatusForMechanicAcceptances(int reviewId, bool response)
        {
            var operatorReview = await _dbContext.MechanicsAcceptances
                .FirstOrDefaultAsync(x => x.Id == reviewId);

            operatorReview.Status = (Status)(response ? StatusForDto.Completed : StatusForDto.RejectedByDriver);

            _dbContext.MechanicsAcceptances.Update(operatorReview);
            await _dbContext.SaveChangesAsync();
        }
    }
}

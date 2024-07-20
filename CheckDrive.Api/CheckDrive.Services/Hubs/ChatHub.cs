using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _context;
        private readonly CheckDriveDbContext _dbContext;
        private static ConcurrentDictionary<string, string> userConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(ILogger<ChatHub> logger, IMapper mapper, IHubContext<ChatHub> context, CheckDriveDbContext checkDriveDbContext)
        {
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context;
            _dbContext = checkDriveDbContext ?? throw new ArgumentNullException(nameof(checkDriveDbContext));
        }

        public async Task SendPrivateRequest(UndeliveredMessageForDto undeliveredMessageForDto)
        {
            _logger.LogInformation($"SendPrivateMessage: {undeliveredMessageForDto.UserId}, {undeliveredMessageForDto.Message}");
            if (userConnections.TryGetValue(undeliveredMessageForDto.UserId, out var connectionId))
            {
                await _context.Clients.Client(connectionId).SendAsync("ReceiveMessage", undeliveredMessageForDto.SendingMessageStatus, undeliveredMessageForDto.ReviewId, undeliveredMessageForDto.Message);
            }
            else
            {
                _logger.LogWarning($"User {undeliveredMessageForDto.UserId} is not connected. Storing message.");
                StoreUndeliveredMessage(undeliveredMessageForDto);
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

        private async Task StoreUndeliveredMessage(UndeliveredMessageForDto undeliveredMessageForDto)
        {
            var undeliveredMassage = _mapper.Map<UndeliveredMessage>(undeliveredMessageForDto);

            _dbContext.UndeliveredMessages.Add(undeliveredMassage);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SendPendingMessages(string userId)
        {
            var messages = await _dbContext.UndeliveredMessages
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (messages.Any())
            {
                var messageDtos = _mapper.Map<List<UndeliveredMessageForDto>>(messages);
                var messagesToRemove = new List<UndeliveredMessage>();

                foreach (var messageDto in messageDtos)
                {
                    var sendingMessageStatus = messageDto.SendingMessageStatus;
                    var reviewId = messageDto.ReviewId;
                    var messageContent = messageDto.Message;

                    if (userConnections.TryGetValue(userId, out var connectionId))
                    {
                        await _context.Clients.Client(connectionId).SendAsync("ReceiveMessage", sendingMessageStatus, reviewId, messageContent);

                        var messageToRemove = messages.FirstOrDefault(m => _mapper.Map<UndeliveredMessageForDto>(m).ReviewId == reviewId);
                        if (messageToRemove != null)
                        {
                            messagesToRemove.Add(messageToRemove);
                        }
                    }
                }

                if (messagesToRemove.Any())
                {
                    _dbContext.UndeliveredMessages.RemoveRange(messagesToRemove);
                    await _dbContext.SaveChangesAsync();
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

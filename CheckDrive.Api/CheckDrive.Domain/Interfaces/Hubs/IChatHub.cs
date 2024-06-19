using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Interfaces.Hubs
{
    public interface IChatHub
    {
        Task SendPrivateRequest(SendingMessageStatus sendingMessageStatus, int id, string userId, string message);

        Task ReceivePrivateResponse(int statusReview, int reviewId, bool response);
    }
}

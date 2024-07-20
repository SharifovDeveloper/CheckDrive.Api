using CheckDrive.ApiContracts;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Interfaces.Hubs
{
    public interface IChatHub
    {
        Task SendPrivateRequest(UndeliveredMessageForDto undeliveredMessageForDto);

        Task ReceivePrivateResponse(int statusReview, int reviewId, bool response);
    }
}

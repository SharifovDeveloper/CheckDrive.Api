namespace CheckDrive.Domain.Interfaces.Hubs
{
    public interface IChatHub
    {
        Task SendPrivateRequest(int id, string userId, string message);

        Task ReceivePrivateResponse(bool response);
    }
}

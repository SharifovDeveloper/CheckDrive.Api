namespace CheckDrive.Domain.Interfaces.Hubs
{
    public interface IChatHub
    {
        Task SendPrivateRequest(string userId, string message);

        Task ReceivePrivateResponse(bool response);
    }
}

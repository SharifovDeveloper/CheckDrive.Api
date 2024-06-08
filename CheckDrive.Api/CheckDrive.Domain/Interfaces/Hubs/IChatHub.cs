namespace CheckDrive.Domain.Interfaces.Hubs
{
    public interface IChatHub
    {
        Task SendPrivateRequest(string userId, string message);

        Task SendPrivateResponse(string employeeId, bool isApproved);
    }
}

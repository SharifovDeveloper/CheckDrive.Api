namespace CheckDrive.Domain.Interfaces.Hubs
{
    public interface IChatHub
    {
        Task SendPrivateRequest(string userId, string massege);

        Task SendPrivateResponse(string employeeId, bool isApproved);
    }
}

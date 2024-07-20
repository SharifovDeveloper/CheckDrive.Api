namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IAuthorizationService
    {
        Task<string> Login(string email, string password);
    }
}

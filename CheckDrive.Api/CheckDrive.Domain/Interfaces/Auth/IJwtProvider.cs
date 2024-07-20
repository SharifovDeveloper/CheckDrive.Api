using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(Account account);
    }
}

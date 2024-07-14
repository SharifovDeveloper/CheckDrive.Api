using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Auth;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace CheckDrive.Services
{
    public class AuthorizationService(CheckDriveDbContext context, IMapper mapper, IJwtProvider jwtProvider) : IAuthorizationService
    {
        private readonly CheckDriveDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<string> Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = await GetByEmailAsync(login, password);
            if (user == null)
            {
                return null;
            }

            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

        private async Task<Account> GetByEmailAsync(string login, string password)
        {
            var userEntity = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

            if (userEntity == null)
            {
                return null;
            }

            return _mapper.Map<Account>(userEntity);
        }
    }
}

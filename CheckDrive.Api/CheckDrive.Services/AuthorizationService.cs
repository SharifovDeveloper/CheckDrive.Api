using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Auth;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace CheckDrive.Services
{
    public class AuthorizationService(CheckDriveDbContext context, IMapper mapper, IPasswordHasher hasher, IJwtProvider jwtProvider) : IAuthorizationService
    {
        private readonly CheckDriveDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher _hasher = hasher;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<string> Login(string email, string password)
        {
            var user = await GetByEmailAsync(email);

            var result = _hasher.Verify(password, user.Password);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var userEntity = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == email);

            if (userEntity == null)
            {
                return null;
            }

            return _mapper.Map<Account>(userEntity);
        }
    }
}

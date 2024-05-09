using CheckDrive.Domain.DTOs.Role;

namespace CheckDrive.Domain.DTOs.Account
{
    public class AccountForUpdateDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Bithdate { get; set; }

        public RoleDto RoleDto { get; set; }
    }
}

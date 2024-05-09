using CheckDrive.Domain.DTOs.Account;

namespace CheckDrive.Domain.DTOs.Role
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccountDto> Accounts { get; set; }
    }
}

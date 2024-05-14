using CheckDrive.Domain.DTOs.Dispatcher;
using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.Mechanic;
using CheckDrive.Domain.DTOs.Operator;
using CheckDrive.Domain.DTOs.Role;

namespace CheckDrive.Domain.DTOs.Account
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Bithdate { get; set; }
        public int RoleId { get; set; }
    }
}

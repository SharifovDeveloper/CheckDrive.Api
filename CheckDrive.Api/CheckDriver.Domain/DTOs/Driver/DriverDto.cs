using CheckDrive.Domain.DTOs.Account;

namespace CheckDrive.Domain.DTOs.Driver
{
    public class DriverDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public AccountDto Account { get; set; }
    }
}

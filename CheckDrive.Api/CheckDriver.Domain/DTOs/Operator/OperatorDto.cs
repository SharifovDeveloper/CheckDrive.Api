using CheckDrive.Domain.DTOs.Account;

namespace CheckDrive.Domain.DTOs.Operator
{
    public class OperatorDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public AccountDto AccountDto { get; set; }
    }
}

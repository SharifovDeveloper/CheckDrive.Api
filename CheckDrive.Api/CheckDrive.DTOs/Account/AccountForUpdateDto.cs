using System;

namespace CheckDrive.DTOs.Account
{
    public class AccountForUpdateDto
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

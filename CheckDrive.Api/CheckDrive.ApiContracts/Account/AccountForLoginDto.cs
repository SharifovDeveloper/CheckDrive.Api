using System.ComponentModel.DataAnnotations;

namespace CheckDrive.ApiContracts.Account
{
    public class AccountForLoginDto
    {
        [Required(ErrorMessage = "Loginni kiritish majburiy")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Login uzunligi 5 dan 30 gacha belgidan iborat bo'lishi kerak")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Parol kiritish majburiy")]
        [StringLength(24, MinimumLength = 4, ErrorMessage = "Parol uzunligi 4 dan 24 gacha belgidan iborat bo'lishi kerak")]
        public string Password { get; set; }
    }
}

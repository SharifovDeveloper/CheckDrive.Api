using System;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.ApiContracts.Account
{
    public class AccountForCreateDto
    {
        [Required(ErrorMessage = "Emailni kiritish majburiy")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Login uzunligi 5 dan 100 gacha belgidan iborat bo'lishi kerak")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Parol kiritish majburiy")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Parol uzunligi 4 dan 8 gacha belgidan iborat bo'lishi kerak")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Telefon raqamni kiritish majburiy")]
        [RegularExpression(@"^\+998\d{9}$", ErrorMessage = "Telefon raqami +998XXXXXXXXX formatida bo'lishi kerak")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Ismni kiritish majburiy")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Familiyani kiritish majburiy")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Tug'ilgan kunni kiritish majburiy")]
        [DataType(DataType.Date, ErrorMessage = "Tug'ilgan kun noto'g'ri formatda")]
        public DateTime Bithdate { get; set; }

        [Required(ErrorMessage = "Rolini kiritish majburiy")]
        public int RoleId { get; set; }
    }
}

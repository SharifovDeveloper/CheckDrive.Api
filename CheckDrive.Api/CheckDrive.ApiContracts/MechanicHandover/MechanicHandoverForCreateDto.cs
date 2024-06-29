using System;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.ApiContracts.MechanicHandover
{
    public class MechanicHandoverForCreateDto
    {
        public bool IsHanded { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Boshlang'ich masofani kiritish majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "Boshlang'ich masofa manfiy bo'lishi mumkin emas")]
        public double Distance { get; set; }
        public int MechanicId { get; set; }
        public int CarId { get; set; }
        public int DriverId { get; set; }
    }
}

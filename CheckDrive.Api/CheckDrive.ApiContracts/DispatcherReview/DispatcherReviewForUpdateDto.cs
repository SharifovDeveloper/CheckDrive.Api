using System;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.ApiContracts.DispatcherReview
{
    public class DispatcherReviewForUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yoqilg'i sarfini kiritish majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "Yoqilg'i sarfi manfiy bo'lishi mumkin emas")]
        public double FuelSpended { get; set; }

        [Required(ErrorMessage = "Oraliq masofani kiritish majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "Oraliq masofa manfiy bo'lishi mumkin emas")]
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }
        public int DispatcherId { get; set; }
        public int OperatorId { get; set; }
        public int MechanicId { get; set; }
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public int MechanicAcceptanceId { get; set; }
        public int MechanicHandoverId { get; set; }
        public int OperatorReviewId { get; set; }
    }
}

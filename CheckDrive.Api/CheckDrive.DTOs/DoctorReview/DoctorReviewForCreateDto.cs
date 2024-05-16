using System;

namespace CheckDrive.DTOs.DoctorReview
{
    public class DoctorReviewForCreateDto
    {
        public bool IsHealthy { get; set; }
        public string Comments { get; set; } = "";
        public DateTime Date { get; set; }
        public int DriverId { get; set; }
        public int DoctorId { get; set; }
    }
}

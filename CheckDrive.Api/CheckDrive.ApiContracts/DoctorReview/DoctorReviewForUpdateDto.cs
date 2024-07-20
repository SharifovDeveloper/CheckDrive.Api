using System;

namespace CheckDrive.ApiContracts.DoctorReview
{
    public class DoctorReviewForUpdateDto
    {
        public int Id { get; set; }
        public bool IsHealthy { get; set; }
        public string Comments { get; set; } = "";
        public DateTime Date { get; set; }
        public int DriverId { get; set; }
        public int DoctorId { get; set; }
    }
}

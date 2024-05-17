using System;

namespace CheckDrive.ApiContracts.DoctorReview
{
    public class DoctorReviewDto
    {
        public int Id { get; set; }
        public bool IsHealthy { get; set; }
        public string Comments { get; set; } = "";
        public DateTime Date { get; set; }
    }
}

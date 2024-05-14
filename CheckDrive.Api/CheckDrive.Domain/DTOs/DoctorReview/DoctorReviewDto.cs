using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.DTOs.Driver;

namespace CheckDrive.Domain.DTOs.DoctorReview
{
    public class DoctorReviewDto
    {
        public int Id { get; set; }
        public bool IsHealthy { get; set; }
        public string? Comments { get; set; }
        public DateTime Date { get; set; }
    }
}

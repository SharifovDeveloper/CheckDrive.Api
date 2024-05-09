using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.DTOs.Driver;

namespace CheckDrive.Domain.DTOs.DoctorReview
{
    public class DoctorReviewForUpdateDto
    {
        public bool IsHealthy { get; set; }
        public string? Comments { get; set; }
        public DateTime Date { get; set; }
        public DriverDto DriverDto { get; set; }
        public DoctorDto DoctorDto { get; set; }
    }
}

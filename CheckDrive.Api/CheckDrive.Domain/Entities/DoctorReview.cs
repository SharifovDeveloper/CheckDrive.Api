using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class DoctorReview : EntityBase
    {
        public bool IsHealthy { get; set; }
        public string? Comments { get; set; }
        public DateTime Date { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}

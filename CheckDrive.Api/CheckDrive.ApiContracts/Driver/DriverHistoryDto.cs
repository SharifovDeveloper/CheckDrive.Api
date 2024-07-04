using System;

namespace CheckDrive.ApiContracts.Driver
{
    public class DriverHistoryDto
    {
        public DateTime Date { get; set; }
        public int? DoctorReviewId { get; set; }
        public bool IsHealthy { get; set; }

        public int? MechanicHandoverId { get; set; }
        public bool IsHanded { get; set; }

        public int? OperatorReviewId { get; set; }
        public bool IsGiven { get; set; }

        public int? MechanicAcceptanceId { get; set; }
        public bool IsAccepted { get; set; }
    }
}

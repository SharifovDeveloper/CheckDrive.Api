using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.Operator;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.DTOs.OperatorReview
{
    public class OperatorReviewForUpdateDto
    {
        public double OilAmount { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public int OperatorId { get; set; }
        public int DriverId { get; set; }
    }
}
